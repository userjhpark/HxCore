using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HxCore
{
    partial class HxUtils
    {
        #region System.Reflection.PropertyInfo 관련 처리
        /// <summary>
        /// 해당 Object/Resource의 Properties(PropertyInfo[])
        /// </summary>
        /// <param name="sender">대상 Resource</param>
        /// <returns>PropertyInfo[](Properties)</returns>
        public static System.Reflection.PropertyInfo[] PropertyInfoArray(Object sender)
        {
            Type type = sender.GetType();
            System.Reflection.PropertyInfo[] Result = type.GetProperties();
            return Result;
        }
        /// <summary>
        ///  해당 Object의 Properties(PropertyInfo[])
        /// </summary>
        /// <param name="sender">대상 Resource</param>
        /// <param name="flags">BindingFlags</param>
        /// <returns>PropertyInfo[](Properties)</returns>
        public static System.Reflection.PropertyInfo[] PropertyInfoArray(Object sender, System.Reflection.BindingFlags flags)
        {
            Type type = sender.GetType();
            System.Reflection.PropertyInfo[] Result = type.GetProperties(flags);
            return Result;
        }
        /// <summary>
        /// 해당 Object/Resource의 Properties / PropertyInfo List
        /// </summary>
        /// <param name="sender">대상 Resource</param>
        /// <returns></returns>
        public static List<System.Reflection.PropertyInfo> PropertyInfoList(Object sender)
        {
            //http://stackoverflow.com/questions/1603170/conversion-of-system-array-to-list
            System.Reflection.PropertyInfo[] PropertyArray = PropertyInfoArray(sender);
            //PropertyArray.t
            //List<System.Reflection.PropertyInfo> Result = new List<System.Reflection.PropertyInfo>(PropertyArray);
            List<System.Reflection.PropertyInfo> Result = new List<System.Reflection.PropertyInfo>();
            if (PropertyArray.Length > 0)
            {
                Result.AddRange(PropertyArray);
            }
            return Result;
        }
        /// <summary>
        /// Object/Resource의 Properties / PropertyInfo List
        /// </summary>
        /// <param name="sender">대상 Resource</param>
        /// <param name="flags">BindingFlags</param>
        /// <returns></returns>
        public static List<System.Reflection.PropertyInfo> PropertyInfoList(Object sender, System.Reflection.BindingFlags flags)
        {
            //http://stackoverflow.com/questions/1603170/conversion-of-system-array-to-list
            System.Reflection.PropertyInfo[] PropertyArray = PropertyInfoArray(sender, flags);
            //PropertyArray.t
            //List<System.Reflection.PropertyInfo> Result = new List<System.Reflection.PropertyInfo>(PropertyArray);
            List<System.Reflection.PropertyInfo> Result = new List<System.Reflection.PropertyInfo>();
            if (PropertyArray.Length > 0)
            {
                Result.AddRange(PropertyArray);
            }
            return Result;
        }
        /// <summary>
        /// 해당 Object의 Property Name의 PropertyInfo 객체 반환
        /// </summary>
        /// <param name="sender">대상 Object</param>
        /// <param name="property_name">대상 Property</param>
        /// <returns>PropertyInfo</returns>
        public static System.Reflection.PropertyInfo PropertyInfo(Object sender, string property_name)
        {
            System.Reflection.PropertyInfo Result = null;
            Type type = sender.GetType();

            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(property_name, System.Reflection.BindingFlags.IgnoreCase);
            if (propertyInfo == null)
            {
                System.Reflection.PropertyInfo[] pInfos = type.GetProperties();
                foreach (System.Reflection.PropertyInfo pinfo in pInfos)
                {
                    if (pinfo.Name.ToLower() == property_name.ToLower())
                    {
                        Result = pinfo;
                        break;
                    }
                }
            }
            else
            {
                Result = propertyInfo;
            }
            return Result;
        }

        /// <summary>
        /// Object의 Property 존재 여부
        /// </summary>
        /// <param name="sender">대상 Object</param>
        /// <param name="property_name">대상 Property</param>
        /// <returns>존재 여부?</returns>
        public static bool IsPropertyInfo(Object sender, string property_name)
        {
            bool Result;
            /*
Type type = sender.GetType();
System.Reflection.PropertyInfo pInfo = type.GetProperty(property_name, System.Reflection.BindingFlags.IgnoreCase);
if (pInfo == null)
{
System.Reflection.PropertyInfo[] pInfos = type.GetProperties();
foreach (System.Reflection.PropertyInfo pinfo in pInfos)
{
if (pinfo.Name.ToLower() == property_name.ToLower())
{
Result = true;
break;
}
}
}
else
{
Result = true;
}*/
            if (PropertyInfo(sender, property_name) != null)
                Result = true;
            else
                Result = false;
            return Result;
        }
        /// <summary>
        /// Object의 Property 속성 값 가져오기
        /// </summary>
        /// <param name="sender">대상 Object</param>
        /// <param name="property_name">대상 Proerty</param>
        /// <returns>속성 값</returns>
        public static object PropertyInfoValue(Object sender, string property_name)
        {
            object Result = null;
            Type type = sender.GetType();
            //Public 항목에서만 검색
            //Result = type.GetProperty(property_name, System.Reflection.BindingFlags.IgnoreCase);
            /*
            System.Reflection.PropertyInfo[] pInfos = type.GetProperties();
            foreach (System.Reflection.PropertyInfo pinfo in pInfos)
            {
                if (pinfo.Name.ToLower() == property_name.ToLower())
                {
                    if (pinfo.CanRead)
                        Result = pinfo.GetValue(sender, null);
                    else
                        Result = null;
                    break;
                }
            }*/
            System.Reflection.PropertyInfo pinfo = PropertyInfo(sender, property_name);
            if (pinfo != null && pinfo.CanRead)
            {
                Result = pinfo.GetValue(sender, null);
            }
            return Result;
        }
        /// <summary>
        /// Object의 Property 속성 값 변경
        /// </summary>
        /// <param name="sender">대상 Object</param>
        /// <param name="property_name">대상 Property</param>
        /// <param name="value">속성 값</param>
        /// <returns>성공 여부?</returns>
        public static bool PropertyInfoValue(object sender, string property_name, object value)
        {
            bool Result = false;
            Type type = sender.GetType();
            /*
            System.Reflection.PropertyInfo[] pInfos = type.GetProperties();
             * */
            System.Reflection.PropertyInfo pInfo = PropertyInfo(sender, property_name);
            if (pInfo != null && pInfo.CanWrite)
            {
                try
                {

                    object objValue = HxConvert.ConvertTo<object>(value);

                    Type pInfoType = pInfo.GetType();
                    Type valueType = value.GetType();

                    //pinfo.SetValue(property_name, value, null);
                    if (valueType == typeof(DBNull))
                    {
                        switch (Type.GetTypeCode(pInfoType))
                        {
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                                objValue = int.MinValue;
                                break;
                            case TypeCode.String:
                                objValue = null;
                                break;
                            case TypeCode.DateTime:
                                objValue = new DateTime(1900, 1, 1); ;
                                break;

                        }
                    }
                    else if (pInfoType != valueType)
                    {
                        //objValue
                        switch (Type.GetTypeCode(pInfoType))
                        {
                            case TypeCode.Decimal:
                                objValue = value.ToConvertEx<decimal>();
                                break;
                            case TypeCode.Int32:
                                objValue = value.ToIntEx();
                                break;
                            case TypeCode.String:
                                objValue = value.ToStringEx();
                                break;
                            case TypeCode.DateTime:
                                objValue = value.ToDateTimeEx();//  new DateTime(1900, 1, 1); ;
                                break;
                            case TypeCode.Double:
                                objValue = value.ToConvertEx<double>();
                                break;
                        }
                    }
                    if (pInfo.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        //s.GetType().GetField("Field").SetValueDirect(__makeref(s), 5);
                        sender.GetType().GetField(pInfo.Name).SetValueDirect(__makeref(sender), objValue);
                        Result = true;
                    }
                    else if (pInfo.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        //sender.GetType().GetField(pInfo.Name).SetValueDirect(__makeref(sender), objValue);
                        sender.GetType().GetProperty(pInfo.Name).SetValue(sender, objValue);
                        Result = true;
                    }
                    //if (pInfo.MemberType == System.Reflection.MemberTypes.Field || pInfo.MemberType == System.Reflection.MemberTypes.Property)
                    //{
                    //    pInfo.SetValue((object)sender, objValue, null);
                    //    Result = true;
                    //}
                    //sender.GetType().GetProperty(pInfo.Name).SetValue((object)sender, objValue);


                    //pInfo.SetValue(sender, value, null);

                }
                catch (Exception ex)
                {
                    Result = false;
                    throw ex;
                }
            }
            else
            {
                Result = false;
            }
            return Result;
        }

        public static bool PropertyInfoValueSet<T>(ref T sender, string property_name, object value)
            where T : struct
        {
            bool Result = false;
            Type type = sender.GetType();
            /*
            System.Reflection.PropertyInfo[] pInfos = type.GetProperties();
             * */
            System.Reflection.PropertyInfo pInfo = PropertyInfo(sender, property_name);
            if (pInfo != null && pInfo.CanWrite)
            {
                try
                {

                    object objValue = HxConvert.ConvertTo<object>(value);

                    Type pInfoType = pInfo.GetType();
                    Type valueType = value.GetType();

                    //pinfo.SetValue(property_name, value, null);
                    if (valueType == typeof(DBNull))
                    {
                        TypeCode typeCode = Type.GetTypeCode(pInfoType);
                        if (pInfo.MemberType == System.Reflection.MemberTypes.Property)
                        {
                            typeCode = Type.GetTypeCode(pInfo.PropertyType);
                        }
                        switch (typeCode)
                        {
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                                objValue = int.MinValue;
                                break;
                            case TypeCode.String:
                                objValue = null;
                                break;
                            case TypeCode.DateTime:
                                objValue = new DateTime(1900, 1, 1); ;
                                break;
                            default:
                                objValue = value.ToConvertEx<object>();
                                break;

                        }
                    }
                    else if (pInfoType != valueType)
                    {
                        //objValue
                        TypeCode typeCode = Type.GetTypeCode(pInfoType);
                        if (pInfo.MemberType == System.Reflection.MemberTypes.Property)
                        {
                            typeCode = Type.GetTypeCode(pInfo.PropertyType);
                        }
                        switch (typeCode)
                        {
                            case TypeCode.Decimal:
                                objValue = value.ToConvertEx<decimal>();
                                break;
                            case TypeCode.Int32:
                                objValue = value.ToIntEx();
                                break;
                            case TypeCode.String:
                                objValue = value.ToStringEx();
                                break;
                            case TypeCode.DateTime:
                                objValue = value.ToDateTimeEx();//  new DateTime(1900, 1, 1); ;
                                break;
                            case TypeCode.Double:
                                objValue = value.ToConvertEx<double>();
                                break;

                        }
                    }

                    //pInfo.SetValue(null,  objValue, null);
                    //var s = new T();
                    if (pInfo.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        //s.GetType().GetField("Field").SetValueDirect(__makeref(s), 5);
                        sender.GetType().GetField(pInfo.Name).SetValueDirect(__makeref(sender), objValue);
                        Result = true;
                    }
                    else if (pInfo.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        object objSender = sender;
                        //sender.GetType().Getpro(pInfo.Name).SetValueDirect(__makeref(sender), objValue);
                        Type propertyType = pInfo.PropertyType;

                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            switch (Type.GetTypeCode(propertyType))
                            {
                                case TypeCode.DateTime:
                                    objValue = new DateTime(1900, 1, 1); ;
                                    break;
                            }
                        }
                        else
                        {
                            pInfo.SetValue(objSender, objValue, null);
                        }
                        sender = (T)objSender;
                        //objSender.GetType().GetProperty(pInfo.Name).SetValue((object)sender, objValue, null);
                        Result = true;
                    }
                    //pInfo.SetValue(sender, value, null);

                }
                catch (Exception ex)
                {
                    Result = false;
                    throw ex;
                }
            }
            else
            {
                Result = false;
            }
            return Result;
        }
        /// <summary>
        /// DataColumn  확장 속성
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="property_name">KEY</param>
        /// <param name="value">VALUE</param>
        /// <param name="bOverWrite">겹쳐쓰기(Override)</param>
        public static void DoPropertyValueAdd(PropertyCollection sender, string property_name, object value, bool bOverWrite = true)
        {
            if (!sender.ContainsKey(property_name))
            {
                sender.Add(property_name, value);
            }
            else if (bOverWrite == true)
            {
                sender[property_name] = value;
            }
        }


        #endregion

        #region Custom Property
        // <summary>
        /// Uses reflection to get the field value from an object.
        /// </summary>
        /// <param name="type">The instance type.</param>
        /// <param name="instance">The instance object.</param>
        /// <param name="fieldName">The field's name which is to be fetched.</param>
        /// <returns>The field value from the object.</returns>
        internal static object GetInstanceMemberFieldValue(Type type, object instance, string fieldName)
        {
            //출처 : https://stackoverflow.com/questions/3303126/how-to-get-the-value-of-private-field-using-reflection
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }

        /// <summary>
        /// Private Member Field 값 가져오기
        /// </summary>
        /// <typeparam name="T">Instance Type</typeparam>
        /// <param name="instance">Instance 객체</param>
        /// <param name="fieldName">Member Field 명</param>
        /// <returns>값</returns>
        public static object GetInstanceMemberFieldValue<T>(T instance, string fieldName, BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
        {
            //출처 : https://stackoverflow.com/questions/3303126/how-to-get-the-value-of-private-field-using-reflection
            if (instance == null || fieldName.IsNullOrWhiteSpaceEx() == true) { return null; }

            //BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = typeof(T).GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
        public static object GetMemberPropertyValue(Object sender, string name, object defaultValue = null, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            object Result = null;
            try
            {
                var v = sender;
                if (v != null)
                {
                    var memberField = flags == BindingFlags.Default ? v.GetType().GetProperty(name) : v.GetType().GetProperty(name, flags);
                    if (bIgnoreCaseToUpperOrToLower == true && memberField == null)
                    {
                        memberField = v.GetType().GetProperty(name.ToUpper(), BindingFlags.Public | BindingFlags.Instance);
                        if (memberField == null)
                        {
                            memberField = v.GetType().GetProperty(name.ToLower(), BindingFlags.Public | BindingFlags.Instance);
                        }
                    }
                    if (memberField != null)
                    {
                        //Type t = Nullable.GetUnderlyingType(memberField.PropertyType) ?? memberField.PropertyType;
                        object safeValue = memberField.GetValue(v);
                        Result = safeValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }
        public static object GetJsonPropertyValue(Object sender, string name, object defaultValue = null, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = false, bool bThrowException = false)
        {
            object Result = null;
            try
            {
                var v = sender;
                if (v != null)
                {

                    var props = flags == BindingFlags.Default ? v.GetType().GetProperties() : v.GetType().GetProperties(flags);
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (object attr in prop.GetCustomAttributes(true))
                        {
                            JsonPropertyAttribute customAttr = (attr as JsonPropertyAttribute);
                            if (customAttr != null)
                            {
                                if (customAttr.PropertyName == name)
                                {
                                    Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                }
                                else if (customAttr.PropertyName == name.ToUpper())
                                {
                                    Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                }
                                else if (customAttr.PropertyName == name.ToLower())
                                {
                                    Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                }
                            }
                            //result += (attr as JsonPropertyAttribute).PropertyName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }
        public static object GetHxAttributePropertyValue(Object sender, string name, object defaultValue = null, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = false, bool bThrowException = false)
        {
            object Result = null;
            try
            {
                var v = sender;
                if (v != null)
                {
                    PropertyInfo[] props = flags == BindingFlags.Default ? v.GetType().GetProperties() : v.GetType().GetProperties(flags);
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (object attr in prop.GetCustomAttributes(true))
                        {
                            HxAttribute customAttr = (attr as HxAttribute);
                            if (customAttr != null)
                            {
                                if (customAttr != null)
                                {
                                    if (customAttr.ColumnName == name)
                                    {
                                        Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.ColumnName == name.ToUpper())
                                    {
                                        Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.ColumnName == name.ToLower())
                                    {
                                        Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower && customAttr.Name == name)
                                    {
                                        Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.Name == name.ToUpper())
                                    {
                                        Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.Name == name.ToLower())
                                    {
                                        Result = GetMemberPropertyValue(sender, prop.Name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                }
                            }
                            //result += (attr as JsonPropertyAttribute).PropertyName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }
        public static object GetCustomPropertyValue(Object sender, string name, object defaultValue = null, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = false, bool bThrowException = false)
        {
            object Result = null;
            try
            {
                Result = GetJsonPropertyValue(sender, name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                if (Result == null)
                {
                    Result = GetHxAttributePropertyValue(sender, name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                }
                if (Result == null)
                {
                    Result = GetMemberPropertyValue(sender, name, defaultValue, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }

        public static bool SetMemberPropertyValue(Object sender, string name, object value, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            bool Result = false;
            try
            {
                var v = sender;
                if (v != null)
                {
                    PropertyInfo memberField = flags == BindingFlags.Default ? v.GetType().GetProperty(name) : v.GetType().GetProperty(name, flags);
                    if (bIgnoreCaseToUpperOrToLower == true && memberField == null)
                    {
                        memberField = flags == BindingFlags.Default ? v.GetType().GetProperty(name.ToUpper()) : v.GetType().GetProperty(name.ToUpper(), flags);
                        if (memberField == null)
                        {
                            memberField = flags == BindingFlags.Default ? v.GetType().GetProperty(name.ToLower()) : v.GetType().GetProperty(name.ToLower(), flags);
                        }
                    }
                    if (memberField != null)
                    {
                        Type t = Nullable.GetUnderlyingType(memberField.PropertyType) ?? memberField.PropertyType;
                        object safeValue = null;// (value == null || value == DBNull.Value) ? null : Convert.ChangeType(value, t);
                        if (value == null || value == DBNull.Value)
                        {
                            safeValue = null;
                        }
                        else if (t == typeof(Nullable<decimal>))
                        {
                            safeValue = value.ToNullableDecimalEx();
                        }
                        else if (t == typeof(Nullable<long>))
                        {
                            safeValue = value.ToNullableLongEx();
                        }
                        else if (t == typeof(Nullable<int>))
                        {
                            safeValue = value.ToNullableIntEx();
                        }
                        else if (t == typeof(Nullable<bool>))
                        {
                            safeValue = value.ToNullableBoolEx();
                        }
                        else if (t == typeof(Nullable<DateTime>) || t == typeof(DateTime))
                        {
                            //safeValue = value.ToDateTimeEx(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern).ToNullableDateTimeEx();
                            DateTime dateTime;
                            bool bSucess = DateTime.TryParse(value.ToString(), out dateTime);
                            if (bSucess == true)
                            {
                                safeValue = dateTime;
                            }
                            else
                            {
                                try
                                {
                                    if (t == typeof(Nullable<DateTime>))
                                    {
                                        safeValue = value.ToNullableDateEx();
                                    }
                                    else
                                    {
                                        safeValue = value.ToDateTimeEx();
                                    }
                                }
                                catch (Exception exDate)
                                {
                                    safeValue = Convert.ChangeType(value, t);
                                    Debug.WriteLine(exDate);
                                }
                                
                            }
                        }
                        else
                        {
                            //safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                            safeValue = Convert.ChangeType(value, t);
                        }

                        var member = memberField.GetValue(v);
                        memberField.SetValue(v, safeValue);
                        //member = memberField.GetValue(v);
                        /*
                        // no flags necessary for a public property
                        var prop = member.GetType().GetProperty(name);
                        if (prop == null)
                        {
                            prop = member.GetType().GetProperty(name?.ToUpper());
                        }
                        if (prop != null)
                        {
                            prop.SetValue(member, value);
                        }
                        */
                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }
        public static bool SetJsonPropertyValue(Object sender, string name, object value, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = false, bool bThrowException = false)
        {
            bool Result = false;
            try
            {
                var v = sender;
                if (v != null)
                {

                    var props = flags == BindingFlags.Default ? v.GetType().GetProperties() : v.GetType().GetProperties(flags);
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (object attr in prop.GetCustomAttributes(true))
                        {
                            JsonPropertyAttribute customAttr = (attr as JsonPropertyAttribute);
                            if (customAttr != null)
                            {
                                if (customAttr.PropertyName == name)
                                {
                                    Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                }
                                else if (customAttr.PropertyName == name.ToUpper())
                                {
                                    Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                }
                                else if (customAttr.PropertyName == name.ToLower())
                                {
                                    Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                }
                            }
                            //result += (attr as JsonPropertyAttribute).PropertyName;
                        }
                    }
                }
                Result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }
        public static bool SetHxAttributePropertyValue(Object sender, string name, object value, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = false, bool bThrowException = false)
        {
            bool Result = false;
            try
            {
                var v = sender;
                if (v != null)
                {
                    PropertyInfo[] props = flags == BindingFlags.Default ? v.GetType().GetProperties() : v.GetType().GetProperties(flags);
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (object attr in prop.GetCustomAttributes(true))
                        {
                            HxAttribute customAttr = (attr as HxAttribute);
                            if (customAttr != null)
                            {
                                if (customAttr != null)
                                {
                                    if (customAttr.ColumnName == name)
                                    {
                                        Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.ColumnName == name.ToUpper())
                                    {
                                        Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.ColumnName == name.ToLower())
                                    {
                                        Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower && customAttr.Name == name)
                                    {
                                        Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.Name == name.ToUpper())
                                    {
                                        Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                    else if (bIgnoreCaseToUpperOrToLower == true && customAttr.Name == name.ToLower())
                                    {
                                        Result = SetMemberPropertyValue(sender, prop.Name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                                    }
                                }
                            }
                            //result += (attr as JsonPropertyAttribute).PropertyName;
                        }
                    }
                }
                Result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }
        public static bool SetCustomPropertyValue(Object sender, string name, object value, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = false, bool bThrowException = false)
        {
            bool Result = false;
            try
            {
                Result = SetJsonPropertyValue(sender, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                if (Result != true)
                {
                    Result = SetHxAttributePropertyValue(sender, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                }
                if (Result != true)
                {
                    Result = SetMemberPropertyValue(sender, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
                }
            }
            catch (Exception ex)
            {
                if (bThrowException == true)
                {
                    throw ex;
                }
            }
            return Result;
        }
        #endregion
    }
}
