using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace LogResReqMiddleware.UnitTest
{
    public class LogResponseMiddlewareTest
    {
        [Fact]
        public async void It_Should_Log_Response()
        {
            var loggerMock = new Mock<ILogger<LogResponseMiddleware>>();            

            var logResponseMiddleware = new LogResponseMiddleware(next: async (innerHttpContext) => 
            {
                await innerHttpContext.Response.WriteAsync("test response body");
            }, logger: loggerMock.Object);

            await logResponseMiddleware.Invoke(new DefaultHttpContext());
            loggerMock.Verify(m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<string>(v => v.Contains("RESPONSE LOG: test response body")),
                    null,
                    It.IsAny<Func<string, Exception, string>>()));
        }
    }
}
