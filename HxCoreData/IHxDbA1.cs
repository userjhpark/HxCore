using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace HxCore.Data
{
    internal interface IHxDbA1<TDbFacoty, TDbConnection, TDbTransaction, TDbCommand, TDbParameter, TDbDataReader>
        where TDbFacoty : DbProviderFactory
        where TDbConnection : DbConnection, IDbConnection
        where TDbTransaction : DbTransaction, IDbTransaction
        where TDbCommand : DbCommand, IDbCommand
        where TDbParameter : DbParameter, IDataParameter
        where TDbDataReader : DbDataReader, IDataReader
    {
        bool ColNameToLower { get; set; }
        TDbCommand Command { get; }
        TDbConnection Conn { get; }
        TDbFacoty DbFactory { get; }
        HxDbOptionRec DbOption { get; }
        bool IsConnRef { get; }
        bool IsDebug { get; set; }
        bool IsOpen { get; }
        bool IsTrans { get; }
        string ParamterSeparatorChar { get; }
        string ProviderName { get; }
        HxDbProviderType ProviderType { get; }
        int QueryID { get; }
        TDbDataReader Reader { get; }
        Dictionary<int, TDbDataReader> ReaderList { get; }
        string SeqTableName { get; }
        TDbTransaction Trans { get; }

        bool BeginTransaction();
        bool Close(int parse = -1);
        bool ColumnContains(string tableName, string columnName);
        bool Commit();
        void Connect(string connectionString);
        void Connect(string userID, string password, string database, string character = null);
        bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects);
        int CurrID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto);
        bool EndTransaction();
        object f(int columnIndex, int parse = -1);
        object f(string columnName, int parse = -1);
        void Free();
        int GetColumnCount(int parse = -1);
        bool GetColumnNameToLower();
        Type GetColumnType(int columnIndex, int parse = -1);
        Type GetColumnType(string columnName, int parse = -1);
        string GetConnectionString(string userID, string password, string database, string character = null);
        string GetName();
        TDbParameter GetParameter(string name, object value);
        TDbParameter[] GetParameters(Dictionary<string, object> bind);
        string GetParamterSeparatorChar();
        HxDbProviderType GetProviderType();
        int GetRowCount();
        int GetRowCount(int parse = -1);
        object GetValue(int columnIndex, int parse = -1);
        object GetValue(string columnName, int parse = -1);
        void Halt(string message);
        bool IsOpened();
        bool IsTransaction();
        bool Lock(string tableName, HxModeType mode = HxModeType.Admin);
        List<HxDbColumnRec> MetaData(string tableName);
        int NextID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto);
        bool NextRecord(int parse = -1);
        int nf();
        string NowDateValue(string dateFormatString = null);
        string np();
        bool Open();
        string p(int columnIndex, int parse = -1);
        string p(string columnName, int parse = -1);
        int Query(string queryString, Dictionary<string, object> bind = null);
        int Query(string queryString, IDataParameter[] parameters);
        int Query(string queryString, TDbParameter[] bind);
        DataTable QueryDataTable(string queryString, IDataParameter[] parameters = null, bool bStoredProcedure = false);
        DataTable QueryDataTable(string queryString, Dictionary<string, object> bind, bool bStoredProcedure = false);
        object QueryOne(string queryString, Dictionary<string, object> bind = null);
        object QueryOne(string queryString, IDataParameter[] parameters);
        object QueryOne(string queryString, TDbParameter[] parameters);
        bool Rollback();
        bool SequenceContains(string name);
        void SetColumnNameConvert(DataTable data);
        void SetColumnNameToLower(bool isLower);
        void SetConnectionString(string connString);
        void SetConnectionString(string userID, string password, string database, string character = null);
        void SetDebugMode(bool bDebug);
        void SetOptions(HxDbOptionRec option);
        void SetParameters(TDbCommand cmd, Dictionary<string, object> bind = null);
        void SetParameters(TDbCommand cmd, TDbParameter[] bind);
        bool SynonymContains(string name);
        bool TableContains(string name);
        List<HxDbTableRec> TableNames();
        bool UnLock();
        DataTable UserColumns();
        DataTable UserTables();
        bool ViewContains(string name);
        List<HxDbTableRec> ViewNames();
    }
}