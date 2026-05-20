using AlphaWebApp.Data.Entities;
using AlphaWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlphaWebApp.Services;

public interface IUserService
{
    Task<bool> CreateUserAsync(RegisterForm form);
    Task<bool> UserAlreadyExists(string email);
}

public class UserService(UserManager<AppUserEntity> userManager) : IUserService
{
    private readonly UserManager<AppUserEntity> _userManager = userManager;

    public async Task<bool> UserAlreadyExists(string email) =>
        await _userManager.Users.AnyAsync(x => x.Email == email);

    public async Task<bool> CreateUserAsync(RegisterForm form)
    {
        var user = new AppUserEntity
        {
            Email = form.Email,
            UserName = form.Email,
            FirstName = form.FirstName,
            LastName = form.LastName
        };
        var result = await _userManager.CreateAsync(user, form.Password);
        return result.Succeeded;
    }
}
