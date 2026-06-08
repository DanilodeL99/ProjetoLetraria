// ===== Models/Compra.cs =====
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("compras")]
    public class Compra
    {
        [Key]
        [Column("id_compra")]
        public int IdCompra { get; set; }

        [Column("id_aluno")]
        public int IdAluno { get; set; }

        [Column("id_livro")]
        public int IdLivro { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }

        [StringLength(20)]
        [Column("metodo_pagamento")]
        public string MetodoPagamento { get; set; } = string.Empty;

        [StringLength(20)]
        [Column("status_pagamento")]
        public string StatusPagamento { get; set; } = "PENDENTE";

        [Column("data_compra")]
        public DateTime DataCompra { get; set; }
    }
}