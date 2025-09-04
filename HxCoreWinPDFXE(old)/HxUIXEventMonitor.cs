using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win.PDFXE
{
    public partial class HxUIXEventMonitor : PDFXEdit.IUIX_EventMonitor
    {
        public HxUIXEventMonitor()
            : base()
        {
        }

        public virtual void OnEventMonitor(PDFXEdit.IUIX_Obj pTarget, PDFXEdit.IUIX_Event pEvent)
        {
            //Debug.WriteLine("OnEventMonitor(code={0})", pEvent.Code);
            // Debug.WriteLine("OnEventMonitor(code={0})", pEvent.Code);
            // pEvent.Code; - code of standard system’s message like: WM_CHAR, WM_KEYDOWN, WM_KEYUP, ... WM_MOUSEMOVE
            // and parameters of standard system’s message
            // pEvent.Param1; == wParam
            // pEvent.Param2; == lParam
            //	bool bEndModal = false;
            //	if ((pEvent.Code == (int)PDFXEdit.UIX_EventCodes.e_BeginModal) || (bEndModal = (pEvent.Code == (int)PDFXEdit.UIX_EventCodes.e_EndModal)))
            //	{
            //		if (pTarget.get_ID() == pTarget.ThreadCtx.Inst.Str2ID("DlgScanProfile"))
            //		{
            //			if (bEndModal)
            //			{
            //				bool bScanBtnClicked = ((int)pEvent.Param1 == pTarget.ThreadCtx.Inst.Str2ID("btn.scan"));
            //			}
            //		}
            //	}
        }
    }
}
