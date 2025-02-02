using Ardalis.Result;
using ECommerce.Application.Common.Helpers;
using MediatR;

namespace ECommerce.Application.Common.CQRS;

public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly L _l;
    public BaseHandler(L l)
    {
        _l = l;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
