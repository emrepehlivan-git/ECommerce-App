using System.Security.Claims;
using ECommerce.Application.Common.Interfaces;
using ECommerce.SharedKernel;

namespace ECommerce.WebAPI.Services;

public sealed class CurrentUserService : ICurrentUserService, IScopedDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId
        => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
