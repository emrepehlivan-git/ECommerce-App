using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Users.Commands;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<Result>;

internal sealed class DeactivateUserCommandHandler : BaseHandler<DeactivateUserCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public DeactivateUserCommandHandler(UserManager<User> userManager, L l) : base(l)
    {
        _userManager = userManager;
    }

    public override async Task<Result> Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Result.NotFound(_l["User.NotFound"]);

        if (!user.IsActive)
            return Result.Success();

        user.Deactivate();
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}