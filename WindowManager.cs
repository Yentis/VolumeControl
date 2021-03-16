using System;
using System.Runtime.InteropServices;

namespace VolumeControl
{
    public class WindowManager
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public static uint? GetProcessID()
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return null;

            _ = GetWindowThreadProcessId(hWnd, out uint pID);
            if (pID == 0) return null;

            return pID;
        }
    }
}