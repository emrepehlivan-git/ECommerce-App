using Ardalis.Result;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Users.Commands;

public sealed record DeactivateUserCommand(Guid UserId) : ICommand<bool>;

internal sealed class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public DeactivateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Result.NotFound("User not found");

        if (!user.IsActive)
            return Result.Success(true);

        user.Deactivate();
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success(true)
            : Result.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}