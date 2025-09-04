using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win.PDFXE
{
    class PdfEditorCommandHandler : PDFXEdit.IUIX_CmdHandler
    {
        //출처 : https://gist.github.com/Polaringu/16bb29fe9f58c3a7a2a14b7cbe36f4e6
        public PDFXEdit.IPXV_Inst m_Inst = null;
        public PDFXEdit.IUIX_CmdHandler m_OldSaveHandler = null;
        public PDFXEdit.IUIX_CmdHandler m_OldSaveAsHandler = null;
        private readonly long m_nSaveID = 0;
        private readonly long m_nSaveAsID = 0;

        public PdfEditorCommandHandler(PDFXEdit.IPXV_Inst Inst)
        {
            m_Inst = Inst;
            m_nSaveID = m_Inst.Str2ID("cmd.save");
            m_nSaveAsID = m_Inst.Str2ID("cmd.saveAs");
        }

        public void OnCreateNewCtl(PDFXEdit.IUIX_Cmd pCmd, PDFXEdit.IUIX_CmdBar pParent, out PDFXEdit.IUIX_Obj pCtl)
        {
            if (pCmd.ID == m_nSaveID)
                m_OldSaveHandler.OnCreateNewCtl(pCmd, pParent, out pCtl);
            else if (pCmd.ID == m_nSaveAsID)
                m_OldSaveAsHandler.OnCreateNewCtl(pCmd, pParent, out pCtl);
            else
                pCtl = null;
        }

        public void OnGetCtlSizes(PDFXEdit.IUIX_CmdItem pItem, ref PDFXEdit.tagSIZE nSize, ref PDFXEdit.tagSIZE nMinSize, ref PDFXEdit.tagSIZE nMaxSize)
        {
            if (pItem.Cmd.ID == m_nSaveID)
                m_OldSaveHandler.OnGetCtlSizes(pItem, nSize, nMinSize, nMaxSize);
            else if (pItem.Cmd.ID == m_nSaveAsID)
                m_OldSaveAsHandler.OnGetCtlSizes(pItem, nSize, nMinSize, nMaxSize);
        }

        public void OnGetItemState(PDFXEdit.IUIX_Cmd pCmd, PDFXEdit.IUIX_CmdItem pItem, PDFXEdit.IUIX_Obj pOwner, out int nState)
        {
            if (pCmd.ID == m_nSaveID)
                m_OldSaveHandler.OnGetItemState(pCmd, pItem, pOwner, out nState);
            else if (pCmd.ID == m_nSaveAsID)
                m_OldSaveAsHandler.OnGetItemState(pCmd, pItem, pOwner, out nState);
            else
                nState = (int)PDFXEdit.UIX_CmdItemState.UIX_CmdItemState_Unknown;
        }

        public void OnGetItemSubMenu(PDFXEdit.IUIX_CmdItem pItem, out PDFXEdit.IUIX_CmdMenu pSubMenu)
        {
            //pSubMenu = pItem.SubMenu;
            if (pItem.Cmd.ID == m_nSaveID)
                m_OldSaveHandler.OnGetItemSubMenu(pItem, out pSubMenu);
            else if (pItem.Cmd.ID == m_nSaveAsID)
                m_OldSaveAsHandler.OnGetItemSubMenu(pItem, out pSubMenu);
            else
                pSubMenu = pItem.SubMenu;
        }

        public void OnNotify(int nCode, PDFXEdit.IUIX_Cmd pCmd, PDFXEdit.IUIX_CmdItem pItem, PDFXEdit.IUIX_Obj pOwner, uint nNotifyData)
        {
            if (pCmd.ID == m_nSaveID)
            {
                //...
                //Save notification
                //...
                m_OldSaveHandler.OnNotify(nCode, pCmd, pItem, pOwner, nNotifyData);
            }
            else if (pCmd.ID == m_nSaveAsID)
            {
                //...
                //Save As notification
                //...
                m_OldSaveAsHandler.OnNotify(nCode, pCmd, pItem, pOwner, nNotifyData);
            }
        }

        public void OnDrawItemIcon(PDFXEdit.IUIX_RenderContext pRC, PDFXEdit.IUIX_CmdItem pItem, ref PDFXEdit.tagRECT stIconRect, ref PDFXEdit.tagRECT stClip)
        {
            //throw new System.NotImplementedException();
            if (pItem.Cmd.ID == m_nSaveID)
                m_OldSaveHandler.OnDrawItemIcon(pRC, pItem, stIconRect, stClip);
            else if (pItem.Cmd.ID == m_nSaveAsID)
                m_OldSaveAsHandler.OnDrawItemIcon(pRC, pItem, stIconRect, stClip);
        }
    }

    class PDFEditorCommandHandlerAsListenerSample
    {
        static PdfEditorCommandHandler cmdHandler = null;

        private static void customizeSaveCommandToolStripMenuItem(AxPDFXEdit.AxPXV_Control pdfCtl)
        {
            cmdHandler = new PdfEditorCommandHandler(pdfCtl.Inst);
            PDFXEdit.IUIX_Inst iuiInst = (PDFXEdit.IUIX_Inst)pdfCtl.Inst.GetExtension("UIX");
            PDFXEdit.IUIX_Cmd cmd = iuiInst.CmdManager.Cmds.Find("cmd.save");
            cmdHandler.m_OldSaveHandler = cmd.Handler;
            cmd.Handler = cmdHandler;
            cmd = iuiInst.CmdManager.Cmds.Find("cmd.saveAs");
            cmdHandler.m_OldSaveAsHandler = cmd.Handler;
            cmd.Handler = cmdHandler;
        }

        private static void customizeCreateSubMenu(AxPDFXEdit.AxPXV_Control pdfCtl)
        {
            if (pdfCtl == null || (pdfCtl != null && pdfCtl.Frame == null))
                return;

            bool bRibbonMode = pdfCtl.Frame.View.IsRibbonMode;
            if (bRibbonMode == false)
            {
                PDFXEdit.IUIX_Inst uiInst = (PDFXEdit.IUIX_Inst)pdfCtl.Inst.GetExtension("UIX");


                PDFXEdit.IUIX_CmdBar cmdBar = pdfCtl.Inst.ActiveMainFrm.View.MenuBar;
                int nFlatIndex = cmdBar.FlatFindFirstItemByCmdName("cmd.help");
                PDFXEdit.IUIX_CmdItem cmdHelp = cmdBar.FlatGetItem(nFlatIndex);
                PDFXEdit.IUIX_CmdMenu helpMenu = cmdHelp.SubMenu;

                cmdHandler = new PdfEditorCommandHandler(pdfCtl.Inst);
                int nCmdLang = pdfCtl.Inst.Str2ID("cmd.help.language");
                PDFXEdit.IUIX_Cmd cmd = uiInst.CmdManager.Cmds.AddNew2(nCmdLang, 0, cmdHandler);
                cmd.Title = "Languages";

                helpMenu.InsertSeparator();
                PDFXEdit.IUIX_CmdMenu cmdLangMenu = helpMenu.InsertItem2(nCmdLang, -1, (int)PDFXEdit.UIX_CmdItemStyleFlags.UIX_CmdItemStyle_HasPopup | (int)PDFXEdit.UIX_CmdItemStyleFlags.UIX_CmdItemStyle_WholePopup, (int)PDFXEdit.UIX_CmdItemStyleFlags.UIX_CmdItemStyle_HasPopup | (int)PDFXEdit.UIX_CmdItemStyleFlags.UIX_CmdItemStyle_WholePopup);
                cmdLangMenu.DeleteAllItems();

                int nCmdLangEng = pdfCtl.Inst.Str2ID("cmd.help.language.english");
                PDFXEdit.IUIX_Cmd cmdEng = uiInst.CmdManager.Cmds.AddNew2(nCmdLangEng, 0, cmdHandler);
                cmdEng.Title = "English";


                cmdLangMenu.InsertItem2(nCmdLangEng);
                cmdLangMenu.InsertSeparator();

                int nCmdLangFr = pdfCtl.Inst.Str2ID("cmd.help.language.french");
                PDFXEdit.IUIX_Cmd cmdFr = uiInst.CmdManager.Cmds.AddNew2(nCmdLangFr, 0, cmdHandler);
                cmdFr.Title = "French";


                cmdLangMenu.InsertItem2(nCmdLangFr);
                cmdLangMenu.InsertSeparator();
            }
        }
    }
}
