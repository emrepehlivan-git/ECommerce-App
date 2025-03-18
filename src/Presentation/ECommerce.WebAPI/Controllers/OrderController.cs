using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Orders.Commands;
using ECommerce.Application.Features.Orders.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public sealed class OrderController() : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] PageableRequestParams requestParams,
        [FromQuery] OrderStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new OrderGetAllQuery(requestParams, status), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new OrderGetByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetOrdersByUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new OrderGetByUserQuery(userId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(OrderPlaceCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{orderId:guid}/items")]
    public async Task<IActionResult> AddOrderItem(
        Guid orderId,
        [FromBody] OrderItemAddCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command with { OrderId = orderId }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{orderId:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveOrderItem(
        Guid orderId,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new OrderItemRemoveCommand(orderId, itemId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("cancel/{orderId:guid}")]
    public async Task<IActionResult> CancelOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new OrderCancelCommand(orderId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("status/{orderId:guid}")]
    public async Task<IActionResult> UpdateOrderStatus(
        Guid orderId,
        [FromBody] OrderStatusUpdateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command with { OrderId = orderId }, cancellationToken);
        return Ok(result);
    }
}
