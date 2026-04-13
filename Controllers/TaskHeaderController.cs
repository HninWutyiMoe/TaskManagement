using BAL.IServiceFunction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MODEL.DTO;
using MODEL.DTOs;
using REPOSITORY;


namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskHeaderController : ControllerBase
    {
        private readonly ITaskHeaderService _taskHeaderService;

        public TaskHeaderController(ITaskHeaderService taskHeaderService)
        {
            _taskHeaderService = taskHeaderService;
        }


        [HttpGet("GetAllTaskHeaders")]
        public async Task<IActionResult> GetAllTaskHeaders()
        {
            try
            {
                var taskHeaders = await _taskHeaderService.GetAllTaskHeaders();
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Task headers retrieved successfully.",
                    Status = ApiStatus.Successful,
                    Data = taskHeaders.ToList()
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving task headers.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpGet("GetTaskHeaderById/{taskId}")]
        public async Task<IActionResult> GetTaskHeaderById(Guid taskId)
        {
            try
            {
                var taskHeader = await _taskHeaderService.GetTaskHeaderById(taskId);
                if (taskHeader == null)
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = "Task header not found.",
                        Status = ApiStatus.Error,
                        Data = null
                    });
                }

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Task header retrieved successfully.",
                    Status = ApiStatus.Successful,
                    Data = taskHeader
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving the task header.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpPost("CreateTaskHeader")]
        public async Task<IActionResult> CreateTaskHeader([FromBody] CreateTaskHeaderDTO req)
        {
            try
            {
                if (req == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid request data.",
                        Status = ApiStatus.Error,
                        Data = null
                    });
                }
                var (message, createdTaskHeader,taskId) = await _taskHeaderService.CreateTaskHeader(req);
                return Ok(new ResponseModel
                {
                    StatusCode = 201,
                    Message = "Task header created successfully.",
                    Status = ApiStatus.Successful,
                    Data = new { TaskHeaderId = taskId, TaskHeader = createdTaskHeader }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while creating the task header.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpPut("UpdateTaskHeader/{taskId}")]
        public async Task<IActionResult> UpdateTaskHeader(Guid taskId, [FromBody] CreateTaskHeaderDTO req)
        {
            try
            {
                if (taskId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid task ID or request data.",
                        Status = ApiStatus.Error,
                        Data = null
                    });
                }

                var updateResult = await _taskHeaderService.UpdateTaskHeader(taskId, req);
                if (!updateResult.success)
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = updateResult.message,
                        Status = ApiStatus.Error,
                        Data = null
                    });
                }
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Task header updated successfully.",
                    Status = ApiStatus.Successful,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while updating the task header.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("DeleteTaskHeader/{taskId}")]
        public async Task<IActionResult> DeleteTaskHeader(Guid taskId)
        {
            try
            {
                if (taskId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid task ID.",
                        Status = ApiStatus.Error,
                        Data = null
                    });
                }
                var deleteResult = await _taskHeaderService.DeleteTaskHeader(taskId);
                if (!deleteResult.success)
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = deleteResult.message,
                        Status = ApiStatus.Error,
                        Data = null
                    });
                }
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Task header deleted successfully.",
                    Status = ApiStatus.Successful,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while deleting the task header.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }
    }
}