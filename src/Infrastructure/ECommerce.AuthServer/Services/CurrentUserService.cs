using System.Security.Claims;
using ECommerce.Application.Common.Interfaces;
using ECommerce.SharedKernel;

namespace ECommerce.AuthServer.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService, IScopedDependency
{
    public string? UserId
        => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}