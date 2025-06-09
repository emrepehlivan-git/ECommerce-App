using ECommerce.Application.Constants;
using ECommerce.Application.Features.Categories.Commands;
using ECommerce.Application.Features.Categories.Queries;
using ECommerce.Application.Parameters;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public sealed class CategoryController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] PageableRequestParams requestParams, [FromQuery] string? orderBy = null, CancellationToken cancellationToken = default)
    {
        var categories = await Mediator.Send(new GetAllCategoriesQuery(requestParams, orderBy), cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(category);
    }

    [Authorize(PermissionConstants.Categories.Create)]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(command, cancellationToken);
        return Ok(category);
    }

    [Authorize(PermissionConstants.Categories.Update)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(category);
    }

    [Authorize(PermissionConstants.Categories.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return Ok(category);
    }
}