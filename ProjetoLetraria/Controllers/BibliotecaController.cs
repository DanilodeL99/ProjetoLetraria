using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;

namespace ProjetoLetraria.Controllers
{
    public class BibliotecaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BibliotecaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var idUsuario =
                HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var livros = _context.BibliotecaPessoais
                .Where(x => x.IdAluno == idUsuario)
                .Join(
                    _context.Livros,
                    b => b.IdLivro,
                    l => l.IdLivro,
                    (b, l) => l
                )
                .ToList();

            return View(livros);
        }
    }
}