using System.Security.Claims;
using InnovationLab.Auth.Constants;
using InnovationLab.Auth.Models;
using Microsoft.AspNetCore.Identity;

namespace InnovationLab.Auth.Middlewares;

public class SecurityStampValidatorMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
    {
        var userPrincipal = context.User;

        if (userPrincipal?.Identity?.IsAuthenticated == true)
        {
            var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var tokenStamp = userPrincipal.FindFirstValue(CustomClaims.SecurityStamp);

            if (userId != null && tokenStamp != null)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null && user.SecurityStamp != tokenStamp)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token is invalid or expired.");
                    return;
                }
            }
        }

        await next(context);
    }
}