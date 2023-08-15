using System.ComponentModel.DataAnnotations;

namespace LearningLanguageWebsite.Models
{
	public class RequestPasswordResetModel
	{
		[Required]
		[EmailAddress]
		[StringLength(64)]
		public string Email { get; set; }
	}
}
