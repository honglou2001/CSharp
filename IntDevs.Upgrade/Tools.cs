using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Data;
using System.Security.Cryptography;
using System.Threading; 

namespace IntDevs.Upgrade
{
    public class Tools
    {
        private static int m1Lbuff_Szie = 256;                      //1级缓存尺寸
        private static byte[] m1L_buff = new byte[m1Lbuff_Szie];    //1级缓存
        private static int m1L_buff_len = 0;                        //1级缓存长度
        private static int m1L_buff_SP = 0;                         //1级缓存存储指针
        private static int m1L_buff_GP = 0;                         //1级缓存读取指针        

        public static string[] sysdevinfo = { "秘盒","","SF" };

        private static log4net.ILog _log = log4net.LogManager.GetLogger("RNCloud.LogWarn");

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

        //
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            returnStr = BitConverter.ToString(bytes);
            returnStr = returnStr.Replace("-", " ");
            return returnStr;
        }


        //1级缓存接收存储
        public static void btStore(byte bt)
        {
            if (m1L_buff_len < m1Lbuff_Szie)
            {
                m1L_buff[m1L_buff_SP] = bt;
                m1L_buff_SP++;
                m1L_buff_SP %= m1Lbuff_Szie;
                m1L_buff_len++;
            }
        }
        //1级缓存获取
        public static int btGet(ref byte bt)
        {
            if (m1L_buff_len > 0)
            {
                bt = m1L_buff[m1L_buff_GP];
                m1L_buff_GP++;
                m1L_buff_GP %= m1Lbuff_Szie;
                m1L_buff_len--;
                return 1;
            }
            return 0;
        }
        //crc计算
        public static int crc_s1(byte[] src, int offset, int len)
        {
            int res = 0xffff;
            int i = -1;
            int j;
            int flag;

            while (++i < len)
            {
                res ^= src[i + offset];
                j = 0x80;
                while (j > 0)
                {
                    flag = 0;
                    if ((res & 0x0001) == 0x0001)
                        flag = 1;
                    res >>= 1;
                    if (flag == 1)
                        res ^= 0xa001;
                    j >>= 1;
                }
            }
            return res;
        }

        //串口发送
        public static void WriteCommand(byte[] bytes, SerialPort port)
        {
            if (port.IsOpen)
            {
                try
                {
                    port.Write(bytes, 0, bytes.Length);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                    //_logALL.Error("WriteCommand-发送数据-" + ex.ToString());
                }
                finally
                {
                    //ReceiveEventFlag = false;      //打开事件
                }
            }
        }
        //命令组装发送
        public static void WriteCmdd(ref byte[] buff, SerialPort port)
        {
            int crc;

            byte[] bytes = { 0x5A, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFB, 0xFF, 0xFF, 0xFF, 0xFF, 0x00 };
            Array.Copy(bytes, 0, buff, 0, 16);
            buff[21 + buff[20]] = 1;//ack
            crc = crc_s1(buff, 1, 25 - 4 + buff[20]);
            buff[21 + buff[20] + 1] = (byte)(crc >> 8);
            buff[21 + buff[20] + 2] = (byte)crc;
            buff[21 + buff[20] + 3] = 0xA5;
            WriteCommand(buff, port);
        }



