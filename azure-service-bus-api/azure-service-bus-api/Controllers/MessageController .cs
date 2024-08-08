using Azure.Messaging.ServiceBus;
using azure_service_bus_api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/message")]
public class MessageController : ControllerBase
{
    private readonly ServiceBusService _serviceBusService;

    // Constructor to inject the ServiceBusService dependency
    public MessageController(ServiceBusService serviceBusService)
    {
        _serviceBusService = serviceBusService;
    }

    /// <summary>
    /// Sends a message to the specified queue.
    /// </summary>
    /// <param name="message">The message to send to the queue.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("queue/send")]
    public async Task<IActionResult> SendMessageToQueue([FromBody] OrderMessage message)
    {
        // Calls the service to send a message to the queue
        await _serviceBusService.SendMessageToQueueAsync(message);
        // Returns a success message
        return Ok("Message sent to queue");
    }
    // Example usage:
    // POST /api/message/queue/send
    // Request body: { "OrderId": 123, "Product": "Widget", "Quantity": 10 }
    // Sends the message to the default queue.

    /// <summary>
    /// Sends a message to the specified topic.
    /// </summary>
    /// <param name="message">The message to send to the topic.</param>
    /// <param name="topic">The name of the topic to send the message to.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("topic/send/{topic}")]
    public async Task<IActionResult> SendMessageToTopic([FromBody] OrderMessage message, string topic)
    {
        // Calls the service to send a message to the specified topic
        await _serviceBusService.SendMessageToTopicAsync(topic, message);
        // Returns a success message
        return Ok("Message sent to topic");
    }
    // Example usage:
    // POST /api/message/topic/send/Orders
    // Request body: { "OrderId": 123, "Product": "Widget", "Quantity": 10 }
    // Sends the message to the 'Orders' topic.

    /// <summary>
    /// Starts processing messages from the queue.
    /// </summary>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("queue/receive")]
    public async Task<IActionResult> ReceiveQueueMessages()
    {
        // Calls the service to start processing queue messages
        await _serviceBusService.ProcessQueueMessagesAsync(ProcessMessageHandler, ErrorHandler);
        // Returns a success message
        return Ok("Started queue message processing");
    }
    // Example usage:
    // POST /api/message/queue/receive
    // Starts processing messages from the default queue.

    /// <summary>
    /// Starts processing messages from the specified topic.
    /// </summary>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("topic/receive")]
    public async Task<IActionResult> ReceiveTopicMessages()
    {
        // Calls the service to start processing topic messages
        await _serviceBusService.ProcessTopicMessagesAsync(ProcessMessageHandler, ErrorHandler);
        // Returns a success message
        return Ok("Started topic message processing");
    }
    // Example usage:
    // POST /api/message/topic/receive
    // Starts processing messages from the default topic.

    /// <summary>
    /// Receives a specified number of messages from a queue.
    /// </summary>
    /// <param name="queueName">The name of the queue to receive messages from.</param>
    /// <param name="maxMessages">The maximum number of messages to receive.</param>
    /// <returns>An IActionResult containing the received messages.</returns>
    [HttpGet("receive-from-queue")]
    public async Task<IActionResult> ReceiveMessagesFromQueue(string queueName, int maxMessages)
    {
        // Calls the service to receive messages from the specified queue
        var messages = await _serviceBusService.ReceiveMessagesFromQueueAsync(queueName, maxMessages);
        // Returns the received messages
        return Ok(messages);
    }
    // Example usage:
    // GET /api/message/receive-from-queue?queueName=MyQueue&maxMessages=5
    // Receives up to 5 messages from the 'MyQueue' queue.

    // Handles processing of incoming messages
    private Task ProcessMessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        var message = JsonConvert.DeserializeObject<OrderMessage>(body);
        return Task.CompletedTask;
    }

    // Handles errors during message processing
    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        return Task.CompletedTask;
    }
}
