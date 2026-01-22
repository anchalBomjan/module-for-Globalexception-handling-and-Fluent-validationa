
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Middleware
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            var problemDetails = new
            {
                Title = "An error occurred",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = exception.Message,
                Errors = new Dictionary<string, string[]>()
            };

            switch (exception)
            {
                case ValidationException validationException:
                    problemDetails = new
                    {
                        Title = "Validation Error",
                        Status = (int)HttpStatusCode.BadRequest,
                        Detail = "One or more validation errors occurred.",
                        Errors = validationException.Errors
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(problemDetails);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
