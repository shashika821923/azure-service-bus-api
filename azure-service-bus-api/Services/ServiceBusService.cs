using Azure.Messaging.ServiceBus;
using azure_service_bus_api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

public class ServiceBusService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusSettings _settings;

    public ServiceBusService(ServiceBusClient serviceBusClient, IOptions<ServiceBusSettings> options)
    {
        _serviceBusClient = serviceBusClient;
        _settings = options.Value;
    }

    public async Task SendMessageToQueueAsync<T>(T message)
    {
        var sender = _serviceBusClient.CreateSender(_settings.QueueName);
        var jsonMessage = JsonConvert.SerializeObject(message);
        var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));
        await sender.SendMessageAsync(serviceBusMessage);
    }

    public async Task SendMessageToTopicAsync<T>(string topicName, T message)
    {
        var sender = _serviceBusClient.CreateSender(topicName);
        var jsonMessage = JsonConvert.SerializeObject(message);
        var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));
        await sender.SendMessageAsync(serviceBusMessage);
    }

    public async Task ProcessQueueMessagesAsync(Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler)
    {
        var processor = _serviceBusClient.CreateProcessor(_settings.QueueName);
        processor.ProcessMessageAsync += messageHandler;
        processor.ProcessErrorAsync += errorHandler;
        await processor.StartProcessingAsync();
    }

    public async Task ProcessTopicMessagesAsync(Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler)
    {
        var processor = _serviceBusClient.CreateProcessor(_settings.OrderPlacedTopic, _settings.InventorySubscription);
        processor.ProcessMessageAsync += messageHandler;
        processor.ProcessErrorAsync += errorHandler;
        await processor.StartProcessingAsync();
    }
    public async Task<List<string>> ReceiveMessagesFromQueueAsync(string queueName, int maxMessages)
    {
        var messages = new List<string>();

        var receiver = _serviceBusClient.CreateReceiver(queueName);

        var receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages);

        foreach (var message in receivedMessages)
        {
            messages.Add(message.Body.ToString());

            await receiver.CompleteMessageAsync(message);
        }
        await receiver.CloseAsync();

        return messages;
    }
}
