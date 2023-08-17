using LearningLanguageWebsite.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LearningLanguageWebsite.Interfaces
{
    public enum UserRequestConfrimStatus
    {
        WrongKey,
        AlreadyConfirmed,
        Confirmed
    };
    public interface IAccountRepository
	{
		public Task<bool> AccountExists(string email, string username);
		public Task<AccountDTO> CreateAccount(string email, string username, string password, List<string> languagesList, bool confirmEmail = false);
        public Task ChangePassword(AccountDTO account, string password, bool logout);
        public Task SendTempPasswordEmail(AccountDTO account, string tempPassword);
        public Task<AccountDTO> GetAccount(string accountId);
		public Task<AccountDTO> GetAccountByEmail(string email);
		public Task DeleteAccount(AccountDTO account);
		public Task ChangeUsername(AccountDTO account, string username);
		public Task SendPasswordResetRequest(AccountDTO account, IUrlHelper Url);
		public Task SendConfirmationEmail(AccountDTO account, IUrlHelper Url);
        public Task UpdateLastEmailConfirmSend(AccountDTO account, long time);
        public Task UpdateLastEmailPasswordSend(AccountDTO account, long time);
        public Task<(UserRequestConfrimStatus, string)> TryConfirmEmail(string key);
        public Task<(UserRequestConfrimStatus, string)> TryConfirmPasswordReset(string key);
    }
}
