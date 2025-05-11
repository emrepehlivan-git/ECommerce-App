using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public record class UpdateProductStock(Guid ProductId, int StockQuantity) : IRequest<Result>, IValidatableRequest, ITransactionalRequest
;

public sealed class UpdateProductStockValidator : AbstractValidator<UpdateProductStock>
{
    public UpdateProductStockValidator(IProductRepository productRepository, LocalizationHelper localizer)
    {
        RuleFor(x => x.ProductId)
            .MustAsync(async (id, ct) => await productRepository.AnyAsync(x => x.Id == id, cancellationToken: ct))
            .WithMessage(localizer[ProductConsts.NotFound]);

        RuleFor(x => x.StockQuantity)
            .GreaterThan(0)
            .WithMessage(localizer[ProductConsts.StockQuantityMustBeGreaterThanZero]);
    }
}

public sealed class UpdateProductStockHandler(
    IStockRepository stockRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<UpdateProductStock, Result>(lazyServiceProvider)
{
    public override async Task<Result> Handle(UpdateProductStock request, CancellationToken cancellationToken)
    {
        await stockRepository.ReserveStockAsync(request.ProductId, request.StockQuantity, cancellationToken);

        return Result.Success();
    }
}