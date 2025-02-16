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
}