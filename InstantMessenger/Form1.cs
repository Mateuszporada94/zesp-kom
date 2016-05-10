using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace InstantMessenger
{
    public partial class Form1 : Form
    {
        IMClient im = new IMClient();

        public Form1()
        {
            InitializeComponent();

            // InstantMessenger Events
            im.LoginOK += new EventHandler(im_LoginOK);
            im.RegisterOK += new EventHandler(im_RegisterOK);
            im.LoginFailed += new IMErrorEventHandler(im_LoginFailed);
            im.RegisterFailed += new IMErrorEventHandler(im_RegisterFailed);
            im.Disconnected += new EventHandler(im_Disconnected);
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            LogRegForm info = new LogRegForm();
            if (info.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                im.Register(info.UserName, info.Password);
                status.Text = "Registering...";
            }
        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            LogRegForm info = new LogRegForm();
            if (info.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                im.Login(info.UserName, info.Password);
                status.Text = "Login...";
            }
        }

        void im_LoginOK(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                status.Text = "Logged in!";
                registerButton.Enabled = false;
                loginButton.Enabled = false;
                logoutButton.Enabled = true;
                talkButton.Enabled = true;
            }));
        }
        void im_RegisterOK(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                status.Text = "Registered!";
                registerButton.Enabled = false;
                loginButton.Enabled = false;
                logoutButton.Enabled = true;
                talkButton.Enabled = true;
            }));
        }
        void im_LoginFailed(object sender, IMErrorEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                status.Text = "Login failed!";
            }));
        }
        void im_RegisterFailed(object sender, IMErrorEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                status.Text = "Register failed!";
            }));
        }
        void im_Disconnected(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                status.Text = "Disconnected!";
                registerButton.Enabled = true;
                loginButton.Enabled = true;
                logoutButton.Enabled = false;
                talkButton.Enabled = false;

                foreach (TalkForm tf in talks)
                    tf.Close();
            }));
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            im.Disconnect();
        }

        List<TalkForm> talks = new List<TalkForm>();
        private void talkButton_Click(object sender, EventArgs e)
        {
            TalkForm tf = new TalkForm(im, sendTo.Text);
            sendTo.Text = "";
            talks.Add(tf);
            tf.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            im.Disconnect();
        }
    }
}
