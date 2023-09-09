using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GraphQLApplication.HealthCheck;

public class StartupService : BackgroundService, IHealthCheck
{
    private volatile bool _isReady;

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_isReady
            ? HealthCheckResult.Healthy("The startup task has completed.")
            : HealthCheckResult.Unhealthy("That startup task is still running."));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Do something on startup: e.g. load configuration data, warmup cache, database connection, etc.

        _isReady = true;
        return Task.CompletedTask;
    }
}