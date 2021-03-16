using System;
using System.Windows.Forms;

namespace VolumeControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            HookManager.HookManager.MouseWheel += HookManager_MouseWheel;
        }

        private void ExitToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HookManager_MouseWheel(object? sender, MouseEventArgs e)
        {
            var pID = WindowManager.GetProcessID() ?? 0;
            if (pID == 0 || e.Delta == 0) return;

            VolumeMixer.ChangeVolume(pID, e.Delta < 0 ? 1 : -1);
        }
    }
}
