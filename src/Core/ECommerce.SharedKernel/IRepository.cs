using System.Linq.Expressions;
using Ardalis.Result;

namespace ECommerce.SharedKernel;

public interface IRepository<TEntity>
{
    IQueryable<TEntity> Query(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        bool isTracking = false);

    Task<PagedResult<ICollection<TEntity>>> GetPagedAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        int page = 1,
        int pageSize = 10,
        bool isTracking = false,
        CancellationToken cancellationToken = default);

    PagedResult<ICollection<TEntity>> GetPaged(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        int page = 1,
        int pageSize = 10,
        bool isTracking = false);

    Task<TEntity?> GetByIdAsync(Guid id, bool isTracking = false, CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    TEntity Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(Guid id);

    void Delete(TEntity entity);
}
