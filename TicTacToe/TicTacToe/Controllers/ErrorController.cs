using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
