using System.Text;
using InnovationLab.Auth.DbContexts;
using InnovationLab.Auth.Middlewares;
using InnovationLab.Auth.Models;
using InnovationLab.Shared.Constants;
using InnovationLab.Shared.Extensions;
using InnovationLab.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDefaultConfiguration(builder.Environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(ConfigurationKeys.PostgresConnection))
);

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowInnovationLabFrontend", policy =>
//     {
//         policy.WithOrigins("https://innovation.iic.edu.np")
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//     });
// });

// Configure JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    JwtOptions jwtOptions = new();
    builder.Configuration.GetSection(ConfigurationKeys.Jwt).Bind(jwtOptions);
    var secretKey = Encoding.UTF8.GetBytes(jwtOptions.Secret);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero, // No tolerance for expired tokens
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseCors("AllowInnovationLabFrontend");

app.UseAuthentication();
app.UseMiddleware<SecurityStampValidatorMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
