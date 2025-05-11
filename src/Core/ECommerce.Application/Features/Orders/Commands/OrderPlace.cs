using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Orders.Commands;

public sealed record OrderPlaceCommand(
    Guid UserId,
    string ShippingAddress,
    string BillingAddress,
    List<OrderItemRequest> Items) : IRequest<Result<Guid>>, IValidatableRequest, ITransactionalRequest;

public sealed record OrderItemRequest(
    Guid ProductId,
    int Quantity);

public sealed class OrderPlaceCommandValidator : AbstractValidator<OrderPlaceCommand>
{
    public OrderPlaceCommandValidator(
        IProductRepository productRepository,
        IIdentityService identityService,
        LocalizationHelper localizer)
    {
        RuleFor(x => x.UserId)
            .MustAsync(async (id, ct) =>
                await identityService.FindByIdAsync(id) != null)
            .WithMessage(localizer[OrderConsts.UserNotFound]);

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
    IIdentityService identityService,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<OrderPlaceCommand, Result<Guid>>(lazyServiceProvider)
{
    public override async Task<Result<Guid>> Handle(OrderPlaceCommand command, CancellationToken cancellationToken)
    {
        var user = await identityService.FindByIdAsync(command.UserId);
        if (user is null)
        {
            return Result.Error(Localizer[OrderConsts.UserNotFound]);
        }

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