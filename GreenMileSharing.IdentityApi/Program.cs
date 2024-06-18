using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using GreenMileSharing.IdentityApi.Application.Consumers;
using GreenMileSharing.IdentityApi.Application.Consumers.Json;
using GreenMileSharing.IdentityApi.Application.Extensions;
using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.IdentityApi.Application.Services;
using GreenMileSharing.IdentityApi.Application.Services.Abstractions;
using GreenMileSharing.IdentityApi.Application.Validation;
using GreenMileSharing.IdentityApi.Persistence;
using GreenMileSharing.Messages;
using GreenMileSharing.Messages.Json;
using GreenMileSharing.Shared.Extensions;
using GreenMileSharing.Shared.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddIniFile("env.ini");
builder.Host.UseSerilog((_, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

builder.Configuration.AddJwtBearer(builder);

builder.Services.AddApplicationService<IAuthService<ApplicationUser>>();
builder.Services.AddApplicationService<ITokenWriter<ApplicationUser>>();

builder.Services.AddKeyedScoped<IAuthService<ApplicationUser>, JsonAuthService>(nameof(JsonAuthService));

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["IdentityDb:ConnectionString"]);
builder.Services.AddIdentityConfiguration();

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<RabbitMqSettings>()
    .Bind(builder.Configuration.GetRequiredSection(RabbitMqSettings.SectionName));

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<LoginRequestValidator>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new ApiVersion(2.0);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    
    busConfigurator.AddConsumer<DeleteEmployeeConsumer>();
    busConfigurator.AddConsumer<UpdateUsernameConsumer>();
    busConfigurator.AddConsumer<UpdateUsernameJsonConsumer>();
    busConfigurator.AddConsumer<UpdateUsernameJsonConsumer>();
    
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
        configurator.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, hostConfigurator =>
        {
            hostConfigurator.Username(rabbitMqSettings.Username);
            hostConfigurator.Password(rabbitMqSettings.Password);
        });
        
        configurator.ConfigureEndpoints(context);
        
        configurator.UseNewtonsoftJsonSerializer();
        configurator.UseNewtonsoftJsonDeserializer();
        
        EndpointConvention.Map<RegisterEmployee>(new Uri("queue:register-employee"));
        EndpointConvention.Map<RegisterEmployeeJson>(new Uri("queue:register-employee-json"));
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();