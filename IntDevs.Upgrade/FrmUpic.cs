/*
 序列号管理，使用zigbee连接器，yangqinxu
 */
using System;
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

namespace IntDevs.Upgrade
{
    public partial class FrmUpic : Form
    {
        private static int task_time = 0;  //计时器

        private static int dev_type = 0;  //设备类型
        private static string use_sn1 = "";  //用户生成 sn1

        private static int task_id = 0;  //任务号
        private static int task_res = 0;  //任务结果
        private static int task_Flag = 1;  //任务标志

        private static int buff_Szie = 256;  //缓存尺寸

       

        private static byte[] m2L_buff = new byte[buff_Szie];  //2级缓存
        private static int m2L_buff_len = 0;  //2级缓存长度
        private static int m2L_buff_SP = 0;  //2级缓存存储指针
        private static int m2L_buff_GP = 0;  //2级缓存读取指针
        private static int GetFrameDataLen = 0; //帧检识长度
        private static int GetFrameState = 0;//帧检识 状态

        private static byte[] RF_buff = new byte[buff_Szie];   //接收帧缓存
        private static byte[] SF_buff = new byte[buff_Szie];   //发送帧缓存

        //日志定义
        private static log4net.ILog _log = null;
        private static log4net.ILog _logFile = null;
        private static log4net.ILog _logALL = null;
        //private delegate void UpdateRxtCallback(string s);

        private static Thread ReceivedComTh = null;
        private static Thread TimeCntTh = null;
        private static Thread HouTaiTh = null;
        
        private static SerialPort _serialPort;




        //初始化日志文件
        private void InitLogConfig()
        {
            _log = log4net.LogManager.GetLogger("RNCloud.LogWarn");
            _logFile = log4net.LogManager.GetLogger("RNCloud.LogDebug");
            _logALL = log4net.LogManager.GetLogger("RNCloud.LogAll");
            _logALL.Info("启动！");
        }
        //设置年、月、号数
        private void SetDate()
        {
            this.txtYear.Text = DateTime.Now.Year.ToString();
            this.txtMonth.Text = DateTime.Now.Month.ToString();
            this.txtCount.Text = "1";
        }

        //串口接收任务
        private static void GetPortBuffer()
        {
            System.Threading.Thread.Sleep(1000);
            while (true)
            {
                System.Threading.Thread.Sleep(1);
                // try
                {
                    if (_serialPort != null)
                        if (_serialPort.IsOpen)
                        {
                            byte bt = Convert.ToByte(_serialPort.ReadByte());
                            Tools.btStore(bt);
                        }
                }
                // catch (System.Exception ex)
                {
                    //     _logFile.Error(ex.Message);

                    //     throw ex;
                }
            }
        }

        //计时器任务
        private static void Timercnt()
        {
            System.Threading.Thread.Sleep(1000);
            while (true)
            {
                System.Threading.Thread.Sleep(10);
                if (task_time > 0)
                    task_time--;
            }
        }

