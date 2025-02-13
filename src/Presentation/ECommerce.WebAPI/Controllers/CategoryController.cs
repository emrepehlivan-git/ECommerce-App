using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Categories.Commands;
using ECommerce.Application.Features.Categories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public sealed class CategoryController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] PageableRequestParams requestParams, CancellationToken cancellationToken)
    {
        var categories = await Mediator.Send(new GetAllCategoriesQuery(requestParams), cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(command, cancellationToken);
        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return Ok(category);
    }
}
