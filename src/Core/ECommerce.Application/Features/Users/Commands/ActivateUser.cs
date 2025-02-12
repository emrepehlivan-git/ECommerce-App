using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Users.Constants;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Features.Users.Commands;

public record ActivateUserCommand(Guid UserId) : IRequest<Result>;

internal sealed class ActivateUserCommandHandler : BaseHandler<ActivateUserCommand, Result>
{
    private readonly IIdentityService _identityService;

    public ActivateUserCommandHandler(IIdentityService identityService, ILazyServiceProvider lazyServiceProvider)
        : base(lazyServiceProvider)
    {
        _identityService = identityService;
    }

    public override async Task<Result> Handle(ActivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Result.NotFound(Localizer[UserConsts.NotFound]);

        if (user.IsActive)
            return Result.Success();

        user.Activate();
        var result = await _identityService.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}