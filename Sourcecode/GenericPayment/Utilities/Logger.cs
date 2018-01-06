using System;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GenericPayment.Utilities
{
    public class Logger
    {
        #region Static Properties
        private static readonly Logger Instance = new Logger();
        public static Logger GetInstance()
        {
            return Instance;
        }
        #endregion

        private readonly ILog _log;

        private Logger()
        {
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        }

        public void Write(string message)
        {
            _log.Info(message);
        }

        public void Write(Exception ex, string message)
        {
            _log.Error(message, ex);
        }
    }
}