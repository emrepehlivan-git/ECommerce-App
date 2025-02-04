using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Features.Users.Commands;

namespace ECommerce.WebAPI.Controllers;

public sealed class UsersController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] PageableRequestParams pageableRequestParams, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUsersQuery(pageableRequestParams), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("activate")]
    public async Task<IActionResult> ActivateUser(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ActivateUserCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeactivateUserCommand(id), cancellationToken);
        return Ok(result);
    }
}
