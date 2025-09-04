using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore.Exchange
{
    using Microsoft.Exchange.WebServices.Data;
    public partial class HxExchangeService
    {
        //참고 : https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd633696(v%3dexchg.80)
        protected ExchangeService ExchangeApp;
        #region 생성자
        public HxExchangeService()
        {
            this.ExchangeApp = new ExchangeService();
        }
        public HxExchangeService(ExchangeVersion exchangeVersion, ITraceListener traceListener = null, bool? bTraceEnabled = null, TraceFlags traceFlagType = TraceFlags.All)
        {
            this.ExchangeApp = new ExchangeService(exchangeVersion);
            if(this.ExchangeApp != null)
            {
                this.SetTraceSetting(traceListener, bTraceEnabled, traceFlagType);
            }
        }
        public HxExchangeService(ExchangeVersion exchangeVersion, string emailAddress, string username, string password, string domain = null)
            : this(exchangeVersion)
        {
            if (this.ExchangeApp != null)
            {
                if (username.IsNullOrWhiteSpaceEx() != true)
                {
                    this.SetCredentials(username, password, domain);
                    this.ExchangeApp.TraceEnabled = true;
                    this.ExchangeApp.TraceFlags = TraceFlags.All;
                }

                if (!emailAddress.IsNullOrWhiteSpaceEx())
                {
                    this.SetAutodiscoverUrl(emailAddress, RedirectionUrlValidationCallback);
                }
            }
        }
        #endregion

        #region 구현??
        protected ExchangeCredentials GetCredentials(string username, string password, string domain = null)
        {
            ExchangeCredentials Result = null;
            if (username.IsNullOrWhiteSpaceEx() != true)
            {
                if (domain.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = new WebCredentials(username, password, domain);
                }
                else
                {
                    Result = new WebCredentials(username, password);
                }
            }
            return Result;
        }
        public void SetCredentials(string username, string password, string domain = null)
        {
            if(this.ExchangeApp != null)
            {
                this.ExchangeApp.Credentials = this.GetCredentials(username, password, domain);
            }
        }

        public void SetUrl(string url)
        {
            if(this.ExchangeApp != null && url.IsNullOrWhiteSpaceEx() != true)
            {
                //https://computername.domain.contoso.com/EWS/Exchange.asmx
                this.ExchangeApp.Url = new Uri(url);
            }
        }
        public void SetAutodiscoverUrl(string emailAddress, Microsoft.Exchange.WebServices.Autodiscover.AutodiscoverRedirectionUrlValidationCallback validateRedirectionUrlCallback = null)
        {
            if (this.ExchangeApp != null && emailAddress.IsNullOrWhiteSpaceEx() != true)
            {
                // Set the URL.
                if(validateRedirectionUrlCallback != null)
                    this.ExchangeApp.AutodiscoverUrl(emailAddress, validateRedirectionUrlCallback);
                else
                    this.ExchangeApp.AutodiscoverUrl(emailAddress);
            }
        }

        protected virtual bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        protected void SetTraceSetting(ITraceListener traceListener = null, bool? bTraceEnabled = null, TraceFlags traceFlagType = TraceFlags.None)
        {
            if(this.ExchangeApp != null)
            {
                if (traceListener != null)
                {
                    this.ExchangeApp.TraceListener = traceListener;
                    if(bTraceEnabled == null)
                        bTraceEnabled = true;
                    if (traceFlagType == TraceFlags.None)
                        traceFlagType = TraceFlags.EwsRequest | TraceFlags.EwsResponse;
                }
                this.ExchangeApp.TraceEnabled = (bTraceEnabled == null ? true : bTraceEnabled.ToConvertEx<bool>());
                this.ExchangeApp.TraceFlags = (bTraceEnabled == null && traceFlagType == TraceFlags.None  ? TraceFlags.EwsRequest | TraceFlags.EwsResponse : traceFlagType);// TraceFlags.All;
            }
        }
        #endregion

        #region Cutom Util Method
        public string GetVersionToProductName(string inputVersion)
        {
            string Result = null;
            if (inputVersion.IsNullOrWhiteSpaceEx() != true)
            {
                Version buildVersion = new Version(inputVersion);
                Result = GetVersionToProductName(buildVersion);
            }
            return Result;
        }
        public string GetVersionToProductName(Version version)
        {
            //참조 : https://social.technet.microsoft.com/wiki/contents/articles/240.exchange-server-and-update-rollup-build-numbers.aspx
            string Result = null;
            if(version != null)
            {
                switch (version.Major)
                {
                    case 6:
                        Result = "Microsoft Exchange Server 2003"; //6.5.6944
                        if (version.Minor == 5)
                        {
                            if (version.Build >= 7226 && version.Build < 7638)
                            {
                                Result = "Microsoft Exchange Server 2003 SP1"; //6.5.7226
                            } else if(version.Build >= 7638){
                                Result = "Microsoft Exchange Server 2003 SP2"; //6.5.7638
                            }
                        }
                        break;
                    case 8:
                        Result = "Microsoft Exchange Server 2007"; //8.0.685.24, 8.0.685.25
                        if (version.Minor == 1)
                        {
                            Result = "Microsoft Exchange Server 2007 SP1"; //8.1.240.6
                        }
                        else if (version.Minor == 2)
                        {
                            Result = "Microsoft Exchange Server 2007 SP2"; //8.2.176.2
                        }
                        else if (version.Minor == 3)
                        {
                            Result = "Microsoft Exchange Server 2007 SP3"; //8.3.083.6
                        }
                        else if(version.Minor > 3)
                        {
                            Result = "Microsoft Exchange Server 2007 SP3 or later";
                        }
                        break;
                    case 14:
                        Result = "Microsoft Exchange Server 2010"; //14.0.639.21
                        if (version.Minor == 1)
                        {
                            Result = "Microsoft Exchange Server 2010 SP1"; //14.1.218.15
                        }
                        else if (version.Minor == 2)
                        {
                            Result = "Microsoft Exchange Server 2010 SP2"; //14.2.247.5
                        }
                        else if (version.Minor == 3)
                        {
                            Result = "Microsoft Exchange Server 2010 SP3"; //14.3.123.4
                        }
                        else if (version.Minor > 3)
                        {
                            Result = "Microsoft Exchange Server 2010 SP3 or later"; //14.3.123.4
                        }
                        break;
                    case 15:
                        Result = "Microsoft Exchange Server 2013"; //15.0.516.32
                        if (version.Minor == 1)
                        {
                            Result = "Microsoft Exchange Server 2016"; //15.1.225.42
                        }
                        else if (version.Minor == 2)
                        {
                            Result = "Microsoft Exchange Server 2019"; //15.1.225.42
                        }
                        else if (version.Minor > 2)
                        {
                            Result = "Microsoft Exchange Server 2019 or later"; //15.1.225.42
                        }
                        break;
                    default:
                        Result = "Unknown";
                        break;
                }
            }
            return Result;
        }
        #endregion
    }
}
