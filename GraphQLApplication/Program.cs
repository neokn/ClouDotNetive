using GraphQLApplication.HealthCheck;
using GraphQLApplication.Types;
using Greet;
using HotChocolate.AspNetCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Person = GraphQLApplication.Models.Person;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSingleton(sp =>
    {
        const string connectionString = "mongodb://root:example@mongo.k3s.svc.cluster.local:32701";
        var mongoConnectionUrl = new MongoUrl(connectionString);
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
        mongoClientSettings.ClusterConfigurator = cb =>
        {
            // This will print the executed command to the console
            cb.Subscribe<CommandStartedEvent>(e => { Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}"); });
        };
        var client = new MongoClient(mongoClientSettings);
        var database = client.GetDatabase("test");
        return database.GetCollection<Person>("person");
    });
builder.Services.AddGrpcClient<Greeter.GreeterClient>(o => { o.Address = new Uri("http://localhost:5001"); });
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddGlobalObjectIdentification()
    // Registers the filter convention of MongoDB
    .AddMongoDbFiltering()
    // Registers the sorting convention of MongoDB
    .AddMongoDbSorting()
    // Registers the projection convention of MongoDB
    .AddMongoDbProjections()
    // Registers the paging providers of MongoDB
    .AddMongoDbPagingProviders();
;
builder.Services.AddHealthCheck<StartupService>();

var app = builder.Build();

app.UseRouting().UseEndpoints(e =>
{
    e.MapKubernetesHealthCheckProbs().RequireHost(Environment.GetEnvironmentVariable("POD_IP") ?? "localhost");
    e.MapGraphQL().WithOptions(new GraphQLServerOptions
    {
        Tool =
        {
            Enable = app.Environment.IsDevelopment()
        }
    });
});

app.Run();