using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ECommerce.Infrastructure.Services;

public sealed class IdentityService(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    RoleManager<Role> roleManager) : IIdentityService
{
    public IQueryable<User> Users => userManager.Users;

    public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return SignInResult.Failed;

        return await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }

    public async Task SignOutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public Task<User?> FindByEmailAsync(string email) => userManager.FindByEmailAsync(email);

    public Task<User?> FindByIdAsync(Guid userId) => userManager.FindByIdAsync(userId.ToString());

    public async Task<IdentityResult> CreateAsync(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
            await AddToRoleAsync(user, "USER");
        return result;
    }

    public async Task<IdentityResult> UpdateAsync(User user)
    {
        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            await userManager.UpdateSecurityStampAsync(user);
            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, isPersistent: false);
        }
        return result;
    }

    public Task<bool> CheckPasswordAsync(User user, string password) => userManager.CheckPasswordAsync(user, password);

    public Task<string> GenerateEmailConfirmationTokenAsync(User user) => userManager.GenerateEmailConfirmationTokenAsync(user);

    public Task<IdentityResult> ConfirmEmailAsync(User user, string token) => userManager.ConfirmEmailAsync(user, token);

    public Task<string> GeneratePasswordResetTokenAsync(User user) => userManager.GeneratePasswordResetTokenAsync(user);

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
    {
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        if (result.Succeeded)
        {
            await userManager.UpdateSecurityStampAsync(user);
            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, isPersistent: false);
        }
        return result;
    }

    public Task<IList<string>> GetRolesAsync(User user) => userManager.GetRolesAsync(user);

    public async Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
        var foundRole = await roleManager.FindByNameAsync(role);
        if (foundRole is null)
            return IdentityResult.Failed();

        return await userManager.AddToRoleAsync(user, foundRole.Name!);
    }

    public Task<IdentityResult> RemoveFromRoleAsync(User user, string role) => userManager.RemoveFromRoleAsync(user, role);

    public Task<User?> GetUserByPrincipalAsync(ClaimsPrincipal principal)
    {
        return userManager.GetUserAsync(principal);
    }

    public async Task<bool> CanSignInAsync(User user)
    {
        return await signInManager.CanSignInAsync(user);
    }
}