using MODEL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServiceFunction
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();

        Task<UserDTO?> GetUserById(Guid userId);

        Task<(bool success, string message, Guid? userId)> CreateUser(CreateUserDTO userDto);
        Task<(bool success, string message)> UpdateUser(Guid userId,CreateUserDTO req);
        Task<(bool success, string message)> DeleteUser(Guid userId);
    }
}
