using Ardalis.Result;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Users.Commands;

public record ActivateUserCommand(Guid UserId) : ICommand<bool>;

internal sealed class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public ActivateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(ActivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Result.NotFound("User not found");

        if (user.IsActive)
            return Result.Success(true);

        user.Activate();
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success(true)
            : Result.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}