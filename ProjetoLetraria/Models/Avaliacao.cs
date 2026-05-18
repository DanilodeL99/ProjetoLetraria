using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("avaliacoes")]
    public class Avaliacao
    {
        [Key]
        [Column("id_avaliacao")]
        public int IdAvaliacao { get; set; }

        [Required]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("id_livro")]
        public int IdLivro { get; set; }

        [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5.")]
        [Column("nota")]
        public int Nota { get; set; }

        [Column("comentario")]
        public string? Comentario { get; set; }

        [Column("data_avaliacao")]
        public DateTime DataAvaliacao { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [ForeignKey("IdLivro")]
        public Livro? Livro { get; set; }
    }
}