        //接收帧组装
        private static int Com1_proc_sfc(byte[] sp, int head, int len)
        {
            int j;
            int crc;

            j = 0;
            while (len-- > 0)
            {
                RF_buff[j++] = sp[head];
                head++;
                head %= buff_Szie;
            }
            crc = Tools.crc_s1(RF_buff, 1, RF_buff[20] + 25 - 4);
            if ((byte)(crc >> 8) == RF_buff[21 + RF_buff[20] + 1] && (byte)(crc >> 0) == RF_buff[21 + RF_buff[20] + 2])
                return 1;
            return 0;
        }
        //检识帧
        private static int Frame_R()
        {
            int res = 0;
            byte tmp = 0;

            while (m2L_buff_len < buff_Szie)
            {
                if (Tools.btGet(ref tmp) == 1)
                {
                    m2L_buff[m2L_buff_SP++] = tmp;
                    m2L_buff_SP %= buff_Szie;
                    ++m2L_buff_len;
                }
                else
                    break;
            }

            switch (GetFrameState)
            {
                case 0://找帧头
                    while (m2L_buff_len > 0)
                    {
                        if (m2L_buff[m2L_buff_GP] == 0x5A)
                        {
                            GetFrameState = 1;
                            break;
                        }
                        else
                        {
                            m2L_buff_GP++;
                            m2L_buff_GP %= buff_Szie;
                            m2L_buff_len--;
                        }
                    }
                    break;
                case 1://找长度
                    if (m2L_buff_len >= 25)
                    {
                        GetFrameDataLen = m2L_buff[(m2L_buff_GP + 20) % buff_Szie];
                        if (GetFrameDataLen > 200)
                        {
                            m2L_buff_GP++;
                            m2L_buff_GP %= buff_Szie;
                            m2L_buff_len--;
                            GetFrameState = 0;
                        }
                        else
                            GetFrameState = 2;
                    }
                    break;
                case 2://找帧尾
                    if (m2L_buff_len >= 25 + GetFrameDataLen)
                    {
                        if (m2L_buff[(m2L_buff_GP + 25 - 1 + GetFrameDataLen) % buff_Szie] == 0xA5)
                        {
                            if (Com1_proc_sfc(m2L_buff, m2L_buff_GP, GetFrameDataLen + 25) == 1)
                                res = 1;
                            m2L_buff_GP += 25 + GetFrameDataLen;
                            m2L_buff_GP %= buff_Szie;
                            m2L_buff_len -= 25 + GetFrameDataLen;
                            GetFrameState = 0;
                        }
                        else
                        {
                            m2L_buff_GP++;
                            m2L_buff_GP %= buff_Szie;
                            m2L_buff_len--;
                            GetFrameState = 0;
                        }
                    }
                    break;
            }
            return res;
        }
        //
        private static void sn2fz(ref byte[] s)
        {
            int i = -1;
            byte tmp;
            while (++i < 3)
            {
                tmp = s[3 + i * 4];
                s[3 + i * 4] = s[0 + i * 4];
                s[0 + i * 4] = tmp;
                tmp = s[2 + i * 4];
                s[2 + i * 4] = s[1 + i * 4];
                s[1 + i * 4] = tmp;
            }
        }
        //时间转byte[]
        private static void Datetobytes(ref byte[] bt)
        {
            DateTime dt = DateTime.Now;
            int year = dt.Year;
            byte BigYear = Convert.ToByte(year / 256);
            byte SmallYear = Convert.ToByte(year - BigYear * 256);
            byte[] tbytes = 
            {  BigYear, SmallYear, Convert.ToByte(dt.Month), Convert.ToByte(dt.Day),
                Convert.ToByte(dt.Hour), Convert.ToByte(dt.Minute), Convert.ToByte(dt.Second) };
            bt = tbytes;
        }
        //添加SN
        private static void ADD_SN(string loc, string sn1, string S, string sn2, int dev)
        {
            string sval = string.Format("{0},{1},{2}", sn1, S, sn2);
            Tools.AddSnLog(sval, loc);
        }
        //删除SN
        private static void DEL_SN(string loc, string sn2)
        {
            Tools.DelSnLog(sn2, loc);
        }
        //实际任务
        private void houtaicaozuo()
        {
            System.Threading.Thread.Sleep(1000);
            byte st = 0;
            int preid = 0;
            int cnt = 0;
            byte flag = 0;
            byte[] by_SN1 = new byte[4];
            byte[] by_SN2 = new byte[12];
            string sn1 = "";
            string sn2 = "";
            string local = "";
            byte[] date = new byte[7];
            string[] strArr = new string[4];


            while (true)
            {
                System.Threading.Thread.Sleep(1);
                if (task_id != preid)
                {
                    preid = task_id;
                    st = 0;
                    cnt = 3;
                    flag = 1;
                    if (task_id == 1)
                        strArr[0] = "WriteTimeLb";
                    else if (task_id == 2)
                        strArr[0] = "Active";
                    else if (task_id == 3)
                        strArr[0] = "DeActive";
                    if (task_id != 0)
                    {
                        strArr[1] = strArr[2] = strArr[3] = "";
                        ShowData(strArr);
                    }
                }
                switch (task_id)
                {
                    case 0:
                        break;
                    case 1://修改时间
                        switch (st)
                        {
                            case 0://下发时间
                                if (cnt-- == 0)
                                {
                                    task_id = 0;
                                    task_res = 1;
                                    task_Flag = 1;
                                    strArr[1] = "写入时间->通信超时";
                                    ShowData(strArr);
                                    break;
                                }
                                SF_buff[17] = 0x03;
                                SF_buff[18] = 0;
                                SF_buff[19] = 40;
                                SF_buff[20] = 7 + 1;
                                SF_buff[21] = 0;
                                Datetobytes(ref date);
                                Array.Copy(date, 0, SF_buff, 22, 7);
                                Tools.WriteCmdd(ref SF_buff, _serialPort);
                                task_time = 200;
                                st = 1;
                                strArr[1] = "写入时间->通信中";
                                ShowData(strArr);
                                break;
                            case 1://等待应答
                                if (Frame_R() == 1)
                                {
                                    if (RF_buff[17] == 0x83 || RF_buff[18] == 0 || RF_buff[19] == 40)
                                    {
                                        task_id = 0;
                                        task_res = 2;
                                        task_Flag = 1;
                                        strArr[1] = "写入时间->成功！";
                                        ShowData(strArr);
                                    }
                                }
                                else if (task_time == 0)
                                {
                                    st = 0;
                                }
                                break;
                            case 200:
                                break;
                        }
                        break;

                    case 2://激活
                        switch (st)
                        {
                            case 0://预处理
                                if (cnt-- == 0)
                                {
                                    task_id = 0;
                                    task_res = 1;//超时
                                    task_Flag = 1;
                                    strArr[1] = "激活->通信超时1";
                                    ShowData(strArr);
                                    break;
                                }
                                SF_buff[17] = 0x06;
                                SF_buff[18] = 0x00;
                                SF_buff[19] = 0x01;
                                SF_buff[20] = 7;
                                Datetobytes(ref date);
                                Array.Copy(date, 0, SF_buff, 21, 7);
                                Tools.WriteCmdd(ref SF_buff, _serialPort);
                                task_time = 200;
                                st = 1;
                                strArr[1] = "激活->通信中1";
                                ShowData(strArr);
                                break;
                            case 1://等待应答
                                if (Frame_R() == 1)
                                {
                                    if (RF_buff[17] == 0x86 || RF_buff[18] == 0 || RF_buff[19] == 1)
                                    {
                                        if (RF_buff[20] == 16)
                                        {
                                            Array.Copy(RF_buff, 21, by_SN2, 0, 12);
                                            sn2fz(ref by_SN2);
                                            Array.Copy(RF_buff, 33, by_SN1, 0, 4);
                                            Tools.my_bytesconver(ref by_SN1, by_SN1);
                                            strArr[2] = BitConverter.ToString(by_SN1);
                                            strArr[3] = BitConverter.ToString(by_SN2);
                                            ShowData(strArr);
                                            int I_SN1 = BitConverter.ToInt32(by_SN1, 0);
                                            if (I_SN1 > 0)
                                            {
                                                Tools.sn1_2typeconv(ref strArr[2], strArr[2], 0, 0);
                                                task_id = 0;
                                                task_res = 2; //已激活
                                                task_Flag = 1;
                                                strArr[1] = "激活->错误1 已激活";
                                                ShowData(strArr);
                                                break;
                                            }
                                            else
                                            {
                                                st = 2;
                                                flag = 1;
                                                cnt = 3;
                                            }
                                        }
                                    }
                                }
                                else if (task_time == 0)
                                {
                                    st = 0;
                                }
                                break;
                            case 2://激活
                                if (cnt-- == 0)
                                {
                                    task_id = 0;
                                    task_res = 3; //超时2
                                    task_Flag = 1;
                                    strArr[1] = "激活->通信超时2";
                                    ShowData(strArr);
                                    break;
                                }
                                if (flag == 1)
                                {
                                    flag = 0;
                                    sn2 = BitConverter.ToString(by_SN2);
                                    string filetm = "SNLog-" + Tools.sysdevinfo[dev_type - 1];
                                    local = filetm;
                                    if (Tools.CheckSN2Exist(ref sn1, sn2, local))//查找存在，找回之前的SN1
                                    {
                                        by_SN1 = Tools.strToToHexByte_w(sn1);
                                        Tools.sn1_2typeconv(ref sn1, sn1, dev_type, 0);
                                    }
                                    else//查找不存在，核对输入的SN1
                                    {
                                        sn1 = use_sn1;
                                        Tools.sn1_2typeconv(ref use_sn1, use_sn1, dev_type, 1);
                                        by_SN1 = Tools.strToToHexByte_w(use_sn1);
                                        if (Tools.CheckSN1Exist(sn1, local))//输入的SN1不合法
                                        {
                                            task_id = 0;
                                            task_res = 4;//错误
                                            task_Flag = 1;
                                            strArr[1] = "激活->错误2 输入的SN1非法";
                                            ShowData(strArr);
                                            break;
                                        }
                                        else//保存SN2及SN1 到数据库，并写日志
                                        {
                                            ADD_SN(local, sn1, use_sn1, sn2, dev_type);
                                        }
                                    }
                                    Tools.my_bytesconver(ref by_SN1, by_SN1);
                                    SF_buff[17] = 0x06;
                                    SF_buff[18] = 0x00;
                                    SF_buff[19] = 0x02;
                                    SF_buff[20] = 16;
                                    sn2fz(ref by_SN2);
                                    Array.Copy(by_SN2, 0, SF_buff, 21, 12);
                                    Array.Copy(by_SN1, 0, SF_buff, 33, 4);
                                    Tools.WriteCmdd(ref SF_buff, _serialPort);
                                }
                                task_time = 200;
                                st = 3;
                                strArr[1] = "激活->通信中2";
                                ShowData(strArr);
                                break;
                            case 3://等待应答
                                if (Frame_R() == 1)
                                {
                                    if (RF_buff[17] == 0x86 || RF_buff[18] == 0 || RF_buff[19] == 2)
                                    {
                                        task_id = 0;
                                        task_res = 4;//成功
                                        task_Flag = 1;
                                        strArr[1] = "激活->成功" + ":" + sn1;
                                        // MessageBox.Show(sn1);
                                        ShowData(strArr);
                                    }
                                }
                                else if (task_time == 0)
                                {
                                    st = 2;
                                }
                                break;
                            case 200:
                                break;
                        }
                        break;


                    case 3://取消激活
                        switch (st)
                        {
                            case 0://预处理
                                if (cnt-- == 0)
                                {
                                    task_id = 0;
                                    task_res = 1;//超时
                                    task_Flag = 1;
                                    strArr[1] = "取消激活->通信超时1";
                                    ShowData(strArr);
                                    break;
                                }
                                SF_buff[17] = 0x06;
                                SF_buff[18] = 0x00;
                                SF_buff[19] = 0x01;
                                SF_buff[20] = 7;
                                Datetobytes(ref date);
                                Array.Copy(date, 0, SF_buff, 21, 7);
                                Tools.WriteCmdd(ref SF_buff, _serialPort);
                                task_time = 100;
                                st = 1;

                                strArr[1] = "取消激活->通信中1";
                                ShowData(strArr);
                                break;
                            case 1://等待应答
                                if (Frame_R() == 1)
                                {
                                    if (RF_buff[17] == 0x86 || RF_buff[18] == 0 || RF_buff[19] == 1)
                                    {
                                        if (RF_buff[20] == 16)
                                        {
                                            Array.Copy(RF_buff, 21, by_SN2, 0, 12);
                                            sn2fz(ref by_SN2);
                                            Array.Copy(RF_buff, 33, by_SN1, 0, 4);
                                            Tools.my_bytesconver(ref by_SN1, by_SN1);
                                            strArr[2] = BitConverter.ToString(by_SN1);
                                            strArr[3] = BitConverter.ToString(by_SN2);
                                            ShowData(strArr);
                                            int I_SN1 = BitConverter.ToInt32(by_SN1, 0);
                                            if (I_SN1 == 0)
                                            {
                                                task_id = 0;
                                                task_res = 2; //已取消激活
                                                task_Flag = 1;
                                                strArr[1] = "取消激活->错误1 已取消";
                                                ShowData(strArr);
                                                break;
                                            }
                                            else
                                            {
                                                Tools.sn1_2typeconv(ref strArr[2], strArr[2], 0, 0);
                                                ShowData(strArr);
                                                st = 2;
                                                flag = 1;
                                                cnt = 3;
                                            }
                                        }
                                    }
                                }
                                else if (task_time == 0)
                                {
                                    st = 0;
                                }
                                break;
                            case 2://取消激活
                                if (cnt-- == 0)
                                {
                                    task_id = 0;
                                    task_res = 3; //超时2
                                    task_Flag = 1;

                                    strArr[1] = "取消激活->通信超时2";
                                    ShowData(strArr);
                                    break;
                                }
                                if (flag == 1)
                                {
                                    flag = 0;
                                    sn2 = BitConverter.ToString(by_SN2);
                                    string filetm = "SNLog-" + Tools.sysdevinfo[by_SN1[0] - 1];
                                    local = filetm;
                                    st = 3;
                                    if (Tools.CheckSN2Exist(ref sn1, sn2, local))//查找存在
                                    {
                                        //st = 3;   
                                    }
                                    else//查找不存在，？
                                    {
                                        //   task_id = 0;
                                        //   task_res = 4; //出错
                                        //   task_Flag = 1;

                                        //  strArr[1] = "取消激活->错误2";
                                        //  ShowData(strArr);
                                        //   break;
                                    }
                                    SF_buff[17] = 0x06;
                                    SF_buff[18] = 0x00;
                                    SF_buff[19] = 0x03;
                                    SF_buff[20] = 0;
                                    Tools.WriteCmdd(ref SF_buff, _serialPort);
                                }

                                strArr[1] = "取消激活->通信中2";
                                ShowData(strArr);
                                task_time = 100;
                                break;
                            case 3://等待应答
                                if (Frame_R() == 1)
                                {
                                    if (RF_buff[17] == 0x86 || RF_buff[18] == 0 || RF_buff[19] == 3)//清除之前的SN2、SN1，并 写日志
                                    {
                                        DEL_SN(local, sn2);
                                        task_id = 0;
                                        task_res = 5;//成功
                                        task_Flag = 1;

                                        strArr[1] = "取消激活->成功";
                                        ShowData(strArr);
                                    }
                                }
                                else if (task_time == 0)
                                {
                                    st = 2;
                                }
                                break;
                            case 200:
                                break;
                        }
                        break;

                }
            }
        }
      
