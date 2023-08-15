using LearningLanguageWebsite.Dto;
using MongoDB.Driver;

namespace LearningLanguageWebsite.Services
{
	public class DatabaseService
	{
		private readonly IMongoCollection<AccountDTO> _accountsCollection;
		private readonly IMongoCollection<PasswordResetDTO> _passwordResetsCollection;
		private readonly IMongoCollection<LanguageDTO> _languagesCollection;

		private MongoClient _client;

		public DatabaseService(IConfiguration configuration)
		{
			_client = new MongoClient(configuration["Mongo:ConnectionString"]);
			var mongoDatabase = _client.GetDatabase(configuration["Mongo:DatabaseName"]);

			_accountsCollection = mongoDatabase.GetCollection<AccountDTO>("accounts");
			_passwordResetsCollection = mongoDatabase.GetCollection<PasswordResetDTO>("password_resets");
            _languagesCollection = mongoDatabase.GetCollection<LanguageDTO>("languages");
		}

		public MongoClient GetMongoClient()
		{
			return _client;
		}

		public IMongoCollection<AccountDTO> GetAccountsCollection()
		{
			return _accountsCollection;
		}

		public IMongoCollection<PasswordResetDTO> GetPasswordResetsCollection()
		{
			return _passwordResetsCollection;
		}

        public IMongoCollection<LanguageDTO> GetLanguagesCollection()
        {
            return _languagesCollection;
        }

        public async Task<List<LanguageDTO>> GetLanguagesAsync()
		{
            return await (await _languagesCollection.FindAsync(Builders<LanguageDTO>.Filter.Empty, new FindOptions<LanguageDTO>() { Sort = Builders<LanguageDTO>.Sort.Ascending(x => x.Id) })).ToListAsync();
        }
    }
}
