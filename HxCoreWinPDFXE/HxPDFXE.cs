using AxPDFXEdit;
using PDFXEdit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HxCore;
using System.Runtime.InteropServices;

namespace HxCore.Win.PDFXE
{
    public partial class HxPDFXE
    {
        

        #region Define / Const Vars
        public const string _PXD_General_AppTitle_ = "General.AppTitle";
        public const string _PXD_Docs_SingleWnd_ = "Docs.SingleWnd";
        public const string _PXD_Docs_HideSingleTab_ = "Docs.HideSingleTab";
        public const string _PXD_Docs_CanOpenByDrop_ = "Docs.CanOpenByDrop";
        //public const string _PXD_Docs_OpenDocInNewWnd_ = "Docs.OpenDocInNewWnd";
        #endregion

        private AxPXV_Control pdfCtl = null;


        public AxPXV_Control PDFCtl
        {
            get => this.pdfCtl;
            protected set => this.pdfCtl = value;
        }

        protected IPXV_Document Doc
        {
            get
            {
                if (this.PDFCtl != null && this.PDFCtl.Inst != null && this.PDFCtl.Doc != null)
                {
                    return this.PDFCtl.Doc;
                }
                else
                {
                    return null;
                }
            }
        }
        protected IPXC_Document CoreDoc
        {
            get
            {
                if (this.PDFCtl != null && this.PDFCtl.Inst != null && this.PDFCtl.Doc != null && this.PDFCtl.Doc.CoreDoc != null)
                {
                    return this.PDFCtl.Doc.CoreDoc;
                }
                else
                {
                    return null;
                } 
            }
        }

        protected PXV_Inst Inst
        {
            get
            {
                if (this.PDFCtl != null && this.PDFCtl.Inst != null)
                {
                    return this.PDFCtl.Inst;
                }
                else
                {
                    return null;
                }
            }
        }

        public IPXC_Inst CoreInst
        {
            get => (IPXC_Inst)Inst.GetExtension("PXC");
        }

        public IPXC_DocSrcInfo SrcInfo
        {
            get => CoreDoc.SrcInfo;
        }

        #region Ref. Control Objects
        //uiInst = (PDFXEdit.IUIX_Inst) PDFCtl.Inst.GetExtension("UIX");
        //fsInst = (PDFXEdit.IAFS_Inst) PDFCtl.Inst.GetExtension("AFS");
        //auxInst = (PDFXEdit.IAUX_Inst) PDFCtl.Inst.GetExtension("AUX");
        //pxsInst = (PDFXEdit.IPXS_Inst) PDFCtl.Inst.GetExtension("PXS");
        //pxcInst = (PDFXEdit.IPXC_Inst) PDFCtl.Inst.GetExtension("PXC");
        public PDFXEdit.IPXS_Inst pxsInst { get => (PDFXEdit.IPXS_Inst)PDFCtl?.Inst?.GetExtension("PXS") ?? null; }
        public PDFXEdit.IPXC_Inst pxcInst { get => (PDFXEdit.IPXC_Inst)PDFCtl?.Inst?.GetExtension("PXC") ?? null; }
        public PDFXEdit.IUIX_Inst uixInst { get => (PDFXEdit.IUIX_Inst)PDFCtl?.Inst?.GetExtension("UIX") ?? null; }
        public PDFXEdit.IAFS_Inst afsInst { get => (PDFXEdit.IAFS_Inst)PDFCtl?.Inst?.GetExtension("AFS") ?? null; }
        public PDFXEdit.IAUX_Inst auxInst { get => (PDFXEdit.IAUX_Inst)PDFCtl?.Inst?.GetExtension("AUX") ?? null; }
        /**
        private IPXS_Inst pxsInst
        {
            get
            {
                //return this.owner.uiInst; 
                return (this.PXVCtl != null ? (IPXS_Inst)this.PXVCtl.Inst.GetExtension("PXS") : null);
            }

        }
        private IPXC_Inst pxcInst
        {
            get
            {
                return (this.PXVCtl != null ? (PDFXEdit.IPXC_Inst)this.PXVCtl.Inst.GetExtension("PXC") : null);
            }
        }
        private IUIX_Inst uiInst
        {
            get
            {
                //return this.owner.uiInst; 
                return (this.PXVCtl != null ? (IUIX_Inst)this.PXVCtl.Inst.GetExtension("UIX") : null);
            }
        }
        private IAFS_Inst fsInst
        {
            get { return (this.PXVCtl != null ? (IAFS_Inst)this.PXVCtl.Inst.GetExtension("AFS") : null); }
        }
        private IAUX_Inst auxInst
        {
            get { return (this.PXVCtl != null ? (IAUX_Inst)this.PXVCtl.Inst.GetExtension("AUX") : null); }
        }
        */
        public IPXS_Inst PXSInst
        {
            get { return this.pxsInst; }
        }
        public IPXC_Inst PXCInst
        {
            get { return this.pxcInst; }
        }
        public IUIX_Inst PXUiInst
        {
            get { return this.uixInst; }
        }
        public IAFS_Inst PXFsFInst
        {
            get { return this.afsInst; }
        }
        public IAUX_Inst PXAuxInst
        {
            get { return this.auxInst; }
        }

        public IUIX_EventMonitor uiEventMon = null;
        #endregion

        private PDFXEdit.IUIX_CmdBar _menuCmdBar { get => PDFCtl?.Inst?.ActiveMainFrm?.View?.MenuBar ?? null; }
        public bool? IsRibbonMode => PDFCtl?.Frame?.View?.IsRibbonMode;
        private PDFXEdit.IPXC_StampsManager _stampManager => CoreInst?.StampsManager;
        public bool IsStartUp { get; protected set; }


        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="axPdf">ActiveX Resource</param>
        /// <param name="licenseKey">License Key</param>
        /// <param name="bUserSettings">userSettingSrc?</param>
        public HxPDFXE(AxPXV_Control axPdf, string licenseKey = "", bool? bUserSettings = null, string userSettingsFilePath = null)
        {
            this.SetAxPDFControl(axPdf, licenseKey, bUserSettings, userSettingsFilePath);
        }

        /// <summary>
        /// Instance 생성자
        /// </summary>
        /// <param name="axPdf">ActiveX Resource</param>
        /// <param name="licenseKey">License Key</param>
        /// /// <param name="bUserSettings">userSettingSrc?</param>
        /// <returns>ActiveX Resource</returns>
        public static HxPDFXE Create(AxPXV_Control axPdf, string licenseKey = "", bool? bUserSettings = null)
        {
            return new HxPDFXE(axPdf, licenseKey, bUserSettings);
        }

        /// <summary>
        /// ActiveX 지정
        /// </summary>
        /// <param name="axPdf">ActiveX Resource</param>
        /// <param name="licenseKey">License Key</param>
        /// <param name="bUserSettings">userSettingSrc?</param>
        public void SetAxPDFControl(AxPXV_Control axPdf, string licenseKey = "", bool? bUserSettings = null, string userSettingsFilePath = null)
        {
            this.PDFCtl = axPdf;
            if (!licenseKey.IsNullOrWhiteSpaceEx())
            {
                this.SetLicenseKey(licenseKey);
            }
            //this.SetLicenseKey(licenseKey);
            //PXVCtl.Inst.LoadUserSettings()
            //PXVCtl.Inst.Init()
            if(bUserSettings == true)
            {
                //this.pdfCtl.
                this.SetUserSettings(true, userSettingsFilePath);
            }
            this.Init();
        }

