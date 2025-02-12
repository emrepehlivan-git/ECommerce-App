using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Users.Constants;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Features.Users.Commands;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<Result>;

internal sealed class DeactivateUserCommandHandler : BaseHandler<DeactivateUserCommand, Result>
{
    private readonly IIdentityService _identityService;

    public DeactivateUserCommandHandler(IIdentityService identityService, ILazyServiceProvider lazyServiceProvider)
        : base(lazyServiceProvider)
    {
        _identityService = identityService;
    }

    public override async Task<Result> Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Result.NotFound(Localizer[UserConsts.NotFound]);

        if (!user.IsActive)
            return Result.Success();

        user.Deactivate();
        var result = await _identityService.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}