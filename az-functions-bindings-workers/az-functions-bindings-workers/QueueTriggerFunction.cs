using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace az_functions_bindings_workers
{
    public class QueueTriggerFunction
    {
        private readonly ILogger<QueueTriggerFunction> _logger;

        public QueueTriggerFunction(ILogger<QueueTriggerFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(QueueTriggerFunction))]
        public void Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {

            _logger.LogInformation($"!!!!!!!!!!!!!!!!!!!!!! Queue trigger function processed: {message.MessageText}");
        }
    }
}
