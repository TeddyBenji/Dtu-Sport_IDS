using Microsoft.AspNetCore.Identity;


public static class RolesDataInitializer
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("User"))
        {
            var userRole = new IdentityRole("User");
            await roleManager.CreateAsync(userRole);
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var adminRole = new IdentityRole("Admin");
            await roleManager.CreateAsync(adminRole);
        }
    }
}
