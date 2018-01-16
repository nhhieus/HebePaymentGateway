using System;
using System.Web.Mvc;

namespace GenericPayment
{
    public class LoggedOrAuthorizedAttribute : AuthorizeAttribute
    {
        public LoggedOrAuthorizedAttribute()
        {
            View = "error";
            Master = String.Empty;
        }

        public String View { get; set; }
        public String Master { get; set; } 

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            CheckIfUserIsAuthenticated(filterContext);
        }

        private void CheckIfUserIsAuthenticated(AuthorizationContext filterContext)
        {
            // If Result is null, we're OK: the user is authenticated and authorized. 
            if (SessionManager.GetInstance().User != null)
                return;

            // If here, you're getting an HTTP 401 status code. In particular,
            // filterContext.Result is of HttpUnauthorizedResult type. Check Ajax here. 
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(View))
                    return;
                var result = new ViewResult { ViewName = View, MasterName = Master };
                filterContext.Result = result;
            }
        }
    }
}