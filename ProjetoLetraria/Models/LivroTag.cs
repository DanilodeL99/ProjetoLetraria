using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("livro_tags")]
    public class LivroTag
    {
        [Column("id_livro")]
        public int IdLivro { get; set; }

        [Column("id_tag")]
        public int IdTag { get; set; }

        public Livro? Livro { get; set; }

        public Tag? Tag { get; set; }
    }
}