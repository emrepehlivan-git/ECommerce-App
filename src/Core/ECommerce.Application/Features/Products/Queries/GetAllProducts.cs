using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Repositories;
using ECommerce.Application.Features.Products.DTOs;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Products.Queries;

public sealed record GetAllProductsQuery(PageableRequestParams PageableRequestParams) : IRequest<PagedResult<List<ProductDto>>>;

internal sealed class GetAllProductsQueryHandler : BaseHandler<GetAllProductsQuery, PagedResult<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(
        IProductRepository productRepository,
        ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        _productRepository = productRepository;
    }

    public override async Task<PagedResult<List<ProductDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await _productRepository.Query()
            .Include(x => x.Category)
            .ProjectToType<ProductDto>()
            .ApplyPagingAsync(query.PageableRequestParams, cancellationToken);
    }
}