using LearningLanguageWebsite.Dto;
using LearningLanguageWebsite.Interfaces;
using LearningLanguageWebsite.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LearningLanguageWebsite.Services
{
	public class AccountRepositoryService : IAccountRepository
	{
		private DatabaseService _LLWService;
		private IPasswordHasher _passwordHasher;
		private IEmailProvider _emailProvider;

		private Collation _ignoreCaseCollation;

		public AccountRepositoryService(DatabaseService LLWService, IPasswordHasher passwordHasher, IEmailProvider emailProvider)
		{
			_LLWService = LLWService;
			_passwordHasher = passwordHasher;
			_emailProvider = emailProvider;

			_ignoreCaseCollation = new Collation("en", strength: CollationStrength.Secondary);
		}

		public async Task<bool> AccountExists(string email, string username)
		{
			var accounts = _LLWService.GetAccountsCollection();
			var builder = Builders<AccountDTO>.Filter;
			var filter = builder.Empty;

			if (!string.IsNullOrEmpty(email))
			{
				filter = builder.Eq(x => x.Email, email);
				if (!string.IsNullOrEmpty(username))
					filter |= builder.Eq(x => x.Username, username);
			}
			else if (!string.IsNullOrEmpty(username))
			{
				filter = builder.Eq(x => x.Username, username);
			}

			return await(await accounts.FindAsync(filter, new FindOptions<AccountDTO>() { Collation = _ignoreCaseCollation })).FirstOrDefaultAsync() != null;
		}

		public async Task<AccountDTO> CreateAccount(string email, string username, string password, List<string> languagesList, bool confirmEmail = true)
		{
			var account = new AccountDTO() { Email = email, Username = username, Password = _passwordHasher.Hash(password), CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), IsAdmin = false, LangueageId = languagesList, CardsId = new List<string>(), ArticlesId = new List<string>() };
			var accounts = _LLWService.GetAccountsCollection();

			await accounts.InsertOneAsync(account);

			return account;
		}

		public async Task ChangePassword(AccountDTO account, string password, bool logout)
		{
			var accounts = _LLWService.GetAccountsCollection();

			if (logout)
			{
				await accounts.UpdateOneAsync(x => x.Id == account.Id, Builders<AccountDTO>.Update.Set(x => x.Password, _passwordHasher.Hash(password)));
				return;
			}

			await accounts.UpdateOneAsync(x => x.Id == account.Id, Builders<AccountDTO>.Update.Set(x => x.Password, _passwordHasher.Hash(password)));
		}

		public async Task ChangeUsername(AccountDTO account, string username)
		{
			var accounts = _LLWService.GetAccountsCollection();
			await accounts.UpdateOneAsync(x => x.Id == account.Id, Builders<AccountDTO>.Update.Set(x => x.Username, username));
		}

		public async Task DeleteAccount(AccountDTO account)
		{
			var accounts = _LLWService.GetAccountsCollection();

			await accounts.DeleteOneAsync(x => x.Id == account.Id);
		}

		public async Task<AccountDTO> GetAccount(string accountId)
		{
			if (string.IsNullOrEmpty(accountId) || !ObjectId.TryParse(accountId, out _))
				return null;

			var accounts = _LLWService.GetAccountsCollection();
			return await(await accounts.FindAsync(x => x.Id == accountId)).FirstOrDefaultAsync();
		}

		public async Task<AccountDTO> GetAccountByEmail(string email)
		{
			var accounts = _LLWService.GetAccountsCollection();
			return await (await accounts.FindAsync(x => x.Email == email, new FindOptions<AccountDTO>() { Collation = _ignoreCaseCollation })).FirstOrDefaultAsync();
		}

		public async Task SendPasswordResetRequest(AccountDTO account, IUrlHelper Url)
		{
			var resets = _LLWService.GetPasswordResetsCollection();
			var resetPassword = new PasswordResetDTO() { AccountId = account.Id, CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), Used = false, Key = Randomizer.RandomString(125) };
			await resets.InsertOneAsync(resetPassword);

			_emailProvider.SendEmail(account.Email, "Hello", "Testing some Mailgun awesomness!");
		}

		public async Task SendConfirmationEmail(AccountDTO account, IUrlHelper Url)
		{
			_emailProvider.SendEmail(account.Email, "Hello", "Testing some Mailgun awesomness!");
		}
	}
}
