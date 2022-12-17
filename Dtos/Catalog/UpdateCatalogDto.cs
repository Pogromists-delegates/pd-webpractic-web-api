using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Hackathon.Dtos.Catalog
{
    public class UpdateCatalogDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool AccessLevel { get; set; } = true;
    }
}