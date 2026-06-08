// Models/Curtida.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("curtidas")]
    public class Curtida
    {
        [Key]
        [Column("id_curtida")]
        public int IdCurtida { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_avaliacao")]
        public int IdAvaliacao { get; set; }

        public Usuario? Usuario { get; set; }

        public Avaliacao? Avaliacao { get; set; }
    }
}