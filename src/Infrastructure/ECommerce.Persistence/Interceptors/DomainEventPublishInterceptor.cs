using ECommerce.SharedKernel.Events;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.Persistence.Interceptors;

public sealed class DomainEventPublishInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DomainEventPublishInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
            return result;

        var domainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .SelectMany(e =>
            {
                var events = e.DomainEvents.ToList();
                e.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}