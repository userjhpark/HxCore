using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    internal struct HxRecords
    {
        string _name;
        public HxRecords(bool bInit = false)
        {
            _name = string.Empty;
        }
    }
    // TODO: (2023.10.23) - HxQueryStringBindRecord
    public struct HxTextKeyValuePairsRec
    {
        public string TextStr;
        public Dictionary<string, object> KeyStrValueObjs;
        public string Remark;
        public int? SeqNo;

        public HxTextKeyValuePairsRec(bool bInit = false)
        {
            TextStr = null;
            KeyStrValueObjs = null;
            Remark = null;
            SeqNo = null;
            if (bInit == true)
            {
                SeqNo = -1;
                TextStr = string.Empty;
                KeyStrValueObjs = new Dictionary<string, object>();
                Remark = string.Empty;
            }
        }

        public HxTextKeyValuePairsRec(string textStr, Dictionary<string, object> keyValues = null, string remark = null, int? seqNo = null)
        {
            TextStr = textStr;
            KeyStrValueObjs = keyValues;
            Remark = remark;
            SeqNo = seqNo;
        }
    }
    public struct HxRequestSuccessCountRec
    {
        public int? RequestCount;
        
        public List<int> SuccessList;
        public string Remark;
        public bool? IsError;
        public string ErrorMessage;
        public int? SuccessCount => SuccessList?.Count;
        public bool? IsSuccess 
        {
            get
            {
                if(RequestCount.IsNullOrMinorEx() != true)
                {
                    if(RequestCount == SuccessCount.ToIntEx(0))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public HxRequestSuccessCountRec(bool bInit = false)
        {
            RequestCount = null;
            //SuccessCount = null;
            SuccessList = null;
            Remark = null;
            IsError = null;
            ErrorMessage = null;
            if (bInit == true)
            {
                RequestCount = -1;
                //SuccessCount = -1;
                SuccessList = new List<int>();
                Remark = string.Empty;
                IsError = false;
                ErrorMessage = string.Empty;
            }
        }
        public HxRequestSuccessCountRec(int? requestCount, int? successCount = 0, string remark = null)
            : this(false)
        {
            SetRequestCount(requestCount);
            SetSuccessCount(successCount);
            SetRemark(remark);
        }
        public void SetRequestCount(int? requestCount)
        {
            RequestCount = requestCount;
        }
        public void SetSuccessNumberAdd(int value)
        {
            if (SuccessList == null) { SuccessList = new List<int>(); }
            SuccessList.Add(value);
        }
        public void AddSuccessNumber(int value)
        {
            SetSuccessNumberAdd(value);
        }
        private void SetSuccessCount(int? successCount)
        {
            //SuccessCount = successCount;
        }
        
        public void SetRemark(string remark)
        {
            Remark = remark;
        }
        public void SetError(bool? bError, string errorMessage = null)
        {
            IsError = bError;
            ErrorMessage = errorMessage;
        }
    }
}
