using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Dtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DepartmentClass Department { get; set; } = DepartmentClass.Commercial;
        public DateTime DateOfBirthday { get; set; }
        public int AccessLevel { get; set; } 
    }
}