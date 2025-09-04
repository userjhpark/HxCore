using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    public struct HxLoadAssemblyCosRec
    {
        private string LoadFullName;
        public Assembly LoadAssembly;
        public Version LoadAssemblyVersion;
        public FileVersionInfo LoadFileInfo;

        //public HxClickOnce AppClickOnce;

        public string AppFriendlyName;
        public string AppProcessName;
        public string AppMainFullName;
        public string AppEnvFullName;

        public string LoadAssemblyDir => LoadAssembly?.CodeBase;
        public string LoadAssemblySubject => LoadFileInfo?.FileDescription;
        public string LoadAssemblyComment => LoadFileInfo?.Comments;
        public string LoadAssemblyCompanyName => LoadFileInfo?.CompanyName;
        public string LoadAssemblyOriginalFilename => LoadFileInfo?.OriginalFilename;
        public string LoadAssemblyProductName => LoadFileInfo?.ProductName;
        public string LoadAssemblyFullName => LoadFileInfo?.FileName;
        public string LoadAssemblyFileName => LoadFileInfo?.InternalName;
        public Version LoadAssemblyFileVersion => LoadFileInfo != null ? new Version(LoadFileInfo.FileMajorPart, LoadFileInfo.FileMinorPart, LoadFileInfo.FileBuildPart, LoadFileInfo.FilePrivatePart) : null;

        //public string 
        //string s1 = AssemblyFile.ProductName;
        //string s2 = AssemblyFile.FileDescription; //Subject
        //string s3 = AssemblyFile.FileName; //Full Name
        //string s4 = AssemblyFile.Comments; //Description
        //string s5 = AssemblyFile.CompanyName;
        //string s6 = AssemblyFile.InternalName; //Name with ext

        public HxLoadAssemblyCosRec(string inputFileName = null, string inputCosUri = null)
        {
            string strFileName = inputFileName;
            string strCosUri = inputCosUri;
            AppFriendlyName = null;
            AppProcessName = null;
            AppMainFullName = null;
            AppEnvFullName = null;
            LoadFullName = null;
            LoadAssembly = null;
            LoadAssemblyVersion = null;
            LoadFileInfo = null;

            //AppClickOnce = null;

            AppFriendlyName = AppDomain.CurrentDomain.FriendlyName;
            AppProcessName = Process.GetCurrentProcess().ProcessName;
            AppMainFullName = Process.GetCurrentProcess().MainModule.FileName;
            AppEnvFullName = Environment.GetCommandLineArgs()?[0];

            strFileName = HxFile.GetFileFullPath(strFileName);

            if (strFileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(strFileName))
            {
                LoadFullName = strFileName;
            }

            if (LoadFullName.IsNullOrWhiteSpaceEx() == true || HxFile.FileExists(LoadFullName) != true)
            {
                if (AppMainFullName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(AppMainFullName))
                {
                    LoadFullName = AppMainFullName;
                }
                else if (AppEnvFullName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(AppEnvFullName))
                {
                    LoadFullName = AppEnvFullName;
                }
            }

            if (LoadFullName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(LoadFullName))
            {
                LoadAssembly = Assembly.LoadFile(LoadFullName);
                AssemblyName an = LoadAssembly?.GetName();
                if (an != null && an.FullName.IsNullOrWhiteSpaceEx() != true)
                {
                    LoadAssemblyVersion = an.Version;
                }

                LoadFileInfo = FileVersionInfo.GetVersionInfo(LoadAssembly.Location);
                string s1 = LoadFileInfo.ProductName;
                string s2 = LoadFileInfo.FileDescription; //Subject
                string s3 = LoadFileInfo.FileName; //Full Name
                string s4 = LoadFileInfo.Comments; //Description
                string s5 = LoadFileInfo.CompanyName;
                string s6 = LoadFileInfo.InternalName; //Name with ext
                //if(FileVersionInfo != null)
                //{
                //    FileVersion = new Version(FileVersionInfo.FileMajorPart, FileVersionInfo.FileMinorPart, FileVersionInfo.FileBuildPart, FileVersionInfo.FilePrivatePart);
                //}
            }

            
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                strCosUri = ad.ActivationUri.AbsoluteUri;
                HxClickOnce.InstallUpdateSyncWithInfo(false);
            }
            else if (inputCosUri.IsNullOrWhiteSpaceEx() != true)
            {
                
            }
        }
    }
    public class HxLoadAssemblyCos : HxLoadAssembly
    {
        public HxLoadAssemblyCos(string inputFileName = null, string inputCosUri = null)
            : base(inputFileName)
        {

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                string strCosUri = ad.ActivationUri.AbsoluteUri;
                HxClickOnce.InstallUpdateSyncWithInfo(false);
            }
            else if (inputCosUri.IsNullOrWhiteSpaceEx() != true)
            {

            }
        }
    }
}