        /// <summary>
        /// 进行DES加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <param name="sKey">密钥，且必须8位</param>
        /// <returns>以Base64格式返回的加密字符串</returns>
        public static string DesEncrypt(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string DesDecrypt(string pToDecrypt, string sKey)
        {
            if ((pToDecrypt.Length % 4) != 0)//因为加密后是base64，所以用4来求余进行验证
            {
                return pToDecrypt;
            }
            if (pToDecrypt.Contains("Password"))//如果包含Password，表示没有加密
            {
                return pToDecrypt;
            }

            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
    
        //
        static UInt16 crc_send(byte[] Buff, int startIndex, int len)
        {
            UInt16 res = 0xffff;
            int i = -1;
            UInt16 j, flag;

            while (++i < len)
            {
                res ^= Buff[i];
                j = 0x80;
                while (j!=0)
                {
                    flag = 0;
                    if ((res & 0x0001) != 0)
                        flag = 1;
                    res >>= 1;
                    if (flag !=0)
                        res ^= 0xa001;
                    j >>= 1;
                }
            }
            return res;
        }        

        //根据设备类型获取设备名
        public static DataTable  GetComDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add(new object[] {  sysdevinfo[0],"1" });
            dt.Rows.Add(new object[] { sysdevinfo[1],"2" });
            dt.Rows.Add(new object[] { sysdevinfo[2],"3" });
            return dt;
        }           

        //创建sn数据目录
        public static void CreateDirectory()
        {
            string dirPath = Program.assemblyDirPath + "\\snrecord";

            if (!Directory.Exists(dirPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(dirPath);
            }
            // Try to create the directory.
        }
        //写sn数据项
        //public static void WriteLog(string sLog, string fileName,bool isAllfile)
        //{
        //    string logPath = Program.assemblyDirPath + "\\snrecord\\" + fileName + ".csv";

        //    if (!string.IsNullOrEmpty(sLog))
        //    {
        //        //如果日期不同则重新生成当日log文件
        //        using (FileStream _fs = new FileStream(logPath, FileMode.OpenOrCreate))
        //        {
        //            string logToWrite = string.Format("{0},{1}\r\n", sLog, DateTime.Now);

        //            //如果为写整个日志文件，不加时间
        //            if (isAllfile)
        //            {
        //                logToWrite = sLog;
        //            }

        //            //记录log
        //            //_logger.Debug(sLog);
        //            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(logToWrite);
        //            if (_fs.Position == 0)
        //            {
        //                _fs.Position = _fs.Length;
        //            }
        //            _fs.Write(bytes, 0, bytes.Length);
        //            _fs.Flush();
        //        }
        //    }
        //}


        //十六进制字符串转 十六进制 byte组
        public static byte[] strToToHexByte_w(string hexString)
        {
            hexString = hexString.Replace("-", "");
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        //byte数组 十六进制字符串
        public static string byteToHexStr_w(byte[] bytes, int index, int length)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = index; i < length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        //sn1 的byte[]倒序
        public static void my_bytesconver(ref byte[] bt, byte[] sr)
        {
            byte[] w = { sr[3], sr[2], sr[1], sr[0] };
            bt = w;
        }
        //生产用sn 与 通信用sn 相互转换
        public static void sn1_2typeconv(ref string sn_d, string sn_s, int dev, int flag)
        {
            byte[] w1 = new byte[4];
            byte[] w2 = new byte[4];
            w1 = strToToHexByte_w(sn_s);
            if (flag == 1)//生产转通信
            {
                w2[0] = (byte)dev;
                w2[1] = (byte)((w1[0] / 16 * 10 + w1[0] % 16 - 14) * 12 + w1[1] / 16 * 10 + w1[1] % 16 - 1);
                w2[2] = (byte)((((w1[2] / 16 * 10 + w1[2] % 16) * 10 + w1[3] / 16) * 10 + w1[3] % 16) / 256);
                w2[3] = (byte)((((w1[2] / 16 * 10 + w1[2] % 16) * 10 + w1[3] / 16) * 10 + w1[3] % 16) % 256);
            }
            else//通信转生产
            {
                w2[0] = (byte)(w1[1] / 12 + 14);
                w2[0] = (byte)(w2[0] / 10 * 16 + w2[0] % 10);
                w2[1] = (byte)(w1[1] % 12 + 1);
                w2[1] = (byte)(w2[1] / 10 * 16 + w2[1] % 10);
                w2[2] = (byte)((w1[2] * 256 + w1[3]) / 1000 * 16 + (w1[2] * 256 + w1[3]) % 1000 / 100);
                w2[3] = (byte)((w1[2] * 256 + w1[3]) % 100 / 10 * 16 + (w1[2] * 256 + w1[3]) % 10);
            }
            sn_d = BitConverter.ToString(w2);
        }
        //文件恢复
        public static void SnLogRec()
        {
            string logBakPath = Program.assemblyDirPath + "\\snrecord\\" + "snlogbak" + ".csv";
            string logTmpPath = Program.assemblyDirPath + "\\snrecord\\" + "snlogbaktmp" + ".csv";
            string strline;
            //   string[] aryline;
            string filename;
            byte[] tmpitem;
            FileStream my_tmp;

            if (!File.Exists(logBakPath))
            {
                return;
            }

            my_tmp = new FileStream(logTmpPath, FileMode.Create);

            using (StreamReader mysr = new StreamReader(logBakPath, Encoding.GetEncoding("GB2312")))
            {
                while ((strline = mysr.ReadLine()) != null)
                {
                    if (strline.IndexOf("file is varify") > -1 && strline.IndexOf(",") < 0 && strline.Length > 15)
                    {
                        filename = strline.Substring(15);
                        string mudewenjian = Program.assemblyDirPath + "\\snrecord\\" + filename + ".csv";
                        File.Copy(logTmpPath, mudewenjian, true);
                    }
                    else
                    {
                        strline += "\r\n";
                        tmpitem = Encoding.GetEncoding("GB2312").GetBytes(strline);
                        my_tmp.Write(tmpitem, 0, tmpitem.Length);
                        my_tmp.Flush();
                    }
                }
            }
            my_tmp.Close();
            File.Delete(logBakPath);
            File.Delete(logTmpPath);
        }
        //添加一条SN记录
        public static void AddSnLog(string sLog, string fileName)
        {            
            string logPath = Program.assemblyDirPath + "\\snrecord\\" + fileName + ".csv";
            string logBakPath = Program.assemblyDirPath + "\\snrecord\\" + "snlogbak" + ".csv";
            byte[] tmpitem;
            FileStream my_file;
            
            if (File.Exists(logPath))
            {
                File.Copy(logPath, logBakPath, true);
            }
            my_file = new FileStream(logBakPath, FileMode.OpenOrCreate);
            {
                if (!string.IsNullOrEmpty(sLog))
                {
                    string logToWrite = string.Format("{0},{1}\r\n", sLog, DateTime.Now);
                    tmpitem = Encoding.GetEncoding("GB2312").GetBytes(logToWrite);
                    if (my_file.Position == 0)
                    {
                        my_file.Position = my_file.Length;
                    }
                    my_file.Write(tmpitem, 0, tmpitem.Length);
                    my_file.Flush();
                }

                string key_w = "file is varify " + fileName;
                tmpitem = Encoding.GetEncoding("GB2312").GetBytes(key_w);
                my_file.Write(tmpitem, 0, tmpitem.Length);
                my_file.Flush();
            }
            my_file.Close();
            SnLogRec();
        }
        //删除指定SN2对应记录
        public static void DelSnLog(string sn2, string fileName)
        {
            string logPath = Program.assemblyDirPath + "\\snrecord\\" + fileName + ".csv";
            string logBakPath = Program.assemblyDirPath + "\\snrecord\\" + "snlogbak" + ".csv";
            string strline;
            string[] aryline;
            byte[] tmpitem;
            FileStream mybak;

            if (!File.Exists(logPath))
            {
                return;
            }
            mybak = new FileStream(logBakPath, FileMode.Create);
            using (StreamReader mysr = new StreamReader(logPath, Encoding.GetEncoding("GB2312")))
            {
                while ((strline = mysr.ReadLine()) != null)
                {
                    //sn1,sn2,时间
                    aryline = strline.Split(new char[] { ',' });
                    if (aryline != null && aryline.Length > 3)
                    {
                        if (sn2 != aryline[2])
                        {
                            strline += "\r\n";
                            tmpitem = Encoding.GetEncoding("GB2312").GetBytes(strline);
                            mybak.Write(tmpitem, 0, tmpitem.Length);
                            mybak.Flush();
                        }
                    }
                }
                string t = "file is varify " + fileName;
                tmpitem = Encoding.GetEncoding("GB2312").GetBytes(t);
                mybak.Write(tmpitem, 0, tmpitem.Length);
                mybak.Flush();
            }
            mybak.Close();
            SnLogRec();
        }    
        //检查SN1是否存在
        public static bool CheckSN1Exist(string sn1, string fileName)
        {
            string logPath = Program.assemblyDirPath + "\\snrecord\\" + fileName + ".csv";

            string strline;
            string[] aryline;

            if (!System.IO.File.Exists(logPath))
            {
                return false;
            }
            using (StreamReader mysr = new StreamReader(logPath, Encoding.GetEncoding("GB2312")))
            {
                while ((strline = mysr.ReadLine()) != null)
                {
                    aryline = strline.Split(new char[] { ',' });
                    if (aryline != null && aryline.Length > 3)
                    {
                        if (sn1 == aryline[0])
                        {
                            return true; 
                        }
                    }
                }
            }

            return false;
        }
        //检查SN2是否存在
        public static bool CheckSN2Exist(ref string sn1, string sn2, string fileName)
        {
            string logPath = Program.assemblyDirPath + "\\snrecord\\" + fileName + ".csv";

            string strline;
            string[] aryline;

            if (!System.IO.File.Exists(logPath))
            {
                return false;
            }

            using (StreamReader mysr = new StreamReader(logPath, Encoding.GetEncoding("GB2312")))
            {
                while ((strline = mysr.ReadLine()) != null)
                {
                    aryline = strline.Split(new char[] { ',' });
                    if (aryline != null && aryline.Length > 3)
                    {
                        if (sn2 == aryline[2])
                        {
                            sn1 = aryline[1];
                            return true;                 }
                    }
                }
            }
            return false;
        }





        //如果已经成功取消激活，把所有已经激活的SN1记录都清除，删除已有的记录文件，创建新的记录文件
        //public static void DeActiveSN1(string sn1, string sn2, string fileName, ref string msg)
        //{

        //    //对取消激活记录保存一份；
        //    string sval = string.Format("{0},{1}", sn1, sn2);

        //    WriteLog(sval, fileName + "_取消激活记录",false);

        //    _log.Info(string.Format("取消激活:{0}", sval));

        //    //备份已有文件并更新已有文件        
        //    DeActiveBackupData(sn1, sn2, fileName, ref msg);         

             
        //}
        
        //private static void DeActiveBackupData(string sn1, string sn2, string fileName, ref string msg)
        //{
        //    string logPath = Program.assemblyDirPath + "\\snrecord\\" + fileName + ".csv";
        //    //备份文件
        //    string logNewPath = Program.assemblyDirPath + "\\snrecord\\" + fileName + "_" + System.Guid.NewGuid().ToString() + ".csv";

        //    string strline;
        //    string[] aryline;


        //    if (!System.IO.File.Exists(logPath))
        //    {
        //        return ;
        //    }

        //    //更新已有的记录文件，并对已有文件备份
        //    StringBuilder sBuilderSN1 = new StringBuilder();

        //    StringBuilder sBuilderSave = new StringBuilder();

        //    using (StreamReader mysr = new StreamReader(logPath, Encoding.GetEncoding("GB2312")))
        //    {
        //        while ((strline = mysr.ReadLine()) != null)
        //        {
        //            //sn1,sn2,时间
        //            aryline = strline.Split(new char[] { ',' });

        //            if (aryline != null && aryline.Length > 2)
        //            {
        //                //if (sn1 == aryline[0])
        //                //{
        //                //    sBuilderSN1.AppendFormat("SN1:{0}成功取消激活,对应的SN2为：{1}\r\n", aryline[0], aryline[1]);
        //                //}
        //                //else
        //                if (sn2 == aryline[1])
        //                {
        //                    sBuilderSN1.AppendFormat("SN2：{0}成功取消激活,原对应的SN1为：{1}，返回的SN1为：{2}\r\n", sn2, sn1, aryline[0]);

        //                }
        //                else
        //                {
        //                    sBuilderSave.AppendLine(strline);
        //                }
        //            }
        //        }
        //    }

        //    msg += "\r\n";

        //    if (sBuilderSN1.Length > 0)
        //    {
        //        msg += sBuilderSN1.ToString().TrimEnd((char[])"\n\r".ToCharArray());

        //    }

        //    sBuilderSN1 = null;

        //    if (File.Exists(logPath))
        //    {
        //        FileInfo file = new FileInfo(logPath);

        //        file.MoveTo(logNewPath);

        //        file.Delete();

        //    }

        //    //重新写入文件
        //    WriteLog(sBuilderSave.ToString(), fileName,true);

        //    sBuilderSave = null;

   
        //}
    }

    //public class QueueData
    //{
    //    private byte[] _bytes = null;

    //    public byte[] Bytes
    //    {
    //        get { return _bytes; }
    //        set { _bytes = value; }
    //    }

    //    private DateTime _DateTime = DateTime.Now;

    //    public DateTime DateTime
    //    {
    //        get { return _DateTime; }
    //        set { _DateTime = value; }
    //    }

    //    public QueueData(byte[] bytes, DateTime dateTime)
    //    {
    //        this.Bytes = bytes;
    //        this.DateTime = dateTime;
    //    }

    //    private int _SentCount = 1;

    //    public int SentCount
    //    {
    //        get { return _SentCount; }
    //        set { _SentCount = value; }
    //    }

    //}
}
