using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTO
{
    public class TaskDetailDTO
    {
        public Guid TaskDetailId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? UserId { get; set; }
        public string? LineNumber { get; set; }
        public string? ItemTitle { get; set; }
        public string? ItemDescription { get; set; }
        public bool? IsCompleted { get; set; } = false;
        public string? Remark { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 
    }

    public class CreateTaskDetailDTO
    {
        public Guid? TaskId { get; set; }
        public Guid? UserId { get; set; }
        public string? LineNumber { get; set; }
        public string? ItemTitle { get; set; }
        public string? ItemDescription { get; set; }
        public bool? IsCompleted { get; set; } = false;
        public string? Remark { get; set; } = string.Empty;
    }
}
