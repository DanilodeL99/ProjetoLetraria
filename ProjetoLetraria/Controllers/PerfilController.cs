// Controllers/PerfilController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.ViewModels;

namespace ProjetoLetraria.Controllers
{
    public class PerfilController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PerfilController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private int? ObterIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        [HttpGet]
        public async Task<IActionResult> Editar()
        {
            var idUsuario = ObterIdUsuario();
            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario.Value);
            if (usuario == null)
                return RedirectToAction("Index", "Login");

            var vm = new EditarPerfilViewModel
            {
                Nome = usuario.Nome,
                NomeExibicao = usuario.NomeExibicao ?? usuario.Nome,
                Email = usuario.Email,
                FotoAtual = usuario.FotoPerfil
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarPerfilViewModel model)
        {
            var idUsuario = ObterIdUsuario();
            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario.Value);
            if (usuario == null)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                model.FotoAtual = usuario.FotoPerfil;
                return View(model);
            }

            var emailExiste = await _context.Usuarios.AnyAsync(x =>
                x.Email == model.Email && x.IdUsuario != idUsuario.Value);

            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Esse e-mail já está em uso.");
                model.FotoAtual = usuario.FotoPerfil;
                return View(model);
            }

            usuario.Nome = model.Nome.Trim();
            usuario.NomeExibicao = model.NomeExibicao.Trim();
            usuario.Email = model.Email.Trim();

            if (model.FotoArquivo != null && model.FotoArquivo.Length > 0)
            {
                var pasta = Path.Combine(_env.WebRootPath, "uploads", "perfis");
                Directory.CreateDirectory(pasta);

                var nomeArquivo = $"{Guid.NewGuid()}{Path.GetExtension(model.FotoArquivo.FileName)}";
                var caminhoFisico = Path.Combine(pasta, nomeArquivo);

                using (var stream = new FileStream(caminhoFisico, FileMode.Create))
                {
                    await model.FotoArquivo.CopyToAsync(stream);
                }

                usuario.FotoPerfil = $"/uploads/perfis/{nomeArquivo}";
            }

            if (!string.IsNullOrWhiteSpace(model.NovaSenha))
            {
                if (string.IsNullOrWhiteSpace(model.SenhaAtual) || model.SenhaAtual != usuario.Senha)
                {
                    ModelState.AddModelError("SenhaAtual", "Senha atual inválida.");
                    model.FotoAtual = usuario.FotoPerfil;
                    return View(model);
                }

                usuario.Senha = model.NovaSenha.Trim();
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("UsuarioNome", usuario.NomeExibicao ?? usuario.Nome);
            HttpContext.Session.SetString("FotoPerfil", usuario.FotoPerfil ?? "");

            return RedirectToAction("Editar");
        }
    }
}