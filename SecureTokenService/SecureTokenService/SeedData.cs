using System.Security.Claims;
using IdentityModel;
using SecureTokenService.Data;
using SecureTokenService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace SecureTokenService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var kobe = userMgr.FindByNameAsync("kobe").Result;
        if (kobe == null)
        {
            kobe = new ApplicationUser
            {
                UserName = "kobe",
                Email = "kobe.brants@euri.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(kobe, "Kobe123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(kobe, new Claim[]
            {
                new(JwtClaimTypes.Name, "Kobe Brants"),
                new(JwtClaimTypes.GivenName, "Kobe"),
                new(JwtClaimTypes.FamilyName, "Brants"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("kobe created");
        }
        else
        {
            Log.Debug("kobe already exists");
        }
    }
}