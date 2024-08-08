using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using azure_service_bus_api.Interfaces;
using azure_service_bus_api.Models;
using azure_service_bus_api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("AzureServiceBus"));


builder.Services.AddSingleton(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
    return new ServiceBusClient(settings.ConnectionString);
});

builder.Services.AddSingleton(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
    return new ServiceBusAdministrationClient(settings.ConnectionString);
});

builder.Services.AddScoped<ITTMQueService, QueueService>();
builder.Services.AddScoped<ITopicServiceBusService, TopicServiceBusService>();


builder.Services.AddHostedService<ServiceBusBackgroundService>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
