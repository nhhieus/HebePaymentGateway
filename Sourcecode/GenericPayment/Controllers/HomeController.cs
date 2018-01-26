using System;
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
            try
            {
                SessionManager.GetInstance().Language = lang;

                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex, string.Format("[Language={0}] Exception thrown in ChangeLanguage", lang));
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}