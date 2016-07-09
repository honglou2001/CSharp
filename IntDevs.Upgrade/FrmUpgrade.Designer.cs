namespace IntDevs.Upgrade
{
    partial class FrmUpgrade
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtComNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSFAddress = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblFileInfo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSFSN = new System.Windows.Forms.TextBox();
            this.btnUpgrade = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNetWorkNo = new System.Windows.Forms.TextBox();
            this.btnGetSFVer = new System.Windows.Forms.Button();
            this.txtSFVersion = new System.Windows.Forms.TextBox();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnListenMB = new System.Windows.Forms.Button();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBaudRate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtComNum
            // 
            this.txtComNum.Location = new System.Drawing.Point(71, 11);
            this.txtComNum.Name = "txtComNum";
            this.txtComNum.Size = new System.Drawing.Size(105, 21);
            this.txtComNum.TabIndex = 0;
            this.txtComNum.Text = "COM10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "串口号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "SF地址：";
            // 
            // txtSFAddress
            // 
            this.txtSFAddress.Location = new System.Drawing.Point(72, 40);
            this.txtSFAddress.Name = "txtSFAddress";
            this.txtSFAddress.ReadOnly = true;
            this.txtSFAddress.Size = new System.Drawing.Size(287, 21);
            this.txtSFAddress.TabIndex = 2;
            this.txtSFAddress.Text = "C8 3D";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "升级文件|*.bin|所有文件|*.*";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // btnGetFile
            // 
            this.btnGetFile.Location = new System.Drawing.Point(16, 67);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(107, 23);
            this.btnGetFile.TabIndex = 4;
            this.btnGetFile.Text = "获取升级文件  ";
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(128, 69);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(231, 21);
            this.txtFilePath.TabIndex = 5;
            // 
            // lblFileInfo
            // 
            this.lblFileInfo.AutoSize = true;
            this.lblFileInfo.Location = new System.Drawing.Point(371, 104);
            this.lblFileInfo.Name = "lblFileInfo";
            this.lblFileInfo.Size = new System.Drawing.Size(65, 12);
            this.lblFileInfo.TabIndex = 6;
            this.lblFileInfo.Text = "文件信息：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(373, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "SF-SN：";
            // 
            // txtSFSN
            // 
            this.txtSFSN.Location = new System.Drawing.Point(430, 69);
            this.txtSFSN.Name = "txtSFSN";
            this.txtSFSN.ReadOnly = true;
            this.txtSFSN.Size = new System.Drawing.Size(285, 21);
            this.txtSFSN.TabIndex = 8;
            this.txtSFSN.Text = "FF FF FF FF";
            // 
            // btnUpgrade
            // 
            this.btnUpgrade.Location = new System.Drawing.Point(16, 129);
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.Size = new System.Drawing.Size(105, 23);
            this.btnUpgrade.TabIndex = 10;
            this.btnUpgrade.Text = "发送升级文件  ";
            this.btnUpgrade.UseVisualStyleBackColor = true;
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 169);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(700, 251);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "网络号：";
            // 
            // txtNetWorkNo
            // 
            this.txtNetWorkNo.Location = new System.Drawing.Point(430, 42);
            this.txtNetWorkNo.Name = "txtNetWorkNo";
            this.txtNetWorkNo.ReadOnly = true;
            this.txtNetWorkNo.Size = new System.Drawing.Size(285, 21);
            this.txtNetWorkNo.TabIndex = 12;
            this.txtNetWorkNo.Text = "FF FF FF FF FF FF";
            // 
            // btnGetSFVer
            // 
            this.btnGetSFVer.Location = new System.Drawing.Point(16, 98);
            this.btnGetSFVer.Name = "btnGetSFVer";
            this.btnGetSFVer.Size = new System.Drawing.Size(105, 23);
            this.btnGetSFVer.TabIndex = 14;
            this.btnGetSFVer.Text = "得到的SF版本号";
            this.btnGetSFVer.UseVisualStyleBackColor = true;
            this.btnGetSFVer.Click += new System.EventHandler(this.btnGetSFVer_Click_1);
            // 
            // txtSFVersion
            // 
            this.txtSFVersion.Location = new System.Drawing.Point(128, 99);
            this.txtSFVersion.Name = "txtSFVersion";
            this.txtSFVersion.ReadOnly = true;
            this.txtSFVersion.Size = new System.Drawing.Size(231, 21);
            this.txtSFVersion.TabIndex = 15;
            this.txtSFVersion.Text = "1.0.0";
            // 
            // btnSuspend
            // 
            this.btnSuspend.Location = new System.Drawing.Point(128, 129);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(105, 23);
            this.btnSuspend.TabIndex = 16;
            this.btnSuspend.Text = "暂 停";
            this.btnSuspend.UseVisualStyleBackColor = true;
            this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(254, 129);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(105, 23);
            this.btnResume.TabIndex = 17;
            this.btnResume.Text = "继 续";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnListenMB
            // 
            this.btnListenMB.Location = new System.Drawing.Point(374, 129);
            this.btnListenMB.Name = "btnListenMB";
            this.btnListenMB.Size = new System.Drawing.Size(104, 23);
            this.btnListenMB.TabIndex = 18;
            this.btnListenMB.Text = "监听秘盒串口";
            this.btnListenMB.UseVisualStyleBackColor = true;
            this.btnListenMB.Click += new System.EventHandler(this.btnListenMB_Click);
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(373, 5);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(105, 23);
            this.btnOpenPort.TabIndex = 19;
            this.btnOpenPort.Text = "打开串口";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "波特率：";
            // 
            // txtBaudRate
            // 
            this.txtBaudRate.Location = new System.Drawing.Point(254, 7);
            this.txtBaudRate.Name = "txtBaudRate";
            this.txtBaudRate.Size = new System.Drawing.Size(105, 21);
            this.txtBaudRate.TabIndex = 20;
            this.txtBaudRate.Text = "38400";
            // 
            // FrmUpgrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 432);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBaudRate);
            this.Controls.Add(this.btnOpenPort);
            this.Controls.Add(this.btnListenMB);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnSuspend);
            this.Controls.Add(this.txtSFVersion);
            this.Controls.Add(this.btnGetSFVer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNetWorkNo);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnUpgrade);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSFSN);
            this.Controls.Add(this.lblFileInfo);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnGetFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSFAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComNum);
            this.MaximizeBox = false;
            this.Name = "FrmUpgrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线升级V3.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtComNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSFAddress;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnGetFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label lblFileInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSFSN;
        private System.Windows.Forms.Button btnUpgrade;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNetWorkNo;
        private System.Windows.Forms.Button btnGetSFVer;
        private System.Windows.Forms.TextBox txtSFVersion;
        private System.Windows.Forms.Button btnSuspend;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnListenMB;
        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBaudRate;
    }
}

