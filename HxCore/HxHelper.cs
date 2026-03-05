using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Linq;

namespace HxCore
{


    public class HxHelper
    {
        //
        // 요약:
        //     데이터 필드 및 매개 변수와 연결된 데이터 형식의 열거형을 나타냅니다.
        // C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.ComponentModel.DataAnnotations.dll
        public static Type GetDataTypeToType(HxDataType type)
        {
            Type Result = typeof(object);
            switch (type)
            {
                case HxDataType.DateTime:
                case HxDataType.Date:
                case HxDataType.Time:
                    Result = typeof(DateTime);
                    break;
                case HxDataType.Number:
                    Result = typeof(int);
                    break;
                case HxDataType.Duration:
                case HxDataType.Double:
                    Result = typeof(Decimal);
                    break;
                case HxDataType.Currency:
                    Result = typeof(ulong);
                    break;
                case HxDataType.PhoneNumber:
                case HxDataType.Text:
                case HxDataType.Html:
                case HxDataType.MultilineText:
                case HxDataType.EmailAddress:
                case HxDataType.Password:
                case HxDataType.Url:
                case HxDataType.ImageUrl:
                case HxDataType.PostalCode:
                case HxDataType.Upload:
                case HxDataType.Base64Text:
                case HxDataType.CryptPassword:
                case HxDataType.CreditCard:
                    Result = typeof(string);
                    break;
                case HxDataType.None:
                case HxDataType.Custom:
                default:
                    break;
            }
            return Result;
        }
        /// <summary>
        /// Struct타입을 DataTable로 구조로 변경
        /// </summary>
        /// <typeparam name="T">Source Struct Type</typeparam>
        /// <param name="tableName">(optional)Table Name</param>
        /// <returns>DataTable</returns>
        
        
        public static bool MergeDataTable(DataTable data1, DataTable data2)
        {
            return HxUtils.MergeDataTable(data1, data2);
        }
        public static bool MergeDataRow(ref DataTable data1, DataRow row)
        {
            return HxUtils.MergeDataRow(ref data1, row);
        }

        public static void SetValue<T>(ref T sender, string inputColumnName, object inputValue)
            where T : struct
        {
            object objValue = inputValue;
            if (objValue == DBNull.Value)
            {
                objValue = null;
            }
            //System.Reflection.PropertyInfo pInfo = HxUtils.GetPropertyInfo(sender, columnName);
            List<PropertyInfo> propList = HxUtils.PropertyInfoList(sender);
            if (propList != null && propList.Count > 0)
            {
                HxAttribute attrClass = sender.GetType().GetCustomAttribute<HxAttribute>();
                if (attrClass != null)
                {
                    foreach (PropertyInfo prop in propList)
                    {
                        Type propertyType = prop.PropertyType;
                        TypeCode typeCode = Type.GetTypeCode(propertyType);
                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            /*
                            switch (Type.GetTypeCode(propertyType))
                            {
                                case TypeCode.DateTime:
                                    objValue = new DateTime(1900, 1, 1); ;
                                    break;
                            }*/
                            
                            switch (typeCode)
                            {
                                case TypeCode.DateTime:
                                    objValue = objValue?.ToDateTimeEx();
                                    break;
                                    //case TypeCode.Decimal:
                                    //    objValue = objValue?.ToConvertEx<decimal>();
                                    //    break;
                                    //case TypeCode.Double:
                                    //    objValue = objValue?.ToConvertEx<double>();
                                    //    break;
                                    //case TypeCode.Int16:
                                    //    objValue = objValue?.ToConvertEx<Int16>();
                                    //    break;
                                    //case TypeCode.Int32:
                                    //    objValue = objValue?.ToIntEx();
                                    //    break;
                                    //case TypeCode.Int64:
                                    //    objValue = objValue?.ToConvertEx<Int64>();
                                    //    break;
                                    //case TypeCode.Single:
                                    //    objValue = objValue?.ToConvertEx<Single>();
                                    //    break;
                                    //case TypeCode.UInt16:
                                    //    objValue = objValue?.ToConvertEx<UInt16>();
                                    //    break;
                                    //case TypeCode.UInt32:
                                    //    objValue = objValue?.ToConvertEx<UInt32>();
                                    //    break;
                                    //case TypeCode.UInt64:
                                    //    objValue = objValue?.ToConvertEx<UInt64>();
                                    //    break;
                                    //case TypeCode.String:
                                    //    objValue = objValue.ToStringEx();
                                    //    break;
                            }
                        }
                        else if (objValue == null || objValue == DBNull.Value)
                        {
                            switch (typeCode)
                            {
                                case TypeCode.DateTime:
                                    objValue = objValue != null ? objValue.ToDateTimeEx() : new DateTime(1900, 1, 1);
                                    break;
                                case TypeCode.Decimal:
                                    objValue = objValue != null ? objValue.ToConvertEx<decimal>() : decimal.MinValue;
                                    break;
                                case TypeCode.Double:
                                    objValue = objValue != null ? objValue?.ToConvertEx<double>() : double.MinValue;
                                    break;
                                case TypeCode.Int16:
                                    objValue = objValue != null ? objValue?.ToConvertEx<Int16>() : Int16.MinValue;
                                    break;
                                case TypeCode.Int32:
                                    objValue = objValue.ToIntEx();
                                    break;
                                case TypeCode.Int64:
                                    objValue = objValue != null ? objValue?.ToConvertEx<Int64>() : Int32.MinValue;
                                    break;
                                case TypeCode.Single:
                                    objValue = objValue != null ? objValue?.ToConvertEx<Single>() : Single.MinValue;
                                    break;
                                case TypeCode.UInt16:
                                    objValue = objValue != null ? objValue?.ToConvertEx<UInt16>() : UInt16.MinValue;
                                    break;
                                case TypeCode.UInt32:
                                    objValue = objValue != null ? objValue?.ToConvertEx<UInt32>() : UInt32.MinValue;
                                    break;
                                case TypeCode.UInt64:
                                    objValue = objValue != null ? objValue?.ToConvertEx<UInt64>() : UInt64.MinValue;
                                    break;
                                case TypeCode.String:
                                    objValue = objValue.ToStringEx();
                                    break;
                            }
                        }
                        List<HxAttribute> attrList = HxUtils.AttributeList<HxAttribute>(prop);
                        if (attrList != null && attrList.Count > 0)
                        {
                            HxAttribute attr = attrList[attrList.Count - 1];
                            if (attr != null && attr.IsCustom != true)
                            {
                                string strPropName = prop.Name;
                                string strColName = attr.ColumnName.IsNullOrWhiteSpaceEx() ? prop.Name.ToLower() : attr.ColumnName;
                                string strColCaption = attr.Description.IsNullOrWhiteSpaceEx() ? prop.Name.ToLower() : attr.Description;
                                if (strColName == inputColumnName)
                                {
                                    if (objValue == null || objValue == DBNull.Value || objValue.IsNullOrWhiteSpaceEx())
                                    {

                                        if(prop.PropertyType.FullName.Contains("System.DateTime"))
                                        {
                                            objValue = HxUtils.MinDateTime();
                                        }
                                    }
                                    if(!objValue.IsNullOrWhiteSpaceEx() && objValue.GetType() == typeof(string))
                                    {
                                        if (prop.PropertyType.FullName.Contains("System.DateTime"))
                                        {
                                            objValue = objValue.ToDateTimeEx();
                                        }
                                    }
                                    object record = (object)sender;
                                    prop.SetValue(record, objValue);
                                    sender = (T)record;
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
