// ViewModels/PerfilUsuarioViewModel.cs
using ProjetoLetraria.Models;

namespace ProjetoLetraria.ViewModels
{
    public class PerfilUsuarioViewModel
    {
        public Usuario UsuarioVisualizado { get; set; } = default!;
        public List<Avaliacao> Avaliacoes { get; set; } = new();
        public List<Avaliacao> Curtidas { get; set; } = new();
        public List<ComentarioAvaliacao> Comentarios { get; set; } = new();

        public int Seguidores { get; set; }
        public int Seguindo { get; set; }

        public bool EhMeuPerfil { get; set; }
        public bool EstouSeguindo { get; set; }
    }
}