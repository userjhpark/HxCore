using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HxCore
{
    public class HxDefs
    {
        internal const string _TEMP_DIR_NAME_ = "TempAppDir";
        public const int _FILE_MAX_PATH_ = 255; //파일명은 전체 경로를 포함하여 최대 259 글자이며, 루트 폴더에 가장 긴 255 글자의 파일을 만들 수 있다.
        public const int _DIR_MAX_PATH_ = 244; //폴더명은 전체 경로를 포함하여 최대 247 글자이며, 루트 폴더에 가장 긴 244 글자의 폴더를 만들 수 있다.

        #region System Define / Const
        //public const string _CUSTOM_USER_AGENT_ = "CUSTOM_USER_AGENT";
        public const string _CUSTOM_USER_AGENT_ = "CUSTOM_USER_AGENT";
        public const string _USER_AGENT_ = "USER_AGENT";
        public const string _REMOTE_ADDR_ = "REMOTE_ADDR";
        public const string _REFERER_ = "REFERER";
        public const string _HOST_ = "HOST";
        public const string _QUERY_STRING_ = "QUERY_STRING";

        public const string _HTTP_USER_AGENT_ = "HTTP_USER_AGENT";
        public const string _HTTP_REFERER_ = "HTTP_REFERER";
        public const string _HTTP_HOST_ = "HTTP_HOST";
        public const string _REQUEST_SCHEME_ = "REQUEST_SCHEME"; //$_SERVER['REQUEST_SCHEME'] : URI 스킴 - http
        public const string _REQUEST_URI_ = "REQUEST_URI"; //$_SERVER['REQUEST_URI'] : 요청 URI. 이 페이지에 접근하기 위해 입력한 URI - /index.html
        #endregion

        #region 날짜 및 DB LOB 포멧
        public const string _FORMAT_DATETIME_ORACLE_ = "YYYY-MM-DD HH24:MI:SS";
        public const string _FORMAT_DATETIME_Csharp_ = "yyyy-MM-dd HH:mm:ss";
        public const string _PREFIX_PARAM_CLOB_ = "CLOB$$__";
        public const string _PREFIX_PARAM_BLOB_ = "BLOB$$__";
        #endregion

        #region 정규식 패턴 (Regexpr Pattern) 조건
        public const string _REGEX_BAD_NAME_PERTTERN_ = @"\\|\/|\:|\*|\?|\""|\<|\>|\|";
        public const string _REGEX_IPv4_PATTERN_ = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})$";
        public const string _REGEX_URI_PATTERN_ = @"^(?:([A-Za-z]+):)?(?:\/{0,3})([0-9.\-A-Za-z]+)(?::(\d+))?(?:\/([^?#]*))?(?:\?([^#]*))?(?:#(.*))?$";
        public const string _REGEX_EMAIL_PATTERN_ = @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?";
        public const string _REGEX_FILE_FULLNAME_START_PATTERN_ = @"^((\\\\\?\\)*([a-zA-Z]{1})(\:\\)|(\\\\)([\\w\\-\\_]+))";
        public const string _REGEX_DBPARAM_NAME_PATTERN = @"(:)(?!YYYY|MM|DD|HH|HH24|MI|SS|YY|MON|MONTH|RR|DY|AM|PM)([\w\$]+)";
        #endregion

        #region OpenAPI
        public const string _API_KEY_NAME_ = "api_key";
        public const string _API_PASS_NAME_ = "api_pass";
        #endregion

        #region CONF / Config Define Fields
        public const string _CONF_JSON_FILE_NAME_ = "Config.json";

        public const string _CONF_CATE_SETT_NAME_ = "Settings";
        public const string _CONF_CATE_DB_NAME_ = "DB";
        public const string _CONF_CATE_API_NAME_ = "API";

        public const string _CONF_MODULE_SMTP_NAME_ = "SMTP";

        public const string _MODULE_DB_NAME_            = "DB";
        public const string _MODULE_API_NAME_           = "API";
        public const string _MODULE_FLOW_NAME_          = "FLOW";


        public const string _CONF_MODULE_BIZ_NAME_ = "BIZ";

        public const string _CONF_MODULE_COMMON_NAME_ = "Common";
        public const string _CONF_MODULE_TIMESHEET_NAME_ = "Timesheet";
        public const string _CONF_MODULE_PROJ_NAME_ = "PMS";
        public const string _CONF_MODULE_DOCS_NAME_ = "DOCS";
        //public const string _CONF_MODULE_TOMAS_NAME_ = "TOMAS";
        public const string _CONF_MODULE_SUBCON_NAME_ = _CONF_MODULE_BIZ_NAME_; //"Subcon";

        public const string _CONF_CRYPT_KEY_NAME_ = "CryptKey";
        public const string _CONF_CRYPT_COL_NAME_ = "CryptPasswordColumn";
        public const string _CONF_CRYPT_DB_ENCODE_NAME_ = "CryptDbEncode";
        public const string _CONF_CRYPT_DB_DECODE_NAME_ = "CryptDbDecode";
        public const string _CONF_BASE64_DB_ENCODE_NAME_ = "Base64DbEncode";
        public const string _CONF_BASE64_DB_DECODE_NAME_ = "Base64DbDecode";

        public const string _CONF_SMTP_FROM_NAME_ = "From";
        public const string _CONF_SMTP_HOST_NAME_ = "Host";
        public const string _CONF_SMTP_PORT_NAME_ = "Port";
        public const string _CONF_SMTP_ESSL_NAME_ = "EnableSsl";
        public const string _CONF_SMTP_CRED_NAME_ = "DefaultCredentials";
        public const string _CONF_SMTP_USER_NAME_ = "UserName";
        public const string _CONF_SMTP_PASSWD_NAME_ = "Password";
        public const string _CONF_SMTP_DOMAIN_NAME_ = "HITECH";

        public const string _CONF_DB_PROVIDER_NAME_ = "Provider";
        public const string _CONF_DB_USER_NAME_ = "User";
        public const string _CONF_DB_PASSWD_NAME_ = "Password";
        public const string _CONF_DB_HOST_NAME_ = "Host";
        public const string _CONF_DB_CRYPT_KEY_NAME_ = _CONF_CRYPT_KEY_NAME_;
        //public const string _CONF_ITEM_CRYPT_KEY_NAME_ = _CONF_CRYPT_KEY_NAME_;

        public const string _CONF_API_PROVIDER_NAME_ = _CONF_DB_PROVIDER_NAME_;
        public const string _CONF_API_KEY_NAME_ = "Key";
        public const string _CONF_API_PASSWD_NAME_ = "Password";
        public const string _CONF_API_HOST_NAME_ = "Host";

        public const string _ATTR_DB_PROVIDER_ = "Provider";
        public const string _ATTR_DB_HOST_     = "Host";
        public const string _ATTR_DB_PORT_     = "Port";
        public const string _ATTR_DB_USER_     = "User";
        public const string _ATTR_DB_PASSWD_   = "Password";
        public const string _ATTR_DB_POOLING_  = "Pooling";

        public const string _ATTR_NO_       = "No";
        public const string _ATTR_NAME_     = "Name";
        public const string _ATTR_TITLE_    = "Title";
        public const string _ATTR_DESC_     = "Description";

        public const string _ATTR_API_KEY_ = "Key";
        public const string _ATTR_API_PASSWD_ = "Password";
        public const string _ATTR_API_HOST_ = "Host";
        public const string _ATTR_API_REMOTE_SERVICE_ = "RemoteService";

        public const string _ATTR_FILE_DOWNLOAD_    = "FileDownload";
        public const string _ATTR_INTRANET_PATTERN_ = "IntranetPattern";
        public const string _ATTR_INTRANET_REMOTE_SERVICE_ = "IntranetRemoteService";
        public const string _ATTR_EXTRANET_REMOTE_SERVICE_ = "ExtranetRemoteService";
        #endregion

        #region RESTful 관련 정의
        public const string _CONTENT_TYPE_KEY_ = "Content-Type";
        public const string _CONTENT_TYPE_APPLICATION_JSON_ = "application/json";
        public const string _CONTENT_TYPE_APPLICATION_FORM_URLENCODED_ = "application/x-www-form-urlencoded";

        public const string _FROM_ENCTYPE_MULTIPART_FORM_DATA_ = "multipart/form-data";
        #endregion

        #region PDF
        public const string _PDF_GTS_PDFA1_ = "GTS_PDFA1";
        public const string _PDF_GTS_PDFA2_ = "GTS_PDFA2";
        public const string _PDF_GTS_PDFA3_ = "GTS_PDFA3";
        #endregion

    }
}
