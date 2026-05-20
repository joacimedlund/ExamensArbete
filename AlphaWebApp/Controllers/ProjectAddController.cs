using AlphaWebApp.Data.Entities;
using AlphaWebApp.Models;
using AlphaWebApp.Models.ViewModels;
using AlphaWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace AlphaWebApp.Controllers;

[Authorize]
public class ProjectAddController(IProjectService svc, UserManager<AppUserEntity> um) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = um.GetUserId(User)!;   
        var items = await svc.GetAllForUserAsync(userId);

        var vms = items.Select(p => new ProjectCardViewModel
        {
            Id = p.Id,
            Name = p.ProjectName,
            Client = p.ClientName,
            Description = p.Description,
            Budget = p.Budget,
            ImageUrl = "~/Images/project-logotype.svg"
        });
        return View(vms);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddProjectForm form)
    {
        TempData["Ping"] = "POST Create kördes";

        if (!ModelState.IsValid) return RedirectToAction("Index", "Projects");

        var userId = um.GetUserId(User)!;
        await svc.CreateAsync(userId, form);
        return RedirectToAction("Index", "Projects");
    }
}



//public class ProjectAddController : Controller
//{
//    [HttpPost]

//    public async Task<IActionResult> Add(AddProjectForm form)
//    {
//        if (!ModelState.IsValid)
//        {
//            var errors = ModelState
//                .Where(x => x.Value?.Errors.Count > 0)
//                .ToDictionary(
//                    kvp => kvp.Key,
//                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
//                );

//            return BadRequest(new { errors });
//        }

//        var result = await _clientService.CreateAsync(form);
//        return result switch
//        {
//            200 => Ok(),
//            409 => Conflict(),
//            _ => Problem(),
//        };

//    }

//}

