using a7D.PDV.Ativacao.API.Data;
using Microsoft.EntityFrameworkCore;

namespace a7D.PDV.Ativacao.API.Extensions;

public static class ServiceCollectionExtensions
{
    
    
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("Default")));
        return services;
    }
}
