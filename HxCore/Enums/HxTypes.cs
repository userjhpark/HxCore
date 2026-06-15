using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HxCore
{
    public partial class HxType : HxBase
    {
        public static string GetEnumMemberValue<T>(T value)
            where T : struct, IConvertible
        {
            //출처 : https://stackoverflow.com/questions/27372816/how-to-read-the-value-for-an-enummember-attribute
            string Result = typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;

            if(Result.IsNullOrWhiteSpaceEx() == true)
            {
                var enumType = typeof(T);
                var memInfo = enumType.GetMember(value.ToString());
                var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    Result = attr.Value;
                }
            }
            if(Result.IsNullOrWhiteSpaceEx() == true)
            {
                Result = value.ToString();
            }
            return Result;
        }
    }
    
    /// <summary>
    /// Platform Type
    /// </summary>
    public enum HxPlatformBitType
    {
        Unknown,
        X86,
        X64,
        IA64 = X64
    }

    /// <summary>
    /// String 타입 체크용
    /// </summary>
    [Flags]
    public enum HxCheckStringType
    {
        Null = 1 << 0,
        Empty = 1 << 1,
        Space = 1 << 2,
        All = HxCheckStringType.Null | HxCheckStringType.Empty | HxCheckStringType.Space
    }

    /// <summary>
    /// Mode Type
    /// </summary>
    [Flags]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxModeType
    {
        [Description("미 지정")]
        [EnumMember(Value = "none")]
        None = 1 << 0,
        [Description("조회")]
        [EnumMember(Value = "read")]
        Read = 1 << 1,
        [Description("생성")]
        [EnumMember(Value = "create")]
        Create = 1 << 2,
        [Description("수정")]
        [EnumMember(Value = "update")]
        Update = 1 << 3,
        [Description("삭제")]
        [EnumMember(Value = "delete")]
        Delete = 1 << 4,
        [Description("Edit")]
        [EnumMember(Value = "writer")]
        Writer = (HxModeType.None | HxModeType.Create | HxModeType.Read | HxModeType.Update),
        [Description("Write")]
        [EnumMember(Value = "editor")]
        Editor = (HxModeType.None | HxModeType.Create | HxModeType.Update | HxModeType.Delete),
        [Description("Aministrator")]
        [EnumMember(Value = "administrator")]
        Administrator = (HxModeType.None | HxModeType.Create | HxModeType.Read | HxModeType.Update | HxModeType.Delete),
        [Description("ALL")]
        [EnumMember(Value = "all")]
        All = Administrator,
        [EnumMember(Value = "write")]
        Write = Writer,
        [EnumMember(Value = "edit")]
        Edit = Editor,
        [EnumMember(Value = "admin")]
        Admin = Administrator,
        [EnumMember(Value = "nil")]
        Nil = None,
        [EnumMember(Value = "null")]
        Null = None
    }

    public enum HxFormModeType
    {
        [Description("미 지정")]
        None,
        List,

        Write,
        View,
        Modify,
        Remove,

        //DB, Action
        Create = Write,
        Read = View,
        Update = Modify,
        Delete = Remove,

        New = Write,
        Info = View,
        Edit = Modify,
        Erase = Remove,
        
    }
    /// <summary>
    /// Form Dialog Type (MDI, SID별 Show() 또는 ShowDialog() 선언)
    /// </summary>
    public enum HxFormDialogType
    {
        [Description("미 지정")]
        None,
        [Description("MDI 자식 폼")]
        MDIChild,
        [Description("MDI 맨 앞 자식 폼")]
        MDIChildTypeTopMost,
        [Description("SDI 자식 폼")]
        SDIChild,
        [Description("SDI 맨 앞 자식 폼")]
        SDIChildTypeTopMost,
        [Description("Show Modal(맨 상위 단일 폼")]
        ShowModal
    }

    /// <summary>
    /// History(Log) Type
    /// </summary>
    public enum HxMessageType
    {
        [Description("미 지정")]
        None = 0,
        [Description("메세지(단순 텍스트)")]
        Message,
        [Description("History(Log)")]
        History,
        [Description("성공")]
        OK,
        [Description("오류")]
        Error,
        [Description("확인")]
        Question,
        [Description("정보(Asterisk)")]
        Information,
        [Description("경고(Exclamation)")]
        Warning,
        [Description("알림")]
        Notice,
        [Description("예외처리(Exception)")]
        Exception,
        [Description("시작")]
        Begin,
        [Description("종료")]
        End,
        [Description("(제어문)정지")]
        Break,
        [Description("로그인")]
        Login,
        [Description("로그아웃")]
        Logout,
        [Description("연결")]
        Connection,
        [Description("연결 종료")]
        Disconnection,
        [Description("추가")]
        Create,
        [Description("조회")]
        Read,
        [Description("수정")]
        Update,
        [Description("삭제")]
        Delete,
        [Description("Application Start")]
        Start,
        [Description("Application Exit")]
        Exit,
        [Description("Application Info")]
        Info = Information,
        [Description("Application Stop")]
        Stop = Exit

    }

    /// <summary>
    /// Export File Type
    /// </summary>
    public enum HxExportType
    {
        [Description("None")]
        None,
        [Description("Text (Tab) format")]
        Text,
        [Description("Comma Separated Value format")]
        Csv,
        [Description("Rich Text Format")]
        Rtf,
        [Description("Microsoft Excel 2003 이하")]
        Excel,
        [Description("Microsoft Excel 2007 이상")]
        ExcelX,
        [Description("Microsoft Word 2003 이하")]
        Word,
        [Description("Microsoft Word 2007 이상")]
        WordX,
        [Description("HyperText Mark-up Language")]
        Html,
        [Description("Portable Document Format")]
        Pdf,
        [Description("GIF Image Format")]
        GIF,
        [Description("JPEG Image Format")]
        JPEG,
        [Description("PNG Image Format")]
        PNG,
        [Description("BMP Image Format")]
        BMP,
        [Description("Tiff Image Format")]
        TIF
    }

    /// <summary>
    /// 사용 되는 Database Type
    /// </summary>
    [Flags]
    public enum HxDatabaseType
    {
        [Description("None")]
        None = 0,
        [Description("Text (Tab) format")]
        TXT, //= 1 << 0,
        [Description("Comma Separated Value format")]
        CSV, //= 1 << 1,
        [Description("Microsoft Excel")]
        Excel, //= 1 << 2,
        [Description("Microsoft Excel 2007 이상")]
        ExcelX, //= 1 << 3,
        [Description("Microsoft Access")]
        Access, //= 1 << 4,
        [Description("Microsoft Access 2007 이상")]
        AccessX, // = 1 << 5,
        [Description("Microsoft SQL Server")]
        MSSQL = 1433,
        [Description("Oracle Database Server")]
        OCI = 1521, // = 1 << 7,
        [Description("My-SQL Database Server")]
        MySQL = 3306, // = 1 << 8,
        [Description("My-SQL InnoDB Server")]
        MySQLi = MySQL, //  = 1 << 9,
        [Description("SQLite Local Database")]
        SQLite, // = 1 << 10,
        [Description("PostgreSQL Database")]
        PostgreSQL = 5432, //  = 1 << 11,
        [Description("Microsoft SQL Server 2000")]
        MSSQL2000 = MSSQL,
        [Description("Oracle 9i Or Older Database Server")]
        Oracle = OCI,
        [Description("My-SQL 3.x Or Older Database Server")]
        MySQL3 = MySQL,
    }

    /// <summary>
    /// Database Provider DLL 타입
    /// </summary>
    [TypeConverterAttribute(typeof(HxEnumConverter)), DefaultValue(HxDbProviderType.Common)]
    public enum HxDbProviderType
    {
        Null,
        /// <summary>
        /// Database Common
        /// </summary>
        [Description("Syste.Data.Common")]
        Common = HxDatabaseType.None,
        /// <summary>
        /// Oracle Database(Oracle.ManagedDataAccess.Client)
        /// </summary>
        [Description("Oracle.ManagedDataAccess.Client")]
        OCI = HxDatabaseType.OCI,
        /// <summary>
        /// Oracle Database(Oracle.DataAccess.Client)
        /// </summary>
        [Description("Oracle.DataAccess.Client")]
        Oracle = HxDatabaseType.Oracle,
        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        [Description("System.Data.SqlClient")]
        MsSQL = HxDatabaseType.MSSQL,
        /// <summary>
        /// SQLite Database
        /// </summary>
        [Description("System.Data.SQLite")]
        SQLite = HxDatabaseType.SQLite,
        /// <summary>
        /// PostgreSQL Database
        /// </summary>
        [Description("Npgsql.dll")]
        PostgreSQL = HxDatabaseType.PostgreSQL,
        /// <summary>
        /// Microsoft Excel
        /// </summary>
        [Description("System.Data.Common")]
        Excel = HxDatabaseType.Excel,
        /// <summary>
        /// Microsoft Access
        /// </summary>
        [Description("System.Data.Common")]
        Access = HxDatabaseType.Access,

        MySQL = HxDatabaseType.MySQL,
        MariaDB = HxDatabaseType.MySQL,
    }

    public enum HxServiceProviderType
    {
        Null,
        Common = HxDbProviderType.Common,
        OCI = HxDatabaseType.OCI,
        Oracle = OCI,
        MsSQL = HxDatabaseType.MSSQL,
        SQLite = HxDatabaseType.SQLite,
        PostgreSQL = HxDatabaseType.PostgreSQL,
        Excel = HxDatabaseType.Excel,
        Access = HxDatabaseType.Access,
        MySQL = HxDatabaseType.MySQL,
        MariaDB = MySQL,

        
        Http = 80,
        Https = 443,

        SSH = 22,
        TELNET = 23,

        FTP = 21,
        FTP_Data = 20,
        SFTP = SSH,

        SMTP = 25,
        SMTP_SSL = 465,
        SMTP_SSL2 = 587,

        IMAP = 143,
        IMAP_SSL = 993,
        POP3 = 110,
        POP3_SSL = 995,

        NNTP = 119,
        NNTP_SSL = 563,

        LDAP = 389,
        LDAP_SSL = 636,

        SMB = 445,

        PPTP = 1723,

        Terminal = 3389,
        MSTSC = Terminal,

        File// = int.MaxValue
    }

    /// <summary>
    /// 상태 현황
    /// </summary>
    public enum HxSatusType
    {
        [Description("미 지정")]
        None,
        [Description("승인")]
        Yes,
        [Description("대기")]
        Pause,
        [Description("거부")]
        No,
        [Description("완료")]
        Ok = Yes,
        [Description("취소")]
        Cancel = No,
        [Description("대기")]
        Wait = Pause,
    }

    /// <summary>
    /// Application 사용자 타입
    /// </summary>
    [FlagsAttribute]
    [DefaultValue(HxUserAuthType.None)]
    [System.ComponentModel.TypeConverter(typeof(HxEnumConverter))]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxUserAuthType
    {
        //[System.ComponentModel.Description("미 지정"), System.ComponentModel.DXDescription("NONE")]
        [System.ComponentModel.Description("미 지정")]
        [EnumMember(Value = "미지정")]
        None = 1 << 0,
        //[System.ComponentModel.Description("손님"), System.ComponentModel.DXDescription("GUEST")]
        [System.ComponentModel.Description("손님")]
        [EnumMember(Value = "손님")]
        Guest = 1 << 1,
        [System.ComponentModel.Description("협력사")]
        [EnumMember(Value = "협력사")]
        Subcon = 1 << 2,
        [System.ComponentModel.Description("고객사")]
        [EnumMember(Value = "고객사")]
        Client = 1 << 3,
        //[System.ComponentModel.Description("조회자"), System.ComponentModel.DXDescription("VIEWER")]
        [System.ComponentModel.Description("조회자")]
        [EnumMember(Value = "조회자")]
        Viewer = 1 << 4,
        //[System.ComponentModel.Description("수정자"), System.ComponentModel.DXDescription("EDITOR")]
        [System.ComponentModel.Description("수정자")]
        [EnumMember(Value = "수정자")]
        Editor = 1 << 5,
        //[System.ComponentModel.Description("사용자"), System.ComponentModel.DXDescription("MEMBER")]
        [System.ComponentModel.Description("사용자")]
        [EnumMember(Value = "사용자")]
        Member = 1 << 6,
        //[System.ComponentModel.Description("사용자"), System.ComponentModel.DXDescription("MEMBER")]
        [System.ComponentModel.Description("LE")]
        [EnumMember(Value = "LE")]
        LE = 1 << 7,
        //[System.ComponentModel.Description("사용자"), System.ComponentModel.DXDescription("MEMBER")]
        [System.ComponentModel.Description("PM")]
        [EnumMember(Value = "PM")]
        PM = 1 << 8,
        [System.ComponentModel.Description("운영")]
        [EnumMember(Value = "운영")]
        Management = 1 << 9,
        [System.ComponentModel.Description("임원")]
        [EnumMember(Value = "임원")]
        Director = 1 << 10,
        //[System.ComponentModel.Description("관리자"), System.ComponentModel.DXDescription("MANAGER")]
        [System.ComponentModel.Description("관리자")]
        [EnumMember(Value = "관리자")]
        Manager = 1 << 11,
        //[System.ComponentModel.Description("Admininstrator"), System.ComponentModel.DXDescription("ADMIN")]
        [System.ComponentModel.Description("Admininstrator")]
        [EnumMember(Value = "Admininstrator")]
        Admin = 1 << 12,
        //[System.ComponentModel.Description("Super Administrator"), System.ComponentModel.DXDescription("SUPER")]
        [System.ComponentModel.Description("Super Administrator")]
        [EnumMember(Value = "Super Administrator")]
        SuperAdmin = HxUserAuthType.None | HxUserAuthType.Guest | HxUserAuthType.Viewer | HxUserAuthType.Editor | HxUserAuthType.Member | HxUserAuthType.LE | HxUserAuthType.PM | HxUserAuthType.Management | HxUserAuthType.Director | HxUserAuthType.Admin
    }

    /// <summary>
    /// Application Service 연결 타입
    /// </summary>
    [System.ComponentModel.TypeConverter(typeof(HxEnumConverter))]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxNetworkConnectionType
    {
        [System.ComponentModel.Description("미 지정")]
        None,
        [System.ComponentModel.Description("내부 네트워크")]
        IN,
        [System.ComponentModel.Description("VPN 네트워크")]
        INwithVPN,
        [System.ComponentModel.Description("외부 네트워크")]
        OUT
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxResultType
    {
        None,
        Success,
        Fail,
        Error,
        Exception
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxResultMessageType
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "Fail")]
        Fail,
        [EnumMember(Value = "Error")]
        Error,
        [EnumMember(Value = "Exception")]
        Exception,
        [EnumMember(Value = "Not Support")]
        NotSupport,
        [EnumMember(Value = "Not Found")]
        NotFound,
        [EnumMember(Value = "Not Authorized")]
        NotAuthorized,
        [EnumMember(Value = "Previous Data Exists")]
        PreviousDataExists
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxLoginValidResult
    {
        [Description("미 지정")]
        None,
        [Description("성공")]
        Success,
        [Description("실패")]
        Fail,
        [Description("다수")]
        Multiple,
        [Description("패스워드 불일치")]
        PasswordNotMatch,
        [Description("사용 제한")]
        NotUse

    }

    [TypeConverter(typeof(HxEnumConverter))]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxCryptType
    {
        None, Hidden, Crypt, MD5, RandPass, Base64, ExportCrypt, ExportMD5
    }

    [DefaultValue(HxDataType.None)]
    public enum HxDataType
    {
        //
        // 요약:
        //     미지정
        None = 0,
        //
        // 요약:
        //     사용자 지정 데이터 형식을 나타냅니다.
        Custom = 99,
        //
        // 요약:
        //     날짜와 시간으로 표현 된 시간 내에 시간을 나타냅니다.
        DateTime = 1,
        //
        // 요약:
        //     날짜 값을 나타냅니다.
        Date = 2,
        //
        // 요약:
        //     시간 값을 나타냅니다.
        Time = 3,
        //
        // 요약:
        //     개체가 이미 있는 연속 시간을 나타냅니다.
        Duration = 4,
        //
        // 요약:
        //     전화 번호 값을 나타냅니다.
        PhoneNumber = 5,
        //
        // 요약:
        //     통화 값을 나타냅니다.
        Currency = 6,
        //
        // 요약:
        //     표시 되는 텍스트를 나타냅니다.
        Text = 7,
        //
        // 요약:
        //     HTML 파일을 나타냅니다.
        Html = 8,
        //
        // 요약:
        //     여러 줄 텍스트를 나타냅니다.
        MultilineText = 9,
        //
        // 요약:
        //     전자 메일 주소를 나타냅니다.
        EmailAddress = 10,
        //
        // 요약:
        //     암호(비밀번호) 값을 나타냅니다.
        Password = 11,
        //
        // 요약:
        //     URL 값을 나타냅니다.
        Url = 12,
        //
        // 요약:
        //     이미지에 URL을 나타냅니다.
        ImageUrl = 13,
        //
        // 요약:
        //     신용 카드 번호를 나타냅니다.
        CreditCard = 14,
        //
        // 요약:
        //     우편 번호를 나타냅니다.
        PostalCode = 15,
        //
        // 요약:
        //     파일 업로드 데이터 형식을 나타냅니다.
        Upload = 16,
        //
        // 요약:
        //     Base64 Encode 값을 나타냅니다.
        Base64Text = 17,
        //
        // 요약:
        //     암호화(Crypt) 값을 나타냅니다.
        CryptPassword = 18,
        //
        // 요약:
        //     암호화 Hash MD5 값을 나타냅니다.
        CryptHashMD5 = 19,
        //
        // 요약:
        //     숫자
        Number = 51,
        //
        // 요약:
        //     실수
        Double = 52,

        Array = 61,
        List = 62,
        DataTable = 71,
        DataSet = 72
    }

    [Flags]
    public enum HxFileOverwriteType
    {
        None = 0,
        OverWrite,
        RenameSequence,
        RenameDateTime,
        RenameDateMicroTime,
    }

    public enum HxMultiplePosition
    {
        None,
        First,
        Last,
        All
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxUseType
    {
        None,  //미지정
        N,  //사용 안함
        C,  //취소
        H,  //HOLD
        P,  //심사 중
        Y,  //사용
        A   //전체
    }
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxStatusType
    {
        N, //None, No(Not)

        X, //실행 (eXecute)
        V, //검증 (Verify)
        L, //로드 (Load)
        O, //열기 (Open)

        J, //반송, reJect
        S, //발신 중, 보내기, Set/Send/Save
        G, //수신 중, 가져오기, Get
        
        H, //Hold
        T, //임시, Temp
        I, //진행 중, ~Ing
        W, //대기 중, Wait
        P, //일시 정지(검토 중, closed), Pause

        C, //쓰기 (Create, Write) / 취소(Cancel)
        R, //읽기 (Read)
        U, //변경 (Update), 개선(Modify) / 이동 (Move) / 업로드 (Upload) / 제출 sUbmit
        D, //삭제 (erease, Delete, remove) / 다운로드 (Download)

        F, //완료, Final(reply)

        Z, //종료(Final Submit)
        A, //전체, ALL

        CC, //취소 (CanCel)
        DN, //다운로드(DowNload)
        UP, //업로드(UPload)
        SU, //제출 (SUbmit)
        RE, //답변 (REply)
        DE, //게시 (DEploy)
        FW, //전달 (ForWard)
        RS, //공개 (ReleaSe)
        DU, //디버그 (DebUg)

        Y  //사용(전송), YES
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxJobStateType
    {
        //진행 상태(Y:진행/H:HOLD/C:취소/S:완료/T:임시/N:사용안함)
        N,
        C,
        H,
        S,
        T,
        Y,
        YS,
        A
    }
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HxHttpMethodType
    {
        NONE = 1 << 0,
        GET = 1 << 1,
        POST = 1 << 2,
        HEADER = 1 << 3,
        COOKIE = 1 << 4,
        GETPOST = GET | POST,
        ALL = GET | POST | HEADER | COOKIE
    }

    public enum HxRemoteServiceType
    {
        None = -1,
        ConnectionFail = 0,
        WebApi,
        DirectDb,
        WebPage,
        FileServer,
        ETC = 99,
        Database = DirectDb,
    }

    public enum HxVersionType
    {
        None = -1,
        All = 0,
        Major,
        Minor,
        Build,
        Revision
    }

    partial class HxType
    {
        public static HxRemoteServiceType GetRemoteServiceType(string strValue)
        {
            HxRemoteServiceType Result;
            switch (strValue?.ToUpper())
            {
                case "DIRECTDB":
                case "DIRECT":
                case "DB":
                case "DBMS":
                case "DATABASE":
                case "DIRECT-DB":
                    Result = HxRemoteServiceType.DirectDb;
                    break;
                case "WEBAPI":
                case "OPENAPI":
                case "HTTP":
                case "REST":
                case "API":
                case "WEB":
                    Result = HxRemoteServiceType.WebApi;
                    break;
                case "CONNECTIONFAIL":
                case "FAIL":
                case "ERROR":
                case "EXCEIPTION":
                    Result = HxRemoteServiceType.ConnectionFail;
                    break;
                case "NONE":
                case "-1":
                case "NULL":
                case "NIL":
                case "EMPT":
                case null:
                    Result = HxRemoteServiceType.None;
                    break;
                case "ETC":
                case "OTHER":
                default:
                    Result = HxRemoteServiceType.ETC;
                    break;
            }
            return Result;
        }
    }
}
