using HxCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HxCore
{
    public interface IHxDb : IDisposable
    {
        #region 2019년 추가 - v1.0.2
        /// <summary>
        /// Get DB Provider Type
        /// </summary>
        /// <returns>Provider Enum Type</returns>
        HxDbProviderType GetProviderType();

        string GetParamterSeparatorChar();

        #endregion
        // <summary>
        /// Debug Mode
        /// </summary>
        /// <param name="bDebug">True : Debug</param>
        void SetDebugMode(bool bDebug);

        /// <summary>
        /// Get Connection Params To Connection String(Connection Object 정보 아님(Not))
        /// </summary>
        /// <param name="userID">Database User ID</param>
        /// <param name="password">Database User Password</param>
        /// <param name="database">Database Service Name Or Server Host Name</param>
        /// <param name="character">Database Character</param>
        /// <returns>Connection String</returns>
        string GetConnectionString(string userID, string password, string database, string character = null, bool? pooling = null);
        /// <summary>
        /// Set Connection Params To Connection Object
        /// </summary>
        /// <param name="userID">Database User ID</param>
        /// <param name="password">Database User Password</param>
        /// <param name="database">Database Service Name Or Server Host Name</param>
        /// <param name="character">Database Character(Database Type에 따른 Option)</param>
        void SetConnectionString(string userID, string password, string database, string character = null, bool ? pooling = null);
        /// <summary>
        /// Set Connection String To Connection Object
        /// </summary>
        /// <param name="connString">Database Connection String</param>
        void SetConnectionString(string connString);
        /// <summary>
        /// Connection Params To Connection Object
        /// </summary>
        /// <param name="userID">Database User ID</param>
        /// <param name="password">Database User Password</param>
        /// <param name="database">Database Service Name Or Server Host Name</param>
        /// <param name="character">Database Character(Database Type에 따른 Option)</param>
        void Connect(string userID, string password, string database, string character = null, bool? pooling = null);
        /// <summary>
        /// Connection String To Connection Object
        /// </summary>
        /// <param name="connectionString">Database Connection String</param>
        void Connect(string connectionString);

        /// <summary>
        /// Column Name To Lower?
        /// </summary>
        /// <returns>True : Low, False : Original</returns>
        bool GetColumnNameToLower();
        /// <summary>
        /// Set Column Name To Lower
        /// </summary>
        /// <param name="bLower">True : Low, False : Original</param>
        void SetColumnNameToLower(bool bLower);

        //DataTable QueryDataTable(string queryString, IDataParameter[] bind = null);
        /// <summary>
        /// Query Result To DataTable
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="bind">Parameter Binding Object</param>
        /// <returns>Query Result(DataTable)</returns>
        DataTable QueryDataTable(string queryString, Dictionary<string, object> bind, bool bStoredProcedure = false);
        //DataTable QueryDataTable(string queryString, IDataParameter[] bind = null);
        /// <summary>
        /// Query Result To DataTable
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="bind">IDataParameter Object</param>
        /// <returns>Query Result(DataTable)</returns>
        DataTable QueryDataTable(string queryString, IDataParameter[] parameters = null, bool bStoredProcedure = false);

        //DataTable QueryDataTable(string queryString, IDataParameter[] bind = null);
        /// <summary>
        /// Query Result To DataTable
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="subWhereString">(Sub) Where Case</param>
        /// <param name="bind">Parameter Binding Object</param>
        /// <returns>Query Result(DataTable)</returns>
        DataTable QueryDataTable(string queryString, string subWhereString, Dictionary<string, object> bind = null, bool bStoredProcedure = false);

        /// <summary>
        /// Query Execute(Do, Run)
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="bind">Parameter Binding Object</param>
        /// <returns>Select : Query Execute ID(Index) / Insert,Update,Delete,... Apply Row Count</returns>
        int Query(string queryString, Dictionary<string, object> bind = null);
        /// <summary>
        /// Query Execute(Do, Run)
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="parameters">IDataParameter Object</param>
        /// <returns>Select : Query Execute ID(Index) / Insert,Update,Delete,... Apply Row Count</returns>
        int Query(string queryString, IDataParameter[] parameters);
        /// <summary>
        /// Query One Column, One Row Execute(Do, Run)
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="bind">Parameter Binding Resource</param>
        /// <returns>Object Value</returns>
        object QueryOne(string queryString, Dictionary<string, object> bind = null);
        /// <summary>
        /// Query One Column, One Row Execute(Do, Run)
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="parameters">Parameter Binding Resource</param>
        /// <returns>Object Value</returns>
        object QueryOne(string queryString, IDataParameter[] parameters);

        /// <summary>
        /// Get Query Execute Result To Column(Field) Count
        /// </summary>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Column(Field) Count</returns>
        int GetColumnCount(int parse = -1);

        /// <summary>
        /// Get Last Query Execute Result To Row(Record) Count
        /// </summary>
        /// <returns>Row(Record) Count</returns>
        int GetRowCount(int parse = -1);

#pragma warning disable IDE1006 // 명명 스타일
        /// <summary>
        /// Get Last Query Execute Result To Row(Record) Count
        /// </summary>
        /// <returns>Row(Record) Count</returns>
        int nf();

        /// <summary>
        /// Get Last Query Execute Result To Row(Record) Count(String)
        /// </summary>
        /// <returns>Row(Record) Count To String</returns>
        string np();
        /// <summary>
        /// Get Query Execute To MoveNext?
        /// </summary>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>MoveNext?</returns>
        bool NextRecord(int parse = -1);
        /// <summary>
        /// Get Current Row(Record) From Value(object)
        /// </summary>
        /// <param name="columnName">Column(Field) Name</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Value(object)</returns>
        object f(string columnName, int parse = -1);
        /// <summary>
        /// Get Current Row(Record) From Value(object)
        /// </summary>
        /// <param name="columnIndex">Column(Field) Index</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Value(object)</returns>
        object f(int columnIndex, int parse = -1);
        /// <summary>
        /// Get Current Row(Record) From Value(string)
        /// </summary>
        /// <param name="columnName">Column(Field) Name</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Value(string)</returns>
        string p(string columnName, int parse = -1);
        /// <summary>
        /// Get Current Row(Record) From Value(string)
        /// </summary>
        /// <param name="columnIndex">Column(Field) Index</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Value(string)</returns>
        string p(int columnIndex, int parse = -1);
#pragma warning restore IDE1006 // 명명 스타일
        /// <summary>
        /// Get Current Row(Record) From Value(object)
        /// </summary>
        /// <param name="columnName">Column(Field) Name</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Value(object)</returns>
        object GetValue(string columnName, int parse = -1);
        /// <summary>
        /// Get Current Row(Record) From Value(object)
        /// </summary>
        /// <param name="columnIndex">Column(Field) Index</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Value(object)</returns>
        object GetValue(int columnIndex, int parse = -1);

        /// <summary>
        /// Get Column Type
        /// </summary>
        /// <param name="columnName">Column(Field) Name</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Column Type</returns>
        Type GetColumnType(string columnName, int parse = -1);
        /// <summary>
        /// Get Column Type
        /// </summary>
        /// <param name="columnName">Column(Field) Index</param>
        /// <param name="parse">Query ID(Index)</param>
        /// <returns>Column Type</returns>
        Type GetColumnType(int columnIndex, int parse = -1);
        /// <summary>
        /// Get Sequence Next Value(Number)
        /// </summary>
        /// <param name="sequencesName">Sequence 명</param>
        /// <param name="mode">Db Sequence 적용 타입(Auto, UseTable : Table이용, UseOracleSequence : Oracle일 경우 Sequence 객체 이용)</param>
        /// <returns>Sequence Number</returns>
        int NextID(string sequencesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto);


        /// <summary>
        ///  Get Sequence Session Current Value(Number)
        /// </summary>
        /// <param name="sequecesName">Sequence 명</param>
        /// <param name="mode">Db Sequence 적용 타입(Auto, UseTable : Table이용, UseOracleSequence : Oracle일 경우 Sequence 객체 이용)</param>
        /// <returns>Sequence Number</returns>
        int CurrID(string sequecesName, HxDbSeqModeType mode = HxDbSeqModeType.Auto);


        /// <summary>
        /// Get Db Table의 Column 정보
        /// </summary>
        /// <param name="tableName">Db Table명</param>
        /// <returns>Columns Info.</returns>
        List<HxDbColumnRec> MetaData(string tableName);
        /// <summary>
        /// Get Db Table들의 정보
        /// </summary>
        /// <returns>Tables Info.</returns>
        List<HxDbTableRec> TableNames();
        /// <summary>
        /// Get Db View들의 정보
        /// </summary>
        /// <returns>View Tables Info.</returns>
        List<HxDbTableRec> ViewNames();

        /// <summary>
        /// 특정 테이블 Lock 걸기
        /// </summary>
        /// <param name="tableName">Db Table명</param>
        /// <param name="mode">Lock Mode(CRUD 개념) Type</param>
        /// <returns>Lock 결과</returns>
        bool Lock(string tableName, HxModeType mode = HxModeType.Writer); //(dnModeType.Create | dnModeType.Update | dnModeType.Delete)
        /// <summary>
        /// Lock 해제
        /// </summary>
        /// <returns>Unlock 결과</returns>
        bool UnLock();

        bool IsTransaction();
        /// <summary>
        /// Db Transaction 시작
        /// </summary>
        /// <returns>Transaction 시작 결과</returns>
        bool BeginTransaction();
        /// <summary>
        /// Db Transaction 종료
        ///     * Commit전 종료시 Rollback 됨
        /// </summary>
        /// <returns>Transaction 종료 결과</returns>
        bool EndTransaction();
        /// <summary>
        /// Db Transaction에 반영 대기 중인 항목 저장(반영)
        /// </summary>
        /// <returns>반영 결과</returns>
        bool Commit();
        /// <summary>
        /// Db Transaction에 반영 대기 중인 항목 취소
        /// </summary>
        /// <returns>반영 결과</returns>
        bool Rollback();
        /// <summary>
        /// Db Open
        /// </summary>
        /// <returns>Open 결과(True : 성공, false : 실패)</returns>
        bool Open();
        /// <summary>
        /// Query 결과 Resource Close
        /// </summary>
        /// <param name="queryID">Query ID(Index) / Default : Last Query ID</param>
        /// <returns>반영 결과</returns>
        bool Close(int queryID = int.MinValue);
        /// <summary>
        /// Db 관련 모든 Resource 반환
        /// </summary>
        void Free();
        /// <summary>
        /// Db Error에 의한 종료
        /// </summary>
        /// <param name="message">Error / Halt Message</param>
        void Halt(string message);

        string GetSchemaName();

        DataTable UserTables();

        DataTable UserColumns();

        /// <summary>
        /// Db에 해당 Object(Table, View, ...) 존재 여부
        /// </summary>
        /// <param name="name">Object Name</param>
        /// <param name="objectType">Db Object Type</param>
        /// <returns>존재 여부</returns>
        bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects);

        /// <summary>
        /// Db에 해당 Table 존재 여부
        /// </summary>
        /// <param name="name">Table Name</param>
        /// <returns>존재 여부</returns>
        bool TableContains(string name);

        /// <summary>
        /// Db에 해당 View 존재 여부
        /// </summary>
        /// <param name="name">View Table Name</param>
        /// <returns>존재여부</returns>
        bool ViewContains(string name);

        /// <summary>
        /// Db에 Synonym 존재 여부
        /// </summary>
        /// <param name="name">Synonym Name</param>
        /// <returns></returns>
        bool SynonymContains(string name);

        /// <summary>
        /// Db에 Seq. 존재 여부
        /// </summary>
        /// <param name="name">Sequence Name</param>
        /// <returns>존재 여부</returns>
        bool SequenceContains(string name);

        /// <summary>
        /// Db의 해당 Table 또는 View에 해당 Column 존재 여부
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        bool ColumnContains(string tableName, string columnName);

        string NowDateValue(string dateFormatString = null);

        void SetOptions(HxDbOptionRec option);
    }
}
