using AlphaWebApp.Data.Enums;

namespace AlphaWebApp.Models.ViewModels;

public class ProjectDetailsViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Client { get; set; } = "";
    public string? Description { get; set; }
    public decimal? Budget { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Planned;
}
