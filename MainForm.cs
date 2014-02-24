using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HostsManager
{
    public partial class MainForm : Form
    {
        private ResolutionTester tester;

        public MainForm()
        {
            InitializeComponent();

            Refresh();
        }

        public void Refresh()
        {
            // Status labels
            statusPath.Text = HostsFileManager.Filename;

            // Comments box
            HostsFileManager.Refresh(richTextBox1, listView1);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Process.Start("cmd.exe", "/c \"ipconfig /flushdns\"");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.tester == null || this.tester.IsDisposed)
            {
                this.tester = new ResolutionTester();
                this.tester.FormClosed += new FormClosedEventHandler(tester_FormClosed);
                tester.Show();
            }
            else
            {
                tester.WindowState = FormWindowState.Normal;
                tester.Activate();
            }
        }

        private void tester_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.tester.Dispose();
            this.tester = null;
        }
    }
}
