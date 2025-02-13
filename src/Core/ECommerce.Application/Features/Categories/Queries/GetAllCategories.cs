using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Repositories;
using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries;

public sealed record GetAllCategoriesQuery(PageableRequestParams PageableRequestParams) : IRequest<PagedResult<List<CategoryDto>>>;

internal sealed class GetAllCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<GetAllCategoriesQuery, PagedResult<List<CategoryDto>>>(lazyServiceProvider)
{
    public override async Task<PagedResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await categoryRepository.Query()
            .ProjectToType<CategoryDto>()
            .ApplyPagingAsync(query.PageableRequestParams, cancellationToken);

    }
}