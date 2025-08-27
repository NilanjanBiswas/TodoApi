using System.Net;
using System.Text.Json;

namespace TodoApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var resp = new
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Details = ex.Message // remove details in production
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(resp));
            }
        }
    }
}
