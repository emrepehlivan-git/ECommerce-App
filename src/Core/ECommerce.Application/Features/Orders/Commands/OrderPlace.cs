using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands;

public sealed record OrderPlaceCommand(
    Guid UserId,
    string ShippingAddress,
    string BillingAddress,
    List<OrderItemRequest> Items) : IRequest<Result<Guid>>, IValidateRequest, ITransactionalRequest;

public sealed record OrderItemRequest(
    Guid ProductId,
    int Quantity);

public sealed class OrderPlaceCommandValidator : AbstractValidator<OrderPlaceCommand>
{
    public OrderPlaceCommandValidator(
        IProductRepository productRepository,
        LocalizationHelper localizer)
    {
        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .WithMessage(localizer[OrderConsts.ShippingAddressRequired]);

        RuleFor(x => x.BillingAddress)
            .NotEmpty()
            .WithMessage(localizer[OrderConsts.BillingAddressRequired]);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(localizer[OrderConsts.EmptyOrder]);

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .MustAsync(async (id, ct) =>
                    await productRepository.AnyAsync(x => x.Id == id, cancellationToken: ct))
                .WithMessage(localizer[OrderConsts.ProductNotFound]);

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(localizer[OrderConsts.QuantityMustBeGreaterThanZero]);
        });
    }
}

public sealed class OrderPlaceCommandHandler(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IStockRepository stockRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<OrderPlaceCommand, Result<Guid>>(lazyServiceProvider)
{
    public override async Task<Result<Guid>> Handle(OrderPlaceCommand command, CancellationToken cancellationToken)
    {
        var order = Order.Create(
            command.UserId,
            command.ShippingAddress,
            command.BillingAddress);

        foreach (var item in command.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken: cancellationToken);
            if (product is not null)
            {
                await stockRepository.ReserveStockAsync(item.ProductId, item.Quantity, cancellationToken);
                order.AddItem(item.ProductId, product.Price, item.Quantity);
            }
        }

        orderRepository.Add(order);

        return Result.Success(order.Id);
    }
}