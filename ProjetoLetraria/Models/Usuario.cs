using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

      //  [Column("tipo_usuario")]
      //  public enum Tipo_Usuario { get; set; } <- Deu erro seu comédia dps tu arruma jão

        [Column("cndb")]
        public string CNDB { get; set; }
    }
}
