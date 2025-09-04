using System;

namespace HxCore.Standard.DE.Pdf
{
    using DevExpress.CodeParser;
    using DevExpress.Pdf;
    using DevExpress.XtraCharts;
    using DevExpress.XtraPrinting;
    using HxCore;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public partial class HxPdfDE : IDisposable
    {
        #region Static Intance
        
        private static HxPdfDE _instance = null;
        static HxPdfDE()
        {
            _instance = Create();
        }
        public static HxPdfDE Instance
        {
            get { return _instance ?? (_instance = Create()); }
            private set { _instance = value; }
        }

        public static HxPdfDE Create()
        {
            return new HxPdfDE();
        }
        #endregion

        /// <summary>
        /// PDF문서관리 객체
        /// </summary>
        protected PdfDocumentProcessor PdfCtl = null;

        protected PdfDocument Doc => PdfCtl?.Document ?? null;

        /// <summary>
        /// 컨텐츠 항목으로 선택적으로 붙여넣기(Flatten) 경우 제외 Annotation 타입(Type)
        /// </summary>
        public static PdfAnnotationType[] AnnotsToNotFlattenType { get; set; } = { PdfAnnotationType.FileAttachment, PdfAnnotationType.Sound, PdfAnnotationType.Movie, PdfAnnotationType.Link, PdfAnnotationType.Annotation3D, PdfAnnotationType.RichMedia};
        /// <summary>
        /// 컨텐츠 항목으로 선택적으로 붙여넣기(Flatten) 경우 제외 Annotation 저자(Author)
        /// </summary>
        public static string[] AnnotsToNotFlattenAuthor { get; set; } = { "AutoCAD SHX Text" };

        

        #region 생성자 및 초기화, 소멸자
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="processor">PDF문서관리 객체</param>
        public HxPdfDE(PdfDocumentProcessor processor = null)
        {
            if (processor == null)
            {
                processor = new PdfDocumentProcessor();
            }
            SetPdfDocumentProcessor(processor);
        }

        //오픈된 문서 닫기(Close)
        public void Close()
        {
            //LoadFileName = null;
            if (IsOpen() == true)
            {
                PdfCtl.CloseDocument();
            }
        }
        /// <summary>
        /// 리소스 해제
        /// </summary>
        /// <param name="disposing">파괴자 호출?</param>

        public void Free(bool disposing = false)
        {
            Close();
            if (disposing == true)
            {
                Dispose();
            }
            PdfCtl = null;
        }

        /// <summary>
        /// 파괴자
        /// </summary>
        public void Dispose()
        {
            Close();
            PdfCtl?.Dispose();
            PdfCtl = null;
            /*
            if (disposing && (this.PdfCtl != null))
            {
                this.PdfCtl?.Dispose();
            }
            //base.Dispose(disposing);
            */
        }
        
        /// <summary>
        /// 소멸자
        /// </summary>
        ~HxPdfDE()
        {
            Free(true);
        }
        #endregion


        /// <summary>
        /// 파일 오픈(Load)
        /// </summary>
        /// <param name="fileName">오픈 파일명</param>
        /// <returns>오픈 여부?</returns>
        public bool SetLoad(string fileName)
        {
            bool Result = false;

            if (PdfCtl == null) return Result;

            //LoadFileName = null;
            PdfCtl.LoadDocument(fileName);
            //LoadFileName = fileName;
            Result = IsOpen();

            /*
            string fieldName = "documentFilePath";
            var str = HxUtils.GetInstanceMemberFieldValue(this.PdfCtl, fieldName).ToStringEx();
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = typeof(PdfDocumentProcessor).GetField(fieldName, bindFlags);
            var val = field.GetValue(this.PdfCtl);
            */

            //PropertyInfo prop = typeof(PdfDocumentProcessor).GetProperty("documentFilePath", BindingFlags.NonPublic | BindingFlags.CreateInstance);
            //MethodInfo getter = prop?.GetGetMethod(nonPublic: true);
            //object bar = getter?.Invoke(this.PdfCtl, null);

            return Result;
        }
        public bool SetSave(string fileName = null)
        {
            bool Result = false;

            if(IsOpen() != true) { return Result; }

            string strPdfFileName = fileName;
            if(fileName.IsNullOrWhiteSpaceEx() != true)
            {
                strPdfFileName = LoadFileName;
            }

            this.PdfCtl.SaveDocument(strPdfFileName);
            if(HxFile.IsFileExists(strPdfFileName)) 
            { 
                Result = true; 
            }

            return Result;
        }

        public string GetOpenFileName()
        {
            //this.PdfCtl.
            return LoadFileName;
        }

        /// <summary>
        /// 문서 오픈 여부?
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return PdfCtl != null && PdfCtl.Document != null && PdfCtl.Document.Pages != null && PdfCtl.Document.Pages.Count > 0;
        }

