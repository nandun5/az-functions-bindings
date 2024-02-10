using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace az_functions_bindings
{
    public class QueueFunction2
    {
        [FunctionName("Queue2")]
        public void Run([QueueTrigger("myqueue-items", Connection = "EventCenterStorageAccount")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"!!!!!!!!!!!!!!!!!!!!!!!!!MESSAGE RECEIVED TO FUNCTION 2 IS: {myQueueItem}");
        }
    }
}
