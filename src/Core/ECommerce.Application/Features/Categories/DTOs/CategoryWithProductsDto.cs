using ECommerce.Application.Features.Products.DTOs;

namespace ECommerce.Application.Features.Categories.DTOs;

public sealed record CategoryWithProductsDto(
    Guid Id,
    string Name,
    List<ProductDto> Products);