using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VolumeControl
{
    class Controller
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        public Controller()
        {
            /*while (true)
            {
                var hWnd = GetForegroundWindow();
                if (hWnd != IntPtr.Zero)
                {
                    uint pID;
                    GetWindowThreadProcessId(hWnd, out pID);
                    if (pID != 0)
                    {
                        Console.WriteLine(GetActiveWindowTitle() + " " + VolumeMixer.GetApplicationVolume(pID));
                        VolumeMixer.SetApplicationVolume(pID, 50f);
                        Console.WriteLine(GetActiveWindowTitle() + " " + VolumeMixer.GetApplicationVolume(pID));
                    }
                }
            }*/
        }
    }
}
