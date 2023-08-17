using LearningLanguageWebsite.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using LearningLanguageWebsite.Dto;

namespace LearningLanguageWebsite.ActionFilter
{
    public class HomeActionFilter : IAsyncActionFilter
    {
        private IUserAuthentication _userAuthentication;

        public HomeActionFilter(IUserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = (string)context.HttpContext.GetRouteValue("action");
            if (action == "Privacy" || action == "Index")
            {
                await next();
                return;
            }

            var account = await _userAuthentication.GetAuthenticatedUser(context.HttpContext);
            if (account == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" })) { Permanent = false };
                return;
            }

            context.HttpContext.Items["userAccount"] = account;

            if (action != "Logout" &&
                action != "ResendEmail")
            {
                if (!account.EmailConfirmed)
                {
                    context.Result = new ViewResult() { ViewName = "~/Views/Home/EmailConfirm.cshtml" };
                    return;
                }
            }

            await next();
        }
    }
}
