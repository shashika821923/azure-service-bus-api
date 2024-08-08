namespace azure_service_bus_api.Models
{
    public class OrderMessage
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        // Add other properties as needed
    }
}
