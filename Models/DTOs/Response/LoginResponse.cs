using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Response
{
    public class LoginResponse
    {
        public string? token { get; set; }
        public UserDto userDto { get; set; }

    }
    public class UserDto
    {
        public int id { get; set; }
        public string? userName { get; set; }

        public string? Email { get; set; }
    }

}

