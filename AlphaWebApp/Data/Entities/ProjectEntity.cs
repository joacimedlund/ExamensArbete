using AlphaWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaWebApp.Data.Entities
{
    public class ProjectEntity
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(200)]
        public string ProjectName { get; set; } = null!;

        [Required, MaxLength(200)]
        public string ClientName { get; set; } = null!;

        [MaxLength(4000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }


        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Planned;

        
        [Required]
        public string UserId { get; set; } = null!;            

        public AppUserEntity User { get; set; } = null!;
    }
}