        //线程开启
        private void RunThread()
        {
            if (ReceivedComTh == null)
            {
                ReceivedComTh = new Thread(new ThreadStart(GetPortBuffer));
                ReceivedComTh.IsBackground = true;
                ReceivedComTh.SetApartmentState(ApartmentState.STA);
                ReceivedComTh.Start();
            }
            if (TimeCntTh == null)
            {
                TimeCntTh = new Thread(new ThreadStart(Timercnt));
                TimeCntTh.IsBackground = true;
                TimeCntTh.SetApartmentState(ApartmentState.STA);
                TimeCntTh.Start();
            }
            if (HouTaiTh == null)
            {
                HouTaiTh = new Thread(new ThreadStart(houtaicaozuo));
                HouTaiTh.IsBackground = true;
                HouTaiTh.SetApartmentState(ApartmentState.STA);
                HouTaiTh.Start();
            }
        }
                
        public FrmUpic()
        {
            InitializeComponent();

            this.combDeviceType.DataSource = Tools.GetComDataTable();
            this.combDeviceType.DisplayMember = "Name";
            this.combDeviceType.ValueMember = "Value";

            InitLogConfig();
            SetDate();

            this.txtComNum.Text = ConfigurationFile.GetKeyVal("COMConfig");
            this.txtBaudRate.Text = ConfigurationFile.GetKeyVal("BaudRate");
            this.txtCount.Text = ConfigurationFile.GetKeyVal("CurrentCount");
            this.combDeviceType.SelectedValue = ConfigurationFile.GetKeyVal("DeviceType");

            Tools.SnLogRec();
            RunThread();

        }

