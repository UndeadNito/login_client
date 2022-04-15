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
    public partial class AdminForm : Form
    {

        private bool MDown = false;
        private Point LastLocation;

        public AdminForm()
        {
            InitializeComponent();
            toolStrip1.Renderer = new MySR();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            LastLocation = e.Location;

            MDown = true;
        }

        private void toolStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MDown)
            {
                Location = new Point(
                (Location.X - LastLocation.X) + e.X, (Location.Y - LastLocation.Y) + e.Y);

                Update();
            }
        }

        private void toolStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            MDown = false;
        }

        private void toolStrip1_MouseLeave(object sender, EventArgs e)
        {
            MDown = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;

            byte secyrityGroup = 1;
            if (checkBox1.Checked) { secyrityGroup = 100; }
            Network.Send(1, textBox1.Text + "!" + textBox2.Text + "!" + textBox3.Text + "!" + secyrityGroup.ToString() + "!");

            switch (Network.Receive(1))
            {
                case ("\0"): label7.Visible = true; break;

                case ("\u0001"): label6.Visible = true; break;

                case ("\u0002"): label8.Visible = true; break;

                default: break;
            }
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Network.IsConnected())
                Network.Disconnect();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label11.Visible = false;
            label13.Visible = false;
            label12.Visible = false;

            Network.Send(2, textBox4.Text + "!");

            switch (Network.Receive(1))
            {
                case ("\0"): label11.Visible = true; break;

                case ("\u0001"): label12.Visible = true; break;

                case ("\u0002"): label13.Visible = true; break;

                default: break;
            }
        }
    }
}
