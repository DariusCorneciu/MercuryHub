using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" }
            );
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var adminRole = context.Roles.First(r => r.Name == "Admin");

            var admin = new ApplicationUser
            {
                userName = "admin",
                RoleId = adminRole.Id
            };

            admin.password = passwordHasher.HashPassword(admin, "admin1234");

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
