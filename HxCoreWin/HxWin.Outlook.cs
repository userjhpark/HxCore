using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    partial class HxWin
    {
        public static string GetOutlookSignature(string signatureName = null, bool bMatcheNameAll = true)
        {
            //string strAppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            //Console.WriteLine(strAppDataDir);
            string Result = null;
            //DirectoryInfo diInfo = new DirectoryInfo(strAppDataDir);
            //if (diInfo.Exists)
            //{
            //
            //
            //    FileInfo[] fiSignature = diInfo.GetFiles("*.htm");
            //
            //    if (fiSignature.Length > 0)
            //    {
            //        StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
            //        Result = sr.ReadToEnd();
            //
            //        if (Result.IsNullOrWhiteSpaceEx() != true)
            //        {
            //            string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
            //            Result = Result.Replace(fileName + "_files/", strAppDataDir + "/" + fileName + "_files/");
            //        }
            //    }
            //}

            Dictionary<string, string> list = GetOutlookSignatureList();
            if(list != null && list.Count > 0)
            {
                IEnumerable<KeyValuePair<string, string>> qSignature = null;
                string strKey = signatureName.SplitEx(":").LastOrDefault().Trim();
                if (strKey.IsNullOrWhiteSpaceEx() != true) 
                {
                    qSignature = list.Where(r => r.Key == strKey);
                    if(qSignature != null && qSignature.Any() == true)
                    {
                        Result = qSignature.FirstOrDefault().Value;
                    }
                }

                if(bMatcheNameAll != true && (qSignature == null || qSignature.Any() != true))
                {
                    Result = list.FirstOrDefault().Value;
                }
            }
            return Result;
        }

        public static Dictionary<string, string> GetOutlookSignatureList()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            //Console.WriteLine(appDataDir);
            Dictionary<string, string> Result = null;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);

            if (diInfo.Exists)
            {
                System.Text.Encoding encodEucKr = HxCore.HxString.EuckrEncoding;

                Result = new Dictionary<string, string>();
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");
                if (fiSignature.Length > 0)
                {
                    for(int i = 0; i < fiSignature.Length; i++)
                    {
                        string strSignatureName = fiSignature[i].Name;
                        StreamReader sr = new StreamReader(fiSignature[i].FullName, encodEucKr);
                        string strSignature = sr.ReadToEnd();
                        if (Result.IsNullOrWhiteSpaceEx() != true)
                        {
                            string fileName = fiSignature[i].Name.Replace(fiSignature[0].Extension, string.Empty);
                            fileName = HxString.HtmlEncode(fileName);
                            string strOldDirPath = fileName + ".files/";
                            string strNewDirPath = (@"file://" + appDataDir + "/" + fileName + ".files/").Replace("\\", "/");
                            string strSignatureContent = strSignature.Replace("%20", " ").Replace(strOldDirPath, strNewDirPath);

                            Result.Add(strSignatureName, strSignatureContent);
                        }
                    }
                }
            }
            return Result;
        }

    }
}
