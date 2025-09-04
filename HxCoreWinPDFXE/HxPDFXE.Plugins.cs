using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win.PDFXE
{
    partial class HxPDFXE
    {
        private Dictionary<string, string> plugins = new Dictionary<string, string>();
        /// <summary>
        /// StartLoadingPlugins
        /// </summary>
        public void PluginStartLoading()
        {
            if (this.Inst != null)
            {
                this.Inst.StartLoadingPlugins();
            }
        }
        /// <summary>
        /// FinishLoadingPlugins
        /// </summary>
        /// TODO : 오류 남...처리 필요 (SDK v6문제인지 체크 필요)
        public void PluginFinishLoading()
        {
            if (this.Inst != null)
            {
                try
                {
                    this.Inst.FinishLoadingPlugins();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
                
            }
        }
        /// <summary>
        /// AddPluginFromFile
        /// </summary>
        /// <param name="path"></param>
        public void PluginAddFromFile(string path)
        {
            if (this.Inst != null && !path.IsNullOrWhiteSpaceEx() && File.Exists(path))
            {
                string fullPath = System.IO.Path.GetFullPath(path);
                string fileName = HxFile.GetFileNameWithOutExt(fullPath);
                switch (fileName)
                {
                    case "DropBox":
                    case "GoogleDrive":
                    case "SharePoint":
                    case "BoxFileSys":
                    case "OneDrive":
                        break;
                    default:
                        try
                        {
                            this.Inst.AddPluginFromFile(fullPath);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            //throw;
                        }
                        
                        break;
                }
            }
        }
        /// <summary>
        /// AddPluginFromDirectory
        /// </summary>
        /// <param name="path"></param>
        public void PluginAddFromDir(string path)
        {
            if (this.Inst != null && !path.IsNullOrWhiteSpaceEx() && Directory.Exists(path))
            {
                string fullPath = System.IO.Path.GetFullPath(path);
                string[] fileList = Directory.GetFiles(fullPath, "*.pvp");
                if (fileList.Length > 0)
                {
                    //this.PluginStartLoading();
                    try
                    {
                        foreach (string fFullName in fileList)
                        {
                            this.PluginAddFromFile(fFullName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message.ToString());
                        throw ex;
                    }
                    finally
                    {
                        //this.PluginFinishLoading();
                    }
                }
            }
        }

        public void PluginLoadFromDir(string path, bool bPluginStartLoading = true, bool bPluginFinishLoading = true)
        {
            if(bPluginStartLoading == true)
            {
                this.PluginStartLoading();
            }
            try
            {
                this.PluginAddFromDir(path);
            }
            finally
            {
                if (bPluginFinishLoading == true)
                {
                    this.PluginFinishLoading();
                }
            }
            
            
        }
    }
}
