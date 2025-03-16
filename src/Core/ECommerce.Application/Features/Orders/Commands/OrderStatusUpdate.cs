using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands;

public sealed record OrderStatusUpdateCommand(
    Guid OrderId,
    OrderStatus NewStatus) : IRequest<Result>, IValidateRequest, ITransactionalRequest;

internal sealed class OrderStatusUpdateCommandValidator : AbstractValidator<OrderStatusUpdateCommand>
{
    public OrderStatusUpdateCommandValidator(
        IOrderRepository orderRepository,
        LocalizationHelper localizer)
    {
        RuleFor(x => x.OrderId)
            .MustAsync(async (id, ct) =>
                await orderRepository.AnyAsync(x => x.Id == id, cancellationToken: ct))
            .WithMessage(localizer[OrderConsts.NotFound]);

        RuleFor(x => x.NewStatus)
            .IsInEnum()
            .WithMessage("Invalid order status");
    }
}

internal sealed class OrderStatusUpdateCommandHandler(
    IOrderRepository orderRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<OrderStatusUpdateCommand, Result>(lazyServiceProvider)
{
    public override async Task<Result> Handle(OrderStatusUpdateCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(command.OrderId, cancellationToken: cancellationToken);

        if (!IsValidStatusTransition(order!.Status, command.NewStatus))
            return Result.Error("Invalid status transition");

        order.UpdateStatus(command.NewStatus);

        orderRepository.Update(order);

        return Result.Success();
    }

    private static bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        return (currentStatus, newStatus) switch
        {
            (OrderStatus.Pending, OrderStatus.Processing) => true,
            (OrderStatus.Pending, OrderStatus.Cancelled) => true,
            (OrderStatus.Processing, OrderStatus.Shipped) => true,
            (OrderStatus.Processing, OrderStatus.Cancelled) => true,
            (OrderStatus.Shipped, OrderStatus.Delivered) => true,
            _ => false
        };
    }
}