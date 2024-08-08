using Azure.Messaging.ServiceBus.Administration;
using Twilio.Rest.Events.V1;

namespace azure_service_bus_api.Services
{
    public class TopicServiceBusService : ITopicServiceBusService
    {
        private readonly ServiceBusAdministrationClient _adminClient;

        public TopicServiceBusService(ServiceBusAdministrationClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<string> CreateTopicAsync(string topicName)
        {
            if (!await _adminClient.TopicExistsAsync(topicName))
            {
                await _adminClient.CreateTopicAsync(topicName);
                return $"Topic '{topicName}' created successfully.";
            }
            return $"Topic '{topicName}' already exists.";
        }

        public async Task<string> UpdateTopicAsync(string topicName, string metadata)
        {
            if (await _adminClient.TopicExistsAsync(topicName))
            {
                if (!await _adminClient.TopicExistsAsync(metadata))
                {
                    TopicProperties topic = await _adminClient.GetTopicAsync(topicName);
                    topic.UserMetadata = metadata;

                    await _adminClient.UpdateTopicAsync(topic);

                    return $"Topic metadata updated '{metadata}'.";
                }
                return $"Topic metadata updated already exists.";
            }
            return $"Topic metadata updated does not exist.";
        }

        public async Task<string> DeleteTopicAsync(string topicName)
        {
            if (await _adminClient.TopicExistsAsync(topicName))
            {
                await _adminClient.DeleteTopicAsync(topicName);
                return $"Topic '{topicName}' deleted successfully.";
            }
            return $"Topic '{topicName}' does not exist.";
        }


        public async Task CreateSubscriptionAsync(string topicName, string subscriptionName)
        {
            await _adminClient.CreateSubscriptionAsync(topicName, subscriptionName);
        }

        public async Task UpdateSubscriptionAsync(string topicName, string subscriptionName, string newMetaData)
        {
            SubscriptionProperties subscription = await _adminClient.GetSubscriptionAsync(topicName, subscriptionName);
            subscription.UserMetadata = newMetaData;
            await _adminClient.UpdateSubscriptionAsync(subscription);
        }

        public async Task DeleteSubscriptionAsync(string topicName, string subscriptionName)
        {
            await _adminClient.DeleteSubscriptionAsync(topicName, subscriptionName);
        }
    }
}
