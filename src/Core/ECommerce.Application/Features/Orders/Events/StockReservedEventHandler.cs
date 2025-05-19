using ECommerce.Application.Repositories;
using ECommerce.Domain.Events.Stock;
using MediatR;

namespace ECommerce.Application.Features.Orders.Events;

public sealed class StockReservedEventHandler : INotificationHandler<StockReservedEvent>
{
    private readonly IStockRepository _stockRepository;

    public StockReservedEventHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task Handle(StockReservedEvent notification, CancellationToken cancellationToken)
    {
        await _stockRepository.ReserveStockAsync(notification.ProductId, notification.Quantity, cancellationToken);
    }
}