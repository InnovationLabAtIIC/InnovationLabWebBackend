using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using InnovationLab.Auth.Dtos;
using InnovationLab.Auth.Models;
using InnovationLab.Shared.Interfaces;
using InnovationLab.Auth.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Auth.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class UsersController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ITokenService tokenService,
    IEmailService emailService
) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IEmailService _emailService = emailService;

    [AllowAnonymous]
    [HttpPost("register", Name = nameof(Register))]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(userRegisterDto.Email);
        if (existingUser != null)
        {
            return BadRequest("A user with this email already exists.");
        }

        var user = userRegisterDto.Adapt<User>();

        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("User registered successfully.");
    }

    [AllowAnonymous]
    [HttpPost("login", Name = nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

        if (user is null || !user.IsActive)
        {
            return Unauthorized("Invalid credentials or inactive user.");
        }

        var result = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (!result)
        {
            return Unauthorized("Invalid credentials.");
        }

        var jwtToken = _tokenService.GenerateToken(user);
        var (refreshToken, refreshTokenExpiresAt) = _tokenService.GenerateRefreshToken(user);
        await _tokenService.SaveRefreshTokenAsync(user, refreshToken, refreshTokenExpiresAt);

        var response = new TokenResponseDto(jwtToken, refreshToken, refreshTokenExpiresAt);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("profile", Name = nameof(GetProfile))]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            return Unauthorized("Invalid token or inactive user.");
        }

        var userDto = user.Adapt<UserReadDto>();
        return Ok(userDto);
    }

    [Authorize]
    [HttpPut("profile", Name = nameof(UpdateProfile))]
    public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto userUpdateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || !user.IsActive)
        {
            return Unauthorized("Invalid token or inactive user.");
        }

        userUpdateDto.Adapt(user);
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var userDto = user.Adapt<UserReadDto>();
        return Ok(userDto);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token", Name = nameof(RefreshToken))]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequestDto)
    {
        var refreshToken = tokenRequestDto.RefreshToken;
        var (success, userId) = await _tokenService.TryGetUserIdByRefreshTokenAsync(refreshToken);

        if (!success || string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid or expired refresh token.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !user.IsActive)
        {
            return Unauthorized("Invalid or expired refresh token.");
        }

        await _tokenService.RevokeRefreshTokenAsync(user, refreshToken);
        var (newRefreshToken, newRefreshTokenExpiry) = _tokenService.GenerateRefreshToken(user);
        await _tokenService.SaveRefreshTokenAsync(user, newRefreshToken, newRefreshTokenExpiry);

        await _userManager.UpdateSecurityStampAsync(user);
        var jwtToken = _tokenService.GenerateToken(user);

        var response = new TokenResponseDto(jwtToken, refreshToken, newRefreshTokenExpiry);

        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout", Name = nameof(Logout))]
    public async Task<IActionResult> Logout([FromBody] TokenRequestDto tokenRequestDto)
    {
        var refreshToken = tokenRequestDto.RefreshToken;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var (success, refreshTokenUserId) = await _tokenService.TryGetUserIdByRefreshTokenAsync(refreshToken);

        if (!success || refreshTokenUserId != userId)
        {
            return Unauthorized("Invalid token or inactive user.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            return Unauthorized("Invalid token or inactive user.");
        }

        await _tokenService.RevokeRefreshTokenAsync(user, refreshToken);
        await _userManager.UpdateSecurityStampAsync(user);
        await _signInManager.SignOutAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPost("deactivate", Name = nameof(Deactivate))]
    public async Task<IActionResult> Deactivate()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            return Unauthorized("Invalid token or inactive user.");
        }

        user.IsActive = false;
        await _userManager.UpdateAsync(user);

        await _tokenService.RevokeAllRefreshTokensAsync(user);
        await _userManager.UpdateSecurityStampAsync(user);
        await _signInManager.SignOutAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPost("reactivate", Name = nameof(Reactivate))]
    public async Task<IActionResult> Reactivate(EmailRequestDto emailRequestDto)
    {
        var email = emailRequestDto.Email;
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null || user.IsActive)
        {
            return Ok("If your account exists and is deactivated, you will receive a reactivation email.");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var reactivationLink = $"https://innovation.iic.edu.np/reactivate?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendEmailAsync(user.Email ?? "", "Reactivate your account",
            $"Click <a href='{reactivationLink}'>here</a> to reactivate your account.");

        return Ok("If your account exists and is deactivated, you will receive a reactivation email.");
    }

    [AllowAnonymous]
    [HttpPost("forgot-password", Name = nameof(ForgotPassword))]
    public async Task<IActionResult> ForgotPassword([FromBody] EmailRequestDto emailRequestDto)
    {
        var email = emailRequestDto.Email;
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null || !user.IsActive)
        {
            return Ok("If your account exists and is active, you will receive a password reset email.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"https://your-frontend.com/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendEmailAsync(user.Email ?? "", "Password Reset Link", resetLink);

        return Ok("If your account exists and is active, you will receive a password reset email.");
    }

    [Authorize]
    [HttpPost("change-password", Name = nameof(ChangePassword))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            return Unauthorized("Invalid token or inactive user.");
        }

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _tokenService.RevokeAllRefreshTokensAsync(user);
        await _userManager.UpdateSecurityStampAsync(user);

        return Ok("Password changed successfully.");
    }

    [Authorize]
    [HttpGet(Name = nameof(GetUsers))]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var users = await _userManager.Users
            .Skip(skip)
            .Take(pageSize)
            .Where(u => u.IsActive)
            .ToListAsync();

        var userDtos = users.Adapt<IList<UserReadDto>>();
        return Ok(userDtos);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

        if (user is null)
        {
            return NotFound("User not found.");
        }

        var userDto = user.Adapt<UserReadDto>();
        return Ok(userDto);
    }
}
