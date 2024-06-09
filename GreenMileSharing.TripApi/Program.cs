using FluentValidation;
using FluentValidation.AspNetCore;
using GreenMileSharing.Messages;
using GreenMileSharing.Shared.Extensions;
using GreenMileSharing.Shared.Settings;
using GreenMileSharing.TripApi.Application.Consumers;
using GreenMileSharing.TripApi.Application.Repositories.Abstractions;
using GreenMileSharing.TripApi.Application.Validators;
using GreenMileSharing.TripApi.Persistence;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddIniFile("../env.ini");
builder.Host.UseSerilog((_, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

builder.Configuration.AddJwtBearer(builder);

builder.Services.AddControllers();
builder.Services.AddNpgsql<TripDbContext>(builder.Configuration["TripsDb:ConnectionString"]);

builder.Services.AddApplicationService<ITripRepository>();
builder.Services.AddApplicationService<IEmployeeRepository>();

builder.Services.AddApplicationService<ICarRepository>();
builder.Services.AddOptionsWithValidateOnStart<RabbitMqSettings>()
    .Bind(builder.Configuration.GetRequiredSection(RabbitMqSettings.SectionName));

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidatorsMarker>(includeInternalTypes: true);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<RegisterEmployeeConsumer>();
    
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
        
        EndpointConvention.Map<DeleteEmployee>(new Uri("queue:delete-employee"));
        EndpointConvention.Map<UpdateUsername>(new Uri("queue:update-username"));
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();