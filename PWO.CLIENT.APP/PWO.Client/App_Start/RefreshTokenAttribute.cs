using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PWO.Client.App_Start
{
    public class RefreshTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apiAccessToken = filterContext.HttpContext.Session["ApiAccessToken"] as string;
            if (string.IsNullOrEmpty(apiAccessToken))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Login"
                }));
                return;
            }


            base.OnActionExecuting(filterContext);
        }
    }
}