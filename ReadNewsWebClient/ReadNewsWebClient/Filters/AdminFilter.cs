using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebClient.Filters
{
    public class AdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //check user is login or not
            var role = filterContext.HttpContext.Session["role"];
            if (role == null || !role.Equals("Admin"))
            {
                filterContext.Result = new RedirectResult("/Account/login");
            }
        }
    }
}