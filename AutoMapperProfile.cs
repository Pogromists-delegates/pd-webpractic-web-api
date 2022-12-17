using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Catalog, GetCatalogDto>();
            CreateMap<AddCatalogDto, Catalog>();
            CreateMap<UpdateCatalogDto, Catalog>();

            CreateMap<Document, GetDocumentDto>();
            CreateMap<AddDocumentDto, Document>();
            CreateMap<UpdateDocumentDto, Document>();
        }
    }
}