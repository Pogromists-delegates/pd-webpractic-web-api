using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Services.CatalogService
{
    public class CatalogService : ICatalogService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CatalogService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        private int GetAdminAccess() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponce<List<GetCatalogDto>>> AddCatalog(AddCatalogDto newCatalog)
        {
            var serviceResponce = new ServiceResponce<List<GetCatalogDto>>();
            var catalog = _mapper.Map<Catalog>(newCatalog);
            catalog.User = await _context.Users.FirstOrDefaultAsync(u => u.AdminAccess == GetAdminAccess());

            _context.Catalogs.Add(catalog);
            await _context.SaveChangesAsync();

            serviceResponce.Data = 
                await _context.Documents
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCatalogDto>(c))
                .ToListAsync();
            return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetCatalogDto>>> DeleteCatalog(int id)
        {
            var serviceResponce = new ServiceResponce<List<GetCatalogDto>>();
            try
            {            
                var catalog = await _context.Catalogs.FirstOrDefaultAsync(c => c.Id == id && c.User!.AdminAccess == GetAdminAccess());
                if(catalog is null)
                    throw new Exception($"Catalog with Id '{id}' not found.");
                _context.Catalogs.Remove(catalog);

                await _context.SaveChangesAsync();

                serviceResponce.Data = 
                    await _context.Catalogs
                    .Where(c => c.User!.AdminAccess == GetAdminAccess())
                    .Select(c => _mapper.Map<GetCatalogDto>(c))
                    .ToListAsync();
            } 
            catch(Exception ex)
            {
                serviceResponce.Success = false;
                serviceResponce.Message = ex.Message;
            }
            return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetCatalogDto>>> GetAllCatalogs()
        {
            var serviceResponce = new ServiceResponce<List<GetCatalogDto>>();
            var dbCatalog = await _context.Catalogs
                .Where(c => c.User!.AdminAccess == GetAdminAccess())
                .ToListAsync();
            serviceResponce.Data = dbCatalog.Select(c => _mapper.Map<GetCatalogDto>(c)).ToList();;
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetCatalogDto>> GetCatalogById(int id)
        {
            var serviceResponce = new ServiceResponce<GetCatalogDto>();
            var dbCatalog = await _context.Catalogs
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.AdminAccess == GetAdminAccess());
            serviceResponce.Data = _mapper.Map<GetCatalogDto> (dbCatalog);
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetCatalogDto>> UpdateCatalog(UpdateCatalogDto updatedCatalog)
        {
            var serviceResponce = new ServiceResponce<GetCatalogDto>();
            try
            {            
                var catalog = 
                    await _context.Catalogs
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCatalog.Id);
                if(catalog is null || catalog.User!.AdminAccess != GetAdminAccess())
                    throw new Exception($"Catalog with Id '{updatedCatalog.Id}' not found.");
                _mapper.Map(updatedCatalog, catalog);

                catalog.Title = updatedCatalog.Title;
                catalog.AccessLevel = updatedCatalog.AccessLevel;
                
                await _context.SaveChangesAsync();
                serviceResponce.Data = _mapper.Map<GetCatalogDto>(catalog);
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