using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApi_Hackathon.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Catalog> Catalogs => Set<Catalog>();
        public DbSet<User> Users => Set<User>();
    }
}