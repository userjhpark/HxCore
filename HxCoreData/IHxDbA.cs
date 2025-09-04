using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace HxCore.Data
{
    internal interface IHxDbA
    {
        bool ColNameToLower { get; }
        IDbCommand Command { get; }
        IDbConnection Conn { get; }
        DbProviderFactory DbFactory { get; }
        HxDbOptionRec DbOption { get; }
        bool IsConnRef { get; }
        bool IsDebug { get; set; }
        bool IsOpen { get; }
        bool IsTrans { get; }
        string ProviderName { get; }
        HxDbProviderType ProviderType { get; }
        int QueryID { get; }
        IDataReader Reader { get; }
        Dictionary<int, IDataParameter> ReaderList { get; }
        string SeqTableName { get; }
        IDbTransaction Trans { get; }

        bool BeginTransaction();
        bool Close(int parse = -1);
        bool ColumnContains(string tableName, string columnName);
        bool Commit();
        void Connect(string connectionString);
        void Connect(string userID, string password, string database, string character = null);
        bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects);
        int CurrID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto);
        bool EndTransaction();
#pragma warning disable IDE1006
        object f(int columnIndex, int parse = -1);
        object f(string columnName, int parse = -1);
        int nf();
        string np();
        string p(int columnIndex, int parse = -1);
        string p(string columnName, int parse = -1);
#pragma warning restore IDE1006
        void Free();
        int GetColumnCount(int parse = -1);
        bool GetColumnNameToLower();
        Type GetColumnType(int columnIndex, int parse = -1);
        Type GetColumnType(string columnName, int parse = -1);
        string GetConnectionString(string userID, string password, string database, string character = null);
        bool GetIsOpened();
        string GetName();
        IDataParameter GetParameter(string name, object value);
        IDataParameter[] GetParameters(Dictionary<string, object> bind);
        int GetRowCount();
        int GetRowCount(int parse = -1);
        object GetValue(int columnIndex, int parse = -1);
        object GetValue(string columnName, int parse = -1);
        void Halt(string message);
        bool Lock(string tableName, HxModeType mode = HxModeType.Admin);
        List<HxDbColumnRec> MetaData(string tableName);
        int NextID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto);
        bool NextRecord(int parse = -1);

        bool Open();

        int Query(string queryString, Dictionary<string, object> bind = null);
        int Query(string queryString, IDataParameter[] bind);
        DataTable QueryDataTable(string queryString, Dictionary<string, object> bind = null);
        DataTable QueryDataTable(string queryString, IDataParameter[] bind);
        bool Rollback();
        bool SequenceContains(string name);
        void SetColumnNameConvert(DataTable dataTable);
        void SetColumnNameToLower(bool isLower);
        void SetConnectionString(string connString);
        void SetConnectionString(string userID, string password, string database, string character = null);
        void SetDebugMode(bool bDebug);
        void SetOptions(HxDbOptionRec option);
        void SetParameters(IDbCommand cmd, Dictionary<string, object> bind = null);
        void SetParameters(IDbCommand cmd, IDbDataParameter[] bind);
        bool SynonymContains(string name);
        bool TableContains(string name);
        List<HxDbTableRec> TableNames();
        bool UnLock();
        DataTable UserColumns();
        DataTable UserTables();
        bool ViewContains(string name);
        List<HxDbTableRec> ViewNames();

        string NowDateValue(string dateFormatString = null);
    }
}