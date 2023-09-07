using GrpcService.HealthCheck;
using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcHealthChecks<StartupService>(o => { o.MapKubernetesHealthCheckProbs(); });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcHealthChecksService().RequireHost(Environment.GetEnvironmentVariable("POD_IP") ?? "localhost");
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();