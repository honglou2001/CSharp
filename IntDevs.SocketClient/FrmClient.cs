using IntDevs.SocketClient;
using IntDevs.SocketClient.TestAsyncTcpSocketClient;
using MutipleClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IntDevs.SocketClient
{
    public partial class FrmClient : Form
    {
        static AsyncTcpSocketClient _client;

        public FrmClient()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.lblConnect.Text = string.Empty;
            string ip = this.txtIP.Text.Trim();
            int port = int.Parse(this.txtPort.Text.Trim());

            if (_client != null && _client.State == TcpSocketConnectionState.Connected)
            {
                return;
            }

            var config = new AsyncTcpSocketClientConfiguration();
            Action<string> uiAction = UpdateReceiveMsgUI;

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            _client = new AsyncTcpSocketClient(remoteEP, new SimpleMessageDispatcher(), config, uiAction);
            _client.Connect().Wait();

            this.lblConnect.Text = "已连接";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await _client.Close();

            this.lblConnect.Text = "未连接";

        }

        private void UpdateReceiveMsgUI(string info)
        {
            // Is this called from a thread other than the one created
            // the control
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() =>
                {
                    UpdateRecvBox(info);

                }));
            }
        }
        private int msgCount = 0;
        private void UpdateRecvBox(string info)
        {

            this.lstRecBox.Items.Add(info);

            msgCount++;
            if (msgCount > 100)
            {
                this.lstRecBox.Items.Clear();
                msgCount = 0;
            }
            //else
            //{
            //this.txtRecvMsg.SelectionStart = this.txtRecvMsg.Text.Length;
            //this.txtRecvMsg.ScrollToCaret();
            //}
            //this.lstRecBox.Items.Add(info);


            //msgCount++;
            //if (msgCount > 100)
            //{
            //    this.richTextBox1.Clear();
            //    msgCount = 0;
            //}
            //else
            //{
            //    this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            //    this.richTextBox1.ScrollToCaret();
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.lstRecBox.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private async void button5_Click_1(object sender, EventArgs e)
        {
            string msg = this.txtSendMsg.Text;

            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            bool bIsHex = this.chkHex.Checked;
            byte[] byData;

            if (bIsHex == true)
            {
                msg = msg.Replace("0x", "");
                byData = Tools.HexStringToByteArray(msg);
            }
            else
            {
                byData = Encoding.UTF8.GetBytes(msg);
            }

            await _client.SendAsync(byData);
        }

        private void lstRecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.lstRecBox.SelectedItem != null){
                this.txtSelectText.Text = this.lstRecBox.SelectedItem.ToString();
            }
        }
    }
}
