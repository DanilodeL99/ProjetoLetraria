// ===== Controllers/LoginController.cs =====
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;
using ProjetoLetraria.ViewModels;

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
        public async Task<IActionResult> Logar(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Senha == model.Senha);

            if (usuario == null)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View("Index", model);
            }

            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetString("UsuarioNome", usuario.NomeExibicao ?? usuario.Nome);
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);
            HttpContext.Session.SetString("FotoPerfil", usuario.FotoPerfil ?? "");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(CadastroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Esse e-mail já está cadastrado.");
                return View(model);
            }

            if (model.TipoUsuario == "PROFESSOR" && string.IsNullOrWhiteSpace(model.Cndb))
            {
                ModelState.AddModelError("Cndb", "A CNDB é obrigatória para professores.");
                return View(model);
            }

            var usuario = new Usuario
            {
                Nome = model.Nome,
                NomeExibicao = model.Nome,
                Email = model.Email,
                Senha = model.Senha,
                TipoUsuario = model.TipoUsuario,
                Cndb = model.TipoUsuario == "PROFESSOR" ? model.Cndb : null,
                DataCriacao = DateTime.Now,
                FotoPerfil = null
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetString("UsuarioNome", usuario.NomeExibicao ?? usuario.Nome);
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);
            HttpContext.Session.SetString("FotoPerfil", "");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}