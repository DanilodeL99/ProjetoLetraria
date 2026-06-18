// Controllers/SistemaController.cs
using Microsoft.AspNetCore.Mvc;

namespace ProjetoLetraria.Controllers
{
    public class SistemaController : Controller
    {
        [HttpGet]
        public IActionResult Configuracoes()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlternarTema()
        {
            var temaAtual = Request.Cookies["tema"];
            var novoTema = temaAtual == "light" ? "dark" : "light";

            Response.Cookies.Append(
                "tema",
                novoTema,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                });

            return RedirectToAction(nameof(Configuracoes));
        }
    }
}