        private void Init(bool bInitApply = false)
        {
            if(this.IsStartUp != true || bInitApply == true)
            {
                try
                {

                    //this.InitIDS();
                    this.nIDS = this.GetIDSArray();
                    //uiInst = (PDFXEdit.IUIX_Inst)PDFCtl.Inst.GetExtension("UIX");
                    //fsInst = (PDFXEdit.IAFS_Inst)PDFCtl.Inst.GetExtension("AFS");
                    //auxInst = (PDFXEdit.IAUX_Inst)PDFCtl.Inst.GetExtension("AUX");
                    //pxsInst = (PDFXEdit.IPXS_Inst)PDFCtl.Inst.GetExtension("PXS");
                    //pxcInst = (PDFXEdit.IPXC_Inst)PDFCtl.Inst.GetExtension("PXC");

                    this.IsStartUp = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                }
            }
        }

        #region Command Bar / Pane(l)
        /// <summary>
        /// Online - Show(On)/Hidden(Off), Menu / Command Bar
        /// </summary>
        /// <param name="barID">Menu / Command Bar HxID</param>
        /// <param name="bShow">Show?</param>
        public void SetShowCommandBar(HxIDS barID, bool bShow)
        {
            try
            {
                PDFCtl.Inst.ShowCmdBar2(nIDS[(int)barID], bShow);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw;
            }
            
        }
        /// <summary>
        /// Online - Show(On)/Hidden(Off), Menu / Command Bar
        /// </summary>
        /// <param name="cmdNuberID">Menu / Command Bar NumberID</param>
        /// <param name="bShow">Show?</param>
        public void SetShowCommandBar(int cmdNuberID, bool bShow)
        {
            try
            {
                PDFCtl.Inst.ShowCmdBar2(cmdNuberID, bShow);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw;
            }
        }
        /// <summary>
        /// Online - Show(On)/Hidden(Off), Menu / Command Bar
        /// </summary>
        /// <param name="cmdStrID">Menu / Command Bar StringID</param>
        /// <param name="bShow">Show?</param>
        public void SetShowCommandBar(string cmdStrID, bool bShow)
        {
            try
            {
                PDFCtl.Inst.ShowCmdBar(cmdStrID, bShow);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw;
            }
        }
        /// <summary>
        /// Display - Visible / Invisible, Menum / Command Bar
        /// </summary>
        /// <param name="barID">Menu / Command Bar HxID</param>
        /// <returns>Visible?</returns>
        public bool GetIsCommandBarVisible(HxIDS barID)
        {
            return PDFCtl.Inst.IsCmdBarVisible2(nIDS[(int)barID]);
        }
        /// <summary>
        /// Display - Visible / Invisible, Menum / Command Bar
        /// </summary>
        /// <param name="barID">Menu / Command Bar NumberID</param>
        /// <returns>Visible?</returns>
        public bool GetIsCommandBarVisible(int barNumberID)
        {
            return PDFCtl.Inst.IsCmdBarVisible2(barNumberID);
        }
        /// <summary>
        /// Display - Visible / Invisible, Menum / Command Bar
        /// </summary>
        /// <param name="barID">Menu / Command Bar StringID</param>
        /// <returns>Visible?</returns>
        public bool GetIsCommandBarVisible(string barStrID)
        {
            return PDFCtl.Inst.IsCmdBarVisible(barStrID);
        }

        public virtual void SetShowPane(HxIDS paneID, bool bShow)
        {
            PDFCtl.ShowPane2(nIDS[(int)paneID], bShow);
        }
        
        /// <summary>
        /// 모든 Pane들 관련 설정
        /// </summary>
        /// <param name="bShow"></param>
        public void ShowCommandPanesAll(bool bShow)
        {
            PDFCtl.VisibleCmdPanes = (bShow == true ? (uint)PDFXEdit.PXV_VisibleCmdPanes.PXV_VisibleCmdPanes_All : 0);
        }

        /// <summary>
        /// FDF/XFDF 등 작업에 경고창 OFF
        /// Warring Dialog Message (Open Site Warning: Potential security issue!)
        /// </summary>
        /// <param name="value">옵션</param>
        public void SetOpenSiteWarningDisable() 
        {
            //1 : Allow it
            PDFCtl.Inst.Settings["Security.OpenFilePerm"].v = 1;
            PDFCtl.Inst.Settings["Security.OpenSitePerm"].v = 1;
            PDFCtl.Inst.FireAppPrefsChanged(PDFXEdit.PXV_AppPrefsChanges.PXV_AppPrefsChange_Security);
        }

        public void DefaultLayout()
        {
            PDFCtl.LockedCmdBars = true;
            PDFCtl.LockedCmdPanes = true;
            PDFCtl.VisibleScrollbars = true;
            this.SetShowCommandBar(HxIDS.cmdbar_menubar, true);
        }
        public void DefaultPane(bool bDefaultShow = false)
        {
            if (bDefaultShow != false && PDFCtl.Inst.DocCount > 0 && PDFCtl.Inst.DocCount <= 1)
            {
                //this.SetShowPane(HxIDS.bookmarksView, true);
                //this.SetShowPane(HxIDS.layersView, true);
                //this.SetShowPane(HxIDS.attachmentsView, true);
                this.SetShowPane(HxIDS.commentsView, true);
                this.SetShowPane(HxIDS.pageThumbnailsView, true);
            }
        }
        #endregion

        

