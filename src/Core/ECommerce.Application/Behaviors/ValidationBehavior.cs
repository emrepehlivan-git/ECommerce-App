using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger,
    ILocalizationService localizationService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IValidateRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(e => e.Errors)
            .Select(e => new ValidationFailure(e.PropertyName, localizationService.GetLocalizedString(e.ErrorMessage)))
            .ToList();

        if (failures.Count > 0)
        {
            logger.LogWarning("Validation failed for request {RequestType}. Errors: {Errors}", typeof(TRequest).Name, failures);
            throw new ValidationException(failures);
        }

        return await next();
    }
}