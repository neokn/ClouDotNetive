using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GraphQLApplication.HealthCheck;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHealthCheck<TStartup>(this IServiceCollection services)
        where TStartup : class, IHealthCheck, IHostedService
    {
        services.AddSingleton<TStartup>();
        services.AddHostedService(sp => sp.GetRequiredService<TStartup>());
        services.AddHealthChecks()
            .AddCheck<TStartup>(nameof(TStartup), tags: new[] { "startup", "ready" })
            .AddCheck("live", () => HealthCheckResult.Healthy());
        return services;
    }
}