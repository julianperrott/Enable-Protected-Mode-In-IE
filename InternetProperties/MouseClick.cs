using System;
using System.Runtime.InteropServices;

namespace InternetProperties
{
    public static class MouseClick
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public static void Execute(IntPtr window, System.Drawing.Point position)
        {
            System.Windows.Forms.Cursor.Position = position;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)position.X, (uint)position.Y, 0, 0);
        }
    }
}