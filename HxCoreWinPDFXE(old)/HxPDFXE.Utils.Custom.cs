using AxPDFXEdit;
using PDFXEdit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win.PDFXE
{
    partial class HxPDFXE
    {
        public enum HxCustomNumberAnnotType
        {
            None,
            Text,
            Circle,
            Triangle,
            Square,
            Hexagon,
            Diamond,
            SquareAndCircle
        }

        public PdfEditorCommandHandler cmdHandler { get; private set; }

        public const string _PDFXE_CNODE_CMD_TOOLS_ = "cmd.tools";

        #region PDF-XChange Editor SDK / CAB Node 사용자(Custom) 명령어
        //public const string _PDFXE_CNODE_CMD_CUSTOM_ABOUT_ = "cmd.custom.About";
        //public const string _PDFXE_CNODE_CMD_CUSTOM_PRINTALL_ = "cmd.custom.printAll";
        public const string _PDFXE_CNODE_CMD_CUSTOM_CopyAnnotsToPages_ = "cmd.custom.CopyAnnotsToPages";
        public const string _PDFXE_CNODE_CMD_CUSTOM_ChangeAnnotsContents_ = "cmd.custom.ChangeAnnotsContents";
        public const string _PDFXE_CNODE_CMD_CUSTOM_NumberCircleStamp_ = "cmd.custom.NumberCircleStamp";
        #endregion
        private const string _CUSTOM_NUMBER_CIRCLE_CATEGORY_NAME_PREFIX_ = "HTE.ZZ.Custom.Number.Circle";
        private const string _CUSTOM_NUMBER_CIRCLE_STAMP_NAME_SUFFIX_ = "Circle.Stamps";


        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_FreeText_ = "HTE.Number.FreeText";
        
        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_Circle_ = "HTE.Number.Circle";
        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_Squire_ = "HTE.Number.Squire";
        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_Diamond_ = "HTE.Number.Diamond";
        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_SquireAndCircle_ = "HTE.Number.SquireAndCircle";

        //const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_Polygon_ = "HTE.Number.Polygon";
        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_Triangle_ = "HTE.Number.Triangle";
        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_Hexagon_ = "HTE.Number.Hexagon";

        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_DivisionLine_ = "HTE.Number.DivisionLine";
        const string _CUSTOM_CUSTOM_NUMBER_SUBJECT_LeaderLine_ = "HTE.Number.LeaderLine";
        //Loading image file

        public void SetLoadHTECustomMenuToolsHandle_All(string author = null, string basePath = null)
        {
            try
            {
                if (pdfCtl == null || (pdfCtl != null && pdfCtl.Frame == null))
                    return;
                bool bRibbonMode = pdfCtl.Frame.View.IsRibbonMode;
                if (bRibbonMode != false) return;

                SetAppendHTECustomMenuToolsHandle_CopyAnnotsToPages(false, basePath);
                SetAppendHTECustomMenuToolsHandle_ChangeAnnotsContents(false, basePath);
                SetAppendPlayHTECustomMenuToolsHandle_NumberCircleStamp(false, basePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            //AxPXV_Control AxPXVCtl
            //AxPXV_Control pdfCtl = AxPXVCtl;

        }

        public void SetAppendHTECustomMenuToolsHandle_CopyAnnotsToPages(bool isMenuInsertSeparator = false, string basePath = null)
        {   //선택한 주석 여러페이지에 복사
            if (this.AxPXVCtl == null || (this.AxPXVCtl != null && this.AxPXVCtl.Frame == null) || this.AxPXVCtl.Inst == null) return;
            if (this.IsRibbonMode != false) return;
            if (this._menuCmdBar == null) return;

            try
            {
                if (basePath.IsNullOrWhiteSpaceEx() == true) basePath = HxUtils.AppBaseDir;

                if (this.cmdHandler == null) cmdHandler = new PdfEditorCommandHandler(this.Inst);
                if (this.cmdHandler == null) return;

                int nFlatToolsIndex = this._menuCmdBar.FlatFindFirstItemByCmdName(_PDFXE_CNODE_CMD_TOOLS_);
                PDFXEdit.IUIX_CmdItem cmdTools = this._menuCmdBar.FlatGetItem(nFlatToolsIndex);
                PDFXEdit.IUIX_CmdMenu cmdToolsMenu = cmdTools.SubMenu;
                #region CUSTOM CopyAnnotsToPages
                if (isMenuInsertSeparator == true) cmdToolsMenu.InsertSeparator();
                IUIX_Cmd PrevCustomCopyAnnotsToPages = _uixInst.CmdManager.Cmds.Find(_PDFXE_CNODE_CMD_CUSTOM_CopyAnnotsToPages_);
                if (PrevCustomCopyAnnotsToPages != null)
                {
                    this.ExecCmdBothOff(PrevCustomCopyAnnotsToPages.ID, false);
                    //this.ExecCmdBothOff(PrevCustomCopyAnnotsToPages.ID, false);
                }
                else
                {
                    int nCmdCustomCopyAnnotsToPages = this.Inst.Str2ID(_PDFXE_CNODE_CMD_CUSTOM_CopyAnnotsToPages_);

                    PDFXEdit.IUIX_Cmd cmdCustomCopyAnnotsToPages = _uixInst.CmdManager.Cmds.AddNew2(nCmdCustomCopyAnnotsToPages, 0, cmdHandler);
                    cmdCustomCopyAnnotsToPages.Title = "선택한 주석 여러페이지에 복사";// "Copy Comment To Multiple Pages";
                    cmdCustomCopyAnnotsToPages.ShortTitle = "Copy Comment To Multiple Pages";
                    cmdCustomCopyAnnotsToPages.Tip = "선택한 주석을 여러 페이지에 복사 (Copy Comment To Multiple Pages)";
                    string iconCustomCopyAnnotsToPagesPath = Path.Combine(basePath, "Images", "Icon-CopyAnnotsToPages.png"); //System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Icon -HiPDFPro_App.png"
                    if (HxFile.FileExists(iconCustomCopyAnnotsToPagesPath))
                    {
                        PDFXEdit.IAFS_Name name = _afsInst.DefaultFileSys.StringToName(iconCustomCopyAnnotsToPagesPath);
                        PDFXEdit.IAFS_File file = _afsInst.DefaultFileSys.OpenFile(name, (int)PDFXEdit.AFS_OpenFileFlags.AFS_OpenFile_Read | (int)PDFXEdit.AFS_OpenFileFlags.AFS_OpenFile_ShareRead);
                        cmdCustomCopyAnnotsToPages.Icon = _uixInst.CreateIconFromIStream(file.GetStream());
                    }
                    cmdToolsMenu.InsertItem2(nCmdCustomCopyAnnotsToPages);
                }
                //cmdToolsMenu.InsertSeparator();
                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
        }
        public void SetAppendHTECustomMenuToolsHandle_ChangeAnnotsContents(bool isMenuInsertSeparator = false, string basePath = null)
        {
            if (this.AxPXVCtl == null || (this.AxPXVCtl != null && this.AxPXVCtl.Frame == null) || this.AxPXVCtl.Inst == null) return;
            if (this.IsRibbonMode != false) return;
            if (this._menuCmdBar == null) return;

            try
            {
                if (basePath.IsNullOrWhiteSpaceEx() == true) basePath = HxUtils.AppBaseDir;

                if (this.cmdHandler == null) cmdHandler = new PdfEditorCommandHandler(this.Inst);
                if (this.cmdHandler == null) return;

                int nFlatToolsIndex = this._menuCmdBar.FlatFindFirstItemByCmdName(_PDFXE_CNODE_CMD_TOOLS_);
                PDFXEdit.IUIX_CmdItem cmdTools = this._menuCmdBar.FlatGetItem(nFlatToolsIndex);
                PDFXEdit.IUIX_CmdMenu cmdToolsMenu = cmdTools.SubMenu;
                #region CUSTOM ChangeAnnotsContents
                if (isMenuInsertSeparator == true) cmdToolsMenu.InsertSeparator();
                int nCmdCustomChangeAnnotsContents = pdfCtl.Inst.Str2ID(_PDFXE_CNODE_CMD_CUSTOM_ChangeAnnotsContents_);

                PDFXEdit.IUIX_Cmd cmdCustomChangeAnnotsContents = this._uixInst.CmdManager.Cmds.AddNew2(nCmdCustomChangeAnnotsContents, 0, cmdHandler);
                cmdCustomChangeAnnotsContents.Title = "선택한 주석 내용 일괄변경";//"Change Comment Contents";
                cmdCustomChangeAnnotsContents.ShortTitle = "Change Comment Contents";
                cmdCustomChangeAnnotsContents.Tip = "선택한 주석의 내용을 일괄변경 (Change the content of the selected comment)";
                string iconCustomChangeAnnotsContentsPath = Path.Combine(basePath, "Images", "Icon-ChangeAnnotsContents.png"); //System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Icon -HiPDFPro_App.png"
                if (HxFile.FileExists(iconCustomChangeAnnotsContentsPath))
                {
                    PDFXEdit.IAFS_Name name = this._afsInst.DefaultFileSys.StringToName(iconCustomChangeAnnotsContentsPath);
                    PDFXEdit.IAFS_File file = this._afsInst.DefaultFileSys.OpenFile(name, (int)PDFXEdit.AFS_OpenFileFlags.AFS_OpenFile_Read | (int)PDFXEdit.AFS_OpenFileFlags.AFS_OpenFile_ShareRead);
                    cmdCustomChangeAnnotsContents.Icon = this._uixInst.CreateIconFromIStream(file.GetStream());
                }
                cmdToolsMenu.InsertItem2(nCmdCustomChangeAnnotsContents);
                //cmdToolsMenu.InsertSeparator();
                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
        }

        public void SetAppendPlayHTECustomMenuToolsHandle_NumberCircleStamp(bool isMenuInsertSeparator = false, string basePath = null)
        {
            if (this.AxPXVCtl == null || (this.AxPXVCtl != null && this.AxPXVCtl.Frame == null) || this.AxPXVCtl.Inst == null) return;
            if (this.IsRibbonMode != false) return;
            if (this._menuCmdBar == null) return;

            try
            {
                if (basePath.IsNullOrWhiteSpaceEx() == true) basePath = HxUtils.AppBaseDir;

                if (this.cmdHandler == null) cmdHandler = new PdfEditorCommandHandler(this.Inst);
                if (this.cmdHandler == null) return;

                int nFlatToolsIndex = this._menuCmdBar.FlatFindFirstItemByCmdName(_PDFXE_CNODE_CMD_TOOLS_);
                PDFXEdit.IUIX_CmdItem cmdTools = this._menuCmdBar.FlatGetItem(nFlatToolsIndex);
                PDFXEdit.IUIX_CmdMenu cmdToolsMenu = cmdTools.SubMenu;
                #region CUSTOM ChangeAnnotsContents
                if (isMenuInsertSeparator == true) cmdToolsMenu.InsertSeparator();
                int nCmdCustomMenu = pdfCtl.Inst.Str2ID(_PDFXE_CNODE_CMD_CUSTOM_NumberCircleStamp_);

                /*
                PDFXEdit.IUIX_Cmd cmdCustomMenu = this._uixInst.CmdManager.Cmds.AddNew2(nCmdCustomMenu, 0, cmdHandler);
                cmdCustomMenu.Title = "번호 스탬프 (원형)";//"Change Comment Contents";
                cmdCustomMenu.ShortTitle = "Number Stamp (Circle)";
                cmdCustomMenu.Tip = "번호 스탬프 (원형) - Number Stamp (Circle)";
                
                cmdToolsMenu.InsertItem2(nCmdCustomMenu);
                */
                //cmdToolsMenu.InsertSeparator();
                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
        }
        public int GetHTECustomTools_NumberCircleStampCollection()
        {
            return HxPDFXE.FindStampCollectionByName(_stampManager, _CUSTOM_NUMBER_CIRCLE_CATEGORY_NAME_PREFIX_);
        }
        public void SetClearHTECustomTools_NumberCircleStampCollection(bool isShowErrorMessage = false)
        {
            try
            {
                RemoveStampCollectionByName(this._stampManager, _CUSTOM_NUMBER_CIRCLE_CATEGORY_NAME_PREFIX_, isShowErrorMessage);
            }
            catch (Exception ex)
            {
                if (isShowErrorMessage == true)
                {
                    MessageBox.Show(ex.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SetAppendPlayHTECustomTools_NumberCircleStamp()
        {

        }

        public void SetAppendHTECustomTools_NumberCircleStamp(HxCustomNumberCircleRec input, PXC_Rect stampRect, string author = null, string basePath = null)
        {
            Bitmap bitmap = HxBitmap.CreateBitmapCustomNumberCircle(input);
            this.SetAppendHTECustomTools_NumberCircleStamp(input.TextValue, bitmap, stampRect, author, basePath);
        }
        public void SetAppendHTECustomTools_NumberCircleStamp(string stampValue, Bitmap stampBitmap, PXC_Rect stampRect, string author = null, string basePath = null)
        {
            if (stampBitmap == null || stampBitmap.Width == 0 || stampBitmap.Height == 0) return;
            if (pdfCtl == null || (pdfCtl != null && pdfCtl.Frame == null)) return;

            if (author.IsNullOrWhiteSpaceEx() == true) author = Environment.UserName;
            if (basePath.IsNullOrWhiteSpaceEx() == true) basePath = HxUtils.AppBaseDir;

            string strTempDirPath = Path.Combine(basePath, "Temp");
            HxFile.DirectoryCreate(strTempDirPath);
            if (HxFile.DirectoryExists(strTempDirPath) != true) return;

            string strStampValue = stampValue;
            string strStampFileOnly = $"{strStampValue.PadLeftEx(5, '0')}.{DateTime.Now.ToDateTimeStringDefaultFormatBEx()}";
            string strStampFileName = $"{strStampFileOnly}_{HxString.GetRandomString()}.png";
            string strStampFullName = Path.Combine(strTempDirPath, strStampFileName);
            strStampFullName = HxFile.GetFileUniquePath(strStampFullName);
            stampBitmap.Save(strStampFullName, System.Drawing.Imaging.ImageFormat.Png);
            if (HxFile.FileExists(strStampFullName) != true || HxFile.GetFileSize(strStampFullName) <= 0) return;

            IAFS_Name name = this._afsInst.DefaultFileSys.StringToName(strStampFullName);
            int openFileFlags = (int)(AFS_OpenFileFlags.AFS_OpenFile_Read | AFS_OpenFileFlags.AFS_OpenFile_ShareRead);
            IAFS_File destFile = this._afsInst.DefaultFileSys.OpenFile(name, openFileFlags);
            int iCustomNumberCircleStamp = GetHTECustomTools_NumberCircleStampCollection();
            IPXC_StampsCollection sc = null;
            if (_stampManager.Count > 0 && _stampManager.Count > iCustomNumberCircleStamp && iCustomNumberCircleStamp >= 0)
            {
                sc = _stampManager[(uint)iCustomNumberCircleStamp];
            }
            if (sc == null) sc = _stampManager.CreateEmptyCollection(_CUSTOM_NUMBER_CIRCLE_CATEGORY_NAME_PREFIX_);

            IPXC_StampInfo si = sc.AddStamp(destFile, $"{strStampFileOnly}");
            IPXC_Pages pages = Doc.CoreDoc.Pages;
            IPXC_Page page = pages[0];
            PXC_Rect rcPB = page.get_Box(PXC_BoxType.PBox_PageBox);
            //Creating stamp annotation
            si.Title = strStampFileName;

            uint nStamp = this._pxsInst.StrToAtom("Stamp");
            double nHeight = 0;
            double nWidth = 0;
            si.GetSize(out nWidth, out nHeight);
            //Increasing width and height by 20
            PXC_Rect rc; //Annotation rectangle
            rc.left = 0;
            rc.right = nWidth;
            rc.top = rcPB.top;
            rc.bottom = rc.top - nHeight;
            IPXC_Annotation annot = page.InsertNewAnnot(nStamp, ref rc, 0);
            //firstPage.InsertNewAnnot()
            IPXC_AnnotData_Stamp stampData = (IPXC_AnnotData_Stamp)annot.Data;
            stampData.set_BBox(rc); //Stamp rectangle boundaries
            stampData.SetStampName(si.ID);
            stampData.Subject = _CUSTOM_NUMBER_CIRCLE_CATEGORY_NAME_PREFIX_;
            stampData.Title = $"{author}";
            //stampData.RichContent = edtNumberStampInputValue.Value.ToStringEx();
            //stampData.Intent = stampValue;
            stampData.Rotation = page.Rotation;
            annot.Data = stampData;

            int nID = Inst.Str2ID("op.annots.addNew", false);
            IOperation pOp = Inst.CreateOp(nID);
            ICabNode input = pOp.Params.Root["Input"];
            input.Add().v = annot;
            pOp.Do();
            //int iMyStamp = stampManager.FindStamp(si.ID);
            //sc.RemoveStamp()

            //int iMyStampsCollection = stampManager.FindCollection(sc.ID);
            //stampManager.RemoveCollection(iMyStampsCollection.ToUIntEx());
            //
            //HxFile.FileDelete(sPath, false);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(page);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pages);
        }

        public enum HxHTECustomToolsNumberRectSizeType
        {
            None,
            Small,
            Middle,
            Large
        }
        public struct HxPDFXE_DrawingColorToIColor
        {
            public float nR;
            public float nG;
            public float nB;
        }

        public IColor GetIColorRGBType(Color color, out float nR, out float nG, out float nB)
        {
            IColor Result = null;
            nR = 0;
            nG = 0;
            nB = 0;
            if (pdfCtl == null || pdfCtl.Inst == null) return Result;
            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            if (auxInst == null) return Result;

            Result = auxInst.CreateColor(ColorType.ColorType_RGB);

            //float nR, nG, nB;
            _ = float.TryParse(color.R.ToString(), out nR);
            _ = float.TryParse(color.G.ToString(), out nG);
            _ = float.TryParse(color.B.ToString(), out nB);

            nR = nR / 255f;
            nG = nG / 255f;
            nB = nB / 255f;

            Result.SetRGB(nR, nG, nB);
            //Result.GetRGB(out nR, out nG, out nB);

            return Result;
        }
        public bool? SetAppendHTECustomTools_NumberLeaderLine(PXC_Point startPoint, PXC_Point endPoint, bool isArrowUse = false)
        {
            return null;
        }
        private void DoStrToAtom()
        {
            PDFXEdit.IPXS_Inst pSInt = Inst?.GetExtension("PXS") as PDFXEdit.IPXS_Inst;
            if (pSInt != null)
            {
                uint nTextBox = pSInt.StrToAtom("FreeText");
                uint nCaret = pSInt.StrToAtom("Caret");
                uint nLine = pSInt.StrToAtom("Line");
                uint nMarkup = pSInt.StrToAtom("Markup");
                uint nPoly = pSInt.StrToAtom("Poly");
                uint nPopup = pSInt.StrToAtom("Popup");
                uint nSquareCircle = pSInt.StrToAtom("SquareCircle");
                uint nText = pSInt.StrToAtom("Text");
                uint nTextMarkup = pSInt.StrToAtom("TextMarkup");
                uint nInk = pSInt.StrToAtom("Ink");
                uint nFileAttachmentAtom = pSInt.StrToAtom("FileAttachment");
                uint nRichMediaAtom = pSInt.StrToAtom("RichMedia");
                uint nSoundAtom = pSInt.StrToAtom("Sound");
                uint nWatermarkAtom = pSInt.StrToAtom("Watermark");
                uint nWidgetAtom = pSInt.StrToAtom("Widget");
                uint nStampAtom = pSInt.StrToAtom("Stamp");
                uint n3DAtom = pSInt.StrToAtom("3D");
                uint nRedaction = pSInt.StrToAtom("Redaction");
                uint nLink = pSInt.StrToAtom("Link");
            }
        }

        public IPXC_Annotation SetAppendHTECustomTools_NumberTypeAnnot(string inputNumberValue, HxCustomNumberAnnotType customAnnotType, ref PXC_Rect rcOut, HxHTECustomToolsNumberRectSizeType rectOptionSize, Color boardColor, Color backgroundColor, Color textColor, Font textFont = null, int nDivisionUse = 0, PXC_Point startPoint = default, PXC_Point endPoint = default, bool? isLeaderLineArrowUse = null, bool? isBorderUse = null, string author = null, bool IsThrowException = false)
        {
            IPXC_Annotation Result = null;
            //string inputNumberPrefix, string inputNumberSuffix, bool isTextNewLine = false
            bool? bSuccess = null;

            if (pdfCtl == null) return Result;
            IPXV_Document doc = pdfCtl.Doc;
            if (doc == null) return Result;
            PXV_Inst pxvInst = pdfCtl.Inst;
            if (pxvInst == null) return Result;
            IPXV_PagesLayoutManager activePageLayout = Inst.ActiveDocView.PagesView.Layout;
            if (activePageLayout == null) return Result;
            IPXC_Pages pages = doc.CoreDoc.Pages;
            if (pages == null) return Result;
            PDFXEdit.IPXC_Page page = Inst.ActiveDoc.CoreDoc.Pages[activePageLayout.CurrentPage];
            if (page == null) return Result;

            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            IPXS_Inst pxsInst = (IPXS_Inst)pxvInst.GetExtension("PXS");

            bSuccess = false;


            IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
            float nR, nG, nB;

            //IPXC_Page firstPage = pages[0];
            PXC_Rect rcPage = page.get_Box(PXC_BoxType.PBox_PageBox);

            int nID = pxvInst.Str2ID("op.annots.addNew", false);
            IOperation pOp = pxvInst.CreateOp(nID);
            ICabNode input = pOp.Params.Root["Input"];

            PXC_Rect rcInput = rcOut;

            IPXC_Annotation annotFreeText = null;
            IPXC_Annotation annotCircle = null;
            IPXC_Annotation annotSquare = null;
            IPXC_Annotation annotDivisionLine1 = null;
            IPXC_Annotation annotDivisionLine2 = null;
            IPXC_Annotation annotPolygon = null;
            IPXC_Annotation annotLeaderLine = null;

            uint nFreeText = pxsInst.StrToAtom("FreeText");
            uint nCircle = pxsInst.StrToAtom("Circle");
            uint nSquire = pxsInst.StrToAtom("Square");
            uint nLine = pxsInst.StrToAtom("Line");
            uint nPolygon = pxsInst.StrToAtom("Polygon");
            //uint nSquareCircle = pxsInst.StrToAtom("SquareCircle");

            int nSelID = pdfCtl.Inst.Str2ID("selection.annots", false);
            try
            {
                if (isLeaderLineArrowUse != null)
                {
                    if (endPoint.x > 0 && endPoint.y > 0)
                    {
                        rcOut.left = endPoint.x;
                        rcOut.right = endPoint.x;
                        rcOut.top = endPoint.y;
                        rcOut.bottom = endPoint.y;
                    }
                }

                #region 색상 및 크기 등 사전 정의
                bool bDrawBorderUse = false;
                if (isBorderUse == null && (boardColor == Color.Empty || boardColor == Color.Transparent))
                {
                    bDrawBorderUse = false;
                    boardColor = Color.White;
                }
                else if(isBorderUse == true)
                {
                    bDrawBorderUse = true;
                }
                
                if (bDrawBorderUse == true && (boardColor == null || boardColor == Color.Empty) )
                {
                    bDrawBorderUse = false;
                    boardColor = Color.White;
                }
                bool bDrawBackgroundColorUse = true;
                if (backgroundColor == null || backgroundColor == Color.Empty || backgroundColor == Color.Transparent)
                {
                    bDrawBackgroundColorUse = false;
                    backgroundColor = Color.White;
                }
                if (textFont == null)
                {
                    //textFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
                    textFont = new Font("Arial", 10, FontStyle.Regular);
                }
                if (author == null)
                {
                    author = Environment.UserName;
                }

                double nCX = rcOut.left + ((rcOut.right - rcOut.left) / 2.0);
                double nCY = rcOut.bottom + ((rcOut.top - rcOut.bottom) / 2.0);

                //PXC_Rect rcOut = new PXC_Rect();


                if (rcOut.left <= 0 && rcOut.right <= 0 && rcOut.top <= 0 && rcOut.bottom <= 0)
                {
                    nCX = (rcPage.right - rcPage.left) / 2.0;
                    nCY = (rcPage.top - rcPage.bottom) / 2.0;

                    rcOut.left = nCX - 50;
                    rcOut.bottom = nCY - 50;
                    rcOut.right = nCX + 50;
                    rcOut.top = nCY + 50;
                }
                //else
                //{
                //    rcOut.right = rcOut.left;
                //    rcOut.top = rcOut.bottom;
                //}

                //rcOut.right = rcOut.left;
                //rcOut.top = rcOut.bottom;

                //IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);

                PXC_AnnotBorder border = new PXC_AnnotBorder();
                border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                double nBorderRound = 10;
                float nBorderWidth = 0.5f;
                if (bDrawBorderUse == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Small)
                {
                    nBorderRound = 7;
                    nBorderWidth = 0.5f;
                }
                else if (bDrawBorderUse == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Middle)
                {
                    nBorderRound = 10;
                    nBorderWidth = 0.5f;
                }
                else if (bDrawBorderUse == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Large)
                {
                    nBorderRound = 14;
                    nBorderWidth = 0.5f;
                }
                else
                {
                    nBorderRound = 10;
                    nBorderWidth = 0f;
                }

                rcOut.left = nCX - nBorderRound;
                rcOut.bottom = nCY - nBorderRound;
                rcOut.right = nCX + nBorderRound;
                rcOut.top = nCY + nBorderRound;
                border.nWidth = nBorderWidth;
                #endregion //색상 및 크기 등 사전 정의

                #region Leader Line - 지시선 그리기
                if (isLeaderLineArrowUse != null && startPoint.x > 0 && endPoint.y > 0) // && customAnnotType != HxCustomNumberAnnotType.Text
                {
                    PXC_Rect rcLeaderLine = rcOut;
                    rcLeaderLine.left = startPoint.x;
                    rcLeaderLine.right = endPoint.x;
                    rcLeaderLine.top = startPoint.y;
                    rcLeaderLine.bottom = endPoint.y;

                    //color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                    annotLeaderLine = unchecked(page.InsertNewAnnot(nLine, ref rcLeaderLine));
                    IPXC_AnnotData_Line adataLeaderLine = annotLeaderLine.Data as IPXC_AnnotData_Line;
                    adataLeaderLine.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_LeaderLine_;
                    adataLeaderLine.Title = author;

                    if (backgroundColor != Color.Empty && backgroundColor != Color.Transparent)
                    {
                        color = GetIColorRGBType(backgroundColor, out nR, out nG, out nB);
                        adataLeaderLine.FColor = color;
                    }

                    PXC_AnnotBorder borderLeaderLine = new PXC_AnnotBorder();
                    //boardColor = Color.Red;
                    if (boardColor != Color.Empty && boardColor != Color.Transparent)
                    {   //지시선은 무조건 나오게
                        color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                        borderLeaderLine.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                        borderLeaderLine.nWidth = 0.5f;
                    }
                    adataLeaderLine.SColor = color;
                    adataLeaderLine.set_Border(borderLeaderLine);

                    adataLeaderLine.SetLinePoints(ref startPoint, ref endPoint);
                    if (isLeaderLineArrowUse == true)
                    {
                        adataLeaderLine.SetLineEndings(PXC_AnnotLineEndingStyle.LE_OpenArrow, PXC_AnnotLineEndingStyle.LE_None);
                    }
                    else
                    {
                        adataLeaderLine.SetLineEndings(PXC_AnnotLineEndingStyle.LE_None, PXC_AnnotLineEndingStyle.LE_None);
                    }
                    //adataLeaderLine.Rotation = page.Rotation;
                    annotLeaderLine.Data = adataLeaderLine;
                    input.Add().v = annotLeaderLine;
                }
                #endregion //지시선 그리기

                #region Polygon(Triangle/Hexagon) - 심볼그리기
                if (customAnnotType == HxCustomNumberAnnotType.Triangle || customAnnotType == HxCustomNumberAnnotType.Hexagon)
                {
                    annotPolygon = unchecked(page.InsertNewAnnot(nPolygon, ref rcOut));
                    IPXC_AnnotData_Poly adataPolygon = annotPolygon.Data as IPXC_AnnotData_Poly;
                    //adataSquareCircle = annotSquareCircle.Data as IPXC_AnnotData_SquareCircle;
                    //adataSquareCircle.Subject = edtNumberStampInputValue.Value.ToStringEx();
                    adataPolygon.Title = author;

                    int nPloygonPoint = 0;
                    IPXC_PolygonSrcF poly = adataPolygon.Vertices;

                    double rPoint = 10;
                    double aPoint = -90;

                    switch (customAnnotType)
                    {
                        case HxCustomNumberAnnotType.Triangle:
                            adataPolygon.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_Triangle_;
                            nPloygonPoint = 3;
                            rPoint = 15;
                            
                            aPoint = -90;
                            break;
                        case HxCustomNumberAnnotType.Hexagon:
                            adataPolygon.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_Hexagon_;
                            nPloygonPoint = 6;
                            rPoint = 10;
                            aPoint = 60;
                            break;
                    }
                    poly.Clear();
                    for (int i = 0; i < nPloygonPoint; i++)
                    {
                        PXC_PointF pointF = new PXC_PointF();
                        pointF.x = (float)(nCX + rPoint * Math.Cos(aPoint * Math.PI / 180.0));
                        pointF.y = (float)(nCY - rPoint * Math.Sin(aPoint * Math.PI / 180.0));
                        aPoint += 360.0 / nPloygonPoint;
                        poly.Insert(ref pointF, 1, uint.MaxValue);
                    }
                    adataPolygon.Vertices = poly;
                    color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                    //adataSquareCircle.SColor.SetRGB(nR, nG, nB); //테두리 색상
                    adataPolygon.SColor = color;
                    adataPolygon.set_Border(border);
                    if (bDrawBackgroundColorUse == true)
                    {
                        color = GetIColorRGBType(backgroundColor, out nR, out nG, out nB);
                        //adataSquareCircle.FColor.SetRGB(nR, nG, nB); //배경 색상
                        adataPolygon.FColor = color;
                    }
                    //adataSquareCircle.Opacity = 1;
                    //Setting dashed border pattern
                    //border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
                    //border.nDashCount = 4; //Number of dashes
                    int iRotation = page.Rotation;
                    
                    if (page.Rotation == 90)
                    {
                        iRotation = 330;
                    }
                    else if (page.Rotation == 180)
                    {
                        iRotation = 180;
                    }
                    else if (page.Rotation == 270)
                    {
                        iRotation = 30;
                    }
                    adataPolygon.Rotation = iRotation;

                    annotPolygon.Data = adataPolygon;
                    input.Add().v = annotPolygon;

                }
                #endregion

                #region Squire/Diamond - 심볼그리기
                if (customAnnotType == HxCustomNumberAnnotType.Square || customAnnotType == HxCustomNumberAnnotType.Diamond || customAnnotType == HxCustomNumberAnnotType.SquareAndCircle)
                {
                    annotSquare = unchecked(page.InsertNewAnnot(nSquire, ref rcOut));
                    IPXC_AnnotData_SquareCircle adataSquare = annotSquare.Data as IPXC_AnnotData_SquareCircle;
                    //adataSquareCircle = annotSquareCircle.Data as IPXC_AnnotData_SquareCircle;
                    //adataSquareCircle.Subject = edtNumberStampInputValue.Value.ToStringEx();
                    adataSquare.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_Squire_;

                    adataSquare.Title = author;
                    color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                    //adataSquareCircle.SColor.SetRGB(nR, nG, nB); //테두리 색상
                    adataSquare.SColor = color;
                    adataSquare.set_Border(border);
                    if (bDrawBackgroundColorUse == true)
                    {
                        color = GetIColorRGBType(backgroundColor, out nR, out nG, out nB);
                        //adataSquareCircle.FColor.SetRGB(nR, nG, nB); //배경 색상
                        adataSquare.FColor = color;
                    }
                    //adataSquareCircle.Opacity = 1;
                    //Setting dashed border pattern
                    //border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
                    //border.nDashCount = 4; //Number of dashes
                    adataSquare.Rotation = page.Rotation;
                    if (customAnnotType == HxCustomNumberAnnotType.Diamond)
                    {
                        adataSquare.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_Diamond_;
                        int iRotation = page.Rotation + 45;
                        if (iRotation > 365)
                        {
                            iRotation = page.Rotation - 45;
                        }
                        adataSquare.Rotation = iRotation;
                    }
                    else if (customAnnotType == HxCustomNumberAnnotType.SquareAndCircle)
                    {
                        adataSquare.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_SquireAndCircle_;
                    }
                    annotSquare.Data = adataSquare;
                    input.Add().v = annotSquare;
                }
                #endregion

                #region Circle - 심볼그리기
                if (customAnnotType == HxCustomNumberAnnotType.Circle || customAnnotType == HxCustomNumberAnnotType.SquareAndCircle)
                {
                    annotCircle = unchecked(page.InsertNewAnnot(nCircle, ref rcOut));
                    IPXC_AnnotData_SquareCircle adataCircle = annotCircle.Data as IPXC_AnnotData_SquareCircle;
                    //adataSquareCircle = annotSquareCircle.Data as IPXC_AnnotData_SquareCircle;
                    //adataSquareCircle.Subject = edtNumberStampInputValue.Value.ToStringEx();
                    adataCircle.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_Circle_;
                    adataCircle.Title = author;
                    color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                    //adataSquareCircle.SColor.SetRGB(nR, nG, nB); //테두리 색상
                    adataCircle.SColor = color;
                    adataCircle.set_Border(border);
                    if (bDrawBackgroundColorUse == true)
                    {
                        color = GetIColorRGBType(backgroundColor, out nR, out nG, out nB);
                        //adataSquareCircle.FColor.SetRGB(nR, nG, nB); //배경 색상
                        adataCircle.FColor = color;
                    }
                    //adataSquareCircle.Opacity = 1;
                    //Setting dashed border pattern
                    //border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
                    //border.nDashCount = 4; //Number of dashes
                    adataCircle.Rotation = page.Rotation;
                    annotCircle.Data = adataCircle;
                    input.Add().v = annotCircle;

                }
                #endregion

                #region FreeText - 입력항목
                string strTextValue = inputNumberValue;
                string[] arrSourceLineStr = strTextValue.Split('\n');
                int nSourceNewLine = arrSourceLineStr.Length - 1;
                bool bSourceWithNewLine = nDivisionUse >= 1 ? true : strTextValue.Contains("\n");
                if (bSourceWithNewLine == true && nSourceNewLine <= 0)
                {
                    nSourceNewLine = 1;
                }
                string sTemp = strTextValue.RegexReplaceEx("\\n", string.Empty);
                int nSourceLength = sTemp.Length;

                #region 입력 글자폭 정의
                PXC_Rect rcFreeText = rcOut;
                PXC_Rect rcBlock = rcOut;
                PXC_Rect rcClip = rcPage;
                PXC_Rect rcTextBounds;

                IUIX_ParaFormat uixParaFmt = _uixInst.CreateParaFormat();
                //uixParaFmt.LineSpacing = 0.5;
                IPXC_ParaFormat pxcParaFmt = _pxcInst.CreateParaFormat();
                //pxcParaFmt.LineSpacing = 0.5;

                IPXC_ContentCreator contentCreator = pdfCtl.Doc.CoreDoc.CreateContentCreator();
                contentCreator.SetTextRenderMode(PXC_TextRenderingMode.TRM_Fill);
                uint nFlags = (uint)PXC_DrawTextFlags.DTF_NoWordWrap;
                int nTextLen = -1; // strTextValue.Length;
                IPXC_CharFormat pCharFmt = null;
                IPXC_ParaFormat pParaFmt = _pxcInst.CreateParaFormat();
                //pParaFmt.LineSpacing = 5;
                IPXC_DrawTextCallbacks pCallbacks = null;
                contentCreator.ShowTextBlock(strTextValue, ref rcBlock, ref rcClip, nFlags, nTextLen, pCharFmt, pParaFmt, pCallbacks, out rcTextBounds);
                //page.PlaceContent(contentCreator.Detach(), (UInt32)PXC_PlaceContentFlags.PlaceContent_After);
                #endregion

                //rcOut.top -= 20;

                double nFontSize = textFont.Size;
                //font.FontInfo.
                //annotFreeText = unchecked(page.InsertNewAnnot(nCircle, ref rcOut));

                switch (nSourceLength)
                {
                    case 1:
                    case 2:
                    case 3:
                        nFontSize = 8;
                        //if (bWithNewLine != true)
                        //{
                        //    rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 2.5;
                        //    rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 2.5;
                        //}
                        //nFontSize = 9;
                        if (bSourceWithNewLine != true)
                        {
                            rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 1.5;
                            rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 1.5;
                        }
                        else if (nSourceNewLine == 1)
                        {
                            //rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 4.5;
                            //rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 4.5;
                            nFontSize = 7;
                        }
                        else
                        {
                            nFontSize = 6;
                        }
                        break;
                    case 4:
                        //if (bWithNewLine != true)
                        //{
                        //    rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 1.5;
                        //    rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 1.5;
                        //}
                        nFontSize = 6;
                        if (bSourceWithNewLine != true)
                        {
                            rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 0.1;
                            rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 0.6;
                        }
                        else if (nSourceNewLine == 1)
                        {
                            rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 3.5;
                            rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 3.5;
                            nFontSize = 5.5;
                        }
                        else
                        {
                            nFontSize = 5;
                        }

                        break;
                    case 5:
                        nFontSize = 5;
                        //if (bWithNewLine != true)
                        //{
                        //    rcOut.top = rcOut.top - (nBorderRound / 2) + 0.7;
                        //    rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 0.7;
                        //}
                        //nFontSize = 6;
                        if (bSourceWithNewLine != true)
                        {
                            rcFreeText.top = rcFreeText.top - (nBorderRound / 2) - 0.5;
                            rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) + 0.5;
                        }
                        else if (nSourceNewLine <= 1)
                        {
                            rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 2.0;
                            rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 2.0;
                        }
                        else
                        {
                            nFontSize = 4.5;
                        }
                        break;
                    default:
                        nFontSize = 4;
                        //if (bWithNewLine != true)
                        //{
                        //    rcOut.top = rcOut.top - (nBorderRound / 2) + 0.7;
                        //    rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 0.7;
                        //}
                        //nFontSize = 6;
                        if (bSourceWithNewLine != true)
                        {
                            rcFreeText.top = rcFreeText.top - (nBorderRound / 2) - 1.0;
                            rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) + 1.0;
                        }
                        else if (nSourceNewLine == 1)
                        {
                            //rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 2.0;
                            //rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 2.0;
                            rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 1.5;
                            rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 1.5;
                        }
                        else
                        {
                            nFontSize = 4;
                        }
                        break;
                }

                /**
                if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Small)
                {
                    switch (strTextValue.Length)
                    {
                        case 1:
                        case 2:
                            if (bSourceWithNewLine != true)
                            {
                                rcFreeText.top = rcFreeText.top - 1.5;
                                rcFreeText.bottom = rcFreeText.bottom + 1.5;
                            }
                            nFontSize = 6.5;
                            break;
                        case 3:
                            if (bSourceWithNewLine != true)
                            {
                                rcFreeText.top = rcFreeText.top - 2;
                                rcFreeText.bottom = rcFreeText.bottom + 2;
                            }
                            nFontSize = 5.5;
                            break;
                        case 4:
                            if (bSourceWithNewLine != true)
                            {
                                rcFreeText.top = rcFreeText.top - 2.5;
                                rcFreeText.bottom = rcFreeText.bottom + 2.5;
                            }
                            nFontSize = 4.5;
                            break;
                        case 5:
                        default:
                            //if (isTextNewLine != true)
                            {
                                rcFreeText.top = rcFreeText.top - 3.5;
                                rcFreeText.bottom = rcFreeText.bottom + 3.5;
                            }
                            nFontSize = 3.5;
                            break;

                    }
                }
                else if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Middle)
                {
                    
                }
                else if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Large)
                {
                    switch (strTextValue.Length)
                    {
                        case 1:
                        case 2:
                        case 3:
                            if (bSourceWithNewLine != true)
                            {
                                rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 2;
                                rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 2;
                            }
                            nFontSize = 11;
                            break;
                        case 4:
                            if (bSourceWithNewLine != true)
                            {
                                rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 1;
                                rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 1;
                            }
                            nFontSize = 9;
                            break;
                        case 5:
                            if (bSourceWithNewLine != true)
                            {
                                rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 0.1;
                                rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 0.1;
                            }
                            nFontSize = 7;
                            break;
                    }
                }
                else
                {
                    if (bSourceWithNewLine != true)
                    {
                        rcFreeText.top = rcFreeText.top - (nBorderRound / 2) + 2;
                        rcFreeText.bottom = rcFreeText.bottom + (nBorderRound / 2) - 2;
                    }
                }
                **/

                PXC_TextJustification pcxTextAlign = PXC_TextJustification.TJ_Middle;

                if (customAnnotType == HxCustomNumberAnnotType.Triangle)
                {
                    if (page.Rotation == 180)
                    {
                        rcFreeText.top -= 5;
                        rcFreeText.bottom += 20;
                    }
                }
                else if (customAnnotType == HxCustomNumberAnnotType.Text)
                {
                    double nFreeTextWidth = (rcTextBounds.right - rcTextBounds.left);
                    if (nFreeTextWidth > 60)
                    {
                        nFreeTextWidth = nFreeTextWidth / 2;
                    }
                    double nFreeTextHeight = (rcTextBounds.top - rcTextBounds.bottom); // (nSourceNewLine + 1);/// ;
                                                                                       //double nCX = rcOut.left + ((rcOut.right - rcOut.left) / 2.0);


                    rcFreeText = rcOut;                                                                   //double nCY = rcOut.bottom + ((rcOut.top - rcOut.bottom) / 2.0);

                    int nMaxLengthbyLine = 0;
                    int nLineCount = 0;
                    foreach (string s in arrSourceLineStr)
                    {
                        if (nMaxLengthbyLine < s.Length)
                        {
                            nMaxLengthbyLine = s.Length;
                        }
                        nLineCount++;
                    }
                    double nWidth = nMaxLengthbyLine * (nFontSize) * 2.7f;
                    double nHeight = nLineCount * 10;

                    rcFreeText.left = nCX;
                    rcFreeText.bottom = nCY + nHeight;
                    rcFreeText.top = nCY;
                    rcFreeText.right = nCX + nWidth;

                    rcFreeText = rcTextBounds;
                    /*
                    rcFreeText.left = nCX;
                    rcFreeText.top = nCY + 5;
                    rcFreeText.right = nCX + nFreeTextWidth;
                    rcFreeText.bottom = nCY - nFreeTextHeight + 5;

                    if (page.Rotation == 90)
                    {
                        rcFreeText.left = nCX;
                        rcFreeText.top = nCY + 5;
                        rcFreeText.right = nCX + nFreeTextWidth;
                        rcFreeText.bottom = nCY - nFreeTextHeight + 5;
                    }
                    else if (page.Rotation == 180)
                    {
                        rcFreeText.left = nCX;
                        rcFreeText.top = nCY + 5;
                        rcFreeText.right = nCX + nFreeTextWidth;
                        rcFreeText.bottom = nCY - nFreeTextHeight + 5;
                    }
                    else if (page.Rotation == 270)
                    {
                        rcFreeText.left = nCX;
                        rcFreeText.top = nCY + 5;
                        rcFreeText.right = nCX + nFreeTextWidth;
                        rcFreeText.bottom = nCY - nFreeTextHeight + 5;
                    }
                    */
                    pcxTextAlign = PXC_TextJustification.TJ_Middle;
                    rcFreeText = rcOut;
                }

                annotFreeText = unchecked(page.InsertNewAnnot(nFreeText, ref rcFreeText));
                IPXC_AnnotData_FreeText adataFreeText = annotFreeText.Data as IPXC_AnnotData_FreeText;
                adataFreeText.Contents = strTextValue;
                adataFreeText.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_FreeText_;
                adataFreeText.Title = author;
                //adataFreeText.Rotation = pdfCtl.PagesViewRotation;
                //IPXC_Font font = doc.CoreDoc.CreateNewFont(fontValueLabel.Font.Name, (uint)PXC_CreateFontFlags.CreateFont_Monospaced, 10);
                IPXC_Font font = doc.CoreDoc.CreateNewFont(textFont.Name, (uint)PXC_CreateFontFlags.CreateFont_Serif, (uint)nFontSize);
                if (customAnnotType == HxCustomNumberAnnotType.Text)
                {
                    if (textFont.Italic == true)
                    {
                        font = doc.CoreDoc.CreateNewFont(textFont.Name, (uint)PXC_CreateFontFlags.CreateFont_Italic, textFont.Size.ToUIntEx());
                    }
                    else
                    {
                        font = doc.CoreDoc.CreateNewFont(textFont.Name, (uint)PXC_CreateFontFlags.CreateFont_Serif, textFont.Size.ToUIntEx());
                        //font.FontInfo.
                    }
                    nFontSize = textFont.Size;
                }

                //a.CalcTextSize(nFontSize, strTextValue, out double nWidth, out double nHeight, strTextValue.Length);
                adataFreeText.DefaultFont = font;
                adataFreeText.DefaultFontSize = nFontSize; //40;
                adataFreeText.DefaultTextAlign = (int)pcxTextAlign;
                //adataFreeText.style
                //pdfCtl.Inst.CommentStylesManager.
                //IPXS_PDFVariant pvCO = annotFreeText.PDFObject.Dict_Get("CL");
                //adataFreeText.defa
                //adataFreeText.DefaultFont
                //color.SetRGB(0.7f, 0.7f, 0.7f);
                color = GetIColorRGBType(textColor, out nR, out nG, out nB);
                //adataFreeText.DefaultTextColor.SetRGB(nR, nG, nB);
                adataFreeText.DefaultTextColor = color;
                //adataFreeText.Opacity = 0.5;
                //aFreeTextData.TextRotation = 90;
                border = new PXC_AnnotBorder();
                border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                if (customAnnotType == HxCustomNumberAnnotType.Text)
                {
                    if (bDrawBorderUse == true && boardColor != Color.Empty && boardColor != Color.Transparent)
                    {
                        color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                        border.nWidth = 0.5f;
                        adataFreeText.SColor = color;
                    }
                    if (backgroundColor != Color.Empty && backgroundColor != Color.Transparent)
                    {
                        color = GetIColorRGBType(backgroundColor, out nR, out nG, out nB);
                        adataFreeText.FColor = color;
                    }

                }
                else
                {
                    border.nWidth = 0.0f;
                }
                adataFreeText.set_Border(border);
                //adataFreeText.DefaultStyle = "R";
                //adataFreeText.ali = 1;
                //adataFreeText.DefaultTextAlign = 1;
                adataFreeText.Rotation = page.Rotation;
                annotFreeText.Data = adataFreeText;
                input.Add().v = annotFreeText;

                if (customAnnotType == HxCustomNumberAnnotType.Text && annotFreeText != null && strTextValue.Length > 3)
                {
                    //nSelID = pdfCtl.Inst.Str2ID("selection.annots", false);
                    PDFXEdit.IPXV_AnnotSelection itSel = (PDFXEdit.IPXV_AnnotSelection)pdfCtl.Doc.CreateStdSel((uint)nSelID);
                    itSel.Items.Insert(annotFreeText, uint.MaxValue);
                    pdfCtl.Doc.ActiveSel = itSel;
                    itSel.Show(true);
                    pdfCtl.Inst.ExecUICmd("cmd.tool.fitBoxByTextContent");
                    //rcFreeText = annotFreeText.get_Rect();
                    //endPoint.x = rcFreeText.left + ((rcFreeText.right - rcFreeText.left) / 2);
                    //endPoint.y = rcFreeText.bottom + ((rcFreeText.top - rcFreeText.bottom) / 2);

                    

                }

                #endregion //입력항목

                #region Division - 분리선 그리기
                if (nDivisionUse > 0 && customAnnotType != HxCustomNumberAnnotType.Text && customAnnotType != HxCustomNumberAnnotType.Triangle)
                {
                    double nLineCX = nCX;
                    double nLineCY = nCY;
                    if (customAnnotType == HxCustomNumberAnnotType.Diamond)
                    {
                        nLineCX = nCX - 3.5;
                    }
                    else if (customAnnotType == HxCustomNumberAnnotType.Triangle)
                    {
                        nLineCX = nCX + 1.5;
                        if (nDivisionUse > 1)
                        {
                            nLineCX = nCX + 1.1;
                        }
                    }
                    border = new PXC_AnnotBorder();
                    border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                    border.nWidth = 0.5f;
                    //PXC_Rect rcDivision = rcOut;

                    PXC_Point startDivisionPoint = new PXC_Point();
                    startDivisionPoint.x = nLineCX - nBorderRound;
                    startDivisionPoint.y = nLineCY;
                    if (nDivisionUse > 1)
                    {
                        startDivisionPoint.x = startDivisionPoint.x + border.nWidth;
                        startDivisionPoint.y = startDivisionPoint.y + 0.7f;
                        //border.nWidth = 0.5f;
                    }

                    PXC_Point endDivisionPiont = new PXC_Point();
                    endDivisionPiont.x = nCX + nBorderRound;
                    if (customAnnotType == HxCustomNumberAnnotType.Diamond)
                    {
                        endDivisionPiont.x = nCX + nBorderRound + 3.5;
                    }
                    else if (customAnnotType == HxCustomNumberAnnotType.Triangle)
                    {
                        endDivisionPiont.x = nCX + nBorderRound - 1.5;
                        if (nDivisionUse > 1)
                        {
                            endDivisionPiont.x = nCX + nBorderRound - 1.1;
                        }
                    }
                    endDivisionPiont.y = startDivisionPoint.y;
                    if (nDivisionUse > 1)
                    {
                        endDivisionPiont.x = endDivisionPiont.x - border.nWidth;
                    }
                    //if(page.Rotation != 0 && page.Rotation != 180)
                    //{
                    //    startPoint.x = nLineCY;
                    //    startPoint.y = nLineCX - nBorderRound;
                    //
                    //    endPiont.x = nCX + nBorderRound;
                    //    endPiont.y = startPoint.y;
                    //}


                    annotDivisionLine1 = unchecked(page.InsertNewAnnot(nLine, ref rcOut));
                    IPXC_AnnotData_Line adataDivisionLine = annotDivisionLine1.Data as IPXC_AnnotData_Line;
                    adataDivisionLine.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_DivisionLine_;
                    adataDivisionLine.Title = author;

                    color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                    //adataSquareCircle.SColor.SetRGB(nR, nG, nB); //테두리 색상
                    adataDivisionLine.SColor = color;
                    adataDivisionLine.set_Border(border);
                    adataDivisionLine.FColor = color;
                    adataDivisionLine.SetLinePoints(ref startDivisionPoint, ref endDivisionPiont);
                    adataDivisionLine.Rotation = page.Rotation;
                    annotDivisionLine1.Data = adataDivisionLine;
                    input.Add().v = annotDivisionLine1;
                    //

                    if (nDivisionUse > 1)
                    {
                        startDivisionPoint.y = startDivisionPoint.y - 1.4f; //1.4f
                        endDivisionPiont.y = startDivisionPoint.y;

                        annotDivisionLine2 = unchecked(page.InsertNewAnnot(nLine, ref rcOut));
                        adataDivisionLine = annotDivisionLine2.Data as IPXC_AnnotData_Line;
                        adataDivisionLine.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_DivisionLine_;
                        adataDivisionLine.Title = author;
                        color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                        //adataSquareCircle.SColor.SetRGB(nR, nG, nB); //테두리 색상
                        adataDivisionLine.SColor = color;
                        adataDivisionLine.set_Border(border);
                        adataDivisionLine.FColor = color;
                        if (customAnnotType == HxCustomNumberAnnotType.Triangle)
                        {
                            startDivisionPoint.x = startDivisionPoint.x - 0.6;
                            endDivisionPiont.x = endDivisionPiont.x + 0.6;
                        }
                        adataDivisionLine.SetLinePoints(ref startDivisionPoint, ref endDivisionPiont);
                        adataDivisionLine.Rotation = page.Rotation;
                        annotDivisionLine2.Data = adataDivisionLine;
                        input.Add().v = annotDivisionLine2;


                    }
                }
                #endregion
                
                //page.AddAnnotToGroup(annotSquareCircle, annotFreeText);

                //annotSquareCircle.name

                if (annotLeaderLine != null)
                {
                    PDFXEdit.IPXV_AnnotSelection itSel = (PDFXEdit.IPXV_AnnotSelection)pdfCtl.Doc.CreateStdSel((uint)nSelID);
                    itSel.Clear();
                    itSel.Items.Insert(annotLeaderLine, uint.MaxValue);
                    pdfCtl.Doc.ActiveSel = itSel;
                    itSel.Show(true);
                    pdfCtl.Inst.ExecUICmd("cmd.order.background");

                    //IPXC_AnnotData_FreeText aFreeTextData = annotFreeText.Data as IPXC_AnnotData_FreeText;
                    PXC_Rect rcTempText = annotFreeText.get_Rect();
                    
                    IPXC_AnnotData_Line aLeaderLine = annotLeaderLine.Data as IPXC_AnnotData_Line;
                    //PXC_Rect rcTempLine = annotLeaderLine.get_Rect();
                    aLeaderLine.GetLinePoints(out PXC_Point ptTempLineStartPoint, out PXC_Point ptTempLineEndPoint);

                    double nTempCX = rcTempText.left + ((rcTempText.right - rcTempText.left) / 2.0);
                    double nTempCY = rcTempText.bottom + ((rcTempText.top - rcTempText.bottom) / 2.0);

                    ptTempLineEndPoint.x = nTempCX;
                    ptTempLineEndPoint.y = nTempCY;
                    aLeaderLine.SetLinePoints(ref ptTempLineStartPoint, ref ptTempLineEndPoint);
                    annotLeaderLine.Data = aLeaderLine;
                }

                #region ANNOTS - 그룹핑
                if (Result == null && annotFreeText != null)
                {
                    if (annotPolygon != null) Result = annotPolygon;
                    else if (annotSquare != null) Result = annotSquare;
                    else if (annotCircle != null) Result = annotCircle;
                    else if (annotFreeText != null) Result = annotFreeText;
                    else if (annotLeaderLine != null) Result = annotLeaderLine;
                }
                if (Result != null)
                {
                    if (annotFreeText != null && annotFreeText != Result) page.AddAnnotToGroup(annotFreeText, Result);

                    if (annotPolygon != null && annotPolygon != Result) page.AddAnnotToGroup(annotPolygon, Result);
                    if (annotSquare != null && annotSquare != Result) page.AddAnnotToGroup(annotSquare, Result);
                    if (annotCircle != null && annotCircle != Result) page.AddAnnotToGroup(annotCircle, Result);
                    if (annotLeaderLine != null && annotLeaderLine != Result) page.AddAnnotToGroup(annotLeaderLine, Result);

                    if (annotDivisionLine1 != null && annotDivisionLine1 != Result) page.AddAnnotToGroup(annotDivisionLine1, Result);
                    if (annotDivisionLine2 != null && annotDivisionLine2 != Result) page.AddAnnotToGroup(annotDivisionLine2, Result);
                }
                #endregion
            }
            catch (Exception ex)
            {
                bSuccess = false;
                //MessageBox.Show(ex.Message);
                Debug.WriteLine(ex);
                if (IsThrowException == true) throw ex;
            }
            finally
            {
                if (input.Count > 0)
                {
                    pOp.Do();
                    bSuccess = true;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(page);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pages);

            }
            if (bSuccess == true)
            {
                return Result;
            }
            else
            {
                return null;
            }
        }

        public void SetSelectionAnnots(IPXC_Annotation annot, bool bAnimated = false, bool bEnsureVisible = false)
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

            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            IPXS_Inst pxsInst = (IPXS_Inst)pxvInst.GetExtension("PXS");

            int nSelID = pdfCtl.Inst.Str2ID("selection.annots", false);
            PDFXEdit.IPXV_AnnotSelection itSel = (PDFXEdit.IPXV_AnnotSelection)pdfCtl.Doc.CreateStdSel((uint)nSelID);
            //IPXC_Annotation annot =Result.
            //IPXC_Annotation
            itSel.Items.Insert(annot, uint.MaxValue);
            pdfCtl.Doc.ActiveSel = itSel;
            itSel.Show(true, bAnimated);
            if(bEnsureVisible == true)
            {
                itSel.EnsureVisible(true);
                itSel.Highlight();
            }
        }



        #region 나중에 정리할것~
        private IPXC_Annotation SetAppendHTECustomTools_NumberCaseAnnot2(string inputNumberValue, HxCustomNumberAnnotType customAnnotType, ref PXC_Rect rcOut, HxHTECustomToolsNumberRectSizeType rectOptionSize, Color boardColor, Color backgroundColor, Color textColor, Font textFont = null, int nDivisionUse = 0, PXC_Point startPoint = default, PXC_Point endPoint = default, bool? isLeaderLineArrowUse = null, bool? isBorderUse = null, string author = null, bool IsThrowException = false)
        {
            //, string inputNumberPrefix, string inputNumberSuffix, bool isTextNewLine = false
            IPXC_Annotation Result = null;
            if (pdfCtl == null) return Result;
            IPXV_Document doc = pdfCtl.Doc;
            if (doc == null) return Result;
            PXV_Inst pxvInst = pdfCtl.Inst;
            if (pxvInst == null) return Result;
            IPXV_PagesLayoutManager activePageLayout = Inst.ActiveDocView.PagesView.Layout;
            if (activePageLayout == null) return Result;
            IPXC_Pages pages = doc.CoreDoc.Pages;
            if (pages == null) return Result;
            PDFXEdit.IPXC_Page page = Inst.ActiveDoc.CoreDoc.Pages[activePageLayout.CurrentPage];
            if (page == null) return Result;
            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            IPXS_Inst pxsInst = (IPXS_Inst)pxvInst.GetExtension("PXS");

            Result = SetAppendHTECustomTools_NumberTypeAnnot(inputNumberValue, customAnnotType, ref rcOut, rectOptionSize, boardColor, backgroundColor, textColor, textFont, nDivisionUse, startPoint, endPoint, isLeaderLineArrowUse, isBorderUse, author, IsThrowException);
            
            pdfCtl.Inst.ExecUICmd("cmd.edit.deselect");
            return Result;
        }

        private IPXC_Annotation SetAppendHTECustomTools_LeaderLineArrowAnnot(PXC_Point startPoint, PXC_Point endPoint, Color boardColor, bool isArrowLineUse = false, string author = null, bool IsThrowException = false)
        {
            IPXC_Annotation Result = null;
            bool? bSuccess = null;
            if (pdfCtl == null) return Result;
            IPXV_Document doc = pdfCtl.Doc;
            if (doc == null) return Result;
            PXV_Inst pxvInst = pdfCtl.Inst;
            if (pxvInst == null) return Result;
            IPXV_PagesLayoutManager activePageLayout = Inst.ActiveDocView.PagesView.Layout;
            if (activePageLayout == null) return Result;
            IPXC_Pages pages = doc.CoreDoc.Pages;
            if (pages == null) return Result;
            PDFXEdit.IPXC_Page page = Inst.ActiveDoc.CoreDoc.Pages[activePageLayout.CurrentPage];
            if (page == null) return Result;
            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            IPXS_Inst pxsInst = (IPXS_Inst)pxvInst.GetExtension("PXS");

            double nLeaderLineCX = startPoint.x;// + ((rcOut.right - rcOut.left) / 2.0);
            double nLeaderLineCY = startPoint.y;// + ((rcOut.top - rcOut.bottom) / 2.0);

            PXC_Rect rcLeaderLine;
            rcLeaderLine.left = startPoint.x;
            rcLeaderLine.right = endPoint.x;
            rcLeaderLine.top = startPoint.y;
            rcLeaderLine.bottom = endPoint.y;

            uint nLine = pxsInst.StrToAtom("Line");

            PXC_Rect rcPage = page.get_Box(PXC_BoxType.PBox_PageBox);
            int nID = pxvInst.Str2ID("op.annots.addNew", false);
            IOperation pOp = pxvInst.CreateOp(nID);
            ICabNode input = pOp.Params.Root["Input"];
            try
            {
                PXC_AnnotBorder border = new PXC_AnnotBorder();
                border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                border.nWidth = 0.5f;

                Result = unchecked(page.InsertNewAnnot(nLine, ref rcLeaderLine));
                IPXC_AnnotData_Line adataDivisionLine = Result.Data as IPXC_AnnotData_Line;
                adataDivisionLine.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_DivisionLine_;
                adataDivisionLine.Title = author;

                IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
                float nR, nG, nB;
                color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                adataDivisionLine.SColor = color;
                adataDivisionLine.set_Border(border);
                adataDivisionLine.FColor = color;
                adataDivisionLine.SetLinePoints(ref startPoint, ref endPoint);
                adataDivisionLine.Rotation = page.Rotation;
                Result.Data = adataDivisionLine;
                input.Add().v = Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if(IsThrowException == true) throw ex;
            }
            finally
            {
                if (input.Count > 0)
                {
                    pOp.Do();
                    if(bSuccess == null) bSuccess = true;
                }
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(page);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pages);
            }
            return Result;
        }

        
        
        private bool? SetAppendHTECustomTools_NumberTextAnnot2(string inputNumberValue, ref PXC_Rect rcFirst, HxHTECustomToolsNumberRectSizeType rectOptionSize, Color boardColor, Color backgroundColor, Color textColor, Font textFont = null, int nDivisionUse = 0, string author = null, bool IsThrowException = false)
        {
            bool? Result = null;
            if (pdfCtl == null) return Result;
            IPXV_Document doc = pdfCtl.Doc;
            if (doc == null) return Result;
            PXV_Inst pxvInst = pdfCtl.Inst;
            if (pxvInst == null) return Result;
            IPXV_PagesLayoutManager activePageLayout = Inst.ActiveDocView.PagesView.Layout;
            if (activePageLayout == null) return Result;
            IPXC_Pages pages = doc.CoreDoc.Pages;
            if (pages == null) return Result;
            PDFXEdit.IPXC_Page page = Inst.ActiveDoc.CoreDoc.Pages[activePageLayout.CurrentPage];
            if (page == null) return Result;

            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            IPXS_Inst pxsInst = (IPXS_Inst)pxvInst.GetExtension("PXS");

            Result = false;
            return Result;
        }
        private bool? SetAppendHTECustomTools_NumberCircleAnnot(string inputNumberValue, ref PXC_Rect rcOut, HxHTECustomToolsNumberRectSizeType rectOptionSize, Color boardColor, Color backgroundColor, Color textColor, Font textFont = null, bool isDivisionUse = false, string author = null, bool IsThrowException = false)
        {
            bool? Result = null;
            if (pdfCtl == null) return Result;
            IPXV_Document doc = pdfCtl.Doc;
            if (doc == null) return Result;
            PXV_Inst pxvInst = pdfCtl.Inst;
            if (pxvInst == null) return Result;
            IPXV_PagesLayoutManager activePageLayout = Inst.ActiveDocView.PagesView.Layout;
            if (activePageLayout == null) return Result;
            IPXC_Pages pages = doc.CoreDoc.Pages;
            if (pages == null) return Result;
            PDFXEdit.IPXC_Page page = Inst.ActiveDoc.CoreDoc.Pages[activePageLayout.CurrentPage];
            if (page == null) return Result;
            
            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            IPXS_Inst pxsInst = (IPXS_Inst)pxvInst.GetExtension("PXS");

            Result = false;

            
            IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
            float nR, nG, nB;

            //IPXC_Page firstPage = pages[0];
            PXC_Rect rcPage = page.get_Box(PXC_BoxType.PBox_PageBox);

            int nID = pxvInst.Str2ID("op.annots.addNew", false);
            IOperation pOp = pxvInst.CreateOp(nID);
            ICabNode input = pOp.Params.Root["Input"];
            try
            {
                bool bUseDrawBorder = true;
                if (boardColor == null || boardColor == Color.Empty)
                {
                    bUseDrawBorder = false;
                    boardColor = Color.White;
                }
                bool bUseBackgroundColor = true;
                if (backgroundColor == null || backgroundColor == Color.Empty || backgroundColor == Color.Transparent)
                {
                    bUseBackgroundColor = false;
                    backgroundColor = Color.White;
                }
                if (textFont == null)
                {
                    //textFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
                    textFont = new Font("Arial", 10, FontStyle.Regular);
                }
                if (author == null)
                {
                    author = Environment.UserName;
                }

                double nCX = rcOut.left + ((rcOut.right - rcOut.left) / 2.0);
                double nCY = rcOut.bottom + ((rcOut.top - rcOut.bottom) / 2.0);

                //PXC_Rect rcOut = new PXC_Rect();


                if (rcOut.left <= 0 && rcOut.right <= 0 && rcOut.top <= 0 && rcOut.bottom <= 0)
                {
                    nCX = (rcPage.right - rcPage.left) / 2.0;
                    nCY = (rcPage.top - rcPage.bottom) / 2.0;

                    rcOut.left = nCX - 50;
                    rcOut.bottom = nCY - 50;
                    rcOut.right = nCX + 50;
                    rcOut.top = nCY + 50;
                }
                //else
                //{
                //    rcOut.right = rcOut.left;
                //    rcOut.top = rcOut.bottom;
                //}

                //rcOut.right = rcOut.left;
                //rcOut.top = rcOut.bottom;

                //IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);

                PXC_AnnotBorder border = new PXC_AnnotBorder();
                border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                double nBorderRound = 10;
                float nBorderWidth = 1.5f;
                if (bUseDrawBorder == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Small)
                {
                    nBorderRound = 7;
                    nBorderWidth = 1.0f;
                }
                else if (bUseDrawBorder == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Middle)
                {
                    nBorderRound = 10;
                    nBorderWidth = 1.0f;
                }
                else if (bUseDrawBorder == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Large)
                {
                    nBorderRound = 14;
                    nBorderWidth = 1.0f;
                }
                else
                {
                    nBorderRound = 10;
                    nBorderWidth = 0f;
                }

                rcOut.left = nCX - nBorderRound;
                rcOut.bottom = nCY - nBorderRound;
                rcOut.right = nCX + nBorderRound;
                rcOut.top = nCY + nBorderRound;
                border.nWidth = nBorderWidth;
                uint nCircle = pxsInst.StrToAtom("Circle");
                IPXC_Annotation annotSquareCircle = unchecked(page.InsertNewAnnot(nCircle, ref rcOut));
                IPXC_AnnotData_SquareCircle adataSquareCircle = annotSquareCircle.Data as IPXC_AnnotData_SquareCircle;
                adataSquareCircle = annotSquareCircle.Data as IPXC_AnnotData_SquareCircle;
                //adataSquareCircle.Subject = edtNumberStampInputValue.Value.ToStringEx();
                adataSquareCircle.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_Circle_;
                adataSquareCircle.Title = author;
                color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                //adataSquareCircle.SColor.SetRGB(nR, nG, nB); //테두리 색상
                adataSquareCircle.SColor = color;
                adataSquareCircle.set_Border(border);
                if (bUseBackgroundColor == true)
                {
                    color = GetIColorRGBType(backgroundColor, out nR, out nG, out nB);
                    //adataSquareCircle.FColor.SetRGB(nR, nG, nB); //배경 색상
                    adataSquareCircle.FColor = color;
                }
                //adataSquareCircle.Opacity = 1;
                //Setting dashed border pattern
                //border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
                //border.nDashCount = 4; //Number of dashes
                annotSquareCircle.Data = adataSquareCircle;
                input.Add().v = annotSquareCircle;

                uint nFreeText = pxsInst.StrToAtom("FreeText");
                //rcOut.top -= 20;
                IPXC_Annotation annotFreeText;
                double nFontSize = textFont.Size;
                IPXC_Font font = doc.CoreDoc.CreateNewFont(textFont.Name);

                //annotFreeText = unchecked(page.InsertNewAnnot(nCircle, ref rcOut));
                string strTextValue = inputNumberValue;

                if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Small)
                {
                    switch (strTextValue.Length)
                    {
                        case 5:
                            rcOut.top = rcOut.top - 3.5;
                            rcOut.bottom = rcOut.bottom + 3.5;
                            nFontSize = 3.5;
                            break;
                        case 4:
                            rcOut.top = rcOut.top - 2.5;
                            rcOut.bottom = rcOut.bottom + 2.5;
                            nFontSize = 4.5;
                            break;
                        case 3:
                            rcOut.top = rcOut.top - 2;
                            rcOut.bottom = rcOut.bottom + 2;
                            nFontSize = 5.5;
                            break;
                        case 2:
                        case 1:
                        default:
                            rcOut.top = rcOut.top - 1.5;
                            rcOut.bottom = rcOut.bottom + 1.5;
                            nFontSize = 6.5;
                            break;
                    }
                }
                else if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Middle)
                {
                    switch (strTextValue.Length)
                    {
                        case 5:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 0.5;
                            rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 0.5;
                            nFontSize = 6;
                            break;
                        case 4:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 1.5;
                            rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 1.5;
                            nFontSize = 7;
                            break;
                        case 3:
                        case 2:
                        case 1:
                        default:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 2.5;
                            rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 2.5;
                            nFontSize = 9;
                            break;
                    }
                }
                else if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Large)
                {
                    switch (strTextValue.Length)
                    {
                        case 5:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 0.1;
                            rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 0.1;
                            nFontSize = 9;
                            break;
                        case 4:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 1;
                            rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 1;
                            nFontSize = 10;
                            break;
                        case 3:
                        case 2:
                        case 1:
                        default:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 2;
                            rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 2;
                            nFontSize = 11;
                            break;
                    }
                }
                else
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 2;
                    rcOut.bottom = rcOut.bottom + (nBorderRound / 2) - 2;
                }
                /*
                else if (radNumberStampSizeM.Checked == true && strTextValue.Length >= 4 && nFontSize >= 8)
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 1;
                    nFontSize = 6.5;
                }
                else if (radNumberStampSizeL.Checked == true && strTextValue.Length >= 4 && nFontSize >= 12)
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 1;
                    nFontSize = 11;
                }
                else
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 2;
                }
                */

                annotFreeText = unchecked(page.InsertNewAnnot(nFreeText, ref rcOut));
                IPXC_AnnotData_FreeText adataFreeText = annotFreeText.Data as IPXC_AnnotData_FreeText;
                adataFreeText.Contents = strTextValue;
                adataFreeText.Subject = _CUSTOM_CUSTOM_NUMBER_SUBJECT_FreeText_;
                adataFreeText.Title = author;
                adataFreeText.Rotation = page.Rotation;
                //adataFreeText.Rotation = pdfCtl.PagesViewRotation;
                //IPXC_Font font = doc.CoreDoc.CreateNewFont(fontValueLabel.Font.Name, (uint)PXC_CreateFontFlags.CreateFont_Monospaced, 10);
                adataFreeText.DefaultFont = font;
                adataFreeText.DefaultFontSize = nFontSize; //40;
                //color.SetRGB(0.7f, 0.7f, 0.7f);
                color = GetIColorRGBType(textColor, out nR, out nG, out nB);
                //adataFreeText.DefaultTextColor.SetRGB(nR, nG, nB);
                adataFreeText.DefaultTextColor = color;
                //adataFreeText.Opacity = 0.5;
                //aFreeTextData.TextRotation = 90;
                border = new PXC_AnnotBorder();
                border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                border.nWidth = 0.0f;
                adataFreeText.set_Border(border);
                adataFreeText.DefaultTextAlign = 1;
                //adataFreeText.DefaultStyle = "R";
                //adataFreeText.ali = 1;
                //adataFreeText.DefaultTextAlign = 1;
                annotFreeText.Data = adataFreeText;
                input.Add().v = annotFreeText;


                //page.AddAnnotToGroup(annotSquareCircle, annotFreeText);
                page.AddAnnotToGroup(annotFreeText, annotSquareCircle);
                //annotSquareCircle.name
            }
            catch (Exception ex)
            {
                Result = false;
                //MessageBox.Show(ex.Message);
                Debug.WriteLine(ex);
                if (IsThrowException == true) throw ex;
            }
            finally
            {
                if (input.Count > 0)
                {
                    pOp.Do();
                    Result = true;
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(page);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pages);
            }
            return Result;
        }
        
        private bool? SetAppendHTECustomTools_NumberCallbackAnnot(string inputNumberValue, ref PXC_Rect rcOut, HxHTECustomToolsNumberRectSizeType rectOptionSize, Color boardColor, Color backgroundColor, Color textColor, Font textFont = null, string author = null, bool IsThrowException = false)
        {
            bool? Result = null;
            if (pdfCtl == null) return Result;
            IPXV_Document doc = pdfCtl.Doc;
            if (doc == null) return Result;
            PXV_Inst pxvInst = pdfCtl.Inst;
            if (pxvInst == null) return Result;
            IPXV_PagesLayoutManager activePageLayout = Inst.ActiveDocView.PagesView.Layout;
            if (activePageLayout == null) return Result;
            IPXC_Pages pages = doc.CoreDoc.Pages;
            if (pages == null) return Result;
            PDFXEdit.IPXC_Page page = Inst.ActiveDoc.CoreDoc.Pages[activePageLayout.CurrentPage];
            if (page == null) return Result;

            Result = false;
            const string _CUSTOM_SUBJECT_SquareCircle_ = "HTE.Number.SquareCircle";
            const string _CUSTOM_SUBJECT_FreeText_ = "HTE.Number.FreeText";


            IAUX_Inst auxInst = (IAUX_Inst)pdfCtl.Inst.GetExtension("AUX");
            IPXS_Inst pxsInst = (IPXS_Inst)pxvInst.GetExtension("PXS");
            IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
            float nR, nG, nB;

            //IPXC_Page firstPage = pages[0];
            PXC_Rect rcPage = page.get_Box(PXC_BoxType.PBox_PageBox);

            int nID = pxvInst.Str2ID("op.annots.addNew", false);
            IOperation pOp = pxvInst.CreateOp(nID);
            ICabNode input = pOp.Params.Root["Input"];
            try
            {
                bool bUseDrawBorder = true;
                if (boardColor == null || boardColor == Color.Empty)
                {
                    bUseDrawBorder = false;
                    boardColor = Color.White;
                }
                bool bUseBackgroundColor = true;
                if (backgroundColor == null || backgroundColor == Color.Empty || backgroundColor == Color.Transparent)
                {
                    bUseBackgroundColor = false;
                    backgroundColor = Color.White;
                }
                if (textFont == null)
                {
                    //textFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
                    textFont = new Font("Arial", 10, FontStyle.Regular);
                }
                if (author == null)
                {
                    author = Environment.UserName;
                }

                double nCX = rcOut.left + ((rcOut.right - rcOut.left) / 2.0);
                double nCY = rcOut.bottom + ((rcOut.top - rcOut.bottom) / 2.0);

                //PXC_Rect rcOut = new PXC_Rect();


                if (rcOut.left <= 0 && rcOut.right <= 0 && rcOut.top <= 0 && rcOut.bottom <= 0)
                {
                    nCX = (rcPage.right - rcPage.left) / 2.0;
                    nCY = (rcPage.top - rcPage.bottom) / 2.0;

                    rcOut.left = nCX - 50;
                    rcOut.bottom = nCY - 50;
                    rcOut.right = nCX + 50;
                    rcOut.top = nCY + 50;
                }
                //else
                //{
                //    rcOut.right = rcOut.left;
                //    rcOut.top = rcOut.bottom;
                //}

                //rcOut.right = rcOut.left;
                //rcOut.top = rcOut.bottom;

                //IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);

                PXC_AnnotBorder border = new PXC_AnnotBorder();
                border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                double nBorderRound = 10;
                float nBorderWidth = 1.5f;
                if (bUseDrawBorder == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Small)
                {
                    nBorderRound = 7;
                    nBorderWidth = 1.0f;
                }
                else if (bUseDrawBorder == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Middle)
                {
                    nBorderRound = 10;
                    nBorderWidth = 1.0f;
                }
                else if (bUseDrawBorder == true && rectOptionSize == HxHTECustomToolsNumberRectSizeType.Large)
                {
                    nBorderRound = 14;
                    nBorderWidth = 1.0f;
                }
                else
                {
                    nBorderRound = 10;
                    nBorderWidth = 0f;
                }

                rcOut.left = nCX - nBorderRound;
                rcOut.bottom = nCY - nBorderRound;
                rcOut.right = nCX + nBorderRound;
                rcOut.top = nCY + nBorderRound;
                border.nWidth = nBorderWidth;
                uint nCircle = pxsInst.StrToAtom("Circle");
                IPXC_Annotation annotSquareCircle = unchecked(page.InsertNewAnnot(nCircle, ref rcOut));
                IPXC_AnnotData_SquareCircle adataSquareCircle = annotSquareCircle.Data as IPXC_AnnotData_SquareCircle;
                adataSquareCircle = annotSquareCircle.Data as IPXC_AnnotData_SquareCircle;
                //adataSquareCircle.Subject = edtNumberStampInputValue.Value.ToStringEx();
                adataSquareCircle.Subject = _CUSTOM_SUBJECT_SquareCircle_;
                adataSquareCircle.Title = author;
                color = GetIColorRGBType(boardColor, out nR, out nG, out nB);
                //adataSquareCircle.SColor.SetRGB(nR, nG, nB); //테두리 색상
                adataSquareCircle.SColor = color;
                adataSquareCircle.set_Border(border);
                if (bUseBackgroundColor == true)
                {
                    color = GetIColorRGBType(backgroundColor, out nR, out nG, out nB);
                    //adataSquareCircle.FColor.SetRGB(nR, nG, nB); //배경 색상
                    adataSquareCircle.FColor = color;
                }
                //adataSquareCircle.Opacity = 1;
                //Setting dashed border pattern
                //border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
                //border.nDashCount = 4; //Number of dashes
                annotSquareCircle.Data = adataSquareCircle;
                input.Add().v = annotSquareCircle;

                uint nFreeText = pxsInst.StrToAtom("FreeText");
                //rcOut.top -= 20;
                IPXC_Annotation annotFreeText;
                double nFontSize = textFont.Size;
                IPXC_Font font = doc.CoreDoc.CreateNewFont(textFont.Name);

                //annotFreeText = unchecked(page.InsertNewAnnot(nCircle, ref rcOut));
                string strTextValue = inputNumberValue;

                if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Small)
                {
                    switch (strTextValue.Length)
                    {
                        case 5:
                            rcOut.top = rcOut.top - 3.5;
                            nFontSize = 3.5;
                            break;
                        case 4:
                            rcOut.top = rcOut.top - 2.5;
                            nFontSize = 4.5;
                            break;
                        case 3:
                            rcOut.top = rcOut.top - 2;
                            nFontSize = 5.5;
                            break;
                        case 2:
                        case 1:
                        default:
                            rcOut.top = rcOut.top - 1.5;
                            nFontSize = 6.5;
                            break;
                    }
                }
                else if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Middle)
                {
                    switch (strTextValue.Length)
                    {
                        case 5:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 0.5;
                            nFontSize = 6;
                            break;
                        case 4:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 1.5;
                            nFontSize = 7;
                            break;
                        case 3:
                        case 2:
                        case 1:
                        default:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 2.5;
                            nFontSize = 9;
                            break;
                    }
                }
                else if (rectOptionSize == HxHTECustomToolsNumberRectSizeType.Large)
                {
                    switch (strTextValue.Length)
                    {
                        case 5:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 0.1;
                            nFontSize = 9;
                            break;
                        case 4:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 1;
                            nFontSize = 10;
                            break;
                        case 3:
                        case 2:
                        case 1:
                        default:
                            rcOut.top = rcOut.top - (nBorderRound / 2) + 2;
                            nFontSize = 11;
                            break;
                    }
                }
                else
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 2;
                }
                /*
                else if (radNumberStampSizeM.Checked == true && strTextValue.Length >= 4 && nFontSize >= 8)
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 1;
                    nFontSize = 6.5;
                }
                else if (radNumberStampSizeL.Checked == true && strTextValue.Length >= 4 && nFontSize >= 12)
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 1;
                    nFontSize = 11;
                }
                else
                {
                    rcOut.top = rcOut.top - (nBorderRound / 2) + 2;
                }
                */

                annotFreeText = unchecked(page.InsertNewAnnot(nFreeText, ref rcOut));
                IPXC_AnnotData_FreeText adataFreeText = annotFreeText.Data as IPXC_AnnotData_FreeText;
                adataFreeText.Contents = strTextValue;
                adataFreeText.Subject = _CUSTOM_SUBJECT_FreeText_;
                adataFreeText.Title = author;

                //IPXC_Font font = doc.CoreDoc.CreateNewFont(fontValueLabel.Font.Name, (uint)PXC_CreateFontFlags.CreateFont_Monospaced, 10);

                adataFreeText.DefaultFont = font;
                adataFreeText.DefaultFontSize = nFontSize; //40;
                //color.SetRGB(0.7f, 0.7f, 0.7f);
                color = GetIColorRGBType(textColor, out nR, out nG, out nB);
                //adataFreeText.DefaultTextColor.SetRGB(nR, nG, nB);
                adataFreeText.DefaultTextColor = color;
                //adataFreeText.Opacity = 0.5;
                //aFreeTextData.TextRotation = 90;
                border = new PXC_AnnotBorder();
                border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
                border.nWidth = 0.0f;
                adataFreeText.set_Border(border);
                adataFreeText.DefaultTextAlign = 1;
                //adataFreeText.DefaultStyle = "R";
                //adataFreeText.ali = 1;
                //adataFreeText.DefaultTextAlign = 1;
                annotFreeText.Data = adataFreeText;
                input.Add().v = annotFreeText;


                page.AddAnnotToGroup(annotSquareCircle, annotFreeText);
                //page.AddAnnotToGroup(annotSquareCircle, annotFreeText);
                //annotSquareCircle.name
            }
            catch (Exception ex)
            {
                Result = false;
                //MessageBox.Show(ex.Message);
                Debug.WriteLine(ex);
                if (IsThrowException == true) throw ex;
            }
            finally
            {
                if (input.Count > 0)
                {
                    pOp.Do();
                    Result = true;
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(page);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pages);
            }
            return Result;
        }
        #endregion //나중에 정리할것~

        /*
        public void SetAppendHTECustomTools_NumberCircleStamp(Image stampImage, PXC_Rect stampPoint, string author = null, string basePath = null)
        {
            if (stampImage == null || stampImage.Width == 0 || stampImage.Height == 0) return;

            if (pdfCtl == null || (pdfCtl != null && pdfCtl.Frame == null)) return;
            if (author.IsNullOrWhiteSpaceEx() == true) author = Environment.UserName;
            if (basePath.IsNullOrWhiteSpaceEx() == true) basePath = HxUtils.AppBaseDir;

            string strTempDirPath = Path.Combine(basePath, "Temp");
            HxFile.DirectoryCreate(strTempDirPath);
            if (HxFile.DirectoryExists(strTempDirPath) != true) return;

            string strStampValue = edtNumberStampInputValue.Value.ToStringEx();
            string strStampFileOnly = $"{strStampValue.PadLeftEx(5, '0')}.{DateTime.Now.ToDateTimeStringDefaultFormatBEx()}";
            string strStampFileName = $"{strStampFileOnly}_{HxString.GetRandomString()}.png";
            string strStampFullName = Path.Combine(strTempDirPath, strStampFileName);
            strStampFullName = HxFile.GetFileUniquePath(strStampFullName);
            stampImage.Save(strStampFullName, System.Drawing.Imaging.ImageFormat.Png);
        }
        */
        public class PdfEditorCommandHandler : PDFXEdit.IUIX_CmdHandler
        {
            //출처 : https://gist.github.com/Polaringu/16bb29fe9f58c3a7a2a14b7cbe36f4e6
            public PDFXEdit.IPXV_Inst m_Inst = null;
            //public PDFXEdit.IUIX_CmdHandler m_CustomAboutHandler = null;
            //public PDFXEdit.IUIX_CmdHandler m_CustomPrintAllHandler = null;
            public PDFXEdit.IUIX_CmdHandler m_CustomCopyAnnotsToPagesHandler = null;
            public PDFXEdit.IUIX_CmdHandler m_CustomChangeAnnotsContentsHandler = null;
            public PDFXEdit.IUIX_CmdHandler m_CustomCustomNumberCircleStampHandler = null;
            //private readonly int m_nCustomAboutID = 0;
            //private readonly int m_nCustomPrintAllID = 0;
            private readonly int m_nCustomCopyAnnotsToPagesID = 0;
            private readonly int m_nCustomChangeAnnotsContentsID = 0;
            private readonly int m_nCustomCustomNumberCircleStampID = 0;
            public string inputAuthor { get; private set; }

            public PdfEditorCommandHandler(PDFXEdit.IPXV_Inst Inst, string author = null)
            {
                m_Inst = Inst;
                if (author.IsNullOrWhiteSpaceEx()) author = Environment.UserName;
                inputAuthor = author;
                //m_nCustomAboutID = m_Inst.Str2ID(Defs._PDFXE_CNODE_CMD_CUSTOM_ABOUT_);
                //m_nCustomPrintAllID = m_Inst.Str2ID(Defs._PDFXE_CNODE_CMD_CUSTOM_PRINTALL_);
                m_nCustomCopyAnnotsToPagesID = m_Inst.Str2ID(_PDFXE_CNODE_CMD_CUSTOM_CopyAnnotsToPages_);
                m_nCustomChangeAnnotsContentsID = m_Inst.Str2ID(_PDFXE_CNODE_CMD_CUSTOM_ChangeAnnotsContents_); //m_nCustomChangeAnnotsContentsID
                m_nCustomCustomNumberCircleStampID = m_Inst.Str2ID(_PDFXE_CNODE_CMD_CUSTOM_NumberCircleStamp_);
            }

            public void OnCreateNewCtl(PDFXEdit.IUIX_Cmd pCmd, PDFXEdit.IUIX_CmdBar pParent, out PDFXEdit.IUIX_Obj pCtl)
            {
                pCtl = null;
                try
                {
                    //if (pCmd.ID == m_nCustomAboutID)
                    //    m_CustomAboutHandler?.OnCreateNewCtl(pCmd, pParent, out pCtl);
                    //else if (pCmd.ID == m_nCustomPrintAllID)
                    //    m_CustomPrintAllHandler?.OnCreateNewCtl(pCmd, pParent, out pCtl);
                    //else 
                    if (pCmd.ID == m_nCustomCopyAnnotsToPagesID)
                        m_CustomCopyAnnotsToPagesHandler?.OnCreateNewCtl(pCmd, pParent, out pCtl);
                    else if (pCmd.ID == m_nCustomChangeAnnotsContentsID)
                        m_CustomChangeAnnotsContentsHandler?.OnCreateNewCtl(pCmd, pParent, out pCtl);
                    else if (pCmd.ID == m_nCustomCustomNumberCircleStampID)
                    {
                        m_CustomCustomNumberCircleStampHandler?.OnCreateNewCtl(pCmd, pParent, out pCtl);
                    }
                    else
                        pCtl = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw ex;
                }
            }

            public void OnGetCtlSizes(PDFXEdit.IUIX_CmdItem pItem, ref PDFXEdit.tagSIZE nSize, ref PDFXEdit.tagSIZE nMinSize, ref PDFXEdit.tagSIZE nMaxSize)
            {
                //if (pItem.Cmd.ID == m_nCustomAboutID)
                //    m_CustomAboutHandler.OnGetCtlSizes(pItem, nSize, nMinSize, nMaxSize);
                //else if (pItem.Cmd.ID == m_nCustomPrintAllID)
                //    m_CustomPrintAllHandler.OnGetCtlSizes(pItem, nSize, nMinSize, nMaxSize);
                //else 
                if (pItem.Cmd.ID == m_nCustomCopyAnnotsToPagesID)
                    m_CustomCopyAnnotsToPagesHandler.OnGetCtlSizes(pItem, nSize, nMinSize, nMaxSize);
                else if (pItem.Cmd.ID == m_nCustomCopyAnnotsToPagesID)
                    m_CustomChangeAnnotsContentsHandler.OnGetCtlSizes(pItem, nSize, nMinSize, nMaxSize);
            }

            public void OnGetItemState(PDFXEdit.IUIX_Cmd pCmd, PDFXEdit.IUIX_CmdItem pItem, PDFXEdit.IUIX_Obj pOwner, out int nState)
            {
                nState = (int)PDFXEdit.UIX_CmdItemState.UIX_CmdItemState_Unknown;
                try
                {
                    //if (pCmd.ID == m_nCustomAboutID && m_CustomAboutHandler != null)
                    //    m_CustomAboutHandler.OnGetItemState(pCmd, pItem, pOwner, out nState);
                    //else if (pCmd.ID == m_nCustomPrintAllID && m_CustomPrintAllHandler != null)
                    //    m_CustomPrintAllHandler.OnGetItemState(pCmd, pItem, pOwner, out nState);
                    //else 
                    if (pCmd.ID == m_nCustomCopyAnnotsToPagesID && m_CustomCopyAnnotsToPagesHandler != null)
                        m_CustomCopyAnnotsToPagesHandler.OnGetItemState(pCmd, pItem, pOwner, out nState);
                    else if (pCmd.ID == m_nCustomChangeAnnotsContentsID && m_CustomChangeAnnotsContentsHandler != null)
                        m_CustomChangeAnnotsContentsHandler.OnGetItemState(pCmd, pItem, pOwner, out nState);
                    else
                        nState = (int)PDFXEdit.UIX_CmdItemState.UIX_CmdItemState_Unknown;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw;
                }

            }

            public void OnGetItemSubMenu(PDFXEdit.IUIX_CmdItem pItem, out PDFXEdit.IUIX_CmdMenu pSubMenu)
            {
                //pSubMenu = pItem.SubMenu;
                //if (pItem.Cmd.ID == m_nCustomAboutID)
                //    m_CustomAboutHandler.OnGetItemSubMenu(pItem, out pSubMenu);
                //else if (pItem.Cmd.ID == m_nCustomPrintAllID)
                //    m_CustomPrintAllHandler.OnGetItemSubMenu(pItem, out pSubMenu);
                //else 
                if (pItem.Cmd.ID == m_nCustomCopyAnnotsToPagesID)
                    m_CustomCopyAnnotsToPagesHandler.OnGetItemSubMenu(pItem, out pSubMenu);
                else if (pItem.Cmd.ID == m_nCustomChangeAnnotsContentsID)
                    m_CustomChangeAnnotsContentsHandler.OnGetItemSubMenu(pItem, out pSubMenu);
                else
                    pSubMenu = pItem.SubMenu;
            }

            public void OnNotify(int nCode, PDFXEdit.IUIX_Cmd pCmd, PDFXEdit.IUIX_CmdItem pItem, PDFXEdit.IUIX_Obj pOwner, uint nNotifyData)
            {
                try
                {
                    //if (pCmd.ID == m_nCustomAboutID)
                    //{
                    //    UbAbout frm = new UbAbout();
                    //    frm.ShowDialog();

                    //    m_CustomAboutHandler?.OnNotify(nCode, pCmd, pItem, pOwner, nNotifyData);
                    //}
                    //else 
                    //                  if (pCmd.ID == m_nCustomPrintAllID)
                    //                  {

                    //                      if (m_Inst != null && m_Inst.DocCount > 0)
                    //                      {
                    //                          IString res = m_Inst.CreateString();
                    //                          string jsString = @"var ad = app.activeDocs;
                    //for (var i = 0; i < ad.length; i++)
                    //{
                    //  var pp = ad[i].getPrintParams();
                    //  // uncomment the next line to print without dialog for each document
                    //  // pp.interactive = pp.constants.interactionLevel.silent;
                    //  ad[i].print(pp);
                    //}";
                    //                          m_Inst.ExecuteJS(m_Inst.ActiveDoc, jsString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                    //                      }
                    //                      m_CustomPrintAllHandler?.OnNotify(nCode, pCmd, pItem, pOwner, nNotifyData);
                    //                  }
                    //                  else 
                    if (pCmd.ID == m_nCustomCopyAnnotsToPagesID)
                    {

                        if (m_Inst != null && m_Inst.DocCount > 0)
                        {
                            IString res = m_Inst.CreateString();
                            #region 주석 처리
                            /*
                            string jsString = @"//var cIconsPath = app.getPath('app', 'root');
//var iconPrintAll = util.iconStreamFromImage(cIconsPath + '/Resources/Icon-CopyAnnotsToPages_24x24.png');
//app.addToolButton({
//    cName: 'js.custom.CopyAnnotsToPages',
//    oIcon: iconPrintAll,
//    cExec: 'CopyAnnotsToPages();',
//    cTooltext: 'Copy Comment To Pages',
//    cEnable: true,
//    nPos: 0
//});

CopyAnnotsToPages();

function CopyAnnotsToPages()
{
    //참고: <https://forum.tracker-software.com/viewtopic.php?f=62&t=30092&p=118915&hilit=stamp+multiple+page#p118915>
    
    //Title: COPY ANNOTATION TO USER DEFINED PAGE RANGE
    //Author:  John Statler
    //Purpose: Prompt user to enter required page
    //         range to copy annotation to other pages, e.g. 1-5,7,9-20
    

    //Get user response

	var selAnnts = null;
	var nSelAnnts = 0;
	try {
		selAnnts = this.selectedAnnots;
		nSelAnnts = selAnnts.length;
		if (!selAnnts || !selAnnts)
		{
			app.alert('복제 할 주석을 선택하지 않았습니다!\n(You have not selected an annotation to duplicate)');
        }
	} catch (e) {
		app.alert('복제 할 주석을 선택하지 않았습니다!\n(You have not selected an annotation to duplicate)\n\n' + e);
	}

	if (selAnnts)
	{
		try {
			var annt = null;
			var props = null;


			//app.alert(nSelAnnts);

			annt = this.selectedAnnots[0];
			props = annt.getProps();
			if (props) {
				var pagesAll = '';
				annt = this.selectedAnnots[0];
				props = annt.getProps();

				var currPageIndex = props.page;
				var currPageNum = currPageIndex + 1;
				var totalPageNum = this.numPages;
				var totalPageIndex = totalPageNum - 1;
				//app.alert(currPageIndex + ' / ' + totalPageIndex);

				if (currPageIndex >= totalPageIndex) {
					app.alert('복제할 페이지와 마지막 페이지가 같습니다!(Source Page is Last Page Equals)');
				}
				else {
					var cResponse = app.response({
						cQuestion: '주석을 반복할 페이지를 입력하세요,\n(Enter in the pages where you wish to repeat the comment)\n' +
							'ex) 1,5,10-19,25\n\n' +
							//'All pages are chosen by default.\n\n' +
							//'For large documents it may take a second to do 8 pages.\n' +
							//'So 120 pages would take 15 seconds to finish.\n\n' +
							'세로 스탬프의 경우 먼저 문서의 방향과 스탬프의 방향을 먼저 확인후 적용 하세요.\n' +
							'(For vertical stamps, first rotate the document, apply stamp, then rotate back)',
						cTitle: 'Copy To Pages',
						cDefault: currPageNum + '-' + this.numPages,
						cLabel: 'Pages:'
					});
					if (cResponse == null) {
						//app.alert('입력 한 페이지가 없습니다.\n(No pages entered)');
					}
					else {
						var anntPage = this.pageNum + 1;
						if (cResponse == null) {
							app.alert('입력 한 페이지가 없습니다.\n(No pages entered)');
						}
						else {
							var d1 = new Date();
							var strInput = cResponse;
							var strChar;
							var arPrint = new Array(10);
							var arCount = 0;
							arPrint[arCount] = '';

							for (var i = 0; i < strInput.length; i++) {

								strChar = strInput.substr(i, 1);

								//Check character and form page group
								if (IsInteger(strChar) == 0) {
									arPrint[arCount] = arPrint[arCount] + strChar;
								}

								if (IsDash(strChar) == 0) {
									arPrint[arCount] = arPrint[arCount] + strChar;
								}

								if (IsComma(strChar) == 0) {
									arCount++;
									arPrint[arCount] = '';
								}

							}

							for (i = 0; i < (arCount + 1); i++) {

								if (arPrint[i].indexOf('-') > 0) {
									var dashPos;
									dashPos = (arPrint[i].indexOf('-'));

									var pageStart = arPrint[i].substr(0, dashPos);
									var pageEnd = arPrint[i].substr(arPrint[i].indexOf('-') + 1,
										(arPrint[i].length - dashPos + 1));
									pagesAll = pagesAll + range(Number(pageStart), pageEnd - pageStart + 1) + ',';
								} else {
									pagesAll = pagesAll + arPrint[i] + ',';
								}
							}
							pagesAll = pagesAll.replace(/,\s *$/, '');
							var arPage = pagesAll.split(',');
							//app.alert( pagesAll + ' / ' + (arPage.length - 1));
							var nPage = arPage.length;
							for (var i = 0; i < nPage - 1; i++) {
								var iPage = arPage[i] - 1;
								//app.alert(arPage[i]);
								for (var j = 0; j < nSelAnnts; j++) {
									var jAnnt = this.selectedAnnots[j];
									var jProps = jAnnt.getProps();
									jProps.page = iPage;
									if (jProps != null) {
										if (jProps.page != currPageIndex) {
											this.addAnnot(jProps);
											//app.alert(i + ' / ' + iPage + ' / ' + props.page + ' / ' + totalPageIndex);
										}
									}
								}


							}
							var d2 = new Date();
							var SecsElapsed = (d2 - d1) / 1000;
							var SecsElapsed2 = SecsElapsed.toFixed(0);
							var MinsElapsed2 = SecsElapsed2 / 60;
							app.alert('완료! / 경과 시간(Minutes elapsed) : ' + MinsElapsed2.toFixed(2));
						}
					}
				}
			}
		} catch (ex) {
			app.alert(ex);
		}
    }
}

function range(start, count) {
	return Array.apply(0, Array(count))
		.map(function (element, index) {
			return index + start;
		});
}

function IsComma(strChar) {

	if (strChar == ',') {
		return 0;
	}
	else {
		return -1;
	}
}

function IsSpace(strChar) {
	if (strChar == ' ') {
		return 0;
	}
	else {
		return -1;
	}
}

function IsDash(strChar) {
	if (strChar == '-') {
		return 0;
	}
	else {
		return -1;
	}
}

function IsInteger(strChar) {
	if (strChar >= 0 || strChar <= 9) {
		return 0;
	}
	else {
		return -1;
	}
}
";
                            */
                            #endregion
                            string jsString = @"//var cIconsPath = app.getPath('app', 'root');
//var iconPrintAll = util.iconStreamFromImage(cIconsPath + '/Resources/Icon-CopyAnnotsToPages_24x24.png');
//app.addToolButton({
//    cName: 'js.custom.CopyAnnotsToPages',
//    oIcon: iconPrintAll,
//    cExec: 'CopyAnnotsToPages();',
//    cTooltext: 'Copy Comment To Pages',
//    cEnable: true,
//    nPos: 0
//});

CopyAnnotsToPages();

function CopyAnnotsToPages() {
	//참고: <https://forum.tracker-software.com/viewtopic.php?f=62&t=30092&p=118915&hilit=stamp+multiple+page#p118915>
    
    //Title: COPY ANNOTATION TO USER DEFINED PAGE RANGE
    //Author:  John Statler
    //Purpose: Prompt user to enter required page
    //         range to copy annotation to other pages, e.g. 1-5,7,9-20
    
	//Get user response
	var selAnnts = null;
	try {
		selAnnts = this.selectedAnnots;
	} catch (e) {
		selAnnts = null;
		//app.alert('복제 할 주석을 선택하지 않았습니다!\n(You have not selected an annotation to duplicate)\n\n' + e);
	}
	if (!selAnnts || selAnnts.length <= 0) {
		app.alert('복제 할 주석을 선택하지 않았습니다!\n(You have not selected an annotation to duplicate)');
	}
	else {
		try {
			var nSelAnnts = selAnnts.length;

			if (nSelAnnts > 0) {
				var annt = this.selectedAnnots[0];
				var props = annt.getProps();
				var pagesAll = '';

				var currPageIndex = props.page;
				var currPageNum = currPageIndex + 1;
				var totalPageNum = this.numPages;
				var totalPageIndex = totalPageNum - 1;
				//app.alert(currPageIndex + ' / ' + totalPageIndex);

				if (currPageIndex >= totalPageIndex) {
					app.alert('복제할 페이지와 마지막 페이지가 같습니다!(Source Page is Last Page Equals)');
				}
				else {
                    app.alert('본 작업은 실행취소(UNDO, Ctrl + Z)가 지원되지 않습니다.\n\n사용에 주의를 요구합니다.\n\n(Undo (UNDO, Ctrl + Z) is not supported for this operation. It requires careful use.)');
					var cResponse = app.response({
						cQuestion: '선택한 주석을 반복할 페이지를 입력하세요,\n(Enter in the pages where you wish to repeat the comment)\n' +
							//'ex) 1,5,10-19,25\n\n' +
							//'All pages are chosen by default.\n\n' +
							//'For large documents it may take a second to do 8 pages.\n' +
							//'So 120 pages would take 15 seconds to finish.\n\n' +
							'세로 스탬프의 경우 먼저 문서의 방향과 스탬프의 방향을 먼저 확인후 적용 하세요.\n' +
							'(For vertical stamps, first rotate the document, apply stamp, then rotate back)\n\n' +
							'본 작업은 실행취소(UNDO, Ctrl + Z)가 지원되지 않습니다.\n' +
							'(Undo (UNDO, Ctrl + Z) is not supported for this operation.)\n\n' +
                            'ex) 1,5,10-19,25'
						,
						cTitle: 'Copy To Pages',
						cDefault: currPageNum + '-' + totalPageNum,
						cLabel: 'Pages:'
					});


					if (!cResponse) {
						//app.alert('입력 한 페이지가 없습니다.\n(No pages entered)');
					}
					else {
						var d1 = new Date();
						//var anntPage = this.pageNum + 1;
						var strInput = cResponse;
						var strChar;
						var arPrint = new Array(10);
						var arCount = 0;
						arPrint[arCount] = '';

						for (var i = 0; i < strInput.length; i++) {

							strChar = strInput.substr(i, 1);

							//Check character and form page group
							if (IsInteger(strChar) == 0) {
								arPrint[arCount] = arPrint[arCount] + strChar;
							}

							if (IsDash(strChar) == 0) {
								arPrint[arCount] = arPrint[arCount] + strChar;
							}

							if (IsComma(strChar) == 0) {
								arCount++;
								arPrint[arCount] = '';
							}

						}

						for (i = 0; i < (arCount + 1); i++) {

							if (arPrint[i].indexOf('-') > 0) {
								var dashPos;
								dashPos = (arPrint[i].indexOf('-'));

								var pageStart = arPrint[i].substr(0, dashPos);
								var pageEnd = arPrint[i].substr(arPrint[i].indexOf('-') + 1,
									(arPrint[i].length - dashPos + 1));
								pagesAll = pagesAll + range(Number(pageStart), pageEnd - pageStart + 1) + ',';
							} else {
								pagesAll = pagesAll + arPrint[i] + ',';
							}
						}
						pagesAll = pagesAll.replace(/,\s *$/, '');
						var arPage = pagesAll.split(',');
						//app.alert( pagesAll + ' / ' + (arPage.length - 1));
						var nArPage = arPage.length - 1;
						for (var k = 0; k < nSelAnnts; k++) {
							props = selAnnts[k].getProps();

							for (var i = 0; i < nArPage; i++) {
								var iPageNum = arPage[i];
								var iPageIndex = arPage[i] - 1;

								props.page = iPageIndex;
								if (props != null) {
									if (props.page != currPageIndex) {
                                        props.lock = false;
                                        props.author = '" + (inputAuthor.IsNullOrWhiteSpaceEx() != true ? inputAuthor : Environment.UserName) + @"';
										this.addAnnot(props);
										//for ( o in props ) console.println( o + ' : ' + props[o] ); 

                                    }
                    }
                }

                        }

            var d2 = new Date();
            var SecsElapsed = (d2 - d1) / 1000;
            var SecsElapsed2 = SecsElapsed.toFixed(0);
            var MinsElapsed2 = SecsElapsed2 / 60;
            app.alert('완료! / 경과 시간(Minutes elapsed) : ' + MinsElapsed2.toFixed(2));
					}
    }
}
		} catch (ex) {
			app.alert(ex);
		}
	}
}

function range(start, count)
{
    return Array.apply(0, Array(count))
        .map(function(element, index) {
        return index + start;
    });
}

function IsComma(strChar)
{

    if (strChar == ',')
    {
        return 0;
    }
    else
    {
        return -1;
    }
}

function IsSpace(strChar)
{
    if (strChar == ' ')
    {
        return 0;
    }
    else
    {
        return -1;
    }
}

function IsDash(strChar)
{
    if (strChar == '-')
    {
        return 0;
    }
    else
    {
        return -1;
    }
}

function IsInteger(strChar)
{
    if (strChar >= 0 || strChar <= 9)
    {
        return 0;
    }
    else
    {
        return -1;
    }
}";
                            m_Inst.ExecuteJS(m_Inst.ActiveDoc, jsString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                        }
                        m_CustomCopyAnnotsToPagesHandler?.OnNotify(nCode, pCmd, pItem, pOwner, nNotifyData);
                    }
                    else if (pCmd.ID == m_nCustomChangeAnnotsContentsID)
                    {
                        if (m_Inst != null && m_Inst.DocCount > 0)
                        {
                            IString res = m_Inst.CreateString();
                            string jsString = @"try {
	var selAntts = this.selectedAnnots;
	if (!selAntts) {
		app.alert('주석을 선택하지 않았습니다!\n(You have not selected an annotation)');
	}
	else {
		var annts = selAntts[0];
		var txt = annts.contents;

		var cResponse = app.response({
			cQuestion: '변경할 내용을 입력하세요!\n(Please enter your changes!)',
			cTitle: 'Change Comment Contents',
			cDefault: txt,
			cLabel: 'Contents :'
		});
		for (var i = 0; i < selAntts.length; i++) {
			annts = selAntts[i];
			if (cResponse) {
				annts.contents = cResponse;
			}
		}
	}
} catch (ex) {
	app.alert('주석을 선택하지 않았습니다!\n(You have not selected an annotation)\n\n' + ex);
}";
                            m_Inst.ExecuteJS(m_Inst.ActiveDoc, jsString, PDFXEdit.PXV_ActionTriggerClass.PAEC_External, PDFXEdit.PXV_ActionTriggerSubclass.PAESC_Exec, null, res);
                        }
                        m_CustomChangeAnnotsContentsHandler?.OnNotify(nCode, pCmd, pItem, pOwner, nNotifyData);
                    }
                    else if (pCmd.ID == m_nCustomCustomNumberCircleStampID)
                    {
                        Debug.WriteLine(pCmd);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw;
                }

            }

            public void OnDrawItemIcon(PDFXEdit.IUIX_RenderContext pRC, PDFXEdit.IUIX_CmdItem pItem, ref PDFXEdit.tagRECT stIconRect, ref PDFXEdit.tagRECT stClip)
            {
                //throw new System.NotImplementedException();
                //if (pItem.Cmd.ID == m_nCustomAboutID)
                //    m_CustomAboutHandler.OnDrawItemIcon(pRC, pItem, stIconRect, stClip);
                //else if (pItem.Cmd.ID == m_nCustomPrintAllID)
                //    m_CustomPrintAllHandler.OnDrawItemIcon(pRC, pItem, stIconRect, stClip);
                //else 
                if (pItem.Cmd.ID == m_nCustomCopyAnnotsToPagesID)
                    m_CustomCopyAnnotsToPagesHandler.OnDrawItemIcon(pRC, pItem, stIconRect, stClip);
                else if (pItem.Cmd.ID == m_nCustomChangeAnnotsContentsID)
                    m_CustomChangeAnnotsContentsHandler.OnDrawItemIcon(pRC, pItem, stIconRect, stClip);
            }
        }
    }
}
