using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    public partial class HxWin
    {

        public static RegistryKey GetRegistryKeyRoot(HxRegistryKeyRootType keyRootType)
        {
            RegistryKey Result = null;
            switch (keyRootType)
            {
                case HxRegistryKeyRootType.ClassesRoot:
                    Result = Registry.ClassesRoot;
                    break;
                case HxRegistryKeyRootType.CurrentUser:
                    Result = Registry.CurrentUser;
                    break;
                case HxRegistryKeyRootType.Default:
                case HxRegistryKeyRootType.LocalMachine:
                    Result = Registry.LocalMachine;
                    break;
                case HxRegistryKeyRootType.Users:
                    Result = Registry.Users;
                    break;
                case HxRegistryKeyRootType.CurrentConfig:
                    Result = Registry.CurrentConfig;
                    break;
                case HxRegistryKeyRootType.PerformanceData:
                    Result = Registry.PerformanceData;
                    break;
                default:
                    Result = null;
                    break;
            }
            return Result;
        }

        public static RegistryKey RegistryCreateSubKey(string keyPath, HxRegistryKeyRootType keyRootType = HxRegistryKeyRootType.Default)
        {
            RegistryKey Result = null;
            try
            {
                RegistryKey registryKey = GetRegistryKeyRoot(keyRootType);
                if (registryKey != null)
                {
                    //if(RegistrySubKeyExists(registryKey, keyPath) == false)
                    //{
                    //    registryKey.CreateSubKey(keyPath);
                    //}
                    Result = registryKey.OpenSubKey(keyPath, true);
                    if (Result == null)
                    {
                        Result = registryKey.CreateSubKey(keyPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
            
        }

        public static bool RegistrySubKeyExists(RegistryKey keyName, string subKeyName)
        {
            RegistryKey regKey = null;

            try
            {
                regKey = keyName.OpenSubKey(subKeyName, true);
                return regKey != null;
            }
            finally
            {
                if (regKey != null)
                {
                    regKey.Close();
                }
            }
        }
        
        private static void RegistryCreateKey(RegistryKey regKey, string subKeyName)
        {
            try
            {
                if (regKey != null)
                {
                    if (RegistrySubKeyExists(regKey, subKeyName) == true)
                    {
                        return;
                    }
                    else
                    {
                        regKey.CreateSubKey(subKeyName);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw;
            }
                
        }
        
        public static void RegistryDeleteSubKey(RegistryKey regKey, string subKeyName, bool deleteAllSubKey)
        {
            if (RegistrySubKeyExists(regKey, subKeyName) == false)
            {
                return;
            }

            if (deleteAllSubKey == true)
            {
                regKey.DeleteSubKeyTree(subKeyName);
            }
            else
            {
                regKey.DeleteSubKey(subKeyName);
            }
        }

        #region Associcate (File Extension) Methods
        public static void SetRegistryLocalMachineAssocFileExt(string assocExeFullPath, string fileExt, string extType, string fileTypeDesc = null, bool bClassTypeSetValue = false)
        {
            using (RegistryKey classesKey = Registry.LocalMachine.OpenSubKey(@"Software\Classes", true))
            {
                if (extType.IsNullOrWhiteSpaceEx() && !assocExeFullPath.IsNullOrWhiteSpaceEx())
                {
                    string[] temps = assocExeFullPath.Split('\\');
                    if (temps != null && temps.Length > 0)
                    {
                        extType = string.Format("{0}.{1}", temps[temps.Length - 1], fileExt.Replace(".", null));
                    }
                }
                using (RegistryKey extKey = classesKey.CreateSubKey(fileExt))
                {
                    if (bClassTypeSetValue == true)
                    {
                        extKey.SetValue(null, extType);
                    }
                }

                // or, use Registry.SetValue method
                using (RegistryKey typeKey = classesKey.CreateSubKey(extType))
                {
                    if (!fileTypeDesc.IsNullOrWhiteSpaceEx())
                    {
                        typeKey.SetValue(null, fileTypeDesc);
                    }
                    using (RegistryKey shellKey = typeKey.CreateSubKey("shell"))
                    {
                        using (RegistryKey openKey = shellKey.CreateSubKey("open"))
                        {
                            using (RegistryKey commandKey = openKey.CreateSubKey("command"))
                            {
                                //string assocExePath = GetAppDirPath();
                                string assocCommand = string.Format("\"{0}\" \"%1\"", assocExeFullPath);

                                commandKey.SetValue(null, assocCommand);
                            }
                        }
                    }
                }
            }
        }
        public static void SetRegistryAssocFileExt(string assocExeFullPath, string fileExt, string extType, string fileTypeDesc = null, bool bClassTypeSetValue = false)
        {
            using (RegistryKey classesKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes", true))
            {
                if (extType.IsNullOrWhiteSpaceEx() && !assocExeFullPath.IsNullOrWhiteSpaceEx())
                {
                    string[] temps = assocExeFullPath.Split('\\');
                    if (temps != null && temps.Length > 0)
                    {
                        extType = string.Format("{0}.{1}", temps[temps.Length - 1], fileExt.Replace(".", null));
                    }
                }
                using (RegistryKey extKey = classesKey.CreateSubKey(fileExt))
                {
                    if (bClassTypeSetValue == true)
                    {
                        extKey.SetValue(null, extType);
                    }
                }

                // or, use Registry.SetValue method
                using (RegistryKey typeKey = classesKey.CreateSubKey(extType))
                {
                    if (!fileTypeDesc.IsNullOrWhiteSpaceEx())
                    {
                        typeKey.SetValue(null, fileTypeDesc);
                    }
                    using (RegistryKey shellKey = typeKey.CreateSubKey("shell"))
                    {
                        using (RegistryKey openKey = shellKey.CreateSubKey("open"))
                        {
                            using (RegistryKey commandKey = openKey.CreateSubKey("command"))
                            {
                                //string assocExePath = GetAppDirPath();
                                string assocCommand = string.Format("\"{0}\" \"%1\"", assocExeFullPath);

                                commandKey.SetValue(null, assocCommand);
                            }
                        }
                    }
                }
            }
        }

        private static void SetRegistryAssocApplication(RegistryKey classesKey, string assocExeFullPath, bool register = true)
        {
            using (RegistryKey appKey = classesKey.OpenSubKey("Applications", true))
            {
                if (register == true)
                {
                    using (RegistryKey exeKey = appKey.CreateSubKey(assocExeFullPath))
                    {
                        SetRegistryAssocShellOpenCommand(exeKey, assocExeFullPath);
                    }
                }
            }
        }

        private static void SetRegistryAssocShellOpenCommand(RegistryKey baseKey, string assocExeFullPath)
        {
            using (RegistryKey shellKey = baseKey.CreateSubKey("shell"))
            {
                using (RegistryKey openKey = shellKey.CreateSubKey("open"))
                {
                    using (RegistryKey commandKey = openKey.CreateSubKey("command"))
                    {
                        //string assocExePath = GetAppDirPath();
                        string assocCommand = string.Format("\"{0}\" \"%1\"", assocExeFullPath);

                        commandKey.SetValue(null, assocCommand);
                    }
                }
            }
        }
        #endregion

        #region Associcate (File Extension) Methods - Type#2
        public static void SetAssociation_User(string fileExt, string OpenWith, string ExecutableName)
        {
            //참조 : https://code-examples.net/ko-kr/q/28ec16
            try
            {
                using (RegistryKey User_Classes = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\", true))
                using (RegistryKey User_Ext = User_Classes.CreateSubKey("." + fileExt))
                using (RegistryKey User_AutoFile = User_Classes.CreateSubKey(fileExt + "_auto_file"))
                using (RegistryKey User_AutoFile_Command = User_AutoFile.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                using (RegistryKey ApplicationAssociationToasts = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts\\", true))
                using (RegistryKey User_Classes_Applications = User_Classes.CreateSubKey("Applications"))
                using (RegistryKey User_Classes_Applications_Exe = User_Classes_Applications.CreateSubKey(ExecutableName))
                using (RegistryKey User_Application_Command = User_Classes_Applications_Exe.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                using (RegistryKey User_Explorer = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\." + fileExt))
                using (RegistryKey User_Choice = User_Explorer.OpenSubKey("UserChoice"))
                {
                    User_Ext.SetValue("", fileExt + "_auto_file", RegistryValueKind.String);
                    User_Classes.SetValue("", fileExt + "_auto_file", RegistryValueKind.String);
                    User_Classes.CreateSubKey(fileExt + "_auto_file");
                    User_AutoFile_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                    ApplicationAssociationToasts.SetValue(fileExt + "_auto_file_." + fileExt, 0);
                    ApplicationAssociationToasts.SetValue(@"Applications\" + ExecutableName + "_." + fileExt, 0);
                    User_Application_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                    User_Explorer.CreateSubKey("OpenWithList").SetValue("a", ExecutableName);
                    User_Explorer.CreateSubKey("OpenWithProgids").SetValue(fileExt + "_auto_file", "0");
                    if (User_Choice != null) User_Explorer.DeleteSubKey("UserChoice");
                    User_Explorer.CreateSubKey("UserChoice").SetValue("ProgId", @"Applications\" + ExecutableName);
                }
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception excpt)
            {
                Debug.WriteLine(excpt);
                //Your code here
            }
        }
        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const int SHCNF_FLUSH = 0x1000;

        private static void FileAssociations()
        {
            var filePath = Process.GetCurrentProcess().MainModule.FileName;
            EnsureAssociationsSet(
            new FileAssociation
            {
                Extension = ".pdf",
                ProgId = "UCS_Editor_File",
                FileTypeDescription = "UCS File",
                ExecutableFilePath = filePath
            });

        }
        public static void EnsureAssociationsSet(params FileAssociation[] associations)
        {
            bool madeChanges = false;
            foreach (var association in associations)
            {
                madeChanges |= SetWriteRegistryAssociation(
                    association.Extension,
                    association.ProgId,
                    association.FileTypeDescription,
                    association.ExecutableFilePath);
            }

            if (madeChanges)
            {
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void EnsureAssociationsSet_New2023(params FileAssociation[] associations)
        {
            bool madeChanges = false;
            foreach (var association in associations)
            {
                madeChanges |= SetWriteRegistryAssociation_New2023(
                    association.Extension,
                    association.ProgId,
                    association.FileTypeDescription,
                    association.ExecutableFilePath);
            }

            if (madeChanges)
            {
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void SetEnsureAssociations(params FileAssociation[] associations)
        {
            EnsureAssociationsSet(associations);
        }

        public static bool SetWriteRegistryAssociation(string extension, string progId, string fileTypeDescription, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{extension}", progId);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}", fileTypeDescription);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }
        public static bool SetWriteRegistryAssociation_New2023(string extension, string progId, string fileTypeDescription, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{extension}", progId);
            madeChanges |= SetKeyDefaultValue($@"Applications\{progId}", fileTypeDescription);
            madeChanges |= SetKeyDefaultValue($@"Applications\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }


        public static void GetRegistryAssociation(string extension, string progId = null)
        {
            string strClassesExtPath = $@"Software\Classes\{extension}";



            if (progId.IsNullOrWhiteSpaceEx() != true)
            {
                //string strClassesProgIdPath = $@"Software\Classes\{progId}";
                try
                {
                    
                    Registry.CurrentUser.CreateSubKey(strClassesExtPath).DeleteSubKey(progId);
                    Registry.ClassesRoot.CreateSubKey(extension).DeleteSubKey(progId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw;
                }
                try
                {
                    string strClassesRootPath = $@"Software\Classes\";
                    Registry.CurrentUser.CreateSubKey(strClassesRootPath).DeleteSubKey(progId);
                    Registry.ClassesRoot.CreateSubKey(extension).DeleteSubKey(progId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw ex;
                }

            }
        }

        public static void SetDeleteRegistryAssociation(string extension, string progId)
        {
            
            //string strClassesCommandPath = $@"Software\Classes\{progId}\shell\open\command";
            //RegistryKey regClassesRootKey = Registry.CurrentUser.CreateSubKey(strClassesRootPath);
            //RegistryKey regClassesExtKey = Registry.CurrentUser.CreateSubKey(strClassesExtPath);
            if (progId.IsNullOrWhiteSpaceEx() != true)
            {
                //string strClassesProgIdPath = $@"Software\Classes\{progId}";
                try
                {
                    string strClassesExtPath = $@"Software\Classes\{extension}";
                    Registry.CurrentUser.CreateSubKey(strClassesExtPath).DeleteSubKey(progId);
                    Registry.ClassesRoot.CreateSubKey(extension).DeleteSubKey(progId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw;
                }
                try
                {
                    string strClassesRootPath = $@"Software\Classes\";
                    Registry.CurrentUser.CreateSubKey(strClassesRootPath).DeleteSubKey(progId);
                    Registry.ClassesRoot.CreateSubKey(extension).DeleteSubKey(progId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw ex;
                }
                
            }

        }

        private static bool SetKeyDefaultValue(string keyPath, string value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key.GetValue(null) as string != value)
                {
                    key.SetValue(null, value);
                    return true;
                }
            }

            return false;
        }
        #endregion

        public static bool CheckInstalledApplications(string programName)
        {
            bool isInstalled = false;

            foreach
            (string item in Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall").GetSubKeyNames())
            {
                object itemProgramName
                = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + item).GetValue("DisplayName");

                Console.WriteLine(itemProgramName);

                if (string.Equals(itemProgramName, programName))
                {
                    Console.WriteLine("Install status: INSTALLED");
                    isInstalled = true;
                    break;
                }
            }
            return isInstalled;
        }
    }
    public class FileAssociation
    {
        public string Extension { get; set; }
        public string ProgId { get; set; }
        public string FileTypeDescription { get; set; }
        public string ExecutableFilePath { get; set; }
    }
}
