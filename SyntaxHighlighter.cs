using System;
using System.Drawing;
using System.Windows.Forms;

namespace HostsManager
{
    public static class SyntaxHighlighter
    {
        public static void Highlight(String[] raw, RichTextBox rtb, ListView lsv)
        {
            lsv.Items.Clear();
            rtb.Clear();

            foreach (string line in raw)
            {
                bool isDisabledEntry = line.StartsWith(HostsFileManager.DISABLED_INDICATOR);

                if (line.StartsWith("#") && !isDisabledEntry)
                {
                    rtb.SelectionColor = Color.DarkGreen;
                    rtb.AppendText(line);
                }
                else
                {
                    string aLine = line;

                    if (isDisabledEntry)
                    {
                        aLine = line.Substring(HostsFileManager.DISABLED_INDICATOR.Length).Trim();
                    }

                    string[] parts = aLine
                        .Replace('\t', ' ')
                        .Trim()
                        .Split(' ');

                    if (parts.Length == 2)
                    {
                        String Address = parts[0];
                        String Host = parts[1];

                        if (isDisabledEntry)
                        {
                            rtb.SelectionColor = Color.DarkGreen;
                            rtb.AppendText(HostsFileManager.DISABLED_INDICATOR);
                            rtb.SelectionColor = Color.Gray;
                            rtb.AppendText(parts[0]);
                            rtb.AppendText(" ");
                            rtb.SelectionColor = Color.DarkGray;
                            rtb.AppendText(parts[1]);
                        }
                        else
                        {
                            rtb.SelectionColor = Color.Blue;
                            rtb.AppendText(parts[0]);
                            rtb.AppendText(" ");
                            rtb.SelectionColor = Color.Purple;
                            rtb.AppendText(parts[1]);
                        }

                        var lvi = new ListViewItem()
                        {
                            Text = isDisabledEntry ? "No" : "Yes"
                        };

                        lvi.SubItems.Add(Host);
                        lvi.SubItems.Add(Address);
                        lvi.Tag = Host;

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
