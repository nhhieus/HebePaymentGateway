using System;
using System.Web;

namespace GenericPayment.Utilities
{
    public class SessionManager
    {
        private static readonly SessionManager Instance = new SessionManager();
        public static SessionManager GetInstance()
        {
            return Instance;
        }

        private string Key_User = "user";

        public GatewayUser User
        {
            get
            {
                if (HttpContext.Current.Session[Key_User] != null)
                    return HttpContext.Current.Session[Key_User] as GatewayUser;
                return null;
            }
            set { HttpContext.Current.Session[Key_User] = value; }
        }

        private string Key_Language = "Language";

        public string Language
        {
            get
            {
                if (HttpContext.Current.Session == null)
                {
                    return ConfigCode.DefaultLanguage;
                }

                if (HttpContext.Current.Session[Key_Language] == null)
                {
                    HttpContext.Current.Session[Key_Language] = ConfigCode.DefaultLanguage;
                }

                return HttpContext.Current.Session[Key_Language] as string;
            }
            set { HttpContext.Current.Session[Key_Language] = value; }
        }
    }

    [Serializable]
    public class GatewayUser
    {
        public string Username;
        public string Role;
    }
}