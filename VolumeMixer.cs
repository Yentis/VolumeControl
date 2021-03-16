using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace VolumeControl
{
    [SupportedOSPlatform("windows")]
    public class VolumeMixer
    {
        public static void ChangeVolume(uint pID, int amount = 1)
        {
            var currentVolume = GetApplicationVolume(pID) ?? -1;
            if (currentVolume == -1) return;

            SetApplicationVolume(pID, currentVolume += amount);
        }

        private static float? GetApplicationVolume(uint pID)
        {
            var volume = GetVolumeObject(pID);
            if (volume == null) return null;

            volume.GetMasterVolume(out float level);
            Marshal.ReleaseComObject(volume);

            return level * 100;
        }

        private static void SetApplicationVolume(uint pID, float level)
        {
            var volume = GetVolumeObject(pID);
            if (volume == null) return;

            var guid = Guid.Empty;
            volume.SetMasterVolume(level / 100, ref guid);
            Marshal.ReleaseComObject(volume);
        }

        private static ISimpleAudioVolume? GetVolumeObject(uint pID)
        {
            // Get the speakers (first render + multimedia device)
            var deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out IMMDevice speakers);

            // Activate the session manager
            var IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
            speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out object o);
            var manager = (IAudioSessionManager2)o;

            // Enumerate sessions on this device
            manager.GetSessionEnumerator(out IAudioSessionEnumerator sessionEnumerator);
            sessionEnumerator.GetCount(out int count);

            ISimpleAudioVolume? volumeControl = null;
            for (int i = 0; i < count; i++)
            {
                sessionEnumerator.GetSession(i, out IAudioSessionControl2 control);
                control.GetProcessId(out int cpID);

                if (cpID == pID)
                {
                    volumeControl = control as ISimpleAudioVolume;
                    break;
                }
                Marshal.ReleaseComObject(control);
            }

            Marshal.ReleaseComObject(sessionEnumerator);
            Marshal.ReleaseComObject(manager);
            Marshal.ReleaseComObject(speakers);
            Marshal.ReleaseComObject(deviceEnumerator);
            return volumeControl;
        }


        [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface ISimpleAudioVolume
        {
            [PreserveSig]
            int SetMasterVolume(float fLevel, ref Guid EventContext);

            [PreserveSig]
            int GetMasterVolume(out float pfLevel);
        }

        [ComImport]
        [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        internal class MMDeviceEnumerator
        { }

        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMMDeviceEnumerator
        {
            int NotImpl1();

            [PreserveSig]
            int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);
        }

        internal enum EDataFlow
        {
            eRender,
            eCapture,
            eAll,
            EDataFlow_enum_count
        }

        internal enum ERole
        {
            eConsole,
            eMultimedia,
            eCommunications,
            ERole_enum_count
        }

        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMMDevice
        {
            [PreserveSig]
            int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        }

        [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioSessionManager2
        {
            int NotImpl1();
            int NotImpl2();

            [PreserveSig]
            int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);
        }

        [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioSessionEnumerator
        {
            [PreserveSig]
            int GetCount(out int SessionCount);

            [PreserveSig]
            int GetSession(int SessionCount, out IAudioSessionControl2 Session);
        }

        [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioSessionControl2
        {
            int NotImpl1();

            int NotImpl2();

            int NotImpl3();

            int NotImpl4();

            int NotImpl5();

            int NotImpl6();

            int NotImpl7();

            int NotImpl8();

            int NotImpl9();

            int NotImpl10();

            int NotImpl11();

            [PreserveSig]
            int GetProcessId(out int pRetVal);
        }
    }
}