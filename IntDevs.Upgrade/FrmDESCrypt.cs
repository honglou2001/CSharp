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
    public partial class FrmDESCrypt : Form
    {
        public FrmDESCrypt()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            this.txtEncrypt.Text = "";

            string key = ConfigurationFile.GetKeyVal("Deskey");


            if (string.IsNullOrEmpty(key) || key.Trim().Length < 8)
            {
                MessageBox.Show("密钥不存在或长度不正确。");
                return;
            }
            key = key.Trim();

            key = Tools.DesDecrypt(key, ConfigurationFile.keystr);

            string str = this.txtString.Text;

            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("加密字符串不能为空");
                return;
            }

            string entstr = Tools.DesEncrypt(str.Trim(), key);

            this.txtEncrypt.Text = entstr;

            Program._logALL.Info(string.Format("加密前：{0},加密后:{1}", str.Trim(), entstr));



        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            this.txtString.Text = "";

            string key = ConfigurationFile.GetKeyVal("Deskey");

            if (string.IsNullOrEmpty(key) || key.Trim().Length < 8)
            {
                MessageBox.Show("密钥不存在或长度不正确。");
                return;
            }
            key = key.Trim();

            key = Tools.DesDecrypt(key, ConfigurationFile.keystr);
            string str = this.txtEncrypt.Text;

            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("要解密字符串不能为空");
                return;
            }

            string detstr = Tools.DesDecrypt(str.Trim(), key);

            this.txtString.Text = detstr;



            Program._logALL.Info(string.Format("解密前：{0},解密后:{1}", str.Trim(), detstr));

        }
    }
}
