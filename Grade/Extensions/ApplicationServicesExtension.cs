using Grade.Services;

namespace Grade.Extensions;

internal static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ITermService, TermService>
        (
            client => client.BaseAddress = 
                new Uri(configuration.GetValue<string>("Api:Term") 
                    ?? throw new InvalidOperationException("link to term api is missing"))
        );

        return services;
    }
}