// Controllers/BibliotecaController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;

namespace ProjetoLetraria.Controllers
{
    public class BibliotecaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BibliotecaController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private int? ObterIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        public async Task<IActionResult> Index()
        {
            var idUsuario = ObterIdUsuario();

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var livros = await (
                from b in _context.BibliotecaPessoais
                where b.IdAluno == idUsuario.Value
                join l in _context.Livros on b.IdLivro equals l.IdLivro
                select l
            ).Distinct().OrderBy(x => x.Titulo).ToListAsync();

            return View(livros);
        }

        [HttpGet]
        public async Task<IActionResult> Ler(int id)
        {
            var idUsuario = ObterIdUsuario();

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var possui = await _context.BibliotecaPessoais.AnyAsync(x =>
                x.IdAluno == idUsuario.Value && x.IdLivro == id);

            if (!possui)
                return RedirectToAction(nameof(Index));

            var livro = await _context.Livros.FirstOrDefaultAsync(x => x.IdLivro == id);

            if (livro == null)
                return NotFound();

            return View(livro);
        }

        [HttpGet]
        public async Task<IActionResult> Arquivo(int id)
        {
            var idUsuario = ObterIdUsuario();

            if (idUsuario == null)
                return Unauthorized();

            var possui = await _context.BibliotecaPessoais.AnyAsync(x =>
                x.IdAluno == idUsuario.Value && x.IdLivro == id);

            if (!possui)
                return NotFound();

            var livro = await _context.Livros.FirstOrDefaultAsync(x => x.IdLivro == id);

            if (livro == null || string.IsNullOrWhiteSpace(livro.ArquivoLivro))
                return NotFound();

            var caminhoRelativo = livro.ArquivoLivro.StartsWith("/")
                ? livro.ArquivoLivro.TrimStart('/')
                : livro.ArquivoLivro;

            var caminhoFisico = Path.Combine(_env.WebRootPath, caminhoRelativo);

            if (!System.IO.File.Exists(caminhoFisico))
                return NotFound();

            var extensao = Path.GetExtension(caminhoFisico).ToLowerInvariant();
            var contentType = extensao switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain; charset=utf-8",
                ".epub" => "application/epub+zip",
                _ => "application/octet-stream"
            };

            var bytes = await System.IO.File.ReadAllBytesAsync(caminhoFisico);

            return File(bytes, contentType);
        }
    }
}