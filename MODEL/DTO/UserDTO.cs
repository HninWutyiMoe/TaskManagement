namespace MODEL.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? RoleName { get; set; }
        public string? DepartmentName { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateUserDTO
    {
        public Guid? RoleId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

}
