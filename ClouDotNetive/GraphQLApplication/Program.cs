using GraphQLApplication.GraphQueries;
using GraphQLApplication.HealthCheck;
using HotChocolate.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>();
builder.Services.AddHealthCheck<StartupService>();

var app = builder.Build();

app.UseRouting().UseEndpoints(e =>
{
    e.MapKubernetesHealthCheckProbs().RequireHost(Environment.GetEnvironmentVariable("POD_IP") ?? "localhost");
    e.MapGraphQL().WithOptions(new GraphQLServerOptions
    {
        Tool = {
            Enable = app.Environment.IsDevelopment()
        }
    });;
});

app.Run();
