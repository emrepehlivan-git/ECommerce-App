using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Common.Repositories;
using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries;

public sealed record GetAllCategoriesQuery(PageableRequestParams PageableRequestParams) : IRequest<PagedResult<List<CategoryDto>>>;

internal sealed class GetAllCategoriesQueryHandler : BaseHandler<GetAllCategoriesQuery, PagedResult<List<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(
        ICategoryRepository categoryRepository,
        ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        _categoryRepository = categoryRepository;
    }

    public override async Task<PagedResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Query()
            .ProjectToType<CategoryDto>()
            .ApplyPagingAsync(query.PageableRequestParams, cancellationToken);

    }
}