using EmployeeApp.Api.Exceptions;
using EmployeeApp.Api.Models.Dtos;
using Microsoft.AspNetCore.Diagnostics;

namespace EmployeeApp.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger= logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An Unexpected Error Occurred : {Message}", exception.Message);

            var (statusCode, message) = exception switch{

                EmployeeNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                InvalidAgeException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, exception.Message)

            };

            var response = new ErrorResponse
            {
                StatusCode=statusCode,
                Message=message,
                TimeStamp=DateTime.UtcNow,
                Path=httpContext.Request.Path

            };
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(response);

            return true;
        }
    }
}
