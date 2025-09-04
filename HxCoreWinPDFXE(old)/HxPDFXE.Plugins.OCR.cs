using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win.PDFXE
{
    partial class HxPDFXE
    {
        public static void OCRPages(PDFXEdit.IPXV_Inst Inst, PDFXEdit.IPXC_Document Doc, Dictionary<string, object> options = null) //PDFXEdit.IPXC_Document
         //where T: PDFXEdit.IPXC_Document
        {
            int nID = Inst.Str2ID("op.document.OCRPages", false);
            PDFXEdit.IOperation Op = Inst.CreateOp(nID);
            PDFXEdit.ICabNode input = Op.Params.Root["Input"];
            input.v = Doc;
            if (options != null && options.Count > 0)
            {
                PDFXEdit.ICabNode cabNodeOption = Op.Params.Root["Options"];
                cabNodeOption["PagesRange.Type"].v = "All"; //OCR all pages
                cabNodeOption["OutputType"].v = 0;
                cabNodeOption["OutputDPI"].v = 300;
                cabNodeOption["ExtParams.Language"].v = "deu"; //separate the needed languages with +
                cabNodeOption["ExtParams.Accuracy"].v = 300;
                cabNodeOption["ExtParams.AutoDeskew"].v = false;
                cabNodeOption["OCRNoTextPagesOnly"].v = false;
                Inst.AsyncDoAndWaitForFinish(Op);

                //PDFXEdit.ICabNode cabNodeOption = Op.Params.Root["Options"];
                //foreach (KeyValuePair<string, object> opt in options)
                //{
                //    try
                //    {
                //        cabNodeOption[opt.Key].v = opt.Value;
                //    }
                //    catch (Exception ex)
                //    {
                //        Debug.WriteLine(ex.Message.ToString());
                //        throw ex;
                //    }

                //}
                //Inst.AsyncDoAndWaitForFinish(Op);

            }


        }
        public void OCRActiveDoc(Dictionary<string, object> options = null)
        {
            if(this.AxPXVCtl != null && this.AxPXVCtl.Inst != null && this.AxPXVCtl?.Doc != null)
            {
                if(options == null)
                {
                    options.Add("PagesRange.Type", "All");
                    options.Add("OutputType", 0);
                    options.Add("OutputDPI", 300);
                    options.Add("ExtParams.Language", "eng"); //separate the needed languages with +
                    options.Add("ExtParams.Accuracy", 300);
                    options.Add("ExtParams.AutoDeskew", false);
                    options.Add("OCRNoTextPagesOnly", false);
                }
                //OCRPages(this.PXVInst, , options?? null);
            }
        }
    }
}
