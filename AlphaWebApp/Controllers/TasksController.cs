using AlphaWebApp.Data.Entities;
using AlphaWebApp.Models;
using AlphaWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaWebApp.Controllers;

[Authorize]
[Route("tasks")]
public class TasksController(ITaskService taskService, UserManager<AppUserEntity> userManager) : Controller
{
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddTaskForm form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Details", "Projects", new { id = form.ProjectId });

        var userId = userManager.GetUserId(User)!;
        var ok = await taskService.CreateAsync(userId, form);

        if (!ok)
            return NotFound();

        return RedirectToAction("Details", "Projects", new { id = form.ProjectId });
    }

    [HttpPost("edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTaskForm form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Details", "Projects", new { id = form.ProjectId });

        var userId = userManager.GetUserId(User)!;
        var projectId = await taskService.UpdateAsync(form, userId);

        if (projectId == null)
            return NotFound();

        return RedirectToAction("Details", "Projects", new { id = projectId.Value });
    }

    [HttpPost("complete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(Guid id)
    {
        var userId = userManager.GetUserId(User)!;
        var projectId = await taskService.MarkDoneAsync(id, userId);

        if (projectId == null)
            return NotFound();

        return RedirectToAction("Details", "Projects", new { id = projectId.Value });
    }

    [HttpPost("delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = userManager.GetUserId(User)!;
        var projectId = await taskService.DeleteAsync(id, userId);

        if (projectId == null)
            return NotFound();

        return RedirectToAction("Details", "Projects", new { id = projectId.Value });
    }
}
