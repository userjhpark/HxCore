using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace HxCore.Data
{
    public class HxSQL : HxBase, IHxBase, IHxDb
    {
        private IHxDb FDB = null;
        public IHxDb DB { get => this.FDB; protected set => SetDb(value); }

        
        public HxSQL(HxDbConnectionRec connection)
            : this(connection.ProviderType, connection.User, connection.Password, connection.HostName, connection.Character, connection.Pooling)
        {
            ; ;
        }

        public HxSQL(HxDbProviderType providerType, string connectionString = null)
        {
            this.DB = CreateDb(providerType, connectionString);
        }

        public HxSQL(HxDbProviderType providerType, string userID, string password, string database, string character = null, bool? pooling = null)
        {
            this.DB = CreateDb(providerType, userID, password, database, character, pooling);
        }

        public HxDbProviderType GetProviderType()
        {
            return this.DB.GetProviderType();
        }



        public virtual void SetDb(IHxDb db)
        {
            this.FDB = db;
        }
        public static IHxDb CreateDb(HxDbProviderType providerType, string connectionString)
        {
            IHxDb Result = null;
            switch (providerType)
            {
                case HxDbProviderType.OCI:
                    Result = new HxDbOci(connectionString);
                    //this.SetDb(new HxDbOci(connectionString));
                    break;
                case HxDbProviderType.SQLite:
                    Result = new HxDbSQLite(connectionString);
                    //this.SetDb(new HxDbSQLite(connectionString));
                    break;
                case HxDbProviderType.PostgreSQL:
                    Result = new HxDbNpgsql(connectionString);
                    //this.SetDb(new HxDbSQLite(connectionString));
                    break;
                case HxDbProviderType.MsSQL:
                    Result = null;
                    break;
                case HxDbProviderType.Common:
                case HxDbProviderType.Excel:
                case HxDbProviderType.Access:
                default:
                    //Result = new HxDbCommon(providerType);
                    Result = null;
                    break;
            }
            return Result;
        }
        public static IHxDb CreateDb(HxDbProviderType providerType, string userID, string password, string database, string character = null, bool ? pooling = null)
        {
            string connString = GetConnStr(providerType, userID, password, database, character, pooling);
            return CreateDb(providerType, connString);
        }
        public static IHxDb CreateDb(HxDbConnectionRec connection)
        {
            return CreateDb(connection.ProviderType, connection.User, connection.Password, connection.HostName, connection.Character, connection.Pooling);
        }

        public static string GetConnStr(HxDbProviderType providerType, string userID, string password, string database, string character = null, bool? pooling = null)
        {
            string Result = null;
            Result = HxDbUtils.ConnectionString(providerType, userID, password, database, character, pooling);
            //switch (providerType)
            //{
            //    case HxDbProviderType.OCI:
            //        Result = HxDbOci.Create().GetConnectionString(userID, password, database, character);
            //        //this.SetDb(new HxDbOci(connectionString));
            //        break;
            //    case HxDbProviderType.SQLite:
            //        Result = HxDbSQLite.Create().GetConnectionString(null, null, database, null);
            //        HxDbSQLite a = HxDbSQLite.Create();
            //        //this.SetDb(new HxDbSQLite(connectionString));
            //        break;
            //    case HxDbProviderType.Common:
            //    case HxDbProviderType.MsSQL:
            //    case HxDbProviderType.Excel:
            //    case HxDbProviderType.Access:
            //    default:
            //        throw new NotSupportedException();
            //}
            return Result;
        }

        #region IHxDb 구현
        public void SetDebugMode(bool bDebug)
        {
            this.DB.SetDebugMode(bDebug);
        }

        public string GetConnectionString(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            return this.DB.GetConnectionString(userID, password, database, character, pooling);
        }

        public void SetConnectionString(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            this.DB.SetConnectionString(userID, password, database, character, pooling);
        }

        public void SetConnectionString(string connString)
        {
            this.DB.SetConnectionString(connString);
        }

        public void Connect(string userID, string password, string database, string character = null, bool? pooling = null)
        {
            this.DB.Connect(userID, password, database, character, pooling);
        }

        public void Connect(string connectionString)
        {
            this.DB.Connect(connectionString);
        }

        public bool GetColumnNameToLower()
        {
            return this.DB.GetColumnNameToLower();
        }

        public void SetColumnNameToLower(bool bLower)
        {
            this.DB.SetColumnNameToLower(bLower);
        }
        public DataTable QueryDataTable(string queryString, IDataParameter[] parameters = null, bool bStoredProcedure = false)
        {
            return this.DB.QueryDataTable(queryString, parameters, bStoredProcedure);
        }
        public DataTable QueryDataTable(string queryString, Dictionary<string, object> bind, bool bStoredProcedure = false)
        {
            return this.DB.QueryDataTable(queryString, bind, bStoredProcedure);
        }
        public DataTable QueryDataTable(string queryString, string subWhereString, Dictionary<string, object> bind = null, bool bStoredProcedure = false)
        {
            return this.DB.QueryDataTable(queryString, subWhereString, bind, bStoredProcedure);
        }

        public int Query(string queryString, Dictionary<string, object> bind = null)
        {
            return this.DB.Query(queryString, bind);
        }

        public int Query(string queryString, IDataParameter[] bind)
        {
            return this.DB.Query(queryString, bind);
        }

        public object QueryOne(string queryString, Dictionary<string, object> bind = null)
        {
            return this.DB.QueryOne(queryString, bind);
        }

        public object QueryOne(string queryString, IDataParameter[] bind)
        {
            return this.DB.QueryOne(queryString, bind);
        }

        public int GetColumnCount(int parse = -1)
        {
            return this.GetColumnCount(parse);
        }

        public int GetRowCount(int parse = -1)
        {
            return this.GetRowCount(parse);
        }

        public int nf()
        {
            return this.DB.nf();
        }

        public string np()
        {
            return this.DB.np();
        }

        public bool NextRecord(int parse = -1)
        {
            return DB.NextRecord(parse);
        }

        public object f(string columnName, int parse = -1)
        {
            return DB.f(columnName, parse);
        }

        public object f(int columnIndex, int parse = -1)
        {
            return DB.f(columnIndex, parse);
        }

        public string p(string columnName, int parse = -1)
        {
            return DB.p(columnName, parse);
        }

        public string p(int columnIndex, int parse = -1)
        {
            return DB.p(columnIndex, parse);
        }

        public object GetValue(string columnName, int parse = -1)
        {
            return DB.GetValue(columnName, parse);
        }

        public object GetValue(int columnIndex, int parse = -1)
        {
            return DB.GetValue(columnIndex, parse);
        }

        public Type GetColumnType(string columnName, int parse = -1)
        {
            return DB.GetColumnType(columnName, parse);
        }

        public Type GetColumnType(int columnIndex, int parse = -1)
        {
            return DB.GetColumnType(columnIndex, parse);
        }

        public int NextID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto)
        {
            return DB.NextID(sequencesName, mode);
        }

        public int CurrID(string sequecesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto)
        {
            return DB.CurrID(sequecesName, mode);
        }

        public List<HxDbColumnRec> MetaData(string tableName)
        {
            return DB.MetaData(tableName);
        }
    

        public List<HxDbTableRec> TableNames()
        {
            return DB.TableNames();
        }

        public List<HxDbTableRec> ViewNames()
        {
            return DB.ViewNames();
        }

        public bool Lock(string tableName, HxModeType mode = HxModeType.Write)
        {
            return DB.Lock(tableName, mode);
        }

        public bool UnLock()
        {
            return DB.UnLock();
        }

        public bool IsTransaction()
        {
            return DB.IsTransaction();
        }

        public bool BeginTransaction()
        {
            return DB.BeginTransaction();
        }

        public bool EndTransaction()
        {
            return DB.EndTransaction();
        }

        public bool Commit()
        {
            return DB.Commit();
        }

        public bool Rollback()
        {
            return DB.Rollback();
        }

        public bool Open()
        {
            if (this.FDB != null)
            {
                return DB.Open();
            }
            return false;
        }

        public bool Close(int queryID = int.MinValue)
        {
            return DB.Close(queryID);
        }

        public void Free()
        {
            DB?.Free();
        }

        public void Halt(string message)
        {
            DB?.Halt(message);
        }

        public DataTable UserTables()
        {
            return DB.UserTables();
        }

        public DataTable UserColumns()
        {
            return DB.UserColumns();
        }

        public bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects)
        {
            return DB.Contains(name, objectType);
        }

        public bool TableContains(string name)
        {
            return DB.TableContains(name);
        }

        public bool ViewContains(string name)
        {
            return DB.ViewContains(name);
        }

        public bool SynonymContains(string name)
        {
            return DB.SynonymContains(name);
        }

        public bool SequenceContains(string name)
        {
            return DB.SequenceContains(name);
        }

        public bool ColumnContains(string tableName, string columnName)
        {
            return DB.ColumnContains(tableName, columnName);
        }

        public string NowDateValue(string dateFormatString = null)
        {
            return DB.NowDateValue(dateFormatString);
        }

        public string GetParamterSeparatorChar()
        {
            return DB.GetParamterSeparatorChar();
        }

        public string GetSchemaName()
        {
            return DB.GetSchemaName();
        }

        public void SetOptions(HxDbOptionRec option)
        {
            try
            {
                DB.SetOptions(option);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw;
            }
        }





        #endregion
    }
}
