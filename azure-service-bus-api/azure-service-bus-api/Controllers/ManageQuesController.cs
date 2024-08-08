using azure_service_bus_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace azure_service_bus_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageQuesController : ControllerBase
    {
        private readonly ITTMQueService _queueService;

        // Constructor to inject the ITTMQueService dependency
        public ManageQuesController(ITTMQueService queueService)
        {
            _queueService = queueService;
        }

        /// <summary>
        /// Creates a new queue with the specified name.
        /// </summary>
        /// <param name="queueName">The name of the queue to create.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateQueueAsync(string queueName)
        {
            // Calls the service to create a new queue
            var result = await _queueService.CreateQueueAsync(queueName);
            // Returns an OK response with the result
            return Ok(result);
        }

        /// <summary>
        /// Renames an existing queue from the old name to the new name.
        /// </summary>
        /// <param name="oldQueueName">The current name of the queue.</param>
        /// <param name="newQueueName">The new name to assign to the queue.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("update")]
        public async Task<IActionResult> RenameQueueAsync(string oldQueueName, string newQueueName)
        {
            // Calls the service to rename the queue
            var result = await _queueService.UpdateQueueAsync(oldQueueName, newQueueName);
            // Returns an OK response with the result
            return Ok(result);
        }

        /// <summary>
        /// Deletes the queue with the specified name.
        /// </summary>
        /// <param name="queueName">The name of the queue to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteQueueAsync(string queueName)
        {
            // Calls the service to delete the queue
            var result = await _queueService.DeleteQueueAsync(queueName);
            // Returns an OK response with the result
            return Ok(result);
        }
    }
}
