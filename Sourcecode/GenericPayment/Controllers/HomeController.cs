using System.Web.Mvc;
using GenericPayment.Utilities;

namespace GenericPayment.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("Welcome");
        }

        [HttpPost]
        public JsonResult ChangeLanguage(string lang)
        {
            SessionManager.GetInstance().Language = lang;

            return Json(new {result = true}, JsonRequestBehavior.AllowGet);
        }
    }
}