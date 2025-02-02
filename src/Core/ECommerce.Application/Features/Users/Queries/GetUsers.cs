using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Features.Users.DTOs;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Users.Queries;

public sealed record GetUsersQuery : IRequest<List<UserDto>>;

internal sealed class GetUsersQueryHandler : BaseHandler<GetUsersQuery, List<UserDto>>
{
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(UserManager<User> userManager, L l) : base(l)
    {
        _userManager = userManager;
    }

    public override async Task<List<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .Select(u => new UserDto(
                u.Id,
                u.Email!,
                u.FullName.ToString(),
                u.IsActive))
            .ToListAsync(cancellationToken);

        return users;
    }
}
