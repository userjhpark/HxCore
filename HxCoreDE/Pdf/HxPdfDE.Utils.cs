using DevExpress.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HxCore.Standard.DE.Pdf
{
    internal class HxPdfDEUtils
    {
        /// <summary>
        /// PDF 인쇄 및 수정 보안이 걸린 경우 DevExpress.PDF의 표준 미준수(버그?)하는 기능을 이용하여 보안설정 무력화
        /// </summary>
        /// <param name="openFileName">오픈 파일명</param>
        /// <param name="saveFileName">저장 파일명</param>
        /// <param name="saveFileOverwriteType">중복될 경우 파일 처리 옵션</param>
        /// <returns>최종 저장 파일명</returns>
        public static string SaveFromEncryptFile(string openFileName, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        { //PDF 인쇄 및 수정 보안이 걸린 경우 DevExpress.PDF의 표준 미준수(버그?)하는 기능을 이용하여 보안설정 무력화
            string Result = null;

            string strPdfFileName = openFileName;
            if (strPdfFileName.IsNullOrWhiteSpaceEx() == true) return Result;
            strPdfFileName = GetLocalFileName(strPdfFileName);
            if (strPdfFileName.IsNullOrWhiteSpaceEx() || HxFile.FileExists(saveFileName)) return Result;

            using (PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor())
            {
                pdfDocumentProcessor.LoadDocument(strPdfFileName);
                //DevExpress.Pdf.PdfSaveOptions saveOptions = new DevExpress.Pdf.PdfSaveOptions();
                PdfPrinterSettings printSettings = new PdfPrinterSettings();
                PdfEncryptionOptions encryptionOptions = new PdfEncryptionOptions
                {
                    PrintingPermissions = PdfDocumentPrintingPermissions.Allowed,
                    DataExtractionPermissions = PdfDocumentDataExtractionPermissions.NotAllowed,
                    ModificationPermissions = PdfDocumentModificationPermissions.DocumentAssembling,
                    InteractivityPermissions = PdfDocumentInteractivityPermissions.Allowed
                };
                //encryptionOptions.
                //Debug.WriteLine(pdfDocumentProcessor.Document.allowpri);
                //DevExpress.Pdf.PdfGraphicsJpegImageQuality im = new DevExpress.Pdf.PdfGraphicsJpegImageQuality();

                DevExpress.XtraPrinting.PdfExportOptions oo = new DevExpress.XtraPrinting.PdfExportOptions
                {
                    RasterizationResolution = 50,
                    ImageQuality = DevExpress.XtraPrinting.PdfJpegImageQuality.High
                };
                //pdfDocumentProcessor.SaveDocument(strTempFullName, true);
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

        private static PdfAnnotationType[] NoneAnnotsFlatten { get; set; } = { PdfAnnotationType.FileAttachment, PdfAnnotationType.Sound, PdfAnnotationType.Movie };

        public static bool DoPageAnnotsFlatten(PdfDocumentProcessor processor, bool isAllPageFlattenAnnotations = false, int[] pages = null, string openFileName = null, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            bool Result = false;

            if (processor == null) return Result;

            string strPdfFileName = GetLocalFileName(openFileName);
            if (processor.Document == null && strPdfFileName.IsNullOrWhiteSpaceEx()) return Result;

            if (strPdfFileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(strPdfFileName))
            {
                processor.LoadDocument(strPdfFileName);
            }
            if (processor.Document == null || processor.DocumentFacade == null || processor.Document.Pages == null || processor.Document.Pages.Count <= 0) return Result;


            int[] workPages = pages ?? Enumerable.Range(0, processor.Document.Pages.Count - 1).ToArray();
            for (int i = 0; i < workPages.Length; i++)
            {
                int iPage = workPages[i];
                PdfPage page = processor.Document.Pages[iPage];
                if (page == null || page.Annotations.Count <= 0) continue;

                PdfPageFacade facadePage = processor.DocumentFacade.Pages[iPage];
                if (facadePage == null || facadePage.Annotations == null || facadePage.Annotations.Count <= 0) continue;

                if (isAllPageFlattenAnnotations == true)
                {
                    facadePage.FlattenAnnotations();
                }
                else
                {
                    PdfAnnotationFacade annot = null;
                    int nPageAnnots = facadePage.Annotations.Count;
                    if (nPageAnnots <= 0) continue;

                    for (int j = nPageAnnots - 1; j >= 0; j--)
                    {
                        annot = facadePage.Annotations[j];
                        if (annot == null) continue;

                        var q = NoneAnnotsFlatten.Contains(annot.Type);
                        if (q == true) continue;

                        annot.Flatten();

                        /*
                        if (annot.Type != PdfAnnotationType.FileAttachment && annot.Type != PdfAnnotationType.Sound && annot.Type != PdfAnnotationType.Movie)
                        {
                            annot.Flatten();
                        }
                        */
                    }
                }
            }

            return Result;
        }


        /// <summary>
        /// 이전 주석(Annots)를 컨텐츠 항목으로 붙이기(Flatten)
        /// </summary>
        /// <param name="openFileName"></param>
        /// <param name="saveFileName"></param>
        /// <param name="saveFileOverwriteType"></param>
        /// <returns></returns>
        public static string OpenToAnnotsFlatten(string openFileName, bool isAllPageFlattenAnnotations = false, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            string Result = null;

            string strPdfFileName = openFileName;
            if (strPdfFileName.IsNullOrWhiteSpaceEx() == true) return Result;
            strPdfFileName = GetLocalFileName(strPdfFileName);
            if (strPdfFileName.IsNullOrWhiteSpaceEx() || HxFile.FileExists(saveFileName)) return Result;

            using (PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor())
            {
                pdfDocumentProcessor.LoadDocument(strPdfFileName);
                _ = DoPageAnnotsFlatten(pdfDocumentProcessor, isAllPageFlattenAnnotations);

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

        private static string GetLocalFileName(string fileName)
        {
            string Result = fileName;

            if (fileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(Result) != true && HxString.IsWebUri(Result))
            {
                Result = HxNet.GetClientDownloadFile(Result);
            }

            return Result;
        }

        #region PDF 주속/마크업/Annots(FDF,XFDF,XML,TXT) Import
        public static PdfFormDataFormat GetPdfFormDataFormatType(HxPdfFormDataFormatType formDataFormatType)
        {
            PdfFormDataFormat Result = PdfFormDataFormat.Fdf;
            switch (formDataFormatType)
            {
                case HxPdfFormDataFormatType.Xml:
                    Result = PdfFormDataFormat.Xml;
                    break;
                case HxPdfFormDataFormatType.Xfdf:
                    Result = PdfFormDataFormat.Xfdf;
                    break;
                case HxPdfFormDataFormatType.Txt:
                    Result = PdfFormDataFormat.Txt;
                    break;
                case HxPdfFormDataFormatType.None:
                case HxPdfFormDataFormatType.Fdf:
                default:
                    Result = PdfFormDataFormat.Fdf;
                    break;
            }

            return Result;
        }

        public static string MergePDFImportDataFormatAnnots(string openFileName, string annotsFileNameStr, HxPdfFormDataFormatType formDataFormatType, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
        {
            string Result = null;

            if (openFileName.IsNullOrWhiteSpaceEx() == true || HxFile.FileExists(annotsFileNameStr) != true) return Result;

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

            PdfFormDataFormat pdfFormDataFormat = GetPdfFormDataFormatType(formDataFormatType);
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
