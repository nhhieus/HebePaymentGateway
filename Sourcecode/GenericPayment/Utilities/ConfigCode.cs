using System.Web;
using System.Web.Configuration;

namespace GenericPayment.Utilities
{
    public class ConfigCode
    {
        private static readonly ConfigCode Instance = new ConfigCode();
        public static ConfigCode GetInstance()
        {
            return Instance;
        }

        private ConfigCode()
        {
        }

        public string DatabasePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/AppData/Database");
            }
        }

        public string MarketPlaceUrl
        {
            get { return WebConfigurationManager.AppSettings["MarketPlaceUrl"]; }
        }

        public string NganLuongApiUrl
        {
            get { return WebConfigurationManager.AppSettings["NganLuongApiUrl"]; }
        }

        public string MerchantID
        {
            get { return WebConfigurationManager.AppSettings["MerchantID"]; }
        }

        public string MerchantPassword
        {
            get { return WebConfigurationManager.AppSettings["MerchantPassword"]; }
        }

        public string ReceiverEmail
        {
            get { return WebConfigurationManager.AppSettings["ReceiverEmail"]; }
        }

        public string AdminUser
        {
            get { return WebConfigurationManager.AppSettings["AdminUser"]; }
        }

        public string AdminPassword
        {
            get { return WebConfigurationManager.AppSettings["AdminPassword"]; }
        }
    }
}