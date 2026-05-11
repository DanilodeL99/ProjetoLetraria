using Microsoft.AspNetCore.Mvc;
using ProjetoLetraria.Data;

namespace ProjetoLetraria.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Logar(string email, string senha, string cndb)
        {

            var usuario = _context.Usuario.FirstOrDefault(u => u.Email == email && u.Senha == senha && u.CNDB == cndb);

            if (usuario != null)
            {
              
                HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}