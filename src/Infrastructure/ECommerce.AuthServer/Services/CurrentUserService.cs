using System.Security.Claims;
using ECommerce.Application.Interfaces;

namespace ECommerce.AuthServer.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId
        => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}