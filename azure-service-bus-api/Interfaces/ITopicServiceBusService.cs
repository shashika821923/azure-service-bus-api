using System.Threading.Tasks;

public interface ITopicServiceBusService
{
    Task<string> CreateTopicAsync(string topicName);
    Task<string> UpdateTopicAsync(string topicName, string metaData);
    Task<string> DeleteTopicAsync(string topicName);
    Task CreateSubscriptionAsync(string topicName, string subscriptionName);
    Task UpdateSubscriptionAsync(string topicName, string subscriptionName, string newMetaData);
    Task DeleteSubscriptionAsync(string topicName, string subscriptionName);
}
