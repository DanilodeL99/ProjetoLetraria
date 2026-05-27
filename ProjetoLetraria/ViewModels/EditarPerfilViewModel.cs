using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProjetoLetraria.ViewModels
{
    public class EditarPerfilViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome de exibição é obrigatório.")]
        public string NomeExibicao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        public string? FotoAtual { get; set; }
        public IFormFile? FotoArquivo { get; set; }

        public string? SenhaAtual { get; set; }
        [MinLength(6, ErrorMessage = "A nova senha deve ter no mínimo 6 caracteres.")]
        public string? NovaSenha { get; set; }
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        public string? ConfirmarNovaSenha { get; set; }
    }
}