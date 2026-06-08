using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("carrinho")]
    public class CarrinhoItem
    {
        [Key]
        [Column("id_carrinho")]
        public int IdCarrinho { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_livro")]
        public int IdLivro { get; set; }

        [Column("quantidade")]
        public int Quantidade { get; set; } = 1;

        [ForeignKey(nameof(IdLivro))]
        public Livro? Livro { get; set; }
    }
}