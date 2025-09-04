using Microsoft.Win32;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    //https://www.codeproject.com/Articles/50064/OSIcon
    //https://pythonq.com/so/c%23/1848022
    //https://docs.microsoft.com/en-us/dotnet/api/system.drawing.icon.extractassociatedicon?redirectedfrom=MSDN&view=net-5.0#System_Drawing_Icon_ExtractAssociatedIcon_System_String_
    partial class HxWin
    {
        private static OSIcon.IconSize GetOSIconSize(uint iconSize = 0)
        {
            OSIcon.IconSize Result = OSIcon.IconSize.Large;
            if (iconSize == 1)
            {
                Result = OSIcon.IconSize.Small;
            }
            else if (iconSize == 2)
            {
                Result = OSIcon.IconSize.ExtraLarge;
            }
            else if (iconSize >= 3)
            {
                Result = OSIcon.IconSize.Jumbo;
            }
            return Result;
        }
        public static Icon GetFileExtIcon(string fileExtName, uint iconSize = 0)
        {
            //Icon ico = Icon.ExtractAssociatedIcon(@"C:\WINDOWS\system32\notepad.exe");
            OSIcon.IconSize size = GetOSIconSize(iconSize);
            return GetFileExtIconInfo(fileExtName, size)?.Icon;
        }
        public static Bitmap GetFileExtIconBitmap(string fileExtName, uint iconSize = 0)
        {
            return GetFileExtIcon(fileExtName, iconSize)?.ToBitmap();
        }

        private static OSIcon.IconInfo GetFileExtIconInfo(string fileExtName, OSIcon.IconSize size = OSIcon.IconSize.Large)
        {
            if (fileExtName.IsNullOrWhiteSpaceEx() == true) return null;

            fileExtName = fileExtName.Trim();
            if (fileExtName.ToLower() != "folder" && fileExtName.StartsWith(".") != true)
            {
                fileExtName = "." + fileExtName;
            }
            OSIcon.IconInfo osiIcon = OSIcon.IconReader.GetFileIcon(fileExtName, size);
            return osiIcon;
        }

        public static Icon GetFilePathIcon(string path, uint iconSize = 0)
        {
            OSIcon.IconSize size = GetOSIconSize(iconSize);
            return GetFilePathIconInfo(path, size)?.Icon;
        }
        public static Bitmap GetFilePathIconBitmap(string path, uint iconSize = 0)
        {
            return GetFilePathIcon(path, iconSize)?.ToBitmap();
        }

        private static OSIcon.IconInfo GetFilePathIconInfo(string path, OSIcon.IconSize size = OSIcon.IconSize.Large)
        {
            return OSIcon.IconReader.ExtractIconFromFileEx(path, size);

        }

        public static Dictionary<string, string> GetFileTypeAndIcon()
        {
            return GetFileTypeAndOSIcon();
        }
        public static Dictionary<string, string> GetFileTypeAndOSIcon()
        {
            return OSIcon.IconReader.GetFileTypeAndIcon();
        }

        public static Dictionary<string, string> GetFileTypeAndRegistryIcon()
        {
            //참조 : https://www.codeproject.com/Articles/29137/Get-Registered-File-Types-and-Their-Associated-Ico
            try
            {
                RegistryKey rkRoot = Registry.ClassesRoot;
                string[] keyNames = rkRoot.GetSubKeyNames();
                Dictionary<string, string> iconsInfo = new Dictionary<string, string>();
                foreach (string keyName in keyNames)
                {
                    if (keyName.IsNullOrWhiteSpaceEx() == true) continue;

                    int indexOfPoint = keyName.IndexOf(".");
                    if (indexOfPoint != 0) continue;

                    RegistryKey rkFileType = rkRoot.OpenSubKey(keyName);
                    if (rkFileType == null) continue;

                    object defaultValue = rkFileType.GetValue("");
                    if (defaultValue == null) continue;

                    string defaultIcon = defaultValue.ToString() + "\\DefaultIcon";
                    RegistryKey rkFileIcon = rkRoot.OpenSubKey(defaultIcon);
                    if (rkFileIcon != null)
                    {
                        //Get the file contains the icon and the index of the icon in that file.
                        object value = rkFileIcon.GetValue("");
                        if (value != null)
                        {
                            //Clear all unnecessary " sign in the string to avoid error.
                            string fileParam = value.ToString().Replace("\"", "");
                            iconsInfo.Add(keyName, fileParam);
                        }
                        rkFileIcon.Close();
                    }
                    rkFileType.Close();
                }
                rkRoot.Close();
                return iconsInfo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw;
            }
            return null;
        }
    }
    
    /*
    // 출처 : https://www.codeproject.com/Articles/29137/Get-Registered-File-Types-and-Their-Associated-Ico
    partial class HxWin
    {
        [DllImport("shell32.dll", EntryPoint = "ExtractIconA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
        private static unsafe extern int DestroyIcon(IntPtr hIcon);

        /// <summary>
        ///Gets registered file types and their associated icon in the system.

        /// </summary>
        /// <returns>Returns a hash table which contains the file extension as keys, 
        /// the icon file and param as values.</returns>
        /// <summary>
        /// Structure that encapsulates basic information of icon embedded in a file.
        /// </summary>
        public static Hashtable GetFileTypeAndIcon()
        {
            try
            {
                // Create a registry key object to represent the 
                // HKEY_CLASSES_ROOT registry section
                RegistryKey rkRoot = Registry.ClassesRoot;
                //Gets all sub keys' names.
                string[] keyNames = rkRoot.GetSubKeyNames();
                Hashtable iconsInfo = new Hashtable();
                //Find the file icon.
                foreach (string keyName in keyNames)
                {
                    if (String.IsNullOrEmpty(keyName))
                        continue;
                    int indexOfPoint = keyName.IndexOf(".");

                    //If this key is not a file extension, .zip), skip it.
                    if (indexOfPoint != 0)
                        continue;
                    RegistryKey rkFileType = rkRoot.OpenSubKey(keyName);
                    if (rkFileType == null)
                        continue;
                    //Gets the default value of this key that 
                    //contains the information of file type.
                    object defaultValue = rkFileType.GetValue("");
                    if (defaultValue == null)
                        continue;
                    //Go to the key that specifies the default icon 
                    //associates with this file type.
                    string defaultIcon = defaultValue.ToString() + "\\DefaultIcon";
                    RegistryKey rkFileIcon = rkRoot.OpenSubKey(defaultIcon);
                    if (rkFileIcon != null)
                    {
                        //Get the file contains the icon and the index of the icon in that file.
                        object value = rkFileIcon.GetValue("");
                        if (value != null)
                        {
                            //Clear all unnecessary " sign in the string to avoid error.
                            string fileParam = value.ToString().Replace("\"", "");
                            iconsInfo.Add(keyName, fileParam);
                        }
                        rkFileIcon.Close();
                    }
                    rkFileType.Close();
                }
                rkRoot.Close();
                return iconsInfo;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Parses the parameters string to the structure of EmbeddedIconInfo.
        /// </summary>
        /// <param name="fileAndParam">The params string, such as ex: 
        ///    "C:\\Program Files\\NetMeeting\\conf.exe,1".</param>
        protected static EmbeddedIconInfo getEmbeddedIconInfo(string fileAndParam)
        {
            EmbeddedIconInfo embeddedIcon = new EmbeddedIconInfo();

            if (String.IsNullOrEmpty(fileAndParam))
                return embeddedIcon;

            //Use to store the file contains icon.
            string fileName = String.Empty;

            //The index of the icon in the file.
            int iconIndex = 0;
            string iconIndexString = String.Empty;

            int commaIndex = fileAndParam.IndexOf(",");
            //if fileAndParam is some thing likes this: 
            //"C:\\Program Files\\NetMeeting\\conf.exe,1".
            if (commaIndex > 0)
            {
                fileName = fileAndParam.Substring(0, commaIndex);
                iconIndexString = fileAndParam.Substring(commaIndex + 1);
            }
            else
                fileName = fileAndParam;

            if (!String.IsNullOrEmpty(iconIndexString))
            {
                //Get the index of icon.
                iconIndex = int.Parse(iconIndexString);
                if (iconIndex < 0)
                    iconIndex = 0;  //To avoid the invalid index.
            }

            embeddedIcon.FileName = fileName;
            embeddedIcon.IconIndex = iconIndex;

            return embeddedIcon;
        }

        /// <summary>
        /// Extract the icon from file.
        /// <param name="fileAndParam">The params string, such as ex: 
        ///    "C:\\Program Files\\NetMeeting\\conf.exe,1".</param>
        /// <returns>This method always returns the large size of the icon 
        ///    (may be 32x32 px).</returns>
        public static Icon ExtractIconFromFile(string fileAndParam)
        {
            try
            {
                EmbeddedIconInfo embeddedIcon = getEmbeddedIconInfo(fileAndParam);

                //Gets the handle of the icon.
                IntPtr lIcon = ExtractIcon(0, embeddedIcon.FileName,
                            embeddedIcon.IconIndex);

                //Gets the real icon.
                return Icon.FromHandle(lIcon);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }

    public struct EmbeddedIconInfo
    {
        public string FileName;
        public int IconIndex;
    }
    public enum ImageSize
    {
        /// <summary>
        /// View image in 16x16 px.
        /// </summary>
        Small,

        /// <summary>
        /// View image in 32x32 px.
        /// </summary>
        Large
    }*/
}
