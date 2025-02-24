using System.Net;
using System.Text.Json;
using Ardalis.Result;
using FluentValidation;

namespace ECommerce.WebAPI.Middlewares;

internal sealed class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var result = exception switch
        {
            ValidationException validationException =>
                Result.Invalid(validationException.Errors.Select(x =>
                    new ValidationError(x.PropertyName, x.ErrorMessage, x.ErrorCode, ValidationSeverity.Error))
                    .ToList()),

            UnauthorizedAccessException =>
                Result.Unauthorized(),

            KeyNotFoundException =>
                Result.NotFound(),

            _ => Result.Error(exception.Message)
        };

        context.Response.StatusCode = result.Status switch
        {
            ResultStatus.Invalid => (int)HttpStatusCode.BadRequest,
            ResultStatus.Unauthorized => (int)HttpStatusCode.Unauthorized,
            ResultStatus.NotFound => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var jsonResponse = JsonSerializer.Serialize(result);
        await context.Response.WriteAsync(jsonResponse);
    }
}