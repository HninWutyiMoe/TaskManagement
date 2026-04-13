using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MODEL.Eneities
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public Guid DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? CreatedBy { get; set; } = null;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
