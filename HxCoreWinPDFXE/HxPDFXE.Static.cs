using AxPDFXEdit;
using PDFXEdit;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HxCore.Win.PDFXE
{
    partial class HxPDFXE
    {
        #region Global / Registry / History Static Methods
        //public const string _REGISTRY_APP_ROOT_PATH_ = "Software\\Hi-Tech Engineering\\PDFXEditor\\";
        public static string GetOptStr(string rootPath, string valName, string keyName = "", string defVal = "")
        {
            //string path = "Software\\Tracker Software\\PDFEditorSDKExamples";
            string path = rootPath;
            if (keyName != "")
            {
                path += "\\";
                path += keyName;
            }
            string res = "";
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(path);
                RegistryValueKind vk = rk.GetValueKind(valName);
                if (vk == RegistryValueKind.String)
                {
                    res = (string)rk.GetValue(valName, defVal);
                }
                else if (vk == RegistryValueKind.MultiString || vk == RegistryValueKind.MultiString)
                {
                    string[] str_arr = (string[])rk.GetValue(valName, defVal); // type is REG_MULTI_SZ
                    foreach (string s in str_arr)
                    {
                        if (res.Length != 0)
                            res += "\r\n";
                        res += s;
                    }
                }
            }
            catch
            {
                res = "";
            }

            if (res.Length == 0)
                res = defVal;

            return res;
        }
        public static int GetOptInt(string rootPath, string valName, string keyName = "", int defVal = 0)
        {
            //string path = "Software\\Tracker Software\\PDFEditorSDKExamples";
            string path = rootPath;
            if (keyName != "")
            {
                path += "\\";
                path += keyName;
            }
            int res = defVal;
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(path);
                res = (int)rk.GetValue(valName, defVal);
            }
            catch { }
            return res;
        }

        public static void SetOptStr(string rootPath, string valName, string val, string keyName = "")
        {
            //string path = "Software\\Tracker Software\\PDFEditorSDKExamples";
            string path = rootPath;
            if (keyName != "")
            {
                path += "\\";
                path += keyName;
            }
            try
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey(path);
                rk.SetValue(valName, val);
            }
            catch { }
        }
        public static void SetOptInt(string rootPath, string valName, int val, string keyName = "")
        {
            //string path = "Software\\Tracker Software\\PDFEditorSDKExamples";
            string path = rootPath;
            if (keyName != "")
            {
                path += "\\";
                path += keyName;
            }
            try
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey(path);
                rk.SetValue(valName, val);
            }
            catch { }
        }

        public static void SetOptBool(string rootPath, string valName, bool val, string keyName = "")
        {
            //string path = "Software\\Tracker Software\\PDFEditorSDKExamples";
            string path = rootPath;
            if (keyName != "")
            {
                path += "\\";
                path += keyName;
            }
            try
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey(path);
                int v = val ? 1 : 0;
                rk.SetValue(valName, v);
            }
            catch { }
        }

        private static void BuildHistFilesNames(string histDir, out string histFile, out string histThumbsFile)
        {
            histFile = "";
            histThumbsFile = "";
            if (histDir.Length == 0)
                return;
            histFile = histDir;
            if (histFile[histFile.Length - 1] != '\\')
                histFile += '\\';
            histThumbsFile = histFile;
            histFile += "History.dat";
            histThumbsFile += "HistoryThumbs.dat";
        }

        public static bool HasIntersect(ref PDFXEdit.PXC_Rect r1, ref PDFXEdit.PXC_Rect r2)
        {
            if ((r1.left >= r2.right) || (r1.right <= r2.left))
                return false;
            if ((r1.top <= r2.bottom) || (r1.bottom >= r2.top))
                return false;
            return true;
        }


        public static string FixupWord(string w)
        {
            if (w == "Doc")
                return "Document";
            else if (w == "Docs")
                return "Documents";
            else if (w == "Annot")
                return "Annotation";
            else if (w == "Annots")
                return "Annotations";
            return w;
        }

        public static string SID2DispName(string id)
        {
            int last = id.LastIndexOf(".");
            if (last < 0)
                last = id.LastIndexOf("_");
            string t;
            if (last >= 0)
                t = id.Substring(last + 1);
            else
                t = id;
            if (t.Length == 0)
                return id;
            t = Char.ToUpper(t[0]) + t.Substring(1); // first char uppercase
            string res = "";
            int k = 0;
            int i = 1; // skip first char
            for (; i < t.Length; i++)
            {
                if ((t[i] - Char.ToUpper(t[i])) == 0) // big letter == new word
                {
                    string w = FixupWord(t.Substring(k, i - k));
                    res += w;
                    k = i;
                    res += ' ';
                }
            }
            if (k < i)
                res += FixupWord(t.Substring(k));
            return res;
        }

        public static string clr2str(Color c)
        {
            return "rgbd(" + c.R + "," + c.G + "," + c.B + ")";
        }
        public static Color rgb2clr(int c)
        {
            int r = c & 0x000000FF;
            int g = (c & 0x0000FF00) >> 8;
            int b = (c & 0x00FF0000) >> 16;
            return Color.FromArgb(r, g, b);
        }
        #endregion

        #region Doc. Permmissions
        public static bool IsRequestPermission(AxPXV_Control pAxPDFCtl, string openFullPath, PXC_RequestObj nReqObj, PXC_RequestOper nReqOper, IntPtr pAuthData = default, bool bDocClose = true)
        {
            bool Result = false;
            if (pAxPDFCtl != null && openFullPath.IsNullOrWhiteSpaceEx() != true)
            {
                try
                {
                    pAxPDFCtl.OpenDocFromPath(openFullPath);
                    PDFXEdit.PXV_Inst m_Inst = pAxPDFCtl.Inst;
                    PDFXEdit.IPXV_Document m_Doc = pAxPDFCtl.Doc;
                    Result = IsRequestPermission(m_Doc, nReqObj, nReqOper, pAuthData);
                    if(bDocClose == true)
                    {
                        m_Doc.Close();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return Result;
        }

        public static bool IsRequestPermission(PDFXEdit.IPXV_Document doc, PXC_RequestObj nReqObj, PXC_RequestOper nReqOper, IntPtr pAuthData = default)
        {
            bool Result = false;
            if(doc != null && doc.CoreDoc != null)
            {
                try
                {
                    PXC_PermStatus responseStatus = doc.CoreDoc.RequestPermission(nReqObj, nReqOper, pAuthData);
                    switch (responseStatus)
                    {
                        case PXC_PermStatus.Perm_ReqGranted:
                            Result = true;
                            break;
                        default:
                            Result = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                
            }
            return Result;
        }


        public static bool IsAcquireOwnerPermissions(PDFXEdit.IPXV_Document doc, PXC_RequestObj nReqObj, PXC_RequestOper nReqOper, IntPtr pAuthData = default, IPXC_DocAuthCallback pAuthCallback = null)
        {
            bool Result = false;
            if (doc != null && doc.CoreDoc != null)
            {
                var responsePasswordAuth = doc.CoreDoc.AcquireOwnerPermissions(pAuthCallback);
                if (responsePasswordAuth == PXC_PermStatus.Perm_ReqGranted)
                {
                    Result = doc.CoreDoc.RequestPermission(nReqObj, nReqOper, pAuthData) == PXC_PermStatus.Perm_ReqGranted ? true : false;
                }
                //doc.CoreDoc.SetDefaultFontPolicy(PXC_FontPolicy.FontPolicy_Auto, PXC_FontPolicy.FontPolicy_Auto);
                //var a = doc.CoreDoc.EnumFonts()
                doc.CoreDoc.SetFontPolicy("맑은 고딕", PXC_FontPolicy.FontPolicy_Auto, PXC_FontPolicy.FontPolicy_Auto);
            }
            return Result;
        }
        #endregion

        public static int FindStampCollectionByName(IPXC_StampsManager pStampManager, string pFindName, bool pThrowException = true)
        {
            int Result = int.MinValue;

            if (pStampManager == null || pStampManager.Count <= 0 || pFindName.IsNullOrWhiteSpaceEx() == true) return Result;

            try
            {
                System.Collections.IEnumerator list = pStampManager.GetEnumerator();
                if (list == null) return Result;

                string strFindStampCollectionID = null;
                while (list.MoveNext())
                {
                    IPXC_StampsCollection curr = (IPXC_StampsCollection)list.Current;
                    if (curr.Name == pFindName)
                    {
                        strFindStampCollectionID = curr.ID;
                    }
                }

                //Result = pStampManager.FindCollection(strFindStampCollectionID);
                Result = FindStampCollectionByID(pStampManager, strFindStampCollectionID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (pThrowException == true) throw ex;
            }
            return Result;
        }

        public static int FindStampCollectionByID(IPXC_StampsManager pStampManager, string pFindID, bool pThrowException = true)
        {
            int Result = int.MinValue;

            if (pStampManager == null || pStampManager.Count <= 0 || pFindID.IsNullOrWhiteSpaceEx() == true) return Result;
            try
            {
                Result = pStampManager.FindCollection(pFindID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (pThrowException == true) throw ex;
            }
            return Result;
        }

        public static int? RemoveStampCollectionByName(IPXC_StampsManager pStampManager, string pFindName, bool pThrowException = true)
        {
            int? Result = null;
            if (pStampManager == null || pStampManager.Count <= 0 || pFindName.IsNullOrWhiteSpaceEx() == true) return Result;
            try
            {
                //Result = pStampManager.FindCollection(strFindStampCollectionID);
                int iStampCollection = FindStampCollectionByName(pStampManager, pFindName);
                Result = RemoveStampCollection(pStampManager, iStampCollection, pThrowException);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (pThrowException == true) throw ex;
            }
            return Result;
        }
        public static int? RemoveStampCollectionByID(IPXC_StampsManager pStampManager, string pFindID, bool pThrowException = true)
        {
            int? Result = null;
            if (pStampManager == null || pStampManager.Count <= 0 || pFindID.IsNullOrWhiteSpaceEx() == true) return Result;
            try
            {
                //Result = pStampManager.FindCollection(strFindStampCollectionID);
                int iStampCollection = FindStampCollectionByID(pStampManager, pFindID);
                Result = RemoveStampCollection(pStampManager, iStampCollection, pThrowException);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (pThrowException == true) throw ex;
            }
            return Result;
        }

        public static int? RemoveStampCollection(IPXC_StampsManager pStampManager, int iStampCollection, bool pThrowException = true)
        {
            int? Result = null;
            try
            {
                IPXC_StampsCollection sc = null;
                if (pStampManager.Count > 0 && pStampManager.Count > iStampCollection && iStampCollection >= 0)
                {
                    sc = pStampManager[(uint)iStampCollection];
                    if (sc != null)
                    {
                        if (sc.Count > 0)
                        {
                            uint n = sc.Count;
                            Result = 0;
                            for (uint i = n; i > 0; i--)
                            {
                                uint idx = i - 1;
                                if (idx >= 0 && idx != uint.MaxValue)
                                {
                                    sc.RemoveStamp(idx);
                                    Result++;
                                }
                            }
                        }
                        pStampManager.RemoveCollection((uint)iStampCollection);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Result != null && Result > 0) Result *= (-1);
                Debug.WriteLine(ex);
                if (pThrowException == true) throw ex;
            }
            return Result;
        }



    }
}
