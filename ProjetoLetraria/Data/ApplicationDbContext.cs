using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext
        (
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {

        }


        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Livro> Livros { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<LivroTag> LivroTags { get; set; }

        public DbSet<Catalogo> Catalogos { get; set; }

        public DbSet<CatalogoLivro> CatalogoLivros { get; set; }

        public DbSet<Avaliacao> Avaliacoes { get; set; }

        public DbSet<Compartilhamento> Compartilhamentos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LivroTag>()
                .HasKey(lt => new
                {
                    lt.IdLivro,
                    lt.IdTag
                });

            modelBuilder.Entity<LivroTag>()
                .HasOne(lt => lt.Livro)
                .WithMany(l => l.LivroTags)
                .HasForeignKey(lt => lt.IdLivro);

            modelBuilder.Entity<LivroTag>()
                .HasOne(lt => lt.Tag)
                .WithMany()
                .HasForeignKey(lt => lt.IdTag);

            modelBuilder.Entity<CatalogoLivro>()
                .HasKey(cl => new
                {
                    cl.IdCatalogo,
                    cl.IdLivro
                });
        }
    }
}