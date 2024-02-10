using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace az_functions_bindings
{
    public class QueueFunction3
    {
        [FunctionName("Queue3")]
        public void Run([QueueTrigger("myqueue-items", Connection = "EventCenterStorageAccount")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
