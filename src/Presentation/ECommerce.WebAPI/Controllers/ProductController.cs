using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Products.Commands;
using ECommerce.Application.Features.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public sealed class ProductController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] PageableRequestParams requestParams, CancellationToken cancellationToken)
    {
        var products = await Mediator.Send(new GetAllProductsQuery(requestParams), cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var product = await Mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await Mediator.Send(command, cancellationToken);
        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await Mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await Mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return Ok(product);
    }

    [HttpGet("{id}/stock")]
    public async Task<IActionResult> GetProductStockInfo(Guid id, CancellationToken cancellationToken)
    {
        var stockInfo = await Mediator.Send(new GetProductStockInfo(id), cancellationToken);
        return Ok(stockInfo);
    }

    [HttpPut("{id}/stock")]
    public async Task<IActionResult> UpdateProductStock(Guid id, UpdateProductStock command, CancellationToken cancellationToken)
    {
        var product = await Mediator.Send(command with { ProductId = id }, cancellationToken);
        return Ok(product);
    }
}
