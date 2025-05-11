using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.CQRS;
using ECommerce.Application.Helpers;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Orders.Commands;

public sealed record OrderItemRemoveCommand(
    Guid OrderId,
    Guid ProductId) : IRequest<Result>, IValidatableRequest, ITransactionalRequest;

public sealed class OrderItemRemoveCommandValidator : AbstractValidator<OrderItemRemoveCommand>
{
    public OrderItemRemoveCommandValidator(
        IOrderRepository orderRepository,
        LocalizationHelper localizer)
    {
        RuleFor(x => x.OrderId)
            .MustAsync(async (id, ct) =>
                await orderRepository.AnyAsync(x => x.Id == id, cancellationToken: ct))
            .WithMessage(localizer[OrderConsts.NotFound]);

        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
            {
                var order = await orderRepository.Query(x => x.Id == command.OrderId)
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(cancellationToken: ct);

                return order is not null && order.Status == OrderStatus.Pending;
            })
            .WithMessage(localizer[OrderConsts.OrderCannotBeModified]);

        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
            {
                var order = await orderRepository.Query(x => x.Id == command.OrderId)
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(cancellationToken: ct);

                return order is not null && order.Items.Any(i => i.ProductId == command.ProductId);
            })
            .WithMessage(localizer[OrderConsts.OrderItemNotFound]);
    }
}

public sealed class OrderItemRemoveCommandHandler(
    IOrderRepository orderRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<OrderItemRemoveCommand, Result>(lazyServiceProvider)
{
    public override async Task<Result> Handle(OrderItemRemoveCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.Query(x => x.Id == command.OrderId)
            .Include(x => x.Items)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null || order.Status != OrderStatus.Pending)
            return Result.Error(Localizer[OrderConsts.OrderCannotBeModified]);

        var orderItem = order.Items.FirstOrDefault(i => i.ProductId == command.ProductId);

        if (orderItem is null)
            return Result.NotFound(Localizer[OrderConsts.OrderItemNotFound]);

        order.RemoveItem(command.ProductId);

        orderRepository.Update(order);

        return Result.Success();
    }
}