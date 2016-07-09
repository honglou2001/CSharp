using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntDevs.Upgrade
{
    public partial class FrmDialog : Form
    {

        private string _msg = string.Empty;

        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }


        private string _caption = string.Empty;

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public FrmDialog(string msg, string caption)
        {
            this.Msg = msg;
            this.Caption = caption;

            InitializeComponent();


            this.Text = Caption;
            this.rxtMsg.Text = Msg;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();  
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();  
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {


            if (this.checkBox1.Checked)
            {

                if (MessageBox.Show("慎重！勾选将不显示异常信息", "是否显示异常信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    ConfigurationFile.bShowDialog = false;

                    MessageBox.Show("操作成功，在此次运行程序期间将不显示异常信息，重新启动后恢复。");

                }
                else
                {
                    this.checkBox1.Checked = false;

                    ConfigurationFile.bShowDialog = true;
                }
               
            }
            else
            {
                ConfigurationFile.bShowDialog = true;
            }
        }
    }
}
