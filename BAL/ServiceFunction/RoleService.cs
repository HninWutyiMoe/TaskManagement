using BAL.IServiceFunction;
using MODEL.DTO;
using MODEL.Eneities;
using REPOSITORY.UnitOfWork;

namespace BAL.ServiceFunction
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRoles()
        {
            var roles = await _unitOfWork.Role.GetAll();
            var departments = await _unitOfWork.Department.GetAll();

            if (roles == null || departments == null)
            {
                return new List<RoleDTO>();
            }

            var result = from role in roles
                         join department in departments
                         on role.DepartmentId equals department.DepartmentId into roleDept
                         from department in roleDept.DefaultIfEmpty()
                         select new RoleDTO
                         {
                             RoleId = role.RoleId,
                             RoleName = role.RoleName,
                             DepartmentName = department?.Name ?? "Unknown Department",
                             CreatedAt = role.CreatedAt,
                             UpdatedAt = role.UpdatedAt
                         };

            return result.ToList();
        }

        public async Task<RoleDTO?> GetRoleById(Guid roleId)
        {
            var roles = await _unitOfWork.Role.GetByCondition(r => r.RoleId == roleId);          
            var role = roles.FirstOrDefault();
            if (role == null)
            {
                return null;
            }

            var departments = await _unitOfWork.Department.GetByCondition(d => d.DepartmentId == role.DepartmentId);
            var department = departments.FirstOrDefault();

            return new RoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                DepartmentName = department?.Name,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt
            };
        }

        public async Task<(bool success, string message, Guid? roleId)> CreateRole(CreateRoleDTO role)
        {
            try
            {
                var existingRoles = await _unitOfWork.Role.GetAll();

                if (existingRoles.Any(x => x.RoleName == role.RoleName))
                {
                    return (false, "Role name already exists", null);
                }

                if (role.DepartmentId == null)
                {
                    return (false, "Department ID is required", null);
                }

                var departments = await _unitOfWork.Department.GetAll();

                var departmentExists = departments.Any(d => d.DepartmentId == role.DepartmentId);

                if (!departmentExists)
                {
                    return (false, $"Department with ID {role.DepartmentId} does not exist", null);
                }

                var newRole = new Role
                {
                    RoleId = Guid.NewGuid(),
                    DepartmentId = role.DepartmentId.Value,
                    RoleName = role.RoleName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Role.Add(newRole);
                await _unitOfWork.SaveChangesAsync();

                return (true, "Role created successfully", newRole.RoleId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating role: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> UpdateRole(Guid roleId,CreateRoleDTO req)
        {
            try
            {
                var existingRoles = await _unitOfWork.Role.GetByCondition(r => r.RoleId == roleId);
                var role = existingRoles?.FirstOrDefault();
                if (role == null)
                {
                    return (false, "Role not found");
                }

                var department = await _unitOfWork.Department.GetByCondition(d => d.DepartmentId == req.DepartmentId);
                if(department == null)
                {
                    return (false, "Department not found");
                }

                role.RoleName = req.RoleName ?? role.RoleName;

                role.DepartmentId = req.DepartmentId ?? role.DepartmentId;
                
                role.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Role.Update(role);
                await _unitOfWork.SaveChangesAsync();

                return (true, "Role updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating role: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteRole(Guid roleId)
        {
            try
            {
                var existingRoles = await _unitOfWork.Role.GetByCondition(r => r.RoleId == roleId);
                var role = existingRoles?.FirstOrDefault();

                if (role == null)
                {
                    return (false, "Role not found");
                }

                _unitOfWork.Role.Delete(role);
                await _unitOfWork.SaveChangesAsync();

                return (true, "Role deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting role: {ex.Message}");
            }
        }
    }
}