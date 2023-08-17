using LearningLanguageWebsite.ActionFilter;
using LearningLanguageWebsite.Dto;
using LearningLanguageWebsite.Interfaces;
using LearningLanguageWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LearningLanguageWebsite.Controllers
{
    [TypeFilter(typeof(HomeActionFilter))]
    public class HomeController : Controller
    {
        private readonly IUserAuthentication _userAuthentication;
        private readonly IAccountRepository _accountRepository;

        public HomeController(IUserAuthentication userAuthentication, IAccountRepository accountRepository)
        {
            _userAuthentication = userAuthentication;
            _accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userAuthentication.LogoutUser(HttpContext);

            return new RedirectResult(Url.Action("Index", "Login"), false);
        }

        [ValidateAntiForgeryToken]
        [HttpGet]
        public async Task<IActionResult> ResendEmail()
        {
            var account = HttpContext.Items["userAccount"] as AccountDTO;

            if (account.EmailConfirmed)
                return Json(new { error = "already_confirmed" });

            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (currentTime - account.LastEmailConfirmSend < 900)
                return Json(new { error = "email_too_fast" });

            await _accountRepository.UpdateLastEmailConfirmSend(account, currentTime);
            await _accountRepository.SendConfirmationEmail(account, Url);

            return Json(new { success = "email_sended" });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}