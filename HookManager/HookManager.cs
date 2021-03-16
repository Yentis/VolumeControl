using System.Windows.Forms;

namespace VolumeControl.HookManager
{
    public static partial class HookManager
    {
        private static event MouseEventHandler? MouseWheelHandler;

        public static event MouseEventHandler MouseWheel
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                MouseWheelHandler += value;
            }
            remove
            {
                MouseWheelHandler -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }
    }
}