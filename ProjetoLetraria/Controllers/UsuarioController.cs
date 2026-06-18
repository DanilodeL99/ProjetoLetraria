// Controllers/UsuarioController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.ViewModels;

namespace ProjetoLetraria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? ObterIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        [HttpGet]
        public async Task<IActionResult> Perfil(int id)
        {
            var idLogado = ObterIdUsuario();

            if (id <= 0 && idLogado.HasValue)
                id = idLogado.Value;

            if (id <= 0)
                return RedirectToAction("Index", "Login");

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            if (usuario == null)
                return NotFound();

            var avaliacoes = await _context.Avaliacoes
                .Include(x => x.Livro)
                .Include(x => x.Usuario)
                .Include(x => x.Curtidas)
                .Include(x => x.Comentarios)
                    .ThenInclude(c => c.Usuario)
                .Where(x => x.IdUsuario == id)
                .OrderByDescending(x => x.DataAvaliacao)
                .ToListAsync();

            var curtidasIds = await _context.Curtidas
                .Where(x => x.IdUsuario == id)
                .Select(x => x.IdAvaliacao)
                .ToListAsync();

            var curtidas = await _context.Avaliacoes
                .Include(x => x.Livro)
                .Include(x => x.Usuario)
                .Include(x => x.Curtidas)
                .Include(x => x.Comentarios)
                    .ThenInclude(c => c.Usuario)
                .Where(x => curtidasIds.Contains(x.IdAvaliacao))
                .OrderByDescending(x => x.DataAvaliacao)
                .ToListAsync();

            var comentarios = await _context.ComentariosAvaliacao
                .Include(x => x.Usuario)
                .Include(x => x.Avaliacao)
                    .ThenInclude(a => a.Livro)
                .Where(x => x.IdUsuario == id)
                .OrderByDescending(x => x.DataComentario)
                .ToListAsync();

            var seguidores = await _context.Seguindo.CountAsync(x => x.IdSeguido == id);
            var seguindo = await _context.Seguindo.CountAsync(x => x.IdSeguidor == id);

            var estouSeguindo = idLogado.HasValue && await _context.Seguindo
                .AnyAsync(x => x.IdSeguidor == idLogado.Value && x.IdSeguido == id);

            var vm = new PerfilUsuarioViewModel
            {
                UsuarioVisualizado = usuario,
                Avaliacoes = avaliacoes,
                Curtidas = curtidas,
                Comentarios = comentarios,
                Seguidores = seguidores,
                Seguindo = seguindo,
                EhMeuPerfil = idLogado.HasValue && idLogado.Value == id,
                EstouSeguindo = estouSeguindo
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlternarSeguir(int id)
        {
            var idLogado = ObterIdUsuario();

            if (idLogado == null)
                return RedirectToAction("Index", "Login");

            if (idLogado.Value == id)
                return RedirectToAction(nameof(Perfil), new { id });

            var relacao = await _context.Seguindo
                .FirstOrDefaultAsync(x => x.IdSeguidor == idLogado.Value && x.IdSeguido == id);

            if (relacao == null)
            {
                _context.Seguindo.Add(new Models.Seguindo
                {
                    IdSeguidor = idLogado.Value,
                    IdSeguido = id,
                    DataSeguimento = DateTime.Now
                });
            }
            else
            {
                _context.Seguindo.Remove(relacao);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Perfil), new { id });
        }
    }
}