using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApi_Hackathon.Services.CatalogService
{
    public interface ICatalogService
    {
        Task<ServiceResponce<List<GetCatalogDto>>> GetAllCatalogs();
        Task<ServiceResponce<GetCatalogDto>> GetCatalogById(int id);
        Task<ServiceResponce<List<GetCatalogDto>>> AddCatalog(AddCatalogDto newCatalog);
        Task<ServiceResponce<GetCatalogDto>> UpdateCatalog(UpdateCatalogDto updatedCatalog);
        Task<ServiceResponce<List<GetCatalogDto>>> DeleteCatalog(int id);
    }
}