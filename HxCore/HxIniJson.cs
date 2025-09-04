using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HxCore
{
    public class HxIniJson
    {
        #region CONF / Config Define Fields
        public const string _CONF_JSON_FILE_NAME_ = "Config.json";

        public const string _CONF_CATE_SETT_NAME_ = "Settings";
        public const string _CONF_CATE_DB_NAME_ = "DB";
        public const string _CONF_CATE_API_NAME_ = "API";

        public const string _CONF_MODULE_SMTP_NAME_ = "SMTP";

        public const string _CONF_MODULE_BIZ_NAME_ = "BIZ";

        public const string _CONF_MODULE_COMMON_NAME_ = "Common";

        public const string _CONF_CRYPT_KEY_NAME_ = "CryptKey";
        public const string _CONF_CRYPT_COL_NAME_ = "CryptPasswordColumn";
        public const string _CONF_CRYPT_DB_ENCODE_NAME_ = "CryptDbEncode";
        public const string _CONF_CRYPT_DB_DECODE_NAME_ = "CryptDbDecode";
        public const string _CONF_BASE64_DB_ENCODE_NAME_ = "Base64DbEncode";
        public const string _CONF_BASE64_DB_DECODE_NAME_ = "Base64DbDecode";


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
        #endregion
        private static string _CRYPT_KEY_ { get; set; }
        public bool IsIniLoad { get; protected set; }

        public string _IniJsonFileName = HxDefs._CONF_JSON_FILE_NAME_;

        public string IniJsonFullName { get; protected set; }

        public string IniJsonFileName { get => _IniJsonFileName; protected set => _IniJsonFileName = value; }

        #region Static Intance
        private static HxIniJson _instance = null;
        static HxIniJson()
        {
            _instance = Create();
        }
        public static HxIniJson Instance
        {
            get { return _instance ?? (_instance = Create()); }
            private set { _instance = value; }
        }
        internal static HxIniJson _
        {
            get { return _instance ?? (_instance = Create()); }
            private set { _instance = value; }
        }

        internal static HxIniJson Create()
        {
            return new HxIniJson();
        }

        public static bool Run(bool bInit = false)
        {
            if (_instance != null || bInit == true)
            {
                _instance = Create();
                if (_instance != null)
                    return true;
            }
            return false;
        }
        #endregion

        
        
        protected JObject GetIniFileLoad(bool bInit = false, string inputFileName = null)
        {
            JObject Result = null;
            if ((IsIniLoad != true || bInit == true) && (inputFileName.IsNullOrWhiteSpaceEx() == true && IniJsonFileName.IsNullOrWhiteSpaceEx() == true) )
            {
                string fileName = inputFileName;
                if (fileName.IsNullOrWhiteSpaceEx() != true && File.Exists(fileName) != true)
                {
                    fileName = string.Format("{0}{1}", HxUtils.AppBaseDir, inputFileName);
                }
                //if (!File.Exists(fileName))
                //{
                //    fileName = this.JsonConfilgFileName;
                //}
                if (fileName.IsNullOrWhiteSpaceEx() == true || File.Exists(fileName) != true)
                {
                    fileName = string.Format("{0}{1}", HxUtils.AppBaseDir, IniJsonFileName);
                }
                if (File.Exists(fileName))
                {
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Exists)
                    {
                        fileName = fi.Name;
                        IniJsonFileName = fi.Name;
                        IniJsonFullName = fi.FullName;
                        JObject jobject = null;
                        try
                        {
                            jobject = HxUtils.JsonFileLoad(fileName);
                            if (jobject != null && jobject[_CONF_CATE_SETT_NAME_] != null)
                            {
                                if (jobject[_CONF_CATE_SETT_NAME_][_CONF_DB_CRYPT_KEY_NAME_] != null)
                                {
                                    string strCryptKey = jobject[_CONF_CATE_SETT_NAME_][_CONF_DB_CRYPT_KEY_NAME_].ToStringEx();
                                    if (!strCryptKey.IsNullOrWhiteSpaceEx())
                                    {
                                        _CRYPT_KEY_ = HxCrypt.Decrypt(strCryptKey, "Pa$$w0rd");
                                        HxCrypt.SetCryptKey(_CRYPT_KEY_);
                                    }
                                }
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                        finally
                        {
                            Result = jobject;
                        }
                    }
                }
            }
            return Result;
        }
    }
}
