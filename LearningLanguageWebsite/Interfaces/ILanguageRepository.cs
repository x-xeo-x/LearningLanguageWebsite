using LearningLanguageWebsite.Dto;

namespace LearningLanguageWebsite.Interfaces
{
    public interface ILanguageRepository
    {
        public Task<LanguageDTO> AddLanguage(string language);
        public Task<List<LanguageDTO>> GetLanguages();
    }
}
