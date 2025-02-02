using Ardalis.Result;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Users.DTOs;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Users.Queries;

public sealed record GetUsersQuery : IQuery<List<UserDto>>;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserDto>>
{
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .Select(u => new UserDto(
                u.Id,
                u.Email!,
                u.FullName.ToString(),
                u.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(users);
    }
}