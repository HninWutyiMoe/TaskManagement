using MODEL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServiceFunction
{
    public interface ITaskDetailService
    {
        public Task<IEnumerable<TaskDetailDTO>> GetAllTaskDetails();
        public Task<TaskDetailDTO> GetTaskDetailById(Guid taskDetailId);
        public Task<(bool success, string message, Guid? taskDetailId)>CreateTasksDetail(CreateTaskDetailDTO req);
        public Task<(bool success, string message)> UpdateTaskDetail(Guid taskDetailId, CreateTaskDetailDTO req);
        public Task<(bool success, string message)> DeleteTaskDetail(Guid taskDetailId);
    }
}
