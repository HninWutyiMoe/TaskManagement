using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MODEL.Eneities
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }
        public Guid DepartmentId { get; set; }
        public string? RoleName { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
