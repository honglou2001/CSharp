using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevs.Upgrade
{
    public static class LogHelper
    {
        private static readonly log4net.ILog _logALL ;

        static LogHelper()
        {
            _logALL = log4net.LogManager.GetLogger("RNCloud.LogAll");
        }

        public static void InfoFormat(string format, params object[] args)
        {
            _logALL.InfoFormat(format, args);
        }

        public static void Info(object message)
        {
            _logALL.Info(message);
        }

        public static void Error(object message, Exception exception)
        {
            _logALL.Error(message, exception);
        }
    }
}
