// using System.Security.Claims;
// using AutoMapper;
// using InnovationLab.Auth.Models;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Upakaar.Api.Dtos.Users;
// using Upakaar.Api.Interfaces;
// using Upakaar.Api.Models;

// namespace InnovationLab.Auth.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class UsersController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IEmailService emailService, IMapper mapper) : ControllerBase
// {
//     private readonly UserManager<User> _userManager = userManager;
//     private readonly SignInManager<User> _signInManager = signInManager;
//     private readonly ITokenService _tokenService = tokenService;
//     private readonly IEmailService _emailService = emailService;
//     private readonly IMapper _mapper = mapper;

//     [HttpPost("login", Name = nameof(Login))]
//     public async Task<ActionResult<TokenResponseDto>> Login(LoginRequestDto loginRequestDto)
//     {
//         var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

//         if (user is null || !user.IsActive)
//         {
//             return Unauthorized("Invalid credentials or inactive user.");
//         }

//         var result = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

//         if (!result)
//         {
//             return Unauthorized("Invalid credentials.");
//         }

//         var jwtToken = _tokenService.GenerateToken(user);
//         var (refreshToken, refreshTokenExpiry) = _tokenService.GenerateRefreshToken(user);
//         await _tokenService.SaveRefreshTokenAsync(user, refreshToken, refreshTokenExpiry);

//         var response = new TokenResponseDto
//         {
//             Token = jwtToken,
//             RefreshToken = refreshToken,
//             RefreshTokenExpiresAt = refreshTokenExpiry
//         };

//         return Ok(response);
//     }

//     [HttpPost("register", Name = nameof(Register))]
//     public async Task<ActionResult> Register(UserRegisterDto userRegisterDto)
//     {
//         var existingUser = await _userManager.FindByEmailAsync(userRegisterDto.Email);
//         if (existingUser != null)
//         {
//             return BadRequest("A user with this email already exists.");
//         }

//         var user = _mapper.Map<User>(userRegisterDto);

//         var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

//         if (!result.Succeeded)
//         {
//             return BadRequest(result.Errors);
//         }

//         return Ok("User registered successfully.");
//     }

//     [Authorize]
//     [HttpGet("profile", Name = nameof(GetProfile))]
//     public async Task<ActionResult<UserReadDto>> GetProfile()
//     {
//         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//         if (string.IsNullOrEmpty(userId))
//         {
//             return Unauthorized("User ID not found in token.");
//         }

//         var user = await _userManager.FindByIdAsync(userId);

//         if (user is null || !user.IsActive)
//         {
//             return Unauthorized("Invalid token or inactive user.");
//         }

//         var userDto = _mapper.Map<UserReadDto>(user);
//         return Ok(userDto);
//     }

//     [Authorize]
//     [HttpPut("profile", Name = nameof(UpdateProfile))]
//     public async Task<ActionResult<UserReadDto>> UpdateProfile(UserUpdateDto updateDto)
//     {
//         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//         if (string.IsNullOrEmpty(userId))
//         {
//             return Unauthorized("User ID not found in token.");
//         }

//         var user = await _userManager.FindByIdAsync(userId);

//         if (user is null || !user.IsActive)
//         {
//             return Unauthorized("Invalid token or inactive user.");
//         }

//         _mapper.Map(updateDto, user);

//         var result = await _userManager.UpdateAsync(user);

//         if (!result.Succeeded)
//         {
//             return BadRequest(result.Errors);
//         }

//         var userDto = _mapper.Map<UserReadDto>(user);
//         return Ok(userDto);
//     }

//     [AllowAnonymous]
//     [HttpPost("refresh-token", Name = nameof(RefreshToken))]
//     public async Task<ActionResult<TokenResponseDto>> RefreshToken(TokenRequestDto tokenRequestDto)
//     {
//         var refreshToken = tokenRequestDto.RefreshToken;
//         var (success, userId) = await _tokenService.TryGetUserIdByRefreshTokenAsync(refreshToken);

//         if (!success || string.IsNullOrEmpty(userId))
//         {
//             return Unauthorized("Invalid or expired refresh token.");
//         }

//         var user = await _userManager.FindByIdAsync(userId);
//         if (user == null || !user.IsActive)
//         {
//             return Unauthorized("Invalid or expired refresh token.");
//         }

//         await _tokenService.RevokeRefreshTokenAsync(user, refreshToken);
//         var (newRefreshToken, newRefreshTokenExpiry) = _tokenService.GenerateRefreshToken(user);
//         await _tokenService.SaveRefreshTokenAsync(user, newRefreshToken, newRefreshTokenExpiry);

//         await _userManager.UpdateSecurityStampAsync(user);
//         var jwtToken = _tokenService.GenerateToken(user);

//         var response = new TokenResponseDto
//         {
//             Token = jwtToken,
//             RefreshToken = newRefreshToken,
//             RefreshTokenExpiresAt = newRefreshTokenExpiry
//         };

//         return Ok(response);
//     }

