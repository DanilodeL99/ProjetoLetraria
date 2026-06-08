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

        public IActionResult Index()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var itens = _context.Carrinho
                .Include(x => x.Livro)
                .Where(x => x.IdUsuario == idUsuario.Value)
                .ToList();

            return View(itens);
        }

        public IActionResult Adicionar(int id)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var item = _context.Carrinho
                .FirstOrDefault(x => x.IdLivro == id && x.IdUsuario == idUsuario.Value);

            if (item == null)
            {
                _context.Carrinho.Add(new CarrinhoItem
                {
                    IdLivro = id,
                    IdUsuario = idUsuario.Value,
                    Quantidade = 1
                });
            }
            else
            {
                item.Quantidade++;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remover(int id)
        {
            var item = _context.Carrinho
                .FirstOrDefault(x => x.IdCarrinho == id);

            if (item != null)
            {
                _context.Carrinho.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}