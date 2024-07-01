using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Request
{
    public class LoginRequest
    {
        public string? email_phone { get; set; }
        public string? password { get; set; }
    }
}
