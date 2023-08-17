using LearningLanguageWebsite.Dto;
using LearningLanguageWebsite.Interfaces;
using MongoDB.Driver;

namespace LearningLanguageWebsite.Services
{
    public class LanguageRepositoryService : ILanguageRepository
    {
        private readonly DatabaseService _databaseService;

        public LanguageRepositoryService(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public async Task<LanguageDTO> AddLanguage(string language)
        {
            var dto = new LanguageDTO();
            dto.Language = language;

            await _databaseService.GetLanguagesCollection().InsertOneAsync(dto);

            return dto;
        }

        public async Task<List<LanguageDTO>> GetLanguages()
        {
            return await _databaseService.GetLanguagesAsync();
        }
    }
}
