using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public UserService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetAdminAccess() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponce<List<GetUserDto>>> AddUser(AddUserDto newUser)
        {
            var serviceResponce = new ServiceResponce<List<GetUserDto>>();
            var user = _mapper.Map<User>(newUser);
            user.Admin = await _context.Users.FirstOrDefaultAsync(u => u.AdminAccess == GetAdminAccess()); 

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            serviceResponce.Data = 
                await _context.Users.Select(c => _mapper.Map<GetUserDto>(c)).ToListAsync();
            return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetUserDto>>> DeleteUser(int id)
        {
            var serviceResponce = new ServiceResponce<List<GetUserDto>>();
            try
            {            
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id && c.Admin!.AdminAccess == GetAdminAccess());
                if(user is null)
                    throw new Exception($"User with Id '{id}' not found.");
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();

                serviceResponce.Data = 
                    await _context.Users
                    .Where(c => c.Admin!.AdminAccess == GetAdminAccess())
                    .Select(c => _mapper.Map<GetUserDto>(c)).ToListAsync();
            } 
            catch(Exception ex)
            {
                serviceResponce.Success = false;
                serviceResponce.Message = ex.Message;
            }
            return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetUserDto>>> GetAllUsers()
        {
            var serviceResponce = new ServiceResponce<List<GetUserDto>>();
            var dbUser = await _context.Users
                .Where(c => c.Admin!.AdminAccess == GetAdminAccess())
                .ToListAsync();
            serviceResponce.Data = dbUser.Select(c => _mapper.Map<GetUserDto>(c)).ToList();;
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetUserDto>> GetUserById(int id)
        {
            var serviceResponce = new ServiceResponce<GetUserDto>();
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == id && c.Admin!.AdminAccess == GetAdminAccess());
            serviceResponce.Data = _mapper.Map<GetUserDto> (dbUser);
            return serviceResponce;throw new NotImplementedException();
        }

        public async Task<ServiceResponce<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
        {
            var serviceResponce = new ServiceResponce<GetUserDto>();
            try
            {            
                var user = 
                    await _context.Users
                    .Include(c => c.Admin)
                    .FirstOrDefaultAsync(c => c.Id == updatedUser.Id);
                if(user is null || user.Admin!.AdminAccess != GetAdminAccess())
                    throw new Exception($"User with Id '{updatedUser.Id}' not found.");
                _mapper.Map(updatedUser, user);

                user.Username = updatedUser.Username;
                user.Email = updatedUser.Email;
                user.Department = updatedUser.Department;
                user.DateOfBirthday = updatedUser.DateOfBirthday;
                user.AdminAccess = updatedUser.AdminAccess;
                
                await _context.SaveChangesAsync();
                serviceResponce.Data = _mapper.Map<GetUserDto>(user);
            } 
            catch(Exception ex)
            {
                serviceResponce.Success = false;
                serviceResponce.Message = ex.Message;
            }
            return serviceResponce;
        }
    }
}