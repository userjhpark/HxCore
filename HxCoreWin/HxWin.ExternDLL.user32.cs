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
        public const int HWND_BROADCAST = 0xFFFF;
        public const int WM_COPYDATA = 0x004A;

        public struct HxCOPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string message);



        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        public const int SW_RESTORE = 9;
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        // 윈도우가 최소화 되어 있다면 활성화 시킨다
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        // 윈도우에 포커스를 줘서 최상위로 만든다
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern IntPtr GetDesktopWindow();

        public struct HxRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref HxRECT rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct HxWINDOWINFO
        {
            public uint cbSize;
            public HxRECT rcWindow;
            public HxRECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public HxWINDOWINFO(Boolean? filler)
                : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(HxWINDOWINFO)));
            }

        }
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref HxWINDOWINFO pwi);


        public const int HWND_NOTOPMOST = -2;
        public const int HWND_TOPMOST = -1;
        public const int HWND_TOP = 0;
        public const int HWND_BOTTOM = 1;

        public const int SWP_HIDEWINDOW = 128;
        public const int SWP_NOACTIVATE = 10;
        public const int SWP_NOMOVE = 2;
        public const int SWP_NOREDRAW = 8;
        public const int SWP_NOSIZE = 1;

        [DllImport("user32")]
        public static extern int SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetFocus();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hwnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);
        //https://pastebin.com/DgtJJGiv
        #region Post/Send WinAPI Message(Command)
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, ref HxCOPYDATASTRUCT lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, ref HxCOPYDATASTRUCT lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);


        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        //출처 : http://ehdrn.tistory.com/295 / http://blog.naver.com/jackylim/100111307833

        #region https://stackoverflow.com/questions/7740379/c-sharp-how-to-use-wm-gettext-getwindowtext-api-window-title
        //출처 : https://stackoverflow.com/questions/7740379/c-sharp-how-to-use-wm-gettext-getwindowtext-api-window-title
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        const int WM_GETTEXT = 0x000D;
        const int WM_GETTEXTLENGTH = 0x000E;
        public static string GetControlText(IntPtr hWnd)
        {
            
            // Get the size of the string required to hold the window title (including trailing null.) 
            Int32 titleSize = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0).ToInt32();

            // If titleSize is 0, there is no title so return an empty string (or null)
            if (titleSize == 0)
                return String.Empty;

            StringBuilder title = new StringBuilder(titleSize + 1);

            SendMessage(hWnd, (int)WM_GETTEXT, title.Capacity, title);

            return title.ToString();
        }
        public static string GetHandleText(IntPtr hWnd)
        {
            return GetControlText(hWnd);
        }

        /// <summary>
        /// The retrieved handle identifies the window of the same type that is highest in the Z order.
        /// <para/>
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        public const uint GW_HWNDFIRST = 0; //최상위 Window를 찾는다.
        /// <summary>
        /// The retrieved handle identifies the window of the same type that is lowest in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        public const uint GW_HWNDLAST = 1; //최하위 Window를 찾는다.
        /// <summary>
        /// The retrieved handle identifies the window below the specified window in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        public const uint GW_HWNDNEXT = 2; //하위 Window를 찾는다.
        /// <summary>
        /// The retrieved handle identifies the window above the specified window in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        public const uint GW_HWNDPREV = 3; //상위 Window를 찾는다.
        /// <summary>
        /// The retrieved handle identifies the specified window's owner window, if any.
        /// </summary>
        public const uint GW_OWNER = 4; //부모 Window를 찾는다.
        /// <summary>
        /// The retrieved handle identifies the child window at the top of the Z order,
        /// if the specified window is a parent window; otherwise, the retrieved handle is NULL.
        /// The function examines only child windows of the specified window. It does not examine descendant windows.
        /// </summary>
        public const uint GW_CHILD = 5; //자식 Window를 찾는다.
        /// <summary>
        /// The retrieved handle identifies the enabled popup window owned by the specified window (the
        /// search uses the first such window found using GW_HWNDNEXT); otherwise, if there are no enabled
        /// popup windows, the retrieved handle is that of the specified window.
        /// </summary>
        public const uint GW_ENABLEDPOPUP = 6;
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        #endregion


        public delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lp);
        [DllImport("user32.dll")]
        public static extern bool EnumThreadWindows(int tid, EnumThreadWndProc lpEnumFunc, IntPtr lp);

        public delegate bool EnumWindowsWndProc(IntPtr hWnd, IntPtr lp);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsWndProc lpEnumFunc, IntPtr lp);
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumChildWindows(parent, (hWnd, lp) => { result.Add(hWnd); return true; }, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();
        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder buffer, int buflen);
        //출처: https://ilbbang-programming.tistory.com/8 [일빵 프로그래밍 공부방]

        [DllImport("user32")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        //출처: https://slaner.tistory.com/55 [꿈꾸는 프로그래머]



        #endregion

        #region Clipboard WinAPI
        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();
        [DllImport("user32.dll")]
        public static extern IntPtr GetClipboardData(uint uFormat);
        [DllImport("user32.dll")]
        public static extern short IsClipboardFormatAvailable(uint uFormat);
        #endregion

        #region Mouse & Keyboard
        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 2,
            LEFTUP = 4,
            MIDDLEDOWN = 20,
            MIDDLEUP = 40,
            MOVE = 1,
            ABSOLUTE = 8000,
            RIGHTDOWN = 8,
            RIGHTUP = 10
        }
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        //private static LowLevelKeyboardProc _proc = HookCallback;
        //public static IntPtr _hookID = IntPtr.Zero;
        //??//public const int WM_LBUTTONDOWN = 0x201;
        //??//public const int WM_LBUTTONUP = 0x202;
        //??//public const int WM_LBUTTONDBLCLK = 0x203;
        //??//public const int WM_RBUTTONDOWN = 0x204;
        //??//public const int WM_RBUTTONUP = 0x205;
        //??//public const int WM_RBUTTONDBLCLK = 0x206;
        public const uint WM_MOUSEMOVE = 0x0001;      // 마우스 이동
        public const uint WM_ABSOLUTEMOVE = 0x8000;   // 전역 위치
        public const uint WM_LBUTTONDOWN = 0x0002;    // 왼쪽 마우스 버튼 눌림
        public const uint WM_LBUTTONUP = 0x0004;      // 왼쪽 마우스 버튼 떼어짐
        public const uint WM_RBUTTONDOWN = 0x0008;    // 오른쪽 마우스 버튼 눌림
        public const uint WM_RBUTTONUP = 0x00010;      // 오른쪽 마우스 버튼 떼어짐
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern byte VkKeyScan(char ch);
        [DllImport("user32.dll")]
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(int wCode, int wMapType);
        #endregion
    }
}
