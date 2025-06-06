using Ardalis.Result;
using ECommerce.Application.CQRS;
using ECommerce.Application.Extensions;
using ECommerce.Application.Repositories;
using ECommerce.Application.Features.Products.DTOs;
using ECommerce.Application.Parameters;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Products.Queries;

public sealed record GetAllProductsQuery(PageableRequestParams PageableRequestParams, bool IncludeCategory = false) : IRequest<PagedResult<List<ProductDto>>>;

public sealed class GetAllProductsQueryHandler(
    IProductRepository productRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<GetAllProductsQuery, PagedResult<List<ProductDto>>>(lazyServiceProvider)
{
    public override async Task<PagedResult<List<ProductDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await productRepository.Query(
             include: x => x.IncludeIf(query.IncludeCategory, y => y.Category))
            .ApplyPagingAsync<Product, ProductDto>(query.PageableRequestParams, cancellationToken: cancellationToken);
    }
}