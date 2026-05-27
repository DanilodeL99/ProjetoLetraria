using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("tags")]
    public class Tag
    {
        [Key]
        [Column("id_tag")]
        public int IdTag { get; set; }

        [Required]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;
    }
}