using BAL.IServiceFunction;
using MODEL.DTO;
using MODEL.Eneities;
using Org.BouncyCastle.Crypto.Generators;
using REPOSITORY.UnitOfWork;
using BCrypt.Net; 

namespace BAL.ServiceFunction
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await _unitOfWork.User.GetAll();
            var roles = await _unitOfWork.Role.GetAll();
            var departments = await _unitOfWork.Department.GetAll();
            if (users == null || roles == null || departments == null)
            {
                return new List<UserDTO>();
            }
            var result = from u in users
                         join r in roles on u.RoleId equals r.RoleId into userRole
                         from role in userRole.DefaultIfEmpty()
                         join d in departments on role.DepartmentId equals d.DepartmentId into roleDept
                         from department in roleDept.DefaultIfEmpty()
                         select new UserDTO
                         {
                             UserId = u.UserId,
                             UserName = u.UserName,
                             RoleName = role?.RoleName,
                             DepartmentName = department?.Name,
                             Email = u.Email,
                             CreatedAt = u.CreatedAt,
                             UpdatedAt = u.UpdatedAt
                         };
            return result.ToList();
        }

        public async Task<UserDTO?> GetUserById(Guid userId)
        {
            var user = await _unitOfWork.User.GetByCondition(u => u.UserId == userId);
            var check = user.FirstOrDefault();
            if (check == null)
            {
                return null;
            }
            var role = await _unitOfWork.Role.GetByCondition(r => r.RoleId == check.RoleId);
            var roleCheck = role.FirstOrDefault();

            var department = await _unitOfWork.Department.GetByCondition(d => d.DepartmentId == roleCheck!.DepartmentId);
            var departmentCheck = department.FirstOrDefault();

            return new UserDTO
            {
                UserId = check.UserId,
                UserName = check.UserName,
                RoleName = roleCheck?.RoleName,
                DepartmentName = departmentCheck?.Name,
                Email = check.Email,
                CreatedAt = check.CreatedAt,
                UpdatedAt = check.UpdatedAt
            };
        }

        public async Task<(bool success, string message, Guid? userId)> CreateUser(CreateUserDTO userDto)
        {
            try
            {
                var users = await _unitOfWork.User.GetAll();
                if (users.Any(u => u.Email == userDto.Email))
                {
                    return (false, "Email already exists", null);
                }
                var role = await _unitOfWork.Role.GetByCondition(r=>r.RoleId == userDto.RoleId);
                var checkRole = role.FirstOrDefault();
                if (checkRole == null)
                {
                    return (false, "Invalid RoleId", null);
                }
                if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                {
                    return (false, "UserName, Email and Password are required", null);
                }
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    RoleId = userDto.RoleId!.Value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _unitOfWork.User.Add(user);
                await _unitOfWork.SaveChangesAsync();
                return (true, "User created successfully", user.UserId);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating role: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> UpdateUser(Guid userId, CreateUserDTO req)
        {
            try
            {
                var existingUsers = await _unitOfWork.User.GetByCondition(u => u.UserId == userId);
                var user = existingUsers?.FirstOrDefault();
                if (user == null)
                {
                    return (false, "User not found");
                }

                var roles = await _unitOfWork.Role.GetByCondition(r=> r.RoleId == req.RoleId);
                if (roles == null)
                {
                    return (false, "Invalid RoleId");
                }

                user.UserName = req.UserName ?? user.UserName;
                user.Email = req.Email ?? user.Email;
                if (!string.IsNullOrEmpty(req.Password))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
                }
                user.RoleId = req.RoleId ?? user.RoleId;
                user.UpdatedAt = DateTime.UtcNow;


                _unitOfWork.User.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return (true, "User updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating user: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteUser(Guid userId)
        {
            try
            {
                var existingUsers = await _unitOfWork.User.GetByCondition(u => u.UserId == userId);
                var user = existingUsers?.FirstOrDefault();
                if (user == null)
                {
                    return (false, "User not found");
                }
                _unitOfWork.User.Delete(user);
                await _unitOfWork.SaveChangesAsync();
                return (true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting user: {ex.Message}");
            }
        }
        }
}
