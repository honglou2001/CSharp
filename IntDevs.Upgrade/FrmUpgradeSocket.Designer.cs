﻿namespace IntDevs.Upgrade
{
    partial class FrmUpgradeSocket
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
            this.txtBaudRate = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.btnGetSFVer = new System.Windows.Forms.Button();
            this.btnUpgrade = new System.Windows.Forms.Button();
            this.lblFileInfo = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComNum = new System.Windows.Forms.TextBox();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lstBoxRec = new System.Windows.Forms.ListBox();
            this.收到信息 = new System.Windows.Forms.Label();
            this.txtSendManual = new System.Windows.Forms.TextBox();
            this.btnSendManual = new System.Windows.Forms.Button();
            this.chkHex = new System.Windows.Forms.CheckBox();
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRecLog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(193, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 42;
            this.label5.Text = "端口号：";
            // 
            // txtBaudRate
            // 
            this.txtBaudRate.Location = new System.Drawing.Point(250, 14);
            this.txtBaudRate.Name = "txtBaudRate";
            this.txtBaudRate.Size = new System.Drawing.Size(105, 21);
            this.txtBaudRate.TabIndex = 41;
            this.txtBaudRate.Text = "8012";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(369, 12);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(105, 23);
            this.btnListen.TabIndex = 40;
            this.btnListen.Text = "打开监听  ";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(250, 71);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(105, 23);
            this.btnResume.TabIndex = 38;
            this.btnResume.Text = "继 续";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnSuspend
            // 
            this.btnSuspend.Location = new System.Drawing.Point(124, 71);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(105, 23);
            this.btnSuspend.TabIndex = 37;
            this.btnSuspend.Text = "暂 停";
            this.btnSuspend.UseVisualStyleBackColor = true;
            this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
            // 
            // btnGetSFVer
            // 
            this.btnGetSFVer.Location = new System.Drawing.Point(369, 42);
            this.btnGetSFVer.Name = "btnGetSFVer";
            this.btnGetSFVer.Size = new System.Drawing.Size(105, 23);
            this.btnGetSFVer.TabIndex = 35;
            this.btnGetSFVer.Text = "升级包版本";
            this.btnGetSFVer.UseVisualStyleBackColor = true;
            // 
            // btnUpgrade
            // 
            this.btnUpgrade.Location = new System.Drawing.Point(12, 71);
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.Size = new System.Drawing.Size(105, 23);
            this.btnUpgrade.TabIndex = 31;
            this.btnUpgrade.Text = "发送升级文件  ";
            this.btnUpgrade.UseVisualStyleBackColor = true;
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // lblFileInfo
            // 
            this.lblFileInfo.AutoSize = true;
            this.lblFileInfo.Location = new System.Drawing.Point(480, 45);
            this.lblFileInfo.Name = "lblFileInfo";
            this.lblFileInfo.Size = new System.Drawing.Size(65, 12);
            this.lblFileInfo.TabIndex = 28;
            this.lblFileInfo.Text = "文件信息：";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(124, 45);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(231, 21);
            this.txtFilePath.TabIndex = 27;
            // 
            // btnGetFile
            // 
            this.btnGetFile.Location = new System.Drawing.Point(12, 43);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(107, 23);
            this.btnGetFile.TabIndex = 26;
            this.btnGetFile.Text = "获取升级文件  ";
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "升级文件|*.bin|所有文件|*.*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "服务器IP：";
            // 
            // txtComNum
            // 
            this.txtComNum.Location = new System.Drawing.Point(78, 18);
            this.txtComNum.Name = "txtComNum";
            this.txtComNum.Size = new System.Drawing.Size(105, 21);
            this.txtComNum.TabIndex = 22;
            this.txtComNum.Text = "127.0.0.1";
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(478, 17);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(65, 12);
            this.lblServerStatus.TabIndex = 43;
            this.lblServerStatus.Text = "运行状态：";
            // 
            // lstClients
            // 
            this.lstClients.FormattingEnabled = true;
            this.lstClients.ItemHeight = 12;
            this.lstClients.Location = new System.Drawing.Point(12, 132);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(217, 292);
            this.lstClients.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "连接上服务器的客户:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(250, 132);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(519, 124);
            this.listBox1.TabIndex = 46;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(248, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 47;
            this.label2.Text = "发送的消息:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(343, 105);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(132, 16);
            this.checkBox1.TabIndex = 48;
            this.checkBox1.Text = "是否记录日志到文本";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lstBoxRec
            // 
            this.lstBoxRec.FormattingEnabled = true;
            this.lstBoxRec.ItemHeight = 12;
            this.lstBoxRec.Location = new System.Drawing.Point(250, 287);
            this.lstBoxRec.Name = "lstBoxRec";
            this.lstBoxRec.Size = new System.Drawing.Size(519, 100);
            this.lstBoxRec.TabIndex = 49;
            this.lstBoxRec.SelectedIndexChanged += new System.EventHandler(this.lstBoxRec_SelectedIndexChanged);
            // 
            // 收到信息
            // 
            this.收到信息.AutoSize = true;
            this.收到信息.Location = new System.Drawing.Point(248, 263);
            this.收到信息.Name = "收到信息";
            this.收到信息.Size = new System.Drawing.Size(71, 12);
            this.收到信息.TabIndex = 50;
            this.收到信息.Text = "收到的消息:";
            // 
            // txtSendManual
            // 
            this.txtSendManual.Location = new System.Drawing.Point(250, 400);
            this.txtSendManual.Name = "txtSendManual";
            this.txtSendManual.Size = new System.Drawing.Size(341, 21);
            this.txtSendManual.TabIndex = 51;
            // 
            // btnSendManual
            // 
            this.btnSendManual.Location = new System.Drawing.Point(668, 400);
            this.btnSendManual.Name = "btnSendManual";
            this.btnSendManual.Size = new System.Drawing.Size(101, 23);
            this.btnSendManual.TabIndex = 52;
            this.btnSendManual.Text = "手动发信息";
            this.btnSendManual.UseVisualStyleBackColor = true;
            this.btnSendManual.Click += new System.EventHandler(this.btnSendManual_Click);
            // 
            // chkHex
            // 
            this.chkHex.AutoSize = true;
            this.chkHex.Location = new System.Drawing.Point(596, 403);
            this.chkHex.Name = "chkHex";
            this.chkHex.Size = new System.Drawing.Size(66, 16);
            this.chkHex.TabIndex = 53;
            this.chkHex.Text = "HEX字符";
            this.chkHex.UseVisualStyleBackColor = true;
            // 
            // txtSelect
            // 
            this.txtSelect.Location = new System.Drawing.Point(570, 100);
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(199, 21);
            this.txtSelect.TabIndex = 54;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(481, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 55;
            this.label3.Text = "选中的消息行:";
            // 
            // chkRecLog
            // 
            this.chkRecLog.AutoSize = true;
            this.chkRecLog.Location = new System.Drawing.Point(343, 265);
            this.chkRecLog.Name = "chkRecLog";
            this.chkRecLog.Size = new System.Drawing.Size(132, 16);
            this.chkRecLog.TabIndex = 56;
            this.chkRecLog.Text = "是否记录日志到文本";
            this.chkRecLog.UseVisualStyleBackColor = true;
            this.chkRecLog.CheckedChanged += new System.EventHandler(this.chkRecLog_CheckedChanged);
            // 
            // FrmUpgradeSocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 433);
            this.Controls.Add(this.chkRecLog);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSelect);
            this.Controls.Add(this.chkHex);
            this.Controls.Add(this.btnSendManual);
            this.Controls.Add(this.txtSendManual);
            this.Controls.Add(this.收到信息);
            this.Controls.Add(this.lstBoxRec);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lstClients);
            this.Controls.Add(this.lblServerStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBaudRate);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnSuspend);
            this.Controls.Add(this.btnGetSFVer);
            this.Controls.Add(this.btnUpgrade);
            this.Controls.Add(this.lblFileInfo);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnGetFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComNum);
            this.Name = "FrmUpgradeSocket";
            this.Text = "TCP 升级 SocketServer—V1.0.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBaudRate;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnSuspend;
        private System.Windows.Forms.Button btnGetSFVer;
        private System.Windows.Forms.Button btnUpgrade;
        private System.Windows.Forms.Label lblFileInfo;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnGetFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtComNum;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ListBox lstBoxRec;
        private System.Windows.Forms.Label 收到信息;
        private System.Windows.Forms.TextBox txtSendManual;
        private System.Windows.Forms.Button btnSendManual;
        private System.Windows.Forms.CheckBox chkHex;
        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRecLog;

    }
}