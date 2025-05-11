using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.WebAPI.Extensions;

public static class AuthorizationExtensions
{
    public static AuthorizationPolicyBuilder RequirePermission(
        this AuthorizationPolicyBuilder builder,
        string permission)
    {
        return builder.AddRequirements(new PermissionRequirement(permission));
    }
}

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;

    public PermissionAuthorizationHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return;
        }

        var userId = Guid.Parse(user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
        var hasPermission = await _permissionService.HasPermissionAsync(userId, requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}