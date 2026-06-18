// Models/Compra.cs
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

        [Column("quantidade")]
        public int Quantidade { get; set; } = 1;

        [Column("formato_compra")]
        public string FormatoCompra { get; set; } = "DIGITAL";

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("frete")]
        public decimal Frete { get; set; }

        [Column("cep")]
        public string? Cep { get; set; }

        [Column("endereco_entrega")]
        public string? EnderecoEntrega { get; set; }

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