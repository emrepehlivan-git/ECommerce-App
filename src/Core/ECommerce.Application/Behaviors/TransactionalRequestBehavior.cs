using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
namespace ECommerce.Application.Behaviors;

public sealed class TransactionalRequestBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionalRequestBehavior<TRequest, TResponse>> logger)
 : IPipelineBehavior<TRequest, TResponse>
 where TRequest : ITransactionalRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var response = await next();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return response;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(exception, "Transaction failed for request {RequestType}", typeof(TRequest).Name);
            throw;
        }
    }
}
