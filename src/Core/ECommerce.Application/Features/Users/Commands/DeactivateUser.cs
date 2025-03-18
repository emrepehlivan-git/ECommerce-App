using Ardalis.Result;
using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Features.Users.Commands;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<Result>, ITransactionalRequest;

public sealed class DeactivateUserCommandHandler(
    IIdentityService identityService,
    ILazyServiceProvider lazyServiceProvider) : BaseHandler<DeactivateUserCommand, Result>(lazyServiceProvider)
{
    public override async Task<Result> Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await identityService.FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Result.NotFound(Localizer[UserConsts.NotFound]);

        if (!user.IsActive)
            return Result.Success();

        user.Deactivate();
        var result = await identityService.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}