using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace InternetProperties
{
    internal class Program
    {
        public static void Main()
        {
            var zones = new List<Zone>();
            if (!IsProtectedModeSet(Zone.Intranet)) { zones.Add(Zone.Intranet); }
            if (!IsProtectedModeSet(Zone.TrustedSites)) { zones.Add(Zone.TrustedSites); }

            if (zones.Any())
            {
                InternetProperties.SetProtectedMode(zones);
            }
        }

        private static bool IsProtectedModeSet(Zone zone)
        {
            var key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\" + ((int)zone));
            Object o = key.GetValue("2500");
            bool isEnabled = o.ToString() == "0";
            return isEnabled;
        }
    }
}