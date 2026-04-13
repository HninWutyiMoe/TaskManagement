using BAL.IServiceFunction;
using MODEL.DTO;
using MODEL.Eneities;
using REPOSITORY.UnitOfWork;

namespace BAL.ServiceFunction
{
    public class TaskHeaderService : ITaskHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskHeaderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TaskHeaderDTO>> GetAllTaskHeaders()
        {
            var Departments = await _unitOfWork.Department.GetAll();
            var taskHeaders = await _unitOfWork.TaskHeader.GetAll();
            var result = from th in taskHeaders
                         join d in Departments on th.AssignToDepartmentId equals d.DepartmentId
                         select new TaskHeaderDTO
                         {
                             TaskId = th.TaskId,
                             AssignedTo = d.Name,
                             TaskCode = th.TaskCode,
                             Title = th.Title,
                             Description = th.Description,
                             Priority = th.Priority!,
                             Status = th.Status,
                             DueDate = th.DueDate,
                             CreatedBy = th.CreatedBy,
                             CreatedAt = th.CreatedAt,
                             UpdatedAt = th.UpdatedAt
                         };
            return result.ToList();
        }


        public async Task<TaskHeaderDTO> GetTaskHeaderById(Guid taskId)
        {
            var taskHeader = await _unitOfWork.TaskHeader.GetByCondition(t => t.TaskId == taskId);
            var task = taskHeader.FirstOrDefault();
            if (taskHeader == null)
            {
                return null;
            }
            var department = await _unitOfWork.Department.GetByCondition(d => d.DepartmentId == task!.AssignToDepartmentId);
            var d = department.FirstOrDefault();

            return new TaskHeaderDTO
            {
                TaskId = task!.TaskId,
                AssignedTo = d?.Name,
                TaskCode = task.TaskCode,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority!,
                Status = task.Status,
                DueDate = task.DueDate,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public async Task<(bool success, string message, Guid taskId)> CreateTaskHeader(CreateTaskHeaderDTO req)
        {
            try
            {
                var department = await _unitOfWork.Department.GetByCondition(d => d.DepartmentId == req.AssignToDepartmentId);

                if (department == null || !department.Any())
                {
                    return (false, "Department does not exist", Guid.Empty);
                }

                var taskCodeExists = await _unitOfWork.TaskHeader.GetByCondition(t => t.TaskCode == req.TaskCode);

                if (taskCodeExists != null && taskCodeExists.Any())
                {
                    return (false, "Task code already exists", Guid.Empty);
                }
                var taskHeader = new TaskHeader
                {
                    TaskId = Guid.NewGuid(),
                    AssignToDepartmentId = req.AssignToDepartmentId,
                    TaskCode = req.TaskCode,
                    Title = req.Title,
                    Description = req.Description,
                    Priority = (int)req.Priority,
                    Status = (int)req.Status,
                    DueDate = req.DueDate,
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _unitOfWork.TaskHeader.Add(taskHeader);
                await _unitOfWork.SaveChangesAsync();
                return (true, "Task header created successfully", taskHeader.TaskId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating task header: {ex.Message}", Guid.Empty);
            }
        }

        public async Task<(bool success, string message)> UpdateTaskHeader(Guid taskId, CreateTaskHeaderDTO req)
        {
            try
            {
                var taskHeaders = await _unitOfWork.TaskHeader.GetByCondition(t => t.TaskId == taskId);
                var taskHeader = taskHeaders.FirstOrDefault();
                if (taskHeader == null)
                {
                    return (false, "Task header not found");
                }
                var department = await _unitOfWork.Department.GetByCondition(d => d.DepartmentId == req.AssignToDepartmentId);
                if (department == null || !department.Any())
                {
                    return (false, "Department does not exist");
                }

                taskHeader.AssignToDepartmentId = req.AssignToDepartmentId ?? taskHeader.AssignToDepartmentId;
                taskHeader.TaskCode = req.TaskCode ?? taskHeader.TaskCode;
                taskHeader.Title = req.Title ?? taskHeader.Title;
                taskHeader.Description = req.Description ?? taskHeader.Description;
                taskHeader.Priority = req.Priority ?? taskHeader.Priority;
                taskHeader.Status = req.Status ?? taskHeader.Status;
                taskHeader.DueDate = req.DueDate ?? taskHeader.DueDate;
                taskHeader.CreatedBy = taskHeader.CreatedBy;
                taskHeader.UpdatedAt = DateTime.UtcNow;

                 _unitOfWork.TaskHeader.Update(taskHeader);
                await _unitOfWork.SaveChangesAsync();
                return (true, "Task header updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating task header: {ex.Message}");
            }
        }
        public async Task<(bool success, string message)> DeleteTaskHeader(Guid taskId)
        {
            try
            {
                var taskHeaders = await _unitOfWork.TaskHeader.GetByCondition(t => t.TaskId == taskId);
                var taskHeader = taskHeaders.FirstOrDefault();
                if (taskHeader == null)
                {
                    return (false, "Task header not found");
                }
                _unitOfWork.TaskHeader.Delete(taskHeader);
                await _unitOfWork.SaveChangesAsync();
                return (true, "Task header deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting task header: {ex.Message}");
            }
        }
    }
}
