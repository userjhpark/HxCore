using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HxCore;

//using TDbFacoty = MongoDB.Driver.Core.Connections.IConnectionFactory;
//using TDbConnection = Oracle.ManagedDataAccess.Client.OracleConnection;
//using TDbTransaction = Oracle.ManagedDataAccess.Client.OracleTransaction;
//using TDbCommand = Oracle.ManagedDataAccess.Client.OracleCommand;
//using TDbParameter = Oracle.ManagedDataAccess.Client.OracleParameter;
//using TDbDataReader = Oracle.ManagedDataAccess.Client.OracleDataReader;
//using TDbDataAdapter = Oracle.ManagedDataAccess.Client.OracleDataAdapter;
//using TDbException = Oracle.ManagedDataAccess.Client.OracleException;
//using TDbOracleClob = Oracle.ManagedDataAccess.Types.OracleClob;
//using TDbOracleBlob = Oracle.ManagedDataAccess.Types.OracleBlob;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using MongoDB.Bson;

namespace HxCore.Data
{
    public class HxDbMongo : IHxDb
    {
        protected Dictionary<string, string> ConnectionOptions { get; set; }
        protected MongoClientSettings MongoSettings { get; set; }
        protected MongoClient MongoClient { get; set; }
        protected IMongoDatabase MongoDatabase { get; set; }
        public BsonDocument MongoDefaultCollection { get; protected set; }

        public HxDbMongo() 
        {
            ; ;
            //예시 : https://jacking75.github.io/DB_MongDB_MongoDBCSharpVer2/
        }

        public string GetConnectionString(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            string Result = null; // $@"mongodb+srv://<id>:<password>@<host>/?retryWrites=true&w=majority&appName=<name>";
            string[] arryUriStr = database.SplitEx("://");

            string strProtocal = arryUriStr[0];
            if (arryUriStr == null || arryUriStr.Length <= 2) return Result;

            string strHostDatabaseOption = arryUriStr[1];
            if (strHostDatabaseOption.IsNullOrWhiteSpaceEx() == true) return Result;

            string[] arrQueryStr = arryUriStr[arryUriStr.Length - 1].SplitEx("/?");
            
            if (arrQueryStr != null && arrQueryStr.Length > 1)
            {
                var arrQueryStr2 = arrQueryStr[1].SplitEx("&");
                if (arrQueryStr2 != null && arrQueryStr2.Length > 0)
                {
                    ConnectionOptions = new Dictionary<string, string>();
                    foreach (var item in arrQueryStr2)
                    {
                        var arrQueryStr3 = item.SplitEx("=");
                        if (arrQueryStr3 != null && arrQueryStr3.Length > 0)
                        {
                            ConnectionOptions.AddEx(arrQueryStr3[0], arrQueryStr3[1]);
                        }
                    }
                }
            }

            Result = $@"{strProtocal}://{userID}:{password}@{strHostDatabaseOption}";
            Debug.WriteLine(ConnectionOptions.Count);
            return Result;
        }

