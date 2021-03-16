using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VolumeControl.HookManager
{
    public static partial class HookManager
    {
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        private static HookProc? MouseDelegateHook;

        private static int s_MouseHookHandle;

        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode < 0) return CallNextHookEx(s_MouseHookHandle, nCode, wParam, lParam);
            var mouseHookStruct = (MouseLLHookStruct?)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct)) ?? default;
            short mouseDelta = 0;

            if (wParam == WM_MOUSEWHEEL)
            {
                mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
            }

            var mouseEvent = new MouseEventExtArgs(
                MouseButtons.None,
                0,
                mouseHookStruct.Point.X,
                mouseHookStruct.Point.Y,
                mouseDelta
            );

            if (MouseWheelHandler != null && mouseDelta != 0)
            {
                MouseWheelHandler.Invoke(null, mouseEvent);
            }

            if (mouseEvent.Handled)
            {
                return -1;
            }

            return CallNextHookEx(s_MouseHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalMouseEvents()
        {
            if (s_MouseHookHandle != 0) return;

            MouseDelegateHook = MouseHookProc;
            s_MouseHookHandle = SetWindowsHookEx(
                WH_MOUSE_LL,
                MouseDelegateHook,
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                0
            );

            if (s_MouseHookHandle == 0)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode);
            }
        }

        private static void TryUnsubscribeFromGlobalMouseEvents()
        {
            if (MouseWheelHandler == null)
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (s_MouseHookHandle == 0) return;

            int result = UnhookWindowsHookEx(s_MouseHookHandle);
            s_MouseHookHandle = 0;
            MouseDelegateHook = null;

            if (result == 0)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode);
            }
        }
    }
}
