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

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200)]
        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O autor é obrigatório.")]
        [StringLength(150)]
        [Column("autor")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O resumo é obrigatório.")]
        [Column("resumo")]
        public string Resumo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O gênero é obrigatório.")]
        [StringLength(100)]
        [Column("genero")]
        public string Genero { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("imagem_capa")]
        public string? ImagemCapa { get; set; }

        [Required(ErrorMessage = "O tipo de acesso é obrigatório.")]
        [Column("tipo_acesso")]
        public string TipoAcesso { get; set; } = string.Empty; // DIGITAL ou COMPRA

        [StringLength(255)]
        [Column("link_compra")]
        public string? LinkCompra { get; set; }

        [StringLength(255)]
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
    }
}