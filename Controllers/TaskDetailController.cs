using BAL.IServiceFunction;
using Microsoft.AspNetCore.Mvc;
using MODEL.DTO;
using MODEL.DTOs;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskDetailController : ControllerBase
    {
        private readonly ITaskDetailService _taskDetailService;

        public TaskDetailController(ITaskDetailService taskDetailService)
        {
            _taskDetailService = taskDetailService;
        }

        [HttpGet("GetAllTaskDetails")]
        public async Task<IActionResult> GetAllTaskDetails()
        {
            try
            {
                var taskDetails = await _taskDetailService.GetAllTaskDetails();
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Roles retrieved successfully.",
                    Status = ApiStatus.Successful,
                    Data = taskDetails.ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving roles.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpGet("GetTaskDetailById/{taskDetailId}")]
        public async Task<IActionResult> GetTaskDetailById(Guid taskDetailId)
        {
            try
            {
                if (taskDetailId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid task detail ID format.",
                        Status = ApiStatus.Error
                    });
                }
                var taskDetail = await _taskDetailService.GetTaskDetailById(taskDetailId);
                if (taskDetail == null)
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = $"Role with ID '{taskDetailId}' not found.",
                        Status = ApiStatus.Error
                    });
                }
                return Ok(taskDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving the role.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpPost("CreateTaskDetail")]
        public async Task<IActionResult> CreateTaskDetail([FromBody] CreateTaskDetailDTO req)
        {
            try
            {
                var (message, createdTaskDetail, taskDetailId) = await _taskDetailService.CreateTasksDetail(req);

                    return Ok(new ResponseModel
                    {
                        StatusCode = 201,
                        Message = "Task detail created successfully.",
                        Status = ApiStatus.Successful,
                        Data = new { TaskDetailId = taskDetailId, TaskDetail = createdTaskDetail }
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving roles.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }
        [HttpPut("UpdateTaskDetail/{taskDetailId}")]
            public async Task<IActionResult> UpdateTaskDetail(Guid taskDetailId, [FromBody] CreateTaskDetailDTO req)
        {
            try
            {
                if(taskDetailId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid task detail ID format.",
                        Status = ApiStatus.Error
                    });
                }
                var result = await _taskDetailService.UpdateTaskDetail(taskDetailId, req);
                if (result.success)
                {
                    return Ok(new ResponseModel
                    {
                        StatusCode = 200,
                        Message = result.message,
                        Status = ApiStatus.Successful
                    });
                }
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = result.message,
                    Status = ApiStatus.Error
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while updating the task detail.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }
        [HttpDelete("DeleteTaskDetail/{taskDetailId}")]
        public async Task<IActionResult> DeleteTaskDetail(Guid taskDetailId)
        {
            try
            {
                    if (taskDetailId == Guid.Empty)
                    {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid! task detail ID format.",
                        Status = ApiStatus.Error
                    });
                }
                var result = await _taskDetailService.DeleteTaskDetail(taskDetailId);
                if (result.success)
                {
                    return Ok(new { Message = result.message });
                }
                return BadRequest(new { Message = result.message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the task detail.", Error = ex.Message });
            }
        }
    }
}