using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Result;
using ECommerce.Application.Common.Parameters;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ECommerce.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static PagedResult<IEnumerable<T>> ApplyPaging<T>(this IQueryable<T> query, PageableRequestParams pageableRequestParams)
    {
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageableRequestParams.PageSize);
        var pageInfo = new PagedInfo(pageableRequestParams.Page, pageableRequestParams.PageSize, totalPages, totalCount);
        var items = query.Take(((pageableRequestParams.Page - 1) * pageableRequestParams.PageSize)..pageableRequestParams.PageSize).ToList();
        return new PagedResult<IEnumerable<T>>(pageInfo, items);
    }

    public static async Task<PagedResult<IEnumerable<TDestination>>> ApplyPagingAsync<TSource, TDestination>(
    this IQueryable<TSource> query,
    PageableRequestParams pageableRequestParams,
    Expression<Func<TSource, bool>>? predicate = null,
    CancellationToken cancellationToken = default)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        int count;
        List<TDestination> items;

        var skip = (pageableRequestParams.Page - 1) * pageableRequestParams.PageSize;
        var take = pageableRequestParams.PageSize;

        if (query.Provider is IAsyncQueryProvider)
        {
            count = await query.CountAsync(predicate ?? (x => true), cancellationToken);

            if (typeof(TSource) == typeof(TDestination))
            {
                var list = await query
                    .Take(skip..take)
                    .Cast<TDestination>()
                    .ToListAsync(cancellationToken);

                items = list;
            }
            else
            {
                items = await query
                    .Take(skip..take)
                    .ProjectToType<TDestination>()
                    .ToListAsync(cancellationToken);
            }
        }
        else
        {
            count = query.Count(predicate ?? (x => true));

            var sourceItems = query
                .Skip(skip)
                .Take(take)
                .ToList();

            if (typeof(TSource) == typeof(TDestination))
            {
                items = sourceItems.Cast<TDestination>().ToList();
            }
            else
            {
                items = sourceItems.Adapt<List<TDestination>>();
            }
        }

        var totalPages = (int)Math.Ceiling((double)count / pageableRequestParams.PageSize);
        var pageInfo = new PagedInfo(pageableRequestParams.Page, pageableRequestParams.PageSize, totalPages, count);
        return new PagedResult<IEnumerable<TDestination>>(pageInfo, items);
    }


    public static IQueryable<T> IncludeIf<T>(this IQueryable<T> query,
    bool condition,
    Expression<Func<T, object>> include)
    where T : class
    {
        if (condition)
        {
            query = query.Include(include);
        }
        return query;
    }

    public static IQueryable<T> OrderByIf<T>(this IQueryable<T> query, string? orderBy, bool condition)
    {
        if (condition && !string.IsNullOrWhiteSpace(orderBy))
        {
            return ApplyOrder(query, orderBy, "OrderBy");
        }
        return query;
    }

    private static IQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
    {
        string[] props = property.Split('.');
        Type type = typeof(T);
        ParameterExpression arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach (string prop in props)
        {
            PropertyInfo? pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (pi == null)
                return source;
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }
        Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

        object? result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, [source, lambda]);

        return (IQueryable<T>)(result ?? source);
    }
}