using Microsoft.Win32;

using Newtonsoft.Json.Linq;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win
{
    using _MessageBox = System.Windows.Forms.MessageBox;
    public partial class HxWin : HxUtils
    {
#region Dll Imports
        /*
        public const int HWND_BROADCAST = 0xFFFF;
        public const int WM_COPYDATA = 0x004A;

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hwnd, uint msg, uint wparam, ref COPYDATASTRUCT lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, ref COPYDATASTRUCT lParam);
        */
#endregion Dll Imports
        public static string GetAppTitle()
        {
            AssemblyTitleAttribute attributes = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false);

            return attributes?.Title;
        }

        internal static string GetAppDirPath()
        {
            //string path = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            //return Path.Combine(path, assocExeFileName);
            return HxCore.HxUtils.AppBaseDir;
        }

        public static string GetProgramFilesx86()
        {
            if (HxUtils.IsApp64Bit || HxUtils.IsOS64Bit || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        #region SystemInfo
        //[DllImport("kernel32.dll")]
        //public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            internal PROCESSOR_INFO_UNION uProcessorInfo;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public int lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort dwProcessorLevel;
            public ushort dwProcessorRevision;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct PROCESSOR_INFO_UNION
        {
            [FieldOffset(0)]
            internal uint dwOemId;
            [FieldOffset(0)]
            internal ushort wProcessorArchitecture;
            [FieldOffset(2)]
            internal ushort wReserved;
        }
        #endregion

        #region SendMessage

        public static void SetSendMessageToForm(string applicationProcessName, string applicationWindowName, object[] args)
        {
            //MessageBox.Show(args.ToStringJoinEx(", "));
            if (!applicationProcessName.IsNullOrWhiteSpaceEx() && args != null && args.Length > 0)
            {
                Process findProcess = HxUtils.ProcessOneByName(applicationProcessName, true);
                if (findProcess != null)
                {
                    string strAppProductName = applicationWindowName.IsNullOrWhiteSpaceEx() != true ? applicationWindowName : findProcess.MainWindowTitle;//Application.ProductName;
                    IntPtr pHandle = findProcess.MainWindowHandle;
                    IntPtr mHandle = findProcess.MainWindowHandle; // findProcess.MainWindowHandle;
                    IntPtr wHandle = FindWindow(null, strAppProductName);
                    pHandle = wHandle != IntPtr.Zero ? wHandle : mHandle;

                    IntPtr wHandle2 = FindWindow(null, strAppProductName);
                    List<IntPtr> handleList = new List<IntPtr>();
                    foreach(ProcessThread thread in findProcess.Threads)
                    {
                        EnumThreadWindows(thread.Id, (hWnd, lp) => { handleList.Add(hWnd); return true; }, IntPtr.Zero);
                        
                    }
                    //List<IntPtr> hWndList = HxWin.GetChildWindows(pHandle);

                    /*
                    if (strAppProductName != findProcess.MainWindowTitle && wHandle.ToInt32() > 0)
                    {
                        pHandle = wHandle;
                    }
                    */

                    

                    foreach (IntPtr ptr in handleList)
                    {
                        if (ptr.ToInt32() > 0) //pHandle.ToInt32() > 0 && 
                        {
                            SetForegroundWindow(pHandle);
                            SetActiveWindow(pHandle);
                            if (args != null && args.Length > 0)
                            {
                                foreach (string arg in args)
                                {
                                    //string msg = arg.Trim().ToStringEx();
                                    string msg = arg;
                                    byte[] buff = System.Text.Encoding.Default.GetBytes(msg);
                                    HxCOPYDATASTRUCT cds = new HxCOPYDATASTRUCT { dwData = IntPtr.Zero, cbData = buff.Length + 1, lpData = msg };
                                    //cds.dwData = IntPtr.Zero; //(IntPtr)(1024 + 604);
                                    //cds.cbData = buff.Length + 1;//(int)msg.Length * sizeof(char);
                                    //cds.lpData = msg;
                                    SendMessage(ptr, WM_COPYDATA, IntPtr.Zero, ref cds);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void SetSendMessageToMainForm(string applicationProcessName, object[] args)
        {
            //MessageBox.Show(args.ToStringJoinEx(", "));
            if (!applicationProcessName.IsNullOrWhiteSpaceEx())
            {
                Process findProcess = HxUtils.ProcessOneByName(applicationProcessName, true);
                if (findProcess != null)
                {
                    //MessageBox.Show(args?.ToStringJoinEx(", "));

                    string strAppProductName = findProcess.MainWindowTitle;
                    IntPtr pHandle = findProcess.MainWindowHandle;
                    IntPtr wHandle = FindWindow(null, strAppProductName);
                    if (pHandle.ToInt32() > 0 && wHandle.ToInt32() > 0) //
                    {
                        if (args != null && args.Length > 0) {
                            foreach (string arg in args)
                            {
                                //string msg = arg.Trim().ToStringEx();
                                string msg = arg;
                                byte[] buff = System.Text.Encoding.Default.GetBytes(msg);
                                HxCOPYDATASTRUCT cds = new HxCOPYDATASTRUCT { dwData = IntPtr.Zero, cbData = buff.Length + 1, lpData = msg };
                                //cds.dwData = IntPtr.Zero; //(IntPtr)(1024 + 604);
                                //cds.cbData = buff.Length + 1;//(int)msg.Length * sizeof(char);
                                //cds.lpData = msg;
                                SendMessage(pHandle, WM_COPYDATA, IntPtr.Zero, ref cds);
                            }
                        }
                        else
                        {
                            ShowWindow(wHandle, SW_SHOWMAXIMIZED);
                        }
                    }
                }
            }
        }
#endregion

        #region DLL 동적 로딩
        //출처: http://rageworx.tistory.com/899 [자유로운 그날을 위해]

        #endregion

        #region MessageBox
        /// <summary>
        /// Get MessageBoxIcon To Caption
        /// </summary>
        /// <param name="icon">메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나</param>
        /// <returns>메시지 상자의 제목 표시줄에 표시할 텍스트</returns>
        public static string GetMessageBoxCaption(MessageBoxIcon icon)
        {
            string Result;
            switch (icon)
            {
                case MessageBoxIcon.Error: // = Hand, Stop
                    Result = "Error(오류)!!";
                    break;
                case MessageBoxIcon.Question:
                    Result = "Question(확인)?";
                    break;
                case MessageBoxIcon.Information: // = Asterisk
                    Result = "Information(정보)!";
                    break;
                case MessageBoxIcon.Warning: // = Exclamation
                    Result = "Warning(경고)*";
                    break;
                case MessageBoxIcon.None:
                default:
                    Result = "Message";
                    break;
            }
            return Result;
        }
        public static DialogResult ShowMessageBox(string text, MessageBoxIcon icon, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            string caption = GetMessageBoxCaption(icon);
            return _MessageBox.Show( text, caption, buttons, icon);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, MessageBoxIcon icon, MessageBoxButtons buttons)
        {
            string caption = GetMessageBoxCaption(icon);
            return _MessageBox.Show(owner, text, caption, buttons, icon);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, MessageBoxIcon icon, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            string caption = GetMessageBoxCaption(icon);
            return _MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
        }

        /// <summary>
        /// 지정된 텍스트가 있는 메시지 상자를 표시
        /// </summary>
        /// <param name="text">메시지 상자에 표시할 텍스트</param>
        /// <returns>System.Windows.Forms.DialogResult 값</returns>
        public static DialogResult ShowMessageBox(string text)
        {
            return _MessageBox.Show(text);
        }
        

        public static DialogResult ShowMessageBox(string text, string caption)
        {
            return _MessageBox.Show(text, caption);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons)
        {
            return _MessageBox.Show(text, caption, buttons);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return _MessageBox.Show(text, caption, buttons, icon);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return _MessageBox.Show(text, caption, buttons, icon, defaultButton);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
        {
            return _MessageBox.Show(text, caption, buttons, icon, defaultButton, options, displayHelpButton);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return _MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            return _MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return _MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return _MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword);
        }
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return _MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        /// 지정된 텍스트가 있는 메시지 상자를 표시
        /// </summary>
        /// <param name="owner">모달 대화 상자를 소유할 객체</param>
        /// <param name="text">메시지 상자에 표시할 텍스트</param>
        /// <returns>System.Windows.Forms.DialogResult 값</returns>
        public static DialogResult ShowMessageBox(IWin32Window owner, string text)
        {
            return _MessageBox.Show(owner, text);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption)
        {
            return _MessageBox.Show(owner, text, caption);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return _MessageBox.Show(owner, text, caption, buttons);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxIcon icon)
        {
            return _MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, icon);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return _MessageBox.Show(owner, text, caption, buttons, icon);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return _MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return _MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            return _MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return _MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return _MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath);
        }
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return _MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword);
        }

        /// <summary>
        /// 지정된 텍스트가 있는 메시지 상자를 표시
        /// </summary>
        /// <param name="ex">Exception Resource</param>
        /// <param name="icon">Box Icon</param>
        /// <param name="buttons">Buttons</param>
        /// <returns>Dialog Result</returns>
        public static DialogResult ShowMessageBox(Exception ex, MessageBoxIcon icon = MessageBoxIcon.Error, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            if (ex != null)
            {
                //MessageBoxIcon icon = MessageBoxIcon.Error;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                return ShowMessageBox(ex.Message, icon, buttons);
            }
            return DialogResult.None;
        }

        /// <summary>
        /// 지정된 텍스트가 있는 메시지 상자를 표시
        /// </summary>
        /// <param name="owner">소유 Resource</param>
        /// <param name="ex">Exception Resource</param>
        /// <param name="icon">Box Icon</param>
        /// <param name="buttons">Buttons</param>
        /// <returns>Dialog Result</returns>
        public static DialogResult ShowMessageBox(IWin32Window owner, Exception ex, MessageBoxIcon icon = MessageBoxIcon.Error, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            if (ex != null)
            {
                //MessageBoxIcon icon = MessageBoxIcon.Error;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                return ShowMessageBox(owner, ex.Message, icon, buttons);
            }
            return DialogResult.None;
        }
        /*
        /// <summary>
        /// 지정된 텍스트와 아이콘이 있는 메시지 상자를 표시
        /// </summary>
        /// <param name="owner">모달 대화 상자 소유 객체</param>
        /// <param name="text">메시지 상자에 표시할 텍스트</param>
        /// <param name="icon">메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나</param>
        /// <returns></returns>
        public static DialogResult ShowMessageBox(IWin32Window owner, string text, MessageBoxIcon icon)
        {
            string caption;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            switch (icon)
            {
                case System.Windows.Forms.MessageBoxIcon.Error: // = Hand, Stop
                    caption = "오류";
                    break;
                case System.Windows.Forms.MessageBoxIcon.Question:
                    caption = "확인?";
                    buttons = MessageBoxButtons.YesNo;
                    break;
                case System.Windows.Forms.MessageBoxIcon.Information: // = Asterisk
                    caption = "정보";
                    break;
                case System.Windows.Forms.MessageBoxIcon.Warning: // = Exclamation
                    caption = "경고";
                    break;
                default:
                    caption = "알림";
                    break;
            }
            if(owner != null)
            {
                return MessageBox.Show(owner, text, caption, buttons, icon);
            } else
            {
                return MessageBox.Show(text, caption, buttons, icon);
            }
            
        }
       

        /// <summary>
        /// 지정된 텍스트, 캡션, 단추, 아이콘 및 기본 단추가 있는 메시지 상자를 지정된 개체 앞에 표시합니다.
        /// </summary>
        /// <param name="text">메시지 상자에 표시할 텍스트입니다.</param>
        /// <param name="icon"메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.></param>
        /// <param name="buttons">메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.</param>
        /// <param name="defaultButton">메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.</param>
        /// <returns>System.Windows.Forms.DialogResult 값 중 하나입니다.</returns>
        public static DialogResult ShowMessageBox(string text, MessageBoxIcon icon, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            string caption = GetMessageBoxCaption(icon);
            return MessageBox.Show(text, caption, buttons, icon, defaultButton);

        }

        /// <summary>
        /// 지정된 텍스트, 캡션, 단추, 아이콘 및 기본 단추가 있는 메시지 상자를 지정된 개체 앞에 표시합니다.
        /// </summary>
        /// <param name="text">메시지 상자에 표시할 텍스트입니다.</param>
        /// <param name="buttons">메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.</param>
        /// <returns>System.Windows.Forms.DialogResult 값 중 하나입니다.</returns>
        public static DialogResult ShowMessageBox(string text, MessageBoxButtons buttons)
        {
            MessageBoxIcon icon;
            string caption;
            MessageBoxDefaultButton defaultButton;
            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:
                case MessageBoxButtons.RetryCancel:
                case MessageBoxButtons.AbortRetryIgnore:
                case MessageBoxButtons.YesNo:
                case MessageBoxButtons.YesNoCancel:
                    icon = MessageBoxIcon.Question;
                    defaultButton = MessageBoxDefaultButton.Button2;
                    break;
                case MessageBoxButtons.OK:
                default:
                    icon = MessageBoxIcon.None;
                    defaultButton = MessageBoxDefaultButton.Button1;
                    break;
            }
            caption = GetMessageBoxCaption(icon);
            return MessageBox.Show(text, caption, buttons, icon, defaultButton);
        }

        /// <summary>
        /// 지정된 텍스트가 있는 메시지 상자를 표시
        /// </summary>
        /// <param name="text">메시지 상자에 표시할 텍스트</param>
        /// <param name="caption">메시지 상자의 제목 표시줄에 표시할 텍스트</param>
        /// <returns>System.Windows.Forms.DialogResult 값</returns>
        public static DialogResult ShowMessageBox(string text, string caption)
        {
            return MessageBox.Show(text, caption);
        }
        /// <summary>
        /// 지정된 텍스트, 캡션 및 단추가 있는 메시지 상자를 지정된 개체 앞에 표시
        /// </summary>
        /// <param name="text">지정된 텍스트, 캡션 및 단추가 있는 메시지 상자를 지정된 개체 앞에 표시합니다.</param>
        /// <param name="caption">메시지 상자의 제목 표시줄에 표시할 텍스트</param>
        /// <param name="buttons">메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나</param>
        /// <returns>System.Windows.Forms.DialogResult 값 중 하나</returns>
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(text, caption, buttons);
        }
        /// <summary>
        /// 지정된 텍스트, 캡션, 단추 및 아이콘이 있는 메시지 상자를 표시
        /// </summary>
        /// <param name="text">메시지 상자에 표시할 텍스트</param>
        /// <param name="caption">메시지 상자의 제목 표시줄에 표시할 텍스트</param>
        /// <param name="buttons">메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나</param>
        /// <param name="icon">메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나</param>
        /// <param name="defaultButton">메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.</param>
        /// <returns>System.Windows.Forms.DialogResult 값 중 하나</returns>
        public static DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            return MessageBox.Show(text, caption, buttons, icon, defaultButton);
        }
        */
        #endregion

        #region Network / PC Domain
        /*
        public static string GetUserDomainName()
        {
            string Result = string.Empty;
            if (Result.IsNullOrWhiteSpaceEx())
            {
                Result = Environment.UserDomainName;
            }
            return Result;
        }

        public static string GetUserWorkgroup()
        {
            string Result = string.Empty;
            try
            {
                SelectQuery query = new SelectQuery("Win32_ComputerSystem");
                if (query != null)
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        if ((bool)mo["partofdomain"] != true)
                        {
                            //Console.WriteLine("Workgroup {0} ", mo["workgroup"]);
                            Result = mo["workgroup"].ToStringEx();
                        }
                        else
                        {
                            //Console.WriteLine("Domain {0} ", mo["workgroup"]);
                            Result = mo["workgroup"].ToStringEx();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex);
                //throw;
            }
            return Result;
        }

        //public static string GetUserGlobalAddress()
        //{
        //    return HxNet.GetUserGlobalAddress();
        //}
        //public static string GetUserIPAddress()
        //{
        //    return HxNet.GetUserIPAddress();
        //}
        //public static string GetUserHostName()
        //{
        //    return HxNet.GetUserHostName();
        //}
        public static string GetUserMacAddress(string ip = null)
        {
            string Result = string.Empty;
            try
            {
                if (!ip.IsNullOrWhiteSpaceEx())
                {
                    ip = GetUserHostAddress();
                }
                if (!ip.IsNullOrWhiteSpaceEx())
                {
                    List<string> list = HxNet.GetUserAdressList();
                    if(list != null && list.Count > 0)
                    {
                        ip = list[0];
                    }
                }
                if (!ip.IsNullOrWhiteSpaceEx())
                {
                    ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
                    ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
                    foreach (ManagementObject mo in query1.Get())
                    {
                        string[] address = (string[])mo["IPAddress"];
                        if (address[0] == ip && mo["MACAddress"] != null)
                        {
                            Result = mo["MACAddress"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result = null;
                //throw ex;
            }
            
            return Result;
        }
        public static string GetUserVolumeId(string drive = null)
        {
            if (drive.IsNullOrWhiteSpaceEx())
            {
                drive = Environment.SystemDirectory;
            }

            if (drive.Contains(":\\"))
            {
                //C:\ -> C
                int index = drive.IndexOf(":\\");
                drive = drive.Substring(0, index);
            }

            

            string Result = string.Empty;
            try
            {
                Result = GetUserVolumeSerial(drive);
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    Result = GetUserVolumeSerial("C");
                }
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM win32_LogicalDisk");
                    ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
                    foreach (ManagementObject mo in query1.Get())
                    {
                        int driveType = mo.GetPropertyValue("DriveType").ToIntEx();
                        string driveName = mo.GetPropertyValue("Name").ToStringEx();
                        Result = mo.GetPropertyValue("VolumeName").ToStringEx();
                        if (!Result.IsNullOrWhiteSpaceEx())
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }

        public static string GetUserVolumeSerial(string drive)
        {
            string Result = string.Empty;
            if (drive.Contains(":\\"))
            {
                //C:\ -> C
                int index = drive.IndexOf(":\\");
                drive = drive.Substring(0, index);
            }
            if (!drive.IsNullOrWhiteSpaceEx())
            {
                using (ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":"""))
                {
                    disk.Get();
                    if (disk != null)
                    {
                        Result = disk["VolumeSerialNumber"].ToString();
                    }
                }
            }
            return Result;
        }

        public static List<string> GetUserVolumeIdList()
        {
            List<string> Result = null;
            ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM win32_LogicalDisk");
            if (oq != null)
            {
                Result = new List<string>();
                ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
                foreach (ManagementObject mo in query1.Get())
                {
                    _ = mo.GetPropertyValue("DriveType").ToIntEx();
                    _ = mo.GetPropertyValue("Name").ToStringEx();
                    string volumeSerial = mo.GetPropertyValue("VolumeName").ToStringEx();
                    Result.AddEx(volumeSerial);
                }
            }
            return Result;
        }
        */
        /*
        public static string GetUserCPUId()
        {
            string Result = string.Empty;
            try
            {
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                foreach (ManagementObject managObj in managCollec)
                {
                    //Get only the first CPU's ID
                    Result = managObj.Properties["processorID"].Value.ToStringEx();
                    if (!Result.IsNullOrWhiteSpaceEx())
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = null;
                //throw ex;
            }

            return Result;
        }
        */
        /*
        public static string GetUserUniqueID(string drive = null)
        {
            string Result = null;
            if (drive.IsNullOrWhiteSpaceEx())
            {
                //Find first drive
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.Contains(":\\"))
            {
                //C:\ -> C
                int index = drive.IndexOf(":\\");
                drive = drive.Substring(0, index);
            }

            string volumeSerial = GetUserVolumeSerial(drive);
            string cpuID = GetUserCPUId();

            //Mix them up and remove some useless 0's
            if (!cpuID.IsNullOrWhiteSpaceEx() && !volumeSerial.IsNullOrWhiteSpaceEx())
            {
                Result =  cpuID.Substring(13) + cpuID.Substring(1, 4) + volumeSerial + cpuID.Substring(4, 4);
            }
            else if(!cpuID.IsNullOrWhiteSpaceEx() && volumeSerial.IsNullOrWhiteSpaceEx())
            {
                Result = cpuID.Substring(13) + cpuID.Substring(1, 4) + "HTHTHTHT" + cpuID.Substring(4, 4);
            }
            else if (cpuID.IsNullOrWhiteSpaceEx() && !volumeSerial.IsNullOrWhiteSpaceEx())
            {
                Result = "HTE" + "HTHT" + volumeSerial + "HTHT";
            }
            else
            {
                Result = null;
            }
            return Result;
        }
        */
#endregion

        public static void ExecuteProcessRun(string fileName, string arguments = null, bool bVerbRunas = false, bool bUseShellExecute = true, bool bCreateNoWindow = true, bool bRedirectStandardOutput = false, bool bWaitForExit = false)
        {
            //상세 참조 : https://skql.tistory.com/510
            //System.Diagnostics.Process.Start(fileName);
            System.Diagnostics.Process procExec = new System.Diagnostics.Process();
            try
            {
                //string strFileName = HxFile.GetLongFileName(fileName);
                procExec.StartInfo.FileName = fileName;
                //reg.StartInfo.Arguments = @"/quiet /passive";
                if (arguments.IsNullOrWhiteSpaceEx() != true)
                {
                    procExec.StartInfo.Arguments = arguments;
                }
                procExec.StartInfo.UseShellExecute = bUseShellExecute;
                procExec.StartInfo.CreateNoWindow = bCreateNoWindow;
                procExec.StartInfo.RedirectStandardOutput = bRedirectStandardOutput;
                //if (Verb.IsNullOrWhiteSpaceEx() != true)
                //{
                //    procExec.StartInfo.Verb = Verb;//"runas";
                //}
                if(bVerbRunas == true)
                {
                    procExec.StartInfo.Verb = "runas";
                }
                procExec.Start();
                if (bWaitForExit == true)
                {
                    procExec.WaitForExit();
                }
                procExec.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
            finally
            {
                procExec.Dispose();
            }
            
        }
        internal static void ExecuteAcrobatReaderRun(string fileName, string arguments = null, bool bVerbRunas = false, bool bUseShellExecute = true, bool bCreateNoWindow = true, bool bRedirectStandardOutput = false, bool bWaitForExit = false)
        {
            //상세 참조 : https://skql.tistory.com/510
            //System.Diagnostics.Process.Start(fileName);
            System.Diagnostics.Process procExec = new System.Diagnostics.Process();
            try
            {
                string readerExecPath = @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";
                if (File.Exists(readerExecPath))
                {
                    procExec.StartInfo.FileName = readerExecPath;
                    //reg.StartInfo.Arguments = @"/quiet /passive";
                    if (fileName.IsNullOrWhiteSpaceEx() != true || arguments.IsNullOrWhiteSpaceEx() != true)
                    {
                        procExec.StartInfo.Arguments = ( "\"" + fileName.Trim() + "\" " + arguments.Trim()).Trim();
                    }
                    procExec.StartInfo.UseShellExecute = bUseShellExecute;
                    procExec.StartInfo.CreateNoWindow = bCreateNoWindow;
                    procExec.StartInfo.RedirectStandardOutput = bRedirectStandardOutput;
                    //if (Verb.IsNullOrWhiteSpaceEx() != true)
                    //{
                    //    procExec.StartInfo.Verb = Verb;//"runas";
                    //}
                    if (bVerbRunas == true)
                    {
                        procExec.StartInfo.Verb = "runas";
                    }
                    procExec.Start();
                    if (bWaitForExit == true)
                    {
                        procExec.WaitForExit();
                    }
                    procExec.Close();
                }
                else
                {
                    ExecuteProcessRun(fileName, arguments, bVerbRunas, bUseShellExecute, bCreateNoWindow, bRedirectStandardOutput, bWaitForExit);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                try
                {
                    ExecuteProcessRun(fileName, arguments, bVerbRunas, bUseShellExecute, bCreateNoWindow, bRedirectStandardOutput, bWaitForExit);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw ex;
                }
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                procExec.Dispose();
            }

        }
        public static void ExecuteProcessRunas(string fileName, string workingDirectory = null)
        {
            try
            {
                if (fileName.IsNullOrWhiteSpaceEx() == true) 
                {
                    fileName = Application.ExecutablePath;
                }
                if(workingDirectory.IsNullOrWhiteSpaceEx() == true)
                {
                    if (HxFile.IsFileExists(fileName))
                    {
                        string fullName = HxFile.GetFileFullPath(fileName);
                        workingDirectory = HxFile.GetFileDirPath(fullName);
                    }
                    if (workingDirectory.IsNullOrWhiteSpaceEx() == true)
                    {
                        workingDirectory = Environment.CurrentDirectory;
                    }
                }

                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = fileName,
                    //WorkingDirectory = Environment.CurrentDirectory,
                    Verb = "runas"
                };
                if(workingDirectory.IsNullOrWhiteSpaceEx() != true)
                {
                    procInfo.WorkingDirectory = workingDirectory;
                }
                System.Diagnostics.Process.Start(procInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            
        }
        /// <summary>
        /// 현재 응용프로그램 관리자 권한 확인
        /// </summary>
        /// <returns>관리자 권한 여부</returns>
        public static bool GetIsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (identity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return false;
        }
        

        private static List<Control> GetControls(System.Windows.Forms.Control.ControlCollection Controls)
        {
            List<Control> Result = null;
            try
            {
                var controls = Controls.OfType<Control>().Select(c => c).Distinct();
                if(controls != null)
                {
                    Result = new List<Control>();
                    foreach (var cmp in controls)
                    {
                        if(cmp is Control ctrl)
                        {
                            Result.Add(cmp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }

        //https://stackoverflow.com/questions/3419159/how-to-get-all-child-controls-of-a-windows-forms-form-of-a-specific-type-button
        /*
        private static IEnumerable<Control> GetFindControls(Control control)
        {
            
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetFindControls(ctrl))
                                      .Concat(controls);
        }
        private static IEnumerable<Control> GetFindControls(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetFindControls(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private static IEnumerable<Control> GetAllControls(System.Windows.Forms.Control.ControlCollection container)
        {
            IEnumerable<Control> controls = container.OfType<Control>().Cast<Control>().ToList();
            return controls;
        }

        private static IEnumerable<T> FindControls<T>(Control control)
            where T : Control
        {
            // we can't cast here because some controls in here will most likely not be <T>
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => FindControls<T>(ctrl))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == typeof(T)).Cast<T>();
        }
        */

        private static IEnumerable<T> FindAllChildrenByType<T>(Control control)
            where T : Control
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            return controls
                .OfType<T>()
                .Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)));
        }

        public static IEnumerable<T> GetFindAllControl<T>(Control control)
            where T : Control
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            return controls
                .OfType<T>()
                .Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)));
        }

        public static T GetFindControlByName<T>(Control control, string name)
            where T : Control
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            return controls
                ?.OfType<T>()
                ?.Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)))
                ?.Where(r => r.Name.Equals(name))
                ?.Last()
                ;
        }


        protected static List<Control> GetFindControls(System.Windows.Forms.Control.ControlCollection Controls)
        {
            return GetControls(Controls);
        }
        protected static IEnumerable<T> GetFindAllControls<T>(Control control)
            where T : Control
        {
            return control.GetFindAllControlEx<T>();
            //return HxWin.GetFindAllControls<T>(control);
        }

        public static IEnumerable<T> GetFindAllFormByType<T>(System.Windows.Forms.FormCollection forms = null)
            where T : System.Windows.Forms.Form
        {
            IEnumerable<T> Result = null;
            if (forms == null)
            {
                forms = Application.OpenForms;
                if (forms != null && forms.Count > 0)
                {
                    Result = Application.OpenForms.OfType<T>();
                }
            }
            return Result;
        }
        public static T GetFindSingleFormByType<T>(System.Windows.Forms.FormCollection forms = null, HxMultiplePosition position = HxMultiplePosition.Last)
            where T : System.Windows.Forms.Form
        {
            T Result = null;
            IEnumerable<T> findAll = GetFindAllFormByType<T>(forms);
            if(findAll != null && findAll.Count() > 0)
            {
                switch (position)
                {
                    case HxMultiplePosition.First:
                        Result = findAll.First();
                        break;
                    case HxMultiplePosition.Last:
                        Result = findAll.Last();
                        break;
                    case HxMultiplePosition.All:
                    default:
                        Result = findAll.Single();
                        break;
                }
            }
            return Result;
        }
        public static T FindSingleFormByType<T>(System.Windows.Forms.FormCollection forms = null)
            where T : System.Windows.Forms.Form
        {
            return GetFindSingleFormByType<T>(forms, HxMultiplePosition.Last);
        }
        public static IEnumerable<T> GetFindTypeFormByTag<T>(object tagValue, System.Windows.Forms.FormCollection forms)
            where T : System.Windows.Forms.Form
        {
            IEnumerable<T> Result = null;
            if (forms == null)
            {
                forms = Application.OpenForms;
            }
            if (forms != null && forms.Count > 0)
            {
                if (tagValue.IsNullOrWhiteSpaceEx() != true)
                    Result = Application.OpenForms.OfType<T>().Where(r => r.Tag.ToStringEx().Equals(tagValue.ToStringEx()));
                else
                    Result = GetFindAllFormByType<T>(forms);
            }
            return Result;
        }


        /// <summary>
        /// Control 객체 목록에서 TAG 값의 패턴을 찾아서 DataSource와 바인딩
        /// </summary>
        /// <param name="controls">Control 객체 목록</param>
        /// <param name="dataSource">Data Source : DataTable, DataView</param>
        /// <param name="regexTagPattern">정규식 TAG 값 패턴</param>
        public static void SetDataBindingsByControlTag(IEnumerable<Control> controls,  object dataSource = null, string regexTagPattern = HxTagTpl._DEF_TAG_VAR_PATTERN_, bool isPrevTextClear = true)
        {
            bool bPrevTextClear = isPrevTextClear;
            if (controls != null && controls.Count() > 0)
            {
                try
                {
                    //DataView dv = row.Table.DefaultView;
                    //dv.RowFilter = "recordid=" + row["recordid"].ToString();
                    DataView dvSource = null;

                    if (dataSource is DataView)
                    {
                        dvSource = dataSource as DataView;
                    }
                    else if(dataSource is DataTable)
                    {
                        dvSource = (dataSource as DataTable)?.AsDataView();
                    }
                    else if(dataSource is DataRow)
                    {
                        DataRow row = dataSource as DataRow;
                        DataTable dt = row?.Table;
                        int rowIndex = dt.Rows.IndexOf(row);
                        string rowIndexColumnName = string.Format("RowIndex_{0}", "CustomMethod_SetDataBindingsByControlTag");
                        if (dt.Columns.Contains(rowIndexColumnName) != true)
                        {
                            dt.Columns.Add(new DataColumn { ColumnName = rowIndexColumnName, DataType = typeof(int) });
                            int iRow = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                dr[rowIndexColumnName] = iRow;
                                iRow++;
                            }
                        }

                        int iRowIndex = row[rowIndexColumnName].ToIntEx();
                        if (iRowIndex < 0)
                            iRowIndex = rowIndex;

                        dvSource = dt?.DefaultView;
                        dvSource.RowFilter = string.Format("{0} = {1}", rowIndexColumnName, iRowIndex);
                        //int index = 
                        //dvSource.RowFilter = "recordid=" + row["recordid"].ToString();
                    }
                    //if (dvSource != null)
                    {
                        foreach (Control ctrl in controls)
                        {
                            if (dvSource != null)
                            {
                                string strControlTagText = ctrl.Tag.ToStringEx();
                                if (strControlTagText.IsNullOrWhiteSpaceEx() != true)
                                {
                                    string strReplaceText = null;
                                    string strColName = null;
                                    if (regexTagPattern.IsNullOrWhiteSpaceEx())
                                    {
                                        regexTagPattern = HxTagTpl._DEF_TAG_VAR_PATTERN_;
                                    }
                                    MatchCollection matches = Regex.Matches(strControlTagText, regexTagPattern);
                                    int n = matches.Count;
                                    if (n > 0)
                                    {
                                        if (bPrevTextClear == true)
                                            ctrl.Text = null;

                                        for (int i = 0; i < n; i++)
                                        {
                                            Match match = matches[i];
                                            if (match.Success)
                                            {
                                                string strValue = match.Value;
                                                string strVarCase = match.Groups[1].Value;
                                                string strVarName = match.Groups[2].Value;
                                                string strVarOption = match.Groups[3].Value;
                                                string strEndDefine = match.Groups[4].Value;
                                                strReplaceText = strControlTagText.Replace(strValue, string.Empty);
                                                if (strColName.IsNullOrWhiteSpaceEx())
                                                    strColName = strVarName;
                                            }
                                        }
                                    }
                                    if (strColName.IsNullOrWhiteSpaceEx() != true && dvSource != null)
                                    {
                                        string strCol = strColName.Trim().ToLower();
                                        if (dvSource.Table.Columns.Contains(strCol))
                                        {
                                            ctrl.DataBindings.Clear();
                                            try
                                            {
                                                try
                                                {
                                                    ctrl.DataBindings.Add("EditValue", dvSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                                }
                                                catch (Exception)
                                                {
                                                    ctrl.DataBindings.Add("Text", dvSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                                    //throw;
                                                }
                                                //ctrl.DataBindings.Add("Text", dataSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                                //ctrl.DataBindings.Add("SetValue", dataSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                            }
                                            catch (Exception exBindingsAddText)
                                            {
                                                Debug.WriteLine(exBindingsAddText);
                                                throw exBindingsAddText;
                                            }
                                            //if (ctrl is CheckedComboBoxEdit boxEdit)
                                            //{
                                            //    //boxEdit.SetEditValue(null);
                                            //    boxEdit.DataBindings.Add("SetValue", dataSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw e;
                }
                
            }
        }

        /// <summary>
        /// Control 객체 목록에서 TAG 값의 패턴을 찾아서 DataSource와 바인딩
        /// </summary>
        /// <typeparam name="T">찾을 타입</typeparam>
        /// <param name="owner">ROOT Colntrol(소유자) 객체</param>
        /// <param name="dataSource">Data Source : DataTable, DataView</param>
        /// <param name="regexTagPattern">정규식 TAG 값 패턴</param>
        public static void SetDataBindingsByControlTag<T>(Control owner, object dataSource, string regexTagPattern = HxTagTpl._DEF_TAG_VAR_PATTERN_, bool isPrevTextClear = true)
            where T : Control
        {
            IEnumerable<T> controls = owner.GetFindAllControlEx<T>();
            if (controls != null && controls.Count() > 0)
            {
                SetDataBindingsByControlTag(controls, dataSource, regexTagPattern, isPrevTextClear);
            }
        }
        /// <summary>
        /// Control 객체 목록에서 TAG 값의 패턴을 찾아서 SetValue
        /// </summary>
        /// <param name="controls">Control 객체 목록</param>
        /// <param name="dataSource">Data Source : DataTable, DataView</param>
        /// <param name="regexTagPattern">정규식 TAG 값 패턴</param>
        public static void SetTextByControlTag(IEnumerable<Control> controls, DataRow row, string regexTagPattern = HxTagTpl._DEF_TAG_VAR_PATTERN_, bool isPrevTextClear = true)
        {
            bool bPrevTextClear = isPrevTextClear;
            if (controls != null && controls.Count() > 0)
            {
                try
                {
                    foreach (Control ctrl in controls)
                    {
                        string strControlTagText = ctrl.Tag.ToStringEx();
                        if (strControlTagText.IsNullOrWhiteSpaceEx() != true)
                        {
                            string strReplaceText = null;
                            string strColName = null;
                            if (regexTagPattern.IsNullOrWhiteSpaceEx())
                            {
                                regexTagPattern = HxTagTpl._DEF_TAG_VAR_PATTERN_;
                            }
                            MatchCollection matches = Regex.Matches(strControlTagText, regexTagPattern);
                            int n = matches.Count;
                            if (n > 0)
                            {
                                if (bPrevTextClear != false)
                                    ctrl.Text = null;

                                for (int i = 0; i < n; i++)
                                {
                                    Match match = matches[i];
                                    if (match.Success)
                                    {
                                        string strValue = match.Value;
                                        string strVarCase = match.Groups[1].Value;
                                        string strVarName = match.Groups[2].Value;
                                        string strVarOption = match.Groups[3].Value;
                                        string strEndDefine = match.Groups[4].Value;
                                        strReplaceText = strControlTagText.Replace(strValue, string.Empty);
                                        if (strColName.IsNullOrWhiteSpaceEx())
                                            strColName = strVarName;
                                    }
                                }
                            }
                            if (strColName.IsNullOrWhiteSpaceEx() != true)
                            {
                                string strCol = strColName.Trim().ToLower();
                                if (row.Table.Columns.Contains(strCol))
                                {
                                    try
                                    {
                                        ctrl.Text = row[strCol].ToStringEx();
                                    }
                                    catch (Exception exSetText)
                                    {
                                        Debug.WriteLine(exSetText);
                                        throw exSetText;
                                    }
                                    //if (ctrl is CheckedComboBoxEdit boxEdit)
                                    //{
                                    //    //boxEdit.SetEditValue(null);
                                    //    boxEdit.DataBindings.Add("SetValue", dataSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                    //}
                                }
                                else
                                {
                                    ctrl.Text = null;
                                }
                            }
                        }
                        
                    }
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw e;
                }

            }
        }
        public static void SetEditValueByControlTag<T>(IEnumerable<T> controls, DataRow row, string regexTagPattern = HxTagTpl._DEF_TAG_VAR_PATTERN_, bool isPrevTextClear = true)
            where T : Control
        {
            bool bPrevTextClear = isPrevTextClear;
            if (controls != null && controls.Count() > 0)
            {
                try
                {
                    foreach (T ctrl in controls)
                    {
                        string strControlTagText = ctrl.Tag.ToStringEx();
                        if (strControlTagText.IsNullOrWhiteSpaceEx() != true)
                        {
                            string strReplaceText = null;
                            string strColName = null;
                            if (regexTagPattern.IsNullOrWhiteSpaceEx())
                            {
                                regexTagPattern = HxTagTpl._DEF_TAG_VAR_PATTERN_;
                            }
                            MatchCollection matches = Regex.Matches(strControlTagText, regexTagPattern);
                            int n = matches.Count;
                            if (n > 0)
                            {
                                if (bPrevTextClear != false)
                                    ctrl.Text = null;

                                for (int i = 0; i < n; i++)
                                {
                                    Match match = matches[i];
                                    if (match.Success)
                                    {
                                        string strValue = match.Value;
                                        string strVarCase = match.Groups[1].Value;
                                        string strVarName = match.Groups[2].Value;
                                        string strVarOption = match.Groups[3].Value;
                                        string strEndDefine = match.Groups[4].Value;
                                        strReplaceText = strControlTagText.Replace(strValue, string.Empty);
                                        if (strColName.IsNullOrWhiteSpaceEx())
                                            strColName = strVarName;
                                    }
                                }
                            }
                            if (strColName.IsNullOrWhiteSpaceEx() != true)
                            {
                                string strCol = strColName.Trim().ToLower();
                                if (row.Table.Columns.Contains(strCol))
                                {
                                    string strValue = row[strCol].ToStringEx();
                                    try
                                    {
                                        //ctrl.DataBindings.Add("EditValue", dvSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                        bool bSucess = HxUtils.SetMemberPropertyValue(ctrl, "EditValue", strValue, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, true);
                                        if(bSucess != true)
                                        {
                                            ctrl.Text = strValue;
                                        }
                                    }
                                    catch (Exception exSetText)
                                    {
                                        ctrl.Text = strValue;
                                        Debug.WriteLine(exSetText);
                                        throw exSetText;
                                    }
                                    //if (ctrl is CheckedComboBoxEdit boxEdit)
                                    //{
                                    //    //boxEdit.SetEditValue(null);
                                    //    boxEdit.DataBindings.Add("SetValue", dataSource, strCol, false, DataSourceUpdateMode.OnPropertyChanged);
                                    //}
                                }
                                else
                                {
                                    ctrl.Text = null;
                                }
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw e;
                }

            }
        }

        public static Version ClickOnceUpdatedVersion
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                    return ad.UpdatedVersion;
                }
                return null;
            }
        }
        public static Version ClickOnceCurrentVersion
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                    return ad.CurrentVersion;
                }
                return null;
            }
        }
        
        

        // Return the short file name for a long file name.
        protected static string ShortFileName(string long_name)
        {
            char[] name_chars = new char[1024];
            long length = GetShortPathName(
                long_name, name_chars,
                name_chars.Length);

            string short_name = new string(name_chars);
            return short_name.Substring(0, (int)length);
        }

        public static string ShortPathName(string path)
        {
            //출처: https://enginhak.tistory.com/entry/Make-Short-Path긴-파일명-짧게-줄이는-방법-파일이름-길이-줄이기 [즐거운 개발을 꿈꾸다]
            StringBuilder strb = new StringBuilder(MAX_PATH);
            GetShortPathName(path, strb, MAX_PATH);
            return strb.ToString();
        }
        public static string DirFullName(string path)
        {
            return new System.IO.DirectoryInfo(path)?.FullName;
        }
        // Return the long file name for a short file name.
        public static string FileFullName(string path)
        {
            return new System.IO.FileInfo(path)?.FullName;
        }

        #region Mouse
        public static void SendMouseLeftButtonClickPosition(Point? point = null, bool bMoveOldPosition = false)
        {
            //Task.Delay(100);
            //System.Threading.Thread.Sleep(1000);

            int old_x, old_y;
            old_x = Cursor.Position.X;
            old_y = Cursor.Position.Y;

            if (point != null)
            {
                int x;
                int y;

                Point p = (Point)point;
                x = p.X;
                y = p.Y;

                SetCursorPos(x, y);
                //Task.Delay(100);
                //System.Threading.Thread.Sleep(1000);
                //mouse_event(WM_LBUTTONUP, 0, 0, 0, (int)UIntPtr.Zero);
                mouse_event(WM_LBUTTONDOWN, 0, 0, 0, (int)UIntPtr.Zero);
                mouse_event(WM_LBUTTONUP, 0, 0, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONDOWN | WM_LBUTTONUP, (uint)x, (uint)y, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONDOWN | WM_LBUTTONUP, (uint)x, (uint)y, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONDOWN | WM_LBUTTONUP, (uint)x, (uint)y, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONDOWN | WM_LBUTTONUP, (uint)x, (uint)y, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONDOWN | WM_LBUTTONUP, (uint)x, (uint)y, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONDOWN | WM_LBUTTONUP, (uint)x, (uint)y, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONDOWN | WM_LBUTTONUP, (uint)x, (uint)y, 0, (int)UIntPtr.Zero);
                //mouse_event(WM_LBUTTONUP, 0, 0, 0, (int)UIntPtr.Zero);

            }
            else
            {
                //mouse_event(WM_LBUTTONDBLCLK, 0, 0, 0, (int)UIntPtr.Zero);
                //System.Threading.Thread.Sleep(1000);
                //mouse_event(WM_RBUTTONDOWN, 0, 0, 0, (int)UIntPtr.Zero);
                //System.Threading.Thread.Sleep(10);
                //Task.Delay(10);
                //mouse_event(WM_RBUTTONUP, 0, 0, 0, (int)UIntPtr.Zero);
                //System.Threading.Thread.Sleep(10);
                //Task.Delay(10);
                mouse_event(WM_LBUTTONDOWN, 0, 0, 0, (int)UIntPtr.Zero);
                //System.Threading.Thread.Sleep(10);
                Task.Delay(10);
                mouse_event(WM_LBUTTONUP, 0, 0, 0, (int)UIntPtr.Zero);
            }
            


            if (point != null && bMoveOldPosition == true)
            {
                SetCursorPos(old_x, old_y);
            }
        }
        #endregion

        public static Bitmap CaptureApplication(IntPtr handle, bool isActive = false)
        {
            Bitmap Result = null;
            try
            {
                if (isActive == true)
                {
                    SetForegroundWindow(handle);
                    ShowWindow(handle, SW_RESTORE);
                    System.Threading.Thread.Sleep(1000);
                }

                HxRECT rect = new HxRECT();
                IntPtr error = GetWindowRect(handle, ref rect);
                // sometimes it gives error.
                while (error == IntPtr.Zero)
                {
                    error = GetWindowRect(handle, ref rect);
                }
                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;

                Result = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (Result != null)
                {
                    Graphics.FromImage(Result).CopyFromScreen(rect.left,
                                                     rect.top,
                                                     0,
                                                     0,
                                                     new Size(width, height),
                                                     CopyPixelOperation.SourceCopy);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }

        /*
        public static long GetVersionNumber(int majorPart, int minorPart = 0, int buildPart = 0, int privatePart = 0)
        {
            return HxUtils.GetVersionNumber(majorPart, minorPart, buildPart, privatePart);
        }
        */

        public static long GetFileVersionValue(string path)
        {
            return HxUtils.FileVersionNumber(path);
        }

        public static string GetNowLongDateTime()
        {
            return HxString.GetNowLongDateTime();
        }
        public static string GetUserCustomAgent()
        {
            return HxUtils.GetOSCustomUserAgent();
        }

        public static string GetUserHostName()
        {
            return HxNet.GetUserHostName();
        }
        
        public static string GetUserLoginName()
        {
            return Environment.UserName;
        }
        public static string GetUserMachineName()
        {
            return Environment.MachineName;
        }

        public static string GetUserGlobalAddress()
        {
            return HxNet.GetUserGlobalAddress();
        }
        public static string GetUserIPAddress()
        {
            return HxNet.GetUserHostAddress();
        }
        
        public static bool GetIsNetworkAvailable()
        {
            return HxNet.GetIsNetworkAvailable();
        }
        public static bool GetIsInternetConnected()
        {
            return HxNet.GetIsInternetConnected();
        }

        public static JToken GetJsonValue(JObject json, string key)
        {
            return HxUtils.FromJObjectFindToValue(json, key);
        }
        public static JToken GetJsonValue(JToken jToken, string key)
        {
            return HxUtils.FromJTokenFindToValue(jToken, key);
        }
        public static void SetAddProgramsRemoveIcon(string iconFileName, string appTitle)
        {
            SetAddProgramsIcon(iconFileName, appTitle);
        }
        public static void SetAddProgramsIcon(string iconFileName, string appTitle)
        {
            //출처: https://oversky.tistory.com/87 [데브타임즈]
            //only run if deployed 
            try
            {
                string iconSourcePath = Path.Combine(System.Windows.Forms.Application.StartupPath, iconFileName);

                if (!File.Exists(iconSourcePath) || appTitle.IsNullOrWhiteSpaceEx() == true)
                    return;
                /*
                RegistryKey myLocalMachineUninstallKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                string[] myLocalMachineSubKeyNames = myLocalMachineUninstallKey.GetSubKeyNames();
                for (int i = 0; i < myLocalMachineSubKeyNames.Length; i++)
                {
                    RegistryKey iconKey = myLocalMachineUninstallKey.OpenSubKey(myLocalMachineSubKeyNames[i], true);
                    object iconValue = iconKey.GetValue("DisplayName");
                    if (iconValue != null && iconValue.ToString() == appTitle)
                    {
                        iconKey.SetValue("DisplayIcon", iconSourcePath);
                        break;
                    }
                }
                */

                RegistryKey myCurrUserUninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                string[] myCurrUserSubKeyNames = myCurrUserUninstallKey.GetSubKeyNames();
                for (int i = 0; i < myCurrUserSubKeyNames.Length; i++)
                {
                    RegistryKey iconKey = myCurrUserUninstallKey.OpenSubKey(myCurrUserSubKeyNames[i], true);
                    object iconValue = iconKey.GetValue("DisplayName");
                    if (iconValue != null && iconValue.ToString() == appTitle)
                    {
                        iconKey.SetValue("DisplayIcon", iconSourcePath);
                        break;
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
            }
        }
        [Obsolete("사용 지양 => SetAddProgramIconRemoveClickOnceIcon 추천")]
        public static void SetAddProgramsClickOnceRemoveIcon(string iconFileName, string appTitle)
        {
            SetAddProgramIconRemoveClickOnceIcon(iconFileName, appTitle);
        }
        public static void SetAddProgramIconRemoveClickOnceIcon(string iconFileName, string appTitle)
        {
            
            //출처: https://oversky.tistory.com/87 [데브타임즈]
            //only run if deployed 
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed && System.Deployment.Application.ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                try
                {
                    SetAddProgramsRemoveIcon(iconFileName, appTitle);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public static IntPtr GetFindWindowHandle(string lpClassName, string lpWindowName, out Point ptFindPoint)
        {
            ptFindPoint = Point.Empty;
            if (lpWindowName.IsNullOrWhiteSpaceEx() == true) return IntPtr.Zero;
            
            IntPtr Result = FindWindow(lpClassName, lpWindowName);

            ptFindPoint = GetFindWindowPoint(Result);

            return Result;
        }

        public static IntPtr GetFindWindowHandleEx(IntPtr hwndParent, IntPtr? hwndChildAfter, string lpClassName, string lpWindowName, out Point ptFindPoint)
        {
            ptFindPoint = Point.Empty;
            if (lpWindowName.IsNullOrWhiteSpaceEx() == true) return IntPtr.Zero;

            if(hwndChildAfter == null)
            {
                hwndChildAfter = IntPtr.Zero;
            }

            IntPtr Result = FindWindowEx(hwndParent, (IntPtr)hwndChildAfter, lpClassName, lpWindowName);
            ptFindPoint = GetFindWindowPoint(Result);

            return Result;
        }

        public static Point GetFindWindowPoint(IntPtr hwnd)
        {
            Point Result = Point.Empty;
            HxWINDOWINFO winFindInfo = new HxWINDOWINFO(false);
            bool bFindWin = GetWindowInfo(hwnd, ref winFindInfo);
            if (bFindWin == true)
            {
                Result = new Point(winFindInfo.rcWindow.left, winFindInfo.rcWindow.top);
            }
            return Result;
        }
        protected static Size GetFindWindowSize(IntPtr hwnd)
        {
            Size Result = Size.Empty;
            HxWINDOWINFO winFindInfo = new HxWINDOWINFO(false);
            bool bFindWin = GetWindowInfo(hwnd, ref winFindInfo);
            if (bFindWin == true)
            {
                int width = winFindInfo.rcWindow.right - winFindInfo.rcWindow.left;
                int height = winFindInfo.rcWindow.bottom = winFindInfo.rcWindow.top;
                Result = new Size(width, height);
            }
            return Result;
        }
        public static string GetFindWindowCommonDialogBoxStaticText(IntPtr hwd, string findButtonText = null)
        {
            //findButtonText = "확인"
            string Result = null;

            List<IntPtr> listChildHandle = GetChildWindows(hwd);
            if (listChildHandle != null && listChildHandle.Count == 2)
            {
                IntPtr hwdButtonOK = FindWindowEx(hwd, IntPtr.Zero, "Button", findButtonText);
                if (hwdButtonOK != null && hwdButtonOK != IntPtr.Zero)
                {
                    //string strButtonText = SbUtils.GetHandleText(hwdButtonOK);

                    IntPtr hwdStaticText = FindWindowEx(hwd, hwdButtonOK, "Static", null);
                    Result = GetHandleText(hwdStaticText);
                }
            }

            return Result;
        }

        private static ManagementObject GetWMI_ProgramName(string programName)
        {
            ManagementObject Result = null;
            if (programName.IsNullOrWhiteSpaceEx() == true) return Result;

            string strApplicationName = programName;//.Trim();

            //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Product");

            //SelectQuery query = new SelectQuery("Win32_Product");
            //ManagementObjectSearcher mos = new ManagementObjectSearcher(query);

            ManagementObjectSearcher mos = new ManagementObjectSearcher(
                  "SELECT * FROM Win32_Product WHERE Name = '" + programName + "'");
            if (mos == null) return Result;

            foreach (ManagementObject mo in mos.Get())
            {
                string strName = mo["Name"].ToStringEx();
                if (strName == strApplicationName)
                {
                    Result = mo;
                }
            }
            return Result;
        }

        private static string GetGUID_ProgramNameWMI(string programName)
        {
            string Result = null;
            if (programName.IsNullOrWhiteSpaceEx() == true) return Result;

            //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Product");

            //SelectQuery query = new SelectQuery("Win32_Product");
            //ManagementObjectSearcher mos = new ManagementObjectSearcher(query);

            ManagementObject mo = GetWMI_ProgramName(programName);
            if (mo == null) return Result;

            string strName = mo["Name"].ToStringEx();
            if (strName.IsNullOrWhiteSpaceEx() != true && strName == programName)
            {
                Result = mo["IdentifyingNumber"].ToStringEx();
            }
            return Result;
        }
        public static string GetGUID_ProgramName(string programName, bool useNotFoundIsWMI = false)
        {
            string Result = null;
            if (programName.IsNullOrWhiteSpaceEx() == true) return Result;
            try
            {
                string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            string displayName = subkey.GetValue("DisplayName").ToStringEx();
                            if (programName == displayName)
                            {
                                Result = subkey_name;
                            }
                        }
                    }
                }
                if(Result.IsNullOrWhiteSpaceEx() == true && useNotFoundIsWMI == true)
                {
                    Result = GetGUID_ProgramNameWMI(programName);
                }
                //was not found...
                return Result;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public static void ExecuteUninstallProgramUseGUID(string programGUID, string optionArg = null)
        {
            //optionArg = "/qr"
            ExecuteProcessRun(@"MsiExec.exe", $"/x {programGUID} {optionArg}"); //{1CFDF3D8-70CA-451B-9BC8-867E93E9B444}
        }
        private static bool ExecuteUninstallProgramUseName(string programName, string optionArg = null, bool useNotFoundIsWMI = false)
        {
            bool Result = false;
            if (programName.IsNullOrWhiteSpaceEx() == true) return Result;
            try
            {
                string strProgramUninstallGUID = GetGUID_ProgramName(programName, useNotFoundIsWMI);
                if(strProgramUninstallGUID.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = true;
                    ExecuteUninstallProgramUseGUID(strProgramUninstallGUID, optionArg);
                }
                //was not found...
                return Result;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }


        private static bool UninstallProgramWMI(string programName)
        {
            bool Result = false;
            if (programName.IsNullOrWhiteSpaceEx() == true) return Result;
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher(
                  "SELECT * FROM Win32_Product WHERE Name = '" + programName + "'");
                if (mos == null) return Result;

                foreach (ManagementObject mo in mos.Get())
                {
                    try
                    {
                        if (mo["Name"].ToString() == programName)
                        {
                            //object hr = mo.InvokeMethod("Uninstall", null);
                            //Result = (bool)hr;
                            ManagementBaseObject hr = mo.InvokeMethod("Uninstall", null, null);
                            var a = hr["ReturnValue"];
                            Result = a.ToBoolEx(false);
                            if (Result != true)
                            {
                                string guid = mo["IdentifyingNumber"].ToString();
                                //HxWin.ExecuteProcessRun(@"MsiExec.exe", $" /x {guid}"); //{1CFDF3D8-70CA-451B-9BC8-867E93E9B444}
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        //this program may not have a name property, so an exception will be thrown
                    }
                }

                //was not found...
                return Result;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }


        /*
        private static bool IsFindDialogBoxStaticText(Process procOwner, IntPtr hwndParent, string findStaticText, out IntPtr hwndDialogBox, string findButtonText = null, bool IsProcessThread = true)
        {
            const string _CLASS_NAME_Common_Dialog_ = "#32770";
            //findStaticText = "대안설정이 완료되었습니다."
            bool Result = false;
            hwndDialogBox = IntPtr.Zero;

            if (IsProcessThread == true)
            {
                List<IntPtr> handleThreadList = new List<IntPtr>();
                foreach (ProcessThread thread in procOwner.Threads)
                {
                    HxWin.EnumThreadWindows(thread.Id, (hWnd, lp) => { handleThreadList.Add(hWnd); return true; }, IntPtr.Zero);
                }

                List<IntPtr> handleChildList = new List<IntPtr>();
                EnumChildWindows(hwndParent, (hWnd, lp) => { handleChildList.Add(hWnd); return true; }, IntPtr.Zero);

                var q = handleThreadList.Where(hwd => GetWindow(hwd, GW_OWNER) == procOwner.MainWindowHandle).Intersect(handleChildList);

                if (q != null && q.Any())
                {
                    // find first window
                    foreach (var hwnd in q)
                    {
                        Result = IsFindDialogBoxStaticText(hwnd, _CLASS_NAME_Common_Dialog_, null, findStaticText, out hwndDialogBox, findButtonText);
                        if (Result == true)
                        {
                            hwndDialogBox = hwnd;
                            break;
                        }
                    }
                }
            }

            if (Result != true)
            {
                Result = IsFindDialogBoxStaticText(hwndParent, _CLASS_NAME_Common_Dialog_, null, findStaticText, out hwndDialogBox, findButtonText);
            }

            Result = false;

            return Result;
        }
        */

        public static void DoRunTaskKill(string applicationFIleName)
        {
            if (applicationFIleName.IsNullOrWhiteSpaceEx() != true)
            {
                string taskName = HxFile.GetFileNameWithOutExt(applicationFIleName);
                if (taskName.IsNullOrWhiteSpaceEx() == true) return;

                Process[] procs = Process.GetProcessesByName(taskName);
                if (procs != null && procs.Length > 0)
                {
                    foreach (Process p in procs) 
                    { 
                        p.Kill(); 
                    }
                }
                procs = Process.GetProcessesByName(taskName);
                if (procs != null && procs.Length > 0)
                {
                    Process.Start("taskkill", $"/F /IM {taskName}.exe");
                }
            }
        }
    }

    

    public class HxWinUtils : HxWin
    {
        static HxWinUtils()
        {
            ; ;
        }
    }
}
