using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HxCore;
using System.Data.Common;
using System.Diagnostics;

namespace HxCore.Data
{
    /// <summary>
    /// Database Abstract Class
    /// </summary>
    /// <typeparam name="TDbFacoty">Provider Factory Object Type</typeparam>
    /// <typeparam name="TDbConnection">Connection Object Type</typeparam>
    /// <typeparam name="TDbTransaction">Transaction Object Type</typeparam>
    /// <typeparam name="TDbCommand">Command Object Type</typeparam>
    /// <typeparam name="TDbParameter">Parameter Object Type</typeparam>
    /// <typeparam name="TDataReader">DataReader Object Type</typeparam>
    /// <typeparam name="TDbDataAdapter">DataAdapter Object Type</typeparam>
    public abstract class HxDbA<TDbFacoty, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDbDataReader, TDbDataAdapter> : HxBase, IHxDb//, IHxDbA1<TDbFacoty, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDbDataReader>
        where TDbFacoty : System.Data.Common.DbProviderFactory
        where TDbConnection : System.Data.Common.DbConnection, IDbConnection
        where TDbTransaction : System.Data.Common.DbTransaction, IDbTransaction
        where TDbCommand : System.Data.Common.DbCommand, IDbCommand
        where TDbParameter : System.Data.Common.DbParameter, IDataParameter
        where TDbDataReader : System.Data.Common.DbDataReader, IDataReader
        where TDbDataAdapter : System.Data.Common.DbDataAdapter, IDataAdapter
    {
        //TConnectionStringBuilder where TConnectionStringBuilder : System.Data.Common.DbConnectionStringBuilder
        /// <summary>
        /// Get Class Name
        /// </summary>
        public override abstract string GetName();
        public virtual string SchemaName { get; protected set; }

        //private string paramterSeparatorChar = "?";
        //public abstract string ParamterSeparatorChar { get => paramterSeparatorChar; protected set => paramterSeparatorChar = value; }
        public abstract string ParamterSeparatorChar { get; }

        #region Private Member Fields
        private int FQueryID = -1;
        //private dnDbProviderType _DatabaseType = dnDatabaseType.None;
        //private string _ProviderName = null;
        //private bool _isConnRef = false;
        //private bool _isTrans = false;
        private bool FisDebug = false;
        //private bool _isOpen = false;
        private HxDbColumnNameCharType _columnNameType = HxDbColumnNameCharType.Lower;

        private HxDbProviderType FProviderType = HxDbProviderType.Common;

        //private TFacoty _Dbfactory = null;
        //private TConnection _Conn = null;
        //private string _ConnString;
        private TDbTransaction FTrans = null;
        private TDbCommand FCommand = null;
        private TDbDataReader FReader = null;

        private Dictionary<int, TDbDataReader> FReaderList = null;

        private DbConnectionStringBuilder FConnStrBuilder = null;
        private HxDbOptionRec FDbOption = new HxDbOptionRec();



        private string FSeqTableName = "db_sequence";

        //private string FUserWinID = null;

        //private string FUserWinPwd = null;

        #endregion

        #region Property Member Fields
        protected string ConnectionString { get; set; }
        /// <summary>
        /// Query Index ID
        /// </summary>
        public int QueryID
        {
            get { return this.FQueryID; }
            protected set { this.FQueryID = value; }
        }
        /// <summary>
        /// Database Provider Type
        /// </summary>
        public HxDbProviderType ProviderType
        {
            get { return this.FProviderType; }
            private set { this.FProviderType = value; }
        }
        /// <summary>
        /// Database Provider Name
        /// </summary>
        public string ProviderName
        {
            get { return this.GetProviderDescription(this.ProviderType); }
            //private set;
        }
        /// <summary>
        /// Debug 모드 사용 여부
        /// </summary>
        public bool IsDebug
        {
            get { return this.FisDebug; }
            set { this.SetDebugMode(value); }
        }
        /// <summary>
        /// Column Name To Lower(소문자) 설정
        /// </summary>
        public bool ColNameToLower
        {
            get { return this.GetColumnNameToLower(); }
            set { this.SetColumnNameToLower(value); }
        }

        /// <summary>
        /// Database Factory Object
        /// </summary>
        public TDbFacoty DbFactory
        {
            get;
            protected set;
        }
        /// <summary>
        /// Transaction 현재 적용(설정) 여부
        /// </summary>
        public bool IsTrans
        {
            get;
            protected set;
        }
        /// <summary>
        /// 접속 여부
        /// </summary>
        public bool IsOpen
        {
            get;
            protected set;
        }
        /// <summary>
        /// Connection Object 참조(Ref) 접속 여부
        /// </summary>
        public bool IsConnRef
        {
            get;
            protected set;
        }

        /*
        protected virtual DbProviderFactory Factory
        {
            get { return this._Factory; }
            set { this._Factory = value; }
        }*/

        public TDbConnection Conn
        {
            get;
            protected set;
        }
        public TDbTransaction Trans
        {
            get { return this.FTrans; }
            protected set { this.FTrans = value; }
        }
        public TDbCommand Command
        {
            get { return this.FCommand; }
            protected set { this.FCommand = value; }
        }
        public TDbDataReader Reader
        {
            get { return this.FReader; }
            protected set { this.FReader = value; }
        }
        public Dictionary<int, TDbDataReader> ReaderList
        {
            get { return this.FReaderList; }
            protected set { this.FReaderList = value; }
        }

        public HxDbOptionRec DbOption
        {
            get { return this.FDbOption; }
            protected set {
                this.FDbOption = value;
                //this.SetOptions(value);
            }
        }


        protected DbConnectionStringBuilder ConnStrBuilder
        {
            get
            {
                //this._ConnStrBuilder.Clear();
                /*
                if (this.Conn != null)
                    this.ConnStrBuilder.ConnectionString = this.Conn.ConnectionString;
                 * */
                return this.FConnStrBuilder;
            }
            set
            {
                this.FConnStrBuilder = value;
                //if (this.Conn != null)
                //    this.Conn.ConnectionString = this.FConnStrBuilder.ConnectionString;
            }
        }

        public string SeqTableName
        {
            get { return this.FSeqTableName; }
            protected set { this.FSeqTableName = value; }
        }

        #endregion

        #region Class 생성자
        public HxDbA(HxDbProviderType providerType)
        {
            this.SetProvider(providerType);
        }
        public HxDbA(HxDbProviderType providerType, string userID, string password, string database, string character = null, bool? pooling = null)
            : this(providerType)
        {
            this.SchemaName = userID;
            this.SetConnectionString(userID, password, database, character, pooling);
        }
        public HxDbA(HxDbProviderType providerType, string userID, string password, string database, HxDbOptionRec option)
            : this(providerType, userID, password, database)
        {
            this.DbOption = option;
        }
        public HxDbA(HxDbProviderType providerType, string connectionString, HxDbOptionRec option = default)
            : this(providerType)
        {
            
            this.ConnStrBuilder.Clear();
            this.ConnStrBuilder.ConnectionString = connectionString;
            this.ConnectionString = this.ConnStrBuilder?.ConnectionString.ToStringEx();
            //this.SetOptions(option);
            this.DbOption = option;
        }

        public HxDbA(HxDbConnectionRec connection)
            : this(connection.ProviderType, connection.User, connection.Password, connection.HostName, connection.Character, connection.Pooling)
        {
            ; ;
        }


        /*
        public dnDbAbstract(string providerName)
        {
            this.ProviderName = providerName;
            this.DebugMessage(string.Format("__Construct By {0} Class...", this.Name));
        }

        public dnDbAbstract(string providerName, string userID, string password, string database, string character = null)
            : this(providerName)
        {
            this.SetConnectionString(userID, password, database, character);
            this.DebugMessage(string.Format("__Construct(User Info) By {0} Class...", this.Name));
        }

        public dnDbAbstract(string providerName, string userID, string password, string database, dnDbOptionRec options)
            : this(providerName, userID, password, database)
        {

        }

        public dnDbAbstract(string providerName, string connString, dnDbOptionRec options = default(dnDbOptionRec))
            : this(providerName)
        {
            this.ConnStrBuilder.Clear();
            this.ConnStrBuilder.ConnectionString = connString;
        }*/

        public HxDbA(TDbConnection connectionResource)
        {
            this.IsConnRef = true;
            this.Conn = connectionResource;
            string strType = connectionResource.GetType().ToString().ToLower();
            switch (strType)
            {
                case "oracleconnection":
                    //this.ProviderType = dnDbProviderType.OCI;
                    //this.ProviderName = "Oracle.ManagedDataAccess.Client";
                    this.SetProvider(HxDbProviderType.OCI);
                    break;
                case "sqlconnection":
                    //this.ProviderType = dnDbProviderType.MsSQL;
                    //this.ProviderName = "System.Data.SqlClient";
                    this.SetProvider(HxDbProviderType.MsSQL);
                    break;
                case "postgresqlconnection":
                    this.SetProvider(HxDbProviderType.PostgreSQL);
                    break;
                default:
                    //this.ProviderType = dnDbProviderType.Common;
                    //this.ProviderName = "System.Data.Common";
                    this.SetProvider(HxDbProviderType.Common);
                    break;
            }
            this.DebugMessage(string.Format("__Construct(Ref) By {0} Class...", this.Name));
            
        }

        #endregion

        #region Abstract Method

        public abstract void SetOptions(HxDbOptionRec option);
        protected abstract void InitVarTypes();
        #endregion

        #region Utility Method
        private void SetProvider(HxDbProviderType providerType)
        {
            this.ProviderType = providerType;
            this.InitVarTypes();
            //this.ProviderName = this.GetProviderDescription(AProviderType);
        }
        private string GetProviderDescription(HxDbProviderType providerType)
        {
            HxEnumHelper<HxDbProviderType> enumHelper = new HxEnumHelper<HxDbProviderType>();
            string Result = enumHelper.GetDescription(providerType);
            return Result;
        }

        protected virtual int QueryExecuteNonQuery(string queryString, TDbParameter[] parameters = null)
        {
            int Result = int.MinValue;
            try
            {
                this.Open();
                if (this.Command == null)
                    this.Command = (TDbCommand)DbFactory.CreateCommand();
                if (this.Command.Connection == null)
                    this.Command.Connection = (TDbConnection)this.Conn;

                queryString = queryString.Trim();
                if (this.Trans != null && this.IsTrans == true && this.Command.Transaction == null)
                    this.Command.Transaction = this.Trans;

                this.Command.CommandType = CommandType.Text;
                this.Command.CommandText = queryString;
                this.Command.Parameters.Clear();
                this.SetParameters(this.Command, parameters);
                Result = this.Command.ExecuteNonQuery();
            }
            catch (DbException ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return Result;
        }
        //[Obsolete("아직 사용되지 않으며 보완 필요하여 사용을 금합니다.")]
        protected virtual int QueryStoredProcedure(string queryString, TDbParameter[] parameters)
        {
            //참고하여 향후 보완 : http://msdn.microsoft.com/en-us/library/ms971518.aspx
            int Result = int.MinValue;
            try
            {
                this.Open();
                if (this.Command == null)
                    this.Command = (TDbCommand)DbFactory.CreateCommand();
                if (this.Command.Connection == null)
                    this.Command.Connection = (TDbConnection)this.Conn;

                queryString = queryString.Trim();
                if (this.Trans != null && this.IsTrans == true && this.Command.Transaction == null)
                    this.Command.Transaction = this.Trans;

                this.Command.CommandType = CommandType.StoredProcedure;
                this.Command.CommandText = queryString;
                this.Command.Parameters.Clear();
                this.SetParameters(this.Command, parameters);
                Result = this.Command.ExecuteNonQuery();
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        #endregion

        protected virtual string CreateSeqTableQueryString
        {
            get { return string.Empty; }
        }
        protected virtual string NowDateTimeDbFunctionString
        {
            get { return string.Empty; }
        }
        //public abstract string NowDateValue();
        public virtual bool Open()
        {
            //Console.WriteLine("Ver : " + this.Conn.ServerVersion);
            bool Result = false;

            if(IsCreated)

            if (this.Conn == null)
            {
                this.Conn = (TDbConnection)DbFactory.CreateConnection();
            }

            //this.Conn.ConnectionString = this.ConnectionString;
            if (this.Conn != null)
            {
                if(this.Conn.ConnectionString.IsNullOrWhiteSpaceEx() && this.ConnectionString.IsNullOrWhiteSpaceEx() != true)
                {
                    //this.Conn?.Close();
                    this.Conn.ConnectionString = this.ConnectionString;
                    this.Conn?.Open();
                }
                Result = this.IsOpened();
                if (Result != true)
                {
                    try
                    {
                        this.Conn.Close();

                        this.Conn.Open();
                        //DbConnectionStringBuilder connStrBuilder = DbFactory.CreateConnectionStringBuilder();
                        //OracleClientFactory
                        //if(this.Debug == true)
                        //this.GetConnectionExportSchema();
                        this.SetOptions(this.DbOption);

                        Result = this.IsOpened();

                        //this.GetConnectionExportSchema();
                    }
                    catch (DbException ex)
                    {
                        Result = false;
                        this.DebugMessage(ex.Message);
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        Result = false;
                        this.DebugMessage(ex.Message);
                        throw ex;
                    }
                }
            }
            return Result;
        }
        public virtual bool IsOpened()
        {
            bool Result = false;
            switch (this.Conn.State)
            {
                case System.Data.ConnectionState.Open:
                case System.Data.ConnectionState.Connecting:
                    //this.Connect.QueryExecuteNonQuery()
                    Result = true;
                    break;
                case System.Data.ConnectionState.Executing:
                case System.Data.ConnectionState.Fetching:
                    //Result = true;
                    //break;
                case System.Data.ConnectionState.Closed:
                case System.Data.ConnectionState.Broken:
                default:
                    Result = false;
                    break;
            }
            return Result;
        }

        public virtual bool IsTransaction()
        {
            return this.IsTrans;
        }

        /// <summary>
        /// Debug 모두 사용 여부 설정
        /// </summary>
        /// <param name="bDebug">True : Debug, False : Not Debug</param>
        public void SetDebugMode(bool bDebug)
        {
            this.FisDebug = bDebug;
        }

        public virtual string GetConnectionString(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            return HxDbUtils.ConnectionString(this.ProviderType, userID, password, database, character, pooling);
            //DbConnectionStringBuilder thatConnStrBuilder = new DbConnectionStringBuilder();
            #region Connection String 관련 주석 처리
            /*
            DbConnectionStringBuilder thatConnStrBuilder = DbFactory.CreateConnectionStringBuilder();
            if (!userID.IsNullOrWhiteSpaceEx() && (userID.Trim() == "/" || userID.Trim().ToLower() == "sspi" || userID.Trim().ToLower() == "true"))
            {
                string FUserWinID = null;
                switch (this.ProviderType)
                {
                    case HxDbProviderType.OCI:
                        FUserWinID = "/";
                        thatConnStrBuilder.Add("User Id", "/");
                        break;
                    case HxDbProviderType.MsSQL:
                        FUserWinID = "true";
                        thatConnStrBuilder.Add("Trusted_Connection", "True");
                        break;
                    default:
                        FUserWinID = userID;
                        thatConnStrBuilder.Add("User Id", userID);
                        break;
                }
            }
            else if (!userID.IsNullOrWhiteSpaceEx())
            {
                thatConnStrBuilder.Add("User Id", userID);
            }

            if (!password.IsNullOrWhiteSpaceEx())
                thatConnStrBuilder.Add("Password", password);

            if (!database.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    string strPattern = @"^([0-9a-zA-Z\.\-_]{1,})+(([:,]{1,1})([0-9]{1,5}))?([\/]{1,1}([0-9a-zA-Z\.\-_]{1,}))$";
                    if (this.ProviderType == HxDbProviderType.MsSQL)
                    {
                        strPattern = @"^([\w\.\-_]{1,})+(([:,]{1,1})([0-9]{1,5}))?([\/]{1,1}([\w\.\-_\s\(\)]{1,}))$";
                    }
                    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(database, strPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        string strDbHost = match.Groups[1].Value;
                        string strDbPortDelimiter = match.Groups[3].Value;
                        string strDbPort = match.Groups[4].Value;
                        string strDbName = match.Groups[6].Value;

                        if (!strDbHost.IsNullOrWhiteSpaceEx() && this.ProviderType == HxDbProviderType.OCI || this.ProviderType == HxDbProviderType.Oracle)
                        {
                            if (strDbPort.IsNullOrWhiteSpaceEx())
                                strDbPort = "1521";
                            string strTns = string.Format("(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})))", strDbHost, strDbPort, strDbName);
                            //string strTns = string.Format("{0}:{1}/{2}", strDbHost, strDbPort, strDbName);
                            thatConnStrBuilder.Add("DATA SOURCE", strTns);
                        }
                        else if (!strDbHost.IsNullOrWhiteSpaceEx() && (this.ProviderType == HxDbProviderType.MsSQL))
                        {
                            if (!strDbPort.IsNullOrWhiteSpaceEx())
                                strDbHost = string.Format("{0},{1}", strDbHost, strDbPort);
                            thatConnStrBuilder.Add("Server", strDbHost);
                            if (!strDbName.IsNullOrWhiteSpaceEx())
                                thatConnStrBuilder.Add("Database", strDbName);
                        }
                        else
                        {
                            thatConnStrBuilder.Add("DATA SOURCE", database);
                        }

                    }
                    else
                    {
                        if (this.ProviderType == HxDbProviderType.SQLite && database.StartsWith(@"\\"))
                            database = @"\\" + database;
                        thatConnStrBuilder.Add("DATA SOURCE", database);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            switch (this.ProviderType)
            {
                case HxDbProviderType.OCI:
                case HxDbProviderType.MsSQL:
                    thatConnStrBuilder.Add("PERSIST SECURITY INFO", "False");
                    break;
                case HxDbProviderType.SQLite:
                    thatConnStrBuilder.Add("Version", "3");
                    thatConnStrBuilder.Add("FailIfMissing", "True");
                    thatConnStrBuilder.Add("PRAGMA journal_mode", "WAL");
                    break;
                //case dnDbProviderType.Access
                default:
                    if (database.EndsWith(@".mdb"))
                    {
                        thatConnStrBuilder.Add("Provider", "Microsoft.Jet.OLEDB.4.0");
                    }
                    break;

            }
            return thatConnStrBuilder.ToString();
            */
            #endregion
        }
        public virtual void SetConnectionString(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            string connString = this.GetConnectionString(userID, password, database, character, pooling);
            if(connString != null && SchemaName.IsNullOrWhiteSpaceEx() == true)
            {
                SchemaName = userID;
            }
            this.SetConnectionString(connString);
        }

        public virtual void SetConnectionString(string connString)
        {
            try
            {
                this.InitVarTypes();
                if (this.ConnStrBuilder == null)
                {
                    this.ConnStrBuilder = (DbConnectionStringBuilder)DbFactory.CreateConnectionStringBuilder();
                    //this.ConnStrBuilder = new DbConnectionStringBuilder();
                }
                if (this.Conn == null)
                    this.Conn = (TDbConnection)DbFactory.CreateConnection();
                 
                this.ConnStrBuilder.ConnectionString = connString;
                
                //this.Conn.ConnectionString = connString;
                this.ConnectionString = this.ConnStrBuilder.ConnectionString;

                this.Conn.ConnectionString = this.ConnectionString;
            }
            catch (DbException ex)
            {
                this.DebugMessage(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                this.DebugMessage(ex.Message);
                throw ex;
            }

        }


        public void Connect(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            this.SetConnectionString(userID, password, database, character, pooling);
        }

        public void Connect(string connectionString)
        {
            this.SetConnectionString(connectionString);
        }

        public bool GetColumnNameToLower()
        {
            bool Result;
            switch (this._columnNameType)
            {
                case HxDbColumnNameCharType.Lower:
                    Result = true;
                    break;
                default:
                    Result = false;
                    break;
            }
            return Result;
        }

        public void SetColumnNameToLower(bool isLower)
        {
            switch (isLower)
            {
                case true:
                    this._columnNameType = HxDbColumnNameCharType.Lower;
                    break;
                default:
                    this._columnNameType = HxDbColumnNameCharType.Original;
                    break;
            }
        }

        public virtual TDbParameter GetParameter(string name, object value)
        {
            TDbParameter Result;
            //Type type = value.GetType();
            Result = (TDbParameter)DbFactory.CreateParameter();
            Result.ParameterName = name;
            if (value == null)
            {
                Result.Value = DBNull.Value;
            }
            else if(value.ToStringEx() == "00" || value.ToStringEx() == "000" || value.ToStringEx() == "0000")
            {
                Result.Value = value.ToStringEx();
            }
            else
            {
                Result.Value = value;
            }
            return Result;
        }

        public TDbParameter[] GetConvertParameters(Dictionary<string, object> bind)
        {
            TDbParameter[] Result = null;
            int n = (bind == null ? 0 : bind.Count);

            if (n > 0)
            {
                Result = new TDbParameter[n];
                int i = 0;
                foreach (KeyValuePair<string, object> de in bind)
                {
                    /*
                    Type type = de.Value == null ? null : de.Value.GetType();
                    if (de.Value == null)
                    {
                        Result[i] = (TParameter)DbFactory.CreateParameter();
                        Result[i].ParameterName = de.Key.ToString();
                        Result[i].Value = DBNull.Value;
                    }

                    else if (de.Value != null && de.Value != DBNull.Value && (
                            type == typeof(byte[]) && ((byte[])de.Value).Length > 4000
                        ) || (
                            type == typeof(System.IO.MemoryStream)
                        ) || (
                            type == typeof(string) && !dnString.isNullOrWhiteSpace(de.Value.ToString()) && de.Value.ToString().Length > 4000
                        )
                    )
                    {
                        Result[i] = this.SetParameter(de.Key.ToString(), de.Value);
                    }
                    else
                    {
                        Result[i] = (TParameter)DbFactory.CreateParameter();
                        Result[i].ParameterName = de.Key.ToString();
                        Result[i].Value = de.Value;
                    }
                    */
                    Type type = de.Value?.GetType();
                    if (de.Value == null)
                    {
                        Result[i] = (TDbParameter)DbFactory.CreateParameter();
                        Result[i].ParameterName = de.Key.ToString();
                        Result[i].Value = DBNull.Value;
                    }
                    else
                    {
                        if(de.Value.IsNullOrWhiteSpaceEx() != true && de.Value.ToStringEx().Length > 4000)
                        {
                            //Result[i] 
                        }
                        Result[i] = this.GetParameter(de.Key.ToString(), de.Value);
                    }
                    i++;
                }
            }
            else
            {
                Result = new TDbParameter[0];
            }
            return Result;
        }

        public virtual void SetParameters(TDbCommand cmd, TDbParameter[] parameters)
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
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SetParameters(TDbCommand cmd, Dictionary<string, object> bind = null)
        {
            TDbParameter[] dbParams = GetConvertParameters(bind);
            this.SetParameters(cmd, dbParams);
        }

        public void SetColumnNameConvert(DataTable data)
        {
            const string _OPT_COL_NAME_LOWER_ = "ColNameToLower";
            if (data != null && data.Columns.Count > 0)
            {
                if (!data.ExtendedProperties.ContainsKey(_OPT_COL_NAME_LOWER_))
                {
                    data.ExtendedProperties.Add(_OPT_COL_NAME_LOWER_, this.ColNameToLower);
                }
                foreach (DataColumn dc in data.Columns)
                {
                    dc.ExtendedProperties.Add("DbColumnName", dc.ColumnName);
                    dc.ExtendedProperties.Add("ColumnName", dc.ColumnName);
                    dc.ExtendedProperties.Add(_OPT_COL_NAME_LOWER_, this.ColNameToLower);
                    //dc.ExtendedProperties.Add("DbCaption")
                    dc.Caption = dc.ColumnName;
                    if (this.ColNameToLower)
                    {
                        dc.ColumnName = dc.ColumnName.ToLower();
                    }
                }
            }
        }

        public DataTable QueryDataTable(string queryString, IDataParameter[] parameters = null, bool bStoredProcedure = false)
        {
            try
            {
                TDbParameter[] dbParam = parameters as TDbParameter[];
                return this.QueryDataTable(queryString, dbParam, bStoredProcedure);
            }
            catch (DbException exDb)
            {
                throw exDb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return null;
        }

        private DataTable QueryDataTable(string queryString, TDbParameter[] parameters = null, bool bStoredProcedure = false)
        {
            DataTable Result = null;
            if (this.Conn != null)
            {
                try
                {
                    
                    //this.Open();
                    queryString = queryString.Trim();
                    if (bStoredProcedure == true)
                    {
                        Result = this.QueryStoredProcedureDataTable(queryString, parameters);
                    }
                    else
                    {
                        string connString = this.Conn.ConnectionString;
                        string strConnString = this.ConnectionString;
                        this.Open();
                        using (TDbCommand cmd = (TDbCommand)this.DbFactory.CreateCommand())
                        {
                            
                            cmd.Connection = this.Conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = queryString;
                            this.SetParameters(cmd, parameters);
                            
                            using (TDbDataAdapter dataAdapter = (TDbDataAdapter)DbFactory.CreateDataAdapter())
                            {
                                DataTable dt = new DataTable();
                                dataAdapter.SelectCommand = cmd;
                                dataAdapter.Fill(dt);
                                this.SetColumnNameConvert(dt);

                                cmd.Parameters.Clear();
                                if (dt != null)
                                {
                                    Result = dt.Copy();
                                }
                            }

                            if (Result == null && cmd.ExecuteReader() is TDbDataReader reader)
                            {
                                Guid dtGuid = Guid.NewGuid();
                                //Result = new DataTable(dtGuid.ToString());
                                if (reader != null && reader.FieldCount > 0)
                                {

                                    try
                                    {
                                        //TDbDataReader reader = cmd.ExecuteReader() as TDbDataReader;
                                        //Guid dtGuid = Guid.NewGuid();
                                        DataTable dtLoad = new DataTable(dtGuid.ToString());
                                        dtLoad.Load(reader);
                                        this.SetColumnNameConvert(dtLoad);
                                        Result = dtLoad.Copy();
                                    }
                                    catch (Exception exLoad)
                                    {
                                        Debug.WriteLine(exLoad);
                                    }

                                    if (reader != null && (Result == null || (Result != null && Result.Columns.Count <= 0)))
                                    {
                                        try
                                        {
                                            DataTable dtSchema = reader.GetSchemaTable();

                                            const string _SCHEMATABLE_COL_ColumnName_ = "ColumnName";
                                            const string _SCHEMATABLE_COL_ColumnOrdinal_ = "ColumnOrdinal";
                                            //const string _SCHEMATABLE_COL_ColumnSize_ = "ColumnSize";
                                            //const string _SCHEMATABLE_COL_NumbericPrecision_ = "NumbericPrecision";
                                            const string _SCHEMATABLE_COL_IsUnique_ = "IsUnique";
                                            //const string _SCHEMATABLE_COL_IsKey_ = "IsKey";
                                            //const string _SCHEMATABLE_COL_BaseColumnName_ = "BaseColumnName";
                                            //const string _SCHEMATABLE_COL_BaseSchemaName_ = "BaseSchemaName";
                                            //const string _SCHEMATABLE_COL_BaseTableName_ = "BaseTableName";
                                            const string _SCHEMATABLE_COL_DataType_ = "DataType";

                                            int nCol = -1;
                                            if (dtSchema != null && (nCol = dtSchema.Rows.Count) > 0)
                                            {
                                                DataTable dtResult = new DataTable();
                                                for (int i = 0; i < nCol; i++)
                                                {
                                                    int colNoColOrdinal = dtSchema.Rows[i][_SCHEMATABLE_COL_ColumnOrdinal_].ToIntEx();
                                                    string colStrColNameDb = dtSchema.Rows[i][_SCHEMATABLE_COL_ColumnName_].ToStringEx();
                                                    string colStrColName2 = dtSchema.Rows[i][_SCHEMATABLE_COL_ColumnName_].ToStringEx();
                                                    Type colDataType = (dtSchema.Rows[i][_SCHEMATABLE_COL_DataType_] as Type);
                                                    //string colDataTypeStr = dtSchema.Rows[i][_SCHEMATABLE_COL_DataType_].ToStringEx();
                                                    bool colIsUnique = dtSchema.Rows[i][_SCHEMATABLE_COL_IsUnique_].ToBoolEx();
                                                    //bool colIsKey = dtSchema.Rows[i][_SCHEMATABLE_COL_IsKey_].ToBoolEx();
                                                    if (colStrColName2.IsNullOrWhiteSpaceEx() != true)
                                                    {
                                                        if (ColNameToLower == true)
                                                        {
                                                            colStrColName2 = colStrColName2.ToLower();
                                                        }
                                                        if (dtResult.Columns.Contains(colStrColName2) != true)
                                                        {
                                                            //bool bTypeOnError = false;
                                                            //Type type = Type.GetType(strDataType, bTypeOnError);
                                                            DataColumn dc = new DataColumn(colStrColName2)
                                                            {
                                                                Caption = colStrColNameDb
                                                            };
                                                            if (colDataType != null)
                                                            {
                                                                dc.DataType = colDataType;
                                                            }
                                                            dc.Unique = colIsUnique;
                                                            dtResult.Columns.Add(dc);
                                                        }
                                                    }
                                                }
                                                if (dtResult != null && dtResult.Columns.Count > 0 && reader.HasRows == true)
                                                {
                                                    while (reader.Read())
                                                    {
                                                        DataRow drResult = dtResult.NewRow();
                                                        for (int iiCol = 0; iiCol < dtResult.Columns.Count; iiCol++)
                                                        {
                                                            DataColumn dc = dtResult.Columns[iiCol];
                                                            object val = reader[dc.Caption];
                                                            if (val != null)
                                                            {
                                                                drResult[dc] = val;
                                                            }
                                                        }
                                                        dtResult.Rows.Add(drResult);
                                                    }
                                                }
                                                dtResult?.AcceptChanges();
                                                Result = dtResult;
                                            }
                                        }
                                        catch (Exception exReader)
                                        {
                                            Debug.WriteLine(exReader);
                                            throw exReader;
                                        }
                                        //DataTable dtSchema = reader.GetSchemaTable();
                                        //        DataTable dtSchema = reDataTableader.GetSchemaTable();
                                        //        DataTable dtData = new ();
                                        //        List<DataColumn> listCols = new List<DataColumn>();
                                        //        if (dtSchema != null)
                                        //        {
                                        //            foreach (DataRow drow in dtSchema.Rows)
                                        //            {
                                        //                string columnName = System.Convert.ToString(drow["ColumnName"]);
                                        //                DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                                        //                //column.Unique = (bool)drow["IsUnique"];
                                        //                //column.AllowDBNull = (bool)drow["AllowDBNull"];
                                        //                //column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                                        //                listCols.Add(column);
                                        //                dtData.Columns.Add(column);
                                        //            }
                                        //            while (reader.Read())
                                        //            {
                                        //                DataRow dataRow = dtData.NewRow();
                                        //                for (int i = 0; i < listCols.Count; i++)
                                        //                {
                                        //                    dataRow[((DataColumn)listCols[i])] = reader[i];
                                        //                }
                                        //                dtData.Rows.Add(dataRow);
                                        //            }
                                        //            Result = dtData.Copy();
                                        //        }
                                    }
                                    
                                }


                            }
                            cmd.Parameters.Clear();
                            cmd.Dispose();
                        }
                    }
                }
                catch (DbException exDb)
                {
                    throw exDb;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
            return Result;
        }

        public DataTable QueryAutoBindingDataTable(string queryString, Dictionary<string, object> bind, bool bStoredProcedure = false, bool bAutoBinding = true)
        {
            try
            {
                string SQL = null;
                TDbParameter[] dbParams = null;
                if (bStoredProcedure != true && bAutoBinding == true)
                {
                    GetQueryAutoBinding(queryString, bind, out SQL, out dbParams);
                }
                if (SQL.IsNullOrWhiteSpaceEx() == true)
                {
                    SQL = queryString;
                }
                if ((dbParams == null || dbParams.Length <= 0) && bind != null && bind.Count > 0)
                {
                    dbParams = GetConvertParameters(bind);
                }

                return this.QueryDataTable(SQL, dbParams, bStoredProcedure);
            }
            catch (DbException exDb)
            {
                throw exDb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable QueryDataTable(string queryString, Dictionary<string, object> bind, bool bStoredProcedure = false)
        {
            try
            {
                return this.QueryAutoBindingDataTable(queryString, bind, bStoredProcedure, false);
            }
            catch (DbException exDb)
            {
                throw exDb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool GetQueryAutoBinding(string queryString, Dictionary<string, object> bind, out string SQL, out Dictionary<string, object> dbBind)
        {
            bool Result = false;
            SQL = null;
            dbBind = null;
            if (queryString.IsNullOrWhiteSpaceEx() != true && bind != null && bind.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                dbBind = new Dictionary<string, object>();

                var liLine = queryString.SplitToLineListEx();
                if (liLine != null && liLine.Count > 0)
                {
                    string strLineAll = null;
                    string strLineQuery = null;
                    string strLineComment = null;
                    int iCommentPos;
                    if (liLine.Count > 1)
                    {
                        Debug.WriteLine(liLine.Count);
                    }
                    for (int index = 0; index < liLine.Count; index++)
                    {
                        string strIndex = index.ToStringEx().PadLeft(3, '0');
                        strLineAll = liLine[index];


                        if (strLineAll.IsNullOrWhiteSpaceEx() == true)
                        {
                            builder.AppendLine(strLineAll);
                            continue;
                        }
                        iCommentPos = strLineAll.IndexOf("--");
                        strLineQuery = strLineAll;
                        strLineComment = null;
                        if (iCommentPos > 0)
                        {
                            strLineQuery = strLineAll.Substring(0, iCommentPos);
                            strLineComment = strLineAll.Substring(iCommentPos);
                        }

                        if (strLineQuery.IsNullOrWhiteSpaceEx() != true)
                        {
                            var matches = HxString.RegexMatches(strLineQuery, HxDefs._REGEX_DBPARAM_NAME_PATTERN, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline); //@"(:)(?!YYYY|MM|DD|HH|HH24|MI|SS|YY|MON|MONTH|RR|DY|AM|PM)([\w\$]+)"
                            if (matches != null && matches.Count > 0)
                            {
                                string paramName = null;
                                object paramValue = null;

                                for (int i = 0; i < matches.Count; i++)
                                {
                                    var match1st = matches[i];
                                    string strNum1st = i.ToStringEx().PadLeft(3,'0');
                                    if (match1st != null && match1st.Success == true && match1st.Value.IsNullOrWhiteSpaceEx() != true)
                                    {
                                        paramName = $"{match1st.Groups[2].Value}_${strIndex}_{strNum1st}";
                                        paramValue = bind.ContainsKey(match1st.Groups[2].Value) ? bind[match1st.Groups[2].Value] : null;

                                        strLineQuery = strLineQuery.Replace(match1st.Value, $":{paramName}");
                                        dbBind.Add(paramName, paramValue);
                                    }


                                    var str2ndArr = strLineQuery.SplitEx($":{paramName}");
                                    if (str2ndArr != null && str2ndArr.Length > 2)
                                    {
                                        dbBind.Remove(paramName);
                                        string strLine2nd = null;
                                        for (int j = 0; j < str2ndArr.Length - 1; j++)
                                        {
                                            string strNum2nd = j.ToStringEx().PadLeft(3, '0');
                                            string paramReName = $"{paramName}_{strNum2nd}";
                                            strLine2nd += str2ndArr[j] + $":{paramReName}";
                                            dbBind.Add(paramReName, paramValue);
                                        }
                                        strLine2nd += str2ndArr[str2ndArr.Length - 1];
                                        strLineQuery = strLine2nd;
                                        strLine2nd = null;
                                    }
                                    /*
                                    var match2nd = HxString.RegexMatches(SQL, paramName, System.Text.RegularExpressions.RegexOptions.Multiline);
                                    if(match2nd != null && match2nd.Count > 1)
                                    {
                                        for(int j = 0; j < match2nd.Count; j++)
                                        {
                                            var m = match2nd[j];

                                        }
                                    }*/

                                }

                                paramName = null;
                                paramValue = null;
                            }
                        }
                        builder.AppendLine(strLineQuery + strLineComment);
                    }
                    strLineAll = null;
                    strLineQuery = null;
                    strLineComment = null;
                }

                if(builder.Length > 0)
                {
                    Result = true;
                    SQL = builder.ToString();
                    builder.Clear();
                }
            }
            return Result;
        }
        public bool GetQueryAutoBinding(string queryString, Dictionary<string, object> bind, out string SQL, out TDbParameter[] dbParams)
        {
            bool Result = false;

            SQL = null;
            dbParams = null;
            Dictionary<string, object> paramDict;
            
            bool bAutoBinding = GetQueryAutoBinding(queryString, bind, out SQL, out paramDict);
            if (bAutoBinding == true && paramDict != null && paramDict.Count > 0)
            {
                dbParams = GetConvertParameters(paramDict);
                paramDict.Clear();
                Result = true;
            }

            return Result;
        }

        public DataTable QueryDataTable(string queryString, string subWhereString, Dictionary<string, object> bind = null, bool bStoredProcedure = false)
        {
            string SQL = HxUtils.SelectQueryString(queryString, subWhereString);
            return QueryDataTable(SQL, bind, bStoredProcedure);
        }

        public int Query(string queryString, IDataParameter[] parameters)
        {
            try
            {
                TDbParameter[] dbParam = parameters as TDbParameter[];
                return this.Query(queryString, parameters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw ex;
            }
            return int.MinValue;
            
        }
        public int Query(string queryString, TDbParameter[] bind)
        {
            int Result = int.MinValue;
            //TDataReader reader = null;
            try
            {
                this.Open();
                queryString = queryString.Trim();
                TDbDataReader reader = null;
                if (queryString.Substring(0, 7).ToLower() == "select " || queryString.Substring(0, 5).ToLower() == "with ")
                {
                    if (this.Command == null)
                        this.Command = (TDbCommand)DbFactory.CreateCommand();
                    this.Command.Connection = this.Conn;
                    this.Command.CommandType = CommandType.Text;
                    this.Command.CommandText = queryString;
                    this.SetParameters(this.Command, bind);
                    reader = this.Command.ExecuteReader() as TDbDataReader;

                    if (this.ReaderList == null)
                        this.ReaderList = new Dictionary<int, TDbDataReader>();
                    int index = this.QueryID + 1;
                    DataTable dtSchema = reader.GetSchemaTable();
                    if (dtSchema != null && dtSchema.Columns.Count > 0)
                    {
                        this.SetColumnNameConvert(dtSchema);
                    }
                    this.ReaderList.Add(index, reader);
                    // Count를 위향 반드시 Parameters 남겨 둘것 
                    //this.Command.Parameters.Clear();
                    this.QueryID = index;
                    Result = this.QueryID;
                }
                else if (queryString.Substring(0, 7).ToLower() == "insert " || queryString.Substring(0, 7).ToLower() == "update " || queryString.Substring(0, 7).ToLower() == "delete " || queryString.Substring(0, 7).ToLower() == "create " || queryString.Substring(0, 6).ToLower() == "alter " || queryString.Substring(0, 5).ToLower() == "drop " || queryString.Substring(0, 9).ToLower() == "truncate ")
                {
                    /*
                    if (this.Command == null)
                        this.Command = (TCommand)DbFactory.CreateCommand();
                    this.Command.Connection = this.Conn;
                    if (this.Trans != null && this.isTrans == true && this.Command.Transaction == null)
                        this.Command.Transaction = this.Trans;
                    this.Command.CommandType = CommandType.Text;
                    this.Command.CommandText = queryString;
                    this.Command.Parameters.Clear();
                    this.SetParameters(this.Command, bind);
                    Result = this.Command.ExecuteNonQuery();
                     * */
                    Result = this.QueryExecuteNonQuery(queryString, bind);
                }
                else if (queryString.Trim().ToLower().StartsWith("merge into "))
                {
                    Result = this.QueryExecuteNonQuery(queryString, bind);
                }
                else if(queryString.Trim().ToLower().StartsWith("execute "))
                {
                    //if(bind.Length > 0)
                    Result = this.QueryStoredProcedure(queryString, bind);
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public int Query(string queryString, Dictionary<string, object> bind = null)
        {
            TDbParameter[] dbParams = GetConvertParameters(bind);
            return this.Query(queryString, dbParams);
        }

        private object QueryOne(int parse)
        {
            object Result = null;
            if (parse > -1)
            {
                if (this.NextRecord(parse) && this.GetColumnCount(parse) > 0)
                {
                    Result = this.f(0, parse);
                }
            }
            return Result;
        }

        public object QueryOne(string queryString, Dictionary<string, object> bind = null)
        {
            int parse = this.Query(queryString, bind);
            //if(parse > -1)
            //{
            //    if (this.NextRecord(parse) && this.GetColumnCount(parse) > 0)
            //    {
            //        Result = this.f(0, parse);
            //    }
            //}
            //return Result;
            return this.QueryOne(parse);
        }

        public object QueryOne(string queryString, IDataParameter[] parameters)
        {
            int parse = this.Query(queryString, parameters);
            return this.QueryOne(parse);
        }

        public object QueryOne(string queryString, TDbParameter[] parameters)
        {
            int parse = this.Query(queryString, parameters);
            return this.QueryOne(parse);
        }

        public int GetRowCount()
        {
            int Result = -1;
            int parse = this.QueryID;
            if (parse >= 0 && ReaderList.ContainsKey(parse))
            {
                /*
                TDataReader reader = ReaderList[parse];
                if (reader != null && !reader.IsClosed && reader.HasRows == true)
                {
                    //int rowCount = reader.Cast<object>().Count();
                    
                    //using (DataTable dt = new DataTable())
                    //{
                    //    dt.Load(reader);
                    //    Result = dt.Rows.Count;
                    //}
                }
                else
                {
                    Result = 0;
                }
                //Type type = this.ReaderList[parse].GetType();
                //System.Reflection.PropertyInfo[] pInfos = type.GetProperties();
                //Result = ReaderList[parse].Cast<object>().Count();
                 * */
                //TDbDataReader reader = ReaderList[parse] as TDbDataReader;
                if (ReaderList[parse] is TDbDataReader reader)
                {
                    Result = 0;
                    if (reader.HasRows == true)
                    {
                        using (DataTable dt = new DataTable())
                        {
                            dt.Load(reader);
                            Result = dt.Rows.Count;
                        }
                    }
                }
            }
            return Result;
        }

        public int GetColumnCount(int parse = -1)
        {
            int Result = -1;
            if (parse < 0 && this.QueryID >= 0)
                parse = this.QueryID;
            if (ReaderList.ContainsKey(parse))
            {
                TDbDataReader reader = ReaderList[parse];
                if (reader != null && !reader.IsClosed)
                {
                    Result = this.ReaderList[parse].FieldCount;
                }
            }
            return Result;
        }

        public int nf()
        {
            return this.GetRowCount();
        }

        public string np()
        {
            int n = this.GetRowCount();
            return (n > -1 ? n.ToString() : null);
        }

        public bool NextRecord(int parse = -1)
        {
            bool Result = false;
            if (parse < 0 && this.QueryID >= 0)
                parse = this.QueryID;
            if (parse > -1)
            {
                if (ReaderList.ContainsKey(parse))
                {
                    TDbDataReader reader = this.ReaderList[parse];
                    if (reader != null && reader.HasRows)
                    {
                        if (reader.IsClosed)
                        {
                            Result = false;
                        }
                        else if (reader.HasRows && reader.Read())
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                        }
                    }
                    else
                    {
                        Result = false;
                    }
                }
            }
            return Result;
        }

        public object f(string columnName, int parse = -1)
        {
            return this.GetValue(columnName, parse);
        }

        public object f(int columnIndex, int parse = -1)
        {
            return this.GetValue(columnIndex, parse);
        }

        public string p(string columnName, int parse = -1)
        {
            string Result = null;
            object value = this.f(columnName, parse);
            Result = value?.ToString();//  (value != null ? value.ToString() : null);
            return Result;
        }

        public string p(int columnIndex, int parse = -1)
        {
            string Result = null;
            object value = this.f(columnIndex, parse);
            Result = value?.ToString(); //(value != null ? value.ToString() : null);
            return Result;
        }

        public object GetValue(string columnName, int parse = -1)
        {
            object Result = null;
            if (parse < 0 && this.QueryID >= 0)
                parse = this.QueryID;
            if (parse > -1)
            {
                try
                {
                    if (ReaderList.ContainsKey(parse))
                    {
                        TDbDataReader reader = this.ReaderList[parse];
                        if (reader != null && !reader.IsClosed)
                            Result = reader[columnName];
                        else
                            Result = null;
                    }
                }
                catch (Exception)
                {
                    Result = null;
                }
            }
            return Result;
        }

        public object GetValue(int columnIndex, int parse = -1)
        {
            object Result = null;
            if (parse < 0 && this.QueryID >= 0)
                parse = this.QueryID;
            if (parse > -1)
            {
                try
                {
                    if (ReaderList.ContainsKey(parse))
                    {
                        TDbDataReader reader = this.ReaderList[parse];
                        if (reader != null && !reader.IsClosed)
                            Result = reader[columnIndex];
                        else
                            Result = null;
                    }
                }
                catch (Exception)
                {
                    Result = null;
                }

            }
            return Result;
        }

        public Type GetColumnType(string columnName, int parse = -1)
        {
            Type Result = null;
            if (parse < 0 && this.QueryID >= 0)
                parse = this.QueryID;
            if (parse > -1)
            {
                if (ReaderList.ContainsKey(parse))
                {
                    TDbDataReader reader = this.ReaderList[parse];
                    Result = reader[columnName].GetType();
                }
            }
            return Result;
        }

        public Type GetColumnType(int columnIndex, int parse = -1)
        {
            Type Result = null;
            if (parse < 0 && this.QueryID >= 0)
                parse = this.QueryID;
            if (parse > -1)
            {
                if (ReaderList.ContainsKey(parse))
                {
                    TDbDataReader reader = this.ReaderList[parse];
                    Result = reader[columnIndex].GetType();
                }
            }
            return Result;
        }

        public virtual int NextID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto)
        {
            int Result = int.MinValue;
            bool bCurrBeginTransaction = false;

            object val = null;
            TDbCommand cmd = (TDbCommand)this.DbFactory.CreateCommand();
            string queryString = string.Empty;
            try
            {
                if (sequencesName.Length > 30)
                {
                    sequencesName = sequencesName.Substring(0, 30);
                }
                this.Open();
                
                if(this.IsTrans != true)
                {
                    this.BeginTransaction();
                    bCurrBeginTransaction = true;
                }
                
                cmd.Connection = this.Conn;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Clear();
                if (!HxString.IsNullOrWhiteSpace(this.NowDateTimeDbFunctionString))
                    queryString = string.Format("UPDATE {0} SET nextid = nextid + 1 WHERE seq_name = '{1}'", this.SeqTableName, sequencesName);
                else
                    queryString = string.Format("UPDATE {0} SET nextid = nextid + 1, mod_date = {2} WHERE seq_name = '{1}'", this.SeqTableName, sequencesName, this.NowDateTimeDbFunctionString);
                cmd.CommandText = queryString;
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                queryString = string.Format("SELECT nextid FROM {0} WHERE seq_name = '{1}'", this.SeqTableName, sequencesName);
                cmd.CommandText = queryString;
                val = cmd.ExecuteScalar();
                Result = (val != null ? Convert.ToInt32(val) : -1);
                //this.DebugMessage(val.GetType().ToString());
                if (bCurrBeginTransaction == true)
                {
                    this.Commit();
                }

            }
            catch (DbException exUpdate)
            {
                this.DebugMessage(exUpdate.Message);
                try
                {
                    this.Open();
                    if (this.IsTrans != true)
                    {
                        this.BeginTransaction();
                        bCurrBeginTransaction = true;
                    }
                    if (!HxString.IsNullOrWhiteSpace(this.CreateSeqTableQueryString))
                    {
                        try
                        {

                            queryString = this.CreateSeqTableQueryString;
                            cmd.Connection = this.Conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = queryString;
                            cmd.ExecuteNonQuery();

                        }
                        catch (DbException exCreate)
                        {
                            this.DebugMessage(exCreate.Message);
                        }
                    }
                    cmd.Parameters.Clear();
                    queryString = string.Format("INSERT INTO {0} (seq_name, nextid) VALUES ('{1}', 1)", this.SeqTableName, sequencesName);
                    cmd.CommandText = queryString;
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    queryString = string.Format("SELECT nextid FROM {0} WHERE seq_name = '{1}'", this.SeqTableName, sequencesName);
                    cmd.CommandText = queryString;
                    val = cmd.ExecuteScalar();
                    Result = (val != null ? Convert.ToInt32(val) : -1);
                    if (bCurrBeginTransaction)
                    {
                        this.Commit();
                    }
                }
                catch (DbException exInsert)
                {
                    Result = int.MinValue;
                    //this.DebugMessage(exInsert.Message);
                    throw exInsert;
                }
                catch (Exception exInsert)
                {
                    Result = int.MinValue;
                    //this.DebugMessage(exInsert.Message);
                    throw exInsert;
                }
                finally
                {
                    if (bCurrBeginTransaction == true)
                    {
                        this.EndTransaction();
                    }
                }

            }
            catch (Exception exUpdate)
            {
                Result = int.MinValue;
                //this.DebugMessage(exUpdate.Message);
                throw exUpdate;
            }
            finally
            {
                if (bCurrBeginTransaction == true)
                {
                    this.EndTransaction();
                }
                cmd?.Dispose();
                //cmd = null;
            }


            return Result;
        }

        public virtual int CurrID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto)
        {
            int Result = int.MinValue;
            object val = null;
            TDbCommand cmd = (TDbCommand)this.DbFactory.CreateCommand();
            string queryString = string.Empty;
            try
            {
                if (sequencesName.Length > 30)
                {
                    sequencesName = sequencesName.Substring(0, 30);
                }
                this.Open();

                cmd.Parameters.Clear();
                queryString = string.Format("SELECT nextid FROM {0} WHERE seq_name = '{1}'", this.SeqTableName, sequencesName);
                cmd.CommandText = queryString;
                val = cmd.ExecuteScalar();
                Result = (val != null ? Convert.ToInt32(val) : -1);
                //this.DebugMessage(val.GetType().ToString());

            }
            catch (DbException exUpdate)
            {
                this.DebugMessage(exUpdate.Message);
                try
                {
                    this.Open();
                    if (!HxString.IsNullOrWhiteSpace(this.CreateSeqTableQueryString))
                    {
                        try
                        {
                            queryString = this.CreateSeqTableQueryString;
                            cmd.Connection = this.Conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = queryString;
                            cmd.ExecuteNonQuery();
                        }
                        catch (DbException exCreate)
                        {
                            this.DebugMessage(exCreate.Message);
                        }
                    }
                    cmd.Parameters.Clear();
                    queryString = string.Format("INSERT INTO {0} (seq_name, nextid) VALUES ('{1}', 0)", this.SeqTableName, sequencesName);
                    cmd.CommandText = queryString;
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    queryString = string.Format("SELECT nextid FROM {0} WHERE seq_name = '{1}'", this.SeqTableName, sequencesName);
                    cmd.CommandText = queryString;
                    val = cmd.ExecuteScalar();
                    Result = (val != null ? Convert.ToInt32(val) : -1);
                }
                catch (DbException exInsert)
                {
                    Result = int.MinValue;
                    //this.DebugMessage(exInsert.Message);
                    throw exInsert;
                }
                catch (Exception exInsert)
                {
                    Result = int.MinValue;
                    //this.DebugMessage(exInsert.Message);
                    throw exInsert;
                }

            }
            catch (Exception exUpdate)
            {
                Result = int.MinValue;
                //this.DebugMessage(exUpdate.Message);
                throw exUpdate;
            }
            finally
            {
                cmd?.Dispose();
                //cmd = null;
            }


            return Result;
        }

        public List<HxDbColumnRec> MetaData(string tableName)
        {
            throw new NotImplementedException();
        }

        public List<HxDbTableRec> TableNames()
        {
            throw new NotImplementedException();
        }

        public List<HxDbTableRec> ViewNames()
        {
            throw new NotImplementedException();
        }

        public bool Lock(string tableName, HxModeType mode = HxModeType.All)
        {
            throw new NotImplementedException();
        }

        public bool UnLock()
        {
            throw new NotImplementedException();
        }

        public bool BeginTransaction()
        {
            try
            {
                this.Open();
                if (this.Trans == null || this.IsTrans != true)
                {
                    this.Trans = (TDbTransaction)this.Conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    this.Command.Transaction = this.Trans;
                }
                this.IsTrans = true;

            }
            catch (DbException ex)
            {
                this.IsTrans = false;
                throw ex;
            }
            catch (Exception ex)
            {
                this.IsTrans = false;
                throw ex;
            }
            return this.IsTrans;
        }

        public bool EndTransaction()
        {
            if (this.IsTrans == true)
            {
                this.Rollback();
            }
            return this.IsTrans;
        }

        public bool Commit()
        {
            bool Result = false;
            try
            {
                this.Trans.Commit();
                Result = true;
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.IsTrans = false;
            return Result;
        }

        public bool Rollback()
        {
            bool Result = false;
            try
            {
                this.Trans.Rollback();
                Result = true;
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.IsTrans = false;
            return Result;
        }

        public bool Close(int parse = -1)
        {
            bool Result = false;
            if (parse < 0 && this.QueryID >= 0)
                parse = this.QueryID;
            if (ReaderList.ContainsKey(parse))
            {
                TDbDataReader reader = this.ReaderList[parse];
                if (reader != null && reader.IsClosed != true)
                    reader.Close();
                reader.Dispose();
                this.ReaderList.Remove(parse);
                Result = true;
            }
            return Result;
        }



        public void Halt(string message)
        {
            throw new NotImplementedException();
        }

        protected void GetConnectionExportSchema()
        {
            //string ProviderName = this.Connection.ModuleName.ToString();
            DataTable dtMetadata =
          this.Conn.GetSchema(DbMetaDataCollectionNames.MetaDataCollections);
            dtMetadata.WriteXml(ProviderName + "_MetaDataCollections.xml");

            //Get Restrictions and write to an XML file.
            DataTable dtRestrictions =
              this.Conn.GetSchema(DbMetaDataCollectionNames.Restrictions);
            dtRestrictions.WriteXml(ProviderName + "_Restrictions.xml");

            //Get DataSourceInformation and write to an XML file.
            DataTable dtDataSrcInfo =
              this.Conn.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
            dtDataSrcInfo.WriteXml(ProviderName + "_DataSourceInformation.xml");

            //DataTypes and write to an XML file.
            DataTable dtDataTypes =
              this.Conn.GetSchema(DbMetaDataCollectionNames.DataTypes);
            dtDataTypes.WriteXml(ProviderName + "_DataTypes.xml");

            //Get ReservedWords and write to an XML file.
            DataTable dtReservedWords =
              this.Conn.GetSchema(DbMetaDataCollectionNames.ReservedWords);
            dtReservedWords.WriteXml(ProviderName + "_ReservedWords.xml");

            //Get all the tables and write to an XML file.
            DataTable dtTables = this.Conn.GetSchema("Tables");
            dtTables.WriteXml(ProviderName + "_Tables.xml");

            //Get all the views and write to an XML file.
            DataTable dtViews = this.Conn.GetSchema("Views");
            dtViews.WriteXml(ProviderName + "_Views.xml");

            //Get all the columns and write to an XML file.
            DataTable dtColumns = this.Conn.GetSchema("Columns");
            dtColumns.WriteXml(ProviderName + "_Columns.xml");
        }

        public void Free()
        {
            this.FreeAndNull();
        }

        protected override void FreeAndNull()
        {
            //this.Dispose();
            try
            {
                if (this.ReaderList != null && this.ReaderList.Count > 0)
                {
                    foreach (KeyValuePair<int, TDbDataReader> de in this.ReaderList)
                    {
                        TDbDataReader reader = de.Value;
                        if (reader != null && !reader.IsClosed)
                            reader.Close();
                        reader.Dispose();
                    }
                    this.ReaderList.Clear();
                }
                if (this.Reader != null)
                {
                    this.Reader.Close();
                    this.Reader.Dispose();
                    this.Reader = null;
                }
                if (this.Trans != null)
                {
                    //this.Trans.Rollback();
                    this.Trans.Dispose();
                }
                if (this.Command != null)
                {
                    this.Command.Parameters.Clear();
                    this.Command.Dispose();
                    this.Command = null;
                }
                if (this.ConnStrBuilder != null)
                {
                    this.ConnStrBuilder.Clear();
                    this.FConnStrBuilder = null;
                }
                if (this.Conn != null)
                {
                    this.Conn.Close();
                    this.Conn.Dispose();
                    this.Conn = null;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 디버깅 메세지 출력
        /// </summary>
        /// <param name="message">Text Message</param>
        //[System.Diagnostics.Conditional("DEBUG")]
        protected override void DebugMessage(string message)
        {
            string str = null;
            if (this.IsDebug == true)
            {
                str += "\n**************************************************************************************************\n";
                str += "Debug : " + message;
                str += "\n**************************************************************************************************\n";
                Console.WriteLine(str);
            }
        }

        public abstract int GetRowCount(int parse = -1);
        public abstract DataTable UserTables();
        public abstract DataTable UserColumns();
        public abstract bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects);
        public abstract bool TableContains(string name);
        public abstract bool ViewContains(string name);
        public abstract bool SynonymContains(string name);
        public abstract bool SequenceContains(string name);
        public abstract bool ColumnContains(string tableName, string columnName);
        public abstract string NowDateValue(string dateFormatString = null);

        protected abstract DataTable QueryStoredProcedureDataTable(string queryString, TDbParameter[] parameters);

        public HxDbProviderType GetProviderType()
        {
            return this.ProviderType;
        }

        public string GetParamterSeparatorChar()
        {
            string Result = ParamterSeparatorChar;
            if (Result.IsNullOrWhiteSpaceEx())
            {
                switch (ProviderType)
                {
                    case HxDbProviderType.OCI:
                    case HxDbProviderType.PostgreSQL:
                        Result = ":";
                        break;
                    case HxDbProviderType.SQLite:
                    case HxDbProviderType.MsSQL:
                        Result = "@";
                        break;
                    default:
                        Result = "?";
                        break;
                }
            }
            return Result;
            
        }

        public string GetSchemaName()
        {
            return SchemaName;
        }
    }
}
