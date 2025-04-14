using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Repositories;
using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using ECommerce.Application.Features.Products.DTOs;
using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Categories.Queries;

public sealed record GetAllCategoriesQuery(PageableRequestParams PageableRequestParams, string? OrderBy = null) : IRequest<PagedResult<IEnumerable<CategoryDto>>>;
public sealed class GetAllCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<GetAllCategoriesQuery, PagedResult<IEnumerable<CategoryDto>>>(lazyServiceProvider)
{
    public override async Task<PagedResult<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await categoryRepository.Query(
            orderBy: x => query.OrderBy == "products" ? x.OrderBy(c => c.Products.Count) : x.OrderBy(c => c.Name)
        )
        .ApplyPagingAsync<Category, CategoryDto>(query.PageableRequestParams, cancellationToken: cancellationToken);
    }
}