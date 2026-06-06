using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.DTOs;
using System.Diagnostics;

namespace CustomTaskFlow.Api.Middlewares
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

        public async Task InvokeAsync(HttpContext context ) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Something went wrong");

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = ApiResponse<string>.ErrorResponse(
                    ["Something unexpected happened"],
                    "Internal server error");

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
