﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace IntDevs.Upgrade
{
    public partial class FrmUpgradeSocket : Form
    {
        private static SerialPort _serialPort;
        private static log4net.ILog _log = null;
        private static log4net.ILog _logFile = null;
        private static log4net.ILog _logALL = null;

        private static byte[] m_File_Bin = null;
        private static long m_File_Len = 0;
        private static byte[] m_File_Ver = new byte[3];
        private static byte m_File_state = 0;
        private static byte m_Send_state = 0;
        private static byte m_Send_enable = 0;
        private static byte[] prg_packt = new byte[149];//默认分配1页内存，并始终限制不允许超过  
        private static int m_packt_indx = 0;
        private static byte m_ACK = 0;

        private static Thread ComTh = null;

        private static byte app_mode = 1;//下载tz6.0，下载6.0及sf程序
        private static bool isListen = false;
        private static AsyncTcpSocketServer socketServer = null;
        private void RunThread()
        {
            if (ComTh == null)
            {
                ComTh = new Thread(new ThreadStart(task_do));
                ComTh.IsBackground = true;
                ComTh.SetApartmentState(ApartmentState.STA);
                ComTh.Start();
            }
        }
        private void InitLogConfig()
        {
            _log = log4net.LogManager.GetLogger("RNCloud.LogWarn");
            _logFile = log4net.LogManager.GetLogger("RNCloud.LogDebug");
            _logALL = log4net.LogManager.GetLogger("RNCloud.LogAll");
            _logALL.Info("启动！");
        }
        public FrmUpgradeSocket()
        {
            InitializeComponent();
            try
            {
                InitLogConfig();

                this.txtFilePath.ReadOnly = true;
                this.txtComNum.Text = ConfigurationFile.ServerIP;
                this.txtBaudRate.Text = ConfigurationFile.ServerPort;              
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("错误：{0}", ex.ToString());
                _logALL.Error(errMsg);
                MessageBox.Show(errMsg);
            }
            //RunThread();
        }

        
        

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        //初始化
        public void InitialPort()
        {
            try
            {
                string comport = this.txtComNum.Text.Trim();
                string baudrate = this.txtBaudRate.Text.Trim();
                _serialPort = new System.IO.Ports.SerialPort();
                _serialPort.PortName = comport;
                _serialPort.BaudRate = int.Parse(baudrate);
                _serialPort.DataBits = 8;
                _serialPort.Parity = System.IO.Ports.Parity.None;
                _serialPort.StopBits = System.IO.Ports.StopBits.One;
                _serialPort.WriteTimeout = 2000;
                _serialPort.ReadTimeout = -1;
                _serialPort.RtsEnable = true;
                _serialPort.DtrEnable = true;

                //_serialPort.Handshake = System.IO.Ports.Handshake.RequestToSend;
                _serialPort.ReceivedBytesThreshold = 1;
                //设置数据流控制；数据传输的握手协议；
                //_serialPort.Handshake = Handshake.None;
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                    System.Threading.Thread.Sleep(150);
                }
                else
                {
                    try
                    {
                        _serialPort.Open();
                    }
                    catch (System.Exception ex)
                    {
                        string str = string.Format("打开串口异常，应用程序需重新打开。");
                        _logALL.Error(ex.Message);
                        MessageBox.Show(str);                      
                        Application.Exit();
                        //this.Close();
                    }
                }                
                _log.Warn(string.Format("成功打开串口:{0}", comport));

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }            
        //打开串口
        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            string comnum = this.txtComNum.Text.Trim();
            string baudrate = this.txtBaudRate.Text.Trim();

            if (string.IsNullOrEmpty(comnum) || string.IsNullOrEmpty(baudrate))
            {
                MessageBox.Show("串口号和波特率不能为空。");
                return;
            }
            try
            {
                ConfigurationFile.UpdateVal("COMConfig", this.txtComNum.Text.Trim());
                ConfigurationFile.UpdateVal("BaudRate", this.txtBaudRate.Text.Trim());
            }
            catch (System.Exception ex)
            {
                _logALL.Error(ex.Message);
            }

            try
            {
                InitialPort();
                //this.btnOpenPort.Text = "串口已经打开";
                //this.btnOpenPort.Enabled = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        //获取文件信息
        private void GetFileByte()
        {
            DialogResult diaresutl = this.openFileDialog1.ShowDialog();
            string filename = this.openFileDialog1.FileName;
            if (string.IsNullOrEmpty(filename))
                return;
            this.txtFilePath.Text = filename;
            long length = 0;
            FileInfo finfo = null;
            string version = string.Empty;

            if (File.Exists(filename))
            {
                finfo = new FileInfo(filename);
                using (Stream reader = new FileStream(filename, FileMode.Open))
                {
                    length = reader.Length;
                    if (length - 3 > 1024 * 100 || length < 3)
                    {
                        MessageBox.Show(string.Format("文件不正确"));
                        return;
                    }
                    using (BinaryReader fileBin = new BinaryReader(reader))
                    {
                        fileBin.BaseStream.Position = 0;
                        m_File_Bin = fileBin.ReadBytes((int)length);
                        length -= 3;
                        m_File_Len = length;
                        //获取版本号,尾部的3个byte为版本号
                        fileBin.BaseStream.Position = length;
                        m_File_Ver = fileBin.ReadBytes(3);
                        string bytestr = BitConverter.ToString(m_File_Ver);
                        version = bytestr;
                    }
                }
                this.lblFileInfo.Text = string.Format("版本号：{0},文件大小：{1}Byte,{2}.{3}", version, length, length / 100, length % 100);
                m_File_state = 1;
            }
        }        
        //获取文件
        private void button1_Click(object sender, EventArgs e)
        {
            GetFileByte();
            if (m_File_state == 1)
            {
                this.btnGetFile.Text = "已获取升级文件";
                this.btnGetFile.Enabled = false;
            }
        }    
        private static Task BroadCastTask = null;
        private static CancellationTokenSource tokenSource;
        private static CancellationToken token;
        private readonly ManualResetEvent _event = new ManualResetEvent(true);

        //发送升级文件
        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            if (!isListen || socketServer == null)
            {
                MessageBox.Show("服务器还没建立监听", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //检查文件
            if(m_File_state == 0)
            {
                MessageBox.Show("先选择升级文件。");
                return;
            }
            //打开发送
            m_Send_enable = 1;
            m_Send_state = 1;

            this.btnUpgrade.Enabled = false;
            this.btnResume.Enabled = false;

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            BroadCastTask = Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        if (!isListen || socketServer == null)
                        {
                            return;
                        }

                        _event.WaitOne();

                        if (token.IsCancellationRequested)
                        {
                            throw new OperationCanceledException(token);
                        }

                        SendPack();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }, token);

            BroadCastTask.Wait();
        }

        private void SetCancel()
        {

            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }

        }

        private void SendPack()
        {
  

            if (m_Send_state == 0)
                return;
            int cnt1 = 100, cnt2 = 0, i = 0;
            int y = (int)(m_File_Len / 100);
            if ((m_File_Len % 100) != 0)
                y += 1;
            if (m_packt_indx >= y)
                m_packt_indx = 0;
            int t2 = m_packt_indx * 100;
            cnt2 = m_File_Bin.Length - 3 - m_packt_indx * 100;
            if (cnt2 < 100)
            {
                cnt1 = cnt2;
                cnt2 = 100 - cnt1;
            }
            else
                cnt2 = 0;

            if (app_mode == 0)
            {
                prg_packt[0] = 0x5A;
                prg_packt[1] = 0xFF;
                prg_packt[2] = 0xFF;
                prg_packt[3] = 0xFF;
                prg_packt[4] = 0xFF;
                prg_packt[5] = 113;
                prg_packt[6] = 0x51;

                prg_packt[7] = 3;

                prg_packt[8] = m_File_Ver[0];
                prg_packt[9] = m_File_Ver[1];
                prg_packt[10] = m_File_Ver[2];

                prg_packt[11] = (byte)((m_File_Len >> 24) & 0xff);
                prg_packt[12] = (byte)((m_File_Len >> 16) & 0xff);
                prg_packt[13] = (byte)((m_File_Len >> 8) & 0xff);
                prg_packt[14] = (byte)((m_File_Len >> 0) & 0xff);

                prg_packt[15] = (byte)((t2 >> 24) & 0xff);
                prg_packt[16] = (byte)((t2 >> 16) & 0xff);
                prg_packt[17] = (byte)((t2 >> 8) & 0xff);
                prg_packt[18] = (byte)((t2 >> 0) & 0xff);

                i = 0;
                Array.Copy(m_File_Bin, t2, prg_packt, 19, cnt1);
                if (cnt2 > 0)
                {
                    i = 0;
                    while (i < cnt2)
                    {
                        prg_packt[19 + cnt1 + i] = 0;
                        i++;
                    }
                }

                prg_packt[119] = m_ACK;
                prg_packt[120] = crc_8(prg_packt, 1, 119);
                prg_packt[121] = 0xA5;
            }
            else
            {
                prg_packt[17] = 3;
                prg_packt[18] = 0x03;
                prg_packt[19] = 44;
                prg_packt[20] = 121 + 1;

                prg_packt[21] = 0;

                prg_packt[22] = 3;

                prg_packt[23] = m_File_Ver[2];
                prg_packt[24] = m_File_Ver[1];
                prg_packt[25] = m_File_Ver[0];
                prg_packt[26] = 0;

                prg_packt[27] = (byte)((y >> 0) & 0xff);
                prg_packt[28] = (byte)((y >> 8) & 0xff);

                prg_packt[29] = (byte)((m_packt_indx >> 0) & 0xff);
                prg_packt[30] = (byte)((m_packt_indx >> 8) & 0xff);

                i = 0;
                Array.Copy(m_File_Bin, t2, s_prgframe, 0, cnt1);
                if (cnt2 > 0)
                {
                    i = 0;
                    while (i < cnt2)
                    {
                        s_prgframe[0 + cnt1 + i] = 0;
                        i++;
                    }
                }
                seacret(s_prgframe, 0, ref scrt_prgframe, 0);
                Array.Copy(scrt_prgframe, 0, prg_packt, 31, 112);
            }
            m_packt_indx++;

            socketServer.BroadcastSync(prg_packt);

            if(m_packt_indx % 10 ==0)
            {
                Thread.Sleep(200);
            }


            //if (app_mode == 0)
            //{
            //    _serialPort.Write(prg_packt, 0, 122);
            //}
            //else
            //    Tools.WriteCmdd(ref prg_packt, _serialPort);

            //  if (m_packt_indx == 107)
            //      return;
          
           
        }
        //获取版本
        private void btnGetSFVer_Click_1(object sender, EventArgs e)
        {
            return;
        }
        //
        private void btnVerResponse_Click(object sender, EventArgs e)
        {            
            
        }
        //暂停
        private void btnSuspend_Click(object sender, EventArgs e)
        {
            //if (m_Send_state == 1 && m_Send_enable == 1)
            //{
            //    m_Send_state = 0;
                this.btnSuspend.Enabled = false;
                this.btnResume.Enabled = true;

                
            //}

                _event.Reset();
        }
        //继续
        private void btnResume_Click(object sender, EventArgs e)
        {
            //if (m_Send_state == 0 && m_Send_enable == 1)
            //{
            //    m_Send_state = 1;
                this.btnResume.Enabled = false;
                this.btnSuspend.Enabled = true;

            //}

                _event.Set();
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {            
        }        
        private void btnListenMB_Click(object sender, EventArgs e)
        {
            return;
        }

        private static byte crc_8(byte[] s, int indx, int cnt)
        {
            byte res = 0;
            int i = 0;
            if(cnt > 0)
                res = s[indx];
            cnt--;
            if (cnt > 0)
            {
                for (i = 0; i < cnt; i++)
                {
                    res ^= s[indx + i + 1];
                }
            }
            return res;
        }
        private static byte[] s_prgframe = new byte[100];
        private static byte[] scrt_prgframe = new byte[112];
        //加密
        private static void seacret(byte[] buff_s,int indx_s,ref byte[] buff_d,int indx_d)
        {
            byte cnt, ad_v, len = 1, indx = 1;

            cnt = ad_v = (byte)(buff_s[indx_s] % 10 + 10);
            while (indx <= 110)
            {
                if (indx == cnt)
                {
                    buff_d[indx_d + indx - 1] = ad_v;
                    ad_v++;
                    cnt += ad_v;
                }
                else if (len <= 100)
                {
                    buff_d[indx_d + indx - 1] = buff_s[indx_s + len - 1];
                    len++;
                }
                else
                {
                    buff_d[indx_d + indx - 1] = 0xff;
                }
                indx++;
            }
        }
        //
        private static void sendpackt()
        {
            if (_serialPort != null && _serialPort.IsOpen) 
            {
                if (m_Send_state == 0)
                    return;
                int cnt1 = 100, cnt2 = 0, i = 0; 
                int y = (int)(m_File_Len / 100);
                if ((m_File_Len % 100) != 0)
                    y += 1;
                if (m_packt_indx >= y)
                    m_packt_indx = 0;
                int t2 = m_packt_indx * 100;
                cnt2 = m_File_Bin.Length - 3 - m_packt_indx * 100;
                if (cnt2 < 100)
                {
                    cnt1 = cnt2;
                    cnt2 = 100 - cnt1;
                }
                else
                    cnt2 = 0;

                if (app_mode == 0)
                {
                    prg_packt[0] = 0x5A;
                    prg_packt[1] = 0xFF;
                    prg_packt[2] = 0xFF;
                    prg_packt[3] = 0xFF;
                    prg_packt[4] = 0xFF;
                    prg_packt[5] = 113;
                    prg_packt[6] = 0x51;

                    prg_packt[7] = 3;

                    prg_packt[8] = m_File_Ver[0];
                    prg_packt[9] = m_File_Ver[1];
                    prg_packt[10] = m_File_Ver[2];

                    prg_packt[11] = (byte)((m_File_Len >> 24) & 0xff);
                    prg_packt[12] = (byte)((m_File_Len >> 16) & 0xff);
                    prg_packt[13] = (byte)((m_File_Len >> 8) & 0xff);
                    prg_packt[14] = (byte)((m_File_Len >> 0) & 0xff);
                    
                    prg_packt[15] = (byte)((t2 >> 24) & 0xff);
                    prg_packt[16] = (byte)((t2 >> 16) & 0xff);
                    prg_packt[17] = (byte)((t2 >> 8) & 0xff);
                    prg_packt[18] = (byte)((t2 >> 0) & 0xff);

                    i = 0;
                    Array.Copy(m_File_Bin,t2,prg_packt,19,cnt1);
                    if (cnt2 > 0)
                    {
                        i = 0;
                        while (i < cnt2)
                        {
                            prg_packt[19 + cnt1 + i] = 0;
                            i++;
                        }
                    }

                    prg_packt[119] = m_ACK;
                    prg_packt[120] = crc_8(prg_packt, 1, 119);
                    prg_packt[121] = 0xA5;
                }
                else 
                {
                    prg_packt[17] = 3;
                    prg_packt[18] = 0x03;
                    prg_packt[19] = 44;
                    prg_packt[20] = 121+1;

                    prg_packt[21] = 0;

                    prg_packt[22] = 3;

                    prg_packt[23] = m_File_Ver[2];
                    prg_packt[24] = m_File_Ver[1];
                    prg_packt[25] = m_File_Ver[0];
                    prg_packt[26] = 0;                    

                    prg_packt[27] = (byte)((y >> 0) & 0xff);
                    prg_packt[28] = (byte)((y >> 8) & 0xff);

                    prg_packt[29] = (byte)((m_packt_indx >> 0) & 0xff);
                    prg_packt[30] = (byte)((m_packt_indx >> 8) & 0xff);

                    i = 0;
                    Array.Copy(m_File_Bin, t2, s_prgframe, 0, cnt1);
                    if (cnt2 > 0)
                    {
                        i = 0;
                        while (i < cnt2)
                        {
                            s_prgframe[0 + cnt1 + i] = 0;
                            i++;
                        }
                    }
                    seacret(s_prgframe, 0, ref scrt_prgframe, 0);
                    Array.Copy(scrt_prgframe, 0, prg_packt, 31, 112);
                }
                {
                    if (app_mode == 0)
                    {                  
                        _serialPort.Write(prg_packt, 0, 122);
                    }
                    else
                        Tools.WriteCmdd(ref prg_packt, _serialPort);
                }

                m_packt_indx++;
            }   
        }
        //
        private void task_do()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(50);
                sendpackt();
            }
        }

        private bool ComfirmStopListen()
        {
            if (isListen)
            {
                DialogResult r = MessageBox.Show("确定停止监听吗?", "确定", MessageBoxButtons.YesNo); 
                if (r == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            if (!ComfirmStopListen())
            {
                return;
            }

            string server = this.txtComNum.Text.Trim();
            string port = this.txtBaudRate.Text.Trim();

            if (!isListen || socketServer == null)
            {
                socketServer = new AsyncTcpSocketServer(server, int.Parse(port));
                socketServer.Listen();

                if (socketServer.IsListening)
                {
                    this.btnListen.Text = "停止监听";
                    this.lblServerStatus.Text = string.Format("服务器在监听,启动时间：{0}", DateTime.Now.ToString());
                    isListen = !isListen;
                }
            }
            else
            {
                socketServer.Shutdown();
                this.btnListen.Text = "打开监听";
                this.lblServerStatus.Text = string.Empty;

                socketServer = null;
                isListen = !isListen;
            }
        }

        private void btnGetFile_Click(object sender, EventArgs e)
        {
            GetFileByte();
            if (m_File_state == 1)
            {
                this.btnGetFile.Text = "已获取升级文件";
                this.btnGetFile.Enabled = false;
            }
        } 
    }
}
