using AlphaWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using TaskStatus = AlphaWebApp.Data.Enums.TaskStatus;

namespace AlphaWebApp.Data.Entities;

public class TaskEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [Required]
    public Guid ProjectId { get; set; }

    public ProjectEntity Project { get; set; } = null!;
}
