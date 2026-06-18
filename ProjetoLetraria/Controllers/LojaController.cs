// Controllers/LojaController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.ViewModels;

namespace ProjetoLetraria.Controllers
{
    public class LojaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LojaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? q)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            var livrosQuery = _context.Livros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                livrosQuery = livrosQuery.Where(l =>
                    l.Titulo.Contains(q) ||
                    l.Autor.Contains(q) ||
                    l.Genero.Contains(q));
            }

            var bibliotecaIds = new HashSet<int>();
            var comprasIds = new HashSet<int>();

            if (idUsuario.HasValue)
            {
                bibliotecaIds = (await _context.BibliotecaPessoais
                    .Where(x => x.IdAluno == idUsuario.Value)
                    .Select(x => x.IdLivro)
                    .ToListAsync())
                    .ToHashSet();

                comprasIds = (await _context.Compras
                    .Where(x => x.IdAluno == idUsuario.Value && x.StatusPagamento == "PAGO")
                    .Select(x => x.IdLivro)
                    .ToListAsync())
                    .ToHashSet();
            }

            var livros = await livrosQuery
                .OrderBy(x => x.Titulo)
                .Select(l => new LojaLivroViewModel
                {
                    IdLivro = l.IdLivro,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    Genero = l.Genero,
                    ImagemCapa = l.ImagemCapa,
                    Preco = l.Preco ?? 0m,
                    PossuiDigital = l.PossuiDigital,
                    PossuiFisico = l.PossuiFisico,
                    NaBiblioteca = bibliotecaIds.Contains(l.IdLivro),
                    JaAdquirido = comprasIds.Contains(l.IdLivro)
                })
                .ToListAsync();

            return View(new LojaIndexViewModel
            {
                Termo = q,
                Livros = livros
            });
        }

        [HttpGet]
        public IActionResult Resgatar(int id)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var livro = _context.Livros.FirstOrDefault(x => x.IdLivro == id);
            if (livro == null)
                return NotFound();

            if ((livro.Preco ?? 0m) > 0m || !livro.PossuiDigital)
                return RedirectToAction(nameof(Index));

            var jaTem = _context.BibliotecaPessoais.Any(x =>
                x.IdAluno == idUsuario.Value &&
                x.IdLivro == id);

            if (!jaTem)
            {
                _context.BibliotecaPessoais.Add(new Models.BibliotecaPessoal
                {
                    IdAluno = idUsuario.Value,
                    IdLivro = id,
                    DataAdicao = DateTime.Now
                });

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Biblioteca");
        }
    }
}