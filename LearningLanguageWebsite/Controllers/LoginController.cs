using LearningLanguageWebsite.ActionFilter;
using LearningLanguageWebsite.Dto;
using LearningLanguageWebsite.Interfaces;
using LearningLanguageWebsite.Models;
using LearningLanguageWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;

namespace LearningLanguageWebsite.Controllers
{
    [TypeFilter(typeof(LoginActionFilter))]
    public class LoginController : Controller
    {
		private readonly IAccountRepository _accountRepository;
		private readonly IUserAuthentication _userAuthentication;
        private DatabaseService _LLWService;

        public LoginController(IAccountRepository accountRepository, IUserAuthentication userAuthentication, DatabaseService LLWService)
		{
			_accountRepository = accountRepository;
			_userAuthentication = userAuthentication;
            _LLWService = LLWService;
		}

		public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> Register()
		{
			var model = new LanguagesViewModel();
			model.Languages = await _LLWService.GetLanguagesAsync();

            return View(model.Languages);
		}
        public IActionResult UpdateNavBar()
        {
            ActionResult actionResult = View();
            return View(actionResult);
        }

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> LoginAccount([FromBody] LoginAccountModel model)
		{
            if (!ModelState.IsValid)
				return Json(new { error = "invalid_model_register" });

			var account = await _accountRepository.GetAccountByEmail(model.Email);

			if (account == null || !_userAuthentication.CheckCredentials(account, model.Password))
				return Json(new { error = "invalid_credentials" });

			await _userAuthentication.AuthorizeForUser(HttpContext, account.Id, model.RememberMe);

			return Json(new { success = "login_success" });
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountModel model)
		{
			model.Email = model.Email.Trim();

			if (!ModelState.IsValid)
				return Json(new { error = "invalid_model_register" });

			var accountExists = await _accountRepository.AccountExists(model.Email, model.Username);
			if (accountExists)
				return Json(new { error = "account_exists" });

			var account = await _accountRepository.CreateAccount(model.Email, model.Username, model.Password, model.LanguageId);
			await _accountRepository.SendConfirmationEmail(account, Url);

			return Json(new { success = "account_created" });
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetModel model)
		{
			if (!ModelState.IsValid)
				return Json(new { error = "invalid_model_register" });

			var account = await _accountRepository.GetAccountByEmail(model.Email);
			if (account != null)
			{
				var currenTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				await _accountRepository.SendPasswordResetRequest(account, Url);
			}

			return Json(new { success = "password_reset_success" });
		}
	}
}
