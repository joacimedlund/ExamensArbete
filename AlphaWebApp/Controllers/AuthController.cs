using ProjectFlowWebApp.Data.Entities;
using ProjectFlowWebApp.Models;
using ProjectFlowWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ProjectFlowWebApp.Controllers;

public class AuthController(IUserService userService, SignInManager<AppUserEntity> signInManager) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly SignInManager<AppUserEntity> _signInManager = signInManager;



    public IActionResult Register()
    {
        ViewBag.ErrorMessage = "";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterForm form)
    {
        ViewBag.ErrorMessage = "";

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        if (await _userService.UserAlreadyExists( form.Email))
        {
            ViewBag.ErrorMessage = "User already exists";
            return View(form);
        }

        var result = await _userService.CreateUserAsync(form);
        if(result)
            return RedirectToAction("Login");


        ViewBag.ErrorMessage = "Unable to register user account right now.";
        return View(form);
    }

    public IActionResult Login()
    {
        ViewBag.ErrorMessage = "";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginForm form)
    {
        ViewBag.ErrorMessage = "";

        if (!ModelState.IsValid) 
        {
            ViewBag.ErrorMessage = "Invalid email or password";
            return View(form);
        }

        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.IsPersistent, false);
        if(result.Succeeded)
            return RedirectToAction("Index", "Dashboard");


        ViewBag.ErrorMessage = "Invalid email or password";
        return View(form);
    }


    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
