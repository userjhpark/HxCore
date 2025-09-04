using System;
//using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TDbFacoty = Npgsql.NpgsqlFactory;
using TDbConnection = Npgsql.NpgsqlConnection;
using TDbTransaction = Npgsql.NpgsqlTransaction;
using TDbCommand = Npgsql.NpgsqlCommand;
using TDbParameter = Npgsql.NpgsqlParameter;
using TDbDataReader = Npgsql.NpgsqlDataReader;
using TDbDataAdapter = Npgsql.NpgsqlDataAdapter;
using TDbException = Npgsql.NpgsqlException;
using Npgsql;
//using TDbConnection = Oracle.ManagedDataAccess.Client.OracleConnection;
//using TDbTransaction = Oracle.ManagedDataAccess.Client.OracleTransaction;
//using TDbCommand = Oracle.ManagedDataAccess.Client.OracleCommand;
//using TDbParameter = Oracle.ManagedDataAccess.Client.OracleParameter;
//using TDbDataReader = Oracle.ManagedDataAccess.Client.OracleDataReader;
//using TDbDataAdapter = Oracle.ManagedDataAccess.Client.OracleDataAdapter;
//using TDbException = Oracle.ManagedDataAccess.Client.OracleException;


namespace HxCore.Data
{
    //HxDbProviderType HxProviderType = HxDbProviderType.PostgreSQL;
    public class HxDbNpgsql : HxDbA<TDbFacoty, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDbDataReader, TDbDataAdapter>
    {
        public override string GetName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            //throw new NotImplementedException();
        }
        public override string ParamterSeparatorChar => ":";

