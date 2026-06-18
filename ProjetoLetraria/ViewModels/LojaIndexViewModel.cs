// ViewModels/LojaIndexViewModel.cs
namespace ProjetoLetraria.ViewModels
{
    public class LojaIndexViewModel
    {
        public string? Termo { get; set; }
        public List<LojaLivroViewModel> Livros { get; set; } = new();
    }

    public class LojaLivroViewModel
    {
        public int IdLivro { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string? ImagemCapa { get; set; }
        public decimal Preco { get; set; }
        public bool PossuiDigital { get; set; }
        public bool PossuiFisico { get; set; }
        public bool JaAdquirido { get; set; }
        public bool NaBiblioteca { get; set; }
        public bool Gratuito => Preco <= 0m;
    }
}