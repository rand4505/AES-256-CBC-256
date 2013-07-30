using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AESEncryptAndDecrypt
{
    public partial class GetString : Form
    {
        private string _Password;
        public string Password { get { return _Password; } set { _Password = value; } }
        public void ClearPass() { Password = "".ToString(); textBox1.Text = "".ToString(); }

        public GetString()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Password = textBox1.Text.Trim();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
