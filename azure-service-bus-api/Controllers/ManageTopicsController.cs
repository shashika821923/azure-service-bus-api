using azure_service_bus_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace azure_service_bus_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageTopicsController : ControllerBase
    {
        private readonly ITopicServiceBusService _topicService;

        // Constructor to inject the ITopicServiceBusService dependency
        public ManageTopicsController(ITopicServiceBusService topicService)
        {
            _topicService = topicService;
        }

        /// <summary>
        /// Creates a new topic with the specified name.
        /// </summary>
        /// <param name="topicName">The name of the topic to create.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateTopicAsync(string topicName)
        {
            // Calls the service to create a new topic
            var result = await _topicService.CreateTopicAsync(topicName);
            // Returns an OK response with the result
            return Ok(result);
        }
        // Example usage:
        // POST /api/managetopics/create?topicName=NewTopic
        // This will create a new topic named 'NewTopic'.

        /// <summary>
        /// Updates metadata for an existing topic.
        /// </summary>
        /// <param name="topicName">The name of the topic to update.</param>
        /// <param name="metaData">The metadata to update for the topic.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("update")]
        public async Task<IActionResult> UpdateTopicAsync(string topicName, string metaData)
        {
            // Calls the service to update the topic with new metadata
            var result = await _topicService.UpdateTopicAsync(topicName, metaData);
            // Returns an OK response with the result
            return Ok(result);
        }
        // Example usage:
        // POST /api/managetopics/update?topicName=ExistingTopic&metaData=NewMetaData
        // This will update the topic 'ExistingTopic' with new metadata 'NewMetaData'.

        /// <summary>
        /// Deletes the topic with the specified name.
        /// </summary>
        /// <param name="topicName">The name of the topic to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTopicAsync(string topicName)
        {
            // Calls the service to delete the topic
            var result = await _topicService.DeleteTopicAsync(topicName);
            // Returns an OK response with the result
            return Ok(result);
        }
        // Example usage:
        // DELETE /api/managetopics/delete?topicName=TopicToDelete
        // This will delete the topic named 'TopicToDelete'.

        /// <summary>
        /// Creates a subscription for a specified topic.
        /// </summary>
        /// <param name="topicName">The name of the topic to create the subscription for.</param>
        /// <param name="subscriptionName">The name of the subscription to create.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("create-subscription")]
        public async Task<IActionResult> CreateSubscription(string topicName, string subscriptionName)
        {
            try
            {
                // Calls the service to create a new subscription for the specified topic
                await _topicService.CreateSubscriptionAsync(topicName, subscriptionName);
                // Returns a success message
                return Ok(new { Message = $"Subscription '{subscriptionName}' created for topic '{topicName}' successfully." });
            }
            catch (Exception ex)
            {
                // Returns an error message if an exception occurs
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        // Example usage:
        // POST /api/managetopics/create-subscription?topicName=TopicName&subscriptionName=NewSubscription
        // This will create a new subscription named 'NewSubscription' for the topic 'TopicName'.

        /// <summary>
        /// Updates the name of an existing subscription for a topic.
        /// </summary>
        /// <param name="topicName">The name of the topic.</param>
        /// <param name="subscriptionName">The current name of the subscription.</param>
        /// <param name="newSubscriptionName">The new name to assign to the subscription.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("update-subscription")]
        public async Task<IActionResult> UpdateSubscription(string topicName, string subscriptionName, string newSubscriptionName)
        {
            try
            {
                // Calls the service to update the subscription name
                await _topicService.UpdateSubscriptionAsync(topicName, subscriptionName, newSubscriptionName);
                // Returns a success message
                return Ok(new { Message = $"Subscription '{subscriptionName}' updated to '{newSubscriptionName}' for topic '{topicName}' successfully." });
            }
            catch (Exception ex)
            {
                // Returns an error message if an exception occurs
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        // Example usage:
        // PUT /api/managetopics/update-subscription?topicName=TopicName&subscriptionName=OldSubscription&newSubscriptionName=NewSubscription
        // This will update the subscription 'OldSubscription' to 'NewSubscription' for the topic 'TopicName'.

        /// <summary>
        /// Deletes a subscription from a specified topic.
        /// </summary>
        /// <param name="topicName">The name of the topic.</param>
        /// <param name="subscriptionName">The name of the subscription to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("delete-subscription")]
        public async Task<IActionResult> DeleteSubscription(string topicName, string subscriptionName)
        {
            try
            {
                // Calls the service to delete the subscription
                await _topicService.DeleteSubscriptionAsync(topicName, subscriptionName);
                // Returns a success message
                return Ok(new { Message = $"Subscription '{subscriptionName}' deleted for topic '{topicName}' successfully." });
            }
            catch (Exception ex)
            {
                // Returns an error message if an exception occurs
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        // Example usage:
        // DELETE /api/managetopics/delete-subscription?topicName=TopicName&subscriptionName=SubscriptionToDelete
        // This will delete the subscription named 'SubscriptionToDelete' from the topic 'TopicName'.
    }
}
