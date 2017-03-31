using System;
using System.Windows.Forms;
using Gma.UserActivityMonitor;
using VolumeControl;

namespace Gma.UserActivityMonitorDemo
{
    public partial class TestFormStatic : Form
    {
        Controller controller = new Controller();
        public TestFormStatic()
        {
            InitializeComponent();
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HookManager.MouseWheel += HookManager_MouseWheel;
        }

        //##################################################################
        #region Event handlers of particular events. They will be activated when an appropriate checkbox is checked.

        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            controller.refreshCurrentWindow();
            if (e.Delta > 0)
            {
                controller.VolumeDown();
            } else
            {
                controller.VolumeUp();
            }
        }

        #endregion
    }
}