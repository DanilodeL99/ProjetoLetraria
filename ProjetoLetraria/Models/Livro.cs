using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("livros")]
    public class Livro
    {
        [Key]
        [Column("id_livro")]
        public int IdLivro { get; set; }

        [Required]
        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Column("autor")]
        public string Autor { get; set; } = string.Empty;

        [Required]
        [Column("resumo")]
        public string Resumo { get; set; } = string.Empty;

        [Required]
        [Column("genero")]
        public string Genero { get; set; } = string.Empty;

        [Column("imagem_capa")]
        public string? ImagemCapa { get; set; }

        [Column("tipo_acesso")]
        public string TipoAcesso { get; set; } = "DIGITAL";

        [Column("link_compra")]
        public string? LinkCompra { get; set; }

        [Column("arquivo_livro")]
        public string? ArquivoLivro { get; set; }

        [Column("possui_amostra")]
        public bool PossuiAmostra { get; set; }

        [Column("limite_amostra")]
        public int? LimiteAmostra { get; set; }

        [Column("preco")]
        public decimal? Preco { get; set; }

        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; }

        public ICollection<LivroTag> LivroTags { get; set; }
            = new List<LivroTag>();

        public ICollection<Avaliacao> Avaliacoes { get; set; }
            = new List<Avaliacao>();
    }
}