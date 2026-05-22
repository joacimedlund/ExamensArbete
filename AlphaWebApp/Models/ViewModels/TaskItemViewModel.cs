using ProjectFlowWebApp.Data.Enums;
using TaskStatus = ProjectFlowWebApp.Data.Enums.TaskStatus;

namespace ProjectFlowWebApp.Models.ViewModels;

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
