// Controllers/AdminController.cs
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
        private readonly IWebHostEnvironment _env;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private bool UsuarioEhAdmin()
        {
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            return tipoUsuario == "ADMIN";
        }

        private async Task<string?> SalvarArquivoAsync(IFormFile? arquivo, string pastaRelativa)
        {
            if (arquivo == null || arquivo.Length == 0)
                return null;

            var pastaFisica = Path.Combine(_env.WebRootPath, pastaRelativa);
            Directory.CreateDirectory(pastaFisica);

            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            var caminhoFisico = Path.Combine(pastaFisica, nomeArquivo);

            using var stream = new FileStream(caminhoFisico, FileMode.Create);
            await arquivo.CopyToAsync(stream);

            return $"/{pastaRelativa.Replace("\\", "/")}/{nomeArquivo}";
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            var totalUsuarios = await _context.Usuarios.CountAsync();
            var totalLivros = await _context.Livros.CountAsync();
            var totalCompras = await _context.Compras.CountAsync(x => x.StatusPagamento == "PAGO");
            var valorArrecadado = await _context.Compras
                .Where(x => x.StatusPagamento == "PAGO")
                .SumAsync(x => (decimal?)x.Valor) ?? 0m;

            var categoriaMaisVendida = await (
                from c in _context.Compras
                where c.StatusPagamento == "PAGO"
                join l in _context.Livros on c.IdLivro equals l.IdLivro
                group c by l.Genero into g
                orderby g.Count() descending
                select g.Key
            ).FirstOrDefaultAsync();

            var livrosMaisVendidos = await (
                from c in _context.Compras
                where c.StatusPagamento == "PAGO"
                join l in _context.Livros on c.IdLivro equals l.IdLivro
                group c by new { l.IdLivro, l.Titulo, l.Autor, l.Genero } into g
                orderby g.Count() descending
                select new LivroMaisVendidoViewModel
                {
                    IdLivro = g.Key.IdLivro,
                    Titulo = g.Key.Titulo,
                    Autor = g.Key.Autor,
                    Genero = g.Key.Genero,
                    QuantidadeVendida = g.Count(),
                    ValorArrecadado = g.Sum(x => x.Valor)
                }
            ).Take(5).ToListAsync();

            var comprasRecentes = await (
                from c in _context.Compras
                join u in _context.Usuarios on c.IdAluno equals u.IdUsuario
                join l in _context.Livros on c.IdLivro equals l.IdLivro
                orderby c.DataCompra descending
                select new CompraRecenteViewModel
                {
                    DataCompra = c.DataCompra,
                    Cliente = u.NomeExibicao ?? u.Nome,
                    Livro = l.Titulo,
                    Valor = c.Valor,
                    MetodoPagamento = c.MetodoPagamento,
                    StatusPagamento = c.StatusPagamento
                }
            ).Take(10).ToListAsync();

            var vm = new AdminDashboardViewModel
            {
                TotalUsuarios = totalUsuarios,
                TotalLivros = totalLivros,
                TotalCompras = totalCompras,
                ValorArrecadado = valorArrecadado,
                CategoriaMaisVendida = categoriaMaisVendida,
                LivrosMaisVendidos = livrosMaisVendidos,
                ComprasRecentes = comprasRecentes
            };

            return View(vm);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarLivro(LivroFormViewModel model)
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                model.TagsDisponiveis = _context.Tags.OrderBy(t => t.Nome).ToList();
                return View(model);
            }

            var capaPath = await SalvarArquivoAsync(model.ImagemCapaArquivo, "uploads/livros/capas");
            var livroPath = await SalvarArquivoAsync(model.ArquivoLivroArquivo, "uploads/livros/arquivos");

            var livro = new Livro
            {
                Titulo = model.Titulo.Trim(),
                Autor = model.Autor.Trim(),
                Resumo = model.Resumo.Trim(),
                Genero = model.Genero.Trim(),
                ImagemCapa = capaPath ?? model.ImagemCapa,
                TipoAcesso = model.TipoAcesso,
                LinkCompra = model.LinkCompra,
                ArquivoLivro = livroPath ?? model.ArquivoLivro,
                Preco = model.LivroGratis ? 0m : model.Preco,
                PossuiDigital = model.PossuiDigital,
                PossuiFisico = model.PossuiFisico,
                DataCadastro = DateTime.Now
            };

            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            foreach (var idTag in model.TagsSelecionadas.Distinct())
            {
                _context.LivroTags.Add(new LivroTag
                {
                    IdLivro = livro.IdLivro,
                    IdTag = idTag
                });
            }

            await _context.SaveChangesAsync();

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
                PossuiDigital = livro.PossuiDigital,
                PossuiFisico = livro.PossuiFisico,
                TagsDisponiveis = _context.Tags.OrderBy(t => t.Nome).ToList(),
                TagsSelecionadas = _context.LivroTags
                    .Where(x => x.IdLivro == id)
                    .Select(x => x.IdTag)
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarLivro(LivroFormViewModel model)
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                model.TagsDisponiveis = _context.Tags.OrderBy(t => t.Nome).ToList();
                return View(model);
            }

            var livro = await _context.Livros.FirstOrDefaultAsync(x => x.IdLivro == model.IdLivro);
            if (livro == null)
                return NotFound();

            var capaPath = await SalvarArquivoAsync(model.ImagemCapaArquivo, "uploads/livros/capas");
            var livroPath = await SalvarArquivoAsync(model.ArquivoLivroArquivo, "uploads/livros/arquivos");

            livro.Titulo = model.Titulo.Trim();
            livro.Autor = model.Autor.Trim();
            livro.Resumo = model.Resumo.Trim();
            livro.Genero = model.Genero.Trim();
            livro.ImagemCapa = capaPath ?? model.ImagemCapa;
            livro.TipoAcesso = model.TipoAcesso;
            livro.LinkCompra = model.LinkCompra;
            livro.ArquivoLivro = livroPath ?? model.ArquivoLivro;
            livro.Preco = model.LivroGratis ? 0m : model.Preco;
            livro.PossuiDigital = model.PossuiDigital;
            livro.PossuiFisico = model.PossuiFisico;

            _context.Livros.Update(livro);

            var tagsAntigas = _context.LivroTags.Where(x => x.IdLivro == livro.IdLivro).ToList();
            _context.LivroTags.RemoveRange(tagsAntigas);

            foreach (var idTag in model.TagsSelecionadas.Distinct())
            {
                _context.LivroTags.Add(new LivroTag
                {
                    IdLivro = livro.IdLivro,
                    IdTag = idTag
                });
            }

            await _context.SaveChangesAsync();

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
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM carrinho WHERE id_livro = {0}", id);

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Livros));
        }
    }
}