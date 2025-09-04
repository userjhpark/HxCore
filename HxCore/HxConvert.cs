using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace HxCore
{
    /// <summary>
    ///  기본 변수 타입은 .NET의 Conver 이용 추천, 사용자 정의 타입변환 추천 Custom Class
    /// </summary>
    public class HxConvert
    {

        #region 변수 Convert
        /// <summary>
        /// System.Object를 변환될 타입별로 반환(Generic 타입)
        /// </summary>
        /// <typeparam name="T">리턴 Type</typeparam>
        /// <param name="value">Object</param>
        /// <returns>Generic Type</returns>
        public static T ToConvert<T>(object value)
        {
            Type type = typeof(T);
            T Result = default;
            try
            {
                if(value == DBNull.Value)
                {
                    value = null;
                }
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (value == null)
                    {
                        Result = default;
                    }
                    else
                    {
                        Type valueType = type.GetGenericArguments()[0];
                        Result = (T)Convert.ChangeType(value, valueType);
                    }
                }
                else if (value != null)
                {
                    Result = (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch (Exception ex)
            {
                Result = default;
                //throw;
                Debug.WriteLine(ex);
            }
            return Result;
        }

        /// <summary>
        /// System.Object를 System.String로 반환 (Null일 경우 Null)
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>String</returns>
        public static string ToString(object value, IFormatProvider formatProv = null)
        {
            if (value != null && value is Byte[])
            {
                string Result = null;
                try
                {

                    //Byte[] b = value as Byte[];
                    if (value is Byte[] b)
                    {
                        Result = Encoding.Default.GetString(b);
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                    //Result = "Error : " + ex.Message;
                }
                return Result;
            }
            else
            {
                if (value is DateTime)
                {
                    if (value == null)
                    {
                        //return DateTime.MinValue.ToString(formatProv);
                        return (new DateTime(1900, 1, 1)).ToString(formatProv);
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
                else
                {
                    return (value?.ToString());
                }
            }
        }
        /// <summary>
        /// DateTime을 지정된 String Format으로 반환(Null일 경우 DateTime(1900, 1, 1);)
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">DateTime String Format</param>
        /// <returns>String DateTime</returns>
        public static string ToString(DateTime value, string dateFormat = "yyyy-MM-dd")
        {
            string Result = string.Empty;
            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
            try
            {
                Result = value.ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                //Result = DateTime.MinValue.ToString(dateFormat);
                Debug.WriteLine(ex.Message.ToString());
                //throw ex;
                throw new HxException(ex.Message.ToString(), ex);
            }

            return Result;
        }

        /// <summary>
        /// System.Object를 Bool로 반환 (Null일 경우 false)
        /// </summary>
        /// <param name="value">Object Value</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>Int</returns>
        public static bool ToBool(object value, bool defaultValue = false)
        {
            bool Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = bool.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }

        /// <summary>
        /// String 값을 Bool로 반환 (Null일 경우 default값)
        /// </summary>
        /// <param name="value">String Value</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>Int</returns>
        public static bool ToBool(string value, bool defaultValue = false)
        {
            bool Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    string inputValue = value.ToStringEx().ToUpper();
                    switch (inputValue)
                    {
                        case "1":
                        case "Y":
                        case "TRUE":
                            Result = true;
                            break;
                        case "0":
                        case "N":
                        case "FALSE":
                            Result = false;
                            break;
                        case "-1":
                        case "NULL":
                        case "DEFAULT":
                        case "NONE":
                            Result = defaultValue;
                            break;
                        default:
                            bool bConvert = bool.TryParse(value.ToStringEx(), out Result);
                            if (!bConvert)
                            {
                                Result = defaultValue;
                            }
                            break;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }

        public static bool? ToNullableBool(object value, bool? defaultValue = null)
        {
            bool? Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    Result = value.ToBoolEx();
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        public static bool? ToNullableBool(string value, bool? defaultValue = null)
        {
            bool? Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    Result = value.ToBoolEx();
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }


        /// <summary>
        /// System.Object를 int/Int32로 반환 (Null일 경우 int.MinValue)
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>Int</returns>
        public static int ToInt(object value, int defaultValue = int.MinValue)
        {
            int Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = int.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        /// <summary>
        /// System.Object를 short/Int16로 반환 (Null일 경우 Int16.MinValue)
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>Int16</returns>
        public static short ToInt16(object value, short defaultValue = short.MinValue)
        {
            short Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = short.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }

        /// <summary>
        /// System.Object를 long/Int64로 반환 (Null일 경우 long.MinValue)
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>long</returns>
        public static long ToInt64(object value, long defaultValue = long.MinValue)
        {
            long Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = long.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        public static uint ToUInt(object value, uint defaultValue = uint.MinValue)
        {
            uint Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = uint.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        public static int? ToInt(object value, int? defaultValue = null)
        {
            int? Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = int.TryParse(value.ToStringEx(), out int conVal);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    else
                    {
                        Result = conVal;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }

        public static int ToPlusInt(int value, int defaultValue = int.MinValue)
        {
            int Result = defaultValue;
            try
            {
                Result = value >= 0 ? value : (value * -1);
            }
            catch (Exception ex)
            {
                Result = defaultValue;
                Debug.WriteLine(ex);
                //throw ex;
            }
            
            return Result;
        }

        /// <summary>
        /// System.Object를 Decimal4로 반환 (Null일 경우 decimal.MinValue)
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>Decimal</returns>
        public static decimal ToDecimal(object value, decimal defaultValue = int.MinValue)
        {
            decimal Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = decimal.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }

        public static decimal? ToDecimal(object value, decimal? defaultValue = null)
        {
            decimal? Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = decimal.TryParse(value.ToStringEx(), out decimal conVal);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    else
                    {
                        Result = conVal;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        /// <summary>
        /// System.Object를 Double로 반환 (Null일 경우 decimal.MinValue)
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>Decimal</returns>
        public static double ToDecimal(object value, double defaultValue = int.MinValue)
        {
            double Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = double.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        /// <summary>
        /// System.Object를 Float로 반환 (Null일 경우 float.MinValue)
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>Decimal</returns>
        public static float ToFloat(object value, float defaultValue = int.MinValue)
        {
            float Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = float.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        public static long ToLong(object value, long defaultValue = int.MinValue)
        {
            long Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = long.TryParse(value.ToStringEx().RegexReplaceEx(",", string.Empty), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        public static long? ToLong(object value, long? defaultValue = null)
        {
            long? Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = long.TryParse(value.ToStringEx(), out long conVal);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    else
                    {
                        Result = conVal;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }

        public static ulong ToULong(object value, ulong defaultValue = uint.MinValue)
        {
            ulong Result = defaultValue;
            try
            {
                if (value == null)
                {
                    Result = defaultValue;
                }
                else
                {
                    bool bConvert = ulong.TryParse(value.ToStringEx(), out Result);
                    if (!bConvert)
                    {
                        Result = defaultValue;
                    }
                    //Result = (value == null ? Int32.MinValue : Convert.ToInt32(value));
                }

            }
            catch (Exception)
            {
                Result = defaultValue;
                //throw;
            }
            return Result;
        }
        /// <summary>
        /// System.Object를 변환될 타입별로 반환(Generic 타입)
        /// </summary>
        /// <typeparam name="T">리턴 Type</typeparam>
        /// <param name="sender">Object</param>
        /// <returns>Generic Type</returns>
        public static T ConvertTo<T>(object sender)
        {
            return ToConvert<T>(sender);
        }
        #endregion

        #region Struct-DataTable Convert

        //================================
        /// <summary>
        /// DataTable의 특정Index를 Struct(Record)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="dataSource">Source DataTable</param>
        /// <param name="index">Index</param>
        /// <returns>Single Struct(Record)</returns>
        public static T ConvertDataTableToRecord<T>(DataTable dataSource, int index = 0)
            where T : IHxSetValue, new()//struct, IHxStructSetValue
        {
            return HxUtils.ConvertDataTableToRecord<T>(dataSource, index);
        }
        /// <summary>
        /// DataTable의 특정Index를 Struct(Record) Nullable Type으로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="dataSource">Source DataTable</param>
        /// <param name="index">Index</param>
        /// <returns>Single Struct(Record) : Nullable Type</returns>
        public static T ConvertDataTableToNullableRecord<T>(DataTable dataSource, int index = 0)
            where T : IHxSetValue, new()//struct, IHxStructSetValue
        {
            return HxUtils.ConvertDataTableToNullableRecord<T>(dataSource, index);
        }
        /// <summary>
        /// DataTable을 Struct Array(RecordSet)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="dataSource">Source DataTable</param>
        /// <returns>Multiple Struct Array(RecordSet)</returns>
        public static T[] ConvertDataTableToRecordSet<T>(DataTable dataSource)
            where T : IHxSetValue, new()//struct, IHxStructSetValue
        {
            return HxUtils.ConvertDataTableToRecordSet<T>(dataSource);
        }
        /// <summary>
        /// DataRow를 Struct(Record)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="rowSource">Sorce DataRow</param>
        /// <returns>Single Struct(Record)</returns>
        public static T ConvertDataRowToRecord<T>(DataRow rowSource)
        where T : IHxSetValue, new()//struct, IHxStructSetValue
        {
            return HxUtils.ConvertDataRowToRecord<T>(rowSource);
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
            return HxUtils.ConvertDataRowToNullableRecord<T>(rowSource);
        }
        
        //===========================

        /// <summary>
        /// Struct타입을 DataTable로 구조로 변경(DataRow 미포함)
        /// </summary>
        /// <typeparam name="T">Source Struct Type</typeparam>
        /// <param name="tableName">(optional)Table Name</param>
        /// <returns>DataTable Not With Data</returns>
        public static DataTable ConvertStructToDataTableNoData<T>(string tableName = null, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue//struct
        {
            return HxUtils.ConvertStructToDataTableNoData<T>(tableName, colNameCharType);
        }
        /// <summary>
        /// Struct(Record)타입을 DataTable로 구조로 변경(Single DataRow 옵션)
        /// </summary>
        /// <typeparam name="T">Source Struct Type</typeparam>
        /// <param name="record">Record</param>
        /// <returns>DataTable (Single Data)</returns>
        public static DataTable ConvertRecordToDataTable<T>(T record, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue//struct
        {
            return ConvertRecordToDataTable<T>(record, null, true, colNameCharType);
        }

        public static Dictionary<TKey, TVal> CovertRecrodToDictionary<T, TKey, TVal>(T record, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
           where T : IHxSetValue//struct
        {
            DataTable dt = ConvertRecordToDataTable<T>(record, colNameCharType);
            if(dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Dictionary<TKey, TVal> Result = new Dictionary<TKey, TVal>();
                foreach(DataColumn dc in dt.Columns)
                {
                    TKey colName = dc.ColumnName.ToConvertEx<TKey>();
                    TVal value = row[dc.ColumnName].ToConvertEx<TVal>();
                    Result.AddEx(colName, value);
                }
                return Result;
            }
            return null;
        }

        public static DataTable ConvertRecordToDataTable<T>(T record, string tableName, bool insertRow = true, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue
        {
            //bool insertRow = false;
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
                int index = 0;
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

                                switch (colNameCharType)
                                {
                                    case HxDbColumnNameCharType.Lower:
                                        strColName = strColName.ToLower();
                                        break;
                                    case HxDbColumnNameCharType.Upper:
                                        strColName = strColName.ToUpper();
                                        break;
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
                                    //_userHidenFieldList.AddEx(attr.ColumnName);
                                }
                            }
                        }
                    }
                    else
                    {
                        
                        string strColName = prop.Name;
                        switch (colNameCharType)
                        {
                            case HxDbColumnNameCharType.Lower:
                                strColName = strColName.ToLower();
                                break;
                            case HxDbColumnNameCharType.Upper:
                                strColName = strColName.ToUpper();
                                break;
                        }
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
                        if(values != null)
                        {
                            var value = prop.GetValue(record, null);
                            values.Add(strColName, value);
                        }
                        index++;
                    }
                }
                Debug.WriteLine(index);
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

        public static DataTable ConvertRecordSetToDataTable<T>(IEnumerable<T> recordSet, string tableName = null, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue, new()//struct
        {
            DataTable Result = null;
            try
            {
                if (recordSet != null)
                {
                    int n = recordSet.Count();
                    if (n == 1)
                    {
                        Result = ConvertRecordToDataTable<T>(recordSet.First(), tableName, true, colNameCharType);
                    }
                    else if (n > 1)
                    {
                        Result = null;
                        //DataTable dt1 = ConvertStructToDataTable<T>(recordSet[0], tableName);
                        int i = 0;
                        foreach(T record in recordSet)
                        {
                            DataTable dt = ConvertRecordToDataTable<T>(record, tableName, true, colNameCharType);
                            if (Result == null)
                            {
                                Result = dt.Copy();
                            }
                            else
                            {
                                Result.Merge(dt);
                            }
                            i++;
                        }
                    }
                    else
                    {
                        Result = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex);
                //throw ex;
            }
            
            return Result;
        }

        public static DataTable ConvertLinqToDataTable<T>(IEnumerable<T> recordSet, string tableName = null, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
        {
            DataTable Result = null;
            try
            {
                if(recordSet != null)
                {
                    FieldInfo[] currFieldInfo;
                    Type currType = typeof(T);
                    // Get the type and fields of FieldInfoClass.
                    currFieldInfo = currType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                    if(currFieldInfo.Count() > 0)
                    {
                        Result = new DataTable();
                        for (int i = 0; i < currFieldInfo.Length; i++)
                        {
                            FieldInfo fld = currFieldInfo[i];
                            string colName = fld.Name;
                            switch (colNameCharType)
                            {
                                case HxDbColumnNameCharType.Lower:
                                    colName = colName.ToLower();
                                    break;
                                case HxDbColumnNameCharType.Upper:
                                    colName = colName.ToUpper();
                                    break;
                            }
                            if (!Result.Columns.Contains(colName))
                            {
                                //DataColumn dc = new DataColumn(colName, fld.FieldType);
                                Result.Columns.Add(colName);
                            }
                            //Console.WriteLine("\nName            : {0}", myFieldInfo[i].Name);
                            //Console.WriteLine("Declaring Type  : {0}", myFieldInfo[i].DeclaringType);
                            //Console.WriteLine("IsPublic        : {0}", myFieldInfo[i].IsPublic);
                            //Console.WriteLine("MemberType      : {0}", myFieldInfo[i].MemberType);
                            //Console.WriteLine("FieldType       : {0}", myFieldInfo[i].FieldType);
                            //Console.WriteLine("IsFamily        : {0}", myFieldInfo[i].IsFamily);
                        }
                    }
                    if(Result != null)
                    {
                        int nCol = Result.Columns.Count;
                        foreach(T record in recordSet)
                        {
                            DataRow row = Result.NewRow();
                            for (int i = 0; i < currFieldInfo.Length; i++)
                            {
                                string colName = currFieldInfo[i].Name;
                                switch (colNameCharType)
                                {
                                    case HxDbColumnNameCharType.Upper:
                                        colName = colName.ToUpper();
                                        break;
                                    case HxDbColumnNameCharType.Lower:
                                        colName = colName.ToLower();
                                        break;
                                    case HxDbColumnNameCharType.Original:
                                    default:
                                        break;
                                }

                                string fldName = currFieldInfo[i].Name;
                                if (Result.Columns.Contains(colName))
                                {
                                    FieldInfo fld = currType.GetField(fldName);
                                    row[colName] = fld.GetValue(record);
                                }
                            }
                            Result.Rows.Add(row);
                        }
                    }
                }
                if(Result != null && tableName.IsNullOrWhiteSpaceEx() != true)
                {
                    Result.TableName = tableName;
                    Result.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }

        //=============================
        public static DataTable ConvertDataTable<T>(IEnumerable<T> self = null, bool isJsonPropertyUse = true)
        {
            
            var properties = typeof(T).GetProperties();
            DataTable Result = new DataTable();
            try
            {
                foreach (var info in properties)
                {
                    string colName = info.Name;
                    if (isJsonPropertyUse == true)
                    {
                        var attr = info.GetCustomAttribute<JsonPropertyAttribute>();
                        if(attr != null && attr.PropertyName.IsNullOrWhiteSpaceEx() != true)
                        {
                            colName = attr.PropertyName;
                        }
                    }
                    Result.Columns.Add(colName, Nullable.GetUnderlyingType(info.PropertyType)
                       ?? info.PropertyType);
                }

                if (Result == null || (Result != null && Result.Columns.Count <= 0)) return null;

                if (Result != null && Result.Columns.Count > 0 && self != null && self.Any())
                {
                    foreach (var entity in self)
                        Result.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        /// <summary>
        /// Source DataTable 구조에 값 복사하기(넣기)
        /// </summary>
        /// <param name="structSource">구조가 되는 DataTable</param>
        /// <param name="dataSource">값 복사대상이 되는 DataTable</param>
        /// <returns>값이 Copy된 DataTable</returns>
        public static DataTable CopyStructDataTable(DataTable structSource, DataTable dataSource)
        {
            return HxUtils.CopyStructDataTable(structSource, dataSource);
        }
        public static DataTable ConvertClassToDataTable<T>(string tableName = null)
            where T : class, new()
        {
            DataTable Result = null;
            try
            {
                T record = new T();
                List<PropertyInfo> propList = HxUtils.PropertyInfoList(record);
                if (propList != null && propList.Count > 0)
                {
                    HxAttribute attrClass = record.GetType().GetCustomAttribute<HxAttribute>();
                    Result = new DataTable(attrClass?.TableName);
                    foreach (PropertyInfo prop in propList)
                    {
                        List<HxAttribute> attrList = HxUtils.AttributeList<HxAttribute>(prop);
                        if (attrList != null && attrList.Count > 0)
                        {
                            HxAttribute attr = attrList[attrList.Count - 1];

                            if (attr != null && attr.IsCustom != true)
                            {

                                string strPropName = prop.Name;
                                string strColName = prop.Name;
                                string strColCaption = prop.Name;
                                if (!attr.ColumnName.IsNullOrWhiteSpaceEx())
                                {
                                    strColName = attr.ColumnName;
                                    strColCaption = attr.Description.IsNullOrWhiteSpaceEx() ? strPropName : attr.Description;
                                }
                                DataColumn col = new DataColumn(strColName)
                                {
                                    DataType = HxHelper.GetDataTypeToType(attr.DataType)
                                };
                                if (attr.IsKey == true)
                                {
                                    //if (attr.IsUnique == null)
                                    //{
                                    //    col.Unique = true;
                                    //}
                                    if (attr.AutoIncrement == true && (col.DataType == typeof(object) || attr.DataType == HxDataType.Number || attr.DataType == HxDataType.Double))
                                    {
                                        col.AutoIncrement = true;
                                        col.AutoIncrementSeed = 1;
                                    }
                                    else
                                    {
                                        col.AutoIncrement = false;
                                    }
                                }

                                if (attr.IsUnique == true && col.Unique != true)
                                {
                                    col.Unique = true;
                                }
                                if (attr.AutoIncrement == true && col.AutoIncrement != true && (col.DataType == typeof(object) || attr.DataType == HxDataType.Number || attr.DataType == HxDataType.Double))
                                {
                                    col.AutoIncrement = true;
                                    col.AutoIncrementSeed = attr.AutoIncrementSeed;
                                }
                                col.Caption = strColCaption ?? prop.Name;
                                col.AllowDBNull = !attr.IsNotNull;
                                if (attr.DefaultValue != null)
                                {
                                    col.DefaultValue = attr.DefaultValue;
                                }


                                if (!col.ExtendedProperties.ContainsKey("TableName"))
                                {
                                    col.ExtendedProperties.Add("TableName", attr.TableName ?? attrClass.TableName);
                                }
                                else
                                {
                                    col.ExtendedProperties["TableName"] = attr.TableName ?? attrClass.TableName;
                                }

                                HxUtils.DoExtendedPropertiesAdd(col, "Remark", attr.Remark);
                                HxUtils.DoExtendedPropertiesAdd(col, "ExtraInfo", attr.ExtraInfo);
                                HxUtils.DoExtendedPropertiesAdd(col, "ExtraGridHidden", attr.ExtraGridHidden);

                                HxUtils.DoExtendedPropertiesAdd(col, "ValueCryptType", attr.ValueCryptType);
                                HxUtils.DoExtendedPropertiesAdd(col, "DefaultValue", attr.DefaultValue);
                                HxUtils.DoExtendedPropertiesAdd(col, "Name", prop.Name);
                                HxUtils.DoExtendedPropertiesAdd(col, "MemberType", prop.MemberType);
                                HxUtils.DoExtendedPropertiesAdd(col, "PropertyType", prop.PropertyType);

                                HxUtils.DoExtendedPropertiesAdd(col, "FormatString", attr.FormatString);
                                HxUtils.DoExtendedPropertiesAdd(col, "CodeKeyValueSet", attr.CodeKeyValueSet);

                                HxUtils.DoExtendedPropertiesAdd(col, "PropName", strPropName);

                                HxUtils.DoExtendedPropertiesAdd(col, "HxDataType", attr.DataType);

                                //dc.ExtendedProperties.Add("Max")
                                if (attr.MaximumLength > 0 && col.DataType == typeof(string))
                                {
                                    col.MaxLength = attr.MaximumLength;
                                }
                                Result.Columns.Add(col);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }
            if (Result != null && !HxString.IsNullOrWhiteSpace(tableName))
            {
                Result.TableName = tableName;
            }
            return Result;
        }
        public static DataTable ConvertStructToDataTable<T>(string tableName = null)
            where T : struct
        {
            DataTable Result = null;
            try
            {
                T record = new T();
                List<PropertyInfo> propList = HxUtils.PropertyInfoList(record);
                if (propList != null && propList.Count > 0)
                {
                    HxAttribute attrClass = record.GetType().GetCustomAttribute<HxAttribute>();
                    Result = new DataTable(attrClass?.TableName);
                    foreach (PropertyInfo prop in propList)
                    {
                        List<HxAttribute> attrList = HxUtils.AttributeList<HxAttribute>(prop);
                        if (attrList != null && attrList.Count > 0)
                        {
                            HxAttribute attr = attrList[attrList.Count - 1];

                            if (attr != null && attr.IsCustom != true)
                            {

                                string strPropName = prop.Name;
                                string strColName = prop.Name;
                                string strColCaption = prop.Name;
                                if (!attr.ColumnName.IsNullOrWhiteSpaceEx())
                                {
                                    strColName = attr.ColumnName;
                                    strColCaption = attr.Description.IsNullOrWhiteSpaceEx() ? strPropName : attr.Description;
                                }
                                DataColumn col = new DataColumn(strColName)
                                {
                                    DataType = HxHelper.GetDataTypeToType(attr.DataType)
                                };
                                if (attr.IsKey == true)
                                {
                                    //if (attr.IsUnique == null)
                                    //{
                                    //    col.Unique = true;
                                    //}
                                    if (attr.AutoIncrement == true && (col.DataType == typeof(object) || attr.DataType == HxDataType.Number || attr.DataType == HxDataType.Double))
                                    {
                                        col.AutoIncrement = true;
                                        col.AutoIncrementSeed = 1;
                                    }
                                    else
                                    {
                                        col.AutoIncrement = false;
                                    }
                                }

                                if (attr.IsUnique == true && col.Unique != true)
                                {
                                    col.Unique = true;
                                }
                                if (attr.AutoIncrement == true && col.AutoIncrement != true && (col.DataType == typeof(object) || attr.DataType == HxDataType.Number || attr.DataType == HxDataType.Double))
                                {
                                    col.AutoIncrement = true;
                                    col.AutoIncrementSeed = attr.AutoIncrementSeed;
                                }
                                col.Caption = strColCaption ?? prop.Name;
                                col.AllowDBNull = !attr.IsNotNull;
                                if (attr.DefaultValue != null)
                                {
                                    col.DefaultValue = attr.DefaultValue;
                                }


                                if (!col.ExtendedProperties.ContainsKey("TableName"))
                                {
                                    col.ExtendedProperties.Add("TableName", attr.TableName ?? attrClass.TableName);
                                }
                                else
                                {
                                    col.ExtendedProperties["TableName"] = attr.TableName ?? attrClass.TableName;
                                }

                                HxUtils.DoExtendedPropertiesAdd(col, "Remark", attr.Remark);
                                HxUtils.DoExtendedPropertiesAdd(col, "ExtraInfo", attr.ExtraInfo);
                                HxUtils.DoExtendedPropertiesAdd(col, "ExtraGridHidden", attr.ExtraGridHidden);

                                HxUtils.DoExtendedPropertiesAdd(col, "ValueCryptType", attr.ValueCryptType);
                                HxUtils.DoExtendedPropertiesAdd(col, "DefaultValue", attr.DefaultValue);
                                HxUtils.DoExtendedPropertiesAdd(col, "Name", prop.Name);
                                HxUtils.DoExtendedPropertiesAdd(col, "MemberType", prop.MemberType);
                                HxUtils.DoExtendedPropertiesAdd(col, "PropertyType", prop.PropertyType);

                                HxUtils.DoExtendedPropertiesAdd(col, "FormatString", attr.FormatString);
                                HxUtils.DoExtendedPropertiesAdd(col, "CodeKeyValueSet", attr.CodeKeyValueSet);

                                HxUtils.DoExtendedPropertiesAdd(col, "PropName", strPropName);

                                HxUtils.DoExtendedPropertiesAdd(col, "HxDataType", attr.DataType);

                                //dc.ExtendedProperties.Add("Max")
                                if (attr.MaximumLength > 0 && col.DataType == typeof(string))
                                {
                                    col.MaxLength = attr.MaximumLength;
                                }
                                Result.Columns.Add(col);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }
            if (Result != null && !HxString.IsNullOrWhiteSpace(tableName))
            {
                Result.TableName = tableName;
            }
            return Result;
        }

        public static DataTable ConvertStructToDataTable<T>(T record, bool bStructOnly = false)
            where T : struct
        {
            DataTable Result = null;
            try
            {
                Result = ConvertStructToDataTable<T>();
                if (Result.Columns.Count > 0 && bStructOnly != true)
                {
                    //DataRow row = Result.NewRow();
                    //foreach (DataColumn col in Result.Columns)
                    //{
                    //    string colName = col.ColumnName;
                    //    object value = HxUtils.GetPropertyInfoValue(record, colName);
                    //    row[colName] = (value == null ? DBNull.Value : value);
                    //}
                    //Result.Rows.Add(row);
                    DataTableRecordAdd<T>(Result, record);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return Result;
        }
        public static bool DataTableRecordAdd<T>(DataTable data, T record)
        {
            bool Result = false;
            try
            {
                if (data != null && data.Columns.Count > 0)
                {
                    DataRow row = data.NewRow();
                    foreach (DataColumn col in data.Columns)
                    {
                        string colName = col.ColumnName;
                        object value = HxUtils.PropertyInfoValue(record, colName);
                        if (col.Unique == true
                            && (col.DataType == typeof(int) || col.DataType == typeof(decimal) || col.DataType == typeof(Single) || col.DataType == typeof(long) || col.DataType == typeof(double) || col.DataType == typeof(float))
                            )
                        {
                            //int no = Convert.ToInt32(data.Select(row).Max(row["no"]));
                            int maxValue = data.Compute(string.Format("MAX({0})", colName), "").ToIntEx();
                            if (maxValue <= int.MinValue)
                            {
                                maxValue = 0;
                            }
                            maxValue += 1;
                            value = maxValue;
                        }
                        row[colName] = (value ?? DBNull.Value);
                    }
                    data.Rows.Add(row);
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                Debug.WriteLine(ex.Message);
                //throw;
            }
            return Result;
        }
        public static bool DataTableRecordSetAdd<T>(DataTable data, List<T> recordSet)
        {
            bool Result = false;
            try
            {
                if (data != null && data.Columns.Count > 0 && recordSet != null && recordSet.Count > 0)
                {
                    foreach (T record in recordSet)
                    {
                        Result = DataTableRecordAdd<T>(data, record);
                    }
                }
            }
            catch (Exception ex)
            {
                Result = false;
                Debug.WriteLine(ex.Message);
                throw;
            }
            return Result;
        }

        [Obsolete("참고용 없애버려~")]
        private static DataTable ConvertStructToDataTableErr<T>(T source, bool bStructOnly = false)
            where T : struct
        {
            DataTable Result = null;
            try
            {
                Type type = typeof(T);

                HxAttribute attrClass = type.GetCustomAttribute<HxAttribute>();
                PropertyInfo[] props = type.GetProperties(BindingFlags.Public);
                if (props != null && props.Length > 0)
                {
                    Result = new DataTable();
                    if (attrClass != null && !attrClass.TableName.IsNullOrWhiteSpaceEx())
                    {
                        Result.TableName = attrClass.TableName;
                    }
                    else
                    {
                        Result.TableName = type.Name;
                    }
                    foreach (PropertyInfo prop in props)
                    {
                        HxAttribute attr = prop.GetType().GetCustomAttribute<HxAttribute>();
                        if (attr != null && attr.IsCustom != true)
                        {
                            string strPropName = prop.Name;
                            string strColName = prop.Name;
                            string strColCaption = prop.Name;
                            if (!attr.ColumnName.IsNullOrWhiteSpaceEx())
                            {
                                strColName = attr.ColumnName;
                                strColCaption = attr.Description.IsNullOrWhiteSpaceEx() ? strPropName : attr.Description;
                            }
                            DataColumn col = new DataColumn(strColName)
                            {
                                DataType = HxHelper.GetDataTypeToType(attr.DataType)
                            };
                            if (attr.IsKey == true)
                            {
                                col.Unique = true;
                                if (col.DataType == typeof(object) || col.DataType == typeof(int)
                                   || attr.DataType == HxDataType.Number || attr.DataType == HxDataType.Double
                                    )
                                {
                                    col.AutoIncrement = true;
                                    col.AutoIncrementSeed = 1;
                                }


                                //if (attr.IsUnique == null)
                                //{
                                //    col.Unique = true;
                                //}
                                //if (attr.IsAutoIncrement == null && (col.DataType == typeof(object) || col.DataType == typeof(int) || attr.DataType == HxDataType.Number || attr.DataType == HxDataType.Double))
                                //{
                                //    col.AutoIncrement = true;
                                //    col.AutoIncrementSeed = 1;
                                //}
                            }

                            if (attr.IsUnique == true && col.Unique != true)
                            {
                                col.Unique = true;
                            }
                            if (attr.AutoIncrement == true && col.AutoIncrement != true && (col.DataType == typeof(object) || attr.DataType == HxDataType.Number || attr.DataType == HxDataType.Double))
                            {
                                col.AutoIncrement = true;
                                col.AutoIncrementSeed = attr.AutoIncrementSeed;
                            }
                            col.Caption = strColCaption ?? prop.Name;
                            col.AllowDBNull = !attr.IsNotNull;
                            if (attr.DefaultValue != null)
                            {
                                col.DefaultValue = attr.DefaultValue;
                            }


                            if (!col.ExtendedProperties.ContainsKey("TableName"))
                            {
                                col.ExtendedProperties.Add("TableName", attr.TableName ?? attrClass.TableName);
                            }
                            else
                            {
                                col.ExtendedProperties["TableName"] = attr.TableName ?? attrClass.TableName;
                            }

                            HxUtils.DoExtendedPropertiesAdd(col, "Remark", attr.Remark);
                            HxUtils.DoExtendedPropertiesAdd(col, "ExtraInfo", attr.ExtraInfo);
                            HxUtils.DoExtendedPropertiesAdd(col, "ExtraGridHidden", attr.ExtraGridHidden);

                            HxUtils.DoExtendedPropertiesAdd(col, "ValueCryptType", attr.ValueCryptType);
                            HxUtils.DoExtendedPropertiesAdd(col, "DefaultValue", attr.DefaultValue);
                            HxUtils.DoExtendedPropertiesAdd(col, "Name", prop.Name);
                            HxUtils.DoExtendedPropertiesAdd(col, "MemberType", prop.MemberType);
                            HxUtils.DoExtendedPropertiesAdd(col, "PropertyType", prop.PropertyType);

                            HxUtils.DoExtendedPropertiesAdd(col, "FormatString", attr.FormatString);
                            HxUtils.DoExtendedPropertiesAdd(col, "CodeKeyValueSet", attr.CodeKeyValueSet);

                            HxUtils.DoExtendedPropertiesAdd(col, "PropName", strPropName);
                            //dc.ExtendedProperties.Add("Max")
                            if (attr.MaximumLength > 0)
                            {
                                col.MaxLength = attr.MaximumLength;
                            }
                            Result.Columns.Add(col);
                        }
                    }

                    if (Result.Columns.Count > 0 && bStructOnly != true)
                    {
                        DataRow row = Result.NewRow();
                        foreach (DataColumn col in Result.Columns)
                        {
                            string colName = col.ColumnName;
                            object value = HxUtils.PropertyInfoValue(source, colName);
                            row[colName] = value;
                        }
                        Result.Rows.Add(row);
                    }
                    //attr
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw ex;
            }
            //if(source == null && bOnlyStruct == true)
            //{
            //    source = new T();
            //}

            return Result;

        }

        public static DataTable ConvertStructToDataTable<T>(List<T> sourceList, bool bStructOnly = false)
            where T : struct
        {
            DataTable Result = null;
            try
            {
                if (sourceList != null && sourceList.Count > 0)
                {
                    Result = ConvertStructToDataTable<T>(sourceList[0], true);
                    if (Result != null && bStructOnly != true)
                    {
                        DataTableRecordSetAdd<T>(Result, sourceList);
                        //foreach (var source in sourceList)
                        //{
                        //    DataRow row = Result.NewRow();
                        //    foreach (DataColumn col in Result.Columns)
                        //    {
                        //        string colName = col.ColumnName;
                        //        object value = HxUtils.GetPropertyInfoValue(source, colName);
                        //        row[colName] = (value == null ? DBNull.Value : value);
                        //    }
                        //    Result.Rows.Add(row);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return Result;
        }
        public static List<T> GetAttributeList<T>(System.Reflection.MemberInfo sender)
            where T : System.Attribute
        {
            List<T> Result = new List<T>();
            //Type type = sender.GetType();
            //Type type = typeof(T);
            //object[] attributes = type.GetCustomAttributes(true);
            object[] attributes = sender.GetCustomAttributes(true);
            if (attributes.Length > 0)
            {
                foreach (object attribute in attributes)
                {
                    //Console.Write("  {0}", attribute.ToString());
                    //T da = attribute as T;
                    if (attribute is T da)
                    {
                        //Console.WriteLine(".Description={0}", da.Description);
                        Result.Add(da);
                    }
                }
            }
            else
            {
                Result.Clear();
                Result = null;
            }
            return Result;
        }

        //public static T GetPostValueSingle<T>(DataTable sender, string name)
        //{
        //    return sender.AsEnumerable().Where(row => row.Field<string>("key").Equals(name)).LastOrDefault().Field<T>("value");
        //}
        //public static T GetPostValueSingle<T>(DataTable sender, Type type)
        //{
        //    HxAttribute attr = type.GetCustomAttribute<HxAttribute>();
        //    if (attr != null)
        //    {
        //        string colName = attr.ColumnName;
        //        return GetPostValueSingle<T>(sender, colName);
        //    }
        //    return default(T);
        //}

        [Obsolete("미구현!!")]
        private static T ConvertDataRowToRecord<T>(DataRow row, bool bInit)
            where T : struct
        {
            T Result = default;
            //record = new TimesheetRec();
            //try
            //{
            if (row != null && row.Table != null && row.Table.Columns.Count > 0)
            {
                DataTable dt = row.Table;
                foreach (DataColumn dc in dt.Columns)
                {
                    string colName = dc.ColumnName;
                    List<PropertyInfo> propList = HxUtils.PropertyInfoList(Result);
                    foreach (PropertyInfo prop in propList)
                    {
                        List<HxAttribute> attrList = HxUtils.AttributeList<HxAttribute>(prop);
                        if (attrList != null && attrList.Count > 0)
                        {
                            foreach (HxAttribute attr in attrList)
                            {
                                if (!HxString.IsNullOrWhiteSpace(attr.ColumnName))
                                {
                                    string strColName = HxString.IsNullOrWhiteSpace(attr.ColumnName) ? prop.Name : attr.ColumnName;
                                    if (colName.ToLower() == strColName.ToLower())
                                    {
                                        //Type type = prop.DeclaringType;
                                        object value = row[colName];
                                        //string strType = prop.PropertyType.FullName;
                                        //string strValueType = value.GetType().ToString();
                                        try
                                        {
                                            prop.SetValue(Result, value);
                                            HxUtils.PropertyInfoValue(prop, strColName, value);
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
            }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    throw ex;
            //}

            return Result;
        }
        [Obsolete("미구현!!")]
        private static bool TryParseDataRowToRecord<T>(DataRow row, out T record)
            where T : struct
        {
            record = default;
            try
            {
                record = ConvertDataRowToRecord<T>(row, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                //throw;
            }
            return true;
        }
        [Obsolete("미구현!!")]
        private static List<T> ConvertDataTableToRecord<T>(DataTable data)
            where T : struct
        {
            List<T> Result = new List<T>();
            if (data != null && data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    T record = ConvertDataRowToRecord<T>(row, true);
                    //bool bSuccess = TryParseDataRowToRecord(row, out record);
                    Result.Add(record);
                }
            }
            return Result;
        }
        #endregion

        #region Object-DataTable Convert
        public static DataTable ConvertObjectPropertiesToDataTable<T>(HxDbColumnNameCharType nameCharType = HxDbColumnNameCharType.Lower)
            //where T : IHxStructSetValue
        {
            DataTable Result = new DataTable();

            //T record = new T();

            //dt.Columns.Add("Column_Name");
            //var a = typeof(T).GetMembers();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                if (info.CanRead && info.CanWrite && info.MemberType == MemberTypes.Property)
                {
                    if (Result.Columns.Contains(info.Name) != true)
                    {
                        string colName = info.Name;
                        if (colName.IsNullOrWhiteSpaceEx() != true)
                        {
                            switch (nameCharType)
                            {
                                case HxDbColumnNameCharType.Lower:
                                    colName = info.Name.ToLower();
                                    break;
                                case HxDbColumnNameCharType.Upper:
                                    colName = info.Name.ToUpper();
                                    break;
                                case HxDbColumnNameCharType.Original:
                                default:
                                    //colName = info.Name;
                                    break;
                            }
                            try
                            {
                                DataColumn dc = new DataColumn(colName, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
                                //dc.DataType = info.PropertyType;
                                Result.Columns.Add(dc);
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    DataColumn dc = new DataColumn(colName);
                                    //dc.DataType = info.PropertyType;
                                    Result.Columns.Add(dc);
                                }
                                catch (Exception ex2)
                                {
                                    throw ex2;
                                }
                                Debug.WriteLine(ex);
                                //throw;
                            }
                            
                        }
                    }
                }
            }

            Result?.AcceptChanges();

            return Result;
        }
        public static DataTable ConvertObjectPropertiesDataTable<T>(IEnumerable<T> list, HxDbColumnNameCharType nameCharType = HxDbColumnNameCharType.Lower)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable Result = ConvertObjectPropertiesToDataTable<T>(nameCharType);
            if (Result != null && Result.Columns.Count > 0)
            {
                foreach (T entity in list)
                {
                    object[] values = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(entity);
                    }

                    Result.Rows.Add(values);
                }
            }

            return Result;
        }

        public static DataTable ConvertObjectToData(object obj, string tableName = null)
        {
            DataTable Result = new DataTable();
            if(tableName.IsNullOrWhiteSpaceEx() != true)
            {
                Result.TableName = tableName;
            }

            DataRow dr = Result.NewRow();
            Result.Rows.Add(dr);

            obj.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(obj, null);
                    Result.Columns.Add(f.Name, f.PropertyType);
                    Result.Rows[0][f.Name] = f.GetValue(obj, null);
                }
                catch { }
            });
            return Result;
        }
        #endregion
        public static string GetEnumMemberValue<T>(T value)
            where T : struct, IConvertible
        {
            return HxType.GetEnumMemberValue<T>(value);
        }

        #region HxResultValue
        public static DataTable ConvertToDataTable(HxResultValue resultValue, string tableName = null)
        {
            if (resultValue != null && resultValue.Value.IsNullOrWhiteSpaceEx() != true)
            {
                string strValue = resultValue.Value?.ToStringEx();

                //var convValue = 

                if(strValue.IsNullOrWhiteSpaceEx() != true)
                {
                    try
                    {
                        if (resultValue.ValueType.ToLower() == "datatable")
                        {
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(strValue);
                            if (dt != null && tableName.IsNullOrWhiteSpaceEx() != true)
                            {
                                dt.TableName = tableName;
                            }
                            return dt;
                        }
                        if (resultValue.ValueType.ToLower() == "dataset")
                        {
                            DataSet ds = JsonConvert.DeserializeObject<DataSet>(strValue);
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                return ds.Tables[0];
                            }
                        }

                        if (resultValue.ValueType.ToLower() == "dataview")
                        {
                            DataView dv = JsonConvert.DeserializeObject<DataView>(strValue);
                            DataTable dt = dv?.Table;
                            if (dt != null && tableName.IsNullOrWhiteSpaceEx() != true)
                            {
                                dt.TableName = tableName;
                            }
                            return dt;
                        }

                        if (resultValue.ValueType.ToLower() == "dictionary")
                        {
                            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(strValue);
                            DataTable dt = new DataTable();
                            dt.Columns.Add("key", typeof(string));
                            dt.Columns.Add("value");
                            foreach (var o in dic)
                            {
                                DataRow dr = dt.NewRow();
                                dr["key"] = o.Key;
                                dr["value"] = o.Value;
                                dt.Rows.Add(dr);
                            }
                            return dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        throw ex;
                    }
                }
            }
            return null;
        }

        
        /*
        public static DataSet ConvertToDataSet(HxResultValue resultValue)
        {
            if (resultValue != null && resultValue.Value != null)
            {
                if (resultValue.Value is DataSet)
                {
                    if (resultValue.Value is DataSet ds && ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
                if(resultValue.Value is DataTable dt)
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
                if(resultValue.Value is DataView dv)
                {
                    DataTable convDT = dv?.ToTable();
                    DataSet ds = new DataSet();
                    ds.Tables.Add(convDT);
                    return ds;
                }
            }
            return null;
        }
        
        public static DataView ConvertToDataView(HxResultValue resultValue)
        {
            if (resultValue != null && resultValue.Value is DataView dv)
            {
                return dv;
            }

            if (resultValue.Value is DataTable dt)
            {
                return dt?.DefaultView;
            }

            if (resultValue.Value is DataSet ds && ds.Tables.Count > 0)
            {
                return ds?.Tables[0]?.DefaultView;
            }

            return null;
        }
        */
        public static DataTable ConvertToDataTable<T>(HxResultValue resultValue, string tableName = null, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue
        {
            if (resultValue != null && resultValue.Value.IsNullOrWhiteSpaceEx() != true)
            {
                string strValue = resultValue.Value?.ToStringEx();
                if(strValue.IsNullOrWhiteSpaceEx() != true)
                {
                    try
                    {
                        IEnumerable<T> value = JsonConvert.DeserializeObject<IEnumerable<T>>(strValue);
                        if (value != null && value.Any() == true)
                        {
                            return HxConvert.ConvertLinqToDataTable<T>(value, tableName, colNameCharType);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        throw ex;
                    }
                    
                }
                
            }
            return null;
        }
        #endregion

        #region JSON Convert
        public static class Converter
        {
            public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                    {
                        new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal }
                    },
            };
        }

        /// <summary>
        /// JsonConvert.SerializeObject To String
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>JSON string representation of the object.</returns>
        public static string JsonSerializeObject(object value)
        {
            string Result = null;
            if(value != null)
            { 
                Result = JsonConvert.SerializeObject(value);
            }
            return Result;
        }
        /// <summary>
        /// JsonConvert.SerializeObject To String
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>JSON string representation of the object.</returns>
        public static string JsonSerializeObject(object value, Newtonsoft.Json.Formatting formatting)
        {
            return JsonConvert.SerializeObject(value, formatting);
        }
        /// <summary>
        /// Serializes the specified object to a JSON string using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The Newtonsoft.Json.JsonSerializerSettings used to serialize the object. If this
        //     is null, default serialization settings will be used.</param>
        /// <returns>JSON string representation of the object.</returns>
        public static string JsonSerializeObject(object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///     Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object JsonDeserializeObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///     Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object JsonDeserializeObject(object value)
        {
            return JsonConvert.DeserializeObject(value.ToStringEx());
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///      Deserializes the JSON to a .NET object using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.
        /// If this is null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object JsonDeserializeObject(string value, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(value, settings);
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///      Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The System.Type of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        /// 
        public static object JsonDeserializeObject(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }
        public static T JsonDeserializeObject<T>(object value)
        {
            return JsonDeserializeObject<T>(value.ToStringEx());
        }
        public static T JsonDeserializeObject<T>(string value)
        {
            bool bException1 = false;
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1);
                bException1 = true;
                //throw ex1;
            }
            if (bException1 == true)
            {
                try
                {
                    object value2 = JsonConvert.DeserializeObject(value);
                    return JsonConvert.DeserializeObject<T>(value2.ToStringEx());
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2);
                    throw ex2;
                }
            }
            return default;
        }

        public class CustomContractResolver : DefaultContractResolver
        {
            private Dictionary<string, string> PropertyMappings { get; set; }

            public CustomContractResolver()
            {
                this.PropertyMappings = new Dictionary<string, string>
                {
                    {"ResultType", "result_type"},
                    {"Value", "value"},
                    {"ValueType", "value_type"},
                    {"Success", "success"},
                    {"MessageType", "message_type"},
                    {"DetailMessage", "detail_message"},
                    {"Count", "count"},
                    {"Value2", "value2"}
                };
            }

            protected override string ResolvePropertyName(string propertyName)
            {
                string resolvedName;
                var resolved = this.PropertyMappings.TryGetValue(propertyName, out resolvedName);
                return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
            }
        }

        public static HxResultValue JsonDeserializeObjectResultValue(string value)
        {

            HxResultValue Result = null;
            bool bException1 = false;
            try
            {
                Result = JsonConvert.DeserializeObject<HxResultValue>(value);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1);
                bException1 = true;
                //throw ex1;
            }
            if (bException1 == true)
            {
                try
                {
                    object value2 = JsonConvert.DeserializeObject(value);
                    //Result = JsonConvert.DeserializeObject<HxResultValue>(value2.ToStringEx());
                    Result = JsonConvert.DeserializeObject<HxResultValue>(value2.ToString());
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2);
                    throw ex2;
                }
            }
            return Result;
            /*
            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
            */
        }

        public static DataTable JsonStringToDataTable(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }


        #endregion

        public static Dictionary<string, object> GetDictionaryValueObject<T>(T value)
            where T : class, new()
        {
            try
            {
                return value?.GetType()?
                            .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?
                            .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(value, null));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return null;
        }
        public static Dictionary<string, TValue> GetDictionary<T, TValue>(T value)
            where T : class, new()
        {
            try
            {
                return value?.GetType()?
                            .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?
                            .ToDictionary(prop => prop.Name, prop => (TValue)prop.GetValue(value, null));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return null;
        }
        #region DataTable / DataRow
        
        public static T[] ConvertDataRowToArray<T>(DataRowCollection rows)
            where T : DataRow
        {
            T[] Result = null;
            if (rows == null || rows.Count <= 0) return Result;

            Result = rows.Cast<T>()?.ToArray();
            
            return Result;
        }
        public static T[] ConvertDataTableToArray<T>(DataTable data)
            where T : DataRow
        {
            T[] Result = null;
            if (data == null || data.Rows.Count <= 0) return Result;

            Result = ConvertDataRowToArray<T>(data.Rows);
            
            return Result;
        }
        public static DataRow[] ConvertToDataRowArray(DataTable data)
        {
            DataRow[] Result = ConvertDataTableToArray<DataRow>(data);
            if (Result == null)
            {
                Result = data.Select();
            }
            return Result;
        }
        public static DataRow[] ConvertToDataRowArray(DataRowCollection rows)
        {
            DataRow[] Result = ConvertDataRowToArray<DataRow>(rows);
            return Result;
        }
        #endregion
    }
}