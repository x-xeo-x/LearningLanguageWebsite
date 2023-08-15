using Microsoft.AspNetCore.Mvc;

namespace LearningLanguageWebsite.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
