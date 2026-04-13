using BAL.IServiceFunction;
using MODEL.DTO;
using MODEL.Eneities;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ServiceFunction
{
    public class TaskDetailService : ITaskDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TaskDetailDTO>> GetAllTaskDetails()
        {
            var taskDetails = await _unitOfWork.TaskDetail.GetAll();
            var taskHeader = await _unitOfWork.TaskHeader.GetAll();
            var users = await _unitOfWork.User.GetAll();
            if(taskDetails == null || taskHeader == null || users == null)
            {
                return new List<TaskDetailDTO>();
            }
            var taskDetailDTOs = from td in taskDetails
                                 join th in taskHeader on td.TaskId equals th.TaskId into thGroup
                                 from th in thGroup.DefaultIfEmpty()
                                 join u in users on td.UserId equals u.UserId into uGroup
                                 from u in uGroup.DefaultIfEmpty()
                                 select new TaskDetailDTO
                                 {
                                     TaskDetailId = td.TaskDetailId,
                                     TaskId = td.TaskId,
                                     UserId = td.UserId,
                                     LineNumber = td.LineNumber,
                                     ItemTitle = td.ItemTitle,
                                     ItemDescription = td.ItemDescription,
                                     IsCompleted = td.IsCompleted,
                                     Remark = td.Remark,
                                     CreatedAt = td.CreatedAt,
                                     UpdatedAt = td.UpdatedAt
                                 };
            return taskDetailDTOs.ToList();

        }
        public async Task<TaskDetailDTO> GetTaskDetailById(Guid taskDetailId)
        {
            var taskDetails = await _unitOfWork.TaskDetail.GetByCondition(td => td.TaskDetailId == taskDetailId);
            var taskDetail = taskDetails.FirstOrDefault();
            if (taskDetail == null)
            {
                return null;
            }
            var taskHeader = await _unitOfWork.TaskHeader.GetByCondition(th => th.TaskId == taskDetail.TaskId);
            var th = taskHeader.FirstOrDefault();
            var user = await _unitOfWork.User.GetByCondition(u => u.UserId == taskDetail.UserId);
            var u = user.FirstOrDefault();
            return new TaskDetailDTO
            {
                TaskDetailId = taskDetail.TaskDetailId,
                TaskId = taskDetail.TaskId,
                UserId = taskDetail.UserId,
                LineNumber = taskDetail.LineNumber,
                ItemTitle = taskDetail.ItemTitle,
                ItemDescription = taskDetail.ItemDescription,
                IsCompleted = taskDetail.IsCompleted,
                Remark = taskDetail.Remark,
                CreatedAt = taskDetail.CreatedAt,
                UpdatedAt = taskDetail.UpdatedAt
            };
        }

        public async Task<(bool success, string message, Guid? taskDetailId)>CreateTasksDetail(CreateTaskDetailDTO req)
        {
            try
            {
                var taskHeader = await _unitOfWork.TaskHeader.GetByCondition(th => th.TaskId == req.TaskId);
                var th = taskHeader.FirstOrDefault();
                if (th == null)
                {
                    return (false, "Associated task header not found", null);
                }

                var user = await _unitOfWork.User.GetByCondition(u => u.UserId == req.UserId);
                var u = user.FirstOrDefault();
                if (u == null)
                {
                    return (false, "Associated user not found", null);
                }


                var newTaskDetail = new TaskDetail
                {
                    TaskDetailId = Guid.NewGuid(),
                    TaskId = req.TaskId,
                    UserId = req.UserId,
                    LineNumber = req.LineNumber,
                    ItemTitle = req.ItemTitle,
                    ItemDescription = req.ItemDescription,
                    IsCompleted = req.IsCompleted,
                    Remark = req.Remark,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _unitOfWork.TaskDetail.Add(newTaskDetail);
                await _unitOfWork.SaveChangesAsync();
                return (true, "Task detail created successfully", newTaskDetail.TaskDetailId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating task detail: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> UpdateTaskDetail(Guid taskDetailId, CreateTaskDetailDTO req)
        {
            try
            {
                var taskDetails = await _unitOfWork.TaskDetail.GetByCondition(td => td.TaskDetailId == taskDetailId);
                var taskDetail = taskDetails.FirstOrDefault();
                if (taskDetail == null)
                {
                    return (false, "Task detail not found");
                }

                var taskHeader = await _unitOfWork.TaskHeader.GetByCondition(th => th.TaskId == taskDetail.TaskId);
                var th = taskHeader.FirstOrDefault();
                if (th == null)
                {
                    return (false, "Associated task header not found");
                }

                var user = await _unitOfWork.User.GetByCondition(u => u.UserId == taskDetail.UserId);
                var u = user.FirstOrDefault();
                if (u == null)
                {
                    return (false, "Associated user not found");
                }

                taskDetail.TaskId = req.TaskId ?? taskDetail.TaskId;
                taskDetail.UserId = req.UserId ?? taskDetail.UserId;
                taskDetail.LineNumber = req.LineNumber ?? taskDetail.LineNumber;
                taskDetail.ItemTitle = req.ItemTitle ?? taskDetail.ItemTitle;
                taskDetail.ItemDescription = req.ItemDescription ?? taskDetail.ItemDescription;
                taskDetail.IsCompleted = req.IsCompleted ?? taskDetail.IsCompleted;
                taskDetail.Remark = req.Remark ?? taskDetail.Remark;
                taskDetail.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.TaskDetail.Update(taskDetail);
                await _unitOfWork.SaveChangesAsync();
                return (true, "Task detail updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating task detail: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteTaskDetail(Guid taskDetailId)
        {
            try
            {
                var taskDetails = await _unitOfWork.TaskDetail.GetByCondition(td => td.TaskDetailId == taskDetailId);
                var taskDetail = taskDetails.FirstOrDefault();
                if (taskDetail == null)
                {
                    return (false, "Task detail not found");
                }
                _unitOfWork.TaskDetail.Delete(taskDetail);
                await _unitOfWork.SaveChangesAsync();
                return (true, "Task detail deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting task detail: {ex.Message}");
            }
        }
    }
}
