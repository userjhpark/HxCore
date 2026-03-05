using DevExpress.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HxCore.Standard.DE.Pdf
{
    partial class HxPdfDE
    {
        /// <summary>
        /// DevExpress.Pdf.PdfFormDataFormat 타입 가져오기
        /// </summary>
        /// <param name="formDataFormatType">HxCore.HxPdfFormDataFormatType 타입</param>
        /// <returns>DevExpress.Pdf.PdfFormDataFormat 타입</returns>
        protected static PdfFormDataFormat PdfFormDataFormatType2DE(HxPdfFormDataFormatType formDataFormatType)
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

        protected PdfFormDataFormat GetPdfFormDataFormatType2DE(HxPdfFormDataFormatType formDataFormatType)
        {
            return PdfFormDataFormatType2DE(formDataFormatType);
        }

        public static bool PageAnnotsToContentsFlatten(PdfDocumentProcessor processor, bool isAllPageFlattenAnnotations = false, int[] pages = null, string openFileName = null, string saveFileName = null, HxFileOverwriteType saveFileOverwriteType = HxFileOverwriteType.None)
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
                    Result = true;
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

                        var qAnnotType = AnnotsToNotFlattenType.Contains(annot.Type);
                        if (qAnnotType == true) continue;

                        if (annot is PdfMarkupAnnotationFacade markup)
                        {
                            var qAnnotAuthor = AnnotsToNotFlattenAuthor.Contains(markup.Author);
                            if (qAnnotAuthor == true) continue;
                        }

                        annot.Flatten();

                        /*
                        if (annot.Type != PdfAnnotationType.FileAttachment && annot.Type != PdfAnnotationType.Sound && annot.Type != PdfAnnotationType.Movie)
                        {
                            annot.Flatten();
                        }
                        */
                        Result = true;
                    }
                }
            }

            return Result;
        }
    }
}
