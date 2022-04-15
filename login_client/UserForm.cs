using System;
using System.IO;
using System.Security.Cryptography;
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
    public partial class UserForm : Form
    {
        string checkSum = string.Empty;

        private bool MDown = false;
        private Point LastLocation;
        private byte[] checkSumBytes;

        public UserForm()
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

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Network.IsConnected())
                Network.Disconnect();
            Application.Exit();
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            if (!File.Exists(fileName)) { return; }
            textBox1.Text = fileName;

            using (SHA256 shaCalculator = SHA256.Create())
            {
                FileStream fileStream = File.OpenRead(fileName);
                fileStream.Position = 0;

                checkSumBytes = shaCalculator.ComputeHash(fileStream);

                foreach (byte bte in checkSumBytes)
                {
                    checkSum += bte.ToString("X2");
                }
            }

            textBox2.Text = checkSum;
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
