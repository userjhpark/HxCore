using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    /*
    using PdfSharp;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.Annotations;
    using PdfSharp.Pdf.IO;

    internal class HxPDF
    {
        public void Open(string filename)
        {
            PdfDocument doc = PdfReader.Open(filename, PdfDocumentOpenMode.ReadOnly);

            //PdfRubberStampAnnotation rsAnnot = new PdfRubberStampAnnotation();

            doc.Close();
        }

        public void Open()
        {
            var document = PdfReader.Open(@"C:/Form.pdf", PdfDocumentOpenMode.Import);
            
            var outputDoc = new PdfDocument();
            outputDoc.AddPage(document.Pages[0]);
            outputDoc.Version = 14;

        }
    }
    */
}
