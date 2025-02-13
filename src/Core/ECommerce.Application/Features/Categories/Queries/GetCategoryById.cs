using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Repositories;
using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryDto>>;

internal sealed class GetCategoryByIdQueryHandler : BaseHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository categoryRepository,
        ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        _categoryRepository = categoryRepository;
    }

    public override async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken: cancellationToken);

        if (category is null)
            return Result.NotFound(Localizer[CategoryConsts.NotFound]);

        return Result.Success(new CategoryDto(
            category.Id,
            category.Name));
    }
}