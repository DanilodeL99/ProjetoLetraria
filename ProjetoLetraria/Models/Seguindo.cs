// Models/Seguindo.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("seguindo")]
    public class Seguindo
    {
        [Key]
        [Column("id_seguindo")]
        public int IdSeguindo { get; set; }

        [Column("id_seguidor")]
        public int IdSeguidor { get; set; }

        [Column("id_seguido")]
        public int IdSeguido { get; set; }

        [Column("data_seguimento")]
        public DateTime DataSeguimento { get; set; }

        [ForeignKey(nameof(IdSeguidor))]
        public Usuario? Seguidor { get; set; }

        [ForeignKey(nameof(IdSeguido))]
        public Usuario? Seguido { get; set; }
    }
}