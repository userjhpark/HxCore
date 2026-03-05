using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HxCore
{
   [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class HxAttribute : Attribute
    {
        public HxDataType DataType { get; set; }
        protected int _DEFAULT_MAX_LENGTH_ = 255;
        protected int _DEFAULT_MIN_LENGTH_ = 1;

        private string _description = null;
        public string Description { get => _description; set => _description = value; }

        //**ADDED**
        

        private bool _required = false;
        public bool Required { get => _required; set => _required = value; }
        
        public string Name { get => Description; set => Description = value; }
        public string GroupName { get; set; }
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public bool _isUnique = false;
        public bool IsUnique { get => _isUnique; set => _isUnique = value; }
        private bool _isKey = false;
        public bool IsKey { get => _isKey; set => _isKey = value; }
        private bool _isAutoIncrement = false;
        public bool AutoIncrement { get => _isAutoIncrement; set => _isAutoIncrement = value; }
        private int _autoIncrementSeed = 0;
        public int AutoIncrementSeed { get => _autoIncrementSeed; set => _autoIncrementSeed = value; }
        private bool _isUse = false;
        public bool IsUse { get => _isUse; set => _isUse = value; }
        private bool _isCustom = false;
        public bool IsCustom { get => _isCustom; set => _isCustom = value; }
        private bool _isNotNull = false;
        public bool IsNotNull { get => _isNotNull; set => _isNotNull = value; }
        public bool IsReadOnly { get; set; }
        public object DefaultValue { get; set; }
        public Type DefaultDataType { get; set; }
        public string DefaultFormatString { get; set; }
        public string ExtraInfo { get; set; }
        private bool _extraGridHidden = false;
        public bool ExtraGridHidden { get { return _extraGridHidden; } set { _extraGridHidden = value; } }
        private HxCryptType _valueCryptType = HxCryptType.None;
        public HxCryptType ValueCryptType { get { return _valueCryptType; } set { _valueCryptType = value; } }
        public string CodeKeyValueSet { get; set; }

        public int? Order { get; set; }
        public string Remark { get; set; }

        private int _maximumLength = int.MinValue;
        public int MaximumLength { get => _maximumLength; set => _maximumLength = value; }
        // public uint? StringLength { get; set; }
        private int _minimumLength = int.MinValue;
        public int MinimumLength { get => _minimumLength; set => _minimumLength = value; }
        
        public string FormatErrorMessage { get; set; }
        public string FormatString { get; set; }
        public string ErrorMessage { get; protected set; }
        //public Uint? 



        public HxAttribute()
        {
            Init();
        }
        public HxAttribute(string description)
        {
            Description = description;
            Init();
        }
        public HxAttribute(string description, string columnName)
        {
            Description = description;
            ColumnName = columnName;
            Init();
        }

        //public HxAttribute(string description, int maximumLength, string formatErrorMessage = null)
        //{
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}


        //public HxAttribute(string description, int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(int maximumLength, string formatErrorMessage = null)
        //{
        //    MaximumLength = maximumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}


        //public HxAttribute(bool required, string description)
        //{
        //    Required = required;
        //    Description = description;
        //    Init();
        //}

        //public HxAttribute(bool required, string description, int maximumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}


        //public HxAttribute(bool required, string description, int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(bool required, int maximumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    MaximumLength = maximumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(bool required, int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}


        //public HxAttribute(HxDataType dataType, string description)
        //{
        //    DataType = dataType;
        //    Description = description;
        //    Init();
        //}

        //public HxAttribute(HxDataType dataType, string description, int maximumLength, string formatErrorMessage = null)
        //{
        //    DataType = dataType;
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}


        //public HxAttribute(HxDataType dataType, string description, int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    DataType = dataType;
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(HxDataType dataType, int? maximumLength = null, string formatErrorMessage = null)
        //{
        //    DataType = dataType;
        //    MaximumLength = maximumLength.ToIntEx();
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(HxDataType dataType, int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    DataType = dataType;
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}



        //public HxAttribute(bool required, HxDataType dataType, string description)
        //{
        //    Required = required;
        //    DataType = dataType;
        //    Description = description;
        //    Init();
        //}

        //public HxAttribute(bool required, HxDataType dataType, string description, int maximumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    DataType = dataType;
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}


        //public HxAttribute(bool required, HxDataType dataType, string description, int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    DataType = dataType;
        //    Description = description;
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(bool required, HxDataType dataType, int maximumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    DataType = dataType;
        //    MaximumLength = maximumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(bool required, HxDataType dataType, int maximumLength, int minimumLength, string formatErrorMessage = null)
        //{
        //    Required = required;
        //    DataType = dataType;
        //    MaximumLength = maximumLength;
        //    MinimumLength = minimumLength;
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}

        //public HxAttribute(string description, bool required, HxDataType dataType, int? maximumLength = null, int? minimumLength = null, string formatErrorMessage = null)
        //{
        //    Description = description;
        //    Required = required;
        //    DataType = dataType;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}
        //public HxAttribute(string description, HxDataType dataType, int? maximumLength = null, int? minimumLength = null, string formatErrorMessage = null)
        //{
        //    Description = description;
        //    DataType = dataType;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}
        //public HxAttribute(string description, bool required, int? maximumLength = null, int? minimumLength = null, string formatErrorMessage = null)
        //{
        //    Description = description;
        //    Required = required;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}
        //public HxAttribute(string description, HxDataType dataType, bool required, int? maximumLength = null, int? minimumLength = null, string formatErrorMessage = null)
        //{
        //    Description = description;
        //    DataType = dataType;
        //    Required = required;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    FormatErrorMessage = formatErrorMessage;
        //    Init();
        //}


        //public HxAttribute(string description, bool required, HxDataType dataType, string formatErrorMessage = null, int? maximumLength = null, int? minimumLength = null)
        //{
        //    Description = description;
        //    Required = required;
        //    DataType = dataType;
        //    FormatErrorMessage = formatErrorMessage;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    Init();
        //}
        //public HxAttribute(string description, HxDataType dataType, string formatErrorMessage = null, int? maximumLength = null, int? minimumLength = null)
        //{
        //    Description = description;
        //    //Required = required;
        //    DataType = dataType;
        //    FormatErrorMessage = formatErrorMessage;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    Init();
        //}
        //public HxAttribute(string description, bool required, string formatErrorMessage = null, int? maximumLength = null, int? minimumLength = null)
        //{
        //    Description = description;
        //    Required = required;
        //    //DataType = dataType;
        //    FormatErrorMessage = formatErrorMessage;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    Init();
        //}
        //public HxAttribute(string description, string formatErrorMessage, int? maximumLength = null, int? minimumLength = null)
        //{
        //    Description = description;
        //    //Required = required;
        //    //DataType = dataType;
        //    FormatErrorMessage = formatErrorMessage;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    Init();
        //}
        //public HxAttribute(string description, int maximumLength, string formatErrorMessage, int? minimumLength = null)
        //{
        //    Description = description;
        //    //Required = required;
        //    //DataType = dataType;
        //    FormatErrorMessage = formatErrorMessage;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    Init();
        //}
        //public HxAttribute(string description, bool required, int maximumLength, string formatErrorMessage, int? minimumLength = null)
        //{
        //    Description = description;
        //    Required = required;
        //    //DataType = dataType;
        //    FormatErrorMessage = formatErrorMessage;
        //    MaximumLength = maximumLength.ToIntEx();
        //    MinimumLength = minimumLength.ToIntEx();
        //    Init();
        //}


        protected virtual void Init()
        {
            if(Required == true)
            {
                if(MaximumLength <= 0)
                {
                    MaximumLength = _DEFAULT_MAX_LENGTH_;
                }
                if(MinimumLength <= 0)
                {
                    MinimumLength = _DEFAULT_MIN_LENGTH_;
                }
            }
        }

        public bool IsValid(object value, string name = null)
        {
            bool Result = true;
            try
            {
                if (value.IsNullOrWhiteSpaceEx())
                {
                    if (Required == true)
                    {
                        Result = false;
                    } else
                    {
                        Result = true;
                    }
                }
                else
                {
                    if(value.GetType() == typeof(string))
                    {
                        string strValue = value.ToStringEx();
                        if(MaximumLength <= strValue.Length)
                        {
                            Result = false;
                        }
                        else if(MinimumLength >= strValue.Length)
                        {
                            Result = false;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                //throw;
            }
            if(Result == false)
            {
                ErrorMessage = GetErrorMessage(FormatErrorMessage, name);
            }
            return Result;
        }

        protected string GetErrorMessage(string formatErrorMessage = null, string name = null)
        {
            string Result = null;
            try
            {
                if (formatErrorMessage.IsNullOrWhiteSpaceEx() && !FormatErrorMessage.IsNullOrWhiteSpaceEx())
                {
                    formatErrorMessage = FormatErrorMessage;
                }
                if (!formatErrorMessage.IsNullOrWhiteSpaceEx())
                {
                    if (name.IsNullOrWhiteSpaceEx() && !Name.IsNullOrWhiteSpaceEx())
                    {
                        name = Name;
                    }
                    Result = string.Format(formatErrorMessage, name, MaximumLength, MinimumLength);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                //throw;
            }
            
            return Result;
        }

        public static TAttribute GetAttribute<TAttribute>(Enum @enum)
            where TAttribute : Attribute
        {
            return @enum?.GetType()?.GetMember(@enum.ToString())?.FirstOrDefault()?.GetCustomAttribute<TAttribute>();
        }
    }

}