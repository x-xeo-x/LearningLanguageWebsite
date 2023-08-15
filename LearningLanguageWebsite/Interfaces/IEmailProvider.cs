namespace LearningLanguageWebsite.Interfaces
{
	public interface IEmailProvider
	{
		public void SendEmail(string to, string subject, string content);
	}
}