        #region Static Intance
        //private static HxDbPgsql _instance = null;
        //static HxDbPgsql()
        //{
        //    _instance = new HxDbPgsql();
        //}
        ///// <summary>
        ///// [Static]Instance Object
        ///// </summary>
        //public static HxDbPgsql Instance
        //{
        //    get { return _instance ?? (_instance = new HxDbPgsql()); }
        //    private set { _instance = value; }
        //}
        #endregion
        #region 생성자
        public static HxDbNpgsql Create()
        {
            return new HxDbNpgsql();
        }
        /// <summary>
        /// 생성자
        /// </summary>
        public HxDbNpgsql()
            : base(HxDbProviderType.PostgreSQL)
        {
            this.InitVarTypes();
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionResource">Connection Resource</param>
        public HxDbNpgsql(TDbConnection connectionResource)
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

        public static HxDbNpgsql Create(string userID, string password, string database, string character = null)
        {
            return new HxDbNpgsql(userID, password, database, character);
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="userID">DB 사용자ID</param>
        /// <param name="password">DB 패스워드</param>
        /// <param name="database">Database Host/Name</param>
        /// <param name="character">DB 문자셋</param>
        public HxDbNpgsql(string userID, string password, string database, string character = null)
            : base(HxDbProviderType.PostgreSQL, userID, password, database, character)
        {
            this.InitVarTypes();
        }

        public HxDbNpgsql(HxDbConnectionRec connection)
            : base(connection.ProviderType, connection.User, connection.Password, connection.HostName, connection.Character)
        {
            ; ;
        }

        public static HxDbNpgsql Create(string userID, string password, string database, HxDbOptionRec option)
        {
            return new HxDbNpgsql(userID, password, database, option);
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="userID">DB 사용자ID</param>
        /// <param name="password">DB 패스워드</param>
        /// <param name="database">Database Host/Name</param>
        /// <param name="option">DB 접속 옵션</param>
        public HxDbNpgsql(string userID, string password, string database, HxDbOptionRec option)
            : base(HxDbProviderType.PostgreSQL, userID, password, database, option)
        {
            this.InitVarTypes();
        }

        public HxDbNpgsql Create(string connectionString, HxDbOptionRec option = default(HxDbOptionRec))
        {
            return new HxDbNpgsql(connectionString, option);
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionString">DB Connection String</param>
        /// <param name="option">DB 접속 옵션</param>
        public HxDbNpgsql(string connectionString, HxDbOptionRec option = default(HxDbOptionRec))
            : base(HxDbProviderType.PostgreSQL, connectionString, option)
        {
            this.InitVarTypes();
        }
        #endregion

        protected override void InitVarTypes()
        {
            if (this.DbFactory == null)
            {
                this.DbFactory = TDbFacoty.Instance;
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

                //string SQL = string.Empty;
                //Dictionary<string, object> param = new Dictionary<string, object>();
                //if (!option.DateFormat.IsNullOrWhiteSpaceEx())
                //{
                //    SQL = "ALTER SESSION SET NLS_LANGUAGE = :paramDateLang";
                //    param.Add("paramDateLang", option.DateLanguage);
                //    this.Query(SQL, param);
                //}
                //if (!option.DateFormat.IsNullOrWhiteSpaceEx())
                //{
                //    SQL = "ALTER SESSION SET NLS_DATE_FORMAT = :paramDateFormat";
                //    param.Add("paramDateFormat", option.DateFormat);
                //    this.Query(SQL, param);
                //}
            }

        }
        #endregion

        #region Virtual Overrid Methods
        protected override string NowDateTimeDbFunctionString
        {
            get { return "now()"; } //current_date
        }

        protected override string CreateSeqTableQueryString
        {
            get
            {
                //string Result = string.Format("CREATE TABLE {0} (seq_name varchar2(30) DEFAULT '' NOT NULL, nextid int DEFAULT 1 NOT NULL, REG_DATE DATE DEFAULT SYSDATE, MOD_DATE DATE DEFAULT SYSDATE, PRIMARY KEY (seq_name))", this.SeqTableName);
                //return Result;
                StringBuilder S = new StringBuilder();
                S.AppendFormat("CREATE TABLE {0} (", this.SeqTableName);
                S.AppendFormat("    seq_name CHARACTER VARYING (30) DEFAULT '' NOT NULL,");
                S.AppendFormat("    nextid integer DEFAULT 1 NOT NULL,");
                S.AppendFormat("    REG_DATE TIMESTAMP DEFAULT {0},", this.NowDateTimeDbFunctionString);
                S.AppendFormat("    MOD_DATE TIMESTAMP DEFAULT {0},", this.NowDateTimeDbFunctionString);
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
                        if (!sequencesName.StartsWith("seq_", StringComparison.OrdinalIgnoreCase))
                        {
                            sequencesName = string.Format("seq_{0}", sequencesName);
                        }
                        if (sequencesName.Length > 30)
                        {
                            sequencesName = sequencesName.Substring(0, 30);
                        }
                        queryString = "SELECT nextval('" + sequencesName + "')";
                        this.Open();

                        cmd.CommandText = queryString;
                        object val = cmd.ExecuteScalar();
                        Result = (val != null ? Convert.ToInt32(val) : -1);
                    }
                    catch (TDbException exUpdate)
                    {
                        this.DebugMessage("nextid : [" + exUpdate.ErrorCode + "]" + exUpdate.Message);
                        if (exUpdate.ErrorCode == 2289)
                        {
                            try
                            {

                                queryString = string.Format("CREATE SEQUENCE {0} INCREMENT BY 1 START WITH 1", sequencesName);
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
                        if (!sequencesName.StartsWith("seq_", StringComparison.OrdinalIgnoreCase))
                        {
                            sequencesName = string.Format("seq_{0}", sequencesName);
                        }
                        if (sequencesName.Length > 30)
                        {
                            sequencesName = sequencesName.Substring(0, 30);
                        }
                        queryString = "SELECT currval('" + sequencesName + "')";
                        this.Open();

                        cmd.CommandText = queryString;
                        object val = cmd.ExecuteScalar();
                        Result = (val != null ? Convert.ToInt32(val) : -1);
                    }
                    catch (TDbException exUpdate)
                    {
                        this.DebugMessage("CurrID : [" + exUpdate.ErrorCode + "]" + exUpdate.Message);
                        if (exUpdate.ErrorCode == 2289)
                        {
                            try
                            {

                                queryString = string.Format("CREATE SEQUENCE {0} INCREMENT BY 1 START WITH 1", sequencesName);
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

        public string GetDateNow()
        {
            string Result = null;

            return Result;
        }

        public override bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects)
        {
            string strDbaObjects = @"select 
    nsp.nspname as SCHEMA_NAME
    ,cls.relname as OBJECT_NAME 
    ,rol.rolname as OBJECT_OWNER
    ,case cls.relkind
        when 'r' then 'TABLE'
        when 'm' then 'MATERIALIZED_VIEW'
        when 'i' then 'INDEX'
        when 'S' then 'SEQUENCE'
        when 'v' then 'VIEW'
        when 'c' then 'TYPE'
		when 'f' then 'FOREIGN_TABLE'
		when 't' then 'TOAST_TABLE'
        else cls.relkind::text
    end as OBJECT_TYPE
from pg_class cls
join pg_roles rol 
	on rol.oid = cls.relowner
join pg_namespace nsp 
	on nsp.oid = cls.relnamespace
where nsp.nspname not in ('information_schema', 'pg_catalog')
    and nsp.nspname not like 'pg_toast%'
    --and rol.rolname = current_user  
order by nsp.nspname, cls.relname";
            bool Result = false;
            try
            {
                string SQL = string.Format("SELECT * FROM ( {0} ) DBA_OBJECTS WHERE 1 = 1 AND OBJECT_NAME = :paramName", strDbaObjects);
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
            //**SELECT * FROM information_schema.tables WHERE 1 = 1 AND table_schema <> 'information_schema' AND table_schema not like 'pg_%' AND table_catalog = current_user;
            DataTable Result = null;
            try
            {
                string strDbaObjects = @"select 
    nsp.nspname as SCHEMA_NAME
    ,cls.relname as OBJECT_NAME 
    ,rol.rolname as OBJECT_OWNER
    ,case cls.relkind
        when 'r' then 'TABLE'
        when 'm' then 'MATERIALIZED_VIEW'
        when 'i' then 'INDEX'
        when 'S' then 'SEQUENCE'
        when 'v' then 'VIEW'
        when 'c' then 'TYPE'
		when 'f' then 'FOREIGN_TABLE'
		when 't' then 'TOAST_TABLE'
        else cls.relkind::text
    end as OBJECT_TYPE
    , pg_catalog.obj_description(cls.oid, 'pg_class') 
from pg_class cls
join pg_roles rol 
	on rol.oid = cls.relowner
join pg_namespace nsp 
	on nsp.oid = cls.relnamespace
where nsp.nspname not in ('information_schema', 'pg_catalog')
    and nsp.nspname not like 'pg_toast%'
    and rol.rolname = current_user
    and cls.relkind in ('r', 'v', 'm', 'f')
order by nsp.nspname, cls.relname";
                Result = this.QueryDataTable(strDbaObjects);
                if (Result != null)
                    Result.TableName = "UserTables";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }

        public override DataTable UserColumns()
        {
            DataTable Result = null;
            try
            {
                string strColumns = @"SELECT 
    table_catalog OWNER_NAME
    , table_schema SCHEMA_NAME
	, table_name TABLE_NAME
    , column_name COLUMN_NAME
	, case udt_name when 'int4' then 'integer' else udt_name end DATA_TYPE
	, COALESCE(datetime_precision, numeric_precision, numeric_precision_radix, character_maximum_length) DATA_LENGTH
	, numeric_precision DATA_PRECISION
    , COALESCE(datetime_precision, numeric_scale) DATA_SCALE
	, case is_nullable WHEN 'YES' THEN 'Y' WHEN 'NO' THEN 'N' ELSE is_nullable END NULLABLE
    , ordinal_position COLUMN_ID
  FROM information_schema.columns
  WHERE 1 = 1 AND table_schema not like 'pg_%' AND table_schema<> 'information_schema' AND table_catalog = current_user"
;
                Result = this.QueryDataTable(strColumns);
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
                string strColumns = @"SELECT 
    table_catalog OWNER_NAME
    , table_schema SCHEMA_NAME
	, table_name TABLE_NAME
    , column_name COLUMN_NAME
	, case udt_name when 'int4' then 'integer' else udt_name end DATA_TYPE
	, COALESCE(datetime_precision, numeric_precision, numeric_precision_radix, character_maximum_length) DATA_LENGTH
	, numeric_precision DATA_PRECISION
    , COALESCE(datetime_precision, numeric_scale) DATA_SCALE
	, case is_nullable WHEN 'YES' THEN 'Y' WHEN 'NO' THEN 'N' ELSE is_nullable END NULLABLE
    , ordinal_position COLUMN_ID
  FROM information_schema.columns
  WHERE 1 = 1 AND table_schema not like 'pg_%' AND table_schema<> 'information_schema' AND table_catalog = current_user"
;
                string SQL = string.Format("SELECT * FROM ( {0} ) as cols WHERE 1 = 1 AND TABLE_NAME = :paramTableName AND COLUMN_NAME = :paramColName", strColumns);
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
                string SQL = string.Format("SELECT TO_CHAR({0}, '{1}') AS NOW_DATE", NowDateTimeDbFunctionString, dateFormatString);
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

        protected override DataTable QueryStoredProcedureDataTable(string queryString, TDbParameter[] parameters)
        {
            throw new NotImplementedException();
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
}
