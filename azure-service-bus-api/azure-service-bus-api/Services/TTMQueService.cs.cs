using Azure.Messaging.ServiceBus.Administration;
using azure_service_bus_api.Interfaces;

namespace azure_service_bus_api.Services
{
    public class QueueService : ITTMQueService
    {
        private readonly ServiceBusAdministrationClient _adminClient;

        public QueueService(ServiceBusAdministrationClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<string> CreateQueueAsync(string queueName)
        {
            if (!await _adminClient.QueueExistsAsync(queueName))
            {
                await _adminClient.CreateQueueAsync(queueName);
                return $"Queue '{queueName}' created successfully.";
            }
            return $"Queue '{queueName}' already exists.";
        }

        public async Task<string> UpdateQueueAsync(string queueName, string newQueueName)
        {
            if (await _adminClient.QueueExistsAsync(queueName))
            {
                QueueProperties queue = await _adminClient.GetQueueAsync(queueName);
                queue.LockDuration = TimeSpan.FromSeconds(60);
                QueueProperties updatedQueue = await _adminClient.UpdateQueueAsync(queue);
                return $"Queue '{queueName}' locked duration to '{60}'.";
            }
            return $"Queue '{queueName}' does not exist.";
        }

        public async Task<string> DeleteQueueAsync(string queueName)
        {
            if (await _adminClient.QueueExistsAsync(queueName))
            {
                await _adminClient.DeleteQueueAsync(queueName);
                return $"Queue '{queueName}' deleted successfully.";
            }
            return $"Queue '{queueName}' does not exist.";
        }
    }
}
