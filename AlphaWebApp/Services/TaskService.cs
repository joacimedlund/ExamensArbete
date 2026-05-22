using ProjectFlowWebApp.Data.Contexts;
using ProjectFlowWebApp.Data.Entities;
using ProjectFlowWebApp.Data.Enums;
using ProjectFlowWebApp.Models;
using Microsoft.EntityFrameworkCore;
using TaskStatus = ProjectFlowWebApp.Data.Enums.TaskStatus;

namespace ProjectFlowWebApp.Services;

public interface ITaskService
{
    Task<bool> CreateAsync(string userId, AddTaskForm form);
    Task<List<TaskEntity>> GetForProjectAsync(Guid projectId, string userId);
    Task<Guid?> UpdateAsync(EditTaskForm form, string userId);
    Task<Guid?> MarkDoneAsync(Guid id, string userId);
    Task<Guid?> DeleteAsync(Guid id, string userId);
}

public class TaskService(DataContext db) : ITaskService
{
    public async Task<bool> CreateAsync(string userId, AddTaskForm form)
    {
        var ownsProject = await db.Projects.AnyAsync(p => p.Id == form.ProjectId && p.UserId == userId);
        if (!ownsProject)
            return false;

        var entity = new TaskEntity
        {
            Title = form.Title,
            Description = form.Description,
            DueDate = form.DueDate,
            Status = form.Status,
            Priority = form.Priority,
            ProjectId = form.ProjectId
        };

        db.Tasks.Add(entity);
        await db.SaveChangesAsync();
        return true;
    }

    public Task<List<TaskEntity>> GetForProjectAsync(Guid projectId, string userId) =>
        db.Tasks
            .Where(t => t.ProjectId == projectId && t.Project.UserId == userId)
            .OrderBy(t => t.Status == TaskStatus.Done)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.Title)
            .ToListAsync();

    public async Task<Guid?> UpdateAsync(EditTaskForm form, string userId)
    {
        var entity = await GetOwnedTaskAsync(form.Id, userId);
        if (entity == null || entity.ProjectId != form.ProjectId)
            return null;

        entity.Title = form.Title;
        entity.Description = form.Description;
        entity.DueDate = form.DueDate;
        entity.Status = form.Status;
        entity.Priority = form.Priority;

        await db.SaveChangesAsync();
        return entity.ProjectId;
    }

    public async Task<Guid?> MarkDoneAsync(Guid id, string userId)
    {
        var entity = await GetOwnedTaskAsync(id, userId);
        if (entity == null)
            return null;

        entity.Status = TaskStatus.Done;
        await db.SaveChangesAsync();
        return entity.ProjectId;
    }

    public async Task<Guid?> DeleteAsync(Guid id, string userId)
    {
        var entity = await GetOwnedTaskAsync(id, userId);
        if (entity == null)
            return null;

        var projectId = entity.ProjectId;
        db.Tasks.Remove(entity);
        await db.SaveChangesAsync();
        return projectId;
    }

    private Task<TaskEntity?> GetOwnedTaskAsync(Guid id, string userId) =>
        db.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project.UserId == userId);
}
