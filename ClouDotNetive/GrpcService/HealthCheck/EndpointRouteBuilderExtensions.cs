using Grpc.AspNetCore.HealthChecks;

namespace GrpcService.HealthCheck;

internal static class EndpointRouteBuilderExtensions
{
    public static void MapKubernetesHealthCheckProbs(this GrpcHealthChecksOptions options)
    {
        options.Services.Map("greet.Greeter", context => context.Tags.Contains("startup") ||context.Tags.Contains("ready") );
        options.Services.Map(string.Empty, context => context.Tags.Contains("live"));
    }
    
    public static void RequireHost(this IEnumerable<IEndpointConventionBuilder> builders, string host)
    {
        foreach (var endpointConventionBuilder in builders)
        {
            endpointConventionBuilder.RequireHost(host);
        }
    }
}