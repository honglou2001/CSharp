using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;

namespace MutipleClient
{
    public delegate void DisplayEmailSendingWork(string text, RichTextBox txtBox);
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class Client : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RichTextBox txtSendMsg;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.RichTextBox txtRecvMsg;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Button button5;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		byte[] m_dataBuffer = new byte[10];
		IAsyncResult result;
		public AsyncCallback pfnCallBack ;
		private System.Windows.Forms.Label status;
		private System.Windows.Forms.Label label5;
        private CheckBox chkIsByte;
        private TextBox sntext;
        private Label label6;
        private Label label7;
        private TextBox txtContent;
        private ComboBox comCMD;
        private Label label8;
        private Button btnDecode;
		public Socket clientSocket;
        private int LENGTH = 0;

        public static DataTable GetComDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            dt.Rows.Add(new object[] { "注册新手环", "0002" });
            dt.Rows.Add(new object[] { "上传心跳包", "0001" });
            dt.Rows.Add(new object[] { "请求同步时间", "9991" });
            dt.Rows.Add(new object[] { "上传手动定位信息", "9982" });
            dt.Rows.Add(new object[] { "上传监听信息", "4130" });

            dt.Rows.Add(new object[] { "上传计步信息", "7101" });
            dt.Rows.Add(new object[] { "上传下线信息", "7200" });

            //4040 001c 30020000000014 7103 3133373133363839313332 a12a 0d0a
            dt.Rows.Add(new object[] { "上传手机号码-上行", "7103" });

            //2424 0013 30020000000001 7102 3120 1cab 0d0a
            dt.Rows.Add(new object[] { "上传脱落信息", "7102" });

            //2424 0013 30020000000001 7105 data 1cab 0d0a
            //data 1,13713975317 - 312C3133373133393735333137
            //1,18058149508  --312C3138303538313439353038
            dt.Rows.Add(new object[] { "上传回拨信息", "7105" });

            //与中心围栏对应的测试数据
            //http://120.24.176.185:8087/sw/electfence/addElectFence?serialNumber=30020000000010&areaNumber=1&name=n&locationInfo=113.84992,22.738597&scope=500&mode=0

            //7c31307c3130307c3163632c302c323632352c6632612c37652c323632352c6536632c38332c323632352c6633342c38312c323632352c6638642c37652c323632352c6536622c37642c323632352c6637302c37392c323632352c313132382c37387c303030307c30307c3035307c303432

            //7c31307c3130307c3163632c302c323533382c6538302c38632c323533382c313031392c38622c323533382c6536312c38342c323837322c6531622c38342c323533382c6537362c38332c323533382c6535382c38322c323533382c6536322c37667c303030307c30307c3036307c303836

            //7c31307c3130307c3163632c302c323566632c6566382c38662c323566632c6637612c39632c323566632c6566382c38312c323739652c6663612c37352c323739652c313063642c37332c323739652c6538302c37312c323739652c6538302c37307c303030307c30307c3036307c303730
            dt.Rows.Add(new object[] { "上传grps定位信息", "9955" });

