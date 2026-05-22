using ProjectFlowWebApp.Data.Entities;
using ProjectFlowWebApp.Data.Enums;
using ProjectFlowWebApp.Models.ViewModels;
using ProjectFlowWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProjectFlowWebApp.Controllers;

[Authorize]
public class DashboardController(IProjectService projectService, UserManager<AppUserEntity> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = userManager.GetUserId(User)!;
        var projects = await projectService.GetAllForUserAsync(userId);
        var today = DateTime.Today;
        var deadlineLimit = today.AddDays(14);

        var upcomingDeadlineProjects = projects
            .Where(p => p.EndDate.HasValue
                && p.EndDate.Value.Date >= today
                && p.EndDate.Value.Date <= deadlineLimit
                && p.Status != ProjectStatus.Completed)
            .ToList();

        var upcomingProjects = upcomingDeadlineProjects
            .OrderBy(p => p.EndDate)
            .Take(5)
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

        var vm = new DashboardViewModel
        {
            TotalProjects = projects.Count,
            StartedProjects = projects.Count(p => p.Status == ProjectStatus.Started),
            CompletedProjects = projects.Count(p => p.Status == ProjectStatus.Completed),
            UpcomingDeadlines = upcomingDeadlineProjects.Count,
            UpcomingProjects = upcomingProjects
        };

        return View(vm);
    }
}
