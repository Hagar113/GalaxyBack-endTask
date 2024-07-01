using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.models
{
    public class Users
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string Email { get; set; }
        public string passWord { get; set; }
        public string Mobile { get; set; }
        public bool? isActive { get; set; }
        public string? token { get; set; }
    }
}
