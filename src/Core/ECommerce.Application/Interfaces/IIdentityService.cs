using System.Security.Claims;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Interfaces;

public interface IIdentityService
{
    IQueryable<User> Users { get; }
    Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignOutAsync();
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid userId);
    Task<User?> GetUserByPrincipalAsync(ClaimsPrincipal principal);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<IdentityResult> UpdateAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    Task<IdentityResult> AddToRoleAsync(User user, string role);
    Task<IdentityResult> RemoveFromRoleAsync(User user, string role);
    Task<bool> CanSignInAsync(User user);

    // Role Management
    Task<IList<string>> GetRolesAsync();
    Task<IList<string>> GetUserRolesAsync(User user);
    Task<Role?> FindRoleByIdAsync(Guid roleId);
    Task<Role?> FindRoleByNameAsync(string roleName);
    Task<IdentityResult> CreateRoleAsync(Role role);
    Task<IdentityResult> UpdateRoleAsync(Role role);
    Task<IdentityResult> DeleteRoleAsync(Role role);
    Task<IList<Role>> GetAllRolesAsync();
    Task<bool> RoleExistsAsync(string roleName);
}