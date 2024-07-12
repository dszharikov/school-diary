using User.Data.Repositories.SchoolRepositories;

namespace User.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISchoolRepository, DapperSchoolRepository>();

        return services;
    }
}