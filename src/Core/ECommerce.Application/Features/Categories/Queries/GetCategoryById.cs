using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Application.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(Guid Id, bool IncludeProducts = false) : IRequest<Result<CategoryWithProductsDto>>;

internal sealed class GetCategoryByIdQueryHandler(
    ICategoryRepository categoryRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<GetCategoryByIdQuery, Result<CategoryWithProductsDto>>(lazyServiceProvider)
{
    public override async Task<Result<CategoryWithProductsDto>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(query.Id,
              include: x => x.IncludeIf(query.IncludeProducts, c => c.Products),
              cancellationToken: cancellationToken);

        if (category is null)
            return Result.NotFound(Localizer[CategoryConsts.NotFound]);

        return Result.Success(category.Adapt<CategoryWithProductsDto>());
    }
}