using System.Runtime.InteropServices;

namespace VolumeControl.HookManager
{
    public static partial class HookManager
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseLLHookStruct
        {
            public Point Point;
            public int MouseData;
        }
    }
}
