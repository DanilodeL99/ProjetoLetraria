// Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.ViewModels;

namespace ProjetoLetraria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<HomeViewModel> MontarViewModelAsync()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            var usuario = idUsuario.HasValue
                ? await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario.Value)
                : null;

            return new HomeViewModel
            {
                UsuarioLogado = usuario,
                LivrosRecentes = await _context.Livros
                    .OrderByDescending(x => x.DataCadastro)
                    .Take(12)
                    .ToListAsync(),
                AvaliacoesRecentes = await _context.Avaliacoes
                    .Include(x => x.Usuario)
                    .Include(x => x.Livro)
                    .Include(x => x.Curtidas)
                    .Include(x => x.Comentarios)
                        .ThenInclude(c => c.Usuario)
                    .OrderByDescending(x => x.DataAvaliacao)
                    .Take(30)
                    .ToListAsync()
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await MontarViewModelAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublicarAvaliacao(HomeViewModel model)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                var vmErro = await MontarViewModelAsync();
                vmErro.LivroSelecionadoId = model.LivroSelecionadoId;
                vmErro.NotaNova = model.NotaNova;
                vmErro.ComentarioNovo = model.ComentarioNovo;
                return View("Index", vmErro);
            }

            _context.Avaliacoes.Add(new ProjetoLetraria.Models.Avaliacao
            {
                IdUsuario = idUsuario.Value,
                IdLivro = model.LivroSelecionadoId!.Value,
                Nota = model.NotaNova!.Value,
                Comentario = model.ComentarioNovo,
                DataAvaliacao = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}