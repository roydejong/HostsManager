using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HostsManager
{
    public static class HostsFileManager
    {
        public static List<HostsEntry> entries = new List<HostsEntry>();

        public static string Filename
        {
            get
            {
                return Path.Combine(Environment.SystemDirectory, @"drivers\etc\HOSTS");
            }
        }

        public static void Refresh(RichTextBox rtb, ListView lsv)
        {
            lsv.Items.Clear();
            rtb.Clear();
            entries.Clear();
            
            var raw = File.ReadAllLines(Filename);

            int idx = 0;
            int len = 0;

            foreach (string line in raw)
            {
                if (line.StartsWith("#"))
                {
                    rtb.SelectionColor = Color.DarkGreen;
                    rtb.AppendText(line);
                }
                else
                {
                    string[] parts = line
                        .Replace('\t', ' ')
                        .Trim()
                        .Split(' ');

                    if (parts.Length == 2)
                    {
                        entries.Add(new HostsEntry()
                        {
                            Address = parts[0],
                            Host = parts[1]
                        });

                        rtb.SelectionColor = Color.Blue;
                        rtb.AppendText(parts[0]);
                        rtb.AppendText(" ");
                        rtb.SelectionColor = Color.Purple;
                        rtb.AppendText(parts[1]);

                        var lvi = new ListViewItem()
                        {
                            Text = "Yes"
                        };

                        lvi.SubItems.Add(parts[0]);
                        lvi.SubItems.Add(parts[1]);

                        lsv.Items.Add(lvi);
                    }
                    else
                    {
                        rtb.SelectionColor = Color.Red;
                        rtb.AppendText(line);
                    }
                }

                rtb.AppendText("\r\n");
            }
        }
    }
}
