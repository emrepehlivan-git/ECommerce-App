using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Domain.Entities;
using Mapster;

namespace ECommerce.Application.Features.Orders;

public static class OrderMapperConfig
{
    public static void Configure(TypeAdapterConfig config)
    {
        config.ForType<Order, OrderDto>()
            .Map(dest => dest.Items, src => src.Items);

        config.ForType<OrderItem, OrderItemDto>()
            .Map(dest => dest.ProductName, src => src.Product.Name);
    }
}