using MODEL.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MODEL.Eneities
{
    [Table("TaskHeader")]
    public class TaskHeader
    {
        [Key]
        public Guid TaskId { get; set; }
        public Guid? AssignToDepartmentId { get; set; }
        public string? TaskCode { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
