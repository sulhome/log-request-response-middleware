using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Http.Extensions;

public class LogRequestMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<LogRequestMiddleware> _logger;

    public LogRequestMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
    {
        this.next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestBodyStream = new MemoryStream();
        var originalRequestBody = context.Request.Body;

        await context.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0,SeekOrigin.Begin);
        
        var url = UriHelper.GetDisplayUrl(context.Request);
        var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();                
        _logger.LogInformation($"REQUEST BODY: {requestBodyText}, REQUEST URL: {url}");

        requestBodyStream.Seek(0,SeekOrigin.Begin);
        context.Request.Body = requestBodyStream;

        await next(context);
        context.Request.Body = originalRequestBody;
    }
}