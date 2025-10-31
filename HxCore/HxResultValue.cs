using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HxCore
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HxResultValue
    {
        [JsonProperty("result_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HxResultType ResultType = HxResultType.None;

        private object _value;

        [JsonProperty("value")]
        public object Value {
            get => _value;
            set
            {
                SetObjectValue(value);
            }
        }

        
        [JsonProperty("value_type")]
        public string ValueType
        {
            get; set;
            //get
            //{
            //    if (Value != null)
            //    {
            //        if (Value is DataSet)
            //        {
            //            return "DataSet";
            //        }
            //        else if (Value is DataTable)
            //        {
            //            return "DataTable";
            //        }
            //        else if (Value is DataView)
            //        {
            //            return "DataView";
            //        }
            //        else if (Value is Array)
            //        {
            //            return "Array";
            //        }
            //        else if (Value is List<object>)
            //        {
            //            return "Array";
            //        }
            //        else if (Value is IEnumerable<object>)
            //        {
            //            return "Array";
            //        }
            //        else if(Value is int || Value is uint)
            //        {
            //            return "Int";
            //        }
            //        else if(Value is Decimal || Value is float)
            //        {
            //            return "Number";
            //        }
            //        else if(Value is bool)
            //        {
            //            return "Bool";
            //        }
            //        else if(Value is string)
            //        {
            //            return "String";
            //        }
            //    }
            //    return null;
            //}
            
        }

        [JsonProperty("success")]
        public bool? Success
        {
            get
            {
                bool? r;
                switch (ResultType)
                {
                    case HxResultType.Success:
                        r = true;
                        break;
                    case HxResultType.Fail:
                    case HxResultType.Exception:
                        r = false;
                        break;
                    case HxResultType.None:
                    default:
                        r = null;
                        break;
                }
                if(r == null && Value != null)
                {
                    r = true;
                }
                return r;
            }
            set
            {
                if (value != null)
                {
                    if (value == true)
                    {
                        ResultType = HxResultType.Success;
                    }
                    else
                    {
                        ResultType = HxResultType.Fail;
                    }
                }
                else
                {
                    ResultType = HxResultType.None;
                }
            }
        }

        [JsonProperty("message_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HxResultMessageType MessageType = HxResultMessageType.None;
        [JsonProperty("detail_message")]
        public string DetailMessage = null;

        [JsonProperty("count")]
        public int? Count
        {
            get
            {
                int? Result;
                try
                {
                    Result = -1;
                    if (Value != null)
                    {
                        if (Value is DataSet ds)
                        {
                            return ds.Tables.Count.ToNullableIntEx(-1);
                        }
                        else if (Value is DataTable dt)
                        {
                            return dt.Rows.Count.ToNullableIntEx(-1);
                        }
                        else if (Value is DataView dv)
                        {
                            return dv.Count.ToNullableIntEx(-1);
                        }
                        else if (Value is Array arr)
                        {
                            return arr.Length.ToNullableIntEx(-1);
                        }
                        else if (Value is List<object> list)
                        {
                            return list.Count.ToNullableIntEx(-1);
                        }
                        else if (Value is IEnumerable<object> val)
                        {
                            return val.Count().ToNullableIntEx(-1);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    Result = int.MinValue;
                    //throw;
                }
                return Result;
            }
        }
        
        [JsonProperty("value2")]
        public object Value2 = null;

        //public string Text = null;
        //public IEnumerable<T> List = null;
        //public DataTable Data = null;
        public HxResultValue()
            : this(true)
        {
            ; ;
        }
        public HxResultValue(bool bInit = false)
        {
            if (bInit != true)
            {
                this.Success = false;
                this.ResultType = HxResultType.None;
                this.MessageType = HxResultMessageType.None;
                this.DetailMessage = null;
                this.Value = null;
                this.Value2 = null;
                //this.Count = 0;
                //this.ValueType = null;
            }
        }

        public HxResultValue(DataTable data)
            : this()
        {
            this.Value = data?.Copy();
        }

        public HxResultValue(object value)
            : this()
        {
            this.Value = value;
        }

        public void SetObjectValue(object pValue)
        {
            _value = pValue;
            
            this.ResultType = HxResultType.None;

            string strType = _value?.GetType().Name;
            try
            {
                if (_value != null)
                {
                    if (_value is DataSet)
                    {
                        strType = "DataSet";
                    }
                    else if (_value is DataTable)
                    {
                        strType = "DataTable";
                    }
                    else if (_value is DataView)
                    {
                        strType = "DataView";
                    }
                    else if (_value is Array || _value is ArrayList)
                    {
                        strType = "Array";
                    }
                    else if (_value is IDictionary)
                    {
                        strType = "Dictionary";
                    }
                    else if (_value is IList)
                    {
                        strType = "List";
                    }
                    else if (_value is List<Dictionary<string, object>>)
                    {
                        strType = "List";
                    }
                    else if (_value is List<System.Collections.DictionaryBase>)
                    {
                        strType = "List";
                    }
                    else if (_value is IList<Object>)
                    {
                        strType = "List";
                    }
                    else if (_value is System.Collections.DictionaryBase)
                    {
                        strType = "Dictionary";
                    }
                    else if (_value is ICollection)
                    {
                        strType = "array";
                    }
                    else if (_value is IEnumerable<object>)
                    {
                        strType = "array";
                    }
                    else if (_value is int || _value is uint || _value is Int16 || _value is Int64)
                    {
                        strType = "Int";
                    }
                    else if (_value is decimal || _value is float || _value is double)
                    {
                        strType = "Number";
                    }
                    else if (_value is bool)
                    {
                        strType = "Bool";
                    }
                    else if (_value is string)
                    {
                        strType = "String";
                    }
                    else if (_value is DateTime)
                    {
                        strType = "DateTime";
                    }
                    else if (strType.EndsWith("Rec") == true)
                    {
                        strType = "Record";
                    }
                    else if (strType.EndsWith("Rec[]") == true)
                    {
                        strType = "RecordSet";
                    }
                }

                ValueType = strType ?? null;

                if (this._value != null && strType.IsNullOrWhiteSpaceEx() != true)
                {
                    this.ResultType = HxResultType.Success;
                }
                else
                {
                    this.ResultType = HxResultType.Fail;
                }
            }
            catch (Exception ex)
            {
                this._value = ex.Message;
                this.ValueType = "Exception";
                this.ResultType = HxResultType.Exception;
                //throw;
            }
            
        }

        public void SetValue(DataTable data)
        {

            this.Value = data?.Copy();
        }

        public void SetValue(string value, HxResultType? resultType = null) 
        { 
            this.Value = value;
            if(resultType != null)
            {
                ResultType = resultType.Value;
            }
        }

        public void SetException(Exception ex)
        {
            this.Success = false;
            this.MessageType = HxResultMessageType.Exception;
            this.DetailMessage = ex.Message;
            this.Value = null;
        }

        public static HxResultValue Exception(Exception ex, bool bInit = true)
        {
            HxResultValue Result = new HxResultValue(bInit);
            Result.SetException(ex);
            return Result;
        }

        public override string ToString()
        {
            return HxUtils.JsonSerializeObject(this);
        }
    }

    public class HxResultValue<T> : HxResultValue
        where T : IHxSetValue
    {

        [JsonProperty("value")]
        public new List<T> Value = null;

        //public string Text = null;
        //public IEnumerable<T> List = null;
        //public DataTable Data = null;

        public HxResultValue(bool bInit = false)
            : base (bInit)
        {
            ; ;
        }
    }

    //public class HxResultValue<T>
    //{
    //    [JsonProperty("result")]
    //    public HxResultType Result = HxResultType.None;
    //    [JsonProperty("message_type")]
    //    public HxResultMessageType MessageType = HxResultMessageType.None;
    //    [JsonProperty("detail_message")]
    //    public string DetailMessage = null;

    //    [JsonProperty("value")]
    //    public IEnumerable<T> Value = null;
    //    [JsonProperty("value2")]
    //    public object Value2 = null;
    //    //public string Text = null;
    //    //public IEnumerable<T> List = null;
    //    //public DataTable Data = null;

    //    [JsonProperty("success")]
    //    public bool? Success
    //    {
    //        get
    //        {
    //            bool? r = null;
    //            switch (Result)
    //            {
    //                case HxResultType.Success:
    //                    r = true;
    //                    break;
    //                case HxResultType.Fail:
    //                case HxResultType.Exception:
    //                    r = false;
    //                    break;
    //                case HxResultType.None:
    //                default:
    //                    r = null;
    //                    break;
    //            }
    //            return r;
    //        }
    //        set
    //        {
    //            if (value != null)
    //            {
    //                if (value == true)
    //                {
    //                    Result = HxResultType.Success;
    //                }
    //                else
    //                {
    //                    Result = HxResultType.Fail;
    //                }
    //            }
    //            else
    //            {
    //                Result = HxResultType.None;
    //            }
    //        }
    //    }
    //    [JsonProperty("count")]
    //    public int? Count
    //    {
    //        get
    //        {
    //            if (Value != null)
    //            {
    //                if (Value is DataSet ds)
    //                {
    //                    return ds.Tables.Count.ToNullableIntEx(-1);
    //                }
    //                else if (Value is DataTable dt)
    //                {
    //                    return dt.Rows.Count.ToNullableIntEx(-1);
    //                }
    //                else if (Value is DataView dv)
    //                {
    //                    return dv.Count.ToNullableIntEx(-1);
    //                }
    //                else if (Value is Array arr)
    //                {
    //                    return arr.Length.ToNullableIntEx(-1);
    //                }
    //                else if (Value is List<object> list)
    //                {
    //                    return list.Count.ToNullableIntEx(-1);
    //                }
    //                else if (Value is IEnumerable<object> val)
    //                {
    //                    return val.Count().ToNullableIntEx(-1);
    //                }
    //            }
    //            return int.MinValue;
    //        }
    //    }
    //    [JsonProperty("value_type")]
    //    public string ValueType
    //    {
    //        get
    //        {
    //            if (Value is DataSet)
    //            {
    //                return "DataSet";
    //            }
    //            else if (Value is DataTable)
    //            {
    //                return "DataTable";
    //            }
    //            else if (Value is DataView)
    //            {
    //                return "DataView";
    //            }
    //            else if (Value is Array)
    //            {
    //                return "Array";
    //            }
    //            else if (Value is List<object>)
    //            {
    //                return "List";
    //            }
    //            else if (Value is IEnumerable<object>)
    //            {
    //                return "IEnumerable";
    //            }
    //            return null;
    //        }
    //    }

    //    public void SetException(Exception ex)
    //    {
    //        this.Success = false;
    //        this.MessageType = HxResultMessageType.Exception;
    //        this.DetailMessage = ex.Message;
    //        this.Value = null;
    //    }
    //}
}
