using ECommerce.Application.Common.Helpers;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Common.CQRS;

public abstract class BaseHandler<TRequest, TResponse>(ILazyServiceProvider lazyServiceProvider) : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected ILazyServiceProvider LazyServiceProvider { get; } = lazyServiceProvider;

    protected LocalizationHelper Localizer => LazyServiceProvider.LazyGetRequiredService<LocalizationHelper>();

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

}
