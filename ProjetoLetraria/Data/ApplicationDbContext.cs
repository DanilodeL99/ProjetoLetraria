// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<LivroTag> LivroTags { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<BibliotecaPessoal> BibliotecaPessoais { get; set; }
        public DbSet<CarrinhoItem> Carrinho { get; set; }
        public DbSet<Curtida> Curtidas { get; set; }
        public DbSet<ComentarioAvaliacao> ComentariosAvaliacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<LivroTag>()
                .HasKey(x => new { x.IdLivro, x.IdTag });

            modelBuilder.Entity<LivroTag>()
                .HasOne(x => x.Livro)
                .WithMany(x => x.LivroTags)
                .HasForeignKey(x => x.IdLivro);

            modelBuilder.Entity<LivroTag>()
                .HasOne(x => x.Tag)
                .WithMany()
                .HasForeignKey(x => x.IdTag);

            modelBuilder.Entity<Avaliacao>()
                .HasOne(x => x.Livro)
                .WithMany(x => x.Avaliacoes)
                .HasForeignKey(x => x.IdLivro);

            modelBuilder.Entity<Avaliacao>()
                .HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario);

            modelBuilder.Entity<Curtida>()
                .HasOne(x => x.Avaliacao)
                .WithMany(x => x.Curtidas)
                .HasForeignKey(x => x.IdAvaliacao);

            modelBuilder.Entity<Curtida>()
                .HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario);

            modelBuilder.Entity<ComentarioAvaliacao>()
                .HasOne(x => x.Avaliacao)
                .WithMany(x => x.Comentarios)
                .HasForeignKey(x => x.IdAvaliacao);

            modelBuilder.Entity<ComentarioAvaliacao>()
                .HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario);
        }
    }
}