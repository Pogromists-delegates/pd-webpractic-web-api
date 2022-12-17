using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponce<List<GetUserDto>>> GetAllUsers();
        Task<ServiceResponce<GetUserDto>> GetUserById(int id);
        Task<ServiceResponce<List<GetUserDto>>> AddUser(AddUserDto newUser);
        Task<ServiceResponce<GetUserDto>> UpdateUser(UpdateUserDto updatedUser);
        Task<ServiceResponce<List<GetUserDto>>> DeleteUser(int id);
    }
}