using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace HxCore
{
    public enum HxAttributeValueType
    {
        Disciption,
        Name,
        ShortName,
        DisplayName,
        TableName,
        ColumnName,
        DefaultValue
    }

    public static class HxEnumHelper
    {
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }
        
        public static string GetAttributeValueToString(HxAttribute en, HxAttributeValueType caseType = HxAttributeValueType.Disciption)
        {
            string Result = en.ToString();
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(HxAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    //int n = attrs.Length;
                    switch (caseType)
                    {
                        case HxAttributeValueType.Disciption:
                            Result = ((HxAttribute)attrs[0])?.Description;
                            break;
                        case HxAttributeValueType.Name:
                            Result = ((HxAttribute)attrs[0])?.Name;
                            break;
                        case HxAttributeValueType.ShortName:
                            Result = ((HxAttribute)attrs[0])?.ShortName;
                            break;
                        case HxAttributeValueType.DisplayName:
                            Result = ((HxAttribute)attrs[0])?.DisplayName;
                            break;
                        case HxAttributeValueType.TableName:
                            Result = ((HxAttribute)attrs[0])?.TableName;
                            break;
                        case HxAttributeValueType.ColumnName:
                            Result = ((HxAttribute)attrs[0])?.ColumnName;
                            break;
                        case HxAttributeValueType.DefaultValue:
                            Result = ((HxAttribute)attrs[0])?.DefaultValue.ToStringEx();
                            break;
                        default:
                            //Result = en.ToString();
                            break;
                    }
                    
                }
            }
            return Result;
        }
    }

    /// <summary>
    /// Enum Helper Class
    /// </summary>
    /// <typeparam name="T">Enum</typeparam>
    public class HxEnumHelper<T> : HxBase
        where T : struct, IComparable, IFormattable, IConvertible
        //where T : struct, IComparable, IFormattable, IConvertible, //struct, IConvertible //struct
    {
        // Key : Enum Item Key
        // Name : Enum Item To String
        // Value : Item Enum Type
        // Description : Enum Item Name or Description To String

        /// <summary>
        /// Get Class Name
        /// </summary>
        public override string GetName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        }


        #region Static Intance
        private static HxEnumHelper<T> _instance = null;
        static HxEnumHelper()
        {
            _instance = new HxEnumHelper<T>();
        }
        public static HxEnumHelper<T> Instance
        {
            //get { return _instance ?? (_instance = new dnEnumHelper<T>()); }
            get { return _instance = new HxEnumHelper<T>(); }
            //private set { _instance = value; }
        }
        #endregion

        private Type _enumType = null;
        private HxEnumConverter _enumConverter = null;

        public HxEnumHelper()
        {
            this._enumType = typeof(T);
            if (this._enumType.BaseType != typeof(Enum))
                throw new HxNotAnEnumException();
            this._enumConverter = new HxEnumConverter(this._enumType);
        }

        /// <summary>
        /// 하나 이상의 열거된 상수의 이름이나 숫자 값의 문자열 표현을 해당하는 열거된 개체로 변환합니다. 매개 변수는 연산이 대/소문자를 구분하는지 여부를 지정합니다.
        /// </summary>
        /// <param name="value">변환할 이름이나 값이 포함된 문자열입니다.</param>
        /// <param name="ignoreCase">true이면 대/소문자를 무시하고, 그렇지 않으면 대/소문자를 구분합니다.</param>
        /// <returns>값이 value로 표현된 열거형 형식의 개체입니다.</returns>
        public object Parse(string value, bool ignoreCase = false)
        {
            return Enum.Parse(this._enumType, value, ignoreCase);
        }

        public int GetKey(T value)
        {
            return (int)this.GetKey(value.ToString());
        }

        public int GetKey(string name, bool ignoreCase = false)
        {
            return (int)this.Parse(name, ignoreCase);
        }

        public int[] GetKeys()
        {
            //Array arr = Enum.GetValues(this._enumType);
            Array arr = this.GetValues();
            //object[] objs = (object[])Enum.GetValues(this._enumType);
            int n = arr.Length;
            int[] Result = new int[n];
            for (int i = 0; i < n; i++)
            {
                string strName = arr.GetValue(i).ToString();
                Result[i] = (int)this.Parse(strName);
                //Result[i] = (int)arr.GetValue(i);
            }
            return Result;
        }

        public T GetValue(int key)
        {
            T Result = default(T);
            int[] keys = this.GetKeys();
            int index = -1;
            for (int i = 0; i < keys.Length; i++)
            {
                if (key == keys[i])
                {
                    index = i;
                    break;
                }
            }
            if (index > 0)
            {
                Array items = this.GetValues();
                object val = items.GetValue(index);
                Result = (T)val;
            }
            return Result;
        }

        public T GetValue(string value, bool ignoreCase = false)
        {
            return (T)this.Parse(value, ignoreCase);
        }
        /// <summary>
        /// 지정된 열거형에서 상수 값의 배열을 검색합니다.
        /// </summary>
        /// <returns>열거형에 있는 상수 값의 System.Array, 배열 요소는 열거형 상수의 이진 값을 기준으로 정렬</returns>
        public Array GetValues()
        {
            return Enum.GetValues(this._enumType);
        }

        /*
        /// <summary>
        /// 열거형의 상수 값 배열
        /// </summary>
        /// <returns></returns>
        public object[] GetValues()
        {
            //Array arr = Enum.GetValues(this._enumType);
            Array arr = this.GetValues();
            //object[] objs = (object[])Enum.GetValues(this._enumType);
            int n = arr.Length;
            object[] Result = new object[n];
            for (int i = 0; i < n; i++)
            {
                Result[i] = arr.GetValue(i);

            }
            return Result;
        }*/

        public string GetName(int key)
        {
            return Enum.GetName(this._enumType, key);
        }
        public string GetName(T value)
        {
            return Enum.GetName(this._enumType, value);
        }
        /// <summary>
        /// 열거형의 상수값 상수 이름 배열
        /// </summary>
        /// <returns></returns>
        public string[] GetNames()
        {
            string[] Result = Enum.GetNames(this._enumType);
            return Result;
        }

        public string GetDescription(int key)
        {
            int[] keys = this.GetKeys();
            int index = -1;
            for (int i = 0; i < keys.Length; i++)
            {
                if (key == keys[i])
                {
                    index = i;
                    break;
                }
            }
            if (index > 0)
            {
                return this._enumConverter.ConvertToString(key);
            }
            return null;

        }
        public string GetDescription(T value)
        {
            //Type type = typeof(System.ComponentModel.DescriptionAttribute);
            return this._enumConverter.ConvertToString(value);
        }
        public string GetDescription(string name)
        {
            string Result = null;
            try
            {
                T value = this.GetValue(name);
                Result = this._enumConverter.ConvertToString(value);
            }
            catch (Exception)
            {
                Result = null;
            }
            return Result;
        }

        public string[] GetDescriptions()
        {
            Array values = this.GetValues();
            string[] Result = new String[values.Length];
            int i = 0;
            foreach (T val in values)
            {
                Result[i] = this.GetDescription(val);
                i++;
            }
            return Result;
        }

        public List<string> ToValueList()
        {
            List<string> Result = new List<string>();
            Array arr = Enum.GetValues(this._enumType);
            for (int i = 0; i < arr.Length; i++)
            {
                string str = arr.GetValue(i).ToString();
                //int index = (int)Enum.Parse(typeof(dnMessageType), str);
                Result.Add(str);
            }
            //var a = Enum.GetValues(typeof(dnCore.dnDatabaseType)).Cast<dnCore.dnDatabaseType>().ToList();
            return Result;
        }

        public List<int> ToKeyList()
        {
            List<int> Result = new List<int>();
            Array arr = Enum.GetValues(this._enumType);
            for (int i = 0; i < arr.Length; i++)
            {
                string str = arr.GetValue(i).ToString();
                int key = (int)Enum.Parse(this._enumType, str);
                Result.Add(key);
            }
            return Result;
        }

        public Dictionary<int, string> ToList()
        {
            Dictionary<int, string> Result = new Dictionary<int, string>();
            Array arr = Enum.GetValues(this._enumType);
            for (int i = 0; i < arr.Length; i++)
            {
                string val = arr.GetValue(i).ToString();
                int key = (int)Enum.Parse(this._enumType, val);
                Result.Add(key, val);
            }

            return Result;
        }
        /*
        public object[] ToDescriptionList()
        {
            Dictionary<int, string> Result = new Dictionary<int, string>();
            Array arr = Enum.GetValues(this._enumType);
            for (int i = 0; i < arr.Length; i++)
            {
                string val = arr.GetValue(i).ToString();
                int key = (int)Enum.Parse(this._enumType, val);
                //string caption = this._enumType
                Result.Add(key, val);
            }
            System.Reflection.FieldInfo[] = _enumType.GetFields();
            //return Result;
        }*/




        /// <summary>
        /// Type이 Enum 형태가 아닐 경우 사용되는 Exception Class
        /// </summary>
        public class HxNotAnEnumException : Exception
        {
            public HxNotAnEnumException() : base(string.Format(@"Type ""{0}"" is not an Enum type.", typeof(T))) { }
        }


    }
}
