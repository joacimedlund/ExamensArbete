using AlphaWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using TaskStatus = AlphaWebApp.Data.Enums.TaskStatus;

namespace AlphaWebApp.Models;

public class EditTaskForm
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [Required(ErrorMessage = "Required")]
    [StringLength(200)]
    [Display(Name = "Title", Prompt = "Enter task title")]
    public string Title { get; set; } = null!;

    [StringLength(2000)]
    [Display(Name = "Description", Prompt = "Add a short description")]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Due Date")]
    public DateTime? DueDate { get; set; }

    [Display(Name = "Status")]
    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    [Display(Name = "Priority")]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
}
