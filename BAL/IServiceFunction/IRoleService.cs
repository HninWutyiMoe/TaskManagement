using Microsoft.AspNetCore.Mvc;
using MODEL.DTO;
using MODEL.Eneities;

namespace BAL.IServiceFunction
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRoles();
        Task<RoleDTO?> GetRoleById(Guid roleId);
        Task<(bool success, string message, Guid? roleId)> CreateRole(CreateRoleDTO role);
        Task<(bool success, string message)> UpdateRole(Guid roleId,CreateRoleDTO req);
        Task<(bool success, string message)> DeleteRole(Guid roleId);
    }
}
