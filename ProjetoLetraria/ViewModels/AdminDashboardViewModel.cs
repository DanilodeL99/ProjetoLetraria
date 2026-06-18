// ViewModels/AdminDashboardViewModel.cs
namespace ProjetoLetraria.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsuarios { get; set; }
        public int TotalLivros { get; set; }
        public int TotalCompras { get; set; }
        public decimal ValorArrecadado { get; set; }
        public string? CategoriaMaisVendida { get; set; }

        public List<LivroMaisVendidoViewModel> LivrosMaisVendidos { get; set; } = new();
        public List<CompraRecenteViewModel> ComprasRecentes { get; set; } = new();
    }

    public class LivroMaisVendidoViewModel
    {
        public int IdLivro { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int QuantidadeVendida { get; set; }
        public decimal ValorArrecadado { get; set; }
    }

    public class CompraRecenteViewModel
    {
        public DateTime DataCompra { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Livro { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; } = string.Empty;
        public string StatusPagamento { get; set; } = string.Empty;
    }
}