using AlphaWebApp.Data.Entities;
using AlphaWebApp.Data.Enums;
using AlphaWebApp.Models;
using AlphaWebApp.Models.ViewModels;
using AlphaWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaWebApp.Controllers;


[Authorize]
[Route("projects")]
public class ProjectsController(IProjectService svc, UserManager<AppUserEntity> um) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index(string? tab = "all")
    {
        var userId = um.GetUserId(User)!;
        var all = await svc.GetAllForUserAsync(userId);

        var started = all.Where(p => p.Status == ProjectStatus.Started).ToList();
        var completed = all.Where(p => p.Status == ProjectStatus.Completed).ToList();

        ViewBag.AllCount = all.Count;
        ViewBag.StartedCount = started.Count;
        ViewBag.CompletedCount = completed.Count;
        ViewBag.ActiveTab = (tab is "started" or "completed") ? tab : "all";

        var filtered = tab switch
        {
            "started" => started,
            "completed" => completed,
            _ => all
        };

        var vms = filtered
            .OrderByDescending(p => p.StartDate ?? DateTime.MinValue)
            .ThenByDescending(p => p.Id)
            .Select(p => new ProjectCardViewModel
            {
                Id = p.Id,
                Name = p.ProjectName,
                Client = p.ClientName,
                Description = p.Description,
                Budget = p.Budget,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status, 
                ImageUrl = "~/Images/project-logotype.svg"
            })
            .ToList();

        return View(vms);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var userId = um.GetUserId(User)!;
        var project = await svc.GetByIdForUserAsync(id, userId);

        if (project == null)
            return NotFound();

        var vm = new ProjectDetailsViewModel
        {
            Id = project.Id,
            Name = project.ProjectName,
            Client = project.ClientName,
            Description = project.Description,
            Budget = project.Budget,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status
        };

        return View(vm);
    }
    //[HttpGet]
    //public async Task<IActionResult> Index()
    //{
    //    var userId = um.GetUserId(User)!;
    //    var items = await svc.GetAllForUserAsync(userId);

    //    var vms = items.Select(p => new ProjectCardViewModel
    //    {
    //        Id = p.Id,
    //        Name = p.ProjectName,
    //        Client = p.ClientName,
    //        Description = p.Description,
    //        Budget = p.Budget,
    //        ImageUrl = "~/Images/project-logotype.svg"
    //    }).ToList();

    //    return View(vms);
    //}



    [HttpPost("edit-ajax")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var userIdInvalid = um.GetUserId(User)!;
            var itemsInvalid = await svc.GetAllForUserAsync(userIdInvalid);
            var vmsInvalid = itemsInvalid.Select(p => new ProjectCardViewModel
            {
                Id = p.Id,
                Name = p.ProjectName,
                Client = p.ClientName,
                Description = p.Description,
                Budget = p.Budget,
                ImageUrl = "~/Images/project-logotype.svg"
            }).ToList();

            ViewBag.EditErrorProjectId = form.Id;
            return View("Index", vmsInvalid);
        }

        var userId = um.GetUserId(User)!;
        var ok = await svc.UpdateAsync(form, userId);

        TempData["Ping"] = ok ? "Project updated." : "Not found or not yours.";
        return RedirectToAction("Index", "Projects");
    }



    [HttpPost ("delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = um.GetUserId(User)!;

        var ok = await svc.DeleteAsync(id, userId);
        if (!ok)
        {
            TempData["Ping"] = "Projekt hittades inte eller tillhör inte dig.";
            return RedirectToAction("Index", "Projects");
        }

        TempData["Ping"] = "Projekt raderat.";
        return RedirectToAction("Index", "Projects");
    }
}


