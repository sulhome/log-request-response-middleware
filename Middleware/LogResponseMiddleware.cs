using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;

public class LogResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogRequestMiddleware> _logger;

    public LogResponseMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var bodyStream = context.Response.Body;

        var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await _next(context);
     
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
        _logger.LogInformation($"RESPONSE LOG: {responseBody}");
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        await responseBodyStream.CopyToAsync(bodyStream);
    }
}