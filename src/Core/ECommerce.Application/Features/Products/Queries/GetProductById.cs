using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Features.Products.DTOs;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Products.Queries;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;

internal sealed class GetProductByIdQueryHandler : BaseHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        _productRepository = productRepository;
    }

    public override async Task<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.Query()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (product is null)
            return Result.NotFound(Localizer[ProductConsts.NotFound]);

        return Result.Success(product.Adapt<ProductDto>());
    }
}