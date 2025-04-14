using System.Linq.Expressions;
using Ardalis.Result;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Parameters;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using ECommerce.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> Table;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        Table = context.Set<TEntity>();
    }

    public TEntity Add(TEntity entity)
    {
        Table.Add(entity);
        return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.CountAsync(predicate, cancellationToken);
    }

    public void Delete(Guid id)
    {
        var entity = Table.Find(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public void Delete(TEntity entity)
    {
        Table.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        Table.RemoveRange(entities);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id,
         Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? include = null,
         bool isTracking = false,
          CancellationToken cancellationToken = default)
    {
        var query = Query(x => x.Id == id, isTracking: isTracking, include: include);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public PagedResult<IEnumerable<TEntity>> GetPaged(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>? orderBy = null,
        Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? include = null,
        int page = 1,
        int pageSize = 10,
        bool isTracking = false)
    {
        var query = Query(predicate, orderBy, include, isTracking);
        return query.ApplyPaging(new PageableRequestParams(page, pageSize));
    }

    public Task<PagedResult<IEnumerable<TEntity>>> GetPagedAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>? orderBy = null,
        Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? include = null,
        int page = 1,
        int pageSize = 10,
        bool isTracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = Query(predicate, orderBy, include, isTracking);
        return query.ApplyPagingAsync<TEntity, TEntity>(new PageableRequestParams(page, pageSize), predicate: predicate, cancellationToken: cancellationToken);
    }

    public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Table.LongCountAsync(predicate, cancellationToken);
    }

    public IQueryable<TEntity> Query(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>? orderBy = null,
        Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? include = null,
        bool isTracking = false)
    {
        var query = Table.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (include != null)
        {
            query = include.Compile()(query);
        }

        if (orderBy != null)
        {
            query = orderBy.Compile()(query);
        }

        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    public void Update(TEntity entity)
    {
        Table.Update(entity);
    }
}