        public void Connect(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            try
            {
                string strConnectString = GetConnectionString(userID, password, database, character, pooling);
                if (strConnectString.IsNullOrWhiteSpaceEx() == true) return;

                Connect(strConnectString);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public void Connect(string connectionString)
        {
            MongoSettings = MongoClientSettings.FromConnectionString(connectionString);
            // Set the ServerApi field of the settings object to set the version of the Stable API on the client
            MongoSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            MongoClient = new MongoClient(MongoSettings);
            // Send a ping to confirm a successful connection
        }

        public void SetDatabaseName(string dbName, string collectionName)
        {
            MongoDatabase = MongoClient.GetDatabase(dbName);
            if(collectionName.IsNullOrWhiteSpaceEx() != true)
            {
                MongoDefaultCollection = (GetDatabaseCollection<BsonDocument>(collectionName) as BsonDocument);
            }
        }
        public IMongoCollection<T> GetDatabaseCollection<T>(string collectionName)
        {
            
            IMongoCollection<T> Result = MongoDatabase.GetCollection<T>(collectionName);
            return Result;
        }

        public bool BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public bool Close(int queryID = int.MinValue)
        {
            throw new NotImplementedException();
        }

        public bool ColumnContains(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        public bool Commit()
        {
            throw new NotImplementedException();
        }

        

        public bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects)
        {
            throw new NotImplementedException();
        }

        public int CurrID(string sequecesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool EndTransaction()
        {
            throw new NotImplementedException();
        }

        public object f(string columnName, int parse = -1)
        {
            throw new NotImplementedException();
        }

        public object f(int columnIndex, int parse = -1)
        {
            throw new NotImplementedException();
        }

        public void Free()
        {
            throw new NotImplementedException();
        }

        public int GetColumnCount(int parse = -1)
        {
            throw new NotImplementedException();
        }

        public bool GetColumnNameToLower()
        {
            throw new NotImplementedException();
        }

        public Type GetColumnType(string columnName, int parse = -1)
        {
            throw new NotImplementedException();
        }

        public Type GetColumnType(int columnIndex, int parse = -1)
        {
            throw new NotImplementedException();
        }

        

        public string GetParamterSeparatorChar()
        {
            throw new NotImplementedException();
        }

        public HxDbProviderType GetProviderType()
        {
            throw new NotImplementedException();
        }

        public int GetRowCount(int parse = -1)
        {
            throw new NotImplementedException();
        }

        public string GetSchemaName()
        {
            throw new NotImplementedException();
        }

        public object GetValue(string columnName, int parse = -1)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int columnIndex, int parse = -1)
        {
            throw new NotImplementedException();
        }

        public void Halt(string message)
        {
            throw new NotImplementedException();
        }

        public bool IsTransaction()
        {
            throw new NotImplementedException();
        }

        public bool Lock(string tableName, HxModeType mode = HxModeType.Write)
        {
            throw new NotImplementedException();
        }

        public List<HxDbColumnRec> MetaData(string tableName)
        {
            throw new NotImplementedException();
        }

        public int NextID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto)
        {
            throw new NotImplementedException();
        }

        public bool NextRecord(int parse = -1)
        {
            throw new NotImplementedException();
        }

        public int nf()
        {
            throw new NotImplementedException();
        }

        public string NowDateValue(string dateFormatString = null)
        {
            throw new NotImplementedException();
        }

        public string np()
        {
            throw new NotImplementedException();
        }

        public bool Open()
        {
            throw new NotImplementedException();
        }

        public string p(string columnName, int parse = -1)
        {
            throw new NotImplementedException();
        }

        public string p(int columnIndex, int parse = -1)
        {
            throw new NotImplementedException();
        }

        public int Query(string queryString, Dictionary<string, object> bind = null)
        {
            throw new NotImplementedException();
        }

        public int Query(string queryString, IDataParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable QueryDataTable(string queryString, Dictionary<string, object> bind, bool bStoredProcedure = false)
        {
            throw new NotImplementedException();
        }

        public DataTable QueryDataTable(string queryString, IDataParameter[] parameters = null, bool bStoredProcedure = false)
        {
            throw new NotImplementedException();
        }

        public DataTable QueryDataTable(string queryString, string subWhereString, Dictionary<string, object> bind = null, bool bStoredProcedure = false)
        {
            throw new NotImplementedException();
        }

        public object QueryOne(string queryString, Dictionary<string, object> bind = null)
        {
            throw new NotImplementedException();
        }

        public object QueryOne(string queryString, IDataParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool Rollback()
        {
            throw new NotImplementedException();
        }

        public bool SequenceContains(string name)
        {
            throw new NotImplementedException();
        }

        public void SetColumnNameToLower(bool bLower)
        {
            throw new NotImplementedException();
        }

        public void SetConnectionString(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            throw new NotImplementedException();
        }

        public void SetConnectionString(string connString)
        {
            throw new NotImplementedException();
        }

        public void SetDebugMode(bool bDebug)
        {
            throw new NotImplementedException();
        }

        public void SetOptions(HxDbOptionRec option)
        {
            throw new NotImplementedException();
        }

        public bool SynonymContains(string name)
        {
            throw new NotImplementedException();
        }

        public bool TableContains(string name)
        {
            throw new NotImplementedException();
        }

        public List<HxDbTableRec> TableNames()
        {
            throw new NotImplementedException();
        }

        public bool UnLock()
        {
            throw new NotImplementedException();
        }

        public DataTable UserColumns()
        {
            throw new NotImplementedException();
        }

        public DataTable UserTables()
        {
            throw new NotImplementedException();
        }

        public bool ViewContains(string name)
        {
            throw new NotImplementedException();
        }

        public List<HxDbTableRec> ViewNames()
        {
            throw new NotImplementedException();
        }
    }
    /*
    public class HxDbMongo : HxDbA<TDbFacoty, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDbDataReader, TDbDataAdapter>
    {
        public override string GetName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            //throw new NotImplementedException();
        }
        public override string ParamterSeparatorChar => ":";


        #region Static Intance
        //private static HxDbOci _instance = null;
        //static HxDbOci()
        //{
        //    _instance = new HxDbOci();
        //}
        ///// <summary>
        ///// [Static]Instance Object
        ///// </summary>
        //public static HxDbOci Instance
        //{
        //    get { return _instance ?? (_instance = new HxDbOci()); }
        //    private set { _instance = value; }
        //}
        #endregion
        #region 생성자
        public static HxDbOci Create()
        {
            return new HxDbOci();
        }
        /// <summary>
        /// 생성자
        /// </summary>
        public HxDbOci()
            : base(HxDbProviderType.OCI)
        {
            this.InitVarTypes();
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionResource">Connection Resource</param>
        public HxDbOci(TDbConnection connectionResource)
            :base(connectionResource)
        {
            //this.isConnRef = false;
            //this.Conn = connectionResource as TConnection;
            //if (this.Conn != null)
            //{
            //    this.isConnRef = true;
            //}
            this.InitVarTypes();

        }

        public HxDbOci(HxDbConnectionRec connection)
            : base(connection.ProviderType, connection.User, connection.Password, connection.HostName, connection.Character)
        {
            ; ;
        }

        public static HxDbOci Create(string userID, string password, string database, string character = null)
        {
            return new HxDbOci(userID, password, database, character);
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="userID">DB 사용자ID</param>
        /// <param name="password">DB 패스워드</param>
        /// <param name="database">Database Host/Name</param>
        /// <param name="character">DB 문자셋</param>
        public HxDbOci(string userID, string password, string database, string character = null)
            : base(HxDbProviderType.OCI, userID, password, database, character)
        {
            this.InitVarTypes();
        }



        public static HxDbOci Create(string userID, string password, string database, HxDbOptionRec option)
        {
            return new HxDbOci(userID, password, database, option);
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="userID">DB 사용자ID</param>
        /// <param name="password">DB 패스워드</param>
        /// <param name="database">Database Host/Name</param>
        /// <param name="option">DB 접속 옵션</param>
        public HxDbOci(string userID, string password, string database, HxDbOptionRec option)
            : base(HxDbProviderType.OCI, userID, password, database, option)
        {
            this.InitVarTypes();
        }

        public HxDbOci Create(string connectionString, HxDbOptionRec option = default(HxDbOptionRec))
        {
            return new HxDbOci(connectionString, option);
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionString">DB Connection String</param>
        /// <param name="option">DB 접속 옵션</param>
        public HxDbOci(string connectionString, HxDbOptionRec option = default(HxDbOptionRec))
            : base(HxDbProviderType.OCI, connectionString, option)
        {
            this.InitVarTypes();
        }
        #endregion

        protected override void InitVarTypes()
        {
            if (this.DbFactory == null)
            {
                this.DbFactory = new TDbFacoty();
                if (this.DbFactory != null)
                {
                    this.Conn = (TDbConnection)this.DbFactory.CreateConnection();
                    this.ConnStrBuilder = DbFactory.CreateConnectionStringBuilder();
                    this.Command = (TDbCommand)this.DbFactory.CreateCommand();
                    //this.Factory = new OracleClientFactory();
                    //this.ConnStrBuilder = new OracleConnectionStringBuilder();
                    //this.Conn = new OracleConnection();
                    //this.Trans = this.Conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    //this.Command = new OracleCommand();
                    //this.Command.Connection = this.Conn;
                    //Oracle.ManagedDataAccess.Client.OracleGlobalization g = new OracleGlobalization();
                }
            }
        }

        #region Abstract Override Methods
        /// <summary>
        /// Database 설정 관련 Option 설정
        /// </summary>
        /// <param name="option">Options</param>
        public override void SetOptions(HxDbOptionRec option)
        {
            if (this.Open())
            {
                //OracleGlobalization ClientGlob;
                //this.Connection.GetSessionInfo(ClientGlob);
                //OracleClientFactory fac = new OracleClientFactory();

                string SQL = string.Empty;
                Dictionary<string, object> param = new Dictionary<string, object>();
                try
                {
                    if (!option.DateFormat.IsNullOrWhiteSpaceEx())
                    {
                        SQL = "ALTER SESSION SET NLS_LANGUAGE = :paramDateLang";
                        param.Add("paramDateLang", option.DateLanguage);
                        this.Query(SQL, param);
                    }
                    if (!option.DateFormat.IsNullOrWhiteSpaceEx())
                    {
                        SQL = "ALTER SESSION SET NLS_DATE_FORMAT = :paramDateFormat";
                        param.Add("paramDateFormat", option.DateFormat);
                        this.Query(SQL, param);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw ex;
                }
                
            }

        }
        #endregion

        #region Virtual Overrid Methods
        protected override string NowDateTimeDbFunctionString
        {
            get { return "SYSDATE"; }
        }
        public string GetDateNow()
        {
            string Result = DateTime.Now.ToDateTimeStringEx();
            return Result;
        }

        protected override string CreateSeqTableQueryString
        {
            get
            {
                //string Result = string.Format("CREATE TABLE {0} (seq_name varchar2(30) DEFAULT '' NOT NULL, nextid int DEFAULT 1 NOT NULL, REG_DATE DATE DEFAULT SYSDATE, MOD_DATE DATE DEFAULT SYSDATE, PRIMARY KEY (seq_name))", this.SeqTableName);
                //return Result;
                StringBuilder S = new StringBuilder();
                S.AppendFormat("CREATE TABLE {0} (", this.SeqTableName);
                S.AppendFormat("    seq_name varchar2(30) DEFAULT '' NOT NULL,");
                S.AppendFormat("    nextid int DEFAULT 1 NOT NULL,");
                S.AppendFormat("    REG_DATE DATE DEFAULT {0},", this.NowDateTimeDbFunctionString);
                S.AppendFormat("    MOD_DATE DATE DEFAULT {0},", this.NowDateTimeDbFunctionString);
                S.AppendFormat("    PRIMARY KEY (seq_name)");
                S.AppendFormat(")");
                return S.ToString();
            }
        }

        public override int NextID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.UseOracleSequence)
        {
            int Result = -1;
            switch (mode)
            {
                case HxDbSeqModeType.UseOracleSequence:
                case HxDbSeqModeType.Auto:
#pragma warning disable IDE0017 // 명명 스타일
                    TDbCommand cmd = new TDbCommand();
#pragma warning restore IDE0017 // 명명 스타일
                    cmd.Connection = this.Conn;
                    cmd.CommandType = CommandType.Text;
                    string queryString;
                    try
                    {
                        if (!sequencesName.StartsWith("SEQ_", StringComparison.OrdinalIgnoreCase))
                        {
                            sequencesName = string.Format("SEQ_{0}", sequencesName);
                        }
                        if (sequencesName.Length > 30)
                        {
                            sequencesName = sequencesName.Substring(0, 30);
                        }
                        queryString = "SELECT " + sequencesName + ".NEXTVAL FROM DUAL";
                        this.Open();

                        cmd.CommandText = queryString;
                        object val = cmd.ExecuteScalar();
                        Result = (val != null ? Convert.ToInt32(val) : -1);
                    }
                    catch (TDbException exUpdate)
                    {
                        this.DebugMessage("nextid : [" + exUpdate.Number + "]" + exUpdate.Message);
                        if (exUpdate.Number == 2289)
                        {
                            try
                            {

                                queryString = string.Format("CREATE SEQUENCE {0} INCREMENT BY 1 START WITH 1 NOCYCLE NOCACHE", sequencesName);
                                cmd.Parameters.Clear();
                                cmd.CommandText = queryString;
                                cmd.ExecuteNonQuery();
                                Result = 1;
                            }
                            catch (TDbException exCreate)
                            {
                                Result = int.MinValue;
                                throw exCreate;
                            }
                            catch (Exception exCreate)
                            {
                                Result = int.MinValue;
                                throw exCreate;
                            }
                        }
                    }
                    catch (Exception exUpdate)
                    {
                        throw exUpdate;
                    }
                    finally
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    break;
                default:
                    Result = base.NextID(sequencesName, mode);
                    break;
            }
            return Result;
        }

        public override int CurrID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto)
        {
            int Result = int.MinValue;
            switch (mode)
            {
                case HxDbSeqModeType.UseOracleSequence:
                case HxDbSeqModeType.Auto:
#pragma warning disable IDE0017 // 명명 스타일
                    TDbCommand cmd = new TDbCommand();
#pragma warning restore IDE0017 // 명명 스타일
                    cmd.Connection = this.Conn;
                    cmd.CommandType = CommandType.Text;
                    string queryString;
                    try
                    {
                        if (!sequencesName.StartsWith("SEQ_", StringComparison.OrdinalIgnoreCase))
                        {
                            sequencesName = string.Format("SEQ_{0}", sequencesName);
                        }
                        if (sequencesName.Length > 30)
                        {
                            sequencesName = sequencesName.Substring(0, 30);
                        }
                        queryString = "SELECT " + sequencesName + ".CURRVAL FROM DUAL";
                        this.Open();

                        cmd.CommandText = queryString;
                        object val = cmd.ExecuteScalar();
                        Result = (val != null ? Convert.ToInt32(val) : -1);
                    }
                    catch (TDbException exUpdate)
                    {
                        this.DebugMessage("CurrID : [" + exUpdate.Number + "]" + exUpdate.Message);
                        if (exUpdate.Number == 2289)
                        {
                            try
                            {

                                queryString = string.Format("CREATE SEQUENCE {0} INCREMENT BY 1 START WITH 1 NOCYCLE NOCACHE", sequencesName);
                                cmd.Parameters.Clear();
                                cmd.CommandText = queryString;
                                cmd.ExecuteNonQuery();
                                Result = -1;
                            }
                            catch (TDbException exCreate)
                            {
                                Result = int.MinValue;
                                throw exCreate;
                            }
                            catch (Exception exCreate)
                            {
                                Result = int.MinValue;
                                throw exCreate;
                            }
                        }
                    }
                    catch (Exception exUpdate)
                    {
                        throw exUpdate;
                    }
                    finally
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    break;
                default:
                    Result = base.CurrID(sequencesName, mode);
                    break;
            }
            return Result;
        }

        

        public override TDbParameter GetParameter(string name, object value)
        {
            TDbParameter Result;
            Result = (TDbParameter)DbFactory.CreateParameter();
            Result.ParameterName = name;
            if (value == null)
            {
                Result.Value = DBNull.Value;
            }
            else if (value.ToStringEx() == "00" || value.ToStringEx() == "000" || value.ToStringEx() == "0000")
            {
                Result.Value = value.ToStringEx();
            }
            else
            {
                if (name.StartsWith(_PREFIX_PARAM_CLOB_) && value.IsNullOrWhiteSpaceEx() != true)
                {
                    Result.OracleDbType = OracleDbType.Clob;
                    Result.Direction = ParameterDirection.Input;

                    //byte[] bytes = HxString.GetString2Bytes(value.ToStringEx(), HxEncodingType.Default);

                    Result.Value = value;
                }
                //else if (name.StartsWith(_PREFIX_PARAM_BLOB_))
                //{
                //    Result.OracleDbType = OracleDbType.Blob;
                //    Result.Direction = ParameterDirection.Input;
                //    Result.Value = (TDbOracleBlob)value;
                //}
                else
                {
                    Result.Value = value;
                }
                
            }
            return Result;
        }


        public override bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects)
        {
            bool Result = false;
            try
            {
                string SQL = string.Format("SELECT OBJECT_NAME, OBJECT_TYPE, CREATED, LAST_DDL_TIME, TIMESTAMP, STATUS, TEMPORARY, GENERATED FROM {0} WHERE 1 = 1 AND OBJECT_NAME = :paramName", "USER_OBJECTS");
                switch (objectType)
                {
                    case HxDbObjectType.SelectOnlyObjects:
                        SQL = string.Format("{0} AND OBJECT_TYPE IN ('TABLE', 'VIEW', 'SYNONYM')", SQL);
                        break;
                    case HxDbObjectType.Table:
                        SQL = string.Format("{0} AND OBJECT_TYPE = 'TABLE'", SQL);
                        break;
                    case HxDbObjectType.View:
                        SQL = string.Format("{0} AND OBJECT_TYPE = 'VIEW'", SQL);
                        break;
                    case HxDbObjectType.Synonym:
                        SQL = string.Format("{0} AND OBJECT_TYPE = 'SYNONYM'", SQL);
                        break;
                    case HxDbObjectType.Sequence:
                        SQL = string.Format("{0} AND OBJECT_TYPE = 'SEQUENCE'", SQL);
                        break;
                    default:
                        break;
                }
                Dictionary<string, object> bind = new Dictionary<string, object>() { { "paramName", name } };
                //bind.Add("paramName", name);
                this.Query(SQL, bind);
                int nRow = this.nf();
                if (nRow > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        public override DataTable UserTables()
        {
            DataTable Result = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine("SELECT");
                sb.AppendLine("  TABLE_NAME, TABLE_TYPE, TABLE_OWNER, REAL_NAME, COMMENTS, DB_LINK ");
                sb.AppendLine("FROM ");
                sb.AppendLine("  (SELECT");
                sb.AppendLine("    T.OBJECT_NAME TABLE_NAME, T.OBJECT_TYPE TABLE_TYPE, USER TABLE_OWNER, T.OBJECT_NAME REAL_NAME, C.COMMENTS, NULL DB_LINK ");
                sb.AppendLine("  FROM USER_OBJECTS T, ");
                sb.AppendLine("    USER_TAB_COMMENTS C ");
                sb.AppendLine("  WHERE T.OBJECT_TYPE IN ('TABLE', 'VIEW') ");
                sb.AppendLine("  AND T.OBJECT_NAME    = C.TABLE_NAME ");
                sb.AppendLine("  UNION ");
                sb.AppendLine("  SELECT ");
                sb.AppendLine("    T.SYNONYM_NAME TABLE_NAME, 'SYNONYM' TABLE_TYPE, T.TABLE_OWNER, T.TABLE_NAME REAL_NAME, C.COMMENTS, T.DB_LINK ");
                sb.AppendLine("  FROM USER_SYNONYMS T, ");
                sb.AppendLine("    ALL_TAB_COMMENTS C ");
                sb.AppendLine("  WHERE T.TABLE_NAME = C.TABLE_NAME ");
                sb.AppendLine("  AND T.TABLE_OWNER  = C.OWNER ");
                sb.AppendLine("  ) ");
                sb.AppendLine("ORDER BY TABLE_NAME");
                Result = this.QueryDataTable(sb.ToString());
                if (Result != null)
                    Result.TableName = "UserTables";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                sb.Clear();
            }
            return Result;
        }

        public override DataTable UserColumns()
        {
            DataTable Result = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("SELECT * ");
                sb.Append("FROM ");
                sb.Append("  (SELECT ");
                sb.Append("    COL.TABLE_NAME, COL.COLUMN_NAME, COL.DATA_TYPE, COL.DATA_LENGTH, COL.DATA_PRECISION, COL.DATA_SCALE, COL.NULLABLE, COL.COLUMN_ID ");
                sb.Append("    , T.OBJECT_TYPE TABLE_TYPE, USER TABLE_OWNER, COL.TABLE_NAME REAL_TABLE_NAME, CC.COMMENTS ");
                sb.Append("  FROM USER_TAB_COLUMNS COL , ");
                sb.Append("    USER_OBJECTS T , ");
                sb.Append("    USER_COL_COMMENTS CC ");
                sb.Append("  WHERE 1             = 1 ");
                sb.Append("  AND T.OBJECT_TYPE  IN ('TABLE', 'VIEW') ");
                sb.Append("  AND COL.TABLE_NAME  = T.OBJECT_NAME ");
                sb.Append("  AND COL.TABLE_NAME  = CC.TABLE_NAME ");
                sb.Append("  AND COL.COLUMN_NAME = CC.COLUMN_NAME ");
                sb.Append("  UNION ");
                sb.Append("  SELECT DISTINCT ");
                sb.Append("    T.SYNONYM_NAME TABLE_NAME, COL.COLUMN_NAME, COL.DATA_TYPE, COL.DATA_LENGTH, COL.DATA_PRECISION, COL.DATA_SCALE, COL.NULLABLE, COL.COLUMN_ID ");
                sb.Append("    , 'SYNONYM' TABLE_TYPE, T.TABLE_OWNER, T.TABLE_NAME REAL_TABLE_NAME, CC.COMMENTS ");
                sb.Append("  FROM ALL_TAB_COLUMNS COL , ");
                sb.Append("    USER_SYNONYMS T , ");
                sb.Append("    ALL_COL_COMMENTS CC ");
                sb.Append("  WHERE 1            = 1 ");
                sb.Append("  AND COL.OWNER      = T.TABLE_OWNER ");
                sb.Append("  AND COL.TABLE_NAME = T.TABLE_NAME ");
                sb.Append("  AND CC.OWNER       = COL.OWNER ");
                sb.Append("  AND CC.TABLE_NAME  = COL.TABLE_NAME ");
                sb.Append("  ) ");
                sb.Append("ORDER BY TABLE_NAME, ");
                sb.Append("  COLUMN_ID");
                Result = this.QueryDataTable(sb.ToString());
                if (Result != null)
                    Result.TableName = "UserColumns";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }

        public override bool TableContains(string name)
        {
            return this.Contains(name, HxDbObjectType.Table);
        }

        public override bool ViewContains(string name)
        {
            return this.Contains(name, HxDbObjectType.View);
        }

        public override bool SynonymContains(string name)
        {
            return this.Contains(name, HxDbObjectType.Synonym);
        }

        public override bool SequenceContains(string name)
        {
            return this.Contains(name, HxDbObjectType.Sequence);
        }

        public override bool ColumnContains(string tableName, string columnName)
        {
            bool Result = false;
            try
            {
                string SQL = string.Format("SELECT TNAME, CNAME, COLTYPE, WIDTH, SCALE, PRECISION, NULLS FROM {0} WHERE 1 = 1 AND TNAME = :paramTableName AND CNAME = :paramColName", "COL");
                Dictionary<string, object> bind = new Dictionary<string, object>()
                {
                    { "paramTableName", tableName }
                    ,{ "paramColName", columnName }
                };
                
                //bind.Add("paramTableName", tableName);
                //bind.Add("paramColName", columnName);
                this.Query(SQL, bind);
                if (this.nf() > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public override int GetRowCount(int parse = -1)
        {
            return this.nf();
        }

        public override string NowDateValue(string dateFormatString = null)
        {
            string Result = null;
            try
            {
                if (dateFormatString.IsNullOrWhiteSpaceEx())
                {
                    dateFormatString = "YYYY-MM-DD HH24:MI:SS";
                }
                string SQL = string.Format("SELECT TO_CHAR({0}, '{1}') AS NOW_DATE FROM DUAL", NowDateTimeDbFunctionString, dateFormatString);
                this.Query(SQL);
                if(this.nf() > 0)
                {
                    if (this.NextRecord())
                    {
                        Result = this.f(0).ToStringEx();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugMessage(ex.Message);
                throw ex;
            }
            return Result;
        }

        public override void SetParameters(TDbCommand cmd, TDbParameter[] parameters)
        {
            try
            {
                if (cmd == null)
                    cmd = (TDbCommand)DbFactory.CreateCommand();
                if (cmd != null && parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.Clear();
                    foreach (TDbParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
            }
            catch (System.Data.Common.DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        protected override DataTable QueryStoredProcedureDataTable(string queryString, TDbParameter[] parameters)
        {
            DataTable Result = null;
            try
            {
                this.Open();

                queryString = queryString.Trim().RegexReplaceEx(@"^exec ", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                queryString = queryString.RegexReplaceEx(@"(\(\s?\))$", string.Empty);

                using (TDbCommand cmd = (TDbCommand)this.DbFactory.CreateCommand())
                {
                    cmd.Connection = this.Conn;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = queryString;
                    this.SetParameters(cmd, parameters);
                    OracleParameter oraOutputCursor = new OracleParameter
                    {
                        //ParameterName = "RESULT",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(oraOutputCursor);

                    Result = new DataTable();

                    //cmd.ExecuteNonQuery();
                    //OracleDataAdapter da = new OracleDataAdapter(cmd);
                    //da.Fill(Result);
                    //return Result;


                    if (cmd.ExecuteReader() is TDbDataReader reader)
                    {
                        int index = this.QueryID + 1;
                        if (this.ReaderList == null)
                            this.ReaderList = new Dictionary<int, TDbDataReader>();
                        this.ReaderList.Add(index, reader);
                        DataTable dtLoad = new DataTable();
                        dtLoad.Load(reader);
                        this.SetColumnNameConvert(dtLoad);
                        Result = dtLoad.Copy();

                        this.QueryID = index;
                    }
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                DebugMessage(ex.Message);
                throw ex;
            }
            return Result;
        }

        #endregion



        //public override void SetConnectionString(string connString)
        //{
        //    try
        //    {
        //        if (DbFactory == null)
        //        {
        //            this.InitVarTypes();
        //        }
        //        if (DbFactory == null)
        //        {
        //            DbFactory = new TDbFacoty();
        //        }
        //        if (this.ConnStrBuilder == null)
        //        {
        //            this.ConnStrBuilder = DbFactory.CreateConnectionStringBuilder();
        //            //this.ConnStrBuilder = new DbConnectionStringBuilder();
        //        }
        //        if (this.Conn == null)
        //            this.Conn = (TDbConnection)DbFactory.CreateConnection();

        //        this.ConnStrBuilder.ConnectionString = connString;
        //        this.Conn.ConnectionString = this.ConnStrBuilder.ConnectionString;
        //        //this.Conn.ConnectionString = connString;
        //    }
        //    catch (System.Data.Common.DbException ex)
        //    {
        //        this.DebugMessage(ex.Message);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.DebugMessage(ex.Message);
        //        throw ex;
        //    }

        //}
    }
    */
}