        /// <summary>
        /// PDF문서관리 객체
        /// </summary>
        /// <param name="processor"></param>
        public void SetPdfDocumentProcessor(PdfDocumentProcessor processor, bool isPrevPdfCtlFree = false)
        {
            Free(isPrevPdfCtlFree);

            PdfCtl = processor;
        }

        #region  문서의 정보 관련 (파일명, 페이지 수)
        /// 열린 파일명
        /// (본 인스턴스 메서드들 활용한 로직으로 이루어진 파일명이며, 직접 오픈한경우 현재는 지원되지 않음. 추후 개선을 위해 찾고있으나 DevExpress.Office API에서 지원이 되지 않고 있음.)
        /// </summary>
        //public string LoadFileName { get; protected set; }
        public string LoadFileName => GetLoadFileName();

        public int PageCount => GetLoadPageCount().ToIntEx(-1);
        protected string GetLoadFileName()
        {
            string Result = null;
            if (this.PdfCtl == null || this.PdfCtl.Document == null || this.PdfCtl.Document.Pages == null) { return Result; }


            string fieldName = "documentFilePath";
            Result = HxUtils.GetInstanceMemberFieldValue(this.PdfCtl, fieldName).ToStringEx();
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
                FieldInfo field = typeof(PdfDocumentProcessor).GetField(fieldName, bindFlags);
                Result = field.GetValue(this.PdfCtl).ToStringEx();
            }
            return Result;
        }

        /// <summary>
        /// Load된 문서의 페이지 수
        /// </summary>
        /// <returns>페이지 수</returns>
        protected int? GetLoadPageCount()
        {
            int? Result = null;

            if (this.IsOpen() != true) { return Result; }

            Result = this.PdfCtl.Document.Pages?.Count ?? -1;

            return Result;
        }
        /// <summary>
        /// 문서의 페이지 수
        /// </summary>
        /// <param name="openFileName">파일 경로</param>
        /// <returns>페이지 수</returns>
        public static int? DocumentPageCount(string openFileName)
        {
            int? Result = null;
            if (openFileName.IsNullOrWhiteSpaceEx() == true) { return Result; }

            try
            {
                using (PdfDocumentProcessor processor = new PdfDocumentProcessor())
                {
                    processor.LoadDocument(openFileName);
                    if (processor.Document != null || processor.Document.Pages != null)
                    {
                        Result = processor.Document.Pages?.Count ?? -1;
                        processor.CloseDocument();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }

            return Result;
        }
        /// <summary>
        /// 문서(Load)의 페이지 수
        /// </summary>
        /// <param name="openFileName">파일 경로</param>
        /// <returns>페이지 수</returns>
        public int? GetPageCount(string openFileName = null)
        {
            if (openFileName.IsNullOrWhiteSpaceEx() != true)
            {   //문서가 지정되어 호출 한 경우
                return DocumentPageCount(openFileName);
            }
            else
            {
                return GetLoadPageCount();
            }
        }

        #endregion


        


        /// <summary>
        /// PDF 인쇄 및 수정 보안이 걸린 경우 DevExpress.PDF의 표준 미준수(버그?)하는 기능을 이용하여 보안설정 무력화
        /// </summary>
        /// <param name="openFileName">오픈 파일명</param>
        /// <param name="saveFileName">저장 파일명</param>
        /// <param name="saveFileOverwriteType">중복될 경우 파일 처리 옵션</param>
        /// <returns>최종 저장 파일명</returns>
        public string SetSaveAsEncryptFileToNormalFile(string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        { //PDF 인쇄 및 수정 보안이 걸린 경우 DevExpress.PDF의 표준 미준수(버그?)하는 기능을 이용하여 보안설정 무력화
            string Result = null;

            if (this.IsOpen() != true) { return Result; }

            string strPdfFileName = this.LoadFileName;

            if (saveFileName.IsNullOrWhiteSpaceEx() == true) { saveFileName = strPdfFileName; }
            //pdfDocumentProcessor.SaveDocument(strTempFullName, true);
            if (saveFileName.IsNullOrWhiteSpaceEx() != true && saveFileOverwriteType != HxFileOverwriteType.None && HxFile.FileExists(saveFileName))
            {
                saveFileName = HxFile.GetFileUniquePath(saveFileName, saveFileOverwriteType);
            }
            else
            {
                saveFileName = strPdfFileName;
            }

            DevExpress.Pdf.PdfSaveOptions saveOptions = new DevExpress.Pdf.PdfSaveOptions();
            PdfPrinterSettings printSettings = new PdfPrinterSettings();
            PdfEncryptionOptions encryptionOptions = new PdfEncryptionOptions
            {
                PrintingPermissions = PdfDocumentPrintingPermissions.Allowed,
                DataExtractionPermissions = PdfDocumentDataExtractionPermissions.NotAllowed,
                ModificationPermissions = PdfDocumentModificationPermissions.Allowed,
                InteractivityPermissions = PdfDocumentInteractivityPermissions.Allowed
            };
            /*
            DevExpress.XtraPrinting.PdfExportOptions oo = new DevExpress.XtraPrinting.PdfExportOptions
            {
                RasterizationResolution = 50,
                ImageQuality = DevExpress.XtraPrinting.PdfJpegImageQuality.High
            };
            */
            saveOptions.EncryptionOptions = encryptionOptions;

            try
            {
                this.PdfCtl.SaveDocument(saveFileName, saveOptions);
                if (saveFileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(saveFileName))
                {

                    Result = saveFileName;
                }
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message);
                //throw ex;
                try
                {
                    using (PdfDocumentProcessor processor = new PdfDocumentProcessor())
                    {
                        processor.LoadDocument(strPdfFileName);
                        saveOptions.EncryptionOptions = encryptionOptions;
                        //encryptionOptions.
                        //Debug.WriteLine(pdfDocumentProcessor.Document.allowpri);
                        //DevExpress.Pdf.PdfGraphicsJpegImageQuality im = new DevExpress.Pdf.PdfGraphicsJpegImageQuality();
                        //pdfDocumentProcessor.SaveDocument(strTempFullName, true);
                        if (saveFileName.IsNullOrWhiteSpaceEx() != true && saveFileOverwriteType != HxFileOverwriteType.None && HxFile.FileExists(saveFileName))
                        {
                            saveFileName = HxFile.GetFileUniquePath(saveFileName, saveFileOverwriteType);
                        }
                        else
                        {
                            saveFileName = strPdfFileName;
                        }
                        processor.SaveDocument(saveFileName, saveOptions, false);
                        processor.CloseDocument();
                        if (saveFileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(saveFileName))
                        {
                            Result = saveFileName;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2.Message);
                    //throw;
                }
            }
            return Result;
        }
        public string SetConvertTypePdfCompatibilityToPdfStandard(string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None, HxPdfCompatibilityType pdfCompatibility = HxPdfCompatibilityType.Pdf)
        {
            //return SetConvertCompatibileFormatToPdfStandard(null, saveFileName, saveFileOverwriteType, pdfCompatibility);

            string Result = null;
            if (this.PdfCtl == null) { return Result; }
            
            if (this.IsOpen() != true) { return Result; }

            //PdfDocumentProcessor processor = this.PdfCtl;
            string strPdfFileName = this.LoadFileName;

            if (saveFileName.IsNullOrWhiteSpaceEx() == true) { saveFileName = strPdfFileName; }
            //pdfDocumentProcessor.SaveDocument(strTempFullName, true);
            if (saveFileName.IsNullOrWhiteSpaceEx() != true && saveFileOverwriteType != HxFileOverwriteType.None && HxFile.FileExists(saveFileName))
            {
                saveFileName = HxFile.GetFileUniquePath(saveFileName, saveFileOverwriteType);
            }
            else
            {
                saveFileName = strPdfFileName;
            }

            if(saveFileName == strPdfFileName)
            {
                //saveFileName = $"{HxFile.DirectoryFullName(saveFileName)}.{HxFile.GetFileExt(saveFileName)}";
                saveFileName = Path.Combine(HxFile.DirectoryFullName(saveFileName), "Create_" + HxFile.GetFileName(saveFileName));
                saveFileName = HxFile.GetFileUniquePath(saveFileName, HxFileOverwriteType.RenameDateMicroTime);
            }

            try
            {
                Debug.WriteLine(this.PdfCtl?.Document?.Version);
                Debug.WriteLine(this.PdfCtl?.Document?.OutputIntents?[0].Subtype);
                //this.PdfCtl.CloseDocument();
                //processor.LoadDocument(openFileName);
                //processor.Document.OpenAction = new PdfJavaScriptAction("this.print({bUI: true,bSilent: false,bShrinkToFit: true});this.closeDoc();",  processor.Document);
                using(PdfDocumentProcessor processor = new PdfDocumentProcessor())
                {
                    PdfCompatibility createPdfCompatibilityType = GetPdfCompatibilityType2DE(pdfCompatibility);
                    PdfCreationOptions creationOptions = new PdfCreationOptions
                    {
                        Compatibility = createPdfCompatibilityType
                    };

                    processor.CreateEmptyDocument(saveFileName, creationOptions);
                    processor.AppendDocument(strPdfFileName);

                    processor.SaveDocument(saveFileName);
                    processor.CloseDocument();
                }
                if (saveFileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(saveFileName))
                {
                    //processor.CloseDocument();
                    //this.SetLoad(Result);
                    Result = saveFileName;
                }
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                //throw;
            }

            return Result;
        }
        public string SetConvertTypePdfCompatibilityToPdfStandardDE(string saveFileName, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None, PdfCompatibility pdfCompatibility = PdfCompatibility.Pdf)
        {
            string Result = null;
            if (this.PdfCtl == null) { return Result; }

            string strLoadFileName = this.LoadFileName;
            if (this.IsOpen() != true && strLoadFileName.IsNullOrWhiteSpaceEx() != true) { this.SetLoad(strLoadFileName); }
            if (this.IsOpen() != true) { return Result; }

            PdfDocumentProcessor processor = this.PdfCtl;
            string strPdfFileName = strLoadFileName;

            if (saveFileName.IsNullOrWhiteSpaceEx() == true) { saveFileName = strLoadFileName; }
            //pdfDocumentProcessor.SaveDocument(strTempFullName, true);
            if (saveFileName.IsNullOrWhiteSpaceEx() != true && saveFileOverwriteType != HxFileOverwriteType.None && HxFile.FileExists(saveFileName))
            {
                saveFileName = HxFile.GetFileUniquePath(saveFileName, saveFileOverwriteType);
            }
            else
            {
                saveFileName = strPdfFileName;
            }

            try
            {
                switch (pdfCompatibility)
                {
                    case PdfCompatibility.Pdf:
                        break;
                    case PdfCompatibility.PdfA1b:
                        break;
                    case PdfCompatibility.PdfA2b:
                        break;
                    case PdfCompatibility.PdfA3b:
                        break;
                    default:
                        pdfCompatibility = PdfCompatibility.Pdf;
                        break;
                }
                PdfCreationOptions creationOptions = new PdfCreationOptions
                {
                    Compatibility = pdfCompatibility
                };



                //processor.LoadDocument(openFileName);
                //processor.Document.OpenAction = new PdfJavaScriptAction("this.print({bUI: true,bSilent: false,bShrinkToFit: true});this.closeDoc();",  processor.Document);


                this.SetLoad(strLoadFileName);
                Debug.WriteLine(this.PdfCtl.Document.Version);
                Debug.WriteLine(this.PdfCtl.Document.OutputIntents?[0].Subtype);

                processor.CreateEmptyDocument(saveFileName, creationOptions);
                string strTempName = this.LoadFileName;
                processor.AppendDocument(strLoadFileName);
                Debug.WriteLine(this.PdfCtl.Document.OutputIntents?[0].Subtype);
                processor.CloseDocument();
                if (saveFileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(saveFileName))
                {
                    Result = saveFileName;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                //throw;
            }

            return Result;
        }

        public string SetConvertTypePdfCompatibilityToPdfStandard(string openFileName, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None, PdfCompatibility pdfCompatibility = PdfCompatibility.Pdf)
        {
            string Result = null;
            if(this.PdfCtl == null) { return Result; }
            if (this.IsOpen() != true) { this.SetLoad(openFileName); }
            if (this.IsOpen() != true) { return Result; }

            PdfDocumentProcessor processor = this.PdfCtl;
            string strPdfFileName = openFileName;

            if (saveFileName.IsNullOrWhiteSpaceEx() == true) { saveFileName = openFileName; }
            //pdfDocumentProcessor.SaveDocument(strTempFullName, true);
            if (saveFileName.IsNullOrWhiteSpaceEx() != true && saveFileOverwriteType != HxFileOverwriteType.None && HxFile.FileExists(saveFileName))
            {
                saveFileName = HxFile.GetFileUniquePath(saveFileName, saveFileOverwriteType);
            }
            else
            {
                saveFileName = strPdfFileName;
            }

            try
            {
                switch (pdfCompatibility)
                {
                    case PdfCompatibility.Pdf:
                        break;
                    case PdfCompatibility.PdfA1b:
                        break;
                    case PdfCompatibility.PdfA2b:
                        break;
                    case PdfCompatibility.PdfA3b:
                        break;
                    default:
                        pdfCompatibility = PdfCompatibility.Pdf;
                        break;
                }
                PdfCreationOptions creationOptions = new PdfCreationOptions
                {
                    Compatibility = pdfCompatibility
                };



                //processor.LoadDocument(openFileName);
                //processor.Document.OpenAction = new PdfJavaScriptAction("this.print({bUI: true,bSilent: false,bShrinkToFit: true});this.closeDoc();",  processor.Document);


                this.SetLoad(openFileName);
                Debug.WriteLine(this.PdfCtl.Document.Version);
                Debug.WriteLine(this.PdfCtl.Document.OutputIntents?[0].Subtype);

                processor.CreateEmptyDocument(saveFileName, creationOptions);
                string strTempName = this.LoadFileName;
                processor.AppendDocument(openFileName);
                Debug.WriteLine(this.PdfCtl.Document.OutputIntents?[0].Subtype);
                processor.CloseDocument();
                if (saveFileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(saveFileName))
                {
                    Result = saveFileName;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                //throw;
            }

            return Result;
        }
        public HxPdfCompatibilityType GetPdfCompatibilityType(PdfCompatibility compatibilityType)
        {
            HxPdfCompatibilityType Result = HxPdfCompatibilityType.None;

            switch (compatibilityType)
            {
                case PdfCompatibility.Pdf:
                    break;
                case PdfCompatibility.PdfA1b:
                    break;
                case PdfCompatibility.PdfA2b:
                    break;
                case PdfCompatibility.PdfA3b:
                    break;
                default:
                    Result = HxPdfCompatibilityType.None;
                    break;
            }
            return Result;
        }
        /// <summary>
        /// Devexpress.Pdf의 PdfCompatibility으로 가져오기
        /// </summary>
        /// <param name="compatibilityType">HxPdfCompatibilityType</param>
        /// <returns>Devexpress.Pdf의 PdfCompatibility</returns>
        public PdfCompatibility GetPdfCompatibilityType2DE(HxPdfCompatibilityType compatibilityType)
        {
            PdfCompatibility Result = PdfCompatibility.Pdf;

            switch (compatibilityType)
            {
                case HxPdfCompatibilityType.Pdf:
                    Result = PdfCompatibility.Pdf;
                    break;
                case HxPdfCompatibilityType.PdfA1b:
                    Result = PdfCompatibility.PdfA1b;
                    break;
                case HxPdfCompatibilityType.PdfA2b:
                    Result = PdfCompatibility.PdfA2b;
                    break;
                case HxPdfCompatibilityType.PdfA3b:
                    Result = PdfCompatibility.PdfA3b;
                    break;
                default:
                    Result = PdfCompatibility.Pdf;
                    break;
            }

            return Result;
        }



        public HxPdfCompatibilityType GetPdfCompatibilityType(string compatibilityStr)
        {
            HxPdfCompatibilityType Result = HxPdfCompatibilityType.None;
            if (compatibilityStr.IsNullOrWhiteSpaceEx() == true) { return Result; }

            string strPdfCompatibility = compatibilityStr?.ToUpper();
            if(strPdfCompatibility.Length > 9)
            {
                strPdfCompatibility = strPdfCompatibility.Substring(0, 9);
            }
            switch (strPdfCompatibility)
            {
                case HxDefs._PDF_GTS_PDFA1_:
                    Result = HxPdfCompatibilityType.PdfA1b;
                    break;
                case HxDefs._PDF_GTS_PDFA2_:
                    Result = HxPdfCompatibilityType.PdfA2b;
                    break;
                case HxDefs._PDF_GTS_PDFA3_:
                    Result = HxPdfCompatibilityType.PdfA3b;
                    break;
                default:
                    Result = HxPdfCompatibilityType.Pdf;
                    break;
            }

            return Result;
        }

        /// <summary>
        /// 로드된 파일의 PDF/A 포멧 여부?
        /// </summary>
        /// <param name="fileName">파일 경로</param>
        /// <returns>PDF/A 여부</returns>
        public bool? IsPdfAFromCompatibilityType(string fileName = null)
        {
            bool? Result = null;

            if(this.PdfCtl == null) { return Result; }

            if(fileName.IsNullOrWhiteSpaceEx() != true)
            {
                this.SetLoad(fileName);
            }

            if (this.IsOpen() == true && this.PdfCtl.Document.OutputIntents != null && this.PdfCtl.Document.OutputIntents.Count > 0)
            {
                string strDocSubtype = this.PdfCtl.Document.OutputIntents?[0].Subtype;
                if (strDocSubtype.IsNullOrWhiteSpaceEx() != true)
                {
                    HxPdfCompatibilityType pdfCompatibilityType = GetPdfCompatibilityType(strDocSubtype);
                    if (pdfCompatibilityType == HxPdfCompatibilityType.PdfA1b || pdfCompatibilityType == HxPdfCompatibilityType.PdfA2b || pdfCompatibilityType == HxPdfCompatibilityType.PdfA3b)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }
            }

            return Result;
        }
        public bool? IsPermisionModifyAnnotations(string fileName = null)
        {
            bool? Result = null;

            if (this.PdfCtl == null) { return Result; }

            if (fileName.IsNullOrWhiteSpaceEx() != true)
            {
                this.SetLoad(fileName);
            }

            if (this.IsOpen() == true && this.Doc != null)
            {
                if(this.Doc.AllowAnnotationsAndFormsModifying == true )
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        }

        public bool? IsPermisionModifyDefault(string fileName = null)
        {
            bool? Result = null;

            if (this.PdfCtl == null) { return Result; }

            if (fileName.IsNullOrWhiteSpaceEx() != true)
            {
                this.SetLoad(fileName);
            }

            if (this.IsOpen() == true && this.Doc != null)
            {
                if (this.Doc.AllowAnnotationsAndFormsModifying == true && this.Doc.AllowPrinting == true) //this.Doc.AllowDataExtraction //this.Doc.AllowModifying
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        }

        /// <summary>
        /// 로컬디스크 파일이 아닌 URI인 경우 로컬로 다운로드 후 임시 파일명을 돌려 줌
        /// </summary>
        /// <param name="fileName">오픈 파일명</param>
        /// <returns>임시(오픈) 파일명</returns>
        private static string GetLocalFileName(string fileName)
        {
            return HxUtils.GetLocalFileName(fileName);
        }

        /// <summary>
        /// DocumentFacade의 Page별 Annotations을 컨텐츠 항목으로 붙이기(Flatten)
        /// </summary>
        /// <param name="openFileName">원본 파일명</param>
        /// <param name="saveFileName">변경 후 저장 파일명</param>
        /// <param name="saveFileOverwriteType">저장 파일명 중복시 처리 옵션</param>
        /// <returns>최종 저장 파일명(empty : 작업을 완료했으나, 변경이 없음, null : 작업 대상이 없거나 작업 중 오류 발생)</returns>
        public string SetOpenToAnnotsFlatten(string openFileName = null, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            string Result = null;

            if(this.PdfCtl == null) { return Result; }


            string strPdfFileName = openFileName;
            if (strPdfFileName.IsNullOrWhiteSpaceEx() == true) return Result;
            strPdfFileName = GetLocalFileName(strPdfFileName);
            if (strPdfFileName.IsNullOrWhiteSpaceEx() || HxFile.FileExists(strPdfFileName) != true) return Result;

            if(strPdfFileName.IsNullOrWhiteSpaceEx() != true)
            {
                this.SetLoad(strPdfFileName);
            }

            if(this.IsOpen())
            {
                this.SetLoad(strPdfFileName);
                bool bAnnotsToFlatten = PageAnnotsToContentsFlatten(PdfCtl, false);

                if (saveFileName.IsNullOrWhiteSpaceEx() != true && saveFileOverwriteType != HxFileOverwriteType.OverWrite)
                {
                    saveFileName = HxFile.GetFileUniquePath(saveFileName, saveFileOverwriteType);
                }
                else if (saveFileName.IsNullOrWhiteSpaceEx() == true)
                {
                    saveFileName = strPdfFileName;
                }
                if (bAnnotsToFlatten == true)
                {
                    /*
                    PdfExportOptions oo = new PdfExportOptions
                    {
                        RasterizationResolution = 50,
                        ImageQuality = PdfJpegImageQuality.Lowest
                    };
                    */

                    /*
                    using (PdfGraphics graphics = this.PdfCtl.CreateGraphics())
                    {
                        graphics.SaveGraphicsState();

                        //Define position for the content on your target page:
                        graphics.TranslateTransform((float)(page.CropBox.Width * 1 / 3),
                        (float)(page.CropBox.Height * 4 / 5));
                        PdfRectangle clip = processor2.Document.Pages[0].CropBox;

                        //Resize source page content to fit the target page:
                        float scaleFactor = (float)(page.CropBox.Width / clip.Width / 3);
                        graphics.ScaleTransform(scaleFactor, scaleFactor);

                        //Crop source content:
                        graphics.IntersectClip(new RectangleF((float)clip.Left, (float)(clip.Top / 2.8),
                        (float)clip.Width, (float)(clip.Height / 2.8)));

                        //Draw the cropped segment in the target page:
                        graphics.DrawPageContent(this.PdfCtl.Document.Pages[0]);
                        graphics.RestoreGraphicsState();

                        //Apply changes:
                        graphics.AddToPageForeground(page, 72, 72);
                    }
                    */

                    this.PdfCtl.SaveDocument(saveFileName, true);
                    Result = saveFileName;
                }
                else if(bAnnotsToFlatten != true && strPdfFileName != saveFileName)
                {
                    //File.Copy(strPdfFileName, saveFileName, true);
                    //Result = saveFileName;
                    Result = string.Empty;
                }
                else if(bAnnotsToFlatten != true)
                {
                    //Result = openFileName;
                    Result = string.Empty;
                }
                else
                {
                    Result = null;
                }

            }

            return Result;
        }


        #region PDF 주속/마크업/Annots(FDF,XFDF,XML,TXT) Import

        public string MergePDFImportDataFormatAnnots(string annotsFileNameStr, HxPdfFormDataFormatType formDataFormatType, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            return null;
        }

        public static string MergePDFImportDataFormatAnnots(string openFileName, string annotsFileNameStr, HxPdfFormDataFormatType formDataFormatType, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            string Result = null;
            if (openFileName.IsNullOrWhiteSpaceEx() == true || HxFile.FileExists(annotsFileNameStr) != true) { return Result; }

            Result = MergePDFImportDataFormatAnnots(openFileName, new string[] { annotsFileNameStr }, formDataFormatType, saveFileName, saveFileOverwriteType);

            return Result;
        }

        public static string MergePDFImportDataFormatAnnots(string openFileName, string[] annotsFileNameArry, HxPdfFormDataFormatType formDataFormatType, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            string Result = null;

            if (openFileName.IsNullOrWhiteSpaceEx() == true || annotsFileNameArry == null || annotsFileNameArry.Length <= 0) return Result;
            //TODO : 코딩 하자! 2023.12.14 17:30 까지 작성 함.
            string strPdfFileName = GetLocalFileName(openFileName);
            if (saveFileName.IsNullOrWhiteSpaceEx() == true || HxString.IsWebUri(strPdfFileName) == true)
            {
                //strPdfFileName = GetLocalFileName(strPdfFileName);
            }
            saveFileName = saveFileName ?? strPdfFileName;

            PdfFormDataFormat pdfFormDataFormat = PdfFormDataFormatType2DE(formDataFormatType);
            using (PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor())
            {
                pdfDocumentProcessor.LoadDocument(strPdfFileName);

                foreach (string strAnnotFileName in annotsFileNameArry)
                {
                    pdfDocumentProcessor.Import(strAnnotFileName, pdfFormDataFormat);
                }

                if (saveFileName.IsNullOrWhiteSpaceEx() != true && saveFileOverwriteType != HxFileOverwriteType.None && HxFile.FileExists(saveFileName))
                {
                    saveFileName = HxFile.GetFileUniquePath(saveFileName, saveFileOverwriteType);
                }
                else
                {
                    saveFileName = strPdfFileName;
                }
                pdfDocumentProcessor.SaveDocument(saveFileName, true);

                Result = saveFileName;
            }

            return Result;
        }

        public string SetImportDataFormatAnnots(string[] annotsFileNameArry, HxPdfFormDataFormatType formDataFormatType, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            string Result = null;

            if (this.IsOpen() != true || annotsFileNameArry == null || annotsFileNameArry.Length <= 0) { return Result; }

            //TODO : 코딩 하자! 2023.12.14 17:30 까지 작성 함.
            string strPdfFileName = this.LoadFileName;
            strPdfFileName = GetLocalFileName(strPdfFileName);
            PdfFormDataFormat pdfFormDataFormat = PdfFormDataFormatType2DE(formDataFormatType);
            foreach (string strAnnotFileName in annotsFileNameArry)
            {
                this.PdfCtl.Import(strAnnotFileName, pdfFormDataFormat);
            }
            this.PdfCtl.SaveDocument(strPdfFileName);

            return Result;
        }


        public static string MergePDFImportDataFormatFdf(string openFileName, string fdfFileName, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            return MergePDFImportDataFormatAnnots(openFileName, fdfFileName, HxPdfFormDataFormatType.Fdf, saveFileName, saveFileOverwriteType);
        }

        public static string MergePDFImportDataFormatXfdf(string openFileName, string xfdfFileName, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            return MergePDFImportDataFormatAnnots(openFileName, xfdfFileName, HxPdfFormDataFormatType.Xfdf, saveFileName, saveFileOverwriteType);
        }
        public static string MergePDFImportDataFormatXml(string openFileName, string xmlFileName, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            return MergePDFImportDataFormatAnnots(openFileName, xmlFileName, HxPdfFormDataFormatType.Xml, saveFileName, saveFileOverwriteType);
        }
        public static string MergePDFImportDataFormatTxt(string openFileName, string txtFileName, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            return MergePDFImportDataFormatAnnots(openFileName, txtFileName, HxPdfFormDataFormatType.Txt, saveFileName, saveFileOverwriteType);
        }
        #endregion //PDF 주속/마크업/Annots(FDF,XFDF,XML,TXT) Import


    }
}
