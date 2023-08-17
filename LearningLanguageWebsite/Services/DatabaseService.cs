using LearningLanguageWebsite.Dto;
using MongoDB.Driver;

namespace LearningLanguageWebsite.Services
{
	public class DatabaseService
	{
        private readonly MongoClient _client;
        private readonly IMongoDatabase _mongoDatabase;

        private readonly IMongoCollection<AccountDTO> _accountsCollection;
		private readonly IMongoCollection<PasswordResetDTO> _passwordResetsCollection;
		private readonly IMongoCollection<LanguageDTO> _languagesCollection;
        private readonly IMongoCollection<DeviceDTO> _devicesCollection;
        private readonly IMongoCollection<EmailConfirmationDTO> _emailConfirmationsCollection;
        public MongoClient MongoClient => _client;

        public DatabaseService(IConfiguration configuration)
		{
            var mongoConfig = configuration.GetSection("Mongo");
            var connectionString = mongoConfig["ConnectionString"];
            var databaseName = mongoConfig["DatabaseName"];

            _client = new MongoClient(connectionString);
            _mongoDatabase = _client.GetDatabase(databaseName);

            _accountsCollection = _mongoDatabase.GetCollection<AccountDTO>("accounts");
            _passwordResetsCollection = _mongoDatabase.GetCollection<PasswordResetDTO>("password_resets");
            _languagesCollection = _mongoDatabase.GetCollection<LanguageDTO>("languages");
            _devicesCollection = _mongoDatabase.GetCollection<DeviceDTO>("devices");
            _emailConfirmationsCollection = _mongoDatabase.GetCollection<EmailConfirmationDTO>("email_confirmations");
        }

		public MongoClient GetMongoClient()
		{
			return _client;
		}
        public IMongoCollection<EmailConfirmationDTO> GetEmailConfirmationsCollection()
        {
            return _emailConfirmationsCollection;
        }
        public IMongoCollection<DeviceDTO> GetDevicesCollection()
        {
            return _devicesCollection;
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
            var filter = Builders<LanguageDTO>.Filter.Empty;
            var sort = Builders<LanguageDTO>.Sort.Ascending(x => x.Id);
            var options = new FindOptions<LanguageDTO> { Sort = sort };

            return await (await _languagesCollection.FindAsync(filter, options)).ToListAsync();
        }
    }
}
