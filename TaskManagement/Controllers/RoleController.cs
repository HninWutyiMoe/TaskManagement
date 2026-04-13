using BAL.IServiceFunction;
using Microsoft.AspNetCore.Mvc;
using MODEL.DTO;
using MODEL.DTOs;
using MODEL.Eneities;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRoles();

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Roles retrieved successfully.",
                    Status = ApiStatus.Successful,
                    Data = roles.ToList()
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

        [HttpGet("GetRoleById/{roleId}")]
        public async Task<IActionResult> GetRoleById(Guid roleId)
        {
            try
            {
                if (roleId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid role ID format.",
                        Status = ApiStatus.Error
                    });
                }

                var role = await _roleService.GetRoleById(roleId);

                if (role == null)
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = $"Role with ID '{roleId}' not found.",
                        Status = ApiStatus.Error
                    });
                }

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Role retrieved successfully.",
                    Status = ApiStatus.Successful,
                    Data = role
                });
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

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDTO role)
        {
            try
            {
                if (role == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Role data cannot be null.",
                        Status = ApiStatus.Error
                    });
                }

                if (string.IsNullOrWhiteSpace(role.RoleName))
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Role name is required.",
                        Status = ApiStatus.Error
                    });
                }

                if (role.DepartmentId == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Department ID is required.",
                        Status = ApiStatus.Error
                    });
                }

                var (success, message, roleId) = await _roleService.CreateRole(role);

                if (!success && message.Contains("already exists"))
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = message,
                        Status = ApiStatus.Error
                    });
                }

                if (!success)
                {
                    return StatusCode(500, new ResponseModel
                    {
                        StatusCode = 500,
                        Message = message,
                        Status = ApiStatus.Error
                    });
                }

                return StatusCode(201, new ResponseModel
                {
                    StatusCode = 201,
                    Message = message,
                    Status = ApiStatus.Successful,
                    Data = new { RoleId = roleId }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while creating the role.",
                    Status = ApiStatus.Error,
                });
            }
        }

        [HttpPut("UpdateRole/{roleId}")]
        public async Task<IActionResult> UpdateRole(Guid roleId,[FromBody] CreateRoleDTO req)
        {
            try
            {

                if (roleId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid role ID.",
                        Status = ApiStatus.Error
                    });
                }

                var (success, message) = await _roleService.UpdateRole(roleId,req);

                if (!success && message.Contains("not found"))
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = message,
                        Status = ApiStatus.Error
                    });
                }

                if (!success)
                {
                    return StatusCode(500, new ResponseModel
                    {
                        StatusCode = 500,
                        Message = message,
                        Status = ApiStatus.Error
                    });
                }

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Role updated successfully.",
                    Status = ApiStatus.Successful
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while updating the role.",
                    Status = ApiStatus.Error,
                });
            }
        }

        [HttpDelete("DeleteRole/{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)  
        {
            try
            {
                if (roleId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid role ID.",
                        Status = ApiStatus.Error
                    });
                }

                var (success, message) = await _roleService.DeleteRole(roleId);  

                if (!success && message.Contains("not found"))
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = message,
                        Status = ApiStatus.Error
                    });
                }

                if (!success)
                {
                    return StatusCode(500, new ResponseModel
                    {
                        StatusCode = 500,
                        Message = message,
                        Status = ApiStatus.Error
                    });
                }

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Role deleted successfully.",
                    Status = ApiStatus.Successful
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while deleting the role.",
                    Status = ApiStatus.Error,
                });
            }
        }
    }
}