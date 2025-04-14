using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ECommerce.Infrastructure.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public IdentityService(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IQueryable<User> Users => _userManager.Users;

    public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return SignInResult.Failed;

        return await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public Task<User?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);

    public Task<User?> FindByIdAsync(string userId) => _userManager.FindByIdAsync(userId);

    public async Task<IdentityResult> CreateAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
            await AddToRoleAsync(user, "USER");
        return result;
    }

    public async Task<IdentityResult> UpdateAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        return result;
    }

    public Task<bool> CheckPasswordAsync(User user, string password) => _userManager.CheckPasswordAsync(user, password);

    public Task<string> GenerateEmailConfirmationTokenAsync(User user) => _userManager.GenerateEmailConfirmationTokenAsync(user);

    public Task<IdentityResult> ConfirmEmailAsync(User user, string token) => _userManager.ConfirmEmailAsync(user, token);

    public Task<string> GeneratePasswordResetTokenAsync(User user) => _userManager.GeneratePasswordResetTokenAsync(user);

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
    {
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (result.Succeeded)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        return result;
    }

    public Task<IList<string>> GetRolesAsync(User user) => _userManager.GetRolesAsync(user);

    public async Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
        var foundRole = await _roleManager.FindByNameAsync(role);
        if (foundRole is null)
            return IdentityResult.Failed();

        return await _userManager.AddToRoleAsync(user, foundRole.Name!);
    }

    public Task<IdentityResult> RemoveFromRoleAsync(User user, string role) => _userManager.RemoveFromRoleAsync(user, role);

    public Task<User?> GetUserByPrincipalAsync(ClaimsPrincipal principal)
    {
        return _userManager.GetUserAsync(principal);
    }

    public async Task<bool> CanSignInAsync(User user)
    {
        return await _signInManager.CanSignInAsync(user);
    }
}