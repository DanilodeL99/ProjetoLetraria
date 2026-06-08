// Controllers/CompraController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;

namespace ProjetoLetraria.Controllers
{
    public class CompraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompraController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Finalizar()
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

        [HttpPost]
        public IActionResult ConfirmarCompra(string metodoPagamento)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var itens = _context.Carrinho
                .Include(x => x.Livro)
                .Where(x => x.IdUsuario == idUsuario.Value)
                .ToList();

            foreach (var item in itens)
            {
                _context.Compras.Add(new Compra
                {
                    IdAluno = idUsuario.Value,
                    IdLivro = item.IdLivro,
                    Valor = item.Livro?.Preco ?? 0,
                    MetodoPagamento = metodoPagamento,
                    StatusPagamento = "PAGO",
                    DataCompra = DateTime.Now
                });

                bool jaPossui = _context.BibliotecaPessoais.Any(x =>
                    x.IdAluno == idUsuario.Value &&
                    x.IdLivro == item.IdLivro);

                if (!jaPossui)
                {
                    _context.BibliotecaPessoais.Add(new BibliotecaPessoal
                    {
                        IdAluno = idUsuario.Value,
                        IdLivro = item.IdLivro,
                        DataAdicao = DateTime.Now
                    });
                }
            }

            _context.Carrinho.RemoveRange(itens);
            _context.SaveChanges();

            return RedirectToAction("Index", "Biblioteca");
        }
    }
}