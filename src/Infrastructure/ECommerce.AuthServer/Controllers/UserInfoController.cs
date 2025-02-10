using ECommerce.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ECommerce.AuthServer.Controllers;

public sealed class UserInfoController : Controller
{
    private readonly IIdentityService _identityService;
    public UserInfoController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
    public async Task<IActionResult> UserInfo()
    {

        var user = await _identityService.FindByIdAsync(User.GetClaim(Claims.Subject));
        if (user is null)

        {
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The specified access token is bound to an account that no longer exists."
                }!));
        }

        var claims = new Dictionary<string, object>
        {
            [Claims.Subject] = user.Id,
            [Claims.Name] = user.UserName!,
            [Claims.Email] = user.Email!,
            [Claims.Role] = await _identityService.GetRolesAsync(user),
            [Claims.Scope] = "api",
            [Claims.Audience] = "api",
            ["fullName"] = user.FullName.ToString(),
        };

        return Ok(claims);
    }

}
