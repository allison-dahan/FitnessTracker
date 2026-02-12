using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace FitnessTracker.Web.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next, 
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Handle status code scenarios
                switch (context.Response.StatusCode)
                {
                    case StatusCodes.Status404NotFound:
                        LogStatusCodeEvent(LogLevel.Warning, "Not Found", context);
                        break;
                    case StatusCodes.Status500InternalServerError:
                        LogStatusCodeEvent(LogLevel.Error, "Internal Server Error", context);
                        break;
                    case StatusCodes.Status403Forbidden:
                        LogStatusCodeEvent(LogLevel.Warning, "Forbidden", context);
                        break;
                    case StatusCodes.Status401Unauthorized:
                        LogStatusCodeEvent(LogLevel.Warning, "Unauthorized", context);
                        break;
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private void LogStatusCodeEvent(LogLevel logLevel, string errorType, HttpContext context)
        {
            _logger.Log(logLevel, 
                "Status Code Event: {ErrorType} - Environment: {Environment}, " +
                "Path: {Path}, Method: {Method}, Remote IP: {RemoteIp}", 
                errorType, 
                _environment.EnvironmentName,
                context.Request.Path,
                context.Request.Method,
                context.Connection.RemoteIpAddress);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Determine status code based on exception type
            var statusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            // Log the exception with environment details
            _logger.LogError(exception, 
                "Unhandled Exception - Environment: {Environment}, " +
                "Path: {Path}, Method: {Method}, Remote IP: {RemoteIp}", 
                _environment.EnvironmentName,
                context.Request.Path,
                context.Request.Method,
                context.Connection.RemoteIpAddress);

            // Prepare error response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Determine error message based on environment
            var errorMessage = _environment.IsDevelopment()
                ? exception.ToString()
                : "An unexpected error occurred.";

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = statusCode,
                Environment = _environment.EnvironmentName,
                Message = errorMessage
            }.ToString());
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Environment { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
        }
    }

    // Extension method to add the middleware
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}