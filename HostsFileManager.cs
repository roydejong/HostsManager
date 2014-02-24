using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace HostsManager
{
    public static class HostsFileManager
    {
        public const string DISABLED_INDICATOR = "# disabled ";

        public static List<HostsEntry> Entries = new List<HostsEntry>();

        public static string Filename
        {
            get
            {
                return Path.Combine(Environment.SystemDirectory, @"drivers\etc\HOSTS");
            }
        }

        public static HostsEntry FindEntry(String hostname)
        {
            foreach (HostsEntry entry in Entries)
            {
                if (entry.Host == hostname)
                {
                    return entry;
                }
            }

            return null;
        }

        public static void DeleteEntry(String hostname)
        {
            foreach (HostsEntry entry in Entries)
            {
                if (entry.Host == hostname)
                {
                    Entries.Remove(entry);
                    break;
                }
            }
        }

        public static string GenerateHostsText()
        {
            string[] lines = GenerateHostsLines();
            StringBuilder outText = new StringBuilder();

            foreach (string ln in lines)
            {
                outText.Append(ln);
                outText.Append(Environment.NewLine);
            }

            return outText.ToString();
        }

        public static string[] GenerateHostsLines()
        {
            List<string> outLines = new List<string>();
            List<string> seenHosts = new List<string>();

            var raw = File.ReadAllLines(Filename);

            foreach (string line in raw)
            {
                bool isDisabledEntry = line.StartsWith(DISABLED_INDICATOR);

                if (line.StartsWith("#") && !isDisabledEntry)
                {
                    outLines.Add(line);
                    continue;
                }

                string aLine = line;

                if (isDisabledEntry)
                {
                    aLine = line.Substring(DISABLED_INDICATOR.Length).Trim();
                }

                string[] parts = aLine
                        .Replace('\t', ' ')
                        .Trim()
                        .Split(' ');

                if (parts.Length == 2)
                {
                    String address = parts[0];
                    String host = parts[1];

                    if (seenHosts.Contains(host))
                    {
                        continue;
                    }

                    seenHosts.Add(host);

                    HostsEntry customEntry = FindEntry(host);

                    // If we do not have a mutation for this line it has been deleted in the editor (or added after saving)
                    if (customEntry == null)
                    {
                        continue;
                    }
                    // If we DO have an entry, it might have been mutated by the editor, so provide an alternate version of this line
                    else
                    {
                        StringBuilder customLine = new StringBuilder();

                        if (!customEntry.Enabled)
                        {
                            customLine.Append(DISABLED_INDICATOR);
                        }

                        customLine.Append(customEntry.Address);
                        customLine.Append(" ");
                        customLine.Append(customEntry.Host);

                        outLines.Add(customLine.ToString());
                    }
                }
                else
                {
                    outLines.Add(line);
                }
            }

            foreach (HostsEntry customEntry in Entries)
            {
                if (seenHosts.Contains(customEntry.Host))
                {
                    continue;
                }

                StringBuilder customLine = new StringBuilder();

                if (!customEntry.Enabled)
                {
                    customLine.Append(DISABLED_INDICATOR);
                }

                customLine.Append(customEntry.Address);
                customLine.Append(" ");
                customLine.Append(customEntry.Host);

                outLines.Add(customLine.ToString());
            }

            return outLines.ToArray();
        }

        public static void RefreshData()
        {
            Entries.Clear();

            var raw = File.ReadAllLines(Filename);

            foreach (string line in raw)
            {
                bool isDisabledEntry = line.StartsWith(DISABLED_INDICATOR);

                if (line.StartsWith("#") && !isDisabledEntry)
                {
                    continue;
                }

                string aLine = line;

                if (isDisabledEntry)
                {
                    aLine = line.Substring(DISABLED_INDICATOR.Length).Trim();
                }

                string[] parts = aLine
                        .Replace('\t', ' ')
                        .Trim()
                        .Split(' ');

                if (parts.Length != 2)
                {
                    continue;
                }

                Entries.Add(new HostsEntry()
                {
                    Address = parts[0],
                    Host = parts[1],
                    Enabled = !isDisabledEntry
                });
            }
        }

        public static void Save()
        {
            File.WriteAllLines(HostsFileManager.Filename, GenerateHostsLines());
        }
    }
}
