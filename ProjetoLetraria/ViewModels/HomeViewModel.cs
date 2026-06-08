// ViewModels/HomeViewModel.cs
using System.ComponentModel.DataAnnotations;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.ViewModels
{
    public class HomeViewModel
    {
        public Usuario? UsuarioLogado { get; set; }

        public List<Livro> LivrosRecentes { get; set; } = new();

        public List<Avaliacao> AvaliacoesRecentes { get; set; } = new();

        [Required(ErrorMessage = "Selecione um livro.")]
        public int? LivroSelecionadoId { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5.")]
        public int? NotaNova { get; set; } = 5;

        [Required(ErrorMessage = "Escreva sua opinião.")]
        [StringLength(1000, ErrorMessage = "Máximo de 1000 caracteres.")]
        public string ComentarioNovo { get; set; } = string.Empty;
    }
}