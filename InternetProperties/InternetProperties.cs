using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InternetProperties
{
    public class InternetProperties
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static IntPtr FindWindow(string title)
        {
            return FindWindow(null, title);
        }

        public static void SetProtectedMode(List<Zone> zones)
        {
            LaunchInternetProperties();

            var window = FindWindow("Internet Properties");
            ForegroundWindow.ForceForegroundWindow(window);
            ClickSecurityTab(window);

            if (zones.Contains(Zone.Intranet))
            {
                ClickLocalIntranet(window);
                Click(window, "Enable Protected Mode");
            }

            if (zones.Contains(Zone.TrustedSites))
            {
                ClickTrustedSites(window);
                Click(window, "Enable Protected Mode");
            }

            Click(window, "Apply");

            Click(window, "OK");
        }

        private static void LaunchInternetProperties()
        {
            var window2 = FindWindow("Internet Properties");
            if (window2 == IntPtr.Zero)
            {
                // Launch Internet Properties Control Panel inetcpl.cpl
                Process.Start("inetcpl.cpl");
                System.Threading.Thread.Sleep(1000);
            }
        }

        private static void ClickSecurityTab(IntPtr window)
        {
            var rect = new WindowHandleInfo(window).GetWindowRect(window);
            MouseClick.Execute(window, new Point(rect.Left + 90, rect.Top + 46));
            MouseClick.Execute(window, new Point(rect.Left + 90, rect.Top + 46));
            MouseClick.Execute(window, new Point(rect.Left + 90, rect.Top + 46));
            MouseClick.Execute(window, new Point(rect.Left + 90, rect.Top + 46));

            System.Threading.Thread.Sleep(1000);
        }

        private static void ClickTrustedSites(IntPtr window)
        {
            var rect = new WindowHandleInfo(window).GetWindowRect(window);
            MouseClick.Execute(window, new Point(rect.Left + 213, rect.Top + 126));
            System.Threading.Thread.Sleep(1000);
        }

        private static void ClickLocalIntranet(IntPtr window)
        {
            var rect = new WindowHandleInfo(window).GetWindowRect(window);
            MouseClick.Execute(window, new Point(rect.Left + 133, rect.Top + 124));
            System.Threading.Thread.Sleep(1000);
        }

        private static void Click(IntPtr window, string startsWith)
        {
            var children = new WindowHandleInfo(window).GetAllChildHandles()
                .Where(c => WindowTextMatches(startsWith, c))
                .ToList();

            children.ForEach(c => Debug.WriteLine($"{c.WindowText} {c.ClassName}"));

            var pos = new Point(children.FirstOrDefault().Rect.Left + 10, children.FirstOrDefault().Rect.Top + 10);
            MouseClick.Execute(window, pos);

            System.Threading.Thread.Sleep(1000);
        }

        private static bool WindowTextMatches(string startsWith, WindowInfo c)
        {
            var windowText = c.WindowText.ToLower().Replace("&", "");
            return windowText.StartsWith(startsWith.ToLower());
        }
    }
}
