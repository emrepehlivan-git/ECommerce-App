using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public IdentityService(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return SignInResult.Failed;

        return await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }

    public Task SignOutAsync() => _signInManager.SignOutAsync();

    public Task<User?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);

    public Task<User?> FindByIdAsync(string userId) => _userManager.FindByIdAsync(userId);

    public Task<IdentityResult> CreateAsync(User user, string password) => _userManager.CreateAsync(user, password);

    public Task<IdentityResult> UpdateAsync(User user) => _userManager.UpdateAsync(user);

    public Task<IdentityResult> DeleteAsync(User user) => _userManager.DeleteAsync(user);

    public Task<bool> CheckPasswordAsync(User user, string password) => _userManager.CheckPasswordAsync(user, password);

    public Task<string> GenerateEmailConfirmationTokenAsync(User user) => _userManager.GenerateEmailConfirmationTokenAsync(user);

    public Task<IdentityResult> ConfirmEmailAsync(User user, string token) => _userManager.ConfirmEmailAsync(user, token);

    public Task<string> GeneratePasswordResetTokenAsync(User user) => _userManager.GeneratePasswordResetTokenAsync(user);

    public Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword) => _userManager.ResetPasswordAsync(user, token, newPassword);

    public Task<IList<string>> GetRolesAsync(User user) => _userManager.GetRolesAsync(user);

    public Task<IdentityResult> AddToRoleAsync(User user, string role) => _userManager.AddToRoleAsync(user, role);

    public Task<IdentityResult> RemoveFromRoleAsync(User user, string role) => _userManager.RemoveFromRoleAsync(user, role);
}