using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IUserService _userService;
        private IEmailService _emailService;
        private ILogger<UserRegistrationController> _logger;

        public UserRegistrationController(IUserService userService, IEmailService emailService, ILogger<UserRegistrationController> logger)
        {
            _userService = userService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.RegisterUser(userModel, true);

                return RedirectToAction(nameof(EmailConfirmation), new { userModel.Email });
            }
            else
            {
                return View(userModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string email)
        {
            _logger.LogInformation($"##Start## Email confirmation process for {email}");

            var user = await _userService.GetUserByEmail(email);

            var urlAction = new UrlActionContext {
                 Action = "ConfirmEmail",
                 Controller = "UserRegistration",
                 Values = new { email, code = await _userService.GetEmailConfirmationCode(user) },
                 Protocol = Request.Scheme,
                 Host = Request.Host.ToString()
            };

            var userRegistrationEmail = new UserRegistrationEmailModel { DisplayName = $"{user.FirstName} {user.LastName}", Email = email, ActionUrl = Url.Action(urlAction) };

            var emailRenderService = HttpContext.RequestServices.GetService<IEmailTemplateRenderService>();

            var message = await emailRenderService.RenderTemplate("EmailTemplates/UserRegistrationEmail", userRegistrationEmail, Request.Host.ToString());

            try
            {
                _emailService.SendEmail(email, "Tic-Tac-Toe Email Confirmation", message).Wait();
            }
            catch (Exception) {

            }

            ViewBag.Email = email;

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var confirmed = await _userService.ConfirmEmail(email, code);

            if (!confirmed) return BadRequest();

            return RedirectToAction("Index", "Home");
        }
    }
}
