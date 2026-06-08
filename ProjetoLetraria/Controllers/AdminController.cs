using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;
using ProjetoLetraria.ViewModels;

namespace ProjetoLetraria.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool UsuarioEhAdmin()
        {
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            return tipoUsuario == "ADMIN";
        }

        public IActionResult Livros()
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            var livros = _context.Livros.ToList();
            return View(livros);
        }

        [HttpGet]
        public IActionResult AdicionarLivro()
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            var vm = new LivroFormViewModel
            {
                TagsDisponiveis = _context.Tags.OrderBy(t => t.Nome).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AdicionarLivro(LivroFormViewModel model)
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                model.TagsDisponiveis = _context.Tags.OrderBy(t => t.Nome).ToList();
                return View(model);
            }

            var livro = new Livro
            {
                Titulo = model.Titulo,
                Autor = model.Autor,
                Resumo = model.Resumo,
                Genero = model.Genero,
                ImagemCapa = model.ImagemCapa,
                TipoAcesso = model.TipoAcesso,
                LinkCompra = model.LinkCompra,
                ArquivoLivro = model.ArquivoLivro,
                Preco = model.LivroGratis ? 0m : model.Preco,
                DataCadastro = DateTime.Now
            };

            _context.Livros.Add(livro);
            _context.SaveChanges();

            foreach (var idTag in model.TagsSelecionadas.Distinct())
            {
                _context.LivroTags.Add(new LivroTag
                {
                    IdLivro = livro.IdLivro,
                    IdTag = idTag
                });
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Livros));
        }

        [HttpGet]
        public IActionResult EditarLivro(int id)
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            var livro = _context.Livros.FirstOrDefault(x => x.IdLivro == id);
            if (livro == null)
                return NotFound();

            var vm = new LivroFormViewModel
            {
                IdLivro = livro.IdLivro,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                Resumo = livro.Resumo,
                Genero = livro.Genero,
                ImagemCapa = livro.ImagemCapa,
                TipoAcesso = livro.TipoAcesso,
                LinkCompra = livro.LinkCompra,
                ArquivoLivro = livro.ArquivoLivro,
                Preco = livro.Preco,
                LivroGratis = (livro.Preco ?? 0m) <= 0m,
                TagsDisponiveis = _context.Tags.OrderBy(t => t.Nome).ToList(),
                TagsSelecionadas = _context.LivroTags
                    .Where(x => x.IdLivro == id)
                    .Select(x => x.IdTag)
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditarLivro(LivroFormViewModel model)
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                model.TagsDisponiveis = _context.Tags.OrderBy(t => t.Nome).ToList();
                return View(model);
            }

            var livro = _context.Livros.FirstOrDefault(x => x.IdLivro == model.IdLivro);
            if (livro == null)
                return NotFound();

            livro.Titulo = model.Titulo;
            livro.Autor = model.Autor;
            livro.Resumo = model.Resumo;
            livro.Genero = model.Genero;
            livro.ImagemCapa = model.ImagemCapa;
            livro.TipoAcesso = model.TipoAcesso;
            livro.LinkCompra = model.LinkCompra;
            livro.ArquivoLivro = model.ArquivoLivro;
            livro.Preco = model.LivroGratis ? 0m : model.Preco;

            _context.Livros.Update(livro);

            var tagsAntigas = _context.LivroTags.Where(x => x.IdLivro == livro.IdLivro);
            _context.LivroTags.RemoveRange(tagsAntigas);

            foreach (var idTag in model.TagsSelecionadas.Distinct())
            {
                _context.LivroTags.Add(new LivroTag
                {
                    IdLivro = livro.IdLivro,
                    IdTag = idTag
                });
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Livros));
        }

        [HttpGet]
        public async Task<IActionResult> ExcluirLivro(int id)
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
                return NotFound();

            await _context.Database.ExecuteSqlRawAsync("DELETE FROM livro_tags WHERE id_livro = {0}", id);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM avaliacoes WHERE id_livro = {0}", id);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM compras WHERE id_livro = {0}", id);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM biblioteca_pessoal WHERE id_livro = {0}", id);

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Livros));
        }
    }
}