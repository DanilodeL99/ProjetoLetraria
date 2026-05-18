using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(150)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(255)]
        [Column("senha")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [Column("tipo_usuario")]
        public string TipoUsuario { get; set; } = string.Empty; // PROFESSOR ou ALUNO

        [StringLength(50)]
        [Column("cndb")]
        public string? Cndb { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }
    }
}