using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Result;
using ECommerce.Application.Common.Parameters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static PagedResult<List<T>> ApplyPaging<T>(this IQueryable<T> query, PageableRequestParams pageableRequestParams)
    {
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageableRequestParams.PageSize);
        var pageInfo = new PagedInfo(pageableRequestParams.Page, pageableRequestParams.PageSize, totalPages, totalCount);
        var items = query.Skip((pageableRequestParams.Page - 1) * pageableRequestParams.PageSize).Take(pageableRequestParams.PageSize).ToList();
        return new PagedResult<List<T>>(pageInfo, items);
    }

    public static async Task<PagedResult<List<T>>> ApplyPagingAsync<T>(this IQueryable<T> query, PageableRequestParams pageableRequestParams, CancellationToken cancellationToken = default)
    {
        var totalCount = await query.LongCountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageableRequestParams.PageSize);
        var pageInfo = new PagedInfo(pageableRequestParams.Page, pageableRequestParams.PageSize, totalPages, totalCount);
        var items = await query.Skip((pageableRequestParams.Page - 1) * pageableRequestParams.PageSize).Take(pageableRequestParams.PageSize)
        .ToListAsync(cancellationToken);
        return new PagedResult<List<T>>(pageInfo, items);
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