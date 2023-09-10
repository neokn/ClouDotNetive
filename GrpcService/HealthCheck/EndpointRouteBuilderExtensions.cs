using Grpc.AspNetCore.HealthChecks;

namespace GrpcService.HealthCheck;

internal static class EndpointRouteBuilderExtensions
{
    public static void MapKubernetesHealthCheckProbs(this GrpcHealthChecksOptions options)
    {
        options.Services.Map("greet.Greeter",
            context => context.Tags.Contains("startup") || context.Tags.Contains("ready"));
        options.Services.Map(string.Empty, context => context.Tags.Contains("live"));
    }
}