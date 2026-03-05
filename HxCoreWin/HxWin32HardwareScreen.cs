using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    /// <summary>
    /// Win32 API (GDI/User32/Shcore) P/Invoke 정의
    /// </summary>
    public class HxWin32HardwareScreen
    {
        // 1. EnumDisplaySettings (재생률, 방향)
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(
            [In] string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        // 2. MonitorFromPoint (HMONITOR 핸들)
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromPoint(
            System.Drawing.Point pt, uint dwFlags);

        // 3. GetDpiForMonitor (확대/축소 배율)
        [DllImport("Shcore.dll")]
        public static extern int GetDpiForMonitor(
            IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const uint MONITOR_DEFAULTTOPRIMARY = 1;

        public HxWin32HardwareScreen()
        {
        }

        public enum MonitorDpiType { MDT_EFFECTIVE_DPI = 0 }

        // DEVMODE 구조체 (필요한 필드만 정의)
        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation; // 0=가로, 1=세로(90), 2=가로(180), 3=세로(270)
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency; // 재생률 (Hz)
        }
    }
}
