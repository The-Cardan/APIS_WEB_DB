using APIS_WEB_DB.Models;
using Microsoft.EntityFrameworkCore;

namespace APIS_WEB_DB.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }
    }
}