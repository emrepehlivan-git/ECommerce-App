using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using ECommerce.SharedKernel;
using Microsoft.Extensions.Logging;

namespace ECommerce.Persistence.Repositories;

public sealed class StockRepository(ApplicationDbContext context, ILogger<StockRepository> logger) : IStockRepository, IScopedDependency
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<StockRepository> _logger = logger;

    public async Task ReserveStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await _context.ProductStocks.FindAsync([productId], cancellationToken);
        if (stock is null)
        {
            _logger.LogError("Stock not found for product {ProductId}", productId);
            throw new NotFoundException($"Stock not found for product {productId}");
        }

        if (stock.Quantity < quantity)
        {
            _logger.LogError("Insufficient stock for product {ProductId}. Available: {Available}, Requested: {Requested}",
                productId, stock.Quantity, quantity);
            throw new BusinessException($"Insufficient stock for product {productId}");
        }

        stock.Reserve(quantity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReleaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await _context.ProductStocks.FindAsync([productId], cancellationToken);
        if (stock is null)
        {
            _logger.LogError("Stock not found for product {ProductId}", productId);
            throw new NotFoundException($"Stock not found for product {productId}");
        }

        stock.Release(quantity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
