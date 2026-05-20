using AlphaWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaWebApp.Models;

public class EditProjectForm
{
    [Required]
    public Guid Id { get; set; }

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    public string ProjectName { get; set; } = null!;

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client Name", Prompt = "Enter client name")]
    public string ClientName { get; set; } = null!;

    [DataType(DataType.Text)]
    [StringLength(4000)]
    [Display(Name = "Description", Prompt = "Type something")]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Status")]
    public ProjectStatus Status { get; set; } = ProjectStatus.Planned;

    [Range(0, 1_000_000_000)]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Budget")]
    public decimal? Budget { get; set; }
}