            return dt;
        }



        public Client()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

            this.comCMD.DataSource = GetComDataTable();
            this.comCMD.DisplayMember = "Name";
            this.comCMD.ValueMember = "Value";

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}
        private void UpdateText(string s, RichTextBox txtBox)
        {
            if (txtBox.InvokeRequired)
            {
                object[] pList = { s, txtBox };
                txtBox.BeginInvoke(new DisplayEmailSendingWork(UpdateSendingText), pList);
            }
            else
            {
                // 创建该控件的主线程直接更新信息列表
                UpdateSendingText(s, txtBox);
            }
        }
        private void UpdateSendingText(string text, RichTextBox txtBox)
        {
            txtBox.Text += text;//;

        }
		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSendMsg = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.txtRecvMsg = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.chkIsByte = new System.Windows.Forms.CheckBox();
            this.sntext = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.comCMD = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnDecode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(80, 24);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(160, 21);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(80, 56);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(40, 21);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "8012";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "发送信息给服务器:";
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(16, 197);
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(224, 99);
            this.txtSendMsg.TabIndex = 5;
            this.txtSendMsg.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 350);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "连接状态:";
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(80, 349);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(160, 23);
            this.status.TabIndex = 7;
            // 
            // txtRecvMsg
            // 
            this.txtRecvMsg.Location = new System.Drawing.Point(264, 80);
            this.txtRecvMsg.Name = "txtRecvMsg";
            this.txtRecvMsg.ReadOnly = true;
            this.txtRecvMsg.Size = new System.Drawing.Size(224, 220);
            this.txtRecvMsg.TabIndex = 8;
            this.txtRecvMsg.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 32);
            this.button1.TabIndex = 9;
            this.button1.Text = "连接";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(384, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 32);
            this.button2.TabIndex = 10;
            this.button2.Text = "断开";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(280, 307);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 32);
            this.button3.TabIndex = 11;
            this.button3.Text = "清空信息";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(384, 306);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 32);
            this.button4.TabIndex = 12;
            this.button4.Text = "关闭";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(264, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "收到服务器发来的信息:";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(16, 310);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(155, 32);
            this.button5.TabIndex = 14;
            this.button5.Text = "发送信息";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // chkIsByte
            // 
            this.chkIsByte.AutoSize = true;
            this.chkIsByte.Checked = true;
            this.chkIsByte.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsByte.Location = new System.Drawing.Point(144, 96);
            this.chkIsByte.Name = "chkIsByte";
            this.chkIsByte.Size = new System.Drawing.Size(84, 16);
            this.chkIsByte.TabIndex = 15;
            this.chkIsByte.Text = "按字节发送";
            this.chkIsByte.UseVisualStyleBackColor = true;
            // 
            // sntext
            // 
            this.sntext.Location = new System.Drawing.Point(80, 121);
            this.sntext.Name = "sntext";
            this.sntext.Size = new System.Drawing.Size(160, 21);
            this.sntext.TabIndex = 16;
            this.sntext.Text = "30020000000010";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "SN:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "内容:";
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(80, 146);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(160, 21);
            this.txtContent.TabIndex = 18;
            // 
            // comCMD
            // 
            this.comCMD.FormattingEnabled = true;
            this.comCMD.Location = new System.Drawing.Point(80, 171);
            this.comCMD.Name = "comCMD";
            this.comCMD.Size = new System.Drawing.Size(160, 20);
            this.comCMD.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 175);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 21;
            this.label8.Text = "命令:";
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(178, 310);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(75, 33);
            this.btnDecode.TabIndex = 22;
            this.btnDecode.Text = "Decode";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // Client
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(512, 376);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comCMD);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.sntext);
            this.Controls.Add(this.chkIsByte);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtRecvMsg);
            this.Controls.Add(this.status);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSendMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.Name = "Client";
            this.Text = "MutipleClient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
        //[STAThread]
        //static void Main() 
        //{
        //    Application.Run(new Client());
        //}

		// 连接按钮
		private void button1_Click(object sender, System.EventArgs e)
		{
                // IP地址和端口号不能为空
            if (txtIP.Text == "" || txtPort.Text == "")
            {
                MessageBox.Show("请先完整填写服务器IP地址和端口号!", "提示");
                return;
            }

            //this.ConnectAnsy(txtIP.Text.Trim(), Int32.Parse(txtPort.Text), 1024);
            this.ConnectNsyc();
		}


        private void ConnectAnsy(String ip, Int32 port, Int32 length)
        {

            this.LENGTH = length;
            //this.Q = new Queue<byte[]>();

            //DnsEndPoint endPoint = new DnsEndPoint("111.222.333.444", port);
            // 得到服务器的IP地址
            IPAddress ipAddress = IPAddress.Parse(ip);
            // 创建远程终结点
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            this.clientSocket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs argz = new SocketAsyncEventArgs();
            argz.UserToken = this.clientSocket;
            argz.RemoteEndPoint = endPoint;
            argz.Completed += new EventHandler<SocketAsyncEventArgs>(OnSocketConnectCompleted);
            this.clientSocket.ConnectAsync(argz);
        }

        private void OnSocketConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e.RemoteEndPoint,1024);
        }

        private void ProcessReceive(EndPoint endPoint, Int32 length)
        {
 

            byte[] response = new byte[length];

            SocketAsyncEventArgs argz = new SocketAsyncEventArgs();
            argz.UserToken = this.clientSocket;
            argz.RemoteEndPoint = endPoint;

            argz.SetBuffer(response, 0, response.Length);
            argz.Completed += new EventHandler<SocketAsyncEventArgs>(OnSocketReceive);
            //Socket socket = (Socket)this.clientSocket;

            UpdateText("connected", txtRecvMsg);

            this.clientSocket.ReceiveAsync(argz);
        }

        private void OnSocketReceive(object sender, SocketAsyncEventArgs e)
        {
            //Q.Enqueue(e.Buffer);
            if (e.BytesTransferred > 0)
            {
                if (e.SocketError == SocketError.Success)
                {
                    int iRx = e.BytesTransferred;
                    byte[] bytes = new byte[iRx];
                    Array.Copy(e.Buffer, 0, bytes, 0, iRx);
                    //显示为16进制               
                    String szData = Tools.ByteToHexStr(bytes);
                    UpdateText(szData, txtRecvMsg);

                    //Prepare to receive more data
                    Socket socket = (Socket)e.UserToken;
                    byte[] response = new byte[this.LENGTH];
                    e.SetBuffer(response, 0, response.Length);

                    socket.ReceiveAsync(e);
                }
            }
            else
            {
                UpdateText("close", txtRecvMsg);
                this.clientSocket.Close();
            }
        }

        private void ConnectNsyc()
        {
            // IP地址和端口号不能为空
            if (txtIP.Text == "" || txtPort.Text == "")
            {
                MessageBox.Show("请先完整填写服务器IP地址和端口号!", "提示");
                return;
            }
            try
            {
                // 创建Socket实例
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 得到服务器的IP地址
                IPAddress ipAddress = IPAddress.Parse(txtIP.Text);
                Int32 port = Int32.Parse(txtPort.Text);
                // 创建远程终结点
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
                // 连接到远程服务器
                clientSocket.Connect(remoteEP);
                if (clientSocket.Connected)
                {
                    UpdateControls(true);

                    //this.ProcessReceive(remoteEP,1024);
                    WaitForData();  // 异步等待数据
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "提示");
                UpdateControls(false);
            }		
        }
		// 等待数据
		public void WaitForData()
		{
			try
			{
				if(pfnCallBack == null) 
				{
					// 当连接上的客户有写的操作的时候，调用回调函数 
					pfnCallBack = new AsyncCallback(OnDataReceived);
				}
				SocketPacket socketPacket = new SocketPacket();
				socketPacket.thisSocket = clientSocket;
                if (clientSocket != null)
                {
                    result = clientSocket.BeginReceive(socketPacket.dataBuffer, 0, socketPacket.dataBuffer.Length,
                        SocketFlags.None, pfnCallBack, socketPacket);
                }
			}
			catch(SocketException se)
			{
				MessageBox.Show(se.Message, "提示");
			}
		}


		// 该类保存Socket以及发送给服务器的数据
		public class SocketPacket
		{
			public System.Net.Sockets.Socket thisSocket;
			public byte[] dataBuffer = new byte[500]; // 发给服务器的数据
		}

		// 接收数据
		public  void OnDataReceived(IAsyncResult asyn)
		{
			try
			{
				SocketPacket theSockId = (SocketPacket)asyn.AsyncState ;
				// EndReceive完成BeginReceive异步调用，返回服务器写入流的字节数				
				int iRx  = theSockId.thisSocket.EndReceive(asyn);
				// 加 1 是因为字符串以 '\0' 作为结束标志符
                //char[] chars = new char[iRx +  1];
                //// 用UTF8格式来将string信息转化成byte数组形式
                //System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();
                //int charLen = decoder.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                //System.String szData = new System.String(chars);
				// 将收到的信息显示在信息列表中
				//txtRecvMsg.Text = txtRecvMsg.Text + szData;


                byte[] bytes = new byte[iRx];

                Array.Copy(theSockId.dataBuffer, bytes, iRx);

                //显示为16进制               
                String szData = Tools.ByteToHexStr(bytes);

                UpdateText(szData, txtRecvMsg);
				// 等待数据
                System.Threading.Thread.Sleep(2000);

				WaitForData();
			}
			catch (ObjectDisposedException)
			{
				System.Diagnostics.Debugger.Log(0,"1","\nOnDataReceived: Socket已经关闭!\n");
			}
			catch(SocketException se)
			{
				if(se.ErrorCode == 10054) 
				{	
					string msg = "服务器"  + "停止服务!" + "\n";
                    UpdateText(msg, txtRecvMsg);
					clientSocket.Close();
					clientSocket = null;
					UpdateControls(false); 	
				}
				else
				{
					MessageBox.Show(se.Message, "提示");
				}
			}
		}	

		// 更新控件。连接和断开（发送信息）按钮的状态是互斥的
		private void UpdateControls(bool connected) 
		{
			button1.Enabled = !connected;
			button2.Enabled = connected;
			button5.Enabled = connected;
			if(connected)
			{
				status.Text = "已连接";
			}
			else
			{
				status.Text = "无连接";
			}
		}

		// 断开按钮
		private void button2_Click(object sender, System.EventArgs e)
		{
			// 关闭Socket
			if(clientSocket != null)
			{
				clientSocket.Close();
				clientSocket = null;
				UpdateControls(false);
			}
		}

        private byte[] GetFrameData()
        {
            //2424 0011 80680000100076 0002 889e 0d0a  -注册信息
            byte[] head = { 0x24, 0x24 };
            byte[] head1 = { 0x40, 0x40 };
            
            string sn = this.sntext.Text.Trim();
            byte[] snnumber = Tools.HexStringToByteArray(sn);

            string scmd = this.comCMD.SelectedValue.ToString();

            byte[] cmd = Tools.HexStringToByteArray(scmd);

            //byte[] cmd = { 0x00, 0x02 };

            string content = this.txtContent.Text;
            byte[] bytecontent = Tools.HexStringToByteArray(content);

            byte[] end = { 0x0d, 0x0a };

            int lcontent = 0;
            if (bytecontent != null && bytecontent.Length > 0)
            {
                lcontent = bytecontent.Length;
            }

            int ilength = head.Length + 2 + snnumber.Length + cmd.Length + lcontent + 2 + end.Length;
            ushort ulength =(ushort)ilength;
            byte[] bength = BitConverter.GetBytes(ulength);
            Array.Reverse(bength);

            byte[] bytes = new byte[ilength];

            int index = 0;

            if (scmd == "7103")
            {
                Array.Copy(head1, 0, bytes, index, 2);
            }
            else
            {
                Array.Copy(head, 0, bytes, index, 2);
            }

            index = index + head.Length;
            Array.Copy(bength, 0, bytes, index, bength.Length);

            index = index + bength.Length;
            Array.Copy(snnumber, 0, bytes, index, snnumber.Length);


            index = index + snnumber.Length;
            Array.Copy(cmd, 0, bytes, index, cmd.Length);

            index = index + cmd.Length;
            if (lcontent > 0)
            {             
                Array.Copy(bytecontent, 0, bytes, index, bytecontent.Length);
            }

            int icrc = Tools.CRC_XModem(bytes);
            byte[] bcrc = BitConverter.GetBytes((ushort)icrc);
            Array.Reverse(bcrc);

            index = index + lcontent;
            Array.Copy(bcrc, 0, bytes, index, bcrc.Length);

            index = index + bcrc.Length;
            Array.Copy(end, 0, bytes, index, end.Length);

            return bytes;
        }

       

		// 发送信息按钮
		private void button5_Click(object sender, System.EventArgs e)
		{

            bool bIsByte = this.chkIsByte.Checked;

            this.txtSendMsg.Text = Tools.ByteToHexStr(this.GetFrameData());

            string msg = txtSendMsg.Text.Trim();          
            if (string.IsNullOrEmpty(msg))
            {
                MessageBox.Show("消息不允许为空");
                return;
            }
            byte[] byData = System.Text.Encoding.UTF8.GetBytes(msg);

            if (bIsByte == true)
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    msg = msg.Replace("0x","");
                    byData = Tools.HexStringToByteArray(msg);

                    //String[] strArray = msg.Split(new char[]{' ',','});

                    //if (strArray.Length > 0)
                    //{
                    //    byData = new byte[strArray.Length];
                    //    for (int i = 0; i < byData.Length; i++)
                    //    {
                    //        byData[i] = Convert.ToByte(strArray[i],16);
                    //        //byData[i] = byte.Parse(strArray[i]);
                    //    }
                    //}
                }
            }

			try
			{
				// 如果客户与服务器有连接，则允许发送信息
				if(clientSocket.Connected)
				{
				
					// 用UTF8格式来将string信息转化成byte数组形式
					
					if(clientSocket != null)
					{
						// 发送数据
						clientSocket.Send(byData);
					}					
				}	
			}
			catch(Exception se)
			{
				MessageBox.Show(se.Message, "提示");
			}	
		}
	
		// 清空按钮
		private void button3_Click(object sender, System.EventArgs e)
		{
			txtRecvMsg.Clear();  // 清空信息列表
		}

		// 关闭按钮
		private void button4_Click(object sender, System.EventArgs e)
		{
			// 关闭Socket
			if(clientSocket != null)
			{
				clientSocket.Close();
				clientSocket = null;
			}		
			Close();  // 关闭窗体
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			UpdateControls(false); // 初始化时，只有连接按钮可用
		}
        //24240011806800001001220001463c0d0a
        //2424
        //0011
        //80680000100122
        //0001
        //463c
        //0d0a

        private byte[] CheckCrc()
        {
            byte[] crc = null;

            //string s = "40400014300200000000212001401017ffde0d0a";
            //string s = "40400017300200000000212001401017401017";
            //242400868068000010012299557c31307c3130307c3163632c302c323739652c6466352c37652c323461342c313137372c37392c323739652c313034332c37382c323739652c313061662c37352c323739652c6537662c37332c323739652c6466342c37312c323739652c313335372c37307c303030307c30307c3036307c30343371200d0a
            string s = "242400868068000010012299557c31307c3130307c3163632c302c323739652c6466352c37652c323461342c313137372c37392c323739652c313034332c37382c323739652c313061662c37352c323739652c6537662c37332c323739652c6466342c37312c323739652c313335372c37307c303030307c30307c3036307c303433";
            
            //4040001a300200000000212001401017401017401017451f0d0a
            byte[] framedata = Tools.HexStringToByteArray(s);

            System.Diagnostics.Trace.WriteLine(BitConverter.ToString(framedata).Replace("-",",0x"));

            //byte[] framedata = {0x24,0x24,0x00,0x11,0x80,0x68,0x00,0x00,0x10,0x01,0x22,0x00,0x01};

            int crcint= ComputeCRC(framedata, framedata.Length);

           
            ushort us = (ushort)crcint;
            crc = BitConverter.GetBytes(us);

            byte[] crcdata =new byte[framedata.Length+2];

            Array.Copy(framedata, 0, crcdata, 0, framedata.Length);

            //Array.Reverse(crc);

            Array.Copy(crc, 0, crcdata, framedata.Length, 2);

            System.Diagnostics.Trace.WriteLine(BitConverter.ToString(crcdata).Replace("-", ",0x"));

            return crcdata;

        }

        public int ComputeCRC(byte[] val, int len)
        {
            long crc;
            long q;
            byte c;
            int i = 0;

            crc = 0;
            for (i = 0; i < len; i++)
            {
                c = val[i];
                q = (crc ^ c) & 0x0f;
                crc = (crc >> 4) ^ (q * 0x1081);
                q = (crc ^ (c >> 4)) & 0xf;
                crc = (crc >> 4) ^ (q * 0x1081);
            }
            return (byte)crc << 8 | (byte)(crc >> 8);
        }
        private void btnDecode_Click(object sender, EventArgs e)
        {
            //CheckCrc();

            //return;
            //解码内容 begin
            string content = this.txtContent.Text;
            byte[] bytecontent = Tools.HexStringToByteArray(content);
            String strText = System.Text.Encoding.UTF8.GetString(bytecontent);
            this.txtSendMsg.Text = strText;

            //byte[] bcontent = System.Text.Encoding.ASCII.GetBytes(content);
            //this.txtSendMsg.Text = Tools.ByteToHexStr(bcontent);

            //BitArray bitarr = new BitArray(8);
            //bitarr[7] = true;
            //bitarr[0] = true;

            //int[] array = new int[1];

            //bitarr.CopyTo(array, 0);

            //PrintValues(bitarr,8);

            //byte hour = Convert.ToByte("0x" + "06", 16);
            //byte minute = Convert.ToByte("0x" + "12", 16);
            //Console.WriteLine(hour);
            //Console.WriteLine(minute);

            //byte b2 = 12;
            //Console.WriteLine(b2);

            //解码内容 end：
        }
        public static void PrintValues(IEnumerable myList, int myWidth)
        {
            int i = myWidth;
            foreach (Object obj in myList)
            {
                if (i <= 0)
                {
                    i = myWidth;
                    Console.WriteLine();
                }
                i--;
                Console.Write("{0,8}", obj);
            }
            Console.WriteLine();
        }
	}
}
