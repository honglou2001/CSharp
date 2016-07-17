using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using Zayko.Dialogs.UnhandledExceptionDlg;
using MutipleClient;
using IntDevs.SocketClient;


namespace IntDevs.Upgrade
{
    static class Program
    {
        public static log4net.ILog _logALL = null;
        public static string assemblyFilePath = "";
        public static string assemblyDirPath = "";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            string configFilePath = assemblyDirPath + "\\log4net.config";
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));

            _logALL = log4net.LogManager.GetLogger("RNCloud.LogAll");

            // Create new instance of UnhandledExceptionDlg:
            UnhandledExceptionDlg exDlg = new UnhandledExceptionDlg();

            // Uncheck "Restart App" check box by default:
            exDlg.RestartApp = false;

            // Add handling of OnShowErrorReport.
            // If you skip this then link to report details won't be showing.
            exDlg.OnShowErrorReport += delegate(object sender, SendExceptionClickEventArgs ar)
            {
                System.Windows.Forms.MessageBox.Show("Handle OnShowErrorReport event to show what you are going to send.\n" +
                    "For example:\n" + ar.UnhandledException.Message + "\n" + ar.UnhandledException.StackTrace +
                    "\n" + (ar.RestartApp ? "This App will be restarted." : "This App will be terminated!"));

                _logALL.Error("Handle OnShowErrorReport event to show what you are going to send.\n" +
                    "For example:\n" + ar.UnhandledException.Message + "\n" + ar.UnhandledException.StackTrace +
                    "\n" + (ar.RestartApp ? "This App will be restarted." : "This App will be terminated!"));


            };

            // Implement your sending protocol here. You can use any information from System.Exception
            exDlg.OnSendExceptionClick += delegate(object sender, SendExceptionClickEventArgs ar)
            {
                // User clicked on "Send Error Report" button:
                if (ar.SendExceptionDetails)
                    System.Windows.Forms.MessageBox.Show(String.Format("Implement your communication part here " +
                        "(do HTTP POST or send e-mail, for example).\nExample:\nError Message: {0}\r" +
                        "Stack Trace:\n{1}",
                        ar.UnhandledException.Message, ar.UnhandledException.StackTrace));


                _logALL.Error(String.Format("Implement your communication part here " +
                        "(do HTTP POST or send e-mail, for example).\nExample:\nError Message: {0}\r" +
                        "Stack Trace:\n{1}",
                        ar.UnhandledException.Message, ar.UnhandledException.StackTrace));

                // User wants to restart the App:
                if (ar.RestartApp)
                {
                    Console.WriteLine("The App will be restarted...");
                    System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath);
                }
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #if (upgrade)

              Application.Run(new FrmUpgrade());

#elif (upgradesocket)

              Application.Run(new FrmUpgradeSocket());

#elif (upic)

               Application.Run(new FrmUpic());

#elif (listen)
                ConfigurationFile.APPMode = 2;
                Application.Run(new FrmUpgrade());

#elif (descrypt)

                Application.Run(new FrmDESCrypt());

#else

            Application.Run(new FrmClient());

#endif

            //Application.Run(new FrmUpgrade());

            //FrmLogin Login = new FrmLogin();  
            //Login.ShowDialog();//显示登陆窗体   
            //if (Login.DialogResult == DialogResult.OK)
            //{
            //    if (Login.Ftype == 2 || Login.Ftype == 3)
            //    {
            //        Application.Run(new FrmUpgrade());//判断登陆成功时主进程显示主窗口   
            //    }
            //    else if (Login.Ftype == 1)
            //    {
            //        Application.Run(new FrmUpic());//判断登陆成功时主进程显示主窗口   
            //    }
            //}
            //else return;  



        }

        static void CustomThreadException()
        {
            throw new ApplicationException("\nSeparate Thread Exception raised!");
        }

        static unsafe byte CustomUnmanagedException()
        {
            byte* buf = null;
            return buf[0];
        }
    }
}
