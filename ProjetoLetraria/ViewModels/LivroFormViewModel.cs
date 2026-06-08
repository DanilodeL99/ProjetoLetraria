using ProjetoLetraria.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjetoLetraria.ViewModels
{
    public class LivroFormViewModel
    {
        public int IdLivro { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O autor é obrigatório.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O resumo é obrigatório.")]
        public string Resumo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O gênero é obrigatório.")]
        public string Genero { get; set; } = string.Empty;

        public string? ImagemCapa { get; set; }

        [Required(ErrorMessage = "O tipo de acesso é obrigatório.")]
        public string TipoAcesso { get; set; } = "DIGITAL";

        public string? LinkCompra { get; set; }

        public string? ArquivoLivro { get; set; }

        public decimal? Preco { get; set; }

        public bool LivroGratis { get; set; }

        public List<int> TagsSelecionadas { get; set; } = new();

        public List<Tag> TagsDisponiveis { get; set; } = new();
    }
}