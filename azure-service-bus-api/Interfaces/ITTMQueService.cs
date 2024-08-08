namespace azure_service_bus_api.Interfaces
{
    public interface ITTMQueService
    {
        Task<string> CreateQueueAsync(string queueName);
        Task<string> UpdateQueueAsync(string queueName, string newQueueName);
        Task<string> DeleteQueueAsync(string queueName);
    }
}
