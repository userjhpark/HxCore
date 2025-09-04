using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win
{
    public partial class HxClickOnce
    {
        public enum CosUpdateResultType
        {
            None,
            DeploymentDownloadException,
            InvalidDeploymentException,
            InvalidOperationException,

        }
        
        public static CosUpdateResultType InstallUpdateSyncWithInfo(bool bErrorMessageShow = true)
        {
            CosUpdateResultType Result = CosUpdateResultType.None;
            UpdateCheckInfo info = null;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    if(bErrorMessageShow == true)
                        MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return CosUpdateResultType.DeploymentDownloadException;
                }
                catch (InvalidDeploymentException ide)
                {
                    if (bErrorMessageShow == true)
                        MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return CosUpdateResultType.InvalidDeploymentException;
                }
                catch (InvalidOperationException ioe)
                {
                    if (bErrorMessageShow == true)
                        MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return CosUpdateResultType.InvalidOperationException;
                }

                if (info.UpdateAvailable)
                {
                    bool bUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            bUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        
                    }

                    if (bUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            if (bErrorMessageShow == true)
                                MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return CosUpdateResultType.DeploymentDownloadException;
                        }
                    }
                }
            }
            return Result;
        }
    }
}
