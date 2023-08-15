using LearningLanguageWebsite.Dto;
using LearningLanguageWebsite.Interfaces;
using LearningLanguageWebsite.Utility;
using MongoDB.Driver;

namespace LearningLanguageWebsite.Services
{
	public class UserAuthenticationService : IUserAuthentication
	{
		private DatabaseService _LLWService;
		private IPasswordHasher _passwordHasher;
		private IAccountRepository _accountRepository;

		public UserAuthenticationService(DatabaseService LLWService, IPasswordHasher passwordHasher, IAccountRepository accountRepository)
		{
			_LLWService = LLWService;
			_passwordHasher = passwordHasher;
			_accountRepository = accountRepository;
		}

		public async Task AuthorizeForUser(HttpContext context, string accountId, bool permanent)
		{
			var accounts = _LLWService.GetAccountsCollection();
			var account = await (await accounts.FindAsync(x => x.Id == accountId)).FirstOrDefaultAsync();
			if (account == null)
				return;

			if (permanent)
			{
				context.Response.Cookies.Append("deviceKey", Randomizer.RandomString(100), new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });
			}
			context.Session.SetString("userId", accountId);
		}

		public async Task LogoutUser(HttpContext context)
		{
			context.Session.Remove("userId");
			context.Response.Cookies.Delete("deviceKey");
		}

		public bool CheckCredentials(AccountDTO account, string password)
		{
			return _passwordHasher.Check(account.Password, password);
		}

		public async Task<AccountDTO> GetAuthenticatedUser(HttpContext context)
		{
			var userId = context.Session.GetString("userId");
			if (string.IsNullOrEmpty(userId))
			{
				return null;
			}

			var account = await _accountRepository.GetAccount(userId);

			if (account == null)
			{
				context.Session.Remove("userId");
				context.Response.Cookies.Delete("deviceKey");
			}

			return account;
		}
	}
}
