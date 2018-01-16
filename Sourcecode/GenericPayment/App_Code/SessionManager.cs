using System;
using System.Web;

namespace GenericPayment
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
    }

    [Serializable]
    public class GatewayUser
    {
        public string Username;
        public string Role;
    }
}