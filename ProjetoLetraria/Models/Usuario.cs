// ===== Models/Usuario.cs =====
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

        [Required]
        [StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("nome_exibicao")]
        public string? NomeExibicao { get; set; }

        [Required]
        [StringLength(150)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("senha")]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("tipo_usuario")]
        public string TipoUsuario { get; set; } = string.Empty;

        [StringLength(50)]
        [Column("cndb")]
        public string? Cndb { get; set; }

        [StringLength(255)]
        [Column("foto_perfil")]
        public string? FotoPerfil { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }
    }
}