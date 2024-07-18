using Homework.Data;
using Microsoft.EntityFrameworkCore;

namespace Homework.Extensions;

internal static class DataAccessExtension
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}