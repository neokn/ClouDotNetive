using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace GraphQLApplication.HealthCheck;

internal static class EndpointRouteBuilderExtensions
{
    public static IEnumerable<IEndpointConventionBuilder> MapKubernetesHealthCheckProbs(this IEndpointRouteBuilder app,
        string prefix = "/healthz")
    {
        yield return app.MapHealthChecks($"{prefix}/startup", new HealthCheckOptions
        {
            Predicate = hcr => hcr.Tags.Contains("startup")
        });
        yield return app.MapHealthChecks($"{prefix}/ready", new HealthCheckOptions
        {
            Predicate = hcr => hcr.Tags.Contains("ready")
        });
        yield return app.MapHealthChecks($"{prefix}/live", new HealthCheckOptions
        {
            Predicate = hcr => hcr.Name == "live"
        });
    }

    public static void RequireHost(this IEnumerable<IEndpointConventionBuilder> builders, string host)
    {
        foreach (var endpointConventionBuilder in builders) endpointConventionBuilder.RequireHost(host);
    }
}