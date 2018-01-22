using System.Web.Mvc;
using GenericPayment.Utilities;

namespace GenericPayment.Controllers
{
    public class AuthUser
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Authenticate(AuthUser user)
        {
            string message = "";
            string redirectUrl = Url.Action("Admin", "NganLuong");

            message = IsValidUser(user);
            if (string.IsNullOrEmpty(message))
            {
                SessionManager.GetInstance().User = new GatewayUser() { Username = user.username };
            }

            return Json(new { result = message, url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            SessionManager.GetInstance().User = null;
            return RedirectToAction("Login");
        }

        private string IsValidUser(AuthUser user)
        {
            if (string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password))
            {
                return "Missing username or password.";
            }

            if (!user.username.Equals(ConfigCode.GetInstance().AdminUser) ||
                !user.password.Equals(ConfigCode.GetInstance().AdminPassword))
            {
                return "Wrong username or password.";
            }

            return string.Empty;
        }
    }
}
