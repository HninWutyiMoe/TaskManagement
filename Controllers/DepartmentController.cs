using BAL.IServiceFunction;
using Microsoft.AspNetCore.Mvc;
using MODEL.DTO;
using MODEL.DTOs;
using MODEL.Eneities;
using REPOSITORY.UnitOfWork;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _teamService;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IDepartmentService teamService , IUnitOfWork unitOfWork)
        {
            _teamService = teamService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllTeams()
        {
            try
            {

                var teams = await _teamService.GetAllTeams();

                return Ok(new ResponseModel
                {
                    Message = "Retrieved teams successfully.",
                    Status = ApiStatus.Successful,
                    Data = teams.ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    Message = "An error occurred while retrieving tags.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpGet("GetDepartmentById/{teamId}")]
        public async Task<IActionResult> GetTeamById(Guid teamId)
        {
            try
            {
                var team = await _teamService.GetTeamById(teamId);
                if (team == null)
                {
                    return NotFound(new ResponseModel
                    {
                        Message = "Team not found.",
                        Status = ApiStatus.Error
                    });
                }

                return Ok(new ResponseModel
                {
                    Message = "Retrieved team successfully.",
                    Status = ApiStatus.Successful,
                    Data = team
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    Message = "An error occurred while retrieving the team.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateTeam(DepartmentDTO team)
        {
            try
            {
                var existData = await _unitOfWork.Department.GetByCondition(d => d.Name == team.Name);
                if (existData == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Department already exists.",
                        Status = ApiStatus.Error
                    });
                }
                var (message, result, id) = await _teamService.CreateTeam(team);

                return Ok( new ResponseModel
                {
                  StatusCode = 201,
                    Message = "Department Created",
                  Status = ApiStatus.Successful,
                  Data = new { TeamId = id }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    Message = "An error occurred while creating the team.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpPut("UpdateDepartment/{id}")]
        public async Task<IActionResult> UpdateTeam(Guid id,[FromBody] DepartmentDTO req)
        {
            try
            {
                var result = await _teamService.UpdateTeam(id, req);
                if (!result.success)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = result.message,
                        Status = ApiStatus.Error
                    });
                }
                return Ok(new ResponseModel
                {
                    Message = Messages.UpdateSuccess,
                    Status = ApiStatus.Successful
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    Message = "An error occurred while updating the role.",
                    Status = ApiStatus.Error,
                });
            }
        }

        [HttpDelete("DeleteDepartment/{teamId}")]
        public async Task<IActionResult> DeleteTeam(Guid teamId)
        {
            try
            {
                await _teamService.DeleteTeam(teamId);
                return Ok(new ResponseModel { Message = Messages.DeleteSucess, Status = ApiStatus.Successful });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    Message = "An error occurred while deleting the role.",
                    Status = ApiStatus.Error,
                });
            }
        }
    }
}