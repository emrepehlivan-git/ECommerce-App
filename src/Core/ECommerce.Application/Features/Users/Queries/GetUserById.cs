using Ardalis.Result;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Users.DTOs;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Users.Queries;


public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(query.UserId.ToString());

        if (user is null)
            return Result.NotFound("User not found");

        return Result.Success(new UserDto(
            user.Id,
            user.Email!,
            user.FullName.ToString(),
            user.IsActive));
    }
}