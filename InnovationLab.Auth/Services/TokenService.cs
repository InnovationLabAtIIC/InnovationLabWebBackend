using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InnovationLab.Auth.Constants;
using InnovationLab.Auth.DbContexts;
using InnovationLab.Auth.Interfaces;
using InnovationLab.Auth.Models;
using InnovationLab.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InnovationLab.Auth.Services;

public class TokenService(IOptions<JwtOptions> jwtOptions, AuthDbContext dbContext) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly AuthDbContext _dbContext = dbContext;

    public string GenerateToken(User user)
    {
        var secretKey = _jwtOptions.Secret;
        var issuer = _jwtOptions.Issuer;
        var audience = _jwtOptions.Audience;
        var expiryMinutes = _jwtOptions.ExpiryMinutes;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
        };

        if (!string.IsNullOrWhiteSpace(user.SecurityStamp))
        {
            claims.Add(new Claim(CustomClaims.SecurityStamp, user.SecurityStamp));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var secretKey = _jwtOptions.Secret;
        var issuer = _jwtOptions.Issuer;
        var audience = _jwtOptions.Audience;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var handler = new JwtSecurityTokenHandler();

        try
        {
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = key,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public (string Token, DateTime ExpiresAt) GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var token = Convert.ToBase64String(randomNumber);
        var expiresAt = DateTime.UtcNow.AddDays(3);

        return (token, expiresAt);
    }

    public async Task SaveRefreshTokenAsync(User user, string refreshToken, DateTime expiresAt)
    {
        var entity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            Revoked = false
        };
        _dbContext.RefreshTokens.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(bool Success, string? UserId)> TryGetUserIdByRefreshTokenAsync(string refreshToken)
    {
        var token = await _dbContext.RefreshTokens
            .Where(rt => rt.Token == refreshToken && !rt.Revoked && rt.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (token != null)
        {
            return (true, token.UserId.ToString());
        }
        return (false, null);
    }

    public async Task RevokeRefreshTokenAsync(User user, string refreshToken)
    {
        var token = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == user.Id && rt.Token == refreshToken && !rt.Revoked)
            .FirstOrDefaultAsync();

        if (token != null)
        {
            token.Revoked = true;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RevokeAllRefreshTokensAsync(User user)
    {
        var tokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == user.Id && !rt.Revoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.Revoked = true;
        }
        await _dbContext.SaveChangesAsync();
    }
}
