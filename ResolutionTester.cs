using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace HostsManager
{
    public partial class ResolutionTester : Form
    {
        public ResolutionTester()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String hostname = textBox1.Text;
            label2.ForeColor = Color.Black;
            label2.Text = "Resolving " + hostname + "...";

            Dns.BeginGetHostEntry(hostname, onResolveResult, null);
        }

        private void onResolveResult(IAsyncResult result)
        {
            IPHostEntry dnsEntry = Dns.EndGetHostEntry(result);

            this.BeginInvoke(new Action(() =>
            {
                if (dnsEntry.AddressList.Length == 0)
                {
                    label2.Text = "Could not resolve " + dnsEntry.HostName;
                }
                else
                {
                    label2.Text = dnsEntry.HostName + " resolves to " + dnsEntry.AddressList[0];
                }
            }));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = textBox1.TextLength > 0;
        }
    }
}
