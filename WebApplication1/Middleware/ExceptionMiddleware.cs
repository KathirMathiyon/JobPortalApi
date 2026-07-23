using System.Text.Json;

namespace WebApplication1.Middleware
{
    public class ExceptionMiddleware
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"An unhandled error occured");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = Azure.Core.ContentType.ApplicationJson.ToString();

                var errorResponse = new
                {
                    Message = "Please try again later",
                    StatusCode = 500
                };

                var json = JsonSerializer.Serialize(errorResponse);

                await context.Response.WriteAsync(json);
            }
        }

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
    }
}