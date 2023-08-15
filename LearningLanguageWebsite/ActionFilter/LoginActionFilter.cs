﻿using LearningLanguageWebsite.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningLanguageWebsite.ActionFilter
{
    public class LoginActionFilter : IAsyncActionFilter
    {
        private IUserAuthentication _userAuthentication;

        public LoginActionFilter(IUserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = (string)context.HttpContext.GetRouteValue("action");
            if (action != "ConfirmEmail")
            {
                var account = await _userAuthentication.GetAuthenticatedUser(context.HttpContext);
                if (account != null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" })) { Permanent = false };
                    return;
                }
            }

            await next();
        }
    }
}
