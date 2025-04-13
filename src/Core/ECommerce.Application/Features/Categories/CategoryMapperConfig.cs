using ECommerce.Application.Features.Categories.DTOs;
using ECommerce.Domain.Entities;
using Mapster;

namespace ECommerce.Application.Features.Categories;

public class CategoryMapperConfig
{
    public static void Configure(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>();
    }
}