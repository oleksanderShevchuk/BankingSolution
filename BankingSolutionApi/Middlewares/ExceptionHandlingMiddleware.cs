using System.Net;
using System.Text.Json;
using BankingSolutionApi.Responses;

namespace BankingSolutionApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    InvalidOperationException => (int)HttpStatusCode.Conflict,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var response = ApiResponse<string>.Fail(ex.Message);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
