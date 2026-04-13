using MODEL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServiceFunction
{
    public interface ITaskHeaderService
    {
        public Task<IEnumerable<TaskHeaderDTO>> GetAllTaskHeaders();
        public Task<TaskHeaderDTO> GetTaskHeaderById(Guid taskId);
        public Task<(bool success, string message,Guid taskId)> CreateTaskHeader(CreateTaskHeaderDTO req);
        public Task<(bool success, string message)> UpdateTaskHeader(Guid taskId, CreateTaskHeaderDTO req);
        public Task<(bool success, string message)> DeleteTaskHeader(Guid taskId);
    }
}
