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
        var stock = await _context.ProductStocks.FindAsync(productId, cancellationToken);
        if (stock is null)
            throw new InvalidOperationException("Stock not found");

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            stock.Reserve(quantity);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error reserving stock for product {ProductId}", productId);
            throw new InvalidOperationException("Stock not found", ex);
        }
    }

    public async Task ReleaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await _context.ProductStocks.FindAsync(productId, cancellationToken);
        if (stock is null)
        {
            _logger.LogError("Stock not found for product {ProductId}", productId);
            throw new InvalidOperationException("Stock not found");
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            stock.Release(quantity);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error releasing stock for product {ProductId}", productId);
            throw new InvalidOperationException("Stock not found", ex);
        }
    }
}
