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
        private readonly ILanguageRepository _languageRepository;

        public LoginController(
            IAccountRepository accountRepository,
            IUserAuthentication userAuthentication,
            ILanguageRepository languageRepository)
        {
            _accountRepository = accountRepository;
            _userAuthentication = userAuthentication;
            _languageRepository = languageRepository;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdateNavBar()
        {
            ActionResult actionResult = View();
            return View(actionResult);
        }

        public async Task<IActionResult> Register()
		{
			var model = new LanguagesViewModel();
            model.Languages = await _languageRepository.GetLanguages();

            return View(model.Languages);
		}

        private IActionResult CheckModelState()
        {
            if (ModelState.IsValid)
            {
                return null;
            }

            return Json(new { error = "invalid_model_register" });
        }

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> LoginAccount([FromBody] LoginAccountModel model)
		{
            var modelStateResult = CheckModelState();
            if (modelStateResult != null)
            {
                return modelStateResult;
            }

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

            var modelStateResult = CheckModelState();
            if (modelStateResult != null)
            {
                return modelStateResult;
            }

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
            var modelStateResult = CheckModelState();
            if (modelStateResult != null)
            {
                return modelStateResult;
            }

            var account = await _accountRepository.GetAccountByEmail(model.Email);
            if (account != null)
            {
                var currenTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (currenTime - account.LastEmailPasswordSend >= (60 * 30))
                {
                    await _accountRepository.UpdateLastEmailPasswordSend(account, currenTime);
                    await _accountRepository.SendPasswordResetRequest(account, Url);
                }
            }

            return Json(new { success = "password_reset_success" });
		}

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string key)
        {
            var result = await _accountRepository.TryConfirmEmail(key);

            if (result.Item1 == UserRequestConfrimStatus.WrongKey)
                return new RedirectResult(Url.Action("Index", "Login", new { error = "wrong_email_key" }), false);

            if (result.Item1 != UserRequestConfrimStatus.Confirmed)
                return new RedirectResult(Url.Action("Index", "Login"), false);

            var currentUser = await _userAuthentication.GetAuthenticatedUser(HttpContext);
            if (currentUser == null)
                await _userAuthentication.AuthorizeForUser(HttpContext, result.Item2, true);

            return new RedirectResult(Url.Action("Index", "Home", new { success = "email_confirmed" }), false);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string key)
        {
            var result = await _accountRepository.TryConfirmPasswordReset(key);
            if (result.Item1 == UserRequestConfrimStatus.WrongKey)
                return new RedirectResult(Url.Action("Index", "Login", new { error = "wrong_email_key" }), false);

            if (result.Item1 != UserRequestConfrimStatus.Confirmed)
                return new RedirectResult(Url.Action("Index", "Login"), false);

            await _userAuthentication.AuthorizeForUser(HttpContext, result.Item2, true);

            return new RedirectResult(Url.Action("Index", "Home", new { success = "password_reseted" }), false);
        }
    }
}
