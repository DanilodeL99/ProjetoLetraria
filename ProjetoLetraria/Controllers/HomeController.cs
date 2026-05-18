using Microsoft.AspNetCore.Mvc;
using ProjetoLetraria.Data;

namespace ProjetoLetraria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var livros = _context.Livros.ToList();

            ViewBag.Usuario =
                HttpContext.Session.GetString("UsuarioNome");

            ViewBag.TipoUsuario =
                HttpContext.Session.GetString("TipoUsuario");

            return View(livros);
        }
    }
}