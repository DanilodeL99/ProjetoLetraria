// Controllers/CarrinhoController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? ObterIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        private async Task<int> ContarItensAsync(int idUsuario)
        {
            return await _context.Carrinho
                .Where(x => x.IdUsuario == idUsuario)
                .SumAsync(x => (int?)x.Quantidade) ?? 0;
        }

        public IActionResult Index()
        {
            var idUsuario = ObterIdUsuario();

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var itens = _context.Carrinho
                .Include(x => x.Livro)
                .Where(x => x.IdUsuario == idUsuario.Value)
                .ToList();

            return View(itens);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarAjax(int id)
        {
            var idUsuario = ObterIdUsuario();

            if (idUsuario == null)
                return Unauthorized();

            var livro = await _context.Livros.FirstOrDefaultAsync(x => x.IdLivro == id);
            if (livro == null)
                return NotFound();

            var item = await _context.Carrinho
                .FirstOrDefaultAsync(x => x.IdLivro == id && x.IdUsuario == idUsuario.Value);

            if (item == null)
            {
                _context.Carrinho.Add(new CarrinhoItem
                {
                    IdLivro = id,
                    IdUsuario = idUsuario.Value,
                    Quantidade = 1,
                    FormatoSelecionado = livro.PossuiDigital ? "DIGITAL" : "FISICO"
                });
            }
            else
            {
                item.Quantidade++;
            }

            await _context.SaveChangesAsync();

            var total = await ContarItensAsync(idUsuario.Value);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, total });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarFormato(int idCarrinho, string formatoSelecionado)
        {
            var idUsuario = ObterIdUsuario();

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var item = await _context.Carrinho
                .FirstOrDefaultAsync(x => x.IdCarrinho == idCarrinho && x.IdUsuario == idUsuario.Value);

            if (item == null)
                return NotFound();

            item.FormatoSelecionado = formatoSelecionado == "FISICO" ? "FISICO" : "DIGITAL";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remover(int id)
        {
            var item = await _context.Carrinho.FirstOrDefaultAsync(x => x.IdCarrinho == id);

            if (item != null)
            {
                _context.Carrinho.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}