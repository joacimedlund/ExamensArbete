using AlphaWebApp.Data.Enums;
using TaskStatus = AlphaWebApp.Data.Enums.TaskStatus;

namespace AlphaWebApp.Models.ViewModels;

public class TaskItemViewModel
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
}
