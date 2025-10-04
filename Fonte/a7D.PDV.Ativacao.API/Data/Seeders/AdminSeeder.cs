using a7D.PDV.Ativacao.API.Model;
using Microsoft.AspNetCore.Identity;

namespace Cariatides.Data.Seeders;


public class AdminSeeder
{
    public static async Task CreateAdminSeeder(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


        List<string> emails =
        [
            "jncosta201@gmail.com",
        ];


        foreach (var email in emails)
        {
            AppUser? user =  await userManager.FindByEmailAsync(email);
            
            if (user == null) return;
            
            await userManager.AddToRoleAsync(user, "admin");
        }
    }
}