        #region PDF-X Editor Control Methods
        /// <summary>
        /// Show Command Bar
        /// </summary>
        /// <param name="barID">Command Bar ID</param>
        /// <param name="bShow">Show On/Off</param>
        public void SetExecShowCmdBar(HxIDS barID, bool bShow)
        {
            PDFCtl.Inst.ShowCmdBar2(this.nIDS[(int)barID], bShow);
        }
        /// <summary>
        /// Show Command Bar [Alias : ShowCmdBar()]
        /// </summary>
        /// <param name="barID">Command Bar ID</param>
        /// <param name="bShow">Show On/Off</param>
        public void ExecShowCmdBar(HxIDS barID, bool bShow)
        {
            this.SetExecShowCmdBar(barID, bShow);
        }
        /// <summary>
        /// Is Visible : Command Bar
        /// </summary>
        /// <param name="barName">Command Bar String ID(Name)</param>
        /// <returns>Visible/Hidden</returns>
        public bool IsCmdBarVisible(string barName)
        {
            return PDFCtl.Inst.IsCmdBarVisible(barName);
        }
        /// <summary>
        /// Is Visible : Command Bar
        /// </summary>
        /// <param name="barID">Command Bar Enum ID</param>
        /// <returns>Visible/Hidden</returns>
        public bool IsCmdBarVisible(HxIDS barID)
        {
            return PDFCtl.Inst.IsCmdBarVisible2(nIDS[(int)barID]);
        }
        /// <summary>
        /// Is Visible : Command Bar
        /// </summary>
        /// <param name="barCmd">Command Bar Object</param>
        /// <returns>Visible/Hidden</returns>
        public bool IsCmdBarVisible(PDFXEdit.IUIX_Cmd barCmd)
        {
            return PDFCtl.Inst.IsCmdBarVisible2(barCmd.ID);
        }
        /// <summary>
        /// Show Pane/Palette View
        /// </summary>
        /// <param name="paneID">Pane/Palette ID</param>
        /// <param name="bShow">Show On/Off</param>
        public void ExecCtlShowPane(HxIDS paneID, bool bShow)
        {
            PDFCtl.ShowPane2(nIDS[(int)paneID], bShow);
        }
        /// <summary>
        /// Show Pane/Palette View [Alias : ShowPane()]
        /// </summary>
        /// <param name="paneID">Pane/Palette ID</param>
        /// <param name="bShow">Show On/Off</param>
        public void ExecShowPane(HxIDS paneID, bool bShow)
        {
            this.ExecCtlShowPane(paneID, bShow);
        }
        /// <summary>
        /// Is Visible : Pane/Palette View
        /// </summary>
        /// <param name="paneName">String ID(Name)</param>
        /// <returns>Visible/Hidden</returns>
        public bool IsPaneVisible(string paneName)
        {
            int nVis = PDFCtl.GetPaneVisibility(paneName);
            return (nVis > 0);
        }
        /// <summary>
        /// Is Visible : Pane/Palette View
        /// </summary>
        /// <param name="paneID">Enum ID</param>
        /// <returns>Visible/Hidden</returns>
        public bool IsPaneVisible(HxIDS paneID)
        {
            int nVis = PDFCtl.GetPaneVisibility2(nIDS[(int)paneID]);
            return (nVis > 0);
        }
        /// <summary>
        /// Is Visible : Pane/Palette View
        /// </summary>
        /// <param name="paneCmd">Command Pane Object</param>
        /// <returns>Visible/Hidden</returns>
        public bool IsPaneVisible(PDFXEdit.IUIX_Cmd paneCmd)
        {
            int nVis = PDFCtl.GetPaneVisibility2(paneCmd.ID);
            return (nVis > 0);
        }
        /// <summary>
        /// Get Find Command Object
        /// </summary>
        /// <param name="sID">String ID</param>
        /// <returns>Command Object</returns>
        public PDFXEdit.IUIX_Cmd GetFindCmd(string sID)
        {
            return PXUiInst.CmdManager.Cmds.Find(sID);
        }
        /// <summary>
        /// Get Find Command Object
        /// </summary>
        /// <param name="nID">Number ID</param>
        /// <returns>Command Object</returns>
        public PDFXEdit.IUIX_Cmd GetFindCmd(int nID)
        {
            return PXUiInst.CmdManager.Cmds.Find2(nID);
        }
        /// <summary>
        /// [Execute] UIX Command
        /// </summary>
        /// <param name="cmd">Command Object</param>
        public void ExecUiCmd(PDFXEdit.IUIX_Cmd cmd)
        {
            if (cmd == null)
                return;
            PDFCtl.Inst.ExecUICmd2(cmd.ID);
        }
        /// <summary>
        /// [Execute] UIX Command
        /// </summary>
        /// <param name="sID">Command String ID</param>
        public void ExecUiCmd(string sID)
        {
            PDFXEdit.IUIX_Cmd cmd = GetFindCmd(sID);
            this.ExecUiCmd(cmd);
        }
        /// <summary>
        /// [Execute] UIX Command
        /// </summary>
        /// <param name="nID">Command Number ID</param>
        public void ExecUiCmd(int nID)
        {
            PDFXEdit.IUIX_Cmd cmd = GetFindCmd(nID);
            if (cmd == null)
                return;
            this.ExecUiCmd(cmd);
        }
        /// <summary>
        /// [Execute] UIX Command - Offline
        /// </summary>
        /// <param name="cmd">Command Object</param>
        /// <param name="bOffline">[Option]null(default) : Online<->Offline, True : Offline, False : Online</param>
        public void ExecCmdOffline(PDFXEdit.IUIX_Cmd cmd, bool? bOffline = null)
        {
            if (cmd == null)
                return;
            if (bOffline == null)
            {
                cmd.Offline = !cmd.Offline;
            }
            else
            {
                cmd.Offline = bOffline.ToConvertEx<bool>();
            }
        }
        /// <summary>
        /// [Execute] UIX Command - Offline
        /// </summary>
        /// <param name="nID">Command Number ID</param>
        /// <param name="bOffline">[Option]null(default) : Online<->Offline, True : Offline, False : Online</param>
        public void ExecCmdOffline(int nID, bool? bOffline = null)
        {
            PDFXEdit.IUIX_Cmd cmd = GetFindCmd(nID);
            if (cmd == null)
                return;
            this.ExecCmdOffline(cmd, bOffline);
        }


