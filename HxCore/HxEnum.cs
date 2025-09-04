using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace HxCore
{
    // <summary>
    /// Enum Class
    /// </summary>
    public class HxEnum : HxBase
    {
        #region Static Intance
        private static HxEnum _instance = null;
        static HxEnum()
        {
            _instance = new HxEnum();
        }
        /// <summary>
        /// [Static]Instance Object
        /// </summary>
        public static HxEnum Instance
        {
            get { return _instance ?? (_instance = new HxEnum()); }
            private set { _instance = value; }
        }
        #endregion

        /// <summary>
        /// Get Class Name
        /// </summary>
        public override string GetName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        }

        /// <summary>
        /// 포함 여부(int형으로 변환 하여 비교)
        /// </summary>
        /// <param name="sourceType">원본 Enum Type</param>
        /// <param name="type">찾을 Enum Type</param>
        /// <returns></returns>
        public bool IsTypeIn(Object source, Object find)
        {
            bool Result = false;
            try
            {
                if (source.GetType() == find.GetType() && ((int)source & (int)find) != 0)
                {
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                string exName = string.Format("{0}.{1} : {2}", Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                Result = false;
                //throw new dnException(ex);
            }

            return Result;
        }
        public bool IsEnumIn(Object source, Object find)
        {
            return this.IsTypeIn(source, find);
        }


        public static string GetEnumName(Enum input)
        {
            return Enum.GetName(input.GetType(), input);
        }
        
        public static TEnumType ConverToEnum<TEnumType>(string input)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), input);
        }
        public static string GetDescriptionAttr<T>(T input)
        {
            FieldInfo fi = input.GetType().GetField(input.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return input.ToString();
        }
        public static HxDbProviderType GetDbProviderType(string providerStr)
        {
            HxDbProviderType Result = HxDbProviderType.Null;
            if (providerStr.IsNullOrWhiteSpaceEx() != true)
            {
                string providerTypeLowerString = providerStr.ToLower();
                switch (providerTypeLowerString)
                {
                    case "oci":
                    case "oracle":
                    case "oci8":
                        Result = HxDbProviderType.OCI;
                        break;
                    case "pgsql":
                    case "npgsql":
                    case "postgresql":
                    case "postgis":
                        Result = HxDbProviderType.PostgreSQL;
                        break;
                    case "sqlite":
                        Result = HxDbProviderType.SQLite;
                        break;
                    case "sqlsrv":
                    case "mssql":
                    case "sql server":
                    case "ms-sql":
                        Result = HxDbProviderType.MsSQL;
                        break;
                    case "mysql":
                    case "mysqli":
                    case "my-sql":
                        Result = HxDbProviderType.Common;
                        break;
                    case "access":
                    case "mdb":
                    case "accdb":
                        Result = HxDbProviderType.Access;
                        break;
                    case "excel":
                    case "xls":
                    case "xlsx":
                        Result = HxDbProviderType.Excel;
                        break;
                    default:
                        //cubrid, firebird, informix // https://www.php.net/manual/en/pdo.drivers.php
                        Result = HxDbProviderType.Common;
                        break;
                }
            }
            return Result;
        }

        public static string GetDbProviderProtocol(HxDbProviderType providerType)
        {
            string Result = null;
            switch (providerType)
            {
                case HxDbProviderType.Null:
                    Result = string.Empty;
                    break;
                case HxDbProviderType.OCI:
                    Result = "oci";
                    break;
                case HxDbProviderType.MsSQL:
                    Result = "sqlsrv"; //sqlsrv / DBLIB : mssql, sybase, dblib
                    break;
                case HxDbProviderType.SQLite:
                    Result = "sqlite";
                    break;
                case HxDbProviderType.PostgreSQL:
                    Result = "pgsql";
                    break;
                case HxDbProviderType.Excel:
                case HxDbProviderType.Access:
                    Result = "odbc";
                    break;
                case HxDbProviderType.MySQL:
                    Result = "mysql";
                    break;
            }

            return Result;
        }


        public static string GetServiceProviderProtocol(HxServiceProviderType providerType)
        {
            string Result = null;

            bool bDefined = Enum.IsDefined(typeof(HxDbProviderType), providerType.ToIntEx());
            if (bDefined == true)
            {
                HxDbProviderType dbProviderType = (HxDbProviderType)providerType;
                Result = GetDbProviderProtocol(dbProviderType);
            }
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                switch (providerType)
                {
                    case HxServiceProviderType.Http:
                        Result = "http";
                        break;
                    case HxServiceProviderType.Https:
                        Result = "https";
                        break;
                    case HxServiceProviderType.SSH:
                        Result = "ssh";
                        break;
                    case HxServiceProviderType.TELNET:
                        Result = "telent";
                        break;
                    case HxServiceProviderType.FTP:
                    case HxServiceProviderType.FTP_Data:
                        Result = "ftp";
                        break;
                    case HxServiceProviderType.SMTP:
                        Result = "smtp";
                        break;
                    case HxServiceProviderType.SMTP_SSL:
                    case HxServiceProviderType.SMTP_SSL2:
                        Result = "smtps";
                        break;
                    case HxServiceProviderType.IMAP:
                        Result = "imap";
                        break;
                    case HxServiceProviderType.IMAP_SSL:
                        Result = "impas";
                        break;
                    case HxServiceProviderType.POP3:
                        Result = "pop3";
                        break;
                    case HxServiceProviderType.POP3_SSL:
                        Result = "pop3s";
                        break;
                    case HxServiceProviderType.NNTP:
                        Result = "nntp";
                        break;
                    case HxServiceProviderType.NNTP_SSL:
                        Result = "nntps";
                        break;
                    case HxServiceProviderType.LDAP:
                        Result = "ldap";
                        break;
                    case HxServiceProviderType.LDAP_SSL:
                        Result = "ldaps";
                        break;
                    case HxServiceProviderType.SMB:
                        Result = "smb";
                        break;
                    case HxServiceProviderType.PPTP:
                        Result = "pptp";
                        break;
                    case HxServiceProviderType.Terminal:
                        Result = "mstsc";
                        break;
                    case HxServiceProviderType.File:
                        Result = "file";
                        break;
                }
            }
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                Result = providerType.ToStringEx();
            }
            return Result;
        }

        public static string GetServiceProviderProtocol(int defaultPort)
        {
            string Result = null;
            bool bDefined = Enum.IsDefined(typeof(HxServiceProviderType), defaultPort);
            if (bDefined == true)
            {
                HxServiceProviderType providerType = (HxServiceProviderType)defaultPort;
                Result = GetServiceProviderProtocol(providerType);
            }
            return Result;
        }
        public static string GetProtocolStartOnly(string protocolStr, bool isToLower = true)
        {
            string Result = protocolStr?.RegexReplaceEx(@"(\:\/\/)$", string.Empty);
            if(Result.IsNullOrWhiteSpaceEx() != true && isToLower == true)
            {
                Result = Result.ToLower();
            }
            return Result;
        }
        public static HxServiceProviderType GetServiceProviderType(string protocolStr)
        {
            HxServiceProviderType Result = HxServiceProviderType.Null;
            string strProtocol = GetProtocolStartOnly(protocolStr);
            switch (strProtocol)
            {
                case "oci":
                case "oracle":
                    Result = HxServiceProviderType.OCI;
                    break;
                case "mssql":
                case "ms-sql":
                    Result = HxServiceProviderType.MsSQL;
                    break;
                case "postgre":
                case "postsql":
                case "postgresql":
                    Result = HxServiceProviderType.PostgreSQL;
                    break;
                case "mmsql":
                case "my-sql":
                    Result = HxServiceProviderType.MySQL;
                    break;
                case "mariadb":
                    Result = HxServiceProviderType.MariaDB;
                    break;
                case "sqllite":
                    Result = HxServiceProviderType.SQLite;
                    break;
                case "excel":
                case "csv":
                    Result = HxServiceProviderType.Excel;
                    break;
                case "access":
                    Result = HxServiceProviderType.Access;
                    break;

                case "http":
                    Result = HxServiceProviderType.Http;
                    break;
                case "https":
                    Result = HxServiceProviderType.Https;
                    break;

                case "ftp":
                    Result = HxServiceProviderType.FTP;
                    break;
                case "ssh":
                    Result = HxServiceProviderType.SSH;
                    break;
                case "sftp":
                    Result = HxServiceProviderType.SFTP;
                    break;
                case "telnet":
                    Result = HxServiceProviderType.TELNET;
                    break;

                case "smtp":
                    Result = HxServiceProviderType.SMTP;
                    break;
                case "imap":
                    Result = HxServiceProviderType.IMAP;
                    break;
                case "pop3":
                    Result = HxServiceProviderType.POP3;
                    break;
                case "ldap":
                    Result = HxServiceProviderType.LDAP;
                    break;

                case "smtps":
                case "smtp_ssl":
                    Result = HxServiceProviderType.SMTP_SSL;
                    break;
                case "imaps":
                case "imap_ssl":
                    Result = HxServiceProviderType.IMAP_SSL;
                    break;
                case "pop3s":
                case "pop3_ssl":
                    Result = HxServiceProviderType.POP3_SSL;
                    break;
                case "ldaps":
                case "ldap_ssl":
                    Result = HxServiceProviderType.LDAP_SSL;
                    break;

                case "file":
                    Result = HxServiceProviderType.File;
                    break;
                default:
                    Result = HxServiceProviderType.Null;
                    break;
            }
            return Result;
        }
        public static HxServiceProviderType GetServiceProviderType(int servicePort)
        {
            HxServiceProviderType Result = HxServiceProviderType.Null;
            try
            {
                bool bExists = Enum.IsDefined(typeof(HxServiceProviderType), servicePort);
                if (bExists == true)
                {
                    Result = (HxServiceProviderType)servicePort;
                    //Enum.TryParse<HxServiceProviderType>(providerPort, out Result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static HxServiceProviderType GetServiceProviderType(HxDbProviderType providerType)
        {
            return GetServiceProviderType((int)providerType);
        }
        public static int GetServiceDefaultPort(HxServiceProviderType providerType)
        {
            int Result = int.MinValue;
            switch (providerType)
            {
                case HxServiceProviderType.OCI:
                    Result = 1521;
                    break;
                case HxServiceProviderType.MsSQL:
                    Result = 1433;
                    break;
                case HxServiceProviderType.PostgreSQL:
                    Result = 5432;
                    break;
                case HxServiceProviderType.MySQL:
                    Result = 3306;
                    break;
                default:
                    Result = providerType.ToIntEx(0);
                    break;
            }
            return Result;
        }
        public static int GetServiceDefaultPort(string providerStr)
        {
            HxServiceProviderType providerType = GetServiceProviderType(providerStr);
            return GetServiceDefaultPort(providerType);
        }
    }
}
