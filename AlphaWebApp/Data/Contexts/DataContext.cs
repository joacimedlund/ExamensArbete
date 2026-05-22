using ProjectFlowWebApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectFlowWebApp.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<AppUserEntity, IdentityRole, string>(options)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<ProjectEntity>()
         .HasOne(p => p.User)
         .WithMany(u => u.Projects)
         .HasForeignKey(p => p.UserId)
         .OnDelete(DeleteBehavior.Cascade);

        b.Entity<TaskEntity>()
         .HasOne(t => t.Project)
         .WithMany(p => p.Tasks)
         .HasForeignKey(t => t.ProjectId)
         .OnDelete(DeleteBehavior.Cascade);

       
    }
}
