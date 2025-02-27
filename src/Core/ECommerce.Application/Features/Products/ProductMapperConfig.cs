using ECommerce.Application.Features.Products.DTOs;
using ECommerce.Domain.Entities;
using Mapster;

namespace ECommerce.Application.Features.Products;

public class ProductMapperConfig
{
    public static void Configure(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.CategoryName, src => src.Category.Name);
    }
}
