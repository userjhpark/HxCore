using AxPDFXEdit;
using Microsoft.Win32;
using PDFXEdit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HxCore;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace HxCore.Win.PDFXE
{
    partial class HxPDFXE
    {
        #region PDFXE Utils
        public IString CreateString(string S = "")
        {
            IString Result = null;
            if (this.AxPXVCtl != null && this.AxPXVCtl.Inst != null)
            {
                Result = this.AxPXVCtl.Inst.CreateString(S);
            }
            return Result;
        }

        public string DoRunJavascript(string jsString)
        {
            string Result;
            try
            {
                PDFXEdit.IString res = AxPXVCtl.Inst.CreateString();
                Result = DoJavascript(ref this.pdfCtl, jsString);
                {
                    //syncAnnotScan();
                    //pdfCtl.Inst.de
                    //*******PDFCtl?.Inst?.ExecuteJS(PDFCtl.Doc, jsString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                    //IPXV_JSValue jsResult = pdfCtl.Inst.ExecuteJSEx(pdfCtl.Doc.CoreDoc, jsString, PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null);
                    //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    //byte[] resbyte = encoding.GetBytes(res.Value);
                    //Result = Convert.ToBase64String(resbyte);
                    //Result = dnCrypt.Instance.base64_encode(res.Value);
                    //******Result = res.Value;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = string.Empty;
            }
            return Result;
        }

        #endregion

        public void SetRibbonModeChange()
        {
            if (this.AxPXVCtl == null || (this.AxPXVCtl != null && this.AxPXVCtl.Frame == null))
                return;
            AxPXVCtl.Inst.EnableRibbonUI(!AxPXVCtl.Frame.View.IsRibbonMode);
            
            //bool bClassic = !pdfCtl.Frame.View.IsRibbonMode;
        }


        public string AnnotTypeToStr(HxAnnotType annotType)
        {
            //https://sdkhelp.tracker-software.com/view/PXV:IPXS_Inst_StrToAtom
            //https://github.com/tracker-software/PDFEditorSDKExamples/blob/master/CSharp/FullDemo/MainFrm.cs
            string Result = null;
            switch (annotType)
            {
                case HxAnnotType.n3D:
                    Result = "3D";
                    break;
                default:
                    Result = annotType.ToString("G"); //https://docs.microsoft.com/ko-kr/dotnet/api/system.enum.tostring?view=netframework-4.8
                    break;
            }
            return Result;
        }

        public string GetAnnotTypeToStr(HxAnnotType annotType)
        {
            return AnnotTypeToStr(annotType);
        }

        public uint GetStrToAtom(HxAnnotType annotType)
        {
            uint Result = int.MaxValue;
            string strAnnotType = GetAnnotTypeToStr(annotType);
            if (strAnnotType.IsNullOrWhiteSpaceEx() != true)
            {
                //PDFXEdit.IPXS_Inst pSInt = (PDFXEdit.IPXS_Inst)this.pxsInst.GetExtension("PXS");
                PDFXEdit.IPXS_Inst pSInt = (PDFXEdit.IPXS_Inst)this._pxsInst;
                Result = pSInt.StrToAtom(strAnnotType);
            }
            return Result;
        }


        #region XFDF관련
        public string GetExportJsAsXFDFStr(string oldAuthor = null, string newAuthor = null, bool bAuthorNotMatcheRemove = false)
        {
            string Result = null;
            string jsQueryString, jsResult;

            AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

            jsQueryString = "syncAnnotScan();";
            jsResult = this.DoRunJavascript(jsQueryString);

            //jsQueryString = "exportAsXFDFStr(true, true, null, true, 1);";
            jsQueryString = "exportAsXFDFStr(true, true, true, true, 1);";
            jsResult = this.DoRunJavascript(jsQueryString);
            if (!jsResult.IsNullOrWhiteSpaceEx() && jsResult != "undefined")
            {
                Result = this.GetXFDFStrToAuthorReplace(jsResult, oldAuthor, newAuthor, bAuthorNotMatcheRemove);
            }
            return Result;
        }

        public string GetExportJsAsXFDFStr(string newAuthor, bool bLockedRemove = true)
        {
            string Result = null;
            string jsQueryString, jsResult;

            AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

            jsQueryString = "syncAnnotScan();";
            jsResult = this.DoRunJavascript(jsQueryString);

            //jsQueryString = "exportAsXFDFStr(true, true, null, true, 1);";
            jsQueryString = "exportAsXFDFStr(true, true, true, true, 1);";
            jsResult = this.DoRunJavascript(jsQueryString);
            if (!jsResult.IsNullOrWhiteSpaceEx() && jsResult != "undefined")
            {
                Result = this.GetXFDFStrToAuthorChange(jsResult, newAuthor, bLockedRemove);
            }
            return Result;
        }

        public bool SetImportOpAsAnnotsFile(string path)
        {
            if (path.IsNullOrWhiteSpaceEx() != true && File.Exists(path))
            {
                try
                {
                    var op = AxPXVCtl.Inst.CreateOp(AxPXVCtl.Inst.Str2ID("op.document.importCommentsAndFields"));

                    if (op == null)
                    {
                        return false;
                    }

                    //op.Params.Root["Input"].v = fsInst.DefaultFileSys.StringToName(path);

                    op.Params.Root["Input"].v = AxPXVCtl.Doc; // put target-document
                    op.Params.Root["Options.FileName"].v = path; // *.xfdf to
                    op.Do();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                    throw ex;
                }
            }
            return false;
        }

        public bool SetImportJsAsXFDFFile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists && file.Length > 0)
                {
                    string jsQueryString, jsResult;
                    AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");
                    jsQueryString = "syncAnnotScan();";
                    //jsResult = this.GetRunJavascript(jsQueryString);
                    string strLoadXfdfFile = "/" + path.Replace(@"\", "/").Replace(":/", "/");
                    jsQueryString = @"importAnXFDF(""" + strLoadXfdfFile + @""")";
                    jsResult = this.DoRunJavascript(jsQueryString);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                throw ex;
            }
            
            return false;
        }

        public bool SetImportJsAsFDFFile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists && file.Length > 0)
                {
                    string jsQueryString, jsResult;
                    AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");
                    jsQueryString = "syncAnnotScan();";
                    //jsResult = this.GetRunJavascript(jsQueryString);
                    string strLoadFile = "/" + path.Replace(@"\", "/").Replace(":/", "/");
                    jsQueryString = @"importAnFDF(""" + strLoadFile + @""")";
                    jsResult = this.DoRunJavascript(jsQueryString);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                throw ex;
            }

            return false;
        }

        public string SetCreateFileXFDF(string xmlString, string saveDirPath = null)
        {
            string Result = null;
            try
            {
                Result = CreateFileXFDF(xmlString, saveDirPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }

        public string SetCreateFileFDF(string inputString, string saveDirPath = null, bool bExistCreate = false)
        {
            string Result = null;
            try
            {                
                 Result = CreateFileFDF(inputString, saveDirPath, HxCryptType.MD5, bExistCreate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }

        public string SetCreateFileFDF(byte[] inputByte, string saveDirPath = null, bool bExistCreate = false)
        {
            string Result;
            try
            {
                Result = CreateFileFDF(inputByte, saveDirPath, HxCryptType.MD5, bExistCreate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public string GetExportJsAsXFDFLoad(string saveFullName = null)
        {
            string Result = null;
            string jsQueryString, jsResult;
            try
            {
                AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

                //jsQueryString = "syncAnnotScan();";
                //jsResult = this.DoRunJavascript(jsQueryString);

                //jsQueryString = "exportAsFDFStr(true, true, null, true, 1);";
                //jsQueryString = "this.exportAsFDFStr({bAllFields: 'true', bFlags: 'true', bAnnotations: 'true'});";
                if (saveFullName.IsNullOrWhiteSpaceEx() == true)
                {
                    saveFullName = Path.Combine(System.Environment.GetEnvironmentVariable("TEMP"), "ExportXFDF_" + HxCrypt.RandPass() + ".xfdf");
                }
                saveFullName = HxFile.GetFileUniquePath(saveFullName, HxFileOverwriteType.RenameSequence);
                string expPath = saveFullName;
                if (expPath.Contains("\\"))
                {
                    expPath = "/" + expPath.Replace("\\", "/").Replace(":/", "/");
                }
                jsQueryString = $@"this.syncAnnotScan();
var expPath = '{expPath}';
this.exportAsXFDF({{ bAnnotations: 'true', cPath: expPath}});";
                jsResult = this.DoRunJavascript(jsQueryString);
                if (jsResult.ToStringEx().Trim() != "undefined" && File.Exists(saveFullName))
                {
                    Result = saveFullName;
                    //HxFile.WriteAllBytes
                    //Result = jsResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }

            return Result;
        }
        public string SetImportAsXFDFStr(string xmlString, string saveDirPath = null)
        {
            string Result = null;
            try
            {
                if (xmlString.IsNullOrWhiteSpaceEx() != true)
                {

                    string saveFullName = SetCreateFileXFDF(xmlString, saveDirPath);
                    bool bResult = SetImportJsAsXFDFFile(saveFullName);
                    if (bResult)
                    {
                        Result = saveFullName;
                    }
                    //{
                    //    FileInfo file = new FileInfo(saveFullName);
                    //    if (file.Exists && file.Length > 0)
                    //    {

                    //        string jsQueryString, jsResult;

                    //        PXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

                    //        jsQueryString = "syncAnnotScan();";
                    //        //jsResult = this.GetRunJavascript(jsQueryString);

                    //        string strLoadXfdfFile = "/" + saveFullName.Replace(@"\", "/").Replace(":/", "/");
                    //        jsQueryString = @"importAnXFDF(""" + strLoadXfdfFile + @""")";
                    //        jsResult = this.DoRunJavascript(jsQueryString);

                    //        Result = file.FullName;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
                throw ex;
            }
            return Result;
        }

        public string SetImportAsFDFStr(string inputString, string saveDirPath = null, bool bExistCreate = true)
        {
            string Result = null;
            try
            {
                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    string saveFullName = SetCreateFileFDF(inputString, saveDirPath, bExistCreate);
                    bool bResult = SetImportJsAsFDFFile(saveFullName);
                    if (bResult)
                    {
                        Result = saveFullName;
                    }
                    //{
                    //    FileInfo file = new FileInfo(saveFullName);
                    //    if (file.Exists && file.Length > 0)
                    //    {

                    //        string jsQueryString, jsResult;

                    //        PXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

                    //        jsQueryString = "syncAnnotScan();";
                    //        //jsResult = this.GetRunJavascript(jsQueryString);

                    //        string strLoadXfdfFile = "/" + saveFullName.Replace(@"\", "/").Replace(":/", "/");
                    //        jsQueryString = @"importAnXFDF(""" + strLoadXfdfFile + @""")";
                    //        jsResult = this.DoRunJavascript(jsQueryString);

                    //        Result = file.FullName;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
                throw ex;
            }
            return Result;
        }

        public string SetImportAsFDFData(byte[] inputByte, string saveDirPath = null, bool bExistCreate = true)
        {
            string Result = null;
            try
            {
                if (inputByte != null && inputByte.ToStringEx().IsNullOrWhiteSpaceEx() != true)
                {
                    string saveFullName = SetCreateFileFDF(inputByte, saveDirPath, bExistCreate);
                    bool bResult = SetImportJsAsFDFFile(saveFullName);
                    if (bResult)
                    {
                        Result = saveFullName;
                    }
                    //{
                    //    FileInfo file = new FileInfo(saveFullName);
                    //    if (file.Exists && file.Length > 0)
                    //    {

                    //        string jsQueryString, jsResult;

                    //        PXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

                    //        jsQueryString = "syncAnnotScan();";
                    //        //jsResult = this.GetRunJavascript(jsQueryString);

                    //        string strLoadXfdfFile = "/" + saveFullName.Replace(@"\", "/").Replace(":/", "/");
                    //        jsQueryString = @"importAnXFDF(""" + strLoadXfdfFile + @""")";
                    //        jsResult = this.DoRunJavascript(jsQueryString);

                    //        Result = file.FullName;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
                throw ex;
            }
            return Result;
        }
        public string SetImportAsFDFBase64(string inputString, string saveDirPath = null, bool bExistCreate = true, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            string Result = null;
            try
            {
                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    //string decodeStr = HxString.GetStringFromBase64Decode(inputString, HxEncodingType.Unicode);
                    //byte[] bytes = HxString.GetString2Bytes(decodeStr, HxEncodingType.UTF8);
                    byte[] bytes = HxString.GetBytesFromBase64Decode(inputString);
                    string saveFullName = SetCreateFileFDF(bytes, saveDirPath, bExistCreate);
                    bool bResult = SetImportJsAsFDFFile(saveFullName);
                    if (bResult)
                    {
                        Result = saveFullName;
                    }
                    //{
                    //    FileInfo file = new FileInfo(saveFullName);
                    //    if (file.Exists && file.Length > 0)
                    //    {

                    //        string jsQueryString, jsResult;

                    //        PXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

                    //        jsQueryString = "syncAnnotScan();";
                    //        //jsResult = this.GetRunJavascript(jsQueryString);

                    //        string strLoadXfdfFile = "/" + saveFullName.Replace(@"\", "/").Replace(":/", "/");
                    //        jsQueryString = @"importAnXFDF(""" + strLoadXfdfFile + @""")";
                    //        jsResult = this.DoRunJavascript(jsQueryString);

                    //        Result = file.FullName;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
                throw ex;
            }
            return Result;
        }

        public string GetExportJsAsFDFStr()
        {
            string Result = null;
            string jsQueryString, jsResult;

            AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

            jsQueryString = "syncAnnotScan();";
            jsResult = this.DoRunJavascript(jsQueryString);

            //jsQueryString = "exportAsFDFStr(true, true, null, true, 1);";
            jsQueryString = "this.exportAsFDFStr({bAllFields: 'true', bFlags: 'true', bAnnotations: 'true'});";
            jsResult = this.DoRunJavascript(jsQueryString);
            if (!jsResult.IsNullOrWhiteSpaceEx() && jsResult != "undefined")
            {
                Result = jsResult;
            }
            return Result;
        }

        public string GetExportJsAsFDFLoad(string saveFullName = null)
        {
            string Result = null;
            string jsQueryString, jsResult;
            try
            {
                AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

                //jsQueryString = "syncAnnotScan();";
                //jsResult = this.DoRunJavascript(jsQueryString);

                //jsQueryString = "exportAsFDFStr(true, true, null, true, 1);";
                //jsQueryString = "this.exportAsFDFStr({bAllFields: 'true', bFlags: 'true', bAnnotations: 'true'});";
                if(saveFullName.IsNullOrWhiteSpaceEx() == true)
                {
                    saveFullName = Path.Combine(System.Environment.GetEnvironmentVariable("TEMP"), "ExportFDF_" + HxCrypt.RandPass() + ".fdf");
                }
                saveFullName = HxFile.GetFileUniquePath(saveFullName, HxFileOverwriteType.RenameSequence);
                string fdfPath = saveFullName;
                if (fdfPath.Contains("\\"))
                {
                    fdfPath = "/" + fdfPath.Replace("\\", "/").Replace(":/", "/");
                }
                jsQueryString = $@"this.syncAnnotScan();
var fdfPath = '{fdfPath}';
this.exportAsFDF({{ bAnnotations: 'true', cPath: fdfPath}});";
                jsResult = this.DoRunJavascript(jsQueryString);
                if (jsResult.ToStringEx().Trim() != "undefined" && File.Exists(saveFullName))
                {
                    Result = saveFullName;
                    //HxFile.WriteAllBytes
                    //Result = jsResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            
            return Result;
        }

        public string GetExportJsAsFDFStr(out bool IsConvert, HxCryptType cryptType = HxCryptType.Base64, string cryptKey = null)
        {
            string Result = GetExportJsAsFDFStr();
            IsConvert = false;
            if (!Result.IsNullOrWhiteSpaceEx() && Result != "undefined")
            {
                try
                {
                    switch (cryptType)
                    {
                        case HxCryptType.Base64:
                            Result = HxCrypt.base64_encode(Result, HxEncodingType.ASCII);
                            IsConvert = true;
                            break;
                        case HxCryptType.MD5:
                        case HxCryptType.ExportMD5:
                            Result = HxCrypt.Md5(Result);
                            IsConvert = true;
                            break;
                        case HxCryptType.Crypt:
                        case HxCryptType.ExportCrypt:
                            Result = HxCrypt.Encrypt(Result, cryptKey);
                            IsConvert = true;
                            break;
                        case HxCryptType.None:
                        default:
                            IsConvert = false;
                            break;
                    }
                }
                catch (Exception e)
                {
                    IsConvert = false;
                    Debug.WriteLine(e);
                    //throw e;
                }
                
            }
            return Result;
        }

        public Dictionary<string, HxAnnotsRec> GetAnnotsRecordList(string author = null)
        {
            Dictionary<string, HxAnnotsRec> Result = null;
            //string jsQueryString, jsResult;
            //jsQueryString = "exportAsXFDFStr(true, true, null, true, 1);";
            //jsResult = this.ExecutePDFXRunJavascript(jsQueryString);
            ////Result = jsResult;
            //Result = this.GetPDFXAnnotsXml(jsResult);
            string jsResult = this.GetExportJsAsXFDFStr();
            Result = this.GetXFDFStrToAnnotsRecordList(jsResult, author);
            return Result;
        }

        public Dictionary<string, HxAnnotsRec> GetXFDFStrToAnnotsRecordList(string xmlString, string author = null)
        {
            Dictionary<string, HxAnnotsRec> Result = null;
            Result = HxUtils.XFDFStrToAnnotsRecordList(xmlString, author);
            return Result;
        }

        public Dictionary<string, HxAnnotsRec> GetXFDFStrToAnnotsLockList(string xmlString, bool bLockAnnots = true)
        {
            Dictionary<string, HxAnnotsRec> Result = null;
            if (!xmlString.IsNullOrWhiteSpaceEx() && xmlString != "undefined")
            {
                Result = new Dictionary<string, HxAnnotsRec>();

                XDocument doc = XDocument.Load(new StringReader(xmlString));
                var elements = doc.Root.Element(XName.Get("annots", "http://ns.adobe.com/xfdf/"))
                                .Elements()
                                .OrderBy(e => int.Parse(e.Attribute("page").Value));
                ;
                StringBuilder builder = new StringBuilder();
                foreach (XElement element in elements)
                {
                    string S = element.ToString();
                    //string author = string.Format("[{0}]{1}/{2}/{3}", SysEnv.Core.LoginID, SysEnv.Core.LoginName, SysEnv.Core.LoginDutyName, SysEnv.Core.LoginDeptName);
                    HxAnnotsRec annotInfo = new HxAnnotsRec(element);
                    if(annotInfo.FlagsLocked == bLockAnnots)
                        Result.Add(annotInfo.Name, annotInfo);
                }
            }
            return Result;
        }
        public Dictionary<string, HxAnnotsRec> GetXFDFStrToAnnotsUnLockList(string xmlString)
        {
            return GetXFDFStrToAnnotsLockList(xmlString, false);
        }
        private string GetXFDFStrToAuthorReplace(string xmlString, string oldAuthor, string newAuthor, bool bAuthorNotMatchedRemove = true)
        {
            string Result = null;
            if (!xmlString.IsNullOrWhiteSpaceEx() && xmlString != "undefined")
            {
                try
                {
                    if (oldAuthor.IsNullOrWhiteSpaceEx() || newAuthor.IsNullOrWhiteSpaceEx())
                    {
                        Result = xmlString;
                    }
                    else
                    {
                        
                        XDocument doc = XDocument.Load(new StringReader(xmlString), LoadOptions.PreserveWhitespace);
                        doc.Declaration = new XDeclaration("1.0", "utf-8", null);
                        var elements = doc.Root.Element(XName.Get("annots", "http://ns.adobe.com/xfdf/"))
                                    .Elements()
                                    .OrderBy(e => int.Parse(e.Attribute("page").Value));
                        foreach (XElement element in elements)
                        {
                            string S = element.ToString();
                            //string author = string.Format("[{0}]{1}/{2}/{3}", SysEnv.Core.LoginID, SysEnv.Core.LoginName, SysEnv.Core.LoginDutyName, SysEnv.Core.LoginDeptName);
                            string xmlAuthor = element.Attribute("title").Value;
                            if(xmlAuthor.ToLower().Trim() == oldAuthor.ToLower().Trim())
                            {
                                element.Attribute("title").Value = newAuthor;
                                xmlAuthor = element.Attribute("title").Value;
                            }
                            //builder.AppendFormat("<p>{0}</p>", element.Attribute("subject").Value);
                            //string funcCD = null;
                            //HxString.RegexReplace();

                            if(bAuthorNotMatchedRemove == true && xmlAuthor != newAuthor)
                            {
                                element.Remove();
                            }
                            
                        }
                        Result = doc.ToStringEx();
                        try
                        {
                            StringBuilder builder = new StringBuilder();
                            using (StringWriter writer = new HxUTF8StringWriter(builder))
                            {
                                
                                doc.Save(writer, SaveOptions.DisableFormatting);
                            }
                            Result = builder.ToStringEx();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            //throw;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            return Result;
        }

        private string GetXFDFStrToAuthorChange(string xmlString, string newAuthorTitle, bool bLockedRemove = true)
        {
            string Result = null;
            if (!xmlString.IsNullOrWhiteSpaceEx() && xmlString != "undefined")
            {
                try
                {
                    if (newAuthorTitle.IsNullOrWhiteSpaceEx())
                    {
                        Result = xmlString;
                    }
                    else
                    {

                        XDocument doc = XDocument.Load(new StringReader(xmlString), LoadOptions.PreserveWhitespace);
                        doc.Declaration = new XDeclaration("1.0", "utf-8", null);
                        var elements = doc.Root.Element(XName.Get("annots", "http://ns.adobe.com/xfdf/"))
                                    .Elements()
                                    .OrderBy(e => int.Parse(e.Attribute("page").Value));
                        foreach (XElement element in elements)
                        {
                            string strAnntoElement = element.ToString();
                            //string author = string.Format("[{0}]{1}/{2}/{3}", SysEnv.Core.LoginID, SysEnv.Core.LoginName, SysEnv.Core.LoginDutyName, SysEnv.Core.LoginDeptName);
                            
                            string strAnnotFlags = element.Attribute("flags").Value;
                            bool bAnntoLocked = false;
                            if (strAnnotFlags.IsNullOrWhiteSpaceEx() != true)
                            {
                                string[] flagArray = strAnnotFlags.SplitEx(",");

                                if (flagArray != null && flagArray.Length > 0)
                                {
                                    foreach(string flag in flagArray)
                                    {
                                        switch (flag.Trim().ToLower())
                                        {
                                            case "locked":
                                                bAnntoLocked = true;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            string strAnnotAuthorTitle = element.Attribute("title").Value;
                            if (bAnntoLocked != true)
                            {
                                //string strAnnotAuthorTitle = element.Attribute("title").Value;
                                element.Attribute("title").Value = newAuthorTitle;
                                strAnnotAuthorTitle = element.Attribute("title").Value;
                            }
                            //builder.AppendFormat("<p>{0}</p>", element.Attribute("subject").Value);
                            //HxString.RegexReplace();
                            if (bLockedRemove == true && bAnntoLocked == true) // && strAnnotAuthorTitle != newAuthorTitle
                            {
                                element.Remove();
                            }

                        }
                        Result = doc.ToStringEx();
                        try
                        {
                            StringBuilder builder = new StringBuilder();
                            using (StringWriter writer = new HxUTF8StringWriter(builder))
                            {

                                doc.Save(writer, SaveOptions.DisableFormatting);
                            }
                            Result = builder.ToStringEx();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            //throw;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            return Result;
        }

        private int SetJsSyncAnnotScan()
        {
            int Result = -1;
            string jsQueryString, jsResult;

            try
            {
                AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");

                jsQueryString = "syncAnnotScan();";
                jsResult = this.DoRunJavascript(jsQueryString);
            }
            catch (Exception)
            {

                throw;
            }
            

            return Result;
        }


        public bool SetAnnotValue(string inputName, string inputAttribute, string inputValue)
        {
            try
            {
                PDFXEdit.IPXV_Document Doc = AxPXVCtl.Doc;
                PDFXEdit.PXV_Inst Inst = AxPXVCtl.Inst;
                PDFXEdit.IPXV_AnnotsList annotsList = AxPXVCtl?.Inst?.CreateAnnotsList();
                for (uint i = 0; i < Doc.CoreDoc.Pages.Count; i++)
                {
                    PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                    uint nCnt = page.GetAnnotsCount();
                    for (uint j = 0; j < nCnt; j++)
                    {
                        PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);
                        //annot.Flags
                        string name = annot.Name;
                        if (name == inputName)
                        {
                            string jsLockString = @"
var obj = this.getAnnot(" + page.Number + @",'" + name + @"');
    if(obj != null){ 
        obj." + inputAttribute + @" = '" + inputValue + @"';
}";
                            //Debug.WriteLine(annot.Flags);
                            this.DoRunJavascript(jsLockString);
                            //Debug.WriteLine(annot.Flags);
                            //Debug.WriteLine(annot.Actions.);
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return false;
        }
        /// <summary>
        /// 코멘트(Comment/Annots/Markup) 삭제
        /// </summary>
        /// <param name="bLockedOnlyRemove">코멘트 삭제 옵션(True : 전체 삭제, False : Lock된 것만)</param>
        public virtual void SetAnnotsRemoveAll(bool bLockedOnlyRemove = false)
        {
            string jsQueryString, jsResult;

            try
            {
                AxPXVCtl.Inst.ExecUICmd("cmd.edit.deselect");
                if (bLockedOnlyRemove == true)
                {
                    jsQueryString = @"this.syncAnnotScan();
var annots = this.getAnnots();
if (annots!=null) {
    for (var i=annots.length-1; i>=0; i--) {
        if (annots[i].lock == true)
            annots[i].destroy();
    }
}
                ";
                }
                else
                {
                    jsQueryString = @"this.syncAnnotScan();
var annots = this.getAnnots();
if (annots!=null) {
    for (var i=annots.length-1; i>=0; i--) {
        annots[i].destroy();
    }
}
                ";
                }
                jsResult = this.DoRunJavascript(jsQueryString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
        }

        //MARKUP/Annots를 잠금(Locked) 모드 변경
        public virtual void SetAnnotsLockAllChange(bool bLock = true)
        {
            string strLock = null;
            if (bLock == true)
            {
                strLock = "true";
            }
            else if (bLock == false)
            {
                strLock = "false";
            }
            //PDFXEdit.IPXC_AnnotsList annotsList = pdfCtl?.Inst?.CreateAnnotsList();
            PDFXEdit.IPXV_Document Doc = AxPXVCtl.Doc;
            PDFXEdit.PXV_Inst Inst = AxPXVCtl.Inst;
            PDFXEdit.IPXV_AnnotsList annotsList = AxPXVCtl?.Inst?.CreateAnnotsList();
            for (uint i = 0; i < Doc.CoreDoc.Pages.Count; i++)
            {
                PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                uint nCnt = page.GetAnnotsCount();
                for (uint j = 0; j < nCnt; j++)
                {
                    PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);
                    //annot.Flags
                    string name = annot.Name;

                    string jsLockString = @"
var obj = this.getAnnot(" + page.Number + @",'" + name + @"');
    if(obj != null){ 
        obj.lock = " + strLock + @";
}";
                    //Debug.WriteLine(annot.Flags);
                    this.DoRunJavascript(jsLockString);
                    //Debug.WriteLine(annot.Flags);
                    //Debug.WriteLine(annot.Actions.);

                }
            }
            /*
            PDFXEdit.IPXV_Document Doc = pdfCtl.Doc;
            PDFXEdit.PXV_Inst Inst = pdfCtl.Inst;
            //PDFXEdit.IOperation Op = Inst.CreateOp
            PDFXEdit.IPXV_AnnotsList annotsList = Inst.CreateAnnotsList();
            for (uint i = 0; i < Doc.CoreDoc.Pages.Count; i++)
            {
                PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                uint nCnt = page.GetAnnotsCount();
                for (uint j = 0; j < nCnt; j++)
                {
                    PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);

                    //Debug.WriteLine(annot.Actions.);

                }
            }

            //annotsList.
            */
        }
                

        public virtual void SetFlattenAllAnnotations(PDFXEdit.IPXV_Document Doc, PDFXEdit.PXV_Inst Inst, string pageRange = null)
        {
            //https://sdkhelp.tracker-software.com/view/PXV:op_annots_flatten
            try
            {
                int nID = Inst.Str2ID("op.annots.flatten", false);
                PDFXEdit.IOperation Op = Inst.CreateOp(nID);
                PDFXEdit.ICabNode input = Op.Params.Root["Input"];
                // If we add document as an input - it means that we'll flatten all of the annotations
                input.Add().v = Doc;
                PDFXEdit.ICabNode options = Op.Params.Root["Options"];
                // Going through first 3 pages
                if (pageRange.IsNullOrWhiteSpaceEx() != true)
                {
                    options["PagesRange.Type"].v = "Exact";
                    options["PagesRange.Text"].v = pageRange;
                }
                else
                {
                    uint pagesCnt = Doc.CoreDoc.Pages.Count;
                    for (uint i = 0; i < pagesCnt; i++)
                    {
                        PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                        uint annotsCnt = page.GetAnnotsCount();
                        for (uint j = 0; j < annotsCnt; j++)
                        {
                            PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);
                            //In our case it's square annotations
                            input.Add().v = annot;
                        }
                    }
                }
                // Flattening non-printable annotations
                options["NonPrintableAction"].v = "Flatten";
                // Ignore form fields
                options["FieldsAction"].v = "LeftAsIs";

                Op.Do();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void SetFlattenCustomAnnotations(PDFXEdit.IPXV_Document Doc = null, PDFXEdit.PXV_Inst Inst = null, bool bConvertFileAttachment = false, bool bConvertTextPopup = false, bool bConvertMedia = false, bool bConvert3D = false)
        {
            try
            {
                //https://sdkhelp.tracker-software.com/view/PXV:op_annots_flatten
                if (Doc == null)
                    Doc = this.Doc;
                if (Inst == null)
                    Inst = this.Inst;
                if(Doc != null && Inst != null)
                {
                    PDFXEdit.IPXS_Inst pSInt = (PDFXEdit.IPXS_Inst)Inst.GetExtension("PXS");
                    //uint nSquareAtom = pSInt.StrToAtom("Square");
                    //string s = AnnotTypeToStr(HxAnnotType.FileAttachment);
                    uint nFileAttachment = GetStrToAtom(HxAnnotType.FileAttachment);
                    uint nSound = GetStrToAtom(HxAnnotType.Sound);
                    uint nMovie = GetStrToAtom(HxAnnotType.Movie);
                    uint nTextPopup = GetStrToAtom(HxAnnotType.Text);
                    uint n3D = GetStrToAtom(HxAnnotType.n3D);
                    int nID = Inst.Str2ID("op.annots.flatten", false);
                    PDFXEdit.IOperation Op = Inst.CreateOp(nID);
                    PDFXEdit.ICabNode input = Op.Params.Root["Input"];
                    // Filling the operation with the annotations that need to be flatten
                    uint pagesCnt = Doc.CoreDoc.Pages.Count;
                    bool bWorked = false;
                    for (uint i = 0; i < pagesCnt; i++)
                    {
                        PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                        uint annotsCnt = page.GetAnnotsCount();
                        for (uint j = 0; j < annotsCnt; j++)
                        {
                            PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);
                            //In our case it's square annotations

                            if (annot.Type != nFileAttachment && annot.Type != nSound && annot.Type != nMovie && annot.Type != nTextPopup && annot.Type != n3D)
                            {
                                input.Add().v = annot;
                                bWorked = true;
                            }
                            else
                            {
                                Debug.WriteLine(annot.Type);
                            }

                            //if (bConvertFileAttachment == true && annot.Type == nFileAttachment)
                            //{
                            //    input.Add().v = annot;
                            //    bWorked = true;
                            //}
                            //else if (bConvertTextPopup == true && annot.Type == nTextPopup)
                            //{
                            //    input.Add().v = annot;
                            //    bWorked = true;
                            //}
                            //else if (bConvertMedia == true && (annot.Type == nSound || annot.Type == nMovie))
                            //{
                            //    input.Add().v = annot;
                            //    bWorked = true;
                            //}
                            //else if (bConvert3D == true && annot.Type == n3D)
                            //{
                            //    input.Add().v = annot;
                            //    bWorked = true;
                            //}
                            //else
                            //{
                            //    input.Add().v = annot;
                            //    bWorked = true;
                            //}
                        }
                    }
                    if (bWorked == true)
                    {
                        PDFXEdit.ICabNode options = Op.Params.Root["Options"];
                        // Flattening non-printable annotations
                        options["NonPrintableAction"].v = "Flatten";
                        // Ignore form fields
                        options["FieldsAction"].v = "LeftAsIs";

                        Op.Do();

                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public void SetPageCustomSinglePageFitMode()
        {
            if (pdfCtl == null) return;
            IPXV_Document doc = pdfCtl.Doc;
            if (doc == null) return;
            PXV_Inst pxvInst = pdfCtl.Inst;
            if (pxvInst == null) return;
            IPXV_PagesLayoutManager activePageLayout = Inst.ActiveDocView.PagesView.Layout;
            if (activePageLayout == null) return;
            IPXC_Pages pages = doc.CoreDoc.Pages;
            if (pages == null) return;
            PDFXEdit.IPXC_Page page = Inst.ActiveDoc.CoreDoc.Pages[activePageLayout.CurrentPage];
            if (page == null) return;

            SetPageLayoutMode(PXC_PagesLayout.PageLayout_SinglePage);
            SetPageZoomMode(PXV_ZoomMode.PXV_ZoomMode_FitPage);
        }
        public void SetPageLayoutMode(PXC_PagesLayout pageLayout)
        {
            if (this.AxPXVCtl != null)
            {
                this.AxPXVCtl.PagesLayoutMode = pageLayout;
            }
        }
        public void SetPageZoomMode(PXV_ZoomMode zoomMode)
        {
            if (this.AxPXVCtl != null)
            {
                this.AxPXVCtl.Inst.ActivePagesView.SetZoom(PXV_ZoomMode.PXV_ZoomMode_FitPage, 0);
            }
        }

        [Obsolete("없애 버려!")]
        private Dictionary<string, HxAnnotsRec> GeXFDFStrToMarkupAnnotRecords(string xmlString, string author = "")
        
        {
            Dictionary<string, HxAnnotsRec> Result = null;
            if (!xmlString.IsNullOrWhiteSpaceEx() && xmlString != "undefined")
            {
                Result = new Dictionary<string, HxAnnotsRec>();

                XDocument doc = XDocument.Load(new StringReader(xmlString));
                var elements = doc.Root.Element(XName.Get("annots", "http://ns.adobe.com/xfdf/"))
                                .Elements()
                                .OrderBy(e => int.Parse(e.Attribute("page").Value));
                ;
                //Result.Add("ALL", new MarkupAnnotsRec(doc));
                /*
                var elements = doc.Root.Element(XName.Get("annots", "http://ns.adobe.com/xfdf/"))
                        .Elements(XName.Get("highlight", "http://ns.adobe.com/xfdf/"))
                        .OrderBy(e => int.Parse(e.Attribute("page").Value));
                */
                StringBuilder builder = new StringBuilder();
                foreach (XElement element in elements)
                {
                    string S = element.ToString();
                    //string author = string.Format("[{0}]{1}/{2}/{3}", SysEnv.Core.LoginID, SysEnv.Core.LoginName, SysEnv.Core.LoginDutyName, SysEnv.Core.LoginDeptName);
                    HxAnnotsRec annotInfo = new HxAnnotsRec(element, author);
                    Result.Add(annotInfo.Name, annotInfo);
                    //builder.AppendFormat("<p>{0}</p>", element.Attribute("subject").Value);
                }
                //Result += builder.ToStringEx();


                //jsQueryString = "exportAsXFDFStr(true, true, null, true, 1);";

                //PDFXEdit.IPXC_AnnotsList annotsList = pdfCtl.Inst.CreateAnnotsList();
                /*
                PDFXEdit.IPXV_Document Doc = pdfCtl.Doc;
                PDFXEdit.PXV_Inst Inst = pdfCtl.Inst;
                //PDFXEdit.IOperation Op = Inst.CreateOp
                PDFXEdit.IPXV_AnnotsList annotsList = Inst.CreateAnnotsList();
                for (uint i = 0; i < Doc.CoreDoc.Pages.Count; i++)
                {
                    PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                    uint nCnt = page.GetAnnotsCount();
                    for (uint j = 0; j < nCnt; j++)
                    {
                        PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);
                    
                        //Debug.WriteLine(annot.Actions.);
                    
                    }
                }
            
                //annotsList.
                */
            }
            return Result;
        }


        #endregion

        public void OpenDocFromPath(string path, bool isPDFAtoStandard = false)
        {
            try
            {
                if (!path.StartsWith("-") && !path.StartsWith("/"))
                {
                    string strFullName = HxFile.GetFileFullPath(path);
                    if (!strFullName.IsNullOrWhiteSpaceEx())
                    {
                        AxPXVCtl.OpenDocFromPath(strFullName);
                    }
                    else
                    {
                        AxPXVCtl.OpenDocFromPath(path);
                    }
                    /**
                    if (PDFCtl?.Doc?.ViewsCount > 0 && PDFCtl.Doc.ActiveView != null && PDFCtl.Doc.ActiveView?.PagesView != null)
                    {
                        PDFCtl.Doc.ActiveView.Panes.Layout.HighlightPane(PDFCtl.Doc.ActiveView.PagesView.Obj);
                        //or //PXVCtl.ShowPane("pagesView", true, true);
                        PDFCtl.Doc.ActiveView.PagesView.Obj.SetInputFocus(true);
                        ////PXVCtl.Doc.ActiveView.PagesView.Obj.SetInputFocus(true);
                        ////PXVCtl.Focus();
                    }
                    */
                    if (AxPXVCtl.Doc != null && isPDFAtoStandard == true)
                    {
                        bool isPdfaDocument = AxPXVCtl.Doc.CoreDoc.Props.PDFStandard != PXC_PDFStandard.PDFS_None;
                        pdfCtl.Doc.DiscardPDFACompilance();
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }

        }
        public void OpenDocFrom(string path, bool isPDFAtoStandard = false)
        {
            try
            {
                AxPXVCtl.OpenDocFrom(path);
                if (AxPXVCtl.Doc != null && isPDFAtoStandard == true)
                {
                    bool isPdfaDocument = AxPXVCtl.Doc.CoreDoc.Props.PDFStandard != PXC_PDFStandard.PDFS_None;
                    pdfCtl.Doc.DiscardPDFACompilance();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void ConvertToPdfa()
        {

        }

        public void Save(string fileName = null, int nFlags = 0, IProgressMon pProgress = null, IPXV_ExportConverter pDestConv = null, ICab pDestConvParams = null, IAFS_FileSys pDestFS = null, ICab pAdvancedParams = null, [ComAliasName("PDFXEdit.HANDLE_T")] uint hWndParent = 0)
        {
            try
            {
                if (fileName != null)
                {
                    PDFXEdit.IString destPath = fileName.IsNullOrWhiteSpaceEx() != true ? AxPXVCtl.Inst.CreateString(fileName) : null;
                    AxPXVCtl.Doc.Save(destPath, nFlags, pProgress, pDestConv, pDestConvParams, pDestFS, pAdvancedParams, hWndParent);
                }
                else
                {
                    pdfCtl.Doc.Save();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            
        }

        #region Static / Utils

        public static string ExecSaveAsWithAnnots(AxPXV_Control pAxPDFCtl, string openFullPath, List<string> annotsList = null, HxAnnotFormatType formatType = HxAnnotFormatType.XFDF, string saveFullPath = null)
        {
            //AxPDFXEdit.IPXC_Inst pdfCoreInst = (AxPDFXEdit.IPXC_Inst)pdfInst.GetExtension("PXC");
            string Result = null;
            if (pAxPDFCtl != null && openFullPath.IsNullOrWhiteSpaceEx() != true)
            {
                try
                {
                    pAxPDFCtl.OpenDocFromPath(openFullPath);
                    PDFXEdit.IPXV_Document m_Doc = pAxPDFCtl.Doc;
                    PDFXEdit.PXV_Inst m_Inst = pAxPDFCtl.Inst;
                    try
                    {
                        //if (annotsFullPath.IsNullOrWhiteSpaceEx() != true && File.Exists(annotsFullPath))
                        //{
                        //    var m_nID = m_Inst.Str2ID("op.document.importCommentsAndFields");
                        //    PDFXEdit.IOperation op = m_Inst.CreateOp(m_nID);
                        //    PDFXEdit.ICabNode input = op.Params.Root["Input"];
                        //    input.Add().v = Doc;
                        //    op.Params.Root["Options.FileName"].v = annotsFullPath; // *.xfdf to
                        //    op.Do();
                        //}
                        

                        if (m_Inst.DocCount > 0)
                        {
                            string saveDirPath = HxFile.GetFileDirPath(saveFullPath ?? openFullPath);
                            if (annotsList != null && annotsList.Count > 0)
                            {
                                foreach (string annots in annotsList)
                                {
                                    string path;
                                    switch (formatType)
                                    {
                                        case HxAnnotFormatType.FDF:
                                            path = CreateFileFDFFromBase64(annots, saveDirPath);
                                            break;
                                        case HxAnnotFormatType.XFDF:
                                        default:
                                            path = CreateFileXFDF(annots, saveDirPath);
                                            break;
                                    }

                                    FileInfo file = new FileInfo(path);
                                    if (file.Exists && file.Length > 0)
                                    {
                                        try
                                        {
                                            PDFXEdit.IString res = m_Inst.CreateString();
                                            string jsQueryString;
                                            //jsQueryString = "syncAnnotScan();";
                                            //m_Inst.ExecuteJS(m_Inst.Doc[0], jsQueryString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                                            string strLoadFilePath = ReplaceFilePath(path);
                                            switch (formatType)
                                            {
                                                case HxAnnotFormatType.FDF:
                                                    //strLoadFilePath = "/" + strLoadFilePath.Replace(@"\", "/").Replace(":/", "/");

                                                    //pAxPDFCtl.Inst.ExecUICmd("cmd.edit.deselect");
                                                    //jsQueryString = "syncAnnotScan();";
                                                    //jsResult = this.GetRunJavascript(jsQueryString);
                                                    //jsQueryString = @"importAnFDF(""" + strLoadFilePath + @""")";
                                                    //1DoJavascript(ref pAxPDFCtl, jsQueryString);

                                                    jsQueryString = @"syncAnnotScan(); importAnFDF(""" + strLoadFilePath + @""");";
                                                    m_Inst.ExecuteJS(m_Doc, jsQueryString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                                                    break;
                                                case HxAnnotFormatType.XFDF:
                                                default:
                                                    jsQueryString = @"syncAnnotScan(); importAnXFDF(""" + strLoadFilePath + @""");";
                                                    m_Inst.ExecuteJS(m_Doc, jsQueryString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                                                    Debug.WriteLine(res);
                                                    break;
                                            }

                                            //m_Inst.ExecuteJS(m_Doc, jsQueryString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                                            //IPXV_JSValue jSValue = m_Inst.ExecuteJSEx(doc, jsQueryString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null);
                                            
                                        }
                                        catch (Exception mEx)
                                        {
                                            Debug.WriteLine(mEx);
                                            //throw m;
                                        }

                                        //string r = res.Value;
                                    }

                                }
                            }
                            PDFXEdit.IString destPath = saveFullPath.IsNullOrWhiteSpaceEx() != true ? m_Inst.CreateString(saveFullPath) : m_Inst.CreateString(HxFile.GetFileUniquePath(openFullPath, HxFileOverwriteType.RenameDateTime));
                            if (saveFullPath.IsNullOrWhiteSpaceEx() != true && destPath != null)
                            {
                                m_Doc.Save(destPath);
                                //m_Inst.Doc[m_Inst.DocCount - 1].Save(destPath);
                                //m_Inst.FindDocByCoreDoc(doc).Save(destPath);
                                //m_Inst.ActiveDoc.Save(destPath);
                                Result = destPath.Value;
                            }
                            else
                            {
                                Result = openFullPath;
                                m_Doc.Save(null);
                                //m_Inst.Doc[m_Inst.DocCount - 1].Save(null);
                                //m_Inst.FindDocByCoreDoc(doc).Save(null);
                                //m_Inst.ActiveDoc.Save(destPath);
                                Result = openFullPath;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return Result;
                        throw e;
                    }
                    finally
                    {
                        m_Doc.Close();
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return Result;
                    throw ex;
                }
                finally
                {
                    //m_Inst.Shutdown();
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                }
            }

            /*
            bool bCreate = false;
            if (axPDFCtl == null)
            {
                //IPXC_con
                axPDFCtl = new AxPXV_Control
                {
                    //axPDFCtl.Show();
                    Enabled = true,
                    //axPDFCtl.Name = "AxPDFCtlExecute";
                    Name = "AxPDF_" + HxCrypt.RandPass()
                };
                axPDFCtl.CreateControl();
                bCreate = true;
            }

            try
            {

                

                //IPXC_Document m_CurDoc = coreDoc;
                //m_CurDoc.WriteToFile(openFullPath);
                //m_pxcInst.OpenDocumentFrom((object)openFullPath, null);

                //subPdfCtl.OpenDocFrom()
                
                if (axPDFCtl != null)
                {
                    //ICab cab = axPDFCtl.Inst.CreateOpenDocParams();
                    if (licenseKey.IsNullOrWhiteSpaceEx() != true)
                        axPDFCtl.SetLicKey(licenseKey);
                    else
                        axPDFCtl.SetLicKey(_PXE_LIC_KEY_);
                    
                    //axPDFCtl.Show();
                    axPDFCtl.OpenDocFrom(openFullPath);
                    ExecImportOpAsAnnotsFile(axPDFCtl, annotsFullPath);
                    ExecDocSave(axPDFCtl, saveFullPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                throw ex;
            }
            finally
            {
                if(bCreate == true)
                {
                    axPDFCtl.Hide();
                    axPDFCtl.Inst.Shutdown();
                    axPDFCtl.Dispose();
                }
            }
            */
            return Result;
        }
        [Obsolete("삭제 필요")]
        private static string ExecSaveAsWithAnnotsTEST(string openFullPath, List<string> annotsList = null, string saveFullPath = null)
        {
            //AxPDFXEdit.IPXC_Inst pdfCoreInst = (AxPDFXEdit.IPXC_Inst)pdfInst.GetExtension("PXC");
            string Result = null;
            if (openFullPath.IsNullOrWhiteSpaceEx() != true)
            {
                var m_Inst = new PDFXEdit.PXV_Inst();
                try
                {
                    m_Inst.Init();
                    m_Inst.SetLicKey(_PXE_LIC_KEY_);

                    var m_pxcInst = (PDFXEdit.IPXC_Inst)m_Inst.GetExtension("PXC");
                    PDFXEdit.IPXC_Document doc = m_pxcInst.OpenDocumentFromFile(openFullPath, null);
                    try
                    {
                        //if (annotsFullPath.IsNullOrWhiteSpaceEx() != true && File.Exists(annotsFullPath))
                        //{
                        //    var m_nID = m_Inst.Str2ID("op.document.importCommentsAndFields");
                        //    PDFXEdit.IOperation op = m_Inst.CreateOp(m_nID);
                        //    PDFXEdit.ICabNode input = op.Params.Root["Input"];
                        //    input.Add().v = Doc;
                        //    op.Params.Root["Options.FileName"].v = annotsFullPath; // *.xfdf to
                        //    op.Do();
                        //}
                        if (m_Inst.DocCount > 0)
                        {
                            string saveDirPath = HxFile.GetFileDirPath( saveFullPath??openFullPath );
                            if (annotsList != null && annotsList.Count > 0)
                            {
                                foreach (string annots in annotsList)
                                {
                                    string path = CreateFileXFDF(annots, saveDirPath);
                                    FileInfo file = new FileInfo(path);
                                    if (file.Exists && file.Length > 0)
                                    {
                                        PDFXEdit.IString res = m_Inst.CreateString();
                                        string jsQueryString;
                                        //jsQueryString = "syncAnnotScan();";
                                        //m_Inst.ExecuteJS(m_Inst.Doc[0], jsQueryString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                                        string strLoadXfdfFile = ReplaceFilePath(path);
                                        jsQueryString = @"syncAnnotScan(); importAnXFDF(""" + strLoadXfdfFile + @""");";
                                        try
                                        {
                                            IPXV_JSValue jSValue = m_Inst.ExecuteJSEx(doc, jsQueryString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null);
                                            Debug.WriteLine(jSValue);
                                        }
                                        catch (Exception m)
                                        {
                                            Debug.WriteLine(m);
                                            //throw m;
                                        }
                                        
                                        //string r = res.Value;
                                    }
                                    
                                }
                            }
                            PDFXEdit.IString destPath = saveFullPath.IsNullOrWhiteSpaceEx() != true ? m_Inst.CreateString(saveFullPath) : m_Inst.CreateString(HxFile.GetFileUniquePath(openFullPath, HxFileOverwriteType.RenameDateTime));
                            if (destPath != null)
                            {
                                m_Inst.Doc[m_Inst.DocCount - 1].Save(destPath);
                                //m_Inst.FindDocByCoreDoc(doc).Save(destPath);
                                m_Inst.ActiveDoc.Save(destPath);
                                Result = destPath.Value;
                            }
                            else
                            {
                                m_Inst.Doc[m_Inst.DocCount - 1].Save(null);
                                //m_Inst.FindDocByCoreDoc(doc).Save(null);
                                //m_Inst.ActiveDoc.Save(destPath);
                                Result = openFullPath;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return Result;
                        throw e;
                    }
                    finally
                    {
                        doc.Close();
                    }
                    
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return Result;
                    throw ex;
                }
                finally
                {
                    m_Inst.Shutdown();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            
            /*
            bool bCreate = false;
            if (axPDFCtl == null)
            {
                //IPXC_con
                axPDFCtl = new AxPXV_Control
                {
                    //axPDFCtl.Show();
                    Enabled = true,
                    //axPDFCtl.Name = "AxPDFCtlExecute";
                    Name = "AxPDF_" + HxCrypt.RandPass()
                };
                axPDFCtl.CreateControl();
                bCreate = true;
            }

            try
            {

                

                //IPXC_Document m_CurDoc = coreDoc;
                //m_CurDoc.WriteToFile(openFullPath);
                //m_pxcInst.OpenDocumentFrom((object)openFullPath, null);

                //subPdfCtl.OpenDocFrom()
                
                if (axPDFCtl != null)
                {
                    //ICab cab = axPDFCtl.Inst.CreateOpenDocParams();
                    if (licenseKey.IsNullOrWhiteSpaceEx() != true)
                        axPDFCtl.SetLicKey(licenseKey);
                    else
                        axPDFCtl.SetLicKey(_PXE_LIC_KEY_);
                    
                    //axPDFCtl.Show();
                    axPDFCtl.OpenDocFrom(openFullPath);
                    ExecImportOpAsAnnotsFile(axPDFCtl, annotsFullPath);
                    ExecDocSave(axPDFCtl, saveFullPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                throw ex;
            }
            finally
            {
                if(bCreate == true)
                {
                    axPDFCtl.Hide();
                    axPDFCtl.Inst.Shutdown();
                    axPDFCtl.Dispose();
                }
            }
            */
            return Result;
        }

        private static bool ImportOpAsAnnotsFile(ref AxPXV_Control axPDFCtl, string path)
        {
            if (path.IsNullOrWhiteSpaceEx() != true && File.Exists(path))
            {
                try
                {
                    if (axPDFCtl == null)
                        axPDFCtl = new AxPXV_Control();

                    var op = axPDFCtl.Inst.CreateOp(axPDFCtl.Inst.Str2ID("op.document.importCommentsAndFields"));

                    if (op == null)
                    {
                        return false;
                    }

                    //op.Params.Root["Input"].v = fsInst.DefaultFileSys.StringToName(path);

                    op.Params.Root["Input"].v = axPDFCtl.Doc; // put target-document
                    op.Params.Root["Options.FileName"].v = path; // *.xfdf to
                    op.Do();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                    throw ex;
                }
            }
            return false;
        }

        private static bool ImportJsAsXFDFFile(ref AxPXV_Control axPDFCtl, string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists && file.Length > 0)
                {
                    string jsQueryString, jsResult;
                    axPDFCtl.Inst.ExecUICmd("cmd.edit.deselect");
                    jsQueryString = "syncAnnotScan();";
                    //jsResult = this.GetRunJavascript(jsQueryString);
                    string strLoadXfdfFile = "/" + path.Replace(@"\", "/").Replace(":/", "/");
                    jsQueryString = @"importAnXFDF(""" + strLoadXfdfFile + @""")";
                    jsResult = DoJavascript(ref axPDFCtl, jsQueryString);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                throw ex;
            }

            return false;
        }

        public static string DoJavascript(ref AxPXV_Control axPDFCtl, string jsString)
        {
            string Result = null;
            if (axPDFCtl != null && axPDFCtl.Inst != null && axPDFCtl.Doc != null && jsString.IsNullOrWhiteSpaceEx() != true)
            {
                try
                {
                    PDFXEdit.IString res = axPDFCtl.Inst.CreateString();
                    //syncAnnotScan();
                    //pdfCtl.Inst.de
                    axPDFCtl?.Inst?.ExecuteJS(axPDFCtl.Doc, jsString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                    //IPXV_JSValue jsResult = pdfCtl.Inst.ExecuteJSEx(pdfCtl.Doc.CoreDoc, jsString, PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null);
                    //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    //byte[] resbyte = encoding.GetBytes(res.Value);
                    //Result = Convert.ToBase64String(resbyte);
                    //Result = dnCrypt.Instance.base64_encode(res.Value);
                    Result = res.Value;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    Result = string.Empty;
                }
            }
            return Result;
        }

        public static bool SaveDoc(ref AxPXV_Control axPDFCtl, string fileName, int nFlags = 0, IProgressMon pProgress = null, IPXV_ExportConverter pDestConv = null, ICab pDestConvParams = null, IAFS_FileSys pDestFS = null, ICab pAdvancedParams = null, [ComAliasName("PDFXEdit.HANDLE_T")] uint hWndParent = 0)
        {
            //(int)(PDFXEdit.PXV_DocSaveFlags.PXV_DocSave_AllowUI | PDFXEdit.PXV_DocSaveFlags.PXV_DocSave_Copy | PDFXEdit.PXV_DocSaveFlags.PXV_DocSave_SwitchToDest)
            try
            {
                PDFXEdit.IString destPath = fileName.IsNullOrWhiteSpaceEx() != true ? axPDFCtl.Inst.CreateString(fileName) : null;
                axPDFCtl.Doc.Save(destPath, nFlags, pProgress, pDestConv, pDestConvParams, pDestFS, pAdvancedParams, hWndParent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                //throw ex;
            }
            return true;
        }

        public static string CreateFileXFDF(string xmlString, string saveDirPath = null, HxCryptType cryptType = HxCryptType.MD5)
        {
            string Result = null;
            try
            {
                if (xmlString.IsNullOrWhiteSpaceEx() != true)
                {
                    if (saveDirPath.IsNullOrWhiteSpaceEx())
                    {
                        saveDirPath = Path.GetTempPath();
                    }
                    string tmpName = HxString.GetNowLongDateTimeString();
                    switch (cryptType)
                    {
                        case HxCryptType.Crypt:
                            tmpName = HxCrypt.Encrypt(tmpName);
                            break;
                        case HxCryptType.RandPass:
                            tmpName = HxCrypt.RandPass();
                            break;
                        case HxCryptType.MD5:
                        case HxCryptType.ExportMD5:
                            tmpName = HxCrypt.Md5(xmlString); //HxCrypt.RandPass();
                            break;
                    }
                    string tmpFullName = Path.Combine(saveDirPath, string.Format("{0}.tmp.xfdf", tmpName));
                    string saveFullName = HxFile.GetFileUniquePath(tmpFullName);
                    HxFile.SetTextFileWriteAllText(saveFullName, xmlString);

                    FileInfo file = new FileInfo(saveFullName);
                    if (file.Exists && file.Length > 0)
                    {
                        Result = file.FullName;
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

        public static string CreateFileFDF(string inputString, string saveDirPath = null, HxCryptType cryptType = HxCryptType.MD5, bool bExistCreate = false)
        {
            string Result = null;
            try
            {
                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    if (saveDirPath.IsNullOrWhiteSpaceEx())
                    {
                        saveDirPath = Path.GetTempPath();
                    }
                    string tmpName = HxString.GetNowLongDateTimeString();
                    switch (cryptType)
                    {
                        case HxCryptType.Crypt:
                            tmpName = HxCrypt.Encrypt(tmpName);
                            break;
                        case HxCryptType.RandPass:
                            tmpName = HxCrypt.RandPass();
                            break;
                        case HxCryptType.MD5:
                        case HxCryptType.ExportMD5:
                            tmpName = HxCrypt.Md5(inputString); //HxCrypt.RandPass();
                            break;
                    }
                    string tmpFullName = Path.Combine(saveDirPath, string.Format("{0}.tmp.fdf", tmpName));
                    if (File.Exists(tmpFullName) != true || bExistCreate == true) 
                    {
                        string saveFullName = HxFile.GetFileUniquePath(tmpFullName);
                        HxFile.SetTextFileWriteAllText(saveFullName, inputString, Encoding.ASCII);

                        FileInfo file = new FileInfo(saveFullName);
                        if (file.Exists && file.Length > 0)
                        {
                            Result = file.FullName;
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
        public static string CreateFileFDFFromBase64(string inputString, string saveDirPath = null, HxCryptType cryptType = HxCryptType.MD5, bool bExistCreate = false)
        {
            byte[] bytes = HxString.GetBytesFromBase64Decode(inputString);
            return CreateFileFDF(bytes, saveDirPath, cryptType, bExistCreate);
        }
        public static string CreateFileFDF(byte[] inputByte, string saveDirPath = null, HxCryptType cryptType = HxCryptType.MD5, bool bExistCreate = false)
        {
            string Result = null;
            try
            {
                string inputString = inputByte.ToStringEx();
                if (inputByte != null && inputString.IsNullOrWhiteSpaceEx() != true && inputByte.Length > 0)
                {
                    if (saveDirPath.IsNullOrWhiteSpaceEx())
                    {
                        saveDirPath = Path.GetTempPath();
                    }
                    string tmpName = HxString.GetNowLongDateTimeString();
                    switch (cryptType)
                    {
                        case HxCryptType.Crypt:
                            tmpName = HxCrypt.Encrypt(tmpName);
                            break;
                        case HxCryptType.RandPass:
                            tmpName = HxCrypt.RandPass();
                            break;
                        case HxCryptType.MD5:
                        case HxCryptType.ExportMD5:
                            tmpName = HxCrypt.Md5(inputString); //HxCrypt.RandPass();
                            break;
                    }
                    string tmpFullName = Path.Combine(saveDirPath, string.Format("{0}.tmp.fdf", tmpName));

                    if (File.Exists(tmpFullName))
                    {
                        Result = tmpFullName;
                    }

                    if (File.Exists(tmpFullName) != true || bExistCreate == true)
                    {
                        string saveFullName = HxFile.GetFileUniquePath(tmpFullName);


                        using (FileStream fls = new FileStream(saveFullName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fls.Write(inputByte, 0, inputByte.Length);
                            //fls.Close();
                        }

                        //HxFile.SetTextFileWriteAllText(saveFullName, inputString, Encoding.ASCII);

                        FileInfo file = new FileInfo(saveFullName);
                        if (file.Exists && file.Length > 0)
                        {
                            Result = file.FullName;
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

        public static string ReplaceFilePath(string path)
        {
            string Result = path;
            if (path.IsNullOrWhiteSpaceEx() != true)
            {
                Result = "/" + path.Replace(@"\", "/").Replace(":/", "/");
            }
            return Result;
        }
        #endregion

        
    }
}
