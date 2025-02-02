using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Users.Commands;

public record ActivateUserCommand(Guid UserId) : IRequest<Result>;

internal sealed class ActivateUserCommandHandler : BaseHandler<ActivateUserCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public ActivateUserCommandHandler(UserManager<User> userManager, L l) : base(l)
    {
        _userManager = userManager;
    }

    public override async Task<Result> Handle(ActivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Result.NotFound(_l["User.NotFound"]);

        if (user.IsActive)
            return Result.Success();

        user.Activate();
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}