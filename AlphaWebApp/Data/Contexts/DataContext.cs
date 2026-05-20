using AlphaWebApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlphaWebApp.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<AppUserEntity, IdentityRole, string>(options)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<ProjectEntity>()
         .HasOne(p => p.User)
         .WithMany(u => u.Projects)
         .HasForeignKey(p => p.UserId)
         .OnDelete(DeleteBehavior.Cascade);

       
    }
}
