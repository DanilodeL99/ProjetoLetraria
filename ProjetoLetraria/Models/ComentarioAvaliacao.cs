using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("comentarios_avaliacao")]
    public class ComentarioAvaliacao
    {
        [Key]
        [Column("id_comentario")]
        public int IdComentario { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_avaliacao")]
        public int IdAvaliacao { get; set; }

        [Column("texto")]
        public string Texto { get; set; } = string.Empty;

        [Column("data_comentario")]
        public DateTime DataComentario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(IdAvaliacao))]
        public Avaliacao? Avaliacao { get; set; }
    }
}