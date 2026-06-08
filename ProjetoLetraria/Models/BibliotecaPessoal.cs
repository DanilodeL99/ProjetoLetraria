// ===== Models/BibliotecaPessoal.cs =====
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoLetraria.Models
{
    [Table("biblioteca_pessoal")]
    public class BibliotecaPessoal
    {
        [Key]
        [Column("id_biblioteca")]
        public int IdBiblioteca { get; set; }

        [Column("id_aluno")]
        public int IdAluno { get; set; }

        [Column("id_livro")]
        public int IdLivro { get; set; }

        [Column("data_adicao")]
        public DateTime DataAdicao { get; set; }
    }
}