using System.ComponentModel.DataAnnotations;

namespace ProjetoLetraria.ViewModels
{
    public class CadastroViewModel
    {
        [Required(ErrorMessage = "Digite seu nome.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite seu e-mail.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite sua senha.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme sua senha.")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione o tipo de usuário.")]
        public string TipoUsuario { get; set; } = string.Empty;

        public string? Cndb { get; set; }
    }
}