namespace azure_service_bus_api.Models
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public string OrderPlacedTopic { get; set; }
        public string InventorySubscription { get; set; }
    }

}
