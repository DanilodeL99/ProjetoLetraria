using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("catalogo_livros")]
    public class CatalogoLivro
    {
        [Key, Column("id_catalogo")]
        public int IdCatalogo { get; set; }

        [Key, Column("id_livro")]
        public int IdLivro { get; set; }

        [ForeignKey("IdCatalogo")]
        public Catalogo? Catalogo { get; set; }

        [ForeignKey("IdLivro")]
        public Livro? Livro { get; set; }
    }
}