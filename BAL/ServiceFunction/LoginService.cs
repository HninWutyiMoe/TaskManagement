using BAL.IServiceFunction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MODEL.DTO;
using REPOSITORY.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BAL.ServiceFunction
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseLoginDTO?> Login(LoginDTO req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return null;

            var email = req.Email.Trim().ToLower();
            var users = await _unitOfWork.User.GetByCondition(u => u.Email!.ToLower() == email);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            // Verify password using BCrypt and if fail return
            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            {
                return null;
            }

            //var key = _configuration["Jwt:Key"] ?? "secret_key_123456";
            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("userName", user.UserName ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
                new Claim("roleId", user.RoleId?.ToString() ?? string.Empty)
            };
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var role = await _unitOfWork.Role.GetByCondition(r => r.RoleId == user.RoleId);
            var roleEntity = role.FirstOrDefault();

            return new ResponseLoginDTO
            {
                Username = user.UserName,
                Email = user.Email,
                token = tokenString,
                RoleName = roleEntity?.RoleName
            };
        }
    }
}
