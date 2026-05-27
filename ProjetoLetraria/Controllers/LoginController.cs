using Microsoft.AspNetCore.Mvc;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;
using ProjetoLetraria.ViewModels;
using Microsoft.AspNetCore.Http;

namespace ProjetoLetraria.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logar(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u =>
                    u.Email == model.Email &&
                    u.Senha == model.Senha);

            if (usuario == null)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View("Index");
            }

            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);
            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(CadastroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool emailExiste = _context.Usuarios
                .Any(u => u.Email == model.Email);

            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrada.");
                return View(model);
            }

            if (model.TipoUsuario == "PROFESSOR" &&
                string.IsNullOrWhiteSpace(model.Cndb))
            {
                ModelState.AddModelError("Cndb",
                    "A CNDB é obrigatória para professores.");

                return View(model);
            }

            Usuario usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha = model.Senha,
                TipoUsuario = model.TipoUsuario,
                Cndb = model.TipoUsuario == "PROFESSOR"
                    ? model.Cndb
                    : null,

                DataCriacao = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}
