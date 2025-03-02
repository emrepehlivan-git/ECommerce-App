using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Domain.Entities;
using Mapster;

namespace ECommerce.Application.Features.Orders;

public static class OrderMapperConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Order, OrderDto>.NewConfig()
            .Map(dest => dest.Items, src => src.Items);

        TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig()
            .Map(dest => dest.ProductName, src => src.Product.Name);
    }
}