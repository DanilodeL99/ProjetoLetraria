// Controllers/FeedController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.Controllers
{
    public class FeedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Curtir(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");

            if (usuarioId == null)
                return RedirectToAction("Index", "Login");

            var existe = await _context.Curtidas
                .AnyAsync(x => x.IdUsuario == usuarioId.Value && x.IdAvaliacao == id);

            if (!existe)
            {
                _context.Curtidas.Add(new Curtida
                {
                    IdUsuario = usuarioId.Value,
                    IdAvaliacao = id
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comentar(int avaliacaoId, string texto)
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");

            if (usuarioId == null)
                return RedirectToAction("Index", "Login");

            if (!string.IsNullOrWhiteSpace(texto))
            {
                _context.ComentariosAvaliacao.Add(new ComentarioAvaliacao
                {
                    IdUsuario = usuarioId.Value,
                    IdAvaliacao = avaliacaoId,
                    Texto = texto.Trim(),
                    DataComentario = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}