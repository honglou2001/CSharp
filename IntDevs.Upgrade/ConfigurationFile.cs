using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace IntDevs.Upgrade
{
    public class ConfigurationFile
    {
        public static readonly string ComConfig = ConfigurationManager.AppSettings["COMConfig"];
        public static readonly string VerConfig = ConfigurationManager.AppSettings["VerConfig"];
        public static readonly string BaudRate = ConfigurationManager.AppSettings["BaudRate"];
        public static readonly string ServerIP = ConfigurationManager.AppSettings["ServerIP"];
        public static readonly string ServerPort = ConfigurationManager.AppSettings["ServerPort"];
        //升级模式：1；秘盒监听模式:2
        public static int APPMode = 1;
        public static int CurrentCount = int.Parse(ConfigurationManager.AppSettings["CurrentCount"].ToString());

        public static readonly string DeviceType = ConfigurationManager.AppSettings["DeviceType"];
        public static readonly string DeviceVer = ConfigurationManager.AppSettings["DeviceVer"];
        public static string keystr = "keyr2014";

        /// <summary>
        /// 是否显示提示异常信息对话框，默认显示
        /// </summary>
        public static bool bShowDialog = true;
        
        /// <summary>
        /// 1为一条条确认后发，2为随机发
        /// </summary>
        public static readonly int RunMode = int.Parse(ConfigurationManager.AppSettings["RunMode"].ToString());

        public static void UpdateVal(string key, string val)
        {
            try
            {
                //System.Configuration.ConfigurationManager.AppSettings["WebDAL"] = "ef";
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings[key].Value = val;
                cfa.Save();
            }
            catch (System.Exception ex)
            {
                Program._logALL.Error(ex.ToString());
            }

        }


        public static string GetKeyVal(string key)
        {
            string sVal = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrEmpty(sVal))
            {
                return sVal;
            }

            return string.Empty;
        }
    
    }

    public interface IFrameBuilder
    {
        IFrameEncoder Encoder { get; }
        IFrameDecoder Decoder { get; }
    }

    public interface IFrameEncoder
    {
        void EncodeFrame(byte[] payload, int offset, int count, out byte[] frameBuffer, out int frameBufferOffset, out int frameBufferLength);
    }

    public interface IFrameDecoder
    {
        bool TryDecodeFrame(byte[] buffer, int offset, int count, out int frameLength, out byte[] payload, out int payloadOffset, out int payloadCount);
    }

    internal static class TplExtensions
    {
        public static void Forget(this Task task) { }
    }
}
