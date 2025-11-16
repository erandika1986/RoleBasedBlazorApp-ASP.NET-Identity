using Microsoft.AspNetCore.Identity;

namespace RoleBasedBlazorApp.Data;

public class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed Roles
        string[] roleNames = { "Admin", "Manager", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Admin User
        var adminEmail = "admin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "System Administrator"
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed Manager User
        var managerEmail = "manager@example.com";
        var managerUser = await userManager.FindByEmailAsync(managerEmail);
        if (managerUser == null)
        {
            managerUser = new ApplicationUser
            {
                UserName = "manager",
                Email = managerEmail,
                EmailConfirmed = true,
                FullName = "Department Manager"
            };
            var result = await userManager.CreateAsync(managerUser, "Manager@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(managerUser, "Manager");
            }
        }

        // Seed Regular User
        var userEmail = "user@example.com";
        var regularUser = await userManager.FindByEmailAsync(userEmail);
        if (regularUser == null)
        {
            regularUser = new ApplicationUser
            {
                UserName = "user",
                Email = userEmail,
                EmailConfirmed = true,
                FullName = "Regular User"
            };
            var result = await userManager.CreateAsync(regularUser, "User@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }
}
