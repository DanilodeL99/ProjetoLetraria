using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Catalogo> Catalogos { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Compartilhamento> Compartilhamentos { get; set; }
        public DbSet<CatalogoLivro> CatalogoLivros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogoLivro>()
                .HasKey(cl => new { cl.IdCatalogo, cl.IdLivro });

            modelBuilder.Entity<CatalogoLivro>()
                .HasOne(cl => cl.Catalogo)
                .WithMany()
                .HasForeignKey(cl => cl.IdCatalogo);

            modelBuilder.Entity<CatalogoLivro>()
                .HasOne(cl => cl.Livro)
                .WithMany()
                .HasForeignKey(cl => cl.IdLivro);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}