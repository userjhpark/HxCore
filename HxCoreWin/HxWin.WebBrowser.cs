using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    public partial class HxWin
    {
        public const string _DEFAULT_WEB_USER_AGENT_ = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
        public const string _REGISTRY_WEBBROWSER_EMULATION_PATH_ = @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

        public static string SetWebBrowserVersionRegstry(string appName, int? ie_emulation = null)
        {
            string Result = null;
            try
            {
                if (appName.IsNullOrWhiteSpaceEx())
                {
                    appName = Process.GetCurrentProcess().ProcessName + ".exe";
                }
                if (appName.IsNullOrWhiteSpaceEx() != true)
                {
                    int ieval;
                    if (ie_emulation == null)
                    {
                        ieval = GetWebBrowserEmulationVersionNumber();
                    }
                    else
                    {
                        ieval = ie_emulation.ToIntEx();
                    }
                    Result = SetIEVersioneKeyforWebBrowserControl(appName, ieval);
                }
            }
            catch (Exception ex)
            {
                Result = ex.Message;
                //throw ex;
            }
            return Result;
            
        }

        public static int SetWebBrowserUserAgentSession(string userAgent = _DEFAULT_WEB_USER_AGENT_)
        {
            int Result = int.MinValue;
            try
            {
                if (userAgent.IsNullOrWhiteSpaceEx())
                {
                    userAgent = _DEFAULT_WEB_USER_AGENT_;
                }
                Result = SetDefaultUserAgent(userAgent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            
            return Result;
        }

        public static Version GetWebBrowserVersion()
        {
            Version Result = null;
            using (System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser())
            {
                Result = wb.Version;
            }
            return Result;
        }

        #region //출처: https://okgood0412.tistory.com/entry/C-WebBrowser-version-변경 [Gonna be happy]
        private const int _DEFAULT_WEBBROWSER_IE_EMULATION_ = 11999;
        //public static int ie_emulation = 11999;
        

        public static int GetWebBrowserEmulationVersionNumber()
        {
            int Result = _DEFAULT_WEBBROWSER_IE_EMULATION_;
            Version version = GetWebBrowserVersion();
            if (version != null)
            {
                int browserver = version.Major;

                if (browserver >= 11)
                    Result = 11001;
                else if (browserver == 10)
                    Result = 10001;
                else if (browserver == 9)
                    Result = 9999;
                else if (browserver == 8)
                    Result = 8888;
                else
                    Result = 7000;
            }
            return Result;
        }

        private static string SetIEVersioneKeyforWebBrowserControl(string appName, int ieval)
        {
            string Result = null;

            RegistryKey Regkey = null;
            try
            {
                Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_REGISTRY_WEBBROWSER_EMULATION_PATH_, true);
                //If the path is not correct or 
                //If user't have priviledges to access registry 
                if (Regkey == null)
                {
                    return "Application FEATURE_BROWSER_EMULATION Failed - Registry key Not found";
                }

                string FindAppkey = Convert.ToString(Regkey.GetValue(appName));
                //Check if key is already present 
                if (FindAppkey == ieval.ToString())
                {
                    Regkey.Close();
                    return ("Application FEATURE_BROWSER_EMULATION already set to " + ieval);
                }
                //If key is not present or different from desired, add/modify the key , key value 
                Regkey.SetValue(appName, unchecked((int)ieval), RegistryValueKind.DWord);
                //check for the key after adding 
                FindAppkey = Convert.ToString(Regkey.GetValue(appName));
                if (FindAppkey == ieval.ToString())
                {
                    Result = ("Application FEATURE_BROWSER_EMULATION changed to " + ieval + "; changes will be visible at application restart");
                }
                else
                {
                    Result = ("Application FEATURE_BROWSER_EMULATION setting failed; current value is  " + ieval);
                }

            }
            catch (Exception ex)
            {
                Result = ("Application FEATURE_BROWSER_EMULATION setting failed; " + ex.Message);
                throw ex;
            }
            finally
            {
                //Close the Registry 
                if (Regkey != null)
                    Regkey.Close();
            }
            return Result;
        }

        
        #endregion

        #region //출처: https://kanisuka.tistory.com/39 [자바의 세계로~]
        //public const string _DEFAULT_USER_AGENT_ = "Mozilla/5.0 (Linux; Android 5.0.1; SHV-E330S Build/LRX22C; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Mobile Safari/537.36";
        

        //[DllImport("urlmon.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        //public static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        private const int URLMON_OPTION_USERAGENT = 0x10000001;
        private const int URLMON_OPTION_USERAGENT_REFRESH = 0x10000002;

        private static int SetDefaultUserAgent(string userAgent)
        {
            int Result;
            try
            {
                Result = UrlMkSetSessionOption(URLMON_OPTION_USERAGENT_REFRESH, null, 0, 0);
                Result = UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, userAgent, userAgent.Length, 0);
            }
            catch (Exception ex)
            {
                Result = int.MinValue;
                throw ex;
            }
            return Result;
        }
        #endregion
    }
}
