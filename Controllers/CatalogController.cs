using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApi_Hackathon.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponce<List<GetCatalogDto>>>> Get()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _catalogService.GetAllCatalogs());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponce<GetCatalogDto>>> GetSingle(int id)
        {
            return Ok(await _catalogService.GetCatalogById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponce<List<GetCatalogDto>>>> AddCatalog(AddCatalogDto newCatalog)
        {
            return Ok(await _catalogService.AddCatalog(newCatalog));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponce<List<GetCatalogDto>>>> UpdateCatalog(UpdateCatalogDto updatedCatalog)
        {
            var response = await _catalogService.UpdateCatalog(updatedCatalog);
            if(response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponce<GetCatalogDto>>> DeleteCatalog(int id)
        {
            var response = await _catalogService.DeleteCatalog(id);
            if(response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}