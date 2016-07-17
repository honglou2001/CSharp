namespace IntDevs.SocketClient
{
    partial class FrmClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.Label();
            this.lblConnect = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstRecBox = new System.Windows.Forms.ListBox();
            this.chkHex = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.txtSendMsg = new System.Windows.Forms.RichTextBox();
            this.txtSelectText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "收到服务器发来的信息:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(132, 325);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 32);
            this.button4.TabIndex = 35;
            this.button4.Text = "关闭";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(28, 326);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 32);
            this.button3.TabIndex = 34;
            this.button3.Text = "清空信息";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(395, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 32);
            this.button2.TabIndex = 33;
            this.button2.Text = "断开";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(291, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 32);
            this.button1.TabIndex = 32;
            this.button1.Text = "连接";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(91, 343);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(160, 23);
            this.status.TabIndex = 30;
            // 
            // lblConnect
            // 
            this.lblConnect.AutoSize = true;
            this.lblConnect.Location = new System.Drawing.Point(27, 371);
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(59, 12);
            this.lblConnect.TabIndex = 29;
            this.lblConnect.Text = "连接状态:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(91, 50);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(40, 21);
            this.txtPort.TabIndex = 26;
            this.txtPort.Text = "8012";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "端口:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(91, 18);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(160, 21);
            this.txtIP.TabIndex = 24;
            this.txtIP.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "服务器IP:";
            // 
            // lstRecBox
            // 
            this.lstRecBox.FormattingEnabled = true;
            this.lstRecBox.ItemHeight = 12;
            this.lstRecBox.Location = new System.Drawing.Point(28, 105);
            this.lstRecBox.Name = "lstRecBox";
            this.lstRecBox.Size = new System.Drawing.Size(223, 208);
            this.lstRecBox.TabIndex = 47;
            this.lstRecBox.SelectedIndexChanged += new System.EventHandler(this.lstRecBox_SelectedIndexChanged);
            // 
            // chkHex
            // 
            this.chkHex.AutoSize = true;
            this.chkHex.Location = new System.Drawing.Point(279, 335);
            this.chkHex.Name = "chkHex";
            this.chkHex.Size = new System.Drawing.Size(66, 16);
            this.chkHex.TabIndex = 51;
            this.chkHex.Text = "HEX字符";
            this.chkHex.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(351, 329);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(150, 32);
            this.button5.TabIndex = 50;
            this.button5.Text = "发送信息";
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(277, 133);
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(224, 180);
            this.txtSendMsg.TabIndex = 49;
            this.txtSendMsg.Text = "";
            // 
            // txtSelectText
            // 
            this.txtSelectText.Location = new System.Drawing.Point(279, 105);
            this.txtSelectText.Name = "txtSelectText";
            this.txtSelectText.Size = new System.Drawing.Size(222, 21);
            this.txtSelectText.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 53;
            this.label3.Text = "选中收到的信息行:";
            // 
            // FrmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 416);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSelectText);
            this.Controls.Add(this.chkHex);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.txtSendMsg);
            this.Controls.Add(this.lstRecBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.lblConnect);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.Name = "FrmClient";
            this.Text = "FrmClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label lblConnect;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstRecBox;
        private System.Windows.Forms.CheckBox chkHex;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.RichTextBox txtSendMsg;
        private System.Windows.Forms.TextBox txtSelectText;
        private System.Windows.Forms.Label label3;
    }
}