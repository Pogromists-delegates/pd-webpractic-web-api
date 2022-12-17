using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponce<int>> Register(User user, string password);
        Task<ServiceResponce<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}