namespace Cariatides.Data.Seeders;

public class SeederManager
{
    public static async Task Run(IServiceProvider serviceProvider)
    {
        await RoleInitializerSeeder.SeedRoles(serviceProvider);
        await AdminSeeder.CreateAdminSeeder(serviceProvider);
    }
}
