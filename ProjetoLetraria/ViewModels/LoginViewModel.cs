using System.ComponentModel.DataAnnotations;

namespace ProjetoLetraria.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Digite seu e-mail.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite sua senha.")]
        public string Senha { get; set; } = string.Empty;
    }
}