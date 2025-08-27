using Microsoft.AspNetCore.Builder;
using TodoApi.Middleware;
namespace TodoApi.Extensions
{
    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
