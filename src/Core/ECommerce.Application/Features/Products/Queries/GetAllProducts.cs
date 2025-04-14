using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Repositories;
using ECommerce.Application.Features.Products.DTOs;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Products.Queries;

public sealed record GetAllProductsQuery(PageableRequestParams PageableRequestParams, bool IncludeCategory = false) : IRequest<PagedResult<IEnumerable<ProductDto>>>;

public sealed class GetAllProductsQueryHandler(
    IProductRepository productRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<GetAllProductsQuery, PagedResult<IEnumerable<ProductDto>>>(lazyServiceProvider)
{
    public override async Task<PagedResult<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await productRepository.Query(
             include: x => x.IncludeIf(query.IncludeCategory, y => y.Category))
            .ApplyPagingAsync<Product, ProductDto>(query.PageableRequestParams, cancellationToken: cancellationToken);
    }
}