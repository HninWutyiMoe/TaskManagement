using BAL.IServiceFunction;
using Microsoft.AspNetCore.Mvc;
using MODEL.DTO;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public AuthenticationController(ILoginService loginService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _loginService = loginService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO req)
        {
            try
            {
                var check = await _unitOfWork.User.GetByCondition(u => u.Email == req.Email);
                if (check == null)
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = "Invalid User"
                    });

                var res = await _loginService.Login(req);
                if (res == null)
                    return Unauthorized(new ResponseModel
                    {
                        StatusCode = 401,
                        Message = "Invalid credentials"
                    });
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Data = res,
                    Message = "Login successful",
                });
            }
            catch (Exception ex) { 
             return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An error occurred during login",
                    Data = ex.Message
                });
            }
        }
    }
}
