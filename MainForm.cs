using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace HostsManager
{
    public partial class MainForm : Form
    {
        private ResolutionTester tester;

        public MainForm()
        {
            InitializeComponent();
            Refresh(true);
            UnmarkUnsavedChanges();
        }

        private void UnmarkUnsavedChanges()
        {
            unsavedNotifier.Hide();
        }

        private void MarkUnsavedChanges()
        {
            unsavedNotifier.Show();
            Refresh(false); // soft refresh - update preview
        }

        private bool HasUnsavedChanges()
        {
            return unsavedNotifier.Visible;
        }

        public void Refresh(Boolean fromFile)
        {
            if (fromFile && HasUnsavedChanges() && MessageBox.Show("You have unsaved changes. These will be overwritten.\nProceed anyway?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            statusPath.Text = HostsFileManager.Filename;

            if (fromFile)
            {
                HostsFileManager.RefreshData();
                UnmarkUnsavedChanges();
            }

            SyntaxHighlighter.Highlight(HostsFileManager.GenerateHostsLines(), richTextBox1, listView1);
            listView1_SelectedIndexChanged(null, null);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Process.Start("cmd.exe", "/c \"ipconfig /flushdns\"");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Refresh(true);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toggleEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem selectedItem = GetSelectedEntry();

            if (selectedItem == null)
            {
                return;
            }

            String hostname = (String)selectedItem.Tag;
            HostsEntry entry = HostsFileManager.FindEntry(hostname);

            if (entry == null)
            {
                return;
            }

            entry.Enabled = !entry.Enabled;
            MarkUnsavedChanges();
        }

        private ListViewItem GetSelectedEntry()
        {
            if (listView1.SelectedItems.Count != 1)
            {
                return null;
            }

            return listView1.SelectedItems[0];
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem selectedItem = GetSelectedEntry();

            if (selectedItem == null)
            {
                return;
            }

            String hostname = (String)selectedItem.Tag;
            HostsFileManager.DeleteEntry(hostname);
            MarkUnsavedChanges();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem selectedItem = GetSelectedEntry();

            if (selectedItem == null)
            {
                return;
            }

            String hostname = (String)selectedItem.Tag;
            HostsEntry entry = HostsFileManager.FindEntry(hostname);

            if (entry == null)
            {
                return;
            }

            EditDialog dialog = new EditDialog(entry);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MarkUnsavedChanges();
                this.Refresh(false);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide all edit-specific items in the context menu if there is no active selection, leaving only the "Create new...".
            bool enableEditOptions = GetSelectedEntry() != null;

            foreach (Object item in contextMenuStrip1.Items)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem menuItem = (ToolStripMenuItem)item;
                    menuItem.Visible = enableEditOptions;
                }
                else if (item.GetType() == typeof(ToolStripSeparator))
                {
                    ToolStripSeparator sepItem = (ToolStripSeparator)item;
                    sepItem.Visible = enableEditOptions;
                }
            }

            addNewToolStripMenuItem.Visible = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            HostsFileManager.Save();
            UnmarkUnsavedChanges();
            Refresh(true);
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDialog dialog = new EditDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MarkUnsavedChanges();
                this.Refresh(false);
            }
        }
    }
}
