// ViewModels/PesquisaViewModel.cs
using ProjetoLetraria.Models;

namespace ProjetoLetraria.ViewModels
{
    public class PesquisaViewModel
    {
        public string? Termo { get; set; }

        public List<Livro> Livros { get; set; } = new();
        public List<Usuario> Usuarios { get; set; } = new();
        public List<Avaliacao> Avaliacoes { get; set; } = new();
    }
}