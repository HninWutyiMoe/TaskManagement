namespace MODEL.DTO
{
    public class RoleDTO
    {
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 
    }

    public class CreateRoleDTO
    {
        public Guid? DepartmentId { get; set; }
        public string? RoleName { get; set; }
    }


}
