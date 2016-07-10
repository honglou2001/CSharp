using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevs.Upgrade
{
    public static class LogHelper
    {
        private static readonly log4net.ILog _logWarn ;
        private static readonly log4net.ILog _logFile ;
        private static readonly log4net.ILog _logALL ;
        private static readonly log4net.ILog _logMsg ;
        

        static LogHelper()
        {
            _logWarn = log4net.LogManager.GetLogger("RNCloud.LogWarn");
            _logFile = log4net.LogManager.GetLogger("RNCloud.LogDebug");
            _logALL = log4net.LogManager.GetLogger("RNCloud.LogAll");
            _logMsg = log4net.LogManager.GetLogger("MsgLogger");
        }

        public static void InfoFormat(string format, params object[] args)
        {
            _logALL.InfoFormat(format, args);
        }

        public static void Info(object message)
        {
            _logALL.Info(message);
        }

        public static void InfoMsg(object message)
        {
            _logMsg.Info(message);
        }

        public static void Error(object message, Exception exception)
        {
            _logALL.Error(message, exception);
        }
    }
}
