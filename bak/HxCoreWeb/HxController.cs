using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;

namespace HxCore.Web
{
    public class HxController : Controller
    {
        public bool IsCreated { get; protected set; }

        

        //public string RemoteIP { get => GetRemoteIP(); }

        protected string GetRemoteIP()
        {
            return HxWebUtils.GetRemoteIP(this.Request);
        }

        protected virtual void Init(bool bInit = false)
        {
            if (this.IsCreated != true || bInit == true)
            {
                this.IsCreated = false;
                //this.SBCommonObj = new TCommon();
                //this.SBCommonObj = TCommon.Create();
            }
        }


        #region Request 관련

        private Dictionary<string, StringValues> GetQueryString(IQueryCollection queryObject = null)
        {
            Dictionary<string, StringValues> Result = null;
            if(queryObject == null)
            {
                queryObject = this.Request.Query;
            }

            if(queryObject != null && queryObject.Count > 0)
            {
                Result = new Dictionary<string, StringValues>();
                foreach(KeyValuePair<string, StringValues> key in queryObject)
                {
                   //esult += " " + 
                }
            }
            return Result;
        }

        private Dictionary<string, string> ConvertToDictionary(FormDataCollection formData)
        {
            //System.Web.HttpContext.Current
            Dictionary<string, string> Result = null;
            if (formData != null)
            {
                Result = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> item in formData)
                {
                    Result.AddEx(item.Key, item.Value);
                }
            }
            return Result;
        }

        protected virtual IHeaderDictionary GetRequestHeaders()
        {
            IHeaderDictionary Result = null;
            if (this.Request != null && this.Request.Headers != null)
            {
                Result = this.Request.Headers;
            }
            return Result;
        }

        protected virtual IHeaderDictionary GetRequestHeaderData()
        {
            return this.GetRequestHeaders();
        }

        protected virtual IQueryCollection GetRequestQueryData()
        {
            IQueryCollection Result = null;
            if (this.Request != null && this.Request.Query != null)
            {
                Result = this.Request.Query;
            }
            return Result;
        }

        protected virtual QueryString GetRequestQueryStringData()
        {
            if (this.Request != null && this.Request.QueryString != null)
            {
                return this.Request.QueryString;
            }
            return QueryString.Empty;
        }
        protected virtual string GetRequestQueryStr()
        {
            if (this.Request != null && GetRequestQueryStringData() != QueryString.Empty)
            {
                return GetRequestQueryStringData().ToString();
            }
            return null;
        }

        protected virtual IFormCollection GetRequestFormData()
        {
            IFormCollection Result = null;
            if (this.Request != null && this.Request.Method.IsNullOrWhiteSpaceEx() != true && (this.Request.Method.ToUpper() == "POST" || this.Request.Method.ToUpper() == "PUT" || this.Request.Method.ToUpper() == "PATCH") ) //&& header.ContainsKey("Content-Type")
            {
                if (this.Request.Form?.Count > 0)
                {
                    Result = this.Request.Form;
                }
            }
            return Result;
        }

        protected virtual Dictionary<string, string> GetRequestValueData(bool bHeader = false, bool bSingleValue = true, HxMultiplePosition position = HxMultiplePosition.None)
        {
            Dictionary<string, string> Result = null;
            IHeaderDictionary header = bHeader == true ? GetRequestHeaderData() : null;
            IQueryCollection query = GetRequestQueryData();
            IFormCollection form = GetRequestFormData();
            
            if(header != null || form != null || query != null)
            {
                Result = new Dictionary<string, string>();
                if(header != null && header.Count > 0)
                {
                    foreach (KeyValuePair<string, StringValues> item in header)
                    {
                        if (bSingleValue == true)
                        {
                            Result.AddEx(item.Key, item.Value.ToStringSingleEx(position), true);
                        }
                        else
                        {
                            Result.AddEx(item.Key, item.Value.ToStringEx(), true);
                        }
                    }
                }

                if(query != null && query.Count > 0)
                {
                    foreach (KeyValuePair<string, StringValues> item in query)
                    {
                        if (bSingleValue == true)
                        {
                            Result.AddEx(item.Key, item.Value.ToStringSingleEx(position), true);
                        }
                        else
                        {
                            Result.AddEx(item.Key, item.Value.ToStringEx(), true);
                        }
                    }
                }

                if (form != null && form.Count > 0)
                {
                    foreach (KeyValuePair<string, StringValues> item in form)
                    {
                        if (bSingleValue == true)
                        {
                            Result.AddEx(item.Key, item.Value.ToStringSingleEx(position), true);
                        }
                        else
                        {
                            Result.AddEx(item.Key, item.Value.ToStringEx(), true);
                        }
                    }
                }
            }
            return Result;
        }



        protected virtual string GetCustomUserAgentString(IHeaderDictionary header = null)
        {
            if (header == null)
            {
                header = GetRequestHeaders();
            }
            if (header != null && header.ContainsKey(HxBase._CUSTOM_USER_AGENT_))
            {
                return header[HxBase._CUSTOM_USER_AGENT_];
            }
            return null;
        }

        protected virtual void SetHeaderCustomValuesToExtendedProperties(DataTable data, IHeaderDictionary header = null)
        {
            if (data != null)
            {
                if (header == null)
                {
                    header = GetRequestHeaders();
                }
                //if(data.ExtendedProperties.ContainsKey("user-agent"))
                //HxUtils.GetUserCustomAgent();
                //DataTable dt = this.Getr
            }
        }
    #endregion

}
}
