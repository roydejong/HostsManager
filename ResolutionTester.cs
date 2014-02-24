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

            Dns.BeginGetHostEntry(hostname, onResolveResult, hostname);
        }

        private void onResolveResult(IAsyncResult result)
        {
            IPHostEntry dnsEntry = null;

            try
            {
                dnsEntry = Dns.EndGetHostEntry(result);
            }
            catch (Exception e) { }

            string inHostname = (String)result.AsyncState;

            this.BeginInvoke(new Action(() =>
            {
                if (dnsEntry == null || dnsEntry.AddressList.Length == 0)
                {
                    label2.Text = "Could not resolve " + inHostname;
                }
                else
                {
                    if (dnsEntry.HostName != inHostname)
                    {
                        inHostname = inHostname + " (" + dnsEntry.HostName + ")";
                    }

                    label2.Text = inHostname + " resolves to " + dnsEntry.AddressList[0];
                }
            }));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = textBox1.TextLength > 0;
        }
    }
}
