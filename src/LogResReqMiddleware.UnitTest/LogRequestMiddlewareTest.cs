using LogResReqMiddleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

public class LogRequestMiddlewareTest
{
    [Fact]
    public async void It_Should_Log_Request()
    {
        var loggerMock = new Mock<ILogger<LogRequestMiddleware>>();
        var requestMock = new Mock<HttpRequest>();        
        requestMock.Setup(x => x.Scheme).Returns("http");
        requestMock.Setup(x => x.Host).Returns(new HostString("localhost"));
        requestMock.Setup(x => x.Path).Returns(new PathString("/test"));
        requestMock.Setup(x => x.PathBase).Returns(new PathString("/"));
        requestMock.Setup(x => x.Method).Returns("GET");
        requestMock.Setup(x => x.Body).Returns(new MemoryStream());
        requestMock.Setup(x => x.QueryString).Returns(new QueryString("?param1=2"));
        

        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(x => x.Request).Returns(requestMock.Object);

        var logRequestMiddleware = new LogRequestMiddleware(next: (innerHttpContext) => Task.FromResult(0), logger: loggerMock.Object);


        await logRequestMiddleware.Invoke(contextMock.Object);
        loggerMock.Verify(m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<string>(v => v.Contains("REQUEST METHOD: GET, REQUEST BODY: , REQUEST URL: http://localhost//test?param1=2")),
                null,
                It.IsAny<Func<string, Exception, string>>()));
    }
}