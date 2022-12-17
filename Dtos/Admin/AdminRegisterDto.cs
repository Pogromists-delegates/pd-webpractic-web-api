using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Dtos.Admin
{
    public class AdminRegisterDto
    {
        public string Adminname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}