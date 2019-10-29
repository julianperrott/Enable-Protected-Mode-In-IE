using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace InternetProperties
{
    public class WindowInfo
    {
        public IntPtr IntPtr { get; set; }

        public string ClassName { get; set; }

        public string WindowText { get; set; }

        public RECT Rect { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    public class WindowHandleInfo
    {
        private const int WM_GETTEXT = 0x000D;
        private const int WM_GETTEXTLENGTH = 0x000E;

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        public RECT GetWindowRect(IntPtr hwnd)
        {
            RECT rect = new RECT();
            GetWindowRect(hwnd, out rect);
            return rect;
        }

        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);

        public static string GetWindowTextRaw(IntPtr hwnd)
        {
            // Allocate correct string length first
            int length = SendMessage(hwnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            StringBuilder sb = new StringBuilder(length + 1);
            SendMessage(hwnd, WM_GETTEXT, (IntPtr)sb.Capacity, sb);
            return sb.ToString();
        }

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)

        {
            this._MainHandle = handle;
        }

        public List<WindowInfo> GetAllChildHandles()

        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);

            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);

                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally

            {
                gcChildhandlesList.Free();
            }

            return childHandles.Select(h => new WindowInfo { IntPtr = h, ClassName = GetClassName(h), WindowText = GetWindowTextRaw(h), Rect = GetWindowRect(h) }).ToList();
        }

        public string GetClassName(IntPtr hWnd)

        {
            int nRet;

            // Pre-allocate 256 characters, since this is the maximum class name length.

            StringBuilder ClassName = new StringBuilder(256);

            //Get the window class name

            nRet = GetClassName(hWnd, ClassName, ClassName.Capacity);

            if (nRet != 0)

            {
                return ClassName.ToString();
            }
            else

            {
                return string.Empty;
            }
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)

        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)

            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;

            childHandles.Add(hWnd);

            return true;
        }
    }
}