using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApi_Hackathon.Services.DocumentService
{
    public interface IDocumentService
    {
        Task<ServiceResponce<List<GetDocumentDto>>> GetAllDocuments();
        Task<ServiceResponce<GetDocumentDto>> GetDocumentById(int id);
        Task<ServiceResponce<List<GetDocumentDto>>> AddDocument(AddDocumentDto newDocument);
        Task<ServiceResponce<GetDocumentDto>> UpdateDocument(UpdateDocumentDto updatedDocument);
        Task<ServiceResponce<List<GetDocumentDto>>> DeleteDocument(int id);
    }
}