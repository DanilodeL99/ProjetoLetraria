// ViewComponents/LivrosDestaqueViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLetraria.Data;

namespace ProjetoLetraria.ViewComponents
{
    public class LivrosDestaqueViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public LivrosDestaqueViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var livros = await _context.Livros
                .OrderByDescending(x => x.DataCadastro)
                .Take(6)
                .ToListAsync();

            return View(livros);
        }
    }
}