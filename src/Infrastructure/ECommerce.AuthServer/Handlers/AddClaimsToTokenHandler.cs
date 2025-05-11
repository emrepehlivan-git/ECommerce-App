using System.Security.Claims;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.AuthServer.Controllers;
using ECommerce.AuthServer.Helpers;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

public class AddClaimsToTokenHandler(IIdentityService identityService, IPermissionService permissionService, IOpenIddictScopeManager scopeManager)
: IOpenIddictServerHandler<ProcessSignInContext>
{
    public async ValueTask HandleAsync(ProcessSignInContext context)
    {
        var principal = context.Principal ?? throw new InvalidOperationException("The principal is null.");

        if (principal is { Identity: { IsAuthenticated: false } })
            return;

        var userId = principal.GetClaim(Claims.Subject);
        if (string.IsNullOrWhiteSpace(userId)) return;

        var user = await identityService.FindByIdAsync(Guid.Parse(userId));
        if (user is null) return;

        var identity = principal?.Identity as ClaimsIdentity ?? throw new InvalidOperationException("The identity is null.");

        identity.SetClaim(Claims.Subject, user.Id.ToString());
        identity.SetClaim(Claims.Email, user.Email);
        identity.SetClaim("fullName", user.FullName.ToString());
        identity.SetClaims(Claims.Role, [.. await identityService.GetUserRolesAsync(user)]);
        identity.SetClaims("permissions", [.. await permissionService.GetUserPermissionsAsync(user.Id)]);
        identity.SetScopes(context.Request.GetScopes());
        identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
        identity.SetDestinations(AuthorizationController.GetDestinations);
    }
}
