using MODEL.DTO;
using MODEL.Eneities;

namespace BAL.IServiceFunction
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllTeams();
        Task<Department> GetTeamById(Guid teamId);
        Task<(bool success, string message,Guid id)> CreateTeam(DepartmentDTO t);
        Task<(bool success, string message)> UpdateTeam(Guid id, DepartmentDTO req);
        Task<(bool success, string message)> DeleteTeam(Guid teamId);
    }
}
