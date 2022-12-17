using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DepartmentClass Department { get; set; } = DepartmentClass.Commercial;
        public DateTime DateOfBirthday { get; set; }
        public int AdminAccess { get; set; } 
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
        public User? Admin { get; set; }

        //public List<Document>? Documents { get; set; }
        //public List<Catalog>? Catalogs { get; set; }
   
    }
}