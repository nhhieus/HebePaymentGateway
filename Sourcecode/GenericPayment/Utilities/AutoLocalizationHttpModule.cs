using System;
using System.Globalization;
using System.Threading;
using System.Web;

namespace GenericPayment.Utilities
{
    public class AutoLocalizationHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += OnAcquireRequestState;
        }

        private void OnAcquireRequestState(object sender, EventArgs e)
        {
            var culture = SessionManager.GetInstance().Language;
            try
            {

                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.Calendar = new GregorianCalendar();
            }
            catch (CultureNotFoundException exception)
            {
                Logger.GetInstance().Write(exception, string.Format("CultureCode {0} not found.", culture));
            }
        }

        public void Dispose()
        {
        }
    }
}
