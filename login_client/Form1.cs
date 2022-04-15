using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace login_client
{
    public partial class Form1 : Form
    {

        private bool MDown;
        private Point LastLocation;

        public Form1()
        {
            InitializeComponent();
            Network.Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Network.IsConnected())
                Network.Disconnect();
            Close();
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MDown = true;
            LastLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MDown)
            {
                Location = new Point(
                (Location.X - LastLocation.X) + e.X, (Location.Y - LastLocation.Y) + e.Y);

                Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            MDown = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { label3.Visible = true; return; }
            if (textBox2.Text == "") { label3.Visible = true; return; }

            Network.Send(0, textBox1.Text + '!' + textBox2.Text + '!');
            label3.Visible = false;
            switch (Network.Receive(1)) {
                case ("\0"):
                    label3.Visible = true;
                    break;
                case ("\u0001"):
                    Hide();
                    UserForm uForm = new UserForm();
                    uForm.Show();
                    break;
                case ("\u0002"):
                    Hide();
                    AdminForm aForm = new AdminForm();
                    aForm.Show();
                    break;
                default:
                    break;
            }
        }
    }
}
