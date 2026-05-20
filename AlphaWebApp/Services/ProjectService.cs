using AlphaWebApp.Data.Contexts;
using AlphaWebApp.Data.Entities;
using AlphaWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AlphaWebApp.Services;


public interface IProjectService
{
    Task<ProjectEntity> CreateAsync(string userId, AddProjectForm form);
    Task<List<ProjectEntity>> GetAllForUserAsync(string userId);
    Task<bool> DeleteAsync(Guid id, string userId);
    Task<ProjectEntity?> GetByIdForUserAsync(Guid id, string userId);
    Task<bool> UpdateAsync(EditProjectForm form, string userId);
}

public class ProjectService(DataContext db) : IProjectService
{
    public async Task<ProjectEntity> CreateAsync(string userId, AddProjectForm form)
    {
        var e = new ProjectEntity
        {
            ProjectName = form.ProjectName,
            ClientName = form.ClientName,
            Description = form.Description,
            StartDate = form.StartDate,   
            EndDate = form.EndDate,
            Status = form.Status,
            Budget = form.Budget,
            UserId = userId
        };
        db.Projects.Add(e);
        await db.SaveChangesAsync();
        return e;
    }

    public async Task<bool> UpdateAsync(EditProjectForm form, string userId)
    {
        var entity = await db.Projects.FirstOrDefaultAsync(p => p.Id == form.Id && p.UserId == userId);
        if (entity == null)
            return false;

        entity.ProjectName = form.ProjectName;
        entity.ClientName = form.ClientName;
        entity.Description = form.Description;
        entity.StartDate = form.StartDate; 
        entity.EndDate = form.EndDate;
        entity.Status = form.Status;
        entity.Budget = form.Budget;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var entity = await db.Projects
            .Where(p => p.Id == id && p.UserId == userId)
            .FirstOrDefaultAsync();

        if (entity == null)
            return false;

        db.Projects.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

    public Task<ProjectEntity?> GetByIdForUserAsync(Guid id, string userId) =>
        db.Projects.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
    public Task<List<ProjectEntity>> GetAllForUserAsync(string userId) =>
        db.Projects
          .Where(p => p.UserId == userId)
          .OrderByDescending(p => p.Id)
          .ToListAsync();
}



//public class ProjectService
//{

//    public async Task<int> CreateAsync(AddProjectForm form)
//    {
//        try
//        {
//            if (await _clientRepository.ExistsAsync(x => x.ProjectName == form.ProjectName))
//                return 409;

//            return 200;
//        }
//        catch
//        {
//            return 500;
//        }
//    }

//}
