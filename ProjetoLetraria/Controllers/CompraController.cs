// Controllers/CompraController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;
using ProjetoLetraria.Models;
using ProjetoLetraria.ViewModels;

namespace ProjetoLetraria.Controllers
{
    public class CompraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompraController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? ObterIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        [HttpGet]
        public IActionResult Finalizar()
        {
            var idUsuario = ObterIdUsuario();
            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var carrinho = _context.Carrinho
                .Include(x => x.Livro)
                .Where(x => x.IdUsuario == idUsuario.Value)
                .OrderBy(x => x.IdCarrinho)
                .ToList();

            if (!carrinho.Any())
                return RedirectToAction("Index", "Carrinho");

            var itens = carrinho.Select(x => new FinalizarCompraItemViewModel
            {
                IdCarrinho = x.IdCarrinho,
                IdLivro = x.IdLivro,
                Titulo = x.Livro?.Titulo ?? "",
                Autor = x.Livro?.Autor ?? "",
                ImagemCapa = x.Livro?.ImagemCapa,
                Quantidade = x.Quantidade,
                PrecoUnitario = x.Livro?.Preco ?? 0m,
                PossuiDigital = x.Livro?.PossuiDigital ?? false,
                PossuiFisico = x.Livro?.PossuiFisico ?? false,
                FormatoSelecionado = string.IsNullOrWhiteSpace(x.FormatoSelecionado)
                    ? (x.Livro?.PossuiDigital == true ? "DIGITAL" : "FISICO")
                    : x.FormatoSelecionado
            }).ToList();

            var subtotal = itens.Sum(x => x.PrecoUnitario * x.Quantidade);

            return View(new FinalizarCompraViewModel
            {
                Itens = itens,
                Subtotal = subtotal,
                Frete = 0m,
                Total = subtotal,
                MetodoPagamento = "PIX"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarCompra(FinalizarCompraViewModel model)
        {
            var idUsuario = ObterIdUsuario();
            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var carrinho = await _context.Carrinho
                .Include(x => x.Livro)
                .Where(x => x.IdUsuario == idUsuario.Value)
                .OrderBy(x => x.IdCarrinho)
                .ToListAsync();

            if (!carrinho.Any())
                return RedirectToAction("Index", "Carrinho");

            if (model.MetodoPagamento == "CARTAO")
            {
                if (string.IsNullOrWhiteSpace(model.NomeTitular))
                    ModelState.AddModelError(nameof(model.NomeTitular), "Informe o nome do titular.");

                if (string.IsNullOrWhiteSpace(model.NumeroCartao))
                    ModelState.AddModelError(nameof(model.NumeroCartao), "Informe o número do cartão.");

                if (string.IsNullOrWhiteSpace(model.ValidadeCartao))
                    ModelState.AddModelError(nameof(model.ValidadeCartao), "Informe a validade.");

                if (string.IsNullOrWhiteSpace(model.Cvv))
                    ModelState.AddModelError(nameof(model.Cvv), "Informe o CVV.");
            }

            if (!ModelState.IsValid)
            {
                return View("Finalizar", RebuildViewModel(carrinho, model));
            }

            foreach (var itemModel in model.Itens)
            {
                var carrinhoItem = carrinho.FirstOrDefault(x => x.IdCarrinho == itemModel.IdCarrinho);
                if (carrinhoItem == null || carrinhoItem.Livro == null)
                    continue;

                var formato = itemModel.FormatoSelecionado == "FISICO" ? "FISICO" : "DIGITAL";
                var subtotalLinha = carrinhoItem.Livro.Preco.GetValueOrDefault() * carrinhoItem.Quantidade;

                _context.Compras.Add(new Compra
                {
                    IdAluno = idUsuario.Value,
                    IdLivro = carrinhoItem.IdLivro,
                    Quantidade = carrinhoItem.Quantidade,
                    FormatoCompra = formato,
                    Valor = subtotalLinha,
                    Frete = 0m,
                    Cep = null,
                    EnderecoEntrega = null,
                    MetodoPagamento = model.MetodoPagamento,
                    StatusPagamento = "PAGO",
                    DataCompra = DateTime.Now
                });

                var jaTemNaBiblioteca = await _context.BibliotecaPessoais.AnyAsync(x =>
                    x.IdAluno == idUsuario.Value && x.IdLivro == carrinhoItem.IdLivro);

                if (!jaTemNaBiblioteca)
                {
                    _context.BibliotecaPessoais.Add(new BibliotecaPessoal
                    {
                        IdAluno = idUsuario.Value,
                        IdLivro = carrinhoItem.IdLivro,
                        DataAdicao = DateTime.Now
                    });
                }
            }

            _context.Carrinho.RemoveRange(carrinho);
            await _context.SaveChangesAsync();

            TempData["CompraSucesso"] = "Compra concluída com sucesso.";
            return RedirectToAction(nameof(CompraConfirmada));
        }

        [HttpGet]
        public IActionResult CompraConfirmada()
        {
            return View();
        }

        private FinalizarCompraViewModel RebuildViewModel(List<CarrinhoItem> carrinho, FinalizarCompraViewModel posted)
        {
            var itens = carrinho.Select(x => new FinalizarCompraItemViewModel
            {
                IdCarrinho = x.IdCarrinho,
                IdLivro = x.IdLivro,
                Titulo = x.Livro?.Titulo ?? "",
                Autor = x.Livro?.Autor ?? "",
                ImagemCapa = x.Livro?.ImagemCapa,
                Quantidade = x.Quantidade,
                PrecoUnitario = x.Livro?.Preco ?? 0m,
                PossuiDigital = x.Livro?.PossuiDigital ?? false,
                PossuiFisico = x.Livro?.PossuiFisico ?? false,
                FormatoSelecionado = posted.Itens.FirstOrDefault(i => i.IdCarrinho == x.IdCarrinho)?.FormatoSelecionado
                    ?? (x.Livro?.PossuiDigital == true ? "DIGITAL" : "FISICO")
            }).ToList();

            var subtotal = itens.Sum(x => x.PrecoUnitario * x.Quantidade);

            return new FinalizarCompraViewModel
            {
                Itens = itens,
                Subtotal = subtotal,
                Frete = 0m,
                Total = subtotal,
                MetodoPagamento = posted.MetodoPagamento,
                NomeTitular = posted.NomeTitular,
                NumeroCartao = posted.NumeroCartao,
                ValidadeCartao = posted.ValidadeCartao,
                Cvv = posted.Cvv
            };
        }
    }
}