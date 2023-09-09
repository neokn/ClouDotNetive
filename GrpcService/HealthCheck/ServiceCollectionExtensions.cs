using Grpc.AspNetCore.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GrpcService.HealthCheck;

public static class ServiceCollectionExtensions
{
    public static IHealthChecksBuilder AddGrpcHealthChecks<TStartup>(this IServiceCollection services,
        Action<GrpcHealthChecksOptions>? configure = null)
        where TStartup : class, IHealthCheck, IHostedService
    {
        services.AddSingleton<TStartup>();
        services.AddHostedService(sp => sp.GetRequiredService<TStartup>());
        return services.AddGrpcHealthChecks(configure ?? new Action<GrpcHealthChecksOptions>(_ => { }))
            .AddCheck<TStartup>(typeof(TStartup).Name, tags: new[] { "startup", "ready" })
            .AddCheck("Live", () => HealthCheckResult.Healthy(), new[] { "live" });
    }
}