using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win
{
    //public static class HxMessageBox<T> : IHxMessageBox
    //    where T : MessageBox
    //{
    //    #region Static Methods
    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static DialogResult Show(IWin32Window owner, string text)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    #endregion

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    DialogResult IHxMessageBox.Show(IWin32Window owner, string text)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
