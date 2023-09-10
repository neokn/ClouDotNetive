using System.Security.Authentication;
using GrpcService.HealthCheck;
using GrpcService.Models;
using GrpcService.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    var http2 = options.Limits.Http2;
    http2.InitialConnectionWindowSize = 1024 * 1024; // Bytes
    http2.InitialStreamWindowSize = 768 * 1024; // Bytes
});
// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcHealthChecks<StartupService>(o => { o.MapKubernetesHealthCheckProbs(); });
builder.Services.AddDbContext<PersonContext>();
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
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcHealthChecksService().RequireHost(Environment.GetEnvironmentVariable("POD_IP") ?? "localhost");
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();