using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Application.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
