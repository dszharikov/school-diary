using User.Data.Repositories.ParentRepositories;
using User.Data.Repositories.SchoolRepositories;
using User.Data.Repositories.UserRepositories;

namespace User.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISchoolRepository, DapperSchoolRepository>();
        services.AddScoped<IUserRepository, DapperUserRepository>();
        services.AddScoped<IParentRepository, DapperParentRepository>();

        return services;
    }
}