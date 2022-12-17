using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Dtos.Document
{
    public class AddDocumentDto
    {
        public string Title { get; set; } = string.Empty;
        public bool AccessLevel { get; set; } = true;
    }
}