using System;
using System.Collections.Generic;
using System.Text;

namespace MutipleClient
{
   public class Tools
    {
       public static byte[] HexStringToByteArray(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            //16进制字符串转化为字节数组
            s = s.ToLower().Replace("0x", "").Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
       //public static string ByteToHexStr(byte[] bytes)
       // {
       //     string returnStr = "";
       //     returnStr = BitConverter.ToString(bytes);
       //     returnStr = returnStr.Replace("-", "");
       //     return returnStr;
       // }

       public static string ByteToHexStr(byte[] bytes)
       {
           string returnStr = "";
           returnStr = BitConverter.ToString(bytes);
           returnStr = returnStr.Replace("-", " ");
           return returnStr;
       }


        public static int CRC_XModem(byte[] bytes)
        {
            int crc = 0x00; // initial value
            int polynomial = 0x1021;
            for (int index = 0; index < bytes.Length; index++)
            {
                byte b = bytes[index];
                for (int i = 0; i < 8; i++)
                {
                    Boolean bit = ((b >> (7 - i) & 1) == 1);
                    Boolean c15 = ((crc >> 15 & 1) == 1);
                    crc <<= 1;
                    if (c15 ^ bit)
                        crc ^= polynomial;
                }
            }
            crc &= 0xffff;
            return crc;
        }

    }
}
