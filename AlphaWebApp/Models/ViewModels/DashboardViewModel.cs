namespace ProjectFlowWebApp.Models.ViewModels;

public class DashboardViewModel
{
    public int TotalProjects { get; set; }
    public int StartedProjects { get; set; }
    public int CompletedProjects { get; set; }
    public int UpcomingDeadlines { get; set; }
    public List<ProjectCardViewModel> UpcomingProjects { get; set; } = [];
}
