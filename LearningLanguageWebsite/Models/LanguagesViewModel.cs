using LearningLanguageWebsite.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningLanguageWebsite.Models
{
    public class LanguagesViewModel
    {
        public List<LanguageDTO> Languages { get; set; }
    }
}
