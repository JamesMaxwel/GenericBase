using GenericBase.Application.Helpers.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace GenericBase.Infra.Ioc.Extensions.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _log;

        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory log)
        {
            _next = next;
            _log = log.CreateLogger("MyErrorHandler");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (StatusCodeException ex)
            {
                await HandleErrorAsync(httpContext, ex, ex.StatusCode);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(httpContext, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            _log.LogError($"Error: {exception.Message} \nStack: {exception.StackTrace}");

            var error = new { Error = exception.Message, StatusCode = statusCode };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}
