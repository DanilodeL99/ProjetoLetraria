using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }

    }
}
