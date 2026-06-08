using System.ComponentModel.DataAnnotations;

namespace ProjetoLetraria.Models
{
    public class Postagem
    {
        [Key]
        public int IdPostagem { get; set; }

        public int IdUsuario { get; set; }

        public int? IdLivro { get; set; }

        public string Texto { get; set; }

        public DateTime DataPostagem { get; set; }

        public Usuario Usuario { get; set; }

        public Livro Livro { get; set; }
    }
}