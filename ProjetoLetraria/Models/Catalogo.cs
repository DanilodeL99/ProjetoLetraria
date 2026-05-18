using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("catalogos")]
    public class Catalogo
    {
        [Key]
        [Column("id_catalogo")]
        public int IdCatalogo { get; set; }

        [Required]
        [Column("id_professor")]
        public int IdProfessor { get; set; }

        [Required(ErrorMessage = "O nome do catálogo é obrigatório.")]
        [StringLength(150)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [ForeignKey("IdProfessor")]
        public Usuario? Professor { get; set; }
    }
}