using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Users.Constants;
using ECommerce.Application.Features.Users.DTOs;
using ECommerce.SharedKernel;
using MediatR;

namespace ECommerce.Application.Features.Users.Queries;


public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;

internal sealed class GetUserByIdQueryHandler : BaseHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IIdentityService _identityService;

    public GetUserByIdQueryHandler(IIdentityService identityService, ILazyServiceProvider lazyServiceProvider)
        : base(lazyServiceProvider)
    {
        _identityService = identityService;
    }

    public override async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByIdAsync(query.UserId.ToString());

        if (user is null)
            return Result.NotFound(Localizer[UserConsts.NotFound]);

        return Result.Success(new UserDto(
            user.Id,
            user.Email!,
            user.FullName.ToString(),
            user.IsActive));
    }
}
