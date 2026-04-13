using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MODEL.Eneities
{
    [Table("TaskDetail")]
    public class TaskDetail
    {
        [Key]
        public Guid TaskDetailId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? UserId { get; set; }
        public string? LineNumber { get; set; }
        public string? ItemTitle { get; set; }
        public string? ItemDescription { get; set; }
        public bool? IsCompleted { get; set; } = false;
        public string? Remark { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
