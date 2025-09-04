using PDFXEdit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win.PDFXE
{
    partial class HxPDFXE
    {
        

        //public const string _PXD_Docs_OpenDocInNewWnd_ = "Docs.OpenDocInNewWnd";

        //private string FGeneralAppTitle;
        public string GeneralAppTitle
        {
            get
            {
                return (string)this.GetOptionSettingItemValue(_PXD_General_AppTitle_);
            }
            protected set
            {
                //this.FGeneralAppTitle = value;
                this.SetOptionSettingItemValue(_PXD_General_AppTitle_, value);
            }
        }
        public void SetGeneralAppTitle(string appName)
        {
            this.GeneralAppTitle = appName;
            //string str = (string)PXVCtl.Inst.Settings[_General_AppTitle_].v;
            //str = PXVCtl.Inst.Settings[].v;
            
        }

        public bool? IsUserSettings {
            get;
            protected set;
        }
        public const string _DefalutOptionSettingFileName_ = "Settings.dat"; //"Settings.xcs";
        private string OptionUserSettingFileName = "Settings.dat"; 
        public void SetUserSettings(bool bEnable, string fileName = null)
        {
            this.IsUserSettings = bEnable;
            this.OptionUserSettingFileName = fileName ?? _DefalutOptionSettingFileName_;
        }
        public bool IsOptionSettingsHistory()
        {
            try
            {
                //PXVCtl.Inst.Settings[]
            }
            catch { }
            return false;
        }
        public void ExportOptionSettingsSave(string settingFileName = null)
        {
            string strSettFileName = settingFileName.IsNullOrWhiteSpaceEx() ? OptionUserSettingFileName : settingFileName;
            try
            {
                if (this.IsUserSettings == true)
                {
                    //PXVCtl.Inst.SaveUserSettings(CreateString(strSettFileName), 0);
                    PDFXEdit.IOperation op = this.AxPXVCtl.Inst.CreateOp(pdfCtl.Inst.Str2ID("op.settings.export"));
                    //op.Params.Root["Options.History"].v = ckSettIncHist.Checked;
                    op.Params.Root["Input"].v = PXFsFInst.DefaultFileSys.StringToName(strSettFileName);
                    op.Do();
                }
            }
            catch { }
        }
        public void ImportOptionSettingsLoad(string settingFileName = null)
        {
            string strSettFileName = settingFileName.IsNullOrWhiteSpaceEx() ? OptionUserSettingFileName : settingFileName;
            try
            {
                if (!File.Exists(strSettFileName))
                {
                    this.ExportOptionSettingsSave(settingFileName);
                }
                if (File.Exists(strSettFileName))
                {
                    //PXVCtl.Inst.LoadUserSettings(CreateString(strSettFileName), 0);
                    PDFXEdit.IOperation op = AxPXVCtl.Inst.CreateOp(AxPXVCtl.Inst.Str2ID("op.settings.import"));
                    //op.Params.Root["Options.History"].v = ckSettIncHist.Checked;
                    op.Params.Root["Input"].v = PXFsFInst.DefaultFileSys.StringToName(strSettFileName);
                    op.Do();
                }
            }
            catch { }
        }
        public object GetOptionSettingItemValue(string optionName)
        {
            object Result = null;
            try
            {
                Result = AxPXVCtl.Inst.Settings[optionName].v;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw ex;
            }
            return Result;
        }
        public virtual void SetOptionSettingItemValue(string optionName, object value)
        {
            try
            {
                AxPXVCtl.Inst.Settings[optionName].v = value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }
        }

        public bool? GetOptionItemMultipleDocMode()
        {
            try
            {
                return !(bool?)this.GetOptionSettingItemValue(_PXD_Docs_SingleWnd_);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }
            return null;
        }
        /// <summary>
        /// Multiple Documents Mode
        /// </summary>
        /// <param name="value">Multiple : Tue / Single : False</param>
        public void SetOptionItemMultipleDocMode(bool value = true)
        {
            //PXVCtl.Inst.Settings[_Docs_SingleWnd_].v = !bFlag;
            this.SetOptionSettingItemValue(_PXD_Docs_SingleWnd_, !value);
            AxPXVCtl.Inst.FireAppPrefsChanged(PDFXEdit.PXV_AppPrefsChanges.PXV_AppPrefsChange_Documents);
            // disable 'dock/undock/reorder' feature for main panes:
            AxPXVCtl.Inst.ActiveMainView.Panes?.Layout?.Obj?.SetStyle((Int64)PDFXEdit.UIX_LayoutStyleFlags.UIX_LayoutStyle_PanesNoReorder, (Int64)PDFXEdit.UIX_LayoutStyleFlags.UIX_LayoutStyle_PanesNoReorder);
            AxPXVCtl.Inst.ActiveMainView.DocViewsArea?.Panes?.Layout?.Obj?.SetStyle((Int64)PDFXEdit.UIX_LayoutStyleFlags.UIX_LayoutStyle_PanesNoReorder, (Int64)PDFXEdit.UIX_LayoutStyleFlags.UIX_LayoutStyle_PanesNoReorder);
        }
        public bool? GetOptionItemHideSingleTab()
        {
            return (bool?)this.GetOptionSettingItemValue(_PXD_Docs_HideSingleTab_);
        }
        public void SetOptionItemHideSingleTab(bool value)
        {
            this.SetOptionSettingItemValue(_PXD_Docs_HideSingleTab_, value);
        }
        public bool? GetOptionItmNewWindow()
        {
            return (bool?)this.GetOptionSettingItemValue(_PXD_Docs_HideSingleTab_);
        }


        #region Drag&Drop
        public virtual void SetFileOpenDragAndDrop(Control sender, bool bFlag = true)
        {
            sender.AllowDrop = bFlag;
            //if (bFlag == true)
            //{
            //    pdfCtl.AllowDrop = true;
            //    pdfCtl.Drag
            //}
            //else
            //{
            //    pdfCtl.AllowDrop = false;
            //}
            //pdfCtl.DoDragDrop
            sender.DragDrop += DoFileOpenControl_DragDrop;
            sender.DragEnter += DoFileOpenControl_DragEnter;
            try
            {
                //pdfCtl.Inst.Settings["Docs.CanOpenByDrop"].v = bFlag;
                this.SetOptionSettingItemValue(_PXD_Docs_CanOpenByDrop_, bFlag);
                AxPXVCtl.Inst.FireAppPrefsChanged(PDFXEdit.PXV_AppPrefsChanges.PXV_AppPrefsChange_Documents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private void DoFileOpenControl_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    e.Effect = DragDropEffects.Copy | DragDropEffects.Scroll;
            //}
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy; // Okay
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
        }

        private void DoFileOpenControl_DragDrop(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            //    foreach (string str in file)
            //    {
            //        MessageBox.Show(str);
            //    }
            //}
            // Extract the data from the DataObject-Container into a string list
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            // Do something with the data...

            // For example add all files into a simple label control:
            foreach (string file in files)
            {
                if (System.IO.File.Exists(file))
                {
                    string fileExt = System.IO.Path.GetExtension(file).ToLower();
                    //if (fileExt == ".pdf")//File.ToLower().EndsWith(".pdf")
                    //{
                    //    //SBPdfCtl.
                    //    //pdfCtl.DoDragDrop(File, DragDropEffects.Copy);
                    //    MessageBox.Show(file);
                    //}
                    //this.label.Text += File + "\n";
                    try
                    {
                        this.AxPXVCtl.OpenDocFromPath(file);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        //throw;
                    }

                }
            }
        }
        #endregion
    }
}
