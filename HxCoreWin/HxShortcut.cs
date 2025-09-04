using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    /*
    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLink
    {
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    internal interface IShellLink
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
        void GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out short pwHotkey);
        void SetHotkey(short wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        void Resolve(IntPtr hwnd, int fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }
    */

    public class HxShortcut
    {
        /*
        private static Type m_type = Type.GetTypeFromProgID("WScript.Shell");
        private static object m_shell = Activator.CreateInstance(m_type);

        [ComImport, TypeLibType((short)0x1040), Guid("F935DC23-1CF0-11D0-ADB9-00C04FD58A0B")]
        private interface IWshShortcut
        {
            [DispId(0)]
            string FullName { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0)] get; }
            [DispId(0x3e8)]
            string Arguments { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0x3e8)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x3e8)] set; }
            [DispId(0x3e9)]
            string Description { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0x3e9)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x3e9)] set; }
            [DispId(0x3ea)]
            string Hotkey { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0x3ea)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x3ea)] set; }
            [DispId(0x3eb)]
            string IconLocation { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0x3eb)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x3eb)] set; }
            [DispId(0x3ec)]
            string RelativePath { [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x3ec)] set; }
            [DispId(0x3ed)]
            string TargetPath { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0x3ed)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x3ed)] set; }
            [DispId(0x3ee)]
            int WindowStyle { [DispId(0x3ee)] get; [param: In] [DispId(0x3ee)] set; }
            [DispId(0x3ef)]
            string WorkingDirectory { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0x3ef)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x3ef)] set; }
            [TypeLibFunc((short)0x40), DispId(0x7d0)]
            void Load([In, MarshalAs(UnmanagedType.BStr)] string PathLink);
            [DispId(0x7d1)]
            void Save();
        }
        
        public static void Create(string fileName, string targetPath, string arguments, string workingDirectory, string description, string hotkey, string iconPath)
        {
            if (!fileName.IsNullOrWhiteSpaceEx())
            {
                if (!fileName.ToLower().Trim().EndsWith(".lnk"))
                {
                    fileName += ".lnk";
                }
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)m_type.InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, m_shell, new object[] { fileName });
                shortcut.Description = description;
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = workingDirectory;
                shortcut.Arguments = arguments;
                if (!hotkey.IsNullOrWhiteSpaceEx())
                {
                    shortcut.Hotkey = hotkey;
                }
                if (!iconPath.IsNullOrWhiteSpaceEx() && File.Exists(iconPath))
                {
                    shortcut.IconLocation = iconPath;
                }
                shortcut.Save();
            }



        }
        */

        public static void CreateShortcut(string dirPath, string linkName, string targetPath, string arguments, string workingDirectory, string description, string hotkey, string iconPath)
        {
            if (dirPath.IsNullOrWhiteSpaceEx())
            {
                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            if (!linkName.IsNullOrWhiteSpaceEx())
            {
                switch (dirPath.ToLower())
                {
                    case "desktop":
                        object shDesktop = (object)"Desktop";
                        //IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                        //dirPath = (string)shell.SpecialFolders.Item(ref shDesktop);
                        dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        break;
                }
                string fileFullPath = Path.Combine(dirPath, linkName);
                Create(fileFullPath, targetPath, arguments, workingDirectory, description, hotkey, iconPath);
                //CreateShortcut()
                //IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                //if (!linkName.ToLower().Trim().EndsWith(".lnk"))
                //{
                //    linkName += ".lnk";
                //}
                //switch (dirPath.ToLower())
                //{
                //    case "desktop":
                //        object shDesktop = (object)"Desktop";
                //        dirPath = (string)shell.SpecialFolders.Item(ref shDesktop);
                //        break;
                //}
                //string shortcutAddress = Path.Combine(dirPath, linkName);
                //IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
                //shortcut.Description = description;
                //shortcut.TargetPath = targetPath;
                //shortcut.WorkingDirectory = workingDirectory;
                //shortcut.Arguments = arguments;
                //if (!hotkey.IsNullOrWhiteSpaceEx())
                //{
                //    shortcut.Hotkey = hotkey;
                //}
                //if (!iconPath.IsNullOrWhiteSpaceEx() && File.Exists(iconPath))
                //{
                //    shortcut.IconLocation = iconPath;
                //}
                //shortcut.Save();
            }
        }

        public static void Create(string fileFullName, string targetPath, string arguments, string workingDirectory, string description, string hotkey, string iconPath)
        {
            if (!fileFullName.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                    if (!fileFullName.ToLower().Trim().EndsWith(".lnk"))
                    {
                        fileFullName += ".lnk";
                    }
                    string shortcutAddress = fileFullName; //Path.Combine(dirPath, linkName);
                    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
                    shortcut.Description = description;
                    shortcut.TargetPath = targetPath;
                    shortcut.WorkingDirectory = workingDirectory;
                    shortcut.Arguments = arguments;
                    if (!hotkey.IsNullOrWhiteSpaceEx())
                    {
                        shortcut.Hotkey = hotkey;
                    }
                    if (!iconPath.IsNullOrWhiteSpaceEx() && File.Exists(iconPath))
                    {
                        shortcut.IconLocation = iconPath;
                    }
                    shortcut.Save();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }

        public static void CreateDesktop(string linkName, string targetPath, string arguments, string workingDirectory, string description, string hotkey, string iconPath)
        {
            CreateShortcut("desktop", linkName, targetPath, arguments, workingDirectory, description, hotkey, iconPath);
            //if (!linkName.IsNullOrWhiteSpaceEx())
            //{
            //    object shDesktop = (object)"Desktop";
            //    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            //    if (!linkName.ToLower().Trim().EndsWith(".lnk"))
            //    {
            //        linkName += ".lnk";
            //    }
            //    string shortcutAddress = Path.Combine((string)shell.SpecialFolders.Item(ref shDesktop), linkName);
            //    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
            //    shortcut.Description = description;
            //    shortcut.TargetPath = targetPath;
            //    shortcut.WorkingDirectory = workingDirectory;
            //    shortcut.Arguments = arguments;
            //    if (!hotkey.IsNullOrWhiteSpaceEx())
            //    {
            //        shortcut.Hotkey = hotkey;
            //    }
            //    if (!iconPath.IsNullOrWhiteSpaceEx() && File.Exists(iconPath))
            //    {
            //        shortcut.IconLocation = iconPath;
            //    }
            //    shortcut.Save();
            //}
        }
    }
}
