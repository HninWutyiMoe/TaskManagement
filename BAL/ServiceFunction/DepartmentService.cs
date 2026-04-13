using BAL.IServiceFunction;
using MODEL.DTO;
using MODEL.Eneities;
using REPOSITORY.UnitOfWork;

namespace BAL.ServiceFunction
{
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Department>> GetAllTeams()
        {
            try
            {
                var result = await _unitOfWork.Department.GetAll();
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<Department> GetTeamById(Guid id)
        {
            try
            {
                var result = await _unitOfWork.Department.GetByCondition(t => t.DepartmentId == id);
                return result.FirstOrDefault()!;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<(bool success, string message,Guid id)> CreateTeam(DepartmentDTO t)
        {
            try
            {
                var existingTeams = await _unitOfWork.Department.GetAll();
                if (existingTeams.Any(x => x.Name == t.Name))
                {
                    return (false, "Team name already exists", Guid.Empty);
                }
                
                var newTeam = new Department
                {
                    DepartmentId = Guid.NewGuid(),
                    Name = t.Name,
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Department.Add(newTeam);
                await _unitOfWork.SaveChangesAsync();

                return (true, "Team created successfully", newTeam.DepartmentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<(bool success, string message)> UpdateTeam(Guid id ,DepartmentDTO req)
        {
            try
            {
                var checkdepartment = await _unitOfWork.Department.GetByCondition(t => t.DepartmentId == id);
                var exist = checkdepartment.FirstOrDefault();
                if (exist != null)
                {
                    exist.Name = req.Name ?? exist.Name;
                    exist.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.Department.Update(exist);
                    await _unitOfWork.SaveChangesAsync();
                    return (true, "Department updated successfully");
                }
                return (false, "Department not found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(bool success, string message)> DeleteTeam(Guid Id)
        {
            try
            {
                var existingTeam = await _unitOfWork.Department.GetByCondition(t => t.DepartmentId == Id);
                var team = existingTeam.FirstOrDefault();
                if (team != null)
                {
                    _unitOfWork.Department.Delete(team);
                    await _unitOfWork.SaveChangesAsync();
                    return (true, "Team deleted successfully");
                }
                return (false, "Team not found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
