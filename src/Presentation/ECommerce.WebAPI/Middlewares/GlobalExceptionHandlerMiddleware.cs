using System.Net;
using System.Text.Json;
using Ardalis.Result;
using ECommerce.Application.Exceptions;
using FluentValidation;

namespace ECommerce.WebAPI.Middlewares;

public sealed class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
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

            NotFoundException =>
                Result.NotFound(exception.Message),

            BusinessException =>
                Result.Error(exception.Message),

            _ => Result.Error(exception.Message)
        };

        context.Response.StatusCode = result.Status switch
        {
            ResultStatus.Invalid => (int)HttpStatusCode.BadRequest,
            ResultStatus.Unauthorized => (int)HttpStatusCode.Unauthorized,
            ResultStatus.NotFound => (int)HttpStatusCode.NotFound,
            ResultStatus.Error => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var jsonResponse = JsonSerializer.Serialize(result);
        await context.Response.WriteAsync(jsonResponse);
    }
}