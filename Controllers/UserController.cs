using BAL.IServiceFunction;
using Microsoft.AspNetCore.Mvc;
using MODEL.DTO;
using MODEL.DTOs;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Users retrieved successfully.",
                    Status = ApiStatus.Successful,
                    Data = users.ToList()
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving roles.",
                    Status = ApiStatus.Error,
                });
            }
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid user ID format.",
                        Status = ApiStatus.Error
                    });
                }
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = $"User with ID {id} not found.",
                        Status = ApiStatus.Error,
                    });
                }
                if (user != null)
                {
                    return Ok(new ResponseModel
                    {
                        StatusCode = 200,
                        Message = "User retrieved successfully.",
                        Status = ApiStatus.Successful,
                        Data = user
                    });
                }
                else
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = "User not found.",
                        Status = ApiStatus.Error,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving the user.",
                    Status = ApiStatus.Error,
                });
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDto)
        {
            try
            {
                var (success, message, userId) = await _userService.CreateUser(userDto);
                if (success)
                {
                    return Ok(new ResponseModel
                    {
                        StatusCode = 200,
                        Message = message,
                        Status = ApiStatus.Successful,
                        Data = new { UserId = userId }
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = message,
                        Status = ApiStatus.Error,
                    });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while creating the user.",
                    Status = ApiStatus.Error,
                });
            }
        }

        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId,[FromBody] CreateUserDTO req)
        {
            try
            {

                if (userId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid user ID.",
                        Status = ApiStatus.Error
                    });
                }


                var (success, message) = await _userService.UpdateUser(userId,req);
                if (success)
                {
                    return Ok(new ResponseModel
                    {
                        StatusCode = 200,
                        Message = message,
                        Status = ApiStatus.Successful,
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = message,
                        Status = ApiStatus.Error,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while updating the user.",
                    Status = ApiStatus.Error,
                });
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid user ID.",
                        Status = ApiStatus.Error
                    });
                }
                var (success, message) = await _userService.DeleteUser(id);
                if (success)
                {
                    return Ok(new ResponseModel
                    {
                        StatusCode = 200,
                        Message = message,
                        Status = ApiStatus.Successful,
                    });
                }
                else
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = message,
                        Status = ApiStatus.Error,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while deleting the user.",
                    Status = ApiStatus.Error,
                });
            }
        }
        }
}
