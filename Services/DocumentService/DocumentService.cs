using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Services.DocumentService
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DocumentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        private int GetAdminAccess() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.Role)!);

        public async Task<ServiceResponce<List<GetDocumentDto>>> AddDocument(AddDocumentDto newDocument)
        {
            var serviceResponce = new ServiceResponce<List<GetDocumentDto>>();
            var document = _mapper.Map<Document>(newDocument);
            document.User = await _context.Users.FirstOrDefaultAsync(u => u.AdminAccess == GetAdminAccess());

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            serviceResponce.Data = 
                await _context.Documents
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetDocumentDto>(c))
                .ToListAsync();
            return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetDocumentDto>>> DeleteDocument(int id)
        {
            var serviceResponce = new ServiceResponce<List<GetDocumentDto>>();
            try
            {            
                var document = await _context.Documents
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.AdminAccess == GetAdminAccess());
                if(document is null)
                    throw new Exception($"Document with Id '{id}' not found.");
                _context.Documents.Remove(document);

                await _context.SaveChangesAsync();

                serviceResponce.Data = 
                    await _context.Documents
                    .Where(c => c.User!.AdminAccess == GetAdminAccess())
                    .Select(c => _mapper.Map<GetDocumentDto>(c))
                    .ToListAsync();
            } 
            catch(Exception ex)
            {
                serviceResponce.Success = false;
                serviceResponce.Message = ex.Message;
            }
            return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetDocumentDto>>> GetAllDocuments()
        {
            var serviceResponce = new ServiceResponce<List<GetDocumentDto>>();
            var dbDocument = await _context.Documents
                .Where(c => c.User!.AdminAccess == GetAdminAccess())
                .ToListAsync();
            serviceResponce.Data = dbDocument.Select(c => _mapper.Map<GetDocumentDto>(c)).ToList();;
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetDocumentDto>> GetDocumentById(int id)
        {
            var serviceResponce = new ServiceResponce<GetDocumentDto>();
            var dbDocument = await _context.Documents
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.AdminAccess == GetAdminAccess());
            serviceResponce.Data = _mapper.Map<GetDocumentDto> (dbDocument);
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetDocumentDto>> UpdateDocument(UpdateDocumentDto updatedDocument)
        {
            var serviceResponce = new ServiceResponce<GetDocumentDto>();
            try
            {            
                var document = 
                    await _context.Documents
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedDocument.Id);
                if(document is null || document.User!.AdminAccess != GetAdminAccess())
                    throw new Exception($"Document with Id '{updatedDocument.Id}' not found.");
                _mapper.Map(updatedDocument, document);

                document.Title = updatedDocument.Title;
                document.AccessLevel = updatedDocument.AccessLevel;
                
                await _context.SaveChangesAsync();
                serviceResponce.Data = _mapper.Map<GetDocumentDto>(document);
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