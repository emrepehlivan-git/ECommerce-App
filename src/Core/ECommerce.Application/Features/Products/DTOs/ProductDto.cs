namespace ECommerce.Application.Features.Products.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    string CategoryName);