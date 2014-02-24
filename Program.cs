using System;
using System.Windows.Forms;

namespace HostsManager
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (WindowsSecurity.NeedsAdministratorRights() && !WindowsSecurity.HasAdministratorRights())
            {
                if (!WindowsSecurity.RunElevated())
                {
                    MessageBox.Show("I do not have permission to write to the HOSTS file right now.\n\nPlease run this application as an Administrator.", "Windows Security", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                Environment.Exit(0);
            }

            Application.Run(new MainForm());
        }
    }
}
