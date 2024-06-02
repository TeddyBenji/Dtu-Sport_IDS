using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Extensions;

namespace Dtu_Sport_IDS.Services;
public interface IUserService
    {
       Task<ServiceResponse<string>> RegisterUserAsync(UserRegistrationModel model); 
       Task<ServiceResponse<string>> DeleteUserAsync(string username);
       Task<ServiceResponse<string>> AssignRoleAsync(AssignRoleModel model);
    }

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ServiceResponse<string>> RegisterUserAsync(UserRegistrationModel model)
    {
        var response = new ServiceResponse<string>();

        var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
        if (existingUserByUsername != null)
        {
            response.Success = false;
            response.Message = "Username is already taken.";
            return response;
        }

        var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
        if (existingUserByEmail != null)
        {
            response.Success = false;
            response.Message = "Email is already in use.";
            return response;
        }

        var user = new IdentityUser { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            response.Message = "User registered successfully";
        }
        else
        {
            response.Success = false;
            response.Message = string.Join("; ", result.Errors.Select(e => e.Description));
        }

        return response;
    }

    public async Task<ServiceResponse<string>> DeleteUserAsync(string username)
        {
            var response = new ServiceResponse<string>();

            
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                response.Message = "User deleted successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "User deletion failed: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return response;
        }

    public async Task<ServiceResponse<string>> AssignRoleAsync(AssignRoleModel model)
    {
        var response = new ServiceResponse<string>();

        var user = await _userManager.FindByNameAsync(model.username);
        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found.";
            return response;
        }

        var roleExists = await _roleManager.RoleExistsAsync(model.Role);
        if (!roleExists)
        {
            response.Success = false;
            response.Message = "Role does not exist.";
            return response;
        }

        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (result.Succeeded)
        {
            response.Message = $"User {model.username} assigned to role {model.Role}.";
        }
        else
        {
            response.Success = false;
            response.Message = "Role assignment failed: " + string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return response;
    }
}
