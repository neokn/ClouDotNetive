using GrpcService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GrpcService.HealthCheck;

public class StartupService : BackgroundService, IHealthCheck
{
    private readonly IServiceScopeFactory _scopeFactory;
    private volatile bool _isReady;

    public StartupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_isReady
            ? HealthCheckResult.Healthy("The startup task has completed.")
            : HealthCheckResult.Unhealthy("That startup task is still running."));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Do something on startup: e.g. load configuration data, warmup cache, database connection, etc.
        using (var serviceScope = _scopeFactory.CreateScope())
        {
            var personContext = serviceScope.ServiceProvider.GetRequiredService<PersonContext>();
            await personContext.Database.MigrateAsync(stoppingToken);
        }

        _isReady = true;
    }
}