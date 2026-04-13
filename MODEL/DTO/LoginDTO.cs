using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTO
{
    public class LoginDTO
    {
        //public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class ResponseLoginDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? token { get; set; }
        public string? RoleName { get; set; }
    }
}
