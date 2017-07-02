using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace thinkforce_desktop
{
    public class WindowHook
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        //public static extern IntPtr PostMessage(IntPtr hWnd, int Msg, int wParam, string lParam);
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static void SendKeystroke(ushort k)
        {
            const uint WM_KEYDOWN = 0x100;
            Process[] thinkforce = Process.GetProcessesByName("thinkforce_desktop.vshost");
            IntPtr result = PostMessage(thinkforce[0].MainWindowHandle, WM_KEYDOWN, ((IntPtr)k), (IntPtr)0);
            Console.WriteLine("Result: " + result);
        }
    }
}
