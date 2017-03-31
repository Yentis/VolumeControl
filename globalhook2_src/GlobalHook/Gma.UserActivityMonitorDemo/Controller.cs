using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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

        private bool modifiable = false;
        private uint pID;
        private IntPtr hWnd;
        public Controller()
        {
            refreshCurrentWindow();
        }

        public void refreshCurrentWindow()
        {
            hWnd = GetForegroundWindow();
            if (hWnd != IntPtr.Zero)
            {
                GetWindowThreadProcessId(hWnd, out pID);
                if (pID != 0)
                {
                    modifiable = true;
                } else
                {
                    modifiable = false;
                }
            }
        }

        public void VolumeUp()
        {
            if(VolumeMixer.GetApplicationVolume(pID) != null && modifiable)
            {
                float currentVolume = (float)VolumeMixer.GetApplicationVolume(pID);
                VolumeMixer.SetApplicationVolume(pID, currentVolume += 1);
            }
        }

        public void VolumeDown()
        {
            if (VolumeMixer.GetApplicationVolume(pID) != null && modifiable)
            {
                float currentVolume = (float)VolumeMixer.GetApplicationVolume(pID);
                VolumeMixer.SetApplicationVolume(pID, currentVolume -= 1);
            }
        }
    }
}
