using GraphQLApplication.GraphQueries;
using HotChocolate.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.UseRouting().UseEndpoints(e =>
{
    e.MapGraphQL().WithOptions(new GraphQLServerOptions
    {
        Tool = {
            Enable = app.Environment.IsDevelopment()
        }
    });;
});

app.Run();
