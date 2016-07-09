namespace IntDevs.Upgrade
{
    partial class FrmLogin
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
            this.btnUpic = new System.Windows.Forms.Button();
            this.btnUpgrade = new System.Windows.Forms.Button();
            this.btnListen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUpic
            // 
            this.btnUpic.Enabled = false;
            this.btnUpic.Location = new System.Drawing.Point(54, 54);
            this.btnUpic.Name = "btnUpic";
            this.btnUpic.Size = new System.Drawing.Size(189, 23);
            this.btnUpic.TabIndex = 0;
            this.btnUpic.Text = "序列号管理-请使用条件编译";
            this.btnUpic.UseVisualStyleBackColor = true;
            this.btnUpic.Click += new System.EventHandler(this.btnUpic_Click);
            // 
            // btnUpgrade
            // 
            this.btnUpgrade.Enabled = false;
            this.btnUpgrade.Location = new System.Drawing.Point(54, 112);
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.Size = new System.Drawing.Size(189, 23);
            this.btnUpgrade.TabIndex = 1;
            this.btnUpgrade.Text = "升级-SF-请使用条件编译  ";
            this.btnUpgrade.UseVisualStyleBackColor = true;
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // btnListen
            // 
            this.btnListen.Enabled = false;
            this.btnListen.Location = new System.Drawing.Point(54, 170);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(189, 23);
            this.btnListen.TabIndex = 2;
            this.btnListen.Text = "监听串口-请使用条件编译 ";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.btnUpgrade);
            this.Controls.Add(this.btnUpic);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "序列号写入/升级-SF/监听秘盒";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpic;
        private System.Windows.Forms.Button btnUpgrade;
        private System.Windows.Forms.Button btnListen;
    }
}