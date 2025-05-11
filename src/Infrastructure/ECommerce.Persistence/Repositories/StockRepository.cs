using ECommerce.Application.Exceptions;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Persistence.Repositories;

public sealed class StockRepository(ApplicationDbContext context, ILogger<StockRepository> logger) :
 BaseRepository<ProductStock>(context),
  IStockRepository
{
    public async Task ReserveStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await Query().FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        if (stock is null)
        {
            logger.LogError("Stock not found for product {ProductId}", productId);
            throw new NotFoundException($"Stock not found for product {productId}");
        }

        if (stock.Quantity < quantity)
        {
            logger.LogError("Insufficient stock for product {ProductId}. Available: {Available}, Requested: {Requested}",
                productId, stock.Quantity, quantity);
            throw new BusinessException($"Insufficient stock for product {productId}");
        }

        stock.Reserve(quantity);
        Update(stock);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReleaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await Query().FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        if (stock is null)
        {
            logger.LogError("Stock not found for product {ProductId}", productId);
            throw new NotFoundException($"Stock not found for product {productId}");
        }

        stock.Release(quantity);
        Update(stock);
        await Context.SaveChangesAsync(cancellationToken);
    }
}
