using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Owniverse.Persistence;
using Owniverse.Infrastructure.Messaging;
using Owniverse.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddAssetStorage(builder.Configuration, builder.Environment.ContentRootPath);

var app = builder.Build();

app.MapControllers();
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("ready")
});

app.Run();