        //串口初始化
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
                //初始化打料数据
                _log.Warn("建立接收上报数据线程");
                //接收上报数据

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
                Tools.CreateDirectory();
                this.btnOpenPort.Text = "串口已经打开";
                this.btnOpenPort.Enabled = false;
            }
            catch (System.Exception ex)
            {
                _logALL.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        //号数+1
        private void btnIncrease_Click(object sender, EventArgs e)
        {
            if (task_Flag == 0)
                return;
            int count = int.Parse(this.txtCount.Text);
            count++;
            this.txtCount.Text = count.ToString();
        }
        //清空
        private void btnEmptyDea_Click(object sender, EventArgs e)
        {
            if (task_Flag == 0)
                return;
            this.txtDeaSN2.Text = string.Empty;
            this.lbldeaStatus.Text = "状态:";
        }
        /// <summary>
        /// 激活
        /// </summary>
        private void btnActive_Click(object sender, EventArgs e)
        {
            if (task_Flag == 0)
                return;
            
            this.lblActStatus.Text = string.Empty;
            this.lblDeActStatus.Text = string.Empty;
            this.txtDeaSN2.Text = string.Empty;
            this.txtSN1.Text = string.Empty;
            this.txtSN2.Text = string.Empty;
            this.lblActStatus.Text = string.Empty;

            if (_serialPort == null || !_serialPort.IsOpen)
            {
                MessageBox.Show("串口没打开", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(this.combDeviceType.Text))//|| string.IsNullOrEmpty(deviceVer))
            {
                MessageBox.Show("请选择设备类型", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dev_type = int.Parse(this.combDeviceType.SelectedValue.ToString());

            string num_year = this.txtYear.Text;
            if (string.IsNullOrEmpty(num_year))
            {
                MessageBox.Show("年不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int year = int.Parse(num_year);
            if (year < 2014 || year > 2099)
            {
                MessageBox.Show("年不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string num_month = this.txtMonth.Text;
            if (string.IsNullOrEmpty(num_month))
            {
                MessageBox.Show("月不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int month = int.Parse(num_month);
            if ((month < 1) || (month > 12))
            {
                MessageBox.Show("月不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string num_count = this.txtCount.Text;
            if (string.IsNullOrEmpty(num_count))
            {
                MessageBox.Show("号不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int count = int.Parse(num_count); 
            if ((count < 1) || (count > 0xffff))
            {
                MessageBox.Show("号不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            use_sn1 = num_year.Substring(2, 2) + "-" + month.ToString("D2") + "-" + count.ToString("00-00");

            ConfigurationFile.UpdateVal("CurrentCount", this.txtCount.Text);
            ConfigurationFile.UpdateVal("DeviceType", this.combDeviceType.SelectedValue.ToString());
       //     ConfigurationFile.UpdateVal("DeviceVer", this.txtDeviceVer.Text);

            task_id = 2;
            task_Flag = 0;
        }
        /// <summary>
        /// 取消激活
        /// </summary>
        private void btnDeactive_Click(object sender, EventArgs e)
        {
            if (task_Flag == 0)
                return;

            this.lblDeActStatus.Text = string.Empty;
            this.txtDeaSN2.Text = string.Empty;
            this.txtSN1.Text = string.Empty;
            this.txtSN2.Text = string.Empty;
            this.lblActStatus.Text = string.Empty;

            if (_serialPort == null || !_serialPort.IsOpen)
            {
                MessageBox.Show("串口没打开", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            task_id = 3;
            task_Flag = 0;
        }
        //写入时间
        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (task_Flag == 0)
                return;

            this.lblDeActStatus.Text = string.Empty;
            this.txtDeaSN2.Text = string.Empty;
            this.txtSN1.Text = string.Empty;
            this.txtSN2.Text = string.Empty;
            this.lblActStatus.Text = string.Empty;

            task_id = 1;
            task_Flag = 0;
        }
        //清空SN
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            if (task_Flag == 0)
                return;
            this.txtSN1.Text = string.Empty;
            this.txtSN2.Text = string.Empty;
        }


        /////////////
        ////////前面通过这个事件激活winform这边的处理函数ShowData（）
        //SendDat += new SendDatDelegate(ShowData);        
        /////////////
        private delegate void ShowTextDelegate(string[] Dat);
        //界面更新委托
        private void ShowData(object Dat)
        {
            if (this.InvokeRequired)
            {
                string[] data = null;
                data = (string[])Dat;//传过来的数据

                object[] obj = { data };

                //异步调用委托
                this.BeginInvoke(new ShowTextDelegate(ShowText), obj);
            }
            else
            {

            }
        }
        //界面上的控件处理
        private void ShowText(string[] Dat)
        {
            try
            {
                //string SubTag = "";

                if (Dat == null)
                    return;               
                
                    if ("WriteTimeLb" == Dat[0])
                    {                        
                        this.lblActStatus.Text = string.Format(Dat[1]);
                        return;
                    }                
                //激活group
                    if ("Active" == Dat[0])
                    {
                        this.lblActStatus.Text = string.Format(Dat[1]);
                        this.txtSN1.Text = string.Format(Dat[2]);
                        this.txtSN2.Text = string.Format(Dat[3]);
                        return;
                    }  

                //取消激活group
                    if ("DeActive" == Dat[0])
                {
                    this.lblDeActStatus.Text = string.Format(Dat[1]);
                    this.txtDeaSN2.Text = string.Format(Dat[2]) + ";"+ string.Format(Dat[3]);
                    return;
                }  
               
            }
            catch (System.Exception ex)
            {
                _logALL.Error(ex.ToString());
            }
        }

        private void btnListenMB_Click(object sender, EventArgs e)
        {



        }    
        private void txtCount_TextChanged(object sender, EventArgs e)
        {

        }
        //
        private void label10_Click(object sender, EventArgs e)
        {

        }
        //
        private void combDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void FrmUpic_Load(object sender, EventArgs e)
        {

        } 
    }
}






/*




        private void WriteActiveData_w()
        {
            string fullyear = this.txtYear.Text.Trim();
            string year = fullyear.Substring(2);//this.txtYear.Text;
            string month = this.txtMonth.Text;
            string count = this.txtCount.Text;

            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(count))
            {
                MessageBox.Show("年、月、排号不能为空");
                return;
            }

            if (count.Length > 4 || int.Parse(count) > 9999)
            {
                MessageBox.Show("排号最大为4位数");
                return;
            }

            byte byear = Convert.ToByte("0x" + year, 16);
            byte bmonth = Convert.ToByte("0x" + month, 16);
            //byte[] bcount = BitConverter.GetBytes(UInt16.Parse(count));
            byte[] bcount = new byte[2];

            if (count.Length <= 2)
            {
                bcount[0] = Convert.ToByte("0x" + count, 16);
            }
            else if (count.Length == 3)
            {
                bcount[0] = Convert.ToByte("0x" + count.Substring(1, 2), 16);
                bcount[1] = Convert.ToByte("0x" + count.Substring(0, 1), 16);
            }
            else if (count.Length == 4)
            {
                bcount[0] = Convert.ToByte("0x" + count.Substring(2, 2), 16);
                bcount[1] = Convert.ToByte("0x" + count.Substring(0, 2), 16);
            }


            byte bdeviceType = byte.Parse(this.combDeviceType.SelectedValue.ToString());
            string deviceVer = this.txtDeviceVer.Text.Trim();
            byte[] bDeviceVer = new byte[2];

            if (!string.IsNullOrEmpty(deviceVer))
            {
                string[] strArr = deviceVer.Split(new char[] { ' ' });

                if (strArr != null && strArr.Length == 2)
                {
                    bDeviceVer[0] = byte.Parse(strArr[0]);
                    bDeviceVer[1] = byte.Parse(strArr[1]);
                }

            }


            //f3 00 00 00 70 00 07 14 07 00 01 01 02 00 00 00
            //                 sn1         硬件版本
            //                               第一个字节表示电路板类别，00表示江强的，01表示彪哥的
            //                            后两个字节表示电路板版本号


            byte[] bsn1 = { byear, bmonth, bcount[1], bcount[0] };

            string strSn1 = Tools.ByteToHexStr(bsn1);
            string strSn2 = System.Guid.NewGuid().ToString();

            this.txtSN1.Text = strSn1;

            string fname = string.Format("{0}-{1}", fullyear, month);

            string msg = "";

            bool CheckSN = Tools.CheckSNExist(strSn1, strSn2, fname, ref msg);


            if (CheckSN == false)
            {
                string text = string.Format("SN1已经存在，不能激活！\r\n请取消激活对应的SN2电路板或手动修改记录文件：snrecord\\{0}.csv:\r\n", fname);
                text += msg;
                _logFile.Debug(text);
                MessageBox.Show(text);
                return;

            }

            

            byte[] CrcBytes = Tools.GetCrcCmd(bytes);

            WriteCommand(CrcBytes);
        }                

        //        激活
        private void HandleQueue()
        {
            System.Threading.Thread.Sleep(1000);
            while (true) //
            {
                System.Threading.Thread.Sleep(100);

                
            }
        }        
		
        private void DealWithReceiveData_w(byte[] bytes)
        {
            string str = string.Format("收到的数据{0}，", Tools.ByteToHexStr(bytes));
            _log.Warn(str);

            byte cmd = bytes[4];
            switch (cmd)
            {
                case 0x61:
                    DealWith61Cmd(bytes);
                    break;
                case 0x71:
                    DealWith71Cmd(bytes);
                    break;
                case 0x73:
                    DealWith73Cmd(bytes);
                    break;
                default:
                    break;

            }

        }
        /// <summary>
        /// 写入时间应答
        /// </summary>
        /// <param name="bytes"></param>
        private void DealWith61Cmd_w(byte[] bytes)
        {
            //激活
            //发送
            //f3 00 00 00 70 00 07 14 07 00 01 01 02 00 00 00

            byte bResult = bytes[7];  //0x01-成功，0x00-失败  

            //激活成功应答
            //F3 A0 00 3D 71 00 11 01 14 07 00 01 05 D7 FF 34 37 30 42 42 43 10 24 44 03 B5
            //        

            byte[] byteSn1 = new byte[4];

            Array.Copy(bytes, 8, byteSn1, 0, 4);

            byte[] byteSn2 = new byte[12];

            Array.Copy(bytes, 12, byteSn2, 0, 12);

            //激活成功应答
            //F3 A0 00 3D 71 00 11 01 14 07 00 01 05 D7 FF 34 37 30 42 42 43 10 24 44 03 B5
            if (bResult == 0x01)
            {
                string strsn2 = Tools.ByteToHexStr(byteSn2);

                string[] strArr = { "WriteTimeLb", "成功写入时间！" };

                ShowData(strArr);
            }
            //激活失败应答
            //F3 A0 00 3D 71 00 11 00 14 07 00 01 05 D7 FF 34 37 30 42 42 43 10 24 44 00 00
            else if (bResult == 0x00)
            {

                string strsn1 = Tools.ByteToHexStr(byteSn1);

                string msg = "写入时间失败！";

                string[] strArr = { "WriteTimeLbErr", msg };

                ShowData(strArr);
            }
        }

        private void DealWith71Cmd_w(byte[] bytes)
        {

            //激活
            //发送
            //f3 00 00 00 70 00 07 14 07 00 01 01 02 00 00 00

            byte bResult = bytes[7];  //0x01-成功，0x00-失败  

            //激活成功应答
            //F3 A0 00 3D 71 00 11 01 14 07 00 01 05 D7 FF 34 37 30 42 42 43 10 24 44 03 B5
            //        

            byte[] byteSn1 = new byte[4];

            Array.Copy(bytes, 8, byteSn1, 0, 4);

            byte[] byteSn2 = new byte[12];

            Array.Copy(bytes, 12, byteSn2, 0, 12);

            //激活成功应答
            //F3 A0 00 3D 71 00 11 01 14 07 00 01 05 D7 FF 34 37 30 42 42 43 10 24 44 03 B5
            if (bResult == 0x01)
            {
                string strsn2 = Tools.ByteToHexStr(byteSn2);

                string[] strArr = { "acsn2", strsn2 };

                ShowData(strArr);
            }
            //激活失败应答
            //F3 A0 00 3D 71 00 11 00 14 07 00 01 05 D7 FF 34 37 30 42 42 43 10 24 44 00 00
            else if (bResult == 0x00)
            {

                string strsn1 = Tools.ByteToHexStr(byteSn1);

                string msg = string.Format("激活失败，该电路板可能已经激活，返回的SN1为：{0}", strsn1);

                string[] strArr = { "acsn2lbl", msg };

                ShowData(strArr);
            }
        }

        private void DealWith73Cmd_w(byte[] bytes)
        {

            //清除激活
            //发送
            //f3 00 00 00 72 00 00 00 00

            byte bResult = bytes[7];  //0x01-成功，0x00-失败  

            byte[] byteSn1 = new byte[4];

            Array.Copy(bytes, 8, byteSn1, 0, 4);

            string strsn1 = Tools.ByteToHexStr(byteSn1);

            byte[] byteSn2 = new byte[12];

            Array.Copy(bytes, 12, byteSn2, 0, 12);

            string strsn2 = Tools.ByteToHexStr(byteSn2);

            //成功应答
            //F3 A0 00 3D 73 00 11 01 14 07 00 01 05 D7 FF 34 37 30 42 42 43 10 24 44 03 B5
            if (bResult == 0x01)
            {
                string[] strArr = { "dacsn2", strsn1, strsn2 };

                ShowData(strArr);
            }
            //失败应答
            //F3 A0 00 3D 73 00 11 00 00 00 00 00 05 D7 FF 34 37 30 42 42 43 10 24 44 00 00
            else if (bResult == 0x00)
            {

                string msg = string.Format("取消激活失败！返回的SN1为：{0}", strsn1);

                string[] strArr = { "dacsn2lbl", msg };

                ShowData(strArr);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //ActiveWriteRecord();

        }  

        private void WriteDateTime()
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                MessageBox.Show("串口没打开", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string deviceType = this.combDeviceType.Text;
            string deviceVer = this.txtDeviceVer.Text;
            string year = this.txtYear.Text;
            string month = this.txtMonth.Text;
            string scount = this.txtCount.Text;

            if (string.IsNullOrEmpty(deviceType) || string.IsNullOrEmpty(deviceVer))
            {
                MessageBox.Show("电路板类别和版本号不能为空", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (deviceVer.Trim().Length != 5 || deviceVer.Trim().IndexOf(" ") < 0)
            {
                MessageBox.Show("电路板版本号格式必须类似为：10 21 ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(scount))
            {
                MessageBox.Show("年、月、排号数不能为空", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (month.StartsWith("0"))
            {
                MessageBox.Show("月份不允许以0开始", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RunThread();

            this.txtSN1.Text = string.Empty;
            this.txtSN2.Text = string.Empty;


            this.lblActStatus.Text = string.Format("写入系统时间:{0}，等待状态返回。", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

            //
            byte[] times = GetTimeByte();

            byte[] CrcBytes = Tools.GetCrcCmd(bytes);

            WriteCommand(CrcBytes);

            _logFile.Debug("写入时间指令。");
            //

            ConfigurationFile.UpdateVal("CurrentCount", this.txtCount.Text);
            ConfigurationFile.UpdateVal("DeviceType", this.combDeviceType.SelectedValue.ToString());
            ConfigurationFile.UpdateVal("DeviceVer", this.txtDeviceVer.Text);
        }        

        private void ActiveWriteRecord()
        {
            string sn1 = this.txtSN1.Text;
            string sn2 = this.txtSN2.Text;
            string year = this.txtYear.Text;
            string month = this.txtMonth.Text;

            if (string.IsNullOrEmpty(sn1) || string.IsNullOrEmpty(sn2))
            {
                MessageBox.Show("SN1或SN2为空，不能保存。");
                return;
            }

            string sval = string.Format("{0},{1}", sn1, sn2);
            string fname = string.Format("{0}-{1}", year, month);


            bool DialogResult = ShowDialog(sn1, sn2, fname);

            if (DialogResult == true)
            {
                return;
            }

            Tools.WriteLog(sval, fname,false);

            MessageBox.Show("成功保存激活记录。");

            this.lblActStatus.Text = string.Empty;
        }

        private void DeActiveWriteRecord(string sn1, string sn2)
        {
            string year = this.txtYear.Text;
            string month = this.txtMonth.Text;

            string sval = string.Format("{0},{1}", sn1, sn2);
            string fname = string.Format("{0}-{1}", year, month);

            string msg = string.Empty;

            Tools.DeActiveSN1(sn1, sn2, fname, ref msg);

            MessageBox.Show("成功消除激活并记录。\r\n" + msg);


            this.lblDeActStatus.Text = string.Empty;
        }

        private bool ShowDialog(string sn1, string sn2, string fname)
        {
            if (ConfigurationFile.bShowDialog == true)
            {
                string msg = string.Empty;

                bool CheckSN = Tools.CheckSNExist(sn1, sn2, fname, ref msg);

                string text = string.Format("已有的记录文件存在以下异常信息，不能保存！\r\n请取消激活或手动修改记录文件：snrecord\\{0}.csv:\r\n", fname);

                text += msg;

                if (CheckSN == false)
                {
                    _logFile.Debug(text);

                    FrmDialog Login = new FrmDialog(text, "已有的记录文件存在异常信息");

                    Login.ShowDialog();//显示登陆窗体   
                    if (Login.DialogResult == DialogResult.Cancel)
                    {
                        MessageBox.Show("已取消保存，此次信息不保存。");

                        _logFile.Debug("已取消保存，此次信息不保存。");

                        return true;
                    }
                }
            }

            return false;
        }

        

        

        
        
    }
}

*/


