using GreenMileSharing.IdentityApi.Application.Consumers;
using GreenMileSharing.IdentityApi.Application.Extensions;
using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.IdentityApi.Application.Services.Abstractions;
using GreenMileSharing.IdentityApi.Persistence;
using GreenMileSharing.Messages;
using GreenMileSharing.Shared.Extensions;
using GreenMileSharing.Shared.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddIniFile("../env.ini");
builder.Host.UseSerilog((_, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

builder.Configuration.AddJwtBearer(builder);

builder.Services.AddApplicationService<IAuthService<ApplicationUser>>();
builder.Services.AddApplicationService<ITokenWriter<ApplicationUser>>();

builder.Services.AddNpgsql<ApplicationDbContext>(builder.Configuration["IdentityDb:ConnectionString"]);
builder.Services.AddIdentityConfiguration();

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<RabbitMqSettings>()
    .Bind(builder.Configuration.GetRequiredSection(RabbitMqSettings.SectionName));


builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    
    busConfigurator.AddConsumer<DeleteEmployeeConsumer>();
    busConfigurator.AddConsumer<UpdateUsernameConsumer>();
    
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
        configurator.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, hostConfigurator =>
        {
            hostConfigurator.Username(rabbitMqSettings.Username);
            hostConfigurator.Password(rabbitMqSettings.Password);
        });

        configurator.UseNewtonsoftJsonSerializer();
        configurator.UseNewtonsoftJsonDeserializer();

        configurator.ConfigureEndpoints(context);
        EndpointConvention.Map<RegisterEmployee>(new Uri("queue:register-employee"));
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();