using GreenMileSharing.Shared.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;
using Ocelot.Provider.Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddIniFile("env.ini", false, true)
    .AddJsonFile("ocelot.json", false, true)
    .AddJwtBearer(builder);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddPolly()
    .AddKubernetes();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();