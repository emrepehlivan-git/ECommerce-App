using ECommerce.Application.Common.Interfaces;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Persistence.Services;

public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<IDbContextTransaction> BeginTransactionAsync() => await context.Database.BeginTransactionAsync();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await context.SaveChangesAsync(cancellationToken);

    public int SaveChanges() => context.SaveChanges();
}
