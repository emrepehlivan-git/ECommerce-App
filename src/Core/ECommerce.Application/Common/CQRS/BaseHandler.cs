using ECommerce.Application.Common.Helpers;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Common.CQRS;

public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected ILazyServiceProvider LazyServiceProvider { get; }

    protected LocalizationHelper Localizer => LazyServiceProvider.LazyGetRequiredService<LocalizationHelper>();

    protected BaseHandler(ILazyServiceProvider lazyServiceProvider)
    {
        LazyServiceProvider = lazyServiceProvider;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

}
