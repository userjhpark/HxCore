using System;
using System.Runtime.InteropServices;

namespace HxCore.Win
{

    partial class HxWin
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        
    }

}