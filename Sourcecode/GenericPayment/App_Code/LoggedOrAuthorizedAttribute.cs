using System.Web.Mvc;
using GenericPayment.Utilities;

namespace GenericPayment
{
    public class LoggedOrAuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            CheckIfUserIsAuthenticated(filterContext);
        }

        private void CheckIfUserIsAuthenticated(ActionExecutingContext filterContext)
        {
            if (SessionManager.GetInstance().User == null || string.IsNullOrEmpty(SessionManager.GetInstance().User.Username))
            {
                var result = new RedirectResult("~/User/Login");
                filterContext.Result = result;
            }
        }
    }
}