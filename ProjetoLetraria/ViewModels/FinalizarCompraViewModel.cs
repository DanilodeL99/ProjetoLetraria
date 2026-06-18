// ViewModels/FinalizarCompraViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace ProjetoLetraria.ViewModels
{
    public class FinalizarCompraViewModel
    {
        public List<FinalizarCompraItemViewModel> Itens { get; set; } = new();

        public decimal Subtotal { get; set; }
        public decimal Frete { get; set; }
        public decimal Total { get; set; }

        [Required]
        public string MetodoPagamento { get; set; } = "PIX";

        public string? NomeTitular { get; set; }
        public string? NumeroCartao { get; set; }
        public string? ValidadeCartao { get; set; }
        public string? Cvv { get; set; }
    }

    public class FinalizarCompraItemViewModel
    {
        public int IdCarrinho { get; set; }
        public int IdLivro { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string? ImagemCapa { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public bool PossuiDigital { get; set; }
        public bool PossuiFisico { get; set; }
        public string FormatoSelecionado { get; set; } = "DIGITAL";
    }
}