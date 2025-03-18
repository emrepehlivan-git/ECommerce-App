using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using MediatR;
using Ardalis.Result;
using ECommerce.Application.Common.CQRS;

namespace ECommerce.Application.Features.Products.Queries;


public sealed record GetProductStockInfo(Guid ProductId) : IRequest<Result<int>>;
public sealed class GetProductStockInfoHandler(
    IProductRepository productRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<GetProductStockInfo, Result<int>>(lazyServiceProvider)
{
    public override async Task<Result<int>> Handle(GetProductStockInfo request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken: cancellationToken);

        if (product is null)
        {
            return Result.NotFound();
        }

        return Result.Success(product.StockQuantity);
    }
}
