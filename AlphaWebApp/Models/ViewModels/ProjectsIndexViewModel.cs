namespace AlphaWebApp.Models.ViewModels
{
    public class ProjectsIndexViewModel
    {
        public required List<ProjectCardViewModel> Cards { get; set; }
        public required int AllCount { get; set; }
        public required int StartedCount { get; set; }
        public required int CompletedCount { get; set; }
        public required string ActiveTab { get; set; } 
    }
}
