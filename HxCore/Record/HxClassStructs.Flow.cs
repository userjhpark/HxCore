using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using System.Data;
    public struct HxFlowApiRec
    {
        public const string _NO_ATTR_ = "no";
        public const string _CATE_FLOW_NAME_ = "FLOW";

        public const string _CDF_FLOW_API_BASE_URL_ATTR_ = "FLOW_API_BASE_URL";
        public const string _CDF_FLOW_CNTS_CRTC_KEY_ATTR_ = "FLOW_CNTS_CRTC_KEY";
        public const string _CDF_FLOW_BOT_API_KEY_ATTR_ = "FLOW_BOT_API_KEY";

        public const string _CDF_FLOW_BOT_ID_TEST_ = "FLOW_BOT_ID_TEST";
        public const string _CDF_FLOW_BOT_ID_VDCS_ = "FLOW_BOT_ID_VDCS";
        public const string _CDF_FLOW_BOT_ID_NOTICE_ = "FLOW_BOT_ID_NOTICE";
        public const string _CDF_FLOW_BOT_ID_DOCSIGN_ = "FLOW_BOT_ID_DOCSIGN";
        public const string _CDF_FLOW_BOT_ID_MAIL_ = "FLOW_BOT_ID_MAIL";

        [JsonProperty(_CDF_FLOW_API_BASE_URL_ATTR_)]
        public string FLOW_API_BASE_URL         { get; set; }
        [JsonProperty(_CDF_FLOW_CNTS_CRTC_KEY_ATTR_)]
        public string FLOW_CNTS_CRTC_KEY        { get; set; }
        [JsonProperty(_CDF_FLOW_BOT_API_KEY_ATTR_)]
        public string FLOW_BOT_API_KEY          { get; set; }
        [JsonProperty(_CDF_FLOW_BOT_ID_TEST_)]
        public string FLOW_BOT_ID_TEST          { get; set; }
        [JsonProperty(_CDF_FLOW_BOT_ID_VDCS_)]
        public string FLOW_BOT_ID_VDCS          { get; set; }
        [JsonProperty(_CDF_FLOW_BOT_ID_NOTICE_)]
        public string FLOW_BOT_ID_NOTICE        { get; set; }
        [JsonProperty(_CDF_FLOW_BOT_ID_DOCSIGN_)]
        public string FLOW_BOT_ID_DOCSIGN       { get; set; }
        [JsonProperty(_CDF_FLOW_BOT_ID_MAIL_)]
        public string FLOW_BOT_ID_MAIL          { get; set; }
        public string REMOTE_ADDR { get; set; }
        public string GLOBAL_ADDR { get; set; }

        public HxFlowApiRec(bool bInit = true)
        {
            FLOW_API_BASE_URL   = null;
            FLOW_CNTS_CRTC_KEY  = null; 
            FLOW_BOT_API_KEY    = null;
            FLOW_BOT_ID_TEST    = null;
            FLOW_BOT_ID_VDCS    = null;
            FLOW_BOT_ID_NOTICE  = null;
            FLOW_BOT_ID_DOCSIGN = null;
            FLOW_BOT_ID_MAIL    = null;
            REMOTE_ADDR         = null;
            GLOBAL_ADDR         = null;
        }

        public static HxFlowApiRec Create(JToken jt, string remoteAddress = null, string globalAddress = null)
        {
            HxFlowApiRec Result = new HxFlowApiRec();
            if (jt != null && jt.HasValues && jt[_CDF_FLOW_API_BASE_URL_ATTR_] != null)
            {
                if (remoteAddress.IsNullOrWhiteSpaceEx() == true || remoteAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                {
                    remoteAddress = HxUtils.GetUserHostAddress(true);
                }
                if (globalAddress.IsNullOrWhiteSpaceEx() == true || globalAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                {
                    remoteAddress = HxUtils.GetUserGlobalAddress(true);
                }

                Result.FLOW_API_BASE_URL = jt[HxFlowApiRec._CDF_FLOW_API_BASE_URL_ATTR_]?.ToStringEx();
                Result.FLOW_CNTS_CRTC_KEY = jt[HxFlowApiRec._CDF_FLOW_CNTS_CRTC_KEY_ATTR_]?.ToStringEx();
                Result.FLOW_BOT_API_KEY = jt[HxFlowApiRec._CDF_FLOW_BOT_API_KEY_ATTR_]?.ToStringEx();


                Result.FLOW_BOT_ID_TEST = jt[HxFlowApiRec._CDF_FLOW_BOT_ID_TEST_]?.ToStringEx();
                Result.FLOW_BOT_ID_VDCS = jt[HxFlowApiRec._CDF_FLOW_BOT_ID_VDCS_]?.ToStringEx();
                Result.FLOW_BOT_ID_NOTICE = jt[HxFlowApiRec._CDF_FLOW_BOT_ID_NOTICE_]?.ToStringEx();
                Result.FLOW_BOT_ID_DOCSIGN = jt[HxFlowApiRec._CDF_FLOW_BOT_ID_DOCSIGN_]?.ToStringEx();
                Result.FLOW_BOT_ID_MAIL = jt[HxFlowApiRec._CDF_FLOW_BOT_ID_MAIL_]?.ToStringEx();

                Result.REMOTE_ADDR = remoteAddress;
                Result.GLOBAL_ADDR = globalAddress;
                //return Result;
            }
            return Result;
        }
    }
}
