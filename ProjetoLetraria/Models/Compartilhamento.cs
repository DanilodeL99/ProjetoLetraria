using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("compartilhamentos")]
    public class Compartilhamento
    {
        [Key]
        [Column("id_compartilhamento")]
        public int IdCompartilhamento { get; set; }

        [Required]
        [Column("id_catalogo")]
        public int IdCatalogo { get; set; }

        [Required]
        [Column("id_aluno")]
        public int IdAluno { get; set; }

        [Column("data_compartilhamento")]
        public DateTime DataCompartilhamento { get; set; }

        [ForeignKey("IdCatalogo")]
        public Catalogo? Catalogo { get; set; }

        [ForeignKey("IdAluno")]
        public Usuario? Aluno { get; set; }
    }
}