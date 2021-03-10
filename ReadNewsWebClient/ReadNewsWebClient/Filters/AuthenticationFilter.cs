using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebClient.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //check user is login or not
            var usernameSession = filterContext.HttpContext.Session["username"];
            if (usernameSession == null)
            {
                filterContext.Result = new RedirectResult("/Account/login");
            }
        }
    }
}