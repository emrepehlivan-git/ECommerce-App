using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Features.Users.DTOs;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Users.Queries;


public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;

internal sealed class GetUserByIdQueryHandler : BaseHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager, L l) : base(l)
    {
        _userManager = userManager;
    }

    public override async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(query.UserId.ToString());

        if (user is null)
            return Result.NotFound(_l["User.NotFound"]);

        return Result.Success(new UserDto(
            user.Id,
            user.Email!,
            user.FullName.ToString(),
            user.IsActive));
    }
}