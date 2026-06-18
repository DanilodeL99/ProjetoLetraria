// Controllers/PesquisaController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.ViewModels;

namespace ProjetoLetraria.Controllers
{
    public class PesquisaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PesquisaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? termo)
        {
            var vm = new PesquisaViewModel
            {
                Termo = termo?.Trim()
            };

            if (!string.IsNullOrWhiteSpace(vm.Termo))
            {
                var like = $"%{vm.Termo}%";

                vm.Livros = await _context.Livros
                    .Where(l =>
                        EF.Functions.Like(l.Titulo, like) ||
                        EF.Functions.Like(l.Autor, like) ||
                        EF.Functions.Like(l.Genero, like) ||
                        EF.Functions.Like(l.Resumo, like))
                    .OrderBy(l => l.Titulo)
                    .ToListAsync();

                vm.Usuarios = await _context.Usuarios
                    .Where(u =>
                        EF.Functions.Like(u.Nome, like) ||
                        EF.Functions.Like(u.NomeExibicao ?? "", like) ||
                        EF.Functions.Like(u.Email, like))
                    .OrderBy(u => u.Nome)
                    .ToListAsync();

                vm.Avaliacoes = await _context.Avaliacoes
                    .Include(a => a.Usuario)
                    .Include(a => a.Livro)
                    .Where(a =>
                        EF.Functions.Like(a.Comentario ?? "", like) ||
                        EF.Functions.Like(a.Livro!.Titulo, like) ||
                        EF.Functions.Like(a.Livro!.Autor, like) ||
                        EF.Functions.Like(a.Usuario!.Nome, like) ||
                        EF.Functions.Like(a.Usuario!.NomeExibicao ?? "", like))
                    .OrderByDescending(a => a.DataAvaliacao)
                    .ToListAsync();
            }

            return View(vm);
        }
    }
}