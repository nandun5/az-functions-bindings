using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace az_functions_bindings_workers;

/// <summary>
/// Series of HTTP Trigger Isolated process functions to test out various bindings
/// </summary>
public class HttpFunction
{
    private readonly ILogger<HttpFunction> _logger;

    public HttpFunction(ILogger<HttpFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Basinc HTTP trigger
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function("http")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("Single HTTP!!!!!!!!!!!!!!!!!!!");
        return req.CreateResponse(HttpStatusCode.Accepted);
    }

    /// <summary>
    /// HTTP Trigger with Queue output binding
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function("http-queue")]
    [QueueOutput("myqueue-items", Connection = "AzureWebJobsStorage")]
    public string[] RunToQueue([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("TO QUEUE HTTP!!!!!!!!!!!!!!!!!!!");

        var res = new[]
            {
                DateTime.Now.ToLongTimeString(),
                req.FunctionContext.FunctionId,
                req.FunctionContext.InvocationId
            };

        return res;
    }

    /// <summary>
    /// HTTP trigger with queue output binding with dynamic queue name
    /// </summary>
    /// <param name="req"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    [Function("http-dynamic-queue")]
    [QueueOutput("myqueue-{source}", Connection = "AzureWebJobsStorage")]
    public string[] RunToDynamicQueue(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "{source}/http-dynamic-queue")] HttpRequestData req,
        string source)
    {
        _logger.LogInformation($"TO QUEUE {source} HTTP!!!!!!!!!!!!!!!!!!!");

        var res = new[]
        {
            DateTime.Now.ToLongTimeString(),
            req.FunctionContext.FunctionId,
            req.FunctionContext.InvocationId
        };

        return null;
    }

    /// <summary>
    /// HTTP trigger with multiple output bindings
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function("http-multiple")]
    public async Task<MultipleResponse> RunMultipleAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("Multiple HTTP ############################");

        var message = new[]
        {
            DateTime.Now.ToLongTimeString(),
            req.FunctionContext.FunctionId,
            req.FunctionContext.InvocationId
        };

        var res = req.CreateResponse(HttpStatusCode.Accepted);

        await res.WriteAsJsonAsync(message);

        return new MultipleResponse
        {
            QueueMessages = message,
            HttpResponse = res
        };
    }

    /// <summary>
    /// Http trigger with multiple output bindings including a queue with dynamic name
    /// </summary>
    /// <param name="req"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    [Function("http-multiple-dynamic")]
    public async Task<MultipleDynamicResponse> RunMultipleDynamicAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "{source}/http-multiple-dynamic")] HttpRequestData req,
        string source)
    {
        _logger.LogInformation("Multiple HTTP ############################");

        var message = new[]
        {
            DateTime.Now.ToLongTimeString(),
            req.FunctionContext.FunctionId,
            req.FunctionContext.InvocationId
        };

        var res = req.CreateResponse(HttpStatusCode.Accepted);

        await res.WriteAsJsonAsync(message);

        return new MultipleDynamicResponse
        {
            QueueMessages = message,
            HttpResponse = res
        };
    }
}

public class MultipleResponse
{
    [QueueOutput("myqueue-items2", Connection = "AzureWebJobsStorage")]
    public string[] QueueMessages { get; set; }

    public HttpResponseData HttpResponse { get; set; }
}


public class MultipleDynamicResponse
{
    [QueueOutput("myqueue-{source}", Connection = "AzureWebJobsStorage")]
    public string[] QueueMessages { get; set; }

    public HttpResponseData HttpResponse { get; set; }
}