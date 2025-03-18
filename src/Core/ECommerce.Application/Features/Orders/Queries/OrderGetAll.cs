using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Enums;
using ECommerce.SharedKernel;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Orders.Queries;

public sealed record OrderGetAllQuery(
    PageableRequestParams PageableRequestParams,
    OrderStatus? Status = null) : IRequest<PagedResult<List<OrderDto>>>;

public sealed class OrderGetAllQueryHandler(
    IOrderRepository orderRepository,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<OrderGetAllQuery, PagedResult<List<OrderDto>>>(lazyServiceProvider)
{
    public override async Task<PagedResult<List<OrderDto>>> Handle(OrderGetAllQuery query, CancellationToken cancellationToken)
    {
        return await orderRepository.Query(
            predicate: query.Status.HasValue ? x => x.Status == query.Status.Value : null,
            orderBy: q => q.OrderByDescending(x => x.OrderDate),
            include: q => q.Include(x => x.Items).ThenInclude(x => x.Product)
        )
        .ProjectToType<OrderDto>()
        .ApplyPagingAsync(query.PageableRequestParams, cancellationToken);
    }
}