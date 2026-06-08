using Microsoft.AspNetCore.Mvc;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;
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

        public IActionResult Index(string? q)
        {
            var livros = _context.Livros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                livros = livros.Where(l =>
                    l.Titulo.Contains(q) ||
                    l.Autor.Contains(q) ||
                    l.Genero.Contains(q));
            }

            return View(livros.ToList());
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

            if ((livro.Preco ?? 0m) > 0m)
                return RedirectToAction("Index");

            var jaTem = _context.BibliotecaPessoais.Any(x =>
                x.IdAluno == idUsuario.Value &&
                x.IdLivro == id);

            if (!jaTem)
            {
                _context.BibliotecaPessoais.Add(new BibliotecaPessoal
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