namespace IntDevs.Upgrade
{
    partial class FrmUpic
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
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComNum = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblActStatus = new System.Windows.Forms.Label();
            this.btnIncrease = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSN2 = new System.Windows.Forms.TextBox();
            this.lblsn1 = new System.Windows.Forms.Label();
            this.txtSN1 = new System.Windows.Forms.TextBox();
            this.btnActive = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMonth = new System.Windows.Forms.TextBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblDeActStatus = new System.Windows.Forms.Label();
            this.btnEmptyDea = new System.Windows.Forms.Button();
            this.btnDeactive = new System.Windows.Forms.Button();
            this.lbldeaStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDeaSN2 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDeviceVer = new System.Windows.Forms.TextBox();
            this.combDeviceType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(230, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "波特率：";
            // 
            // txtBaudRate
            // 
            this.txtBaudRate.Location = new System.Drawing.Point(287, 8);
            this.txtBaudRate.Name = "txtBaudRate";
            this.txtBaudRate.Size = new System.Drawing.Size(121, 21);
            this.txtBaudRate.TabIndex = 25;
            this.txtBaudRate.Text = "38400";
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(424, 6);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(89, 23);
            this.btnOpenPort.TabIndex = 24;
            this.btnOpenPort.Text = "打开串口";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "串口号：";
            // 
            // txtComNum
            // 
            this.txtComNum.Location = new System.Drawing.Point(74, 12);
            this.txtComNum.Name = "txtComNum";
            this.txtComNum.Size = new System.Drawing.Size(150, 21);
            this.txtComNum.TabIndex = 22;
            this.txtComNum.Text = "COM1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblActStatus);
            this.groupBox1.Controls.Add(this.btnIncrease);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnEmpty);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtSN2);
            this.groupBox1.Controls.Add(this.lblsn1);
            this.groupBox1.Controls.Add(this.txtSN1);
            this.groupBox1.Controls.Add(this.btnActive);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtCount);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMonth);
            this.groupBox1.Controls.Add(this.txtYear);
            this.groupBox1.Location = new System.Drawing.Point(19, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(513, 145);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // lblActStatus
            // 
            this.lblActStatus.AutoSize = true;
            this.lblActStatus.ForeColor = System.Drawing.Color.Red;
            this.lblActStatus.Location = new System.Drawing.Point(55, 50);
            this.lblActStatus.Name = "lblActStatus";
            this.lblActStatus.Size = new System.Drawing.Size(53, 12);
            this.lblActStatus.TabIndex = 40;
            this.lblActStatus.Tag = "acsn2lbl";
            this.lblActStatus.Text = "        ";
            // 
            // btnIncrease
            // 
            this.btnIncrease.Location = new System.Drawing.Point(404, 78);
            this.btnIncrease.Name = "btnIncrease";
            this.btnIncrease.Size = new System.Drawing.Size(90, 23);
            this.btnIncrease.TabIndex = 39;
            this.btnIncrease.Text = "号数加1";
            this.btnIncrease.UseVisualStyleBackColor = true;
            this.btnIncrease.Click += new System.EventHandler(this.btnIncrease_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(404, 48);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 38;
            this.btnSave.Text = "写入时间";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave1_Click);
            // 
            // btnEmpty
            // 
            this.btnEmpty.Location = new System.Drawing.Point(404, 107);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(90, 23);
            this.btnEmpty.TabIndex = 37;
            this.btnEmpty.Text = "清空-SN";
            this.btnEmpty.UseVisualStyleBackColor = true;
            this.btnEmpty.Click += new System.EventHandler(this.btnEmpty_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(16, 51);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 29;
            this.lblStatus.Tag = "";
            this.lblStatus.Text = "状态：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 35;
            this.label6.Text = "返回的SN2：";
            // 
            // txtSN2
            // 
            this.txtSN2.Location = new System.Drawing.Point(97, 108);
            this.txtSN2.Name = "txtSN2";
            this.txtSN2.Size = new System.Drawing.Size(292, 21);
            this.txtSN2.TabIndex = 36;
            this.txtSN2.Tag = "acsn2";
            // 
            // lblsn1
            // 
            this.lblsn1.AutoSize = true;
            this.lblsn1.Location = new System.Drawing.Point(15, 78);
            this.lblsn1.Name = "lblsn1";
            this.lblsn1.Size = new System.Drawing.Size(71, 12);
            this.lblsn1.TabIndex = 28;
            this.lblsn1.Text = "得到的SN1：";
            // 
            // txtSN1
            // 
            this.txtSN1.Location = new System.Drawing.Point(97, 76);
            this.txtSN1.Name = "txtSN1";
            this.txtSN1.Size = new System.Drawing.Size(292, 21);
            this.txtSN1.TabIndex = 34;
            this.txtSN1.Tag = "acsn1";
            // 
            // btnActive
            // 
            this.btnActive.Location = new System.Drawing.Point(404, 20);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(90, 23);
            this.btnActive.TabIndex = 33;
            this.btnActive.Text = "激 活";
            this.btnActive.UseVisualStyleBackColor = true;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(219, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 32;
            this.label4.Text = "号数：";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(268, 20);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(121, 21);
            this.txtCount.TabIndex = 31;
            this.txtCount.TextChanged += new System.EventHandler(this.txtCount_TextChanged);
            this.txtCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserId_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 30;
            this.label3.Text = "月：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "年：";
            // 
            // txtMonth
            // 
            this.txtMonth.Location = new System.Drawing.Point(136, 20);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(69, 21);
            this.txtMonth.TabIndex = 29;
            this.txtMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserId_KeyPress);
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(45, 20);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(54, 21);
            this.txtYear.TabIndex = 0;
            this.txtYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserId_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblDeActStatus);
            this.groupBox2.Controls.Add(this.btnEmptyDea);
            this.groupBox2.Controls.Add(this.btnDeactive);
            this.groupBox2.Controls.Add(this.lbldeaStatus);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtDeaSN2);
            this.groupBox2.Location = new System.Drawing.Point(19, 257);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(513, 144);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            // 
            // lblDeActStatus
            // 
            this.lblDeActStatus.AutoSize = true;
            this.lblDeActStatus.ForeColor = System.Drawing.Color.Red;
            this.lblDeActStatus.Location = new System.Drawing.Point(55, 60);
            this.lblDeActStatus.Name = "lblDeActStatus";
            this.lblDeActStatus.Size = new System.Drawing.Size(71, 12);
            this.lblDeActStatus.TabIndex = 40;
            this.lblDeActStatus.Tag = "dacsn2lbl";
            this.lblDeActStatus.Text = "           ";
            // 
            // btnEmptyDea
            // 
            this.btnEmptyDea.Location = new System.Drawing.Point(405, 93);
            this.btnEmptyDea.Name = "btnEmptyDea";
            this.btnEmptyDea.Size = new System.Drawing.Size(90, 23);
            this.btnEmptyDea.TabIndex = 38;
            this.btnEmptyDea.Text = "清 空";
            this.btnEmptyDea.UseVisualStyleBackColor = true;
            this.btnEmptyDea.Click += new System.EventHandler(this.btnEmptyDea_Click);
            // 
            // btnDeactive
            // 
            this.btnDeactive.Location = new System.Drawing.Point(14, 19);
            this.btnDeactive.Name = "btnDeactive";
            this.btnDeactive.Size = new System.Drawing.Size(99, 23);
            this.btnDeactive.TabIndex = 37;
            this.btnDeactive.Text = "取 消 激 活";
            this.btnDeactive.UseVisualStyleBackColor = true;
            this.btnDeactive.Click += new System.EventHandler(this.btnDeactive_Click);
            // 
            // lbldeaStatus
            // 
            this.lbldeaStatus.AutoSize = true;
            this.lbldeaStatus.Location = new System.Drawing.Point(16, 61);
            this.lbldeaStatus.Name = "lbldeaStatus";
            this.lbldeaStatus.Size = new System.Drawing.Size(41, 12);
            this.lbldeaStatus.TabIndex = 37;
            this.lbldeaStatus.Text = "状态：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 38;
            this.label8.Text = "返回的SN12：";
            // 
            // txtDeaSN2
            // 
            this.txtDeaSN2.Location = new System.Drawing.Point(97, 95);
            this.txtDeaSN2.Name = "txtDeaSN2";
            this.txtDeaSN2.Size = new System.Drawing.Size(292, 21);
            this.txtDeaSN2.TabIndex = 39;
            this.txtDeaSN2.Tag = "dacsn2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtDeviceVer);
            this.groupBox3.Controls.Add(this.combDeviceType);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(19, 39);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(513, 61);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "电路板";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(407, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "获取系统时间";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(211, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "版本号：";
            // 
            // txtDeviceVer
            // 
            this.txtDeviceVer.Location = new System.Drawing.Point(268, 18);
            this.txtDeviceVer.Name = "txtDeviceVer";
            this.txtDeviceVer.Size = new System.Drawing.Size(121, 21);
            this.txtDeviceVer.TabIndex = 30;
            this.txtDeviceVer.Text = "01 00";
            // 
            // combDeviceType
            // 
            this.combDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combDeviceType.FormattingEnabled = true;
            this.combDeviceType.Location = new System.Drawing.Point(54, 21);
            this.combDeviceType.Name = "combDeviceType";
            this.combDeviceType.Size = new System.Drawing.Size(151, 20);
            this.combDeviceType.TabIndex = 31;
            this.combDeviceType.SelectedIndexChanged += new System.EventHandler(this.combDeviceType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 30;
            this.label7.Text = "类别：";
            // 
            // FrmUpic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 425);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBaudRate);
            this.Controls.Add(this.btnOpenPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComNum);
            this.MaximizeBox = false;
            this.Name = "FrmUpic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "序列号管理-ver2.2";
            this.Load += new System.EventHandler(this.FrmUpic_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBaudRate;
        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtComNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSN2;
        private System.Windows.Forms.Label lblsn1;
        private System.Windows.Forms.TextBox txtSN1;
        private System.Windows.Forms.Button btnActive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMonth;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDeactive;
        private System.Windows.Forms.Label lbldeaStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDeaSN2;
        private System.Windows.Forms.Button btnEmpty;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEmptyDea;
        private System.Windows.Forms.Button btnIncrease;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDeviceVer;
        private System.Windows.Forms.ComboBox combDeviceType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblActStatus;
        private System.Windows.Forms.Label lblDeActStatus;
        private System.Windows.Forms.Label label10;
    }
}