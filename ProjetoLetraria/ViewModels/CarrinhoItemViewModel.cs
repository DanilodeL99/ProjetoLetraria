// ===== ViewModels/CarrinhoItemViewModel.cs =====
namespace ProjetoLetraria.ViewModels
{
    public class CarrinhoItemViewModel
    {
        public int IdLivro { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string? ImagemCapa { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; } = 1;
    }
}