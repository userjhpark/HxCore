using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.Data.Sqlite;
//using TFacoty = Microsoft.Data.Sqlite.SqliteFactory;
//using TConnection = Microsoft.Data.Sqlite.SqliteConnection;
//using TTransaction = Microsoft.Data.Sqlite.SqliteTransaction;
//using TCommand = Microsoft.Data.Sqlite.SqliteCommand;
//using TParameter = Microsoft.Data.Sqlite.SqliteParameter;
//using TDataReader = Microsoft.Data.Sqlite.SqliteDataReader;
//using TDataAdapter = System.Data.Common.DbDataAdapter;

namespace HxCore.Data
{
    using System.Data;
    using System.Data.SQLite;
    using TFacoty = System.Data.SQLite.SQLiteFactory;
    using TConnection = System.Data.SQLite.SQLiteConnection;
    using TTransaction = System.Data.SQLite.SQLiteTransaction;
    using TCommand = System.Data.SQLite.SQLiteCommand;
    using TParameter = System.Data.SQLite.SQLiteParameter;
    using TDataReader = System.Data.SQLite.SQLiteDataReader;
    using TDataAdapter = System.Data.SQLite.SQLiteDataAdapter;
    public class HxDbSQLite : HxDbA<TFacoty, TConnection, TTransaction, TCommand, TParameter, TDataReader, TDataAdapter>
    {
        public override string GetName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            //throw new NotImplementedException();
        }
        public override string ParamterSeparatorChar => "@";

        #region Static Intance
        private static HxDbSQLite _instance = null;
        static HxDbSQLite()
        {
            _instance = new HxDbSQLite();
        }
        /// <summary>
        /// [Static]Instance Object
        /// </summary>
        public static HxDbSQLite Instance
        {
            get { return _instance ?? (_instance = new HxDbSQLite()); }
            private set { _instance = value; }
        }
        #endregion
        #region 생성자
        public static HxDbSQLite Create()
        {
            return new HxDbSQLite();
        }
        /// <summary>
        /// 생성자
        /// </summary>
        public HxDbSQLite()
            : base(HxDbProviderType.SQLite)
        {
            this.InitVarTypes();
        }

        protected override void InitVarTypes()
        {
            this.DbFactory = TFacoty.Instance;
            if (this.DbFactory != null)
            {
                this.Conn = (TConnection)this.DbFactory.CreateConnection();

                this.ConnStrBuilder = DbFactory.CreateConnectionStringBuilder();
                this.Command = (TCommand)this.DbFactory.CreateCommand();
                //TParameter param = (TParameter)this.DbFactory.CreateParameter();
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionResource">Connection Resource</param>
        public HxDbSQLite(TConnection connectionResource)
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

        public HxDbSQLite(HxDbConnectionRec connection)
            : base(connection.ProviderType, connection.User, connection.Password, connection.HostName, connection.Character)
        {
            ; ;
        }

        public HxDbSQLite Create(string connectionString, HxDbOptionRec option = default(HxDbOptionRec))
        {
            return new HxDbSQLite(connectionString, option);
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionString">DB Connection String</param>
        /// <param name="option">DB 접속 옵션</param>
        public HxDbSQLite(string connectionString, HxDbOptionRec option = default(HxDbOptionRec))
            : base(HxDbProviderType.SQLite, connectionString, option)
        {
            this.InitVarTypes();
        }

        public HxDbSQLite(string userID, string password, string database, string character = null) : base(HxDbProviderType.SQLite, userID, password, database, character)
        {
            this.InitVarTypes();
        }

        public HxDbSQLite(string userID, string password, string database, HxDbOptionRec option) : base(HxDbProviderType.SQLite, userID, password, database, option)
        {
            this.InitVarTypes();
        }

        #endregion

        /// <summary>
        /// Sequence Table 생성(Create)
        /// </summary>
        protected override string CreateSeqTableQueryString
        {
            get
            {
                //string Result = string.Format("CREATE TABLE {0} ( seq_name varchar(30) NOT NULL Primary Key, nextid int NOT NULL DEFAULT 1, reg_date DATETIME DEFAULT (DateTime('now')), mod_date DATETIME DEFAULT (DateTime('now')))", this.SeqTableName);

                StringBuilder S = new StringBuilder();
                S.AppendFormat("CREATE TABLE {0} (", this.SeqTableName);
                S.AppendFormat("  seq_name varchar(30) NOT NULL Primary Key,");
                S.AppendFormat("  nextid int NOT NULL DEFAULT 1,");
                S.AppendFormat("  reg_date DATETIME DEFAULT ({0}), ", this.NowDateTimeDbFunctionString);
                S.AppendFormat("  mod_date DATETIME DEFAULT ({0}) ", this.NowDateTimeDbFunctionString);
                S.Append(")");
                return S.ToString();
            }
        }

        /// <summary>
        /// Get Now DateTime Database Function Format
        /// </summary>
        protected override string NowDateTimeDbFunctionString
        {
            get { return "DateTime('now')"; }
        }
#pragma warning disable CS0809 // 명명 스타일

        /// <summary>
        /// Set Database Options
        /// </summary>
        /// <param name="option">Database Options</param>
        [Obsolete("미구현! 아무 기능도 하지 않습니다.", true)]
        public override void SetOptions(HxDbOptionRec option)
        {
            ; ;
        }
        /// TODO : 미구현!
        public override int GetRowCount(int parse = -1)
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override DataTable UserTables()
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override DataTable UserColumns()
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects)
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override bool TableContains(string name)
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override bool ViewContains(string name)
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override bool SynonymContains(string name)
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override bool SequenceContains(string name)
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override bool ColumnContains(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }
        /// TODO : 미구현!
        public override string NowDateValue(string dateFormatString = null)
        {
            throw new NotImplementedException();
        }

        protected override DataTable QueryStoredProcedureDataTable(string queryString, TParameter[] parameters)
        {
            throw new NotImplementedException();
        }
#pragma warning restore CS0809 // 명명 스타일
    }
}
