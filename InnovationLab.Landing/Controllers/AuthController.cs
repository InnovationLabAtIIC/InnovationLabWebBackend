using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using InnovationLabBackend.Api.Dtos.Users;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InnovationLabBackend.Api.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        IMapper mapper
    ) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        private async Task<string> GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("JWT Key, Issuer or Audience is not configured correctly");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Email, user.Email!),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.NameIdentifier, user.Id),
                new (new ClaimsIdentityOptions().SecurityStampClaimType, await _userManager.GetSecurityStampAsync(user))
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register", Name = "Register")]
        public async Task<ActionResult<UserRegisterResponseDto>> Register([FromBody] UserRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                await _userManager.SetTwoFactorEnabledAsync(user, true);

                var secret = await _userManager.GetAuthenticatorKeyAsync(user);
                const string issuer = "InnovationLab";
                var qrCodeUrl = $"otpauth://totp/{issuer}:{user.Email}?secret={secret}&issuer={issuer}&digits=6";

                var response = new UserRegisterResponseDto
                {
                    Message = "User registered successfully",
                    QrCodeUrl = qrCodeUrl
                };

                return Ok(response);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login", Name = "Login")]
        public async Task<ActionResult<UserLoginResponseDto>> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { Message = "Invalid credentials or user inactive" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (await _userManager.GetTwoFactorEnabledAsync(user))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }

                var response = new UserLoginResponseDto
                {
                    Message = "Two-factor authentication is required",
                    Requires2fa = true,
                    Email = user.Email
                };

                return Ok(response);
            }

            if (result.IsLockedOut)
            {
                return Unauthorized(new { Message = "User account locked out" });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }

        [HttpPost("verify", Name = "VerifyOtp")]
        public async Task<ActionResult<UserVerifyResponseDto>> VerifyOtp([FromBody] UserVerifyDto userVerifyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(userVerifyDto.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid user" });
            }

            var isValidOtp = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, userVerifyDto.Otp);

            if (!isValidOtp)
            {
                return Unauthorized(new { Message = "Invalid or expired otp" });
            }

            var response = new UserVerifyResponseDto
            {
                Message = "Login successful",
                Token = await GenerateJwtToken(user)
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile", Name = "GetProfile")]
        public async Task<ActionResult<UserResponseDto>> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserResponseDto>(user);
            return Ok(userDto);
        }

        [Authorize]
        [HttpPost("logout", Name = "Logout")]
        public async Task<ActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // Update the security stamp to invalidate existing tokens for this user
                await _userManager.UpdateSecurityStampAsync(user);
            }

            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
