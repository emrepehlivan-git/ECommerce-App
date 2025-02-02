using Ardalis.Result;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IValidetableRequest
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(e => e.Errors)
            .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage, e.ErrorCode, ValidationSeverity.Error))
            .ToList();

        if (failures.Any())
            return (TResponse)Result.Invalid(failures);

        return await next();
    }
}