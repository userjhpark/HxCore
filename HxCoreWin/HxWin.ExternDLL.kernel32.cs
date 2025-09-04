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
        #region SystemInfo
        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);
        #endregion

        #region DLL 동적 로딩
        //출처: http://rageworx.tistory.com/899 [자유로운 그날을 위해]
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        public extern static int LoadLibrary(string librayName);
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi)]
        public extern static IntPtr GetProcAddress(int hwnd, string procedureName);
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        public extern static bool FreeLibrary(int hModule);
        #endregion

        #region IsWow64Process
        /// <summary>
        /// OS 64Bit?
        /// </summary>
        /// <param name="hProcess">Process</param>
        /// <param name="lpSystemInfo">Out SystemInfo</param>
        /// <returns>Boolean</returns>
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([System.Runtime.InteropServices.In] IntPtr hProcess, [System.Runtime.InteropServices.Out] out bool lpSystemInfo);
        #endregion

        #region Profile String
        /// <summary>
        /// ini 파일에서 정보를 가져오기 위한 API 기초 함수
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(
                    string section,
                    string key,
                    string def,
                    StringBuilder retVal,
                    int size,
                    string filePath);
        /// <summary>
        /// ini 파일에서 정보를 쓰기위한 위한 API 기초 함수
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(
                    string section,
                    string key,
                    string val,
                    string filePath);
        #endregion

        #region Sort Path
        // 참조 : https://www.pinvoke.net/default.aspx/kernel32.GetShortPathName
        // 참조 : http://csharphelper.com/blog/2015/01/convert-between-long-and-short-file-names-in-c/
        // 참조 : https://enginhak.tistory.com/entry/Make-Short-Path%EA%B8%B4-%ED%8C%8C%EC%9D%BC%EB%AA%85-%EC%A7%A7%EA%B2%8C-%EC%A4%84%EC%9D%B4%EB%8A%94-%EB%B0%A9%EB%B2%95-%ED%8C%8C%EC%9D%BC%EC%9D%B4%EB%A6%84-%EA%B8%B8%EC%9D%B4-%EC%A4%84%EC%9D%B4%EA%B8%B0
        const int MAX_PATH = 255;

        // Define GetShortPathName API function.
        //[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern uint GetShortPathName(string lpszLongPath,char[] lpszShortPath, int cchBuffer);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath, uint cchBuffer);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName(string lpszLongPath, char[] lpszShortPath, int cchBuffer);

        
        #endregion
    }
}
