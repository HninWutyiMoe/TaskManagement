using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MODEL.Eneities
{
    [Table("UploadedFile")]
    public class UploadedFile
    {
        [Key]
        public Guid FileId { get; set; }
        public Guid? TaskDetailId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
