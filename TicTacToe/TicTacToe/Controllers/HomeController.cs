using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => {
                var culture = Request.HttpContext.Session.GetString("culture");

                ViewBag.Language = culture;

                return View();
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SetCulture(string culture)
        {
            return await Task.Run(() => {
                Request.HttpContext.Session.SetString("culture", culture);

                return RedirectToAction("Index");
            });
        }

        [Authorize(Policy = "AdministratorAccessLevelPolicy")]
        public async Task<IActionResult> SecuredPage()
        {
            return await Task.Run(() => {
                ViewBag.SecureWord = "Secured Page";
                return View("SecuredPage");
            });
        }
    }
}