        /// <summary>
        /// [Execute] UIX Command - Offline
        /// </summary>
        /// <param name="sID">Command String ID</param>
        /// <param name="bOnline">[Option]null(default) : Online<->Offline, True : Offline, False : Online</param>
        public void ExecCmdOffline(string sID, bool? bOffline = null)
        {
            PDFXEdit.IUIX_Cmd cmd = GetFindCmd(sID);
            if (cmd == null)
                return;
            this.ExecCmdOffline(cmd, bOffline);
        }
        /// <summary>
        /// Show UIX Command - Hidden
        /// </summary>
        /// <param name="cmd">Command Object</param>
        /// <param name="bHidden">[Option]null(default) : Visible<->Hidden, True : Hidden, False : Visible</param>
        public void ExecCmdHidden(PDFXEdit.IUIX_Cmd cmd, bool? bHidden = null)
        {
            if (cmd == null)
                return;
            if (bHidden == null)
            {
                cmd.Hidden = !cmd.Hidden;
            }
            else
            {
                cmd.Hidden = bHidden.ToConvertEx<bool>();
            }
        }
        /// <summary>
        /// Show UIX Command - Hidden
        /// </summary>
        /// <param name="sID">Command String ID</param>
        /// <param name="bHidden">[Option]null(default) : Visible<->Hidden, True : Hidden, False : Visible</param>
        public void ExecCmdHidden(string sID, bool? bHidden = null)
        {
            PDFXEdit.IUIX_Cmd cmd = GetFindCmd(sID);
            if (cmd == null)
                return;
            this.ExecCmdHidden(cmd, bHidden);
        }
        /// <summary>
        /// Show UIX Command - Hidden
        /// </summary>
        /// <param name="nID">Command Number ID</param>
        /// <param name="bHidden">[Option]null(default) : Visible<->Hidden, True : Hidden, False : Visible</param>
        public void ExecCmdHidden(int nID, bool? bHidden = null)
        {
            PDFXEdit.IUIX_Cmd cmd = GetFindCmd(nID);
            if (cmd == null)
                return;
            this.ExecCmdHidden(cmd, bHidden);
        }
        /// <summary>
        /// Show UIX Command - Offline & Hidden
        /// </summary>
        /// <param name="cmd">Command Object</param>
        /// <param name="bHidden">[Option]null(default) : Visible<->Hidden / Online <-> Offline, True : Hidden / Offline, False : Visible / Online</param>
        public void ExecCmdBothOff(PDFXEdit.IUIX_Cmd cmd, bool? bHidden = null)
        {
            this.ExecCmdOffline(cmd, bHidden);
            this.ExecCmdHidden(cmd, bHidden);
        }
        /// <summary>
        /// Show UIX Command - Offline & Hidden
        /// </summary>
        /// <param name="sID">Command String ID</param>
        /// <param name="bHidden">[Option]null(default) : Visible<->Hidden / Online <-> Offline, True : Hidden / Offline, False : Visible / Online</param>
        public void ExecCmdBothOff(string sID, bool? bHidden = null)
        {
            this.ExecCmdOffline(sID, bHidden);
            this.ExecCmdHidden(sID, bHidden);
        }
        /// <summary>
        /// Show UIX Command - Offline & Hidden
        /// </summary>
        /// <param name="sIDList">Command String ID List</param>
        /// <param name="bHidden">[Option]null(default) : Visible<->Hidden / Online <-> Offline, True : Hidden / Offline, False : Visible / Online</param>
        public void ExecCmdBothOff(List<string> sIDList, bool? bHidden = null)
        {
            try
            {
                if(sIDList != null)
                {
                    int n = sIDList.Count;
                    for(int i = 0; i < n; i++)
                    {
                        this.ExecCmdBothOff(sIDList[i], bHidden);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
        }

        /// <summary>
        /// Show UIX Command - Offline & Hidden
        /// </summary>
        /// <param name="nID">Command Number ID</param>
        /// <param name="bHidden">[Option]null(default) : Visible<->Hidden / Online <-> Offline, True : Hidden / Offline, False : Visible / Online</param>
        public void ExecCmdBothOff(int nID, bool? bHidden = null)
        {
            this.ExecCmdOffline(nID, bHidden);
            this.ExecCmdHidden(nID, bHidden);
        }
        /// <summary>
        /// Shortcut 사용
        /// </summary>
        /// <param name="bOption"></param>
        public void ExecAllowShortcuts(bool? bOption = null)
        {
            if (bOption == null)
            {
                PDFCtl.AllowedShortcuts = uixInst.CmdManager.AccelsAllowed;
            }
            else
            {
                PDFCtl.AllowedShortcuts = bOption.ToConvertEx<bool>(); // pdfCtl.AllowedShortcuts == uiInst.CmdManager.AccelsAllowed
            }
        }
        /// <summary>
        /// Show UIX Command List
        /// </summary>
        /// <returns>UIX Command DataTable</returns>
        public DataTable GetCommandDataTable()
        {
            DataTable Result = null;
            if (uixInst != null && uixInst.CmdManager != null)
            {
                PDFXEdit.IUIX_CmdCollection cmds = uixInst.CmdManager.Cmds;
                if (cmds != null)
                {
                    Result = new DataTable("Command List");
                    Result.Columns.AddRange(new DataColumn[]{
                    new DataColumn { ColumnName = "seq_no", Caption = "Seq. No", DataType = typeof(int), AutoIncrement = true, AutoIncrementSeed = 1, Unique = true}
                    , new DataColumn { ColumnName = "id", Caption = "Number ID", DataType = typeof(int) }
                    , new DataColumn { ColumnName = "name", Caption = "String Name", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "title", Caption = "Title", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "offline", Caption = "Offline", DataType = typeof(bool) }
                    , new DataColumn { ColumnName = "hidden", Caption = "Hidden", DataType = typeof(bool) }
                    , new DataColumn { ColumnName = "alias", Caption = "Alias", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "tip", Caption = "Tip", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "icon", Caption = "Icon", DataType=typeof(object) }
                    , new DataColumn { ColumnName = "ctgid", Caption = "CtgID" }
                    , new DataColumn { ColumnName = "flags", Caption = "Flags" }
                    , new DataColumn { ColumnName = "handler", Caption = "Handler", DataType = typeof(object) }
                    , new DataColumn { ColumnName = "shortcutscount", Caption = "ShortcutsCount" }
                    , new DataColumn { ColumnName = "shorttitle", Caption = "ShortTitle" }
                    , new DataColumn { ColumnName = "targetid", Caption = "targetid" }
                    , new DataColumn { ColumnName = "userdata", Caption = "UserData" }
                    , new DataColumn { ColumnName = "newitemstyle", Caption = "NewItemStyle" }
                    , new DataColumn { ColumnName = "newitemstylemaskeu", Caption = "NewItemStyleMaskEU" }
                });
                    uint cnt = cmds.Count;
                    for (uint i = 0; i < cnt; i++)
                    {
                        PDFXEdit.IUIX_Cmd cmd = cmds[i];
                        //ListViewItem it = new ListViewItem(new[] { cmd.ID.ToStringEx(), pdfCtl.Inst.ID2Str(cmd.ID), cmd.Title, (cmd.Offline ? "Yes" : "No"), (cmd.Hidden ? "Yes" : "No"), cmd.Alias, cmd.Tip });
                        //lvCmds.Items.Add(it);
                        DataRow dr = Result.NewRow();
                        dr["id"] = cmd.ID;
                        dr["name"] = PDFCtl.Inst.ID2Str(cmd.ID);
                        dr["title"] = cmd.Title;
                        dr["offline"] = cmd.Offline;
                        dr["hidden"] = cmd.Hidden;
                        dr["alias"] = cmd.Alias;
                        dr["tip"] = cmd.Tip;
                        dr["icon"] = cmd.Icon;
                        dr["ctgid"] = cmd.CtgID;
                        dr["flags"] = cmd.Flags;
                        //dr["handler"] = cmd.Handler;
                        dr["shortcutscount"] = cmd.ShortcutsCount;
                        dr["shorttitle"] = cmd.ShortTitle;
                        dr["targetid"] = cmd.TargetID;
                        dr["userdata"] = cmd.UserData;
                        dr["newitemstyle"] = cmd.NewItemStyle;
                        dr["newitemstylemaskeu"] = cmd.NewItemStyleMaskEU;
                        Result.Rows.Add(dr);
                    }
                }
            }
            return Result;
        }
        #endregion

        

        #region SDK License
        private static string _PXE_LIC_KEY_ =
                "dEmOk4WZ3uxwI/9oEZODemogFi61n61nNVt6UTzu16hUPvDEmots7yE5IUcd4Z+NvMgQzdQ1" +
                "7lAG3IrDeMogTpzOfxzKsRfNQRD9UqkyyZx6sYwPrDtnWzndqSV/zSl+0QpJ5b8QtdemOBsq" +
                "8511v3l+sgec6JExR944vi35DEMOGC1GOsXDd9LzSU/Eg9TY/Y3yctxyh5UA9ljIWviQ9W4T" +
                "OzDmaiyv5giyCPYwO2HyZdemoa3fi8zpvOy2EeYgWvfPSGjRqxlCT1a0wBxpNe4QB5R6tr+X" +
                "qR9JPV/p8DJ4vRqDDsDEMOX4xm/iXP3fdz/1KQs/elwMqwtUUrJYjzvDu7AwBpWEQ9so04ZO" +
                "baGYL3C6N/oaKioFL+0d7cyEA+2+/CdEMoelQKDEVqvEUxatrMJsD6yald01Cd1DA1eq7Tt1" +
                "b3vn58E2dEMobiBmg4qkdOpLtjcYxh69t3BVtKxmu6uyXZd+gO0NZxHkQT+6/U1334DEMO+H" +
                "oou1/TmICS9GS6p+nfTQLZpButSOkGfaT7V17n6NkTvSKwLtrwDEMO=="
            ;

        protected string GetLicenseKey(string rootPath = null)
        {
            string Result = _PXE_LIC_KEY_;
            if (!rootPath.IsNullOrWhiteSpaceEx())
            {
                Result = GetRegistryLicenseKey(rootPath);
            }
            return Result;
        }
        private string GetRegistryLicenseKey(string rootPath)
        {
            string Result = null;
            if (!rootPath.IsNullOrWhiteSpaceEx())
            {
                string pubLicKey = GetOptStr(rootPath, "LicKey");
                if (pubLicKey.Length != 0)
                    Result = pubLicKey;
            }
            return Result;
        }

        public void SetLicenseKey(string licenseKey = null)
        {
            if (!licenseKey.IsNullOrWhiteSpaceEx())
            {
                _PXE_LIC_KEY_ = licenseKey;
            }
            if (this.PDFCtl != null && this.PDFCtl.Inst != null)
            {
                this.PDFCtl.SetLicKey(_PXE_LIC_KEY_);
            }
            this.Inst.Init(_PXE_LIC_KEY_);
        }
        #endregion

        #region Custom Methods
        
        public bool SetCoreDocSrcInfoCustomDisplayTitle(string displayTitle, string displayFileName = null, string customFileName = null)
        {
            bool Result = false;
            if (CoreDoc != null)
            {
                try
                {
                    //CoreDoc?.Props?.set_ViewPrefFlag(PDFXEdit.PXC_DocumentViewFlags.DocViewFlag_DisplayDocTitle, true);
                    if (displayTitle != null)
                    {
                        //string strDisplayTitle = srcCoreDoc.SrcInfo.CustDispTitle;
                        CoreDoc.SrcInfo.CustDispTitle = displayTitle;
                    }
                    if (displayFileName != null)
                    {
                        //string strDisplayFileName = srcCoreDoc.SrcInfo.CustDispFileName;
                        CoreDoc.SrcInfo.CustDispFileName = displayFileName;
                    }
                    if (customFileName != null)
                    {
                        //string strCustomFileName = srcCoreDoc.SrcInfo.CustFileName;
                        CoreDoc.SrcInfo.CustFileName = customFileName;
                    }

                    Result = true;

                    try
                    {
                        CoreDoc.Props.set_ViewPrefFlag(PDFXEdit.PXC_DocumentViewFlags.DocViewFlag_DisplayDocTitle, true);
                    }
                    catch (Exception exProps)
                    {
                        Result = false;
                        Debug.WriteLine(exProps);
                        //throw exProps;
                    }

                    //string strActiveCustomFileName = cmp.Doc.CoreDoc.SrcInfo.CustFileName;
                    try
                    {
                        /*
                        //if(pdfCtl.)
                        var evtID = Inst.Str2ID("e.document.propsChanged", false);
                        IntPtr unkPtr = System.Runtime.InteropServices.Marshal.GetIUnknownForObject(Doc);
                        PDFXEdit.IEvent evt = Doc.EventServer.CreateNewEvent(evtID, (uint)unkPtr, (uint)PDFXEdit.PXC_DocumentViewFlags.DocViewFlag_DisplayDocTitle);
                        Doc.EventServer.FireEvent(evt, Doc);
                        

                        int eventID2 = Inst.Str2ID("e.document.applyCachedChanges");
                        IEvent evt2 = Doc.EventServer.CreateNewEvent(eventID2, 0, 0);
                        Doc.EventServer.FireEvent(evt, Doc);
                        */
                    }
                    catch (Exception exEvt)
                    {
                        Result = false;
                        Debug.WriteLine(exEvt);
                        //throw exEvt;
                    }
                }
                catch (Exception ex)
                {
                    Result = false;
                    Debug.WriteLine(ex);
                    //throw ex;
                }
                
            }
            return Result;
        }

        public void SetDocCoreDocInfo(string title, string author = null, string subject = null, string keywords = null, string creator = null, string producer = null, string creatorTool = null, DateTime? creationDate = null, DateTime? modificationDate = null)
        {
            SetDocCoreDocInfo_Title(title);
            SetDocCoreDocInfo_Author(author);
            SetDocCoreDocInfo_Subject(subject);
            SetDocCoreDocInfo_Keywords(keywords);
            SetDocCoreDocInfo_Creator(creator);
            SetDocCoreDocInfo_Producer(producer);
            SetDocCoreDocInfo_CreatorTool(creatorTool);
            SetDocCoreDocInfo_CreationDate(creationDate);
            SetDocCoreDocInfo_ModificationDate(modificationDate);
        }
        public void SetDocCoreDocInfo_Title(string value)
        {
            if (value != null)
            {
                CoreDoc.Info[PXC_DocumentInfoKey.DocInfo_Title] = value;
            }

        }
        public void SetDocCoreDocInfo_Author(string value = null)
        {
            if (value != null)
            {
                CoreDoc.Info[PXC_DocumentInfoKey.DocInfo_Author] = value;
            }
        }
        public void SetDocCoreDocInfo_Subject(string value = null)
        {
            if (value != null)
            {
                CoreDoc.Info[PXC_DocumentInfoKey.DocInfo_Subject] = value;
            }
        }
        public void SetDocCoreDocInfo_Keywords(string value = null)
        {
            if (value != null)
            {
                CoreDoc.Info[PXC_DocumentInfoKey.DocInfo_Keywords] = value;
            }
        }
        public void SetDocCoreDocInfo_Creator(string value = null)
        {
            if (value != null)
            {
                CoreDoc.Info[PXC_DocumentInfoKey.DocInfo_Creator] = value;
            }
        }
        public void SetDocCoreDocInfo_Producer(string value = null)
        {
            if (value != null)
            {
                CoreDoc.Info[PXC_DocumentInfoKey.DocInfo_Producer] = value;
            }
        }
        public void SetDocCoreDocInfo_CreatorTool(string value = null)
        {
            if (value != null)
            {
                CoreDoc.Info[PXC_DocumentInfoKey.DocInfo_CreatorTool] = value;
            }
        }
        public void SetDocCoreDocInfo_CreationDate(DateTime? value = null)
        {
            if (value != null && value.HasValue)
            {
                CoreDoc.Info.SetCreationDate((DateTime)value);
            }
        }
        public void SetDocCoreDocInfo_ModificationDate(DateTime? value = null)
        {
            if (value != null && value.HasValue)
            {
                CoreDoc.Info.SetModificationDate((DateTime)value);
            }
        }
        public void SetDocCoreDocInfo_ModDat(DateTime? value = null)
        {
            SetDocCoreDocInfo_ModificationDate(value);
        }


        public bool IsDocFindCustomTitle(string pDisplayTitle)
        {
            bool Result = false;
            if (PDFCtl.HasDoc == true && pDisplayTitle.IsNullOrWhiteSpaceEx() != true)
            {
                uint n = Inst.DocCount;
                for (uint i = 0; i < n; i++)
                {
                    var doc = PDFCtl.Inst.Doc[i];
                    if (doc.CoreDoc.SrcInfo.CustDispTitle.IsNullOrWhiteSpaceEx() != true && doc.CoreDoc.SrcInfo.CustDispTitle == pDisplayTitle)
                    {
                        return true;
                    }
                }
            }
            return Result;
        }

        public IPXV_Document GetDocFindCustomTitle(string pDisplayTitle)
        {
            IPXV_Document Result = null;
            if (PDFCtl.HasDoc == true && pDisplayTitle.IsNullOrWhiteSpaceEx() != true)
            {
                uint n = Inst.DocCount;
                for (uint i = 0; i < n; i++)
                {
                    var doc = PDFCtl.Inst.Doc[i];
                    if (doc.CoreDoc.SrcInfo.CustDispTitle.IsNullOrWhiteSpaceEx() != true && doc.CoreDoc.SrcInfo.CustDispTitle == pDisplayTitle)
                    {
                        return doc;
                    }
                }
            }
            return Result;
        }

        public IPXV_Document GetDocFindActualFileName(string pFileFullName)
        {
            IPXV_Document Result = null;
            if (PDFCtl.HasDoc == true && pFileFullName.IsNullOrWhiteSpaceEx() != true)
            {
                uint n = Inst.DocCount;
                for (uint i = 0; i < n; i++)
                {
                    var doc = PDFCtl.Inst.Doc[i];
                    if (doc.CoreDoc.SrcInfo.ActualFileName.IsNullOrWhiteSpaceEx() != true && doc.CoreDoc.SrcInfo.ActualFileName == pFileFullName)
                    {
                        return doc;
                    }
                }
            }
            return Result;
        }


        public bool? OpenFileLoad(string pOpenFile, string pDisplayTitle = null, bool pOverload = false)
        {
            bool? Result = false;
            if (pOpenFile.IsNullOrWhiteSpaceEx() != true && File.Exists(pOpenFile) == true)
            {
                try
                {
                    if (pDisplayTitle != null && pOverload == false && PDFCtl.HasDoc == true)
                    {
                        uint n = Inst.DocCount;
                        for (uint i = 0; i < n; i++)
                        {
                            var doc = PDFCtl.Inst.Doc[i];
                            if (doc.CoreDoc.SrcInfo.CustDispTitle.IsNullOrWhiteSpaceEx() != true && doc.CoreDoc.SrcInfo.CustDispTitle == pDisplayTitle)
                            {
                                PDFCtl.Inst.ActiveDoc = doc;
                                return false;
                            }
                            //this.OpenDocFromPath(pOpenFile);
                            //if (pDisplayTitle != null)
                            //{
                            //    SetCoreDocSrcInfoCustomDisplayTitle(pDisplayTitle);
                            //}
                            //Result = true;
                        }
                    }

                    this.OpenDocFromPath(pOpenFile, !pDisplayTitle.IsNullOrWhiteSpaceEx());
                    Result = true;

                    if (pDisplayTitle != null)
                    {
                        bool bDisplayTitleChaged = SetCoreDocSrcInfoCustomDisplayTitle(pDisplayTitle);
                        if(bDisplayTitleChaged != true)
                        {
                            Result = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Result = false;
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            return Result;
        }
        public void DiscardPDFACompilance()
        {
            bool isPdfaDocument = this.PDFCtl.Doc.CoreDoc.Props.PDFStandard != PXC_PDFStandard.PDFS_None;
            if (this.PDFCtl != null && this.PDFCtl.Doc != null && isPdfaDocument == true)
            {
                this.PDFCtl.Doc.DiscardPDFACompilance();
            }
        }
        #endregion

        public uint GetEmbeddedFileCount()
        {
            uint Result = 0;

            PDFXEdit.IPXV_Document doc = PDFCtl.Doc;
            if (doc == null) return Result;

            IPXC_NameTree TreeEmbeddedFiles = doc.CoreDoc.GetNameTree("EmbeddedFiles");
            if (TreeEmbeddedFiles == null) return Result;

            uint nAttachmentsCount = TreeEmbeddedFiles.Count;
            Result = nAttachmentsCount;
            /*
            IPXC_NameTree TreePages = Doc.CoreDoc.GetNameTree("Pages");
            uint nAttachmentsCount2 = TreePages.Count;
            Result = nAttachmentsCount > nAttachmentsCount2 ? nAttachmentsCount : nAttachmentsCount2;
            */
            
            PDFXEdit.PXV_Inst inst = PDFCtl?.Inst;
            if (inst == null) return Result;

            PDFXEdit.IPXV_AnnotsList annotsList = inst?.CreateAnnotsList();
            if (annotsList == null) return Result;

            for (uint i = 0; i < Doc.CoreDoc.Pages.Count; i++)
            {
                PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                uint nCnt = page.GetAnnotsCount();
                for (uint j = 0; j < nCnt; j++)
                {
                    PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);

                    //annot.Flags
                    string name = annot.Name;
                    uint type = annot.Type;
                    //string strType = inst.(type.ToIntEx());

                    uint iAtomType_FileAttachment = GetStrToAtom(HxAnnotType.FileAttachment);
                    uint iAtomType_Sound = GetStrToAtom(HxAnnotType.Sound);
                    uint iAtomType_Movie = GetStrToAtom(HxAnnotType.Movie);
                    //uint nTextPopup = GetStrToAtom(HxAnnotType.Text);
                    //uint n3D = GetStrToAtom(HxAnnotType.n3D);

                    if (annot.Type == iAtomType_FileAttachment) //|| annot.Type == nSound || annot.Type == nMovie || annot.Type == nTextPopup || annot.Type == n3D
                    {
                        Result++;
                    }

                }
            }
            return Result;
        }
        public enum HxAttachmentFilleType
        {
            None,
            EmbeddedFile,
            AnnotationFile,
            Other
        }
        public struct HxEmbeddedFileRec
        {
            public const string _CDF_No_ = "No";
            public const string _CDF_AttachmentFileType_ = "EmbeddedFileType";
            public const string _CDF_FileType_ = "FileType";
            public const string _CDF_FileName_ = "FileName";
            public const string _CDF_Description_ = "Description";
            public const string _CDF_UnCompressedSize_ = "UnCompressedSize";
            public const string _CDF_CompressedSize_ = "CompressedSize";
            public const string _CDF_Size_ = "Size";
            public const string _CDF_RegDate_ = "RegDate";
            public const string _CDF_ModDate_ = "ModDate";
            public const string _CDF_Location_ = "Location";
            public const string _CDF_PageIndex_ = "PageIndex";
            public const string _CDF_EmbedIndex_ = "EmbedIndex";
            public const string _CDF_Annotation_OBJ_ = "Annotation";
            public const string _CDF_Rect_OBJ_ = "Rect";
            public const string _CDF_EmbeddedFile_OBJ_ = "EmbeddedFile";

            public const string _UDF_Location_EmbeddedFile_ = "첨부 파일";
            public const string _UDF_Location_AnnotsFile_Prefix_ = "페이지 ";

            public HxAttachmentFilleType AttachmentFileType;
            public string FileType;
            public string FileName;
            public string Description;
            public long UnCompressedSize;
            public long CompressedSize;
            public string Size;
            public int PageIndex;
            public string Location;
            public int EmbedIndex;
            public DateTime RegDate;
            public DateTime ModDate;
            public IPXC_Annotation AnnotationObj;
            public PXC_Rect RectObj;
            public IPXC_EmbeddedFileStream EmbeddedFileObj;

            public static DataTable GetDataTable(List<HxEmbeddedFileRec> list)
            {
                if (list == null || list.Count <= 0) { return null; }

                DataTable Result = new DataTable();
                Result.Columns.AddRange(new DataColumn[] {
                    new DataColumn { ColumnName = _CDF_No_, DataType = typeof(int), AutoIncrement = true, AutoIncrementSeed = 1 },
                    new DataColumn { ColumnName = _CDF_AttachmentFileType_, DataType = typeof(HxAttachmentFilleType) },
                    new DataColumn { ColumnName = _CDF_FileType_, DataType = typeof(string) },
                    new DataColumn { ColumnName = _CDF_FileName_, DataType = typeof(string) },
                    new DataColumn { ColumnName = _CDF_Description_, DataType = typeof(string) },
                    new DataColumn { ColumnName = _CDF_UnCompressedSize_, DataType = typeof(long) },
                    new DataColumn { ColumnName = _CDF_CompressedSize_, DataType = typeof(long) },
                    new DataColumn { ColumnName = _CDF_Size_, DataType = typeof(string) },
                    new DataColumn { ColumnName = _CDF_RegDate_, DataType = typeof(DateTime) },
                    new DataColumn { ColumnName = _CDF_ModDate_, DataType = typeof(DateTime) },
                    new DataColumn { ColumnName = _CDF_Location_, DataType = typeof(string) },
                    new DataColumn { ColumnName = _CDF_PageIndex_, DataType = typeof(int), DefaultValue = -1 },
                    new DataColumn { ColumnName = _CDF_EmbedIndex_, DataType = typeof(int), DefaultValue = -1 },
                    new DataColumn { ColumnName = _CDF_Annotation_OBJ_, DataType = typeof(IPXC_Annotation) },
                    new DataColumn { ColumnName = _CDF_Rect_OBJ_, DataType = typeof(PXC_Rect) },
                    new DataColumn { ColumnName = _CDF_EmbeddedFile_OBJ_, DataType = typeof(IPXC_EmbeddedFileStream) },
                });
                foreach (HxEmbeddedFileRec item in list)
                {
                    DataRow row = Result.NewRow();
                    row[_CDF_AttachmentFileType_] = item.AttachmentFileType;
                    row[_CDF_FileType_] = item.FileType;
                    row[_CDF_FileName_] = item.FileName;
                    row[_CDF_Description_] = item.Description;
                    row[_CDF_UnCompressedSize_] = item.UnCompressedSize;
                    row[_CDF_CompressedSize_] = item.CompressedSize;
                    row[_CDF_Size_] = item.Size;
                    row[_CDF_RegDate_] = item.RegDate;
                    row[_CDF_ModDate_] = item.ModDate;
                    row[_CDF_Location_] = item.Location;
                    row[_CDF_PageIndex_] = item.PageIndex;
                    row[_CDF_EmbedIndex_] = item.EmbedIndex;
                    row[_CDF_Annotation_OBJ_] = item.AnnotationObj;
                    row[_CDF_Rect_OBJ_] = item.RectObj;
                    row[_CDF_EmbeddedFile_OBJ_] = item.EmbeddedFileObj;
                    Result.Rows.Add(row);
                }
                return Result;
            }
        }
        public List<HxEmbeddedFileRec> GetEmbeddedFiles()
        {
            List<HxEmbeddedFileRec> Result = null;

            uint nEmbeddedFiles = 0;

            PDFXEdit.IPXV_Document doc = PDFCtl.Doc;

            IPXC_NameTree TreeEmbeddedFiles = doc.CoreDoc.GetNameTree("EmbeddedFiles");
            nEmbeddedFiles = TreeEmbeddedFiles.Count;
            /*
            IPXC_NameTree TreePages = Doc.CoreDoc.GetNameTree("Pages");
            uint nAttachmentsCount2 = TreePages.Count;
            Result = nAttachmentsCount > nAttachmentsCount2 ? nAttachmentsCount : nAttachmentsCount2;
            */
            if(nEmbeddedFiles > 0)
            {
                if (Result == null)
                {
                    Result = new List<HxEmbeddedFileRec>();
                }
                for (uint i = 0; i < nEmbeddedFiles; i++)
                {
                    TreeEmbeddedFiles.Item(i, out string sName, out IPXS_PDFVariant pValue);
                    if (sName.IsNullOrWhiteSpaceEx() == true || pValue == null) continue;
                    IPXC_FileSpec atch = CoreDoc.GetFileSpecFromVariant(pValue);

                    if (atch != null)
                    {
                        HxEmbeddedFileRec rec = new HxEmbeddedFileRec();
                        rec.AttachmentFileType = HxAttachmentFilleType.EmbeddedFile;
                        string strFileType = atch.EmbeddedFile.FileType;
                        rec.FileType = strFileType;
                        rec.FileName = atch.FileName;
                        rec.Description = atch.Description;
                        //rec.ModDate = atch.FieldDate["Modified"];
                        long UnCompressedSize = atch.EmbeddedFile.UnCompressedSize;
                        long CompressedSize = atch.EmbeddedFile.CompressedSize;
                        rec.UnCompressedSize = UnCompressedSize;
                        rec.CompressedSize = CompressedSize;
                        rec.Size = $"{CompressedSize.ToSize2HumanSizeStringEx()} ({UnCompressedSize.ToNumberStringEx()} bytes)";
                        DateTime ModificationDate = atch.EmbeddedFile.ModificationDate;
                        rec.RegDate = atch.EmbeddedFile.CreationDate;
                        rec.ModDate = atch.EmbeddedFile.ModificationDate;
                        rec.Location = HxEmbeddedFileRec._UDF_Location_EmbeddedFile_;
                        rec.PageIndex = -1;
                        rec.EmbedIndex = i.ToIntEx();
                        rec.AnnotationObj = null;
                        //string astr = atch.EmbeddedFile.s
                        rec.EmbeddedFileObj = atch.EmbeddedFile;
                        Result.Add(rec);
                    }
                }
            }
            PDFXEdit.PXV_Inst inst = PDFCtl?.Inst;
            PDFXEdit.IPXV_AnnotsList annotsList = inst?.CreateAnnotsList();
            if(annotsList != null)
            {
                for (uint i = 0; i < Doc.CoreDoc.Pages.Count; i++)
                {
                    PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                    uint nCnt = page.GetAnnotsCount();
                    for (uint j = 0; j < nCnt; j++)
                    {
                        PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);

                        //annot.Flags
                        string name = annot.Name;
                        uint type = annot.Type;
                        //string strType = inst.(type.ToIntEx());

                        uint iAtomType_FileAttachment = GetStrToAtom(HxAnnotType.FileAttachment);
                        uint iAtomType_Sound = GetStrToAtom(HxAnnotType.Sound);
                        uint iAtomType_Movie = GetStrToAtom(HxAnnotType.Movie);

                        if (annot.Type == iAtomType_FileAttachment)
                        {
                            if (Result == null)
                            {
                                Result = new List<HxEmbeddedFileRec>();
                            }

                            nEmbeddedFiles++;
                            IPXC_AnnotData_FileAttachment data = annot.Data as IPXC_AnnotData_FileAttachment;
                            if (data != null)
                            {
                                var atch = data.FileAttachment;
                                if (atch != null)
                                {
                                    HxEmbeddedFileRec rec = new HxEmbeddedFileRec();
                                    rec.AttachmentFileType = HxAttachmentFilleType.AnnotationFile;
                                    string strFileType = atch.EmbeddedFile.FileType;
                                    rec.FileType = strFileType;
                                    rec.FileName = atch.FileName;
                                    rec.Description = atch.Description;
                                    //rec.ModDate = atch.FieldDate["Modified"];
                                    long UnCompressedSize = atch.EmbeddedFile.UnCompressedSize;
                                    long CompressedSize = atch.EmbeddedFile.CompressedSize;
                                    rec.UnCompressedSize = UnCompressedSize;
                                    rec.CompressedSize = CompressedSize;
                                    rec.Size = $"{CompressedSize.ToSize2HumanSizeStringEx()} ({UnCompressedSize.ToNumberStringEx()} bytes)";
                                    DateTime ModificationDate = atch.EmbeddedFile.ModificationDate;
                                    rec.RegDate = atch.EmbeddedFile.CreationDate;
                                    rec.ModDate = atch.EmbeddedFile.ModificationDate;
                                    //string astr = atch.EmbeddedFile.s
                                    rec.Location = $"{HxEmbeddedFileRec._UDF_Location_AnnotsFile_Prefix_}{rec.PageIndex + 1}";
                                    rec.PageIndex = annot.PageIndex.ToIntEx();
                                    rec.EmbedIndex = -1;
                                    rec.AnnotationObj = annot;
                                    //var aaaaaaa = annot
                                    rec.RectObj = annot.get_Rect();
                                    //rec.Location = $"{rec.Rect.top}, {rec.Rect.left}";
                                    //SetSelectionAnnots()
                                    rec.EmbeddedFileObj = atch.EmbeddedFile;
                                    Result.Add(rec);
                                }
                            }
                        }
                    }
                }
            }
            
            return Result;
        }
        #region DevExpress.Pdf
        public int GetAttachemntFileCount(string strFileFullName)
        {
            int Result = 0;
            if (strFileFullName.IsNullOrWhiteSpaceEx() == true || HxFile.FileExists(strFileFullName) != true) return -1;

            using (DevExpress.Pdf.PdfDocumentProcessor pdfDocumentProcessor = new DevExpress.Pdf.PdfDocumentProcessor())
            {
                pdfDocumentProcessor.LoadDocument(strFileFullName);
                //int nAnnotsTotal = pdfDocumentProcessor.Document.Pages.Sum(r => r.Annotations.Count);
                /*
                int nAnnotsTotal = pdfDocumentProcessor.Document.Pages.Sum(r => r.Annotations.Count);
                if (nAnnotsTotal > 0)
                {
                    nAnnotsTotal = 0;
                    foreach (DevExpress.Pdf.PdfPage page in pdfDocumentProcessor.Document.Pages)
                    {
                        int iPage = page.GetPageIndex();
                        IList<DevExpress.Pdf.PdfAnnotation> annots = pdfDocumentProcessor.Document.Pages[iPage].Annotations;

                        foreach (DevExpress.Pdf.PdfAnnotation item in annots)
                        {
                            Debug.WriteLine(item.GetType());
                            if ((item as DevExpress.Pdf.PdfFileAttachmentAnnotation) != null) { continue; }
                            if ((item as DevExpress.Pdf.PdfPopupAnnotation) != null) { continue; }
                            if ((item as DevExpress.Pdf.PdfSoundAnnotation) != null) { continue; }
                            if ((item as DevExpress.Pdf.PdfMovieAnnotation) != null) { continue; }
                            nAnnotsTotal++;
                        }
                    }
                }
                */
                int nAttachmentsCount = pdfDocumentProcessor.Document.FileAttachments.Count();
                pdfDocumentProcessor.CloseDocument();
                Result = nAttachmentsCount;
            }
            return Result;
        }
        #endregion


        public static AxPXV_Control CreateAxPDFCtl(string licKeyStr, string ctlNameStr = "pdfCtl", Form.ControlCollection Controls = null)
        {
            AxPXV_Control Result = new AxPXV_Control();

            try
            {
                Result.Enabled = true;
                Result.Name = ctlNameStr.IsNullOrWhiteSpaceEx() == true ? "pdfCtl" : ctlNameStr;
                //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVdcsMngDocEditor));
                //this.pdfCtl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("pdfCtl.OcxState")));
                if (Controls != null)
                {
                    if (Controls.ContainsKey(ctlNameStr))
                    {
                        Result.Name += $"_{DateTime.Now.ToDateStringEx("yyyyMMddHHmmss")}_{HxCrypt.RandPass()}";
                    }
                    Controls.Add(Result);
                    Result.Location = new System.Drawing.Point(300, 300);
                    Result.Size = new System.Drawing.Size(400, 400);
                    Result.TabIndex = 0;
                    Result.Visible = false;
                }
                Result.CreateControl();

                if (Result != null && Result.Inst != null && licKeyStr.IsNullOrWhiteSpaceEx() != true)
                {
                    Result.SetLicKey(licKeyStr);
                    Result.Inst.Init(licKeyStr);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                throw ex;
            }
            
            return Result;
        }

        public static HxPDFXE CreateHxPDFCtl(string licKeyStr = null, string ctlNameStr = "pdfCtl", Form.ControlCollection Controls = null)
        {
            HxPDFXE Result = null;
            AxPXV_Control pdfCtl = CreateAxPDFCtl(licKeyStr, ctlNameStr, Controls);
            if (pdfCtl != null)
            {
                Result = CreateHxPDFCtl(pdfCtl, licKeyStr);
            }
            return Result;
        }

        public static HxPDFXE CreateHxPDFCtl(AxPXV_Control pdfCtl, string licKeyStr = null)
        {
            HxPDFXE Result = null;
            if (pdfCtl != null)
            {
                Result = new HxPDFXE(pdfCtl, licKeyStr);
            }
            return Result;
        }

        
    }
}
