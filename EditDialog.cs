using System;
using System.Windows.Forms;
using System.Net;

namespace HostsManager
{
    public partial class EditDialog : Form
    {
        private HostsEntry entry;

        public EditDialog(HostsEntry entry = null)
        {
            InitializeComponent();

            this.entry = entry;

            if (this.entry == null)
            {
                this.Text = "Add new entry";
            }
            else
            {
                this.textBox1.Text = entry.Host;
                this.textBox2.Text = entry.Address;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress parsedIp = IPAddress.Parse(textBox2.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid IP address.", "Invalid IP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (textBox1.TextLength == 0)
            {
                MessageBox.Show("Please fill in a hostname.", "Missing hostname", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            bool createNew = false;

            if (entry == null)
            {
                createNew = true;

                entry = new HostsEntry();
                entry.Enabled = true;
            }

            entry.Host = textBox1.Text;
            entry.Address = textBox2.Text;

            if (createNew)
            {
                HostsFileManager.Entries.Add(entry);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
