using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Application.Repositories;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Orders.Queries;

public sealed record OrderGetByUserQuery(Guid UserId) : IRequest<Result<List<OrderDto>>>;

public sealed class OrderGetByUserQueryHandler(
    IOrderRepository orderRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<OrderGetByUserQuery, Result<List<OrderDto>>>(lazyServiceProvider)
{
    public override async Task<Result<List<OrderDto>>> Handle(OrderGetByUserQuery query, CancellationToken cancellationToken)
    {
        return await orderRepository.Query(x => x.UserId == query.UserId)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ProjectToType<OrderDto>()
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync(cancellationToken);
    }
}