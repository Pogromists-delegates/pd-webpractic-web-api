using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApi_Hackathon.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        public readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
            
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponce<List<GetDocumentDto>>>> Get()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _documentService.GetAllDocuments());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponce<GetDocumentDto>>> GetSingle(int id)
        {
            return Ok(await _documentService.GetDocumentById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponce<List<GetDocumentDto>>>> AddDocument(AddDocumentDto newDocument)
        {
            return Ok(await _documentService.AddDocument(newDocument));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponce<List<GetDocumentDto>>>> UpdateDocument(UpdateDocumentDto updatedDocument)
        {
            var response = await _documentService.UpdateDocument(updatedDocument);
            if(response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponce<GetDocumentDto>>> DeleteDocument(int id)
        {
            var response = await _documentService.DeleteDocument(id);
            if(response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}