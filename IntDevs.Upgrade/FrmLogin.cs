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
    public partial class FrmLogin : Form
    {
        private int _ftype = 0;

        public int Ftype
        {
            get { return _ftype; }
            set { _ftype = value; }
        }

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnUpic_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态   
            this.Ftype = 1;
            this.Close();  
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            ConfigurationFile.APPMode = 1;

            this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态   
            this.Ftype = 2;
            this.Close();  

        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            ConfigurationFile.APPMode = 2;

            this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
            this.Ftype = 3;
            this.Close();  
        }
    }
}
