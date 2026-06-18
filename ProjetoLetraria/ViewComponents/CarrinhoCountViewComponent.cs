// ViewComponents/CarrinhoCountViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;

namespace ProjetoLetraria.ViewComponents
{
    public class CarrinhoCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoCountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                return View(0);

            var total = await _context.Carrinho
                .Where(x => x.IdUsuario == idUsuario.Value)
                .SumAsync(x => (int?)x.Quantidade) ?? 0;

            return View(total);
        }
    }
}