//     [Authorize]
//     [HttpPost("logout", Name = nameof(Logout))]
//     public async Task<ActionResult> Logout(TokenRequestDto tokenRequestDto)
//     {
//         var refreshToken = tokenRequestDto.RefreshToken;
//         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//         if (string.IsNullOrEmpty(userId))
//         {
//             return Unauthorized("User ID not found in token.");
//         }

//         var (success, refreshTokenUserId) = await _tokenService.TryGetUserIdByRefreshTokenAsync(refreshToken);

//         if (!success || refreshTokenUserId != userId)
//         {
//             return Unauthorized("Invalid token or inactive user.");
//         }

//         var user = await _userManager.FindByIdAsync(userId);

//         if (user is null || !user.IsActive)
//         {
//             return Unauthorized("Invalid token or inactive user.");
//         }

//         await _tokenService.RevokeRefreshTokenAsync(user, refreshToken);
//         await _userManager.UpdateSecurityStampAsync(user);
//         await _signInManager.SignOutAsync();

//         return NoContent();
//     }

//     [Authorize]
//     [HttpPost("deactivate", Name = nameof(Deactivate))]
//     public async Task<ActionResult> Deactivate()
//     {
//         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//         if (string.IsNullOrEmpty(userId))
//         {
//             return Unauthorized("User ID not found in token.");
//         }

//         var user = await _userManager.FindByIdAsync(userId);

//         if (user is null || !user.IsActive)
//         {
//             return Unauthorized("Invalid token or inactive user.");
//         }

//         user.IsActive = false;
//         await _userManager.UpdateAsync(user);

//         await _tokenService.RevokeAllRefreshTokensAsync(user);
//         await _userManager.UpdateSecurityStampAsync(user);
//         await _signInManager.SignOutAsync();

//         return NoContent();
//     }

//     [AllowAnonymous]
//     [HttpPost("reactivate", Name = nameof(Reactivate))]
//     public async Task<ActionResult> Reactivate(EmailRequestDto emailRequestDto)
//     {
//         var email = emailRequestDto.Email;
//         var user = await _userManager.FindByEmailAsync(email);

//         if (user is null || user.IsActive)
//         {
//             return Ok("If your account exists and is deactivated, you will receive a reactivation email.");
//         }

//         var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//         var reactivationLink = $"https://your-frontend.com/reactivate?userId={user.Id}&token={Uri.EscapeDataString(token)}";

//         await _emailService.SendEmailAsync(user.Email ?? "", "Reactivate your account",
//             $"Click <a href='{reactivationLink}'>here</a> to reactivate your account.");

//         return Ok("If your account exists and is deactivated, you will receive a reactivation email.");
//     }

//     [AllowAnonymous]
//     [HttpPost("forgot-password", Name = nameof(ForgotPassword))]
//     public async Task<ActionResult> ForgotPassword(EmailRequestDto emailRequestDto)
//     {
//         var email = emailRequestDto.Email;
//         var user = await _userManager.FindByEmailAsync(email);

//         if (user is null || !user.IsActive)
//         {
//             return Ok("If your account exists and is active, you will receive a password reset email.");
//         }

//         var token = await _userManager.GeneratePasswordResetTokenAsync(user);
//         var resetLink = $"https://your-frontend.com/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

//         await _emailService.SendPasswordResetLinkAsync(user, user.Email ?? "", resetLink);

//         return Ok("If your account exists and is active, you will receive a password reset email.");
//     }

//     [Authorize]
//     [HttpPost("change-password", Name = nameof(ChangePassword))]
//     public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
//     {
//         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//         if (string.IsNullOrEmpty(userId))
//         {
//             return Unauthorized("User ID not found in token.");
//         }

//         var user = await _userManager.FindByIdAsync(userId);

//         if (user is null || !user.IsActive)
//         {
//             return Unauthorized("Invalid token or inactive user.");
//         }

//         var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

//         if (!result.Succeeded)
//         {
//             return BadRequest(result.Errors);
//         }

//         await _tokenService.RevokeAllRefreshTokensAsync(user);
//         await _userManager.UpdateSecurityStampAsync(user);

//         return Ok("Password changed successfully.");
//     }

//     [Authorize]
//     [HttpGet(Name = nameof(GetUsers))]
//     public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
//     {
//         var users = await _userManager.Users
//             .Where(u => u.IsActive)
//             .ToListAsync();

//         var userDtos = _mapper.Map<IEnumerable<UserReadDto>>(users);
//         return Ok(userDtos);
//     }

//     [AllowAnonymous]
//     [HttpGet("{slug}", Name = nameof(GetUserByIdOrUsername))]
//     public async Task<ActionResult<UserReadDto>> GetUserByIdOrUsername(string slug)
//     {
//         User? user = null;

//         // Try to find by Id (GUID)
//         if (Guid.TryParse(slug, out var userId))
//         {
//             user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId.ToString() && u.IsActive);
//         }

//         // If not found by Id, try by username
//         if (user == null)
//         {
//             user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == slug && u.IsActive);
//         }

//         if (user == null)
//         {
//             return NotFound("User not found.");
//         }

//         var userDto = _mapper.Map<UserReadDto>(user);
//         return Ok(userDto);
//     }
// }