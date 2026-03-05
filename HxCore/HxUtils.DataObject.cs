using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Common;

namespace HxCore
{
    partial class HxUtils
    {
        //public const string _FORMAT_DATETIME_ORACLE_ = "YYYY-MM-DD HH24:MI:SS";
        //public const string _FORMAT_DATETIME_Csharp_ = "yyyy-MM-dd HH:mm:ss";
        //public const string _PREFIX_PARAM_CLOB_ = "CLOB$$__";
        //public const string _PREFIX_PARAM_BLOB_ = "BLOB$$__";

        //public static DbProviderFactory DbFactory;

        //private static DbConnectionStringBuilder FDbConnStrB = null;

        //public static DbConnectionStringBuilder DbConnStrB
        //{
        //    get
        //    {
        //        if(FDbConnStrB == null)
        //        {
        //            FDbConnStrB = DbFactory.CreateConnectionStringBuilder();
        //        }
        //        return FDbConnStrB;
        //    }
        //    set
        //    {
        //        FDbConnStrB = value;
        //    }
        //}

        /// <summary>
        /// Connection String 만들기
        /// </summary>
        /// <param name="providerType">DB Type</param>
        /// <param name="userID">DB 사용자ID</param>
        /// <param name="password">DB 패스워드</param>
        /// <param name="database">DB Host/Name</param>
        /// <param name="character">DB 문자셋</param>
        /// <param name="pooling">Connection Pooling 사용 여부</param>
        /// <returns>Connection String</returns>
        public static string ConnectionString(HxDbProviderType providerType, string userID, string password, string database, string character = null, bool? pooling = null)
        {
            //DbConnectionStringBuilder thatConnStrBuilder = DbFactory.CreateConnectionStringBuilder();
            DbConnectionStringBuilder thatConnStrBuilder = new DbConnectionStringBuilder();
            //string FUserWinID;
            if (!userID.IsNullOrWhiteSpaceEx() && (userID.Trim() == "/" || userID.Trim().ToLower() == "sspi" || userID.Trim().ToLower() == "true"))
            {
                switch (providerType)
                {
                    case HxDbProviderType.OCI:
                        //FUserWinID = "/";
                        thatConnStrBuilder.Add("User Id", "/");
                        break;
                    case HxDbProviderType.MsSQL:
                        //FUserWinID = "true";
                        thatConnStrBuilder.Add("Trusted_Connection", "True");
                        break;
                    default:
                        //FUserWinID = userID;
                        thatConnStrBuilder.Add("User Id", userID);
                        break;
                }
            }
            else if (!userID.IsNullOrWhiteSpaceEx())
            {
                thatConnStrBuilder.Add("User Id", userID);
            }

            if (!password.IsNullOrWhiteSpaceEx())
            {
                if (password.Contains("?"))
                {
                    try
                    {
                        string[] splitText = password.Split('?');
                        string txtKey = null;
                        if (!splitText[0].IsNullOrWhiteSpaceEx())
                        {
                            txtKey = HxCrypt.Decrypt(splitText[0], null);
                        }
                        string txtValue = splitText[1];
                        if (txtValue.IsNullOrWhiteSpaceEx() && !txtKey.IsNullOrWhiteSpaceEx())
                        {
                            txtValue = txtKey;
                            txtKey = null;
                        }
                        password = HxCrypt.Decrypt(txtValue, txtKey);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        //throw;
                    }

                }
                thatConnStrBuilder.Add("Password", password);
            }

            if (!database.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    string strPattern = @"^([0-9a-zA-Z\.\-_]{1,})+(([:,]{1,1})([0-9]{1,5}))?([\/]{1,1}([0-9a-zA-Z\.\-_]{1,}))$";
                    if (providerType == HxDbProviderType.MsSQL || providerType == HxDbProviderType.PostgreSQL)
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

                        if (!strDbHost.IsNullOrWhiteSpaceEx() && providerType == HxDbProviderType.OCI || providerType == HxDbProviderType.Oracle)
                        {
                            if (strDbPort.IsNullOrWhiteSpaceEx())
                                strDbPort = "1521";
                            string strTns = string.Format("(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})))", strDbHost, strDbPort, strDbName);
                            //string strTns = string.Format("{0}:{1}/{2}", strDbHost, strDbPort, strDbName);
                            thatConnStrBuilder.Add("DATA SOURCE", strTns);
                        }
                        else if (!strDbHost.IsNullOrWhiteSpaceEx() && (providerType == HxDbProviderType.MsSQL))
                        {
                            if (strDbPort.IsNullOrWhiteSpaceEx())
                                strDbHost = string.Format("{0},{1}", strDbHost, strDbPort);
                            thatConnStrBuilder.Add("Server", strDbHost);
                            if (!strDbName.IsNullOrWhiteSpaceEx())
                                thatConnStrBuilder.Add("Database", strDbName);
                        }
                        else if (!strDbHost.IsNullOrWhiteSpaceEx() && (providerType == HxDbProviderType.PostgreSQL))
                        {
                            if (strDbPort.IsNullOrWhiteSpaceEx())
                                strDbPort = "5432";
                            thatConnStrBuilder.Add("Server", strDbHost);
                            thatConnStrBuilder.Add("Port", strDbPort);
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
                        if (providerType == HxDbProviderType.SQLite && database.StartsWith(@"\\"))
                            database = @"\\" + database;
                        thatConnStrBuilder.Add("DATA SOURCE", database);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            switch (providerType)
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
                    if (database != null && database.EndsWith(@".mdb"))
                    {
                        thatConnStrBuilder.Add("Provider", "Microsoft.Jet.OLEDB.4.0");
                    }
                    break;

            }

            if (pooling != null)
            {
                if (pooling == true)
                {
                    thatConnStrBuilder.Add("Pooling", "True");
                }
                else
                {
                    thatConnStrBuilder.Add("Pooling", "False");
                }
            }
            return thatConnStrBuilder.ToString();
        }
        public static string ConnectionString(HxDbConnectionRec connInfo)
        {
            return HxUtils.ConnectionString(connInfo.ProviderType, connInfo.User, connInfo.Password, connInfo.HostName, connInfo.Character, connInfo.Pooling);
        }
        //public static HxDbProviderType GetDbProviderType(string providerTypeString)
        //{
        //    return HxUtils.GetDbProviderType(providerTypeString);
        //}

        /// <summary>
        /// DataColumn  확장 속성
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="property_name">KEY</param>
        /// <param name="value">VALUE</param>
        /// <param name="bOverWrite">겹쳐쓰기(Override)</param>
        public static void DoExtendedPropertiesAdd(DataColumn sender, string property_name, object value, bool bOverWrite = true)
        {
            DoExtendedPropertiesAdd(sender.ExtendedProperties, property_name, value, bOverWrite);
        }
        /// <summary>
        /// Property  확장 속성
        /// </summary>
        /// <param name="sender">Source Resource</param>
        /// <param name="property_name">KEY</param>
        /// <param name="value">VALUE</param>
        /// <param name="bOverWrite">겹쳐쓰기(Override)</param>
        public static void DoExtendedPropertiesAdd(PropertyCollection sender, string property_name, object value, bool bOverWrite = true)
        {
            if ((!sender.ContainsKey(property_name) || bOverWrite == true))
            {
                DoPropertyValueAdd(sender, property_name, value, bOverWrite);
                //if (!sender.ContainsKey(property_name))
                //{
                //    sender.Add(property_name, value);
                //}
                //else
                //{
                //    sender[property_name] = value;
                //}
            }

        }

        /// <summary>
        /// Struct Source DataTable구조에 값 복사하기
        /// </summary>
        /// <param name="structSource">구조가 되는 DataTable</param>
        /// <param name="dataSource">값 복사대상이 되는 DataTable</param>
        /// <returns>값이 Copy된 DataTable</returns>
        public static DataTable CopyDataTable(DataTable structSource, DataTable dataSource)
        {
            DataTable Result = null;
            if (structSource != null && structSource.Columns.Count > 0)
            {
                Result = structSource.Clone();
            }

            if ((structSource == null || structSource.Columns.Count == 0) && dataSource != null)
            {
                Result = dataSource;
            }
            else
            {
                foreach (DataRow srcDr in dataSource.Rows)
                {
                    DataRow resultDr = Result.NewRow();
                    bool isFlag = false;
                    foreach (DataColumn resultDc in Result.Columns)
                    {
                        if (resultDc.ExtendedProperties["ColumnName"] != null)
                        {
                            string strExtColName = resultDc.ExtendedProperties["ColumnName"].ToString();
                            if (!HxString.IsNullOrWhiteSpace(strExtColName) && dataSource.Columns.Contains(strExtColName))
                            {
                                resultDr[resultDc.ColumnName] = srcDr[strExtColName];
                                isFlag = true;
                            }
                        }

                        //dr[srcCol
                    }
                    if (isFlag == true)
                    {
                        Result.Rows.Add(resultDr);
                    }
                }
            }
            return Result;
        }

        

        public static bool MergeDataTable(DataTable data1, DataTable data2)
        {
            bool Result = false;
            if (data1 == null) return Result;

            try
            {
                DataTable dt = data2.Copy();
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow row = data1.NewRow();
                    foreach (DataColumn col in data1.Columns)
                    {
                        if (dt.Columns.Contains(col.ColumnName) && col.AutoIncrement != true)
                        {
                            row[col.ColumnName] = dr[col.ColumnName];
                        }
                        //data1.ImportRow(row);

                    }
                    data1.Rows.Add(row);
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }

        public static bool MergeDataRow(ref DataTable data, DataRow row)
        {
            bool Result = false;
            if (row != null && row.Table != null)
            {
                if (data == null)
                {
                    data = row.Table.Copy();
                    Result = true;
                }
                else
                {
                    DataTable dt = row.Table.Clone();
                    dt.ImportRow(row);
                    Result = MergeDataTable(data, dt);
                }

            }
            return Result;
        }

        public static bool MergeDataRow(ref DataTable data, DataRow[] rows)
        {
            bool Result = false;

            if (rows != null && rows[0].Table != null)
            {
                foreach (DataRow row in rows)
                {
                    Result = MergeDataRow(ref data, row);
                }
            }
            return Result;
        }

        public static bool MergeDataColumn(ref DataTable dataTable, DataTable structDataTable)
        {
            bool Result = false;

            if (structDataTable == null || structDataTable.Columns.Count <= 0) return Result;

            if(dataTable == null)
            {
                dataTable = structDataTable.Clone();
                if(dataTable != null && dataTable.Columns.Count >= 0)
                {
                    Result = true;
                }
            }
            else
            {
                try
                {
                    int iCol = 0;
                    foreach (DataColumn col in structDataTable.Columns)
                    {
                        if (dataTable.Columns.Contains(col.ColumnName) != true)
                        {
                            DataColumn dc = new DataColumn(col.ColumnName, col.DataType);
                            if (col.DefaultValue != null)
                            {
                                dc.DefaultValue = col.DefaultValue;
                            }
                            if (col.AutoIncrement == true)
                            {
                                dc.AutoIncrement = col.AutoIncrement;
                            }
                            if (col.AutoIncrementSeed > 0)
                            {
                                dc.AutoIncrementSeed = col.AutoIncrementSeed;
                            }
                            if (col.AutoIncrementStep > 0)
                            {
                                dc.AutoIncrementStep = col.AutoIncrementStep;
                            }
                            dc.Caption = col.Caption;
                            dc.AllowDBNull = col.AllowDBNull;
                            dc.MaxLength = col.MaxLength;
                            dc.Expression = col.Expression;
                            dc.ReadOnly = col.ReadOnly;
                            dc.Unique = col.Unique;
                            dataTable.Columns.Add(dc);
                            iCol++;
                        }
                    }
                    if(iCol > 0)
                    {
                        Result = true;
                    }
                }
                catch (Exception ex)
                {
                    Result = false;
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            return Result;
        }
        public static bool MergeDataColumnAddRow(DataTable data1, DataTable data2, string columnName = null)
        {
            bool Result = false;

            if (data2 == null || data2.Columns.Count <= 0) return Result;

            if (data1 == null)
            {
                data1 = data2.Clone();
                if (data1 != null && data1.Columns.Count >= 0)
                {
                    Result = true;
                }
            }
            else
            {
                try
                {
                    int iCol = 0;
                    foreach (DataColumn col in data2.Columns)
                    {
                        if (data1.Columns.Contains(col.ColumnName) != true)
                        {
                            DataColumn dc = new DataColumn(col.ColumnName, col.DataType);
                            if (col.DefaultValue != null)
                            {
                                dc.DefaultValue = col.DefaultValue;
                            }
                            if (col.AutoIncrement == true)
                            {
                                dc.AutoIncrement = col.AutoIncrement;
                            }
                            if (col.AutoIncrementSeed > 0)
                            {
                                dc.AutoIncrementSeed = col.AutoIncrementSeed;
                            }
                            if (col.AutoIncrementStep > 0)
                            {
                                dc.AutoIncrementStep = col.AutoIncrementStep;
                            }
                            dc.Caption = col.Caption;
                            dc.AllowDBNull = col.AllowDBNull;
                            dc.MaxLength = col.MaxLength;
                            dc.Expression = col.Expression;
                            dc.ReadOnly = col.ReadOnly;
                            dc.Unique = col.Unique;
                            data1.Columns.Add(dc);
                            iCol++;
                        }
                    }
                    if (iCol > 0)
                    {
                        Result = true;
                    }
                    if(Result == true && columnName.IsNullOrWhiteSpaceEx() != true && data1.Columns.IndexOf(columnName) > -1 && data2.Columns.IndexOf(columnName) > -1)
                    {
                        foreach (DataRow row in data1.Rows)
                        {
                            string strKeyValue = row[columnName].ToStringEx();
                            if (strKeyValue.IsNullOrWhiteSpaceEx() != true)
                            {
                                DataRow findLastRow = data2.Select($"{columnName} = '{strKeyValue}'").LastOrDefault();
                                if(findLastRow != null)
                                {
                                    foreach(DataColumn dc in findLastRow.Table.Columns)
                                    {
                                        string strFindColumnName = dc.ColumnName;
                                        if(strFindColumnName != strKeyValue && row.Table.Columns.IndexOf(strFindColumnName) > -1)
                                        {
                                            row[strFindColumnName] = findLastRow[strFindColumnName];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Result = false;
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            return Result;
        }

        /// <summary>
        /// DataTable(with key / value COLUMNS)인 경우 최종값 하나만 가져오기
        /// </summary>
        /// <typeparam name="T">Retrun Type</typeparam>
        /// <param name="sender">DataTable Resource</param>
        /// <param name="name">Key name</param>
        /// <param name="column">Value Column-name</param>
        /// <returns>VALUE</returns>
        public static T SingleLastValue<T>(DataTable sender, string name, string column = "value")
        {
            T Result;
            //try
            //{
            //    Result = sender.AsEnumerable().Where(row => row.Field<string>("key").ToLower().Equals(name.ToLower())).LastOrDefault().Field<T>("value");
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //    throw ex;
            //}

            //return Result;
            bool bFlag = TryParseSingleLastValue<T>(sender, name, column, out Result);
            if (bFlag == false)
            {
                Result = default;
            }
            return Result;
        }

        /// <summary>
        /// DataTable(with key / value COLUMNS)인 경우 최종값 하나만 출력하고(OUT) 성공여부 리턴
        /// </summary>
        /// <typeparam name="T">OUT Type</typeparam>
        /// <param name="sender">DataTable Resource</param>
        /// <param name="name">Key name</param>
        /// <param name="column">Value Column-name</param>
        /// <param name="value">VALUE</param>
        /// <returns>성공 여부?</returns>
        public static bool TryParseSingleLastValue<T>(DataTable sender, string name, string column, out T value)
        {
            bool Result = false;
            //T obj = Activator.CreateInstance<T>();  
            value = default;
            try
            {
                if (column.IsNullOrWhiteSpaceEx())
                {
                    column = "value";
                }
                if (sender.Columns.Contains("key") && sender.Columns.Contains(column))
                {
                    value = sender.AsEnumerable().Where(row => row.Field<string>("key").ToLower().Equals(name.ToLower())).LastOrDefault().Field<T>(column);
                }
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                Debug.WriteLine(ex.Message);
                //throw ex;
            }

            return Result;
        }

        public static T TryGetValue<T>(DataTable sender, string name, string column = "value")
        {
            T Result;
            //try
            //{
            //    Result = sender.AsEnumerable().Where(row => row.Field<string>("key").ToLower().Equals(name.ToLower())).LastOrDefault().Field<T>("value");
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //    throw ex;
            //}

            //return Result;
            bool bFlag = TryParseSingleLastValue<T>(sender, name, column, out Result);
            if (bFlag == false)
            {
                Result = default;
            }
            return Result;
        }

        public static bool TryParseValue<T>(DataTable sender, string name, string column, out T value)
        {
            //T obj = Activator.CreateInstance<T>();  
            value = default;
            bool Result;
            try
            {
                if (column.IsNullOrWhiteSpaceEx())
                {
                    column = "value";
                }
                if (sender.Columns.Contains("key") && sender.Columns.Contains(column))
                {
                    //value = sender.AsEnumerable().Where(row => row.Field<string>("key").ToLower().Equals(name.ToLower())).Field<T>(column);
                    //value = sender.AsEnumerable().Where(row => row.Field<string>("key").ToLower().Equals(name.ToLower())).GetEnumerator().
                }
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                Debug.WriteLine(ex.Message);
                //throw ex;
            }

            return Result;
        }

        //================================

        /// <summary>
        /// DataTable의 특정Index를 Struct(Record)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="dataSource">Source DataTable</param>
        /// <param name="index">Index</param>
        /// <returns>Single Struct(Record)</returns>
        public static T ConvertDataTableToRecord<T>(DataTable dataSource, int index = 0)
            where T : IHxSetValue, new()
        {
            T Result = new T();
            try
            {
                int n = 0;
                if (dataSource != null && (n = dataSource.Rows.Count) > 0)
                {
                    if (index >= n)
                    {
                        index = n - 1;
                    }
                    Result.SetValue(dataSource, index);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        /// <summary>
        /// DataTable의 특정Index를 Struct(Record) Nullable Type으로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="dataSource">Source DataTable</param>
        /// <param name="index">Index</param>
        /// <returns>Single Struct(Record) : Nullable Type</returns>
        public static T ConvertDataTableToNullableRecord<T>(DataTable dataSource, int index = 0)
            where T : IHxSetValue, new()
        {
            T Result = default;
            try
            {
                int n = 0;
                if (dataSource != null && (n = dataSource.Rows.Count) > 0)
                {
                    if (index >= n)
                    {
                        index = n - 1;
                    }
                    Result = new T();
                    Result?.SetValue(dataSource, index);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Result;
                throw ex;
            }
            return Result;
        }
        /// <summary>
        /// DataTable을 Struct Array(RecordSet)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="dataSource">Source DataTable</param>
        /// <returns>Multiple Struct Array(RecordSet)</returns>
        public static T[] ConvertDataTableToRecordSet<T>(DataTable dataSource)
            where T : IHxSetValue, new()
        {
            T[] Result = null;
            try
            {
                int n = 0;
                if (dataSource != null && (n = dataSource.Rows.Count) > 0)
                {
                    Result = new T[n];
                    for (int i = 0; i < n; i++)
                    {
                        Result[i] = new T();
                        Result[i].SetValue(dataSource, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
                throw ex;
            }
            return Result;
        }
        /// <summary>
        /// DataRow를 Struct(Record)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="rowSource">Sorce DataRow</param>
        /// <returns>Single Struct(Record)</returns>
        public static T ConvertDataRowToRecord<T>(DataRow rowSource)
        where T : IHxSetValue, new()
        {
            T Result = new T();
            try
            {
                if (rowSource != null)
                {
                    //Result = new T();
                    Result.SetValue(rowSource);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        /// <summary>
        /// DataRow를 Struct(Record) Nullable Type으로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="rowSource">Sorce DataRow</param>
        /// <returns>Single Struct(Record) : Nullable</returns>
        public static T ConvertDataRowToNullableRecord<T>(DataRow rowSource)
        where T : IHxSetValue, new()//struct, IHxStructSetValue
        {
            T Result = default;
            try
            {
                if (rowSource != null)
                {
                    Result = new T();
                    Result?.SetValue(rowSource);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return default;
                throw ex;
            }
            return Result;
        }
        //===========================

        /// <summary>
        /// Struct타입을 DataTable로 구조로 변경(DataRow 미포함)
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="tableName">(optional)Table Name</param>
        /// <returns>DataTable Not With Data</returns>
        public static DataTable ConvertStructToDataTableNoData<T>(string tableName = null, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue//struct, IHxStructSetValue
        {
            DataTable Result = new DataTable();
            if (!tableName.IsNullOrWhiteSpaceEx())
            {
                Result.TableName = tableName;
            }
            try
            {
                T record = default;
                List<PropertyInfo> propList = HxUtils.PropertyInfoList(record);
                foreach (PropertyInfo prop in propList)
                {
                    List<HxAttribute> attrList = HxUtils.AttributeList<HxAttribute>(prop);
                    if (attrList != null && attrList.Count > 0)
                    {
                        foreach (HxAttribute attr in attrList)
                        {
                            if (!HxString.IsNullOrWhiteSpace(attr.ColumnName))
                            {
                                //this._userFieldList.AddEx(attr.ColumnName);
                                string strColName = HxString.IsNullOrWhiteSpace(attr.ColumnName) ? prop.Name : attr.ColumnName;
                                if (strColName.IsNullOrWhiteSpaceEx() != true)
                                {
                                    switch (colNameCharType)
                                    {
                                        case HxDbColumnNameCharType.Upper:
                                            strColName = strColName.ToUpper();
                                            break;
                                        case HxDbColumnNameCharType.Lower:
                                            strColName = strColName.ToLower();
                                            break;
                                    }
                                }
                                DataColumn col = null;
                                if (Result.Columns.Contains(strColName))
                                {
                                    col = Result.Columns[strColName];
                                }
                                else
                                {
                                    col = new DataColumn(strColName);
                                }
                                //col.ColumnName = attr.ColumnName.IsNullOrWhiteSpaceEx() ? prop.Name : attr.ColumnName;
                                col.Caption = attr.Description ?? prop.Name;
                                col.AutoIncrement = attr.AutoIncrement;
                                col.ReadOnly = attr.IsReadOnly;
                                col.AllowDBNull = !attr.IsNotNull;
                                col.Unique = attr.IsUnique;

                                //if (attr.DefaultDataType != null)
                                //{
                                //    col.DataType = attr.DefaultDataType;
                                //}
                                if (attr.DefaultValue != null)
                                {
                                    col.DefaultValue = attr.DefaultValue;
                                }

                                if (!col.ExtendedProperties.ContainsKey("TableName"))
                                {
                                    col.ExtendedProperties.Add("TableName", attr.TableName);
                                }
                                else
                                {
                                    col.ExtendedProperties["TableName"] = attr.TableName;
                                }

                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "TableName", attr.TableName);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ColumnName", attr.ColumnName);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "Remark", attr.Remark);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ExtraInfo", attr.ExtraInfo);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ExtraGridHidden", attr.ExtraGridHidden);

                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ValueCryptType", attr.ValueCryptType);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "DefaultValue", attr.DefaultValue);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "Name", prop.Name);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "MemberType", prop.MemberType);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "PropertyType", prop.PropertyType);

                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "DefaultDataType", attr.DefaultDataType);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "DefaultFormatString", attr.DefaultFormatString);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "CodeKeyValueSet", attr.CodeKeyValueSet);


                                Result.Columns.Add(col);
                            }
                            if (attr.ValueCryptType == HxCryptType.Hidden)
                            {
                                if (!HxString.IsNullOrWhiteSpace(attr.ColumnName))
                                {
                                    //this._userHidenFieldList.AddEx(attr.ColumnName);
                                }
                            }
                        }
                    }
                    else
                    {
                        string strColName = prop.Name;
                        DataColumn col = null;
                        if (Result.Columns.Contains(strColName))
                        {
                            col = Result.Columns[strColName];
                            string strName = col.ExtendedProperties["ColumnName"].ToStringEx();
                            if (!HxString.IsNullOrWhiteSpace(strName))
                            {
                                col = null;
                            }
                        }
                        else
                        {
                            col = new DataColumn(strColName);
                        }
                        if (col != null)
                        {
                            col.Caption = prop.Name;
                            //col.ReadOnly = ;
                            HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "Name", prop.Name);
                            HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "MemberType", prop.MemberType);
                            HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "PropertyType", prop.PropertyType);
                            Result.Columns.Add(col);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Result;
        }


        /// <summary>
        /// Struct(Record)의 Property 타입을 DataTable로 구조로 변경(Single DataRow 옵션)
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="record">Source : Record</param>
        /// <param name="insertRow">값 포함 여부</param>
        /// <returns>DataTable (With Single Data Option)</returns>
        public static DataTable ConvertStructToDataTable<T>(T record, bool insertRow = true)
            where T : struct
        {
            return ConvertStructToDataTable<T>(record, insertRow, null);
        }
        
        
        /// <summary>
        /// Struct(Record)의 Property 타입을 DataTable로 구조로 변경(Single DataRow 옵션)
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="record">Source : Record</param>
        /// <param name="insertRow">값 포함 여부</param>
        /// <param name="tableName">TableName</param>
        /// <returns>DataTable (With Single Data Option)</returns>
        public static DataTable ConvertStructToDataTable<T>(T record, bool insertRow, string tableName = null)
            where T : struct
        {
            //DataTable Result = this.ConvertStructToDataTable<T>(tableName);
            //if (Result != null && Result.Columns.Count > 0)
            //{
            //    DataRow row = Result.NewRow();
            //    foreach (DataColumn col in Result.Columns)
            //    {
            //        string colName = col.ColumnName;
            //        //if (dnUtils.Instance.isPropertyInfo(record, colName))
            //        //{
            //        //    object val = dnUtils.Instance.GetPropertyInfoValue(record, colName);
            //        //    row[colName] = val;
            //        //}

            //    }
            //    Result.Rows.Add(row);
            //}
            //return Result;

            DataTable Result = new DataTable();
            if (!HxString.IsNullOrWhiteSpace(tableName))
            {
                Result.TableName = tableName;
            }
            try
            {
                List<PropertyInfo> propList = HxUtils.PropertyInfoList(record);
                Dictionary<string, object> values = null;
                if (insertRow == true)
                {
                    values = new Dictionary<string, object>();
                }
                foreach (PropertyInfo prop in propList)
                {
                    List<HxAttribute> attrList = HxUtils.AttributeList<HxAttribute>(prop);
                    if (attrList != null && attrList.Count > 0)
                    {
                        foreach (HxAttribute attr in attrList)
                        {
                            if (!HxString.IsNullOrWhiteSpace(attr.ColumnName))
                            {
                                //this._userFieldList.AddEx(attr.ColumnName);
                                string strColName = HxString.IsNullOrWhiteSpace(attr.ColumnName) ? prop.Name : attr.ColumnName;

                                DataColumn col = null;
                                if (Result.Columns.Contains(strColName))
                                {
                                    col = Result.Columns[strColName];
                                }
                                else
                                {
                                    col = new DataColumn(strColName);
                                }
                                //col.ColumnName = attr.ColumnName.IsNullOrWhiteSpaceEx() ? prop.Name : attr.ColumnName;
                                col.Caption = attr.Description ?? prop.Name;
                                col.AutoIncrement = attr.AutoIncrement;
                                col.ReadOnly = attr.IsReadOnly;
                                col.AllowDBNull = !attr.IsNotNull;
                                col.Unique = attr.IsUnique;
                                if (insertRow == true && values != null)
                                {
                                    values.Add(col.ColumnName, HxUtils.PropertyInfoValue(record, prop.Name));
                                }

                                //if (attr.DefaultDataType != null)
                                //{
                                //    col.DataType = attr.DefaultDataType;
                                //}
                                if (attr.DefaultValue != null)
                                {
                                    col.DefaultValue = attr.DefaultValue;
                                }

                                if (!col.ExtendedProperties.ContainsKey("TableName"))
                                {
                                    col.ExtendedProperties.Add("TableName", attr.TableName);
                                }
                                else
                                {
                                    col.ExtendedProperties["TableName"] = attr.TableName;
                                }

                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "TableName", attr.TableName);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ColumnName", attr.ColumnName);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "Remark", attr.Remark);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ExtraInfo", attr.ExtraInfo);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ExtraGridHidden", attr.ExtraGridHidden);

                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "ValueCryptType", attr.ValueCryptType);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "DefaultValue", attr.DefaultValue);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "Name", prop.Name);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "MemberType", prop.MemberType);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "PropertyType", prop.PropertyType);

                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "DefaultDataType", attr.DefaultDataType);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "DefaultFormatString", attr.DefaultFormatString);
                                HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "CodeKeyValueSet", attr.CodeKeyValueSet);


                                Result.Columns.Add(col);
                            }
                            if (attr.ValueCryptType == HxCryptType.Hidden)
                            {
                                if (!HxString.IsNullOrWhiteSpace(attr.ColumnName))
                                {
                                    //this._userHidenFieldList.AddEx(attr.ColumnName);
                                }
                            }
                        }
                    }
                    else
                    {
                        string strColName = prop.Name;
                        DataColumn col = null;
                        if (Result.Columns.Contains(strColName))
                        {
                            col = Result.Columns[strColName];
                            string strName = col.ExtendedProperties["ColumnName"].ToStringEx();
                            if (!HxString.IsNullOrWhiteSpace(strName))
                            {
                                col = null;
                            }
                        }
                        else
                        {
                            col = new DataColumn(strColName);
                        }
                        if (col != null)
                        {
                            col.Caption = prop.Name;
                            //col.ReadOnly = ;
                            HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "Name", prop.Name);
                            HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "MemberType", prop.MemberType);
                            HxUtils.DoExtendedPropertiesAdd(col.ExtendedProperties, "PropertyType", prop.PropertyType);
                            Result.Columns.Add(col);
                            if (insertRow == true && values != null)
                            {
                                values.Add(col.ColumnName, HxUtils.PropertyInfoValue(record, prop.Name));
                            }
                        }
                    }
                }
                if (insertRow == true && values != null && values.Count > 0)
                {
                    DataRow row = Result.NewRow();
                    foreach (KeyValuePair<string, object> val in values)
                    {
                        if (Result.Columns.Contains(val.Key))
                        {
                            row[val.Key] = val.Value;
                        }
                    }
                    Result.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Result;
        }

        public static DataTable ConvertStructToDataTable<T>(T[] records, bool insertRow = true)
            where T : struct
        {
            return ConvertStructPropertiesToDataTable(records, insertRow, null);
        }

        /// <summary>
        /// Struct Array(RecordSet)의 Properties 타입을 DataTable로 구조로 변경(Multiple DataRow 옵션)
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="recordSet">Source : Struct Array</param>
        /// <param name="insertRow">값 포함 여부</param>
        /// <param name="tableName">TableName</param>
        /// <returns>DataTable (With Multiple Data Option)</returns>
        public static DataTable ConvertStructPropertiesToDataTable<T>(T[] recordSet, bool insertRow = true, string tableName = null)
            where T : struct
        {
            //DataTable Result = this.ConvertStructToDataTable<T>(tableName);
            //if (Result != null && Result.Columns.Count > 0)
            //{
            //    DataRow row = Result.NewRow();
            //    foreach (DataColumn col in Result.Columns)
            //    {
            //        string colName = col.ColumnName;
            //        //if (dnUtils.Instance.isPropertyInfo(record, colName))
            //        //{
            //        //    object val = dnUtils.Instance.GetPropertyInfoValue(record, colName);
            //        //    row[colName] = val;
            //        //}

            //    }
            //    Result.Rows.Add(row);
            //}
            //return Result;

            DataTable Result = null;

            if (recordSet != null && recordSet.Length > 0)
            {
                foreach (T record in recordSet)
                {
                    if (Result == null)
                    {
                        Result = ConvertStructToDataTable<T>(record, insertRow, tableName);
                    }
                    else
                    {
                        DataTable dt = ConvertStructToDataTable<T>(record, insertRow, tableName);
                        if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
                        {
                            Result.Merge(dt);
                        }
                    }
                }
            }
            if (!HxString.IsNullOrWhiteSpace(tableName))
            {
                Result.TableName = tableName;
            }
            return Result;
        }
        /// <summary>
        /// Struct(Record)의 Properties 타입을 DataTable로 구조로 변경(Single DataRow 옵션)
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="recordSet">Source : Struct Array</param>
        /// <param name="insertRow">값 포함 여부</param>
        /// <param name="tableName">TableName</param>
        /// <returns>DataTable (With Single Data Option)</returns>
        public static DataTable ConvertStructPropertiesToDataTable<T>(T record, bool insertRow = true, string tableName = null)
            where T : struct
        {
            return ConvertStructPropertiesToDataTable<T>(new T[] { record }, insertRow, tableName);
        }
        /// <summary>
        /// Source DataTable 구조에 값 복사하기(넣기)
        /// </summary>
        /// <param name="structSource">구조가 되는 DataTable</param>
        /// <param name="dataSource">값 복사대상이 되는 DataTable</param>
        /// <returns>값이 Copy된 DataTable</returns>
        public static DataTable CopyStructDataTable(DataTable structSource, DataTable dataSource)
        {
            DataTable Result = null;
            if (structSource != null && structSource.Columns.Count > 0)
            {
                Result = structSource.Clone();
            }

            if ((structSource == null || structSource.Columns.Count == 0) && dataSource != null)
            {
                Result = dataSource;
            }
            else
            {
                foreach (DataRow srcDr in dataSource.Rows)
                {
                    DataRow resultDr = Result.NewRow();
                    bool isFlag = false;
                    foreach (DataColumn resultDc in Result.Columns)
                    {
                        if (resultDc.ExtendedProperties["ColumnName"] != null)
                        {
                            string strExtColName = resultDc.ExtendedProperties["ColumnName"].ToString();
                            if (!HxString.IsNullOrWhiteSpace(strExtColName) && dataSource.Columns.Contains(strExtColName))
                            {
                                resultDr[resultDc.ColumnName] = srcDr[strExtColName];
                                isFlag = true;
                            }
                        }

                        //dr[srcCol
                    }
                    if (isFlag == true)
                    {
                        Result.Rows.Add(resultDr);
                    }
                }
            }
            return Result;
        }

        public static Dictionary<TKey, TVal> CovertRecrodToDictionary<T, TKey, TVal>(T record, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
          where T : IHxSetValue//struct
        {
            return HxConvert.CovertRecrodToDictionary<T, TKey, TVal>(record, colNameCharType);
        }
        //=============================

        [Obsolete("아직 쓰지마...미구현이여~")]
        private static T ConvertDataRowToRecordNoneFinal<T>(DataRow row)
            where T : struct
        {
            T Result = default;
            //record = new TimesheetRec();
            //try
            //{
            if (row != null && row.Table != null && row.Table.Columns.Count > 0)
            {
                Result = new T();
                DataTable dt = row.Table;
                foreach (DataColumn dc in dt.Columns)
                {
                    string colName = dc.ColumnName;
                    List<PropertyInfo> propList = HxUtils.PropertyInfoList(Result);
                    foreach (PropertyInfo prop in propList)
                    {
                        string strColName = prop.Name;
                        List<HxAttribute> attrList = HxUtils.AttributeList<HxAttribute>(prop);
                        if (attrList != null && attrList.Count > 0)
                        {
                            foreach (HxAttribute attr in attrList)
                            {
                                strColName = HxString.IsNullOrWhiteSpace(attr.ColumnName) ? prop.Name : attr.ColumnName;
                                if (colName.ToLower() == strColName.ToLower())
                                {
                                    //Type type = prop.DeclaringType;
                                    object value = row[colName];
                                    //string strType = prop.PropertyType.FullName;
                                    //string strValueType = value.GetType().ToString();
                                    try
                                    {
                                        bool b = HxUtils.PropertyInfoValueSet<T>(ref Result, prop.Name, value);
                                        //bool b = HxUtils.SetPropertyInfoValue<T>(ref Result, prop.Name, value);
                                        //b = HxUtils.SetPropertyInfoValue(prop, prop.Name, value);
                                        if (!b)
                                        {
                                            //prop.SetValue(Result, value);
                                            Debug.WriteLine(string.Format("{0} : {1}", strColName, value));
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex);
                                        throw ex;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    throw ex;
            //}

            return Result;
        }


        /// <summary>
        /// Converts an Object Array to a System.Data.DataTable using Reflection.
        /// </summary>
        /// <param name="inArray">Object array to be converted</param>
        /// <returns></returns>
        private static DataTable ConvertArrayToDataTable(object[] inArray)
        //출처 : https://forums.asp.net/t/1639567.aspx?Convert+array+of+objects+to+DataTable
        {
            DataTable dt = new DataTable();


            if (inArray.Length == 0)
                return new DataTable();
            //initialize a new type of our inArray type
            Type type = inArray[0].GetType();


            //extract all our properties (public & static) from our type (generic)
            PropertyInfo[] proInfo = type.GetProperties();


            //create the columns for each property setting the name & type (generic)
            foreach (PropertyInfo i in proInfo)
                dt.Columns.Add(i.Name, i.PropertyType);


            //loop through each object in the array
            foreach (object o in inArray)
            {
                //create a new datarow
                DataRow r = dt.NewRow();
                //loop through each property in order and set our row columns to the value of the property
                for (int i = 0; i < proInfo.Length; i++)
                    r[i] = o.GetType().InvokeMember(proInfo[i].Name, BindingFlags.GetProperty, null, o, null);


                //add the row to our table
                dt.Rows.Add(r);
            }
            return dt;
        }
        /// <summary>
        /// Converts an object Array to a DataTable using XML Serialization.
        /// </summary>
        /// <remarks>Not recommended for use. Use Reflection method</remarks>
        /// <param name="inArray">object Array to be converted</param>
        /// <returns></returns>
        public static DataTable ConvertArrayToDataTableXML(object[] inArray)
        {
            Type type = inArray.GetType();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
            System.IO.StringWriter sw = new System.IO.StringWriter();
            serializer.Serialize(sw, inArray);
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            System.IO.StringReader reader = new System.IO.StringReader(sw.ToString());


            ds.ReadXml(reader);
            dt = ds.Tables[0];
            return dt;
        }

        public static DataTable ConvertObjectPropertiesToDataTable<T>(HxDbColumnNameCharType nameCharType = HxDbColumnNameCharType.Lower)
        //where T : IHxStructSetValue
        {
            return HxConvert.ConvertObjectPropertiesToDataTable<T>(nameCharType);
        }

        public static string GetParamTypeName(string inputColName, HxDbParamValueType paramOptionType = HxDbParamValueType.Default)
        {
            string Result = null;
            if (inputColName.IsNullOrWhiteSpaceEx() == true) return Result;

            switch (paramOptionType)
            {
                case HxDbParamValueType.CLOB:
                    Result = $"{_PREFIX_PARAM_CLOB_}{inputColName}";
                    break;
                case HxDbParamValueType.BLOB:
                    Result = $"{_PREFIX_PARAM_BLOB_}{inputColName}";
                    break;
                case HxDbParamValueType.Default:
                //case HxDbParamValueType.Input:
                //case HxDbParamValueType.Param:
                case HxDbParamValueType.Date:
                case HxDbParamValueType.Time:
                case HxDbParamValueType.DateTime:
                case HxDbParamValueType.UnixTime:
                default:
                    Result = inputColName;
                    break;
            }
            return Result;
        }

        public static HxResultValue SetArrangeDbInsertKeyValuePairsType(string inputColName, object inputObjValue, StringBuilder builderCols, StringBuilder builderVals, Dictionary<string, object> pairsBind, bool? IsNotParamBindAndCustomValueWithQuotesUse = null, HxDbParamValueType paramValueType = HxDbParamValueType.Default, bool? bNotUseValueIsNull = null)
        {
            HxResultValue Result = null;
            if (inputColName.IsNullOrWhiteSpaceEx() != true && builderCols != null && builderVals != null && pairsBind != null)
            {

                Result = new HxResultValue();

                if (bNotUseValueIsNull == true && inputObjValue.IsNullOrWhiteSpaceEx() == true) return Result;

                try
                {
                    if (builderCols.Length > 0)
                    {
                        builderCols.AppendLine(",");
                        builderVals.AppendLine(",");
                    }
                    builderCols.AppendFormat(" {0}", inputColName);
                    if (IsNotParamBindAndCustomValueWithQuotesUse == null)
                    {
                        string bindColName = GetParamTypeName(inputColName, paramValueType);
                        builderVals.AppendFormat(":insert{0}", bindColName);
                        pairsBind.AddEx($"insert{bindColName}", inputObjValue, true);
                    }
                    else if (IsNotParamBindAndCustomValueWithQuotesUse == false)
                    {
                        builderVals.AppendFormat(" {0}", inputColName);
                    }
                    else if (IsNotParamBindAndCustomValueWithQuotesUse == true)
                    {
                        builderVals.AppendFormat(" '{0}'", inputColName);
                    }

                    Result.Value = true;
                }
                catch (Exception ex)
                {
                    Result.SetException(ex);
                    //Result = false;
                    Debug.WriteLine(ex);
                    //throw ex;
                }
            }
            return Result;
        }

        public static HxResultValue SetArrangeDbInsertDateTimeType(string inputColName, DateTime? inputDateValue, StringBuilder builderCols, StringBuilder builderVals, Dictionary<string, object> pairsBind, HxDbProviderType providerType = HxDbProviderType.Oracle)
        {
            HxResultValue Result = null;
            if (inputColName.IsNullOrWhiteSpaceEx() != true)
            {
                Result = new HxResultValue();
                if (builderCols == null)
                {
                    builderCols = new StringBuilder();
                }
                if (builderVals == null)
                {
                    builderVals = new StringBuilder();
                }
                if (pairsBind == null)
                {
                    pairsBind = new Dictionary<string, object>();
                }

                try
                {
                    //if(inputDateValue == null)
                    //{
                    //    inputDateValue = DateTime.Now;
                    //}
                    if (builderCols.Length > 0)
                    {
                        builderCols.AppendLine(",");
                        builderVals.AppendLine(",");
                    }
                    if (inputDateValue != null)
                    {
                        builderCols.Append($" {inputColName}");
                        builderVals.Append($" TO_DATE(:{inputColName}, '{_FORMAT_DATETIME_ORACLE_}')");
                        pairsBind.AddEx(inputColName, inputDateValue.ToNullableDateTimeStringEx(_FORMAT_DATETIME_Csharp_), true);
                    }
                    else
                    {
                        builderCols.Append($" {inputColName}");
                        builderVals.Append(" SYSDATE");
                    }
                    Result.Value = true;
                }
                catch (Exception ex)
                {
                    Result.SetException(ex);
                    Debug.WriteLine(ex);
                    //throw ex;
                }
            }
            return Result;
        }

        public static HxResultValue SetArrangeDbUpdateKeyValuePairsType(string inputColName, object inputObjValue, StringBuilder builderSQL, Dictionary<string, object> pairsBind)
        {
            HxResultValue Result = null;
            try
            {
                if (inputColName.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = new HxResultValue();
                    if (builderSQL == null)
                    {
                        builderSQL = new StringBuilder();
                    }
                    if (pairsBind == null)
                    {
                        pairsBind = new Dictionary<string, object>();
                    }
                    if (builderSQL.Length > 0)
                    {
                        builderSQL.AppendLine(", ");
                    }
                    builderSQL.AppendFormat(" {0} = :update_{0}", inputColName);
                    pairsBind.AddEx($"update_{inputColName}", inputObjValue, true);
                }
            }
            catch (Exception ex)
            {
                Result.SetException(ex);
                //Result = false;
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }

        public static HxResultValue SetArrangeDbUpdateDateTimeType(string inputColName, DateTime? inputDateValue, StringBuilder builderSQL, Dictionary<string, object> pairsBind, HxDbProviderType providerType = HxDbProviderType.Oracle)
        {
            HxResultValue Result = null;
            if (inputColName.IsNullOrWhiteSpaceEx() != true)
            {
                Result = new HxResultValue();
                if (builderSQL == null)
                {
                    builderSQL = new StringBuilder();
                }
                if (pairsBind == null)
                {
                    pairsBind = new Dictionary<string, object>();
                }

                try
                {
                    //if(inputDateValue == null)
                    //{
                    //    inputDateValue = DateTime.Now;
                    //}
                    if (builderSQL.Length > 0)
                    {
                        builderSQL.AppendLine(",");
                    }
                    if (inputDateValue != null)
                    {
                        builderSQL.Append($" {inputColName} = TO_DATE(:{inputColName}, '{_FORMAT_DATETIME_ORACLE_}')");
                        pairsBind.AddEx(inputColName, inputDateValue.ToNullableDateTimeStringEx(_FORMAT_DATETIME_Csharp_), true);
                    }
                    else
                    {
                        builderSQL.Append($" {inputColName} = SYSDATE");
                    }
                    Result.Value = true;
                }
                catch (Exception ex)
                {
                    Result.SetException(ex);
                    Debug.WriteLine(ex);
                    //throw ex;
                }
            }
            return Result;
        }
        public static decimal? GetCutomMajorAndMinorValueToDecimalValue(decimal? majorValue, decimal? minorValue = 0)
        {
            if (majorValue != null)
            {
                return (majorValue ?? 0).ToDecimalEx() + ((minorValue ?? 0).ToIntEx() * 0.1).ToDecimalEx();
            }
            return null;
        }

        
    }
}
