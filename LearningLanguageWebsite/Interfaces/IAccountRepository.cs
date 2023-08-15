using LearningLanguageWebsite.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LearningLanguageWebsite.Interfaces
{
	public interface IAccountRepository
	{
		public Task<bool> AccountExists(string email, string username);
		public Task<AccountDTO> CreateAccount(string email, string username, string password, List<string> languagesList, bool confirmEmail = true);
		public Task ChangePassword(AccountDTO account, string password, bool logout);
		public Task<AccountDTO> GetAccount(string accountId);
		public Task<AccountDTO> GetAccountByEmail(string email);
		public Task DeleteAccount(AccountDTO account);
		public Task ChangeUsername(AccountDTO account, string username);
		public Task SendPasswordResetRequest(AccountDTO account, IUrlHelper Url);
		public Task SendConfirmationEmail(AccountDTO account, IUrlHelper Url);
	}
}
