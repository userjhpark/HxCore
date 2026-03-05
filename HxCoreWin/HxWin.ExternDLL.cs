using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    partial class HxWin
    {
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WNetGetConnection(
                [MarshalAs(UnmanagedType.LPTStr)] string localName,
                [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName, ref int length);

        [DllImport("urlmon.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);
    }
}