using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Authorize]
    public class LeaderBoardController : Controller
    {
        private IUserService _userservice;

        public LeaderBoardController(IUserService service)
        {
            _userservice = service;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _userservice.GetTopUsers(10);

            return View(users);
        }
    }
}
