using InnovationLab.Auth.Models;

namespace InnovationLab.Auth.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    (string Token, DateTime ExpiresAt) GenerateRefreshToken(User user);
    Task SaveRefreshTokenAsync(User user, string refreshToken, DateTime expiredAt);
    Task<(bool Success, string? UserId)> TryGetUserIdByRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(User user, string refreshToken);
    Task RevokeAllRefreshTokensAsync(User user);
}