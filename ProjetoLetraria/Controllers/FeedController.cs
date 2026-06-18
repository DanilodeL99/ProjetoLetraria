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

        private bool PodeExcluirAvaliacao(Avaliacao avaliacao)
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (tipoUsuario == "ADMIN" || tipoUsuario == "PROFESSOR")
                return true;

            return idUsuario.HasValue && avaliacao.IdUsuario == idUsuario.Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Curtir(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");

            if (usuarioId == null)
                return Unauthorized();

            var curtida = await _context.Curtidas
                .FirstOrDefaultAsync(x => x.IdUsuario == usuarioId.Value && x.IdAvaliacao == id);

            bool liked;

            if (curtida == null)
            {
                _context.Curtidas.Add(new Curtida
                {
                    IdUsuario = usuarioId.Value,
                    IdAvaliacao = id
                });

                liked = true;
            }
            else
            {
                _context.Curtidas.Remove(curtida);
                liked = false;
            }

            await _context.SaveChangesAsync();

            var total = await _context.Curtidas.CountAsync(x => x.IdAvaliacao == id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    success = true,
                    liked,
                    total
                });
            }

            var returnUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

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

            var returnUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes
                .Include(x => x.Comentarios)
                .Include(x => x.Curtidas)
                .FirstOrDefaultAsync(x => x.IdAvaliacao == id);

            if (avaliacao == null)
                return NotFound();

            if (!PodeExcluirAvaliacao(avaliacao))
                return Forbid();

            var comentarios = _context.ComentariosAvaliacao.Where(x => x.IdAvaliacao == id).ToList();
            var curtidas = _context.Curtidas.Where(x => x.IdAvaliacao == id).ToList();

            _context.ComentariosAvaliacao.RemoveRange(comentarios);
            _context.Curtidas.RemoveRange(curtidas);
            _context.Avaliacoes.Remove(avaliacao);

            await _context.SaveChangesAsync();

            var returnUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}