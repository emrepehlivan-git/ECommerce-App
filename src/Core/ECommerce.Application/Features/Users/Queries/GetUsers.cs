using Ardalis.Result;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Extensions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Users.DTOs;
using ECommerce.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Users.Queries;

public sealed record GetUsersQuery(PageableRequestParams PageableRequestParams) : IRequest<PagedResult<ICollection<UserDto>>>;

internal sealed class GetUsersQueryHandler : BaseHandler<GetUsersQuery, PagedResult<ICollection<UserDto>>>
{
    private readonly IIdentityService _identityService;

    public GetUsersQueryHandler(IIdentityService identityService, ILazyServiceProvider lazyServiceProvider)
        : base(lazyServiceProvider)
    {
        _identityService = identityService;
    }

    public override async Task<PagedResult<ICollection<UserDto>>> Handle(GetUsersQuery query,
    CancellationToken cancellationToken)
    {
        return await _identityService.Users
            .AsNoTracking()
            .Select(u => new UserDto(
                u.Id,
                u.Email!,
                u.FullName.ToString(),
                u.IsActive))
            .ApplyPagingAsync(query.PageableRequestParams, cancellationToken);
    }
}
