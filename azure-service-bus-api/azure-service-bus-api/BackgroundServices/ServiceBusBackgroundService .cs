using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using System.Threading;
using System.Threading.Tasks;

public class ServiceBusBackgroundService : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<ServiceBusBackgroundService> _logger;

    public ServiceBusBackgroundService(ServiceBusClient client, ILogger<ServiceBusBackgroundService> logger)
    {
        _client = client;
        _logger = logger;
        _processor = _client.CreateProcessor("it-worx-test-topic", "subscription-1");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        _logger.LogInformation($"Received message: {body}");


        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error processing message");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await _processor.CloseAsync(stoppingToken);
        await base.StopAsync(stoppingToken);
    }
}
