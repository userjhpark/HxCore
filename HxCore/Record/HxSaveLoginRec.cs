using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore
{
    public struct HxSaveLoginRec
    {
        public const string _FILE_SAVELOGIN_NAME_ = "SaveLogin.json";
        private const int _DECODE_LENGTH_ = 32;

        public const string _CATE_SaveLogin_NAME_       = "SaveLogin";
        public const string _PROP_RemoteSource_NAME_    = "RemoteSource";
        public const string _PROP_RemoteServcie_NAME_   = "RemoteService";
        public const string _PROP_LoginID_NAME_         = "LoginID";
        public const string _PROP_Machine_NAME_         = "Machine";
        public const string _PROP_Password_NAME_        = "Password";
        public const string _PROP_PIN_NAME_             = "PIN";
        public const string _PROP_FirstDate_NAME_       = "FirstDate";
        public const string _PROP_LastDate_NAME_        = "LastDate";
        public const string _FORMAT_DateTIME_STRING_    = "yyyy-MM-dd HH:mm:ss";


        private JObject JCustomRoot { get; set; }
        private JToken JCustomProps { get; set; }
        public string FullName { get; set; }

        private string RemoteSource { get; set; }
        private string RemoteService { get; set; }

        private string SaveLoginID { get; set; }
        private string SaveMachineID { get; set; }
        private string SavePassword { get; set; }
        private string SavePIN { get; set; }

        private DateTime? SaveFirstDate { get; set; }
        private DateTime? SaveLastDate { get; set; }

        private static string _MachineID = null;
        public static string MachineID
        {
            get
            {
                if (_MachineID.IsNullOrWhiteSpaceEx() == true)
                {
                    _MachineID = HxUtils.GetUserUniqueID();
                }
                return _MachineID;
            }
            private set
            {
                _MachineID = value;
            }
        }

        public bool IsLoadComplete
        {
            get
            {
                if(FullName.IsNullOrWhiteSpaceEx() != true && SaveLoginID.IsNullOrWhiteSpaceEx() != true && SaveMachineID.IsNullOrWhiteSpaceEx() != true && SavePassword.IsNullOrWhiteSpaceEx() != true && SavePIN.IsNullOrWhiteSpaceEx() != true)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsMachineCompare
        {
            get
            {
                if (MachineID.IsNullOrWhiteSpaceEx() != true && SaveLoginID.IsNullOrWhiteSpaceEx() != true && SaveMachineID.IsNullOrWhiteSpaceEx() != true )
                {
                    string strDecrypt = HxCrypt.Decrypt(SaveMachineID, MachineID);
                    if (MachineID == strDecrypt)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsSuccess
        {
            get
            {
                if(IsLoadComplete == true && IsMachineCompare == true)
                {
                    string strDecodePassword = GetLoginPassword();
                    string strDecodePIN = GetLoginPIN();
                    if (strDecodePassword.IsNullOrWhiteSpaceEx() != true && strDecodePIN.IsNullOrWhiteSpaceEx() != true)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public HxSaveLoginRec(string fullName = null, string machineID = null)
        {
            _MachineID = null;
            JCustomRoot = null;
            JCustomProps = null;
            FullName = null;

            RemoteSource = null;
            RemoteService = null;

            SaveLoginID = null;
            SaveMachineID = null;
            SavePassword = null;
            SavePIN = null;
            SaveFirstDate = null;
            SaveLastDate = null;

            MachineID = machineID;

            if (MachineID.IsNullOrWhiteSpaceEx() == true)
            {
                MachineID = HxUtils.GetUserUniqueID();
            }
            
            if (fullName.IsNullOrWhiteSpaceEx() != true)
            {
                FullName = GetFileLoad(fullName);
            }
        }

        public string GetFileLoad(string fileName)
        {
            string Result = null;

            string strFullName = HxFile.GetFileFullPath(fileName);
            if (strFullName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(strFullName) == true)
            {
                JCustomRoot = HxUtils.JsonFileLoad(strFullName);
                JCustomProps = JCustomRoot[_CATE_SaveLogin_NAME_];
                if (JCustomProps != null)
                {
                    if (JCustomProps[_PROP_RemoteSource_NAME_] != null)
                    {
                        this.RemoteSource = JCustomProps[_PROP_RemoteSource_NAME_].ToStringEx();
                    }
                    if (JCustomProps[_PROP_RemoteServcie_NAME_] != null)
                    {
                        this.RemoteService = JCustomProps[_PROP_RemoteServcie_NAME_].ToStringEx();
                    }

                    if (JCustomProps[_PROP_LoginID_NAME_] != null)
                    {
                        this.SaveLoginID = JCustomProps[_PROP_LoginID_NAME_].ToStringEx();
                    }
                    if (JCustomProps[_PROP_Password_NAME_] != null)
                    {
                        this.SavePassword = JCustomProps[_PROP_Password_NAME_].ToStringEx();
                    }
                    if (JCustomProps[_PROP_Machine_NAME_] != null)
                    {
                        this.SaveMachineID = JCustomProps[_PROP_Machine_NAME_].ToStringEx();
                    }
                    if (JCustomProps[_PROP_PIN_NAME_] != null)
                    {
                        this.SavePIN = JCustomProps[_PROP_PIN_NAME_].ToStringEx();
                    }
                    if (JCustomProps[_PROP_FirstDate_NAME_] != null)
                    {
                        try
                        {
                            this.SaveLastDate = JCustomProps[_PROP_FirstDate_NAME_]?.ToStringEx()?.ToDateTimeEx(_FORMAT_DateTIME_STRING_);
                        }
                        catch (Exception ex)
                        {
                            this.SaveLastDate = null;
                            Debug.WriteLine(ex);
                            //throw ex;
                        }
                    }
                    if (JCustomProps[_PROP_LastDate_NAME_] != null)
                    {
                        try
                        {
                            this.SaveLastDate = JCustomProps[_PROP_LastDate_NAME_]?.ToStringEx()?.ToDateTimeEx(_FORMAT_DateTIME_STRING_);
                        }
                        catch (Exception ex)
                        {
                            this.SaveLastDate = null;
                            Debug.WriteLine(ex);
                            //throw ex;
                        }
                    }
                }
                Result = fileName;
            }
            return Result;
        }

        public string GetDataSource()
        {
            return this.RemoteSource;
        }
        public string GetRemoteService()
        {
            return this.RemoteService;
        }

        public string GetLoginID()
        {
            return this.SaveLoginID;
        }
        public string GetLoginPIN()
        {
            string Result = null;
            if (IsLoadComplete == true && IsMachineCompare == true)
            {
                string strDecode = HxCrypt.Decrypt(SavePIN, MachineID);
                if (strDecode.IsNullOrWhiteSpaceEx() != true && strDecode.Length == _DECODE_LENGTH_)
                {
                    Result = strDecode;
                }
            }
            return Result;
        }
        public string GetLoginPassword()
        {
            string Result = null;
            if(IsLoadComplete == true && IsMachineCompare == true)
            {
                string strDecode = HxCrypt.Decrypt(SavePassword, MachineID);
                if(strDecode.IsNullOrWhiteSpaceEx() != true && strDecode.Length == _DECODE_LENGTH_)
                {
                    Result = strDecode;
                }
            }
            return Result;
        }

        public DateTime? GetLoginLastDate()
        {
            DateTime? Result = null;
            if (IsLoadComplete == true && IsMachineCompare == true)
            {
                Result = SaveLastDate;
            }
            return Result;
        }

        public string SetFileWrite(string loginID, string machineUniqueID, string pinCode, string password, string fileName = _FILE_SAVELOGIN_NAME_, string remoteSource = "-", string remoteService = "None")
        {
            string Result = null;

            if (fileName.IsNullOrWhiteSpaceEx() == true || loginID.IsNullOrWhiteSpaceEx() == true || machineUniqueID.IsNullOrWhiteSpaceEx() == true || pinCode.IsNullOrWhiteSpaceEx() == true || password.IsNullOrWhiteSpaceEx() == true) return Result;

            string strFullName = HxFile.GetFileFullPath(fileName);

            if(strFullName.IsNullOrWhiteSpaceEx() == true)
            {
                strFullName = fileName;
            }

            if (strFullName.IsNullOrWhiteSpaceEx() != true)
            {
                if (HxFile.FileExists(strFullName) == true)
                {
                    HxFile.FileDelete(strFullName, false);
                }
                if (HxFile.FileExists(strFullName) != true)
                {
                    machineUniqueID = machineUniqueID.Trim();
                    try
                    {
                        string strSaveLoginID = loginID.Trim();
                        string strSaveMachine = machineUniqueID.Trim();
                        string strSavePIN = pinCode.Trim();
                        string strSavePassword = password.Trim();
                        string strNowDateTime = DateTime.Now.ToDateTimeStringEx(_FORMAT_DateTIME_STRING_);

                        if (strSavePIN.Length != _DECODE_LENGTH_)
                        {
                            strSavePIN = HxCrypt.Md5(strSavePIN);
                        }
                        if (strSavePassword.Length != _DECODE_LENGTH_)
                        {
                            strSavePassword = HxCrypt.Md5(strSavePassword);
                        }

                        strSaveMachine = HxCrypt.Encrypt(strSaveMachine, machineUniqueID);
                        strSavePIN = HxCrypt.Encrypt(strSavePIN, machineUniqueID);
                        strSavePassword = HxCrypt.Encrypt(strSavePassword, machineUniqueID);

                        JObject jAttr = new JObject
                        {
                            { _PROP_LoginID_NAME_, strSaveLoginID },
                            { _PROP_Machine_NAME_, strSaveMachine },
                            { _PROP_PIN_NAME_, strSavePIN },
                            { _PROP_Password_NAME_, strSavePassword },
                            { _PROP_FirstDate_NAME_,  strNowDateTime },
                            { _PROP_LastDate_NAME_,  strNowDateTime }
                        };
                        if(remoteSource != null)
                        {
                            jAttr.Add(_PROP_RemoteSource_NAME_, remoteSource);
                        }
                        if(RemoteService != null)
                        {
                            jAttr.Add(_PROP_RemoteServcie_NAME_, remoteService);
                        }

                        JObject jRoot = new JObject(new JProperty(_CATE_SaveLogin_NAME_, jAttr));

                        HxFile.SetTextFileWriteAllText(strFullName, jRoot.ToString());

                        if (HxFile.FileExists(strFullName))
                        {
                            Result = strFullName;
                        }
                    }
                    catch (Exception ex)
                    {
                        Result = null;
                        throw ex;
                    }
                }
            }
            return Result;
        }

        public void SetNowToLastDate(string remoteSource = null, string remoteService = null)
        {
            if (IsSuccess == true && JCustomRoot != null && JCustomProps != null)
            {
                string strNowDateTime = DateTime.Now.ToDateTimeStringEx(_FORMAT_DateTIME_STRING_);
                JCustomProps[_PROP_LastDate_NAME_] = strNowDateTime;
                try
                {
                    if(remoteSource != null)
                    {
                        JCustomProps[_PROP_RemoteSource_NAME_] = remoteSource;
                    }
                    if (remoteService != null)
                    {
                        JCustomProps[_PROP_RemoteServcie_NAME_] = remoteService;
                    }
                    HxFile.SetTextFileWriteAllText(FullName, JCustomRoot.ToString());
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }
        }
    }
}
