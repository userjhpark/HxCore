using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

using HxCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;
using System.Data;
using System.Diagnostics;
using System.Net.Http;

namespace HxCore.Web
{
    public class HxWebUtils :HxBase
    {
        public const string _X_FORWARDED_FOR_ = "X-Forwarded-For";
        //public const string _USER_AGENT_ = "USER-AGENT";
        //public const string _REMOTE_ADDR_ = "REMOTE_ADDR";
        //public const string _HTTP_REFERER_ = "HTTP_REFERER";
        //public const string _HTTP_HOST_ = "HTTP_HOST";

        public static string GetRemoteIP(HttpRequest request, bool bTryUseXForwardFor = true)
        {

            #region Startup.cs 추가 항목
            /*
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                //참조 : https://stackoverflow.com/questions/35441521/remoteipaddress-is-always-null?noredirect=1&lq=1
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });
            */
            #endregion

            //참고 https://stackoverflow.com/questions/28664686/how-do-i-get-client-ip-address-in-asp-net-core
            string Result = null;

            if (request != null)
            {

                // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

                // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
                // for 99% of cases however it has been suggested that a better (although tedious)
                // approach might be to read each IP from right to left and use the first public IP.
                // http://stackoverflow.com/a/43554000/538763
                //
                if (bTryUseXForwardFor)
                    Result = GetHeaderValue<string>(request, _X_FORWARDED_FOR_).SplitCsvEx().FirstOrDefault();

                // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
                if (Result.IsNullOrWhiteSpaceEx() && request?.HttpContext?.Connection?.RemoteIpAddress != null)
                    Result = request.HttpContext.Connection.RemoteIpAddress.ToString();

                if (Result.IsNullOrWhiteSpaceEx() && request?.HttpContext?.Features?.Get<IHttpConnectionFeature>()?.RemoteIpAddress != null)
                    Result = request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString(); //.< IHttpConnectionFeature > ()?.RemoteIpAddress

                if (Result.IsNullOrWhiteSpaceEx())
                    Result = GetHeaderValue<string>(request, _REMOTE_ADDR_);

                // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

                if (Result.IsNullOrWhiteSpaceEx())
                    throw new Exception("Unable to determine caller's IP.");
            }

            return Result;




            //System.Web.HttpContext context = System.Web.HttpContext.Current;
            //string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (!string.IsNullOrEmpty(ipAddress))
            //{
            //    string[] addresses = ipAddress.Split(',');
            //    if (addresses.Length != 0)
            //    {
            //        return addresses[0];
            //    }
            //}
            //return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        public static string GetRemoteIP(HttpContext context, bool bTryUseXForwardFor = true)
        {
            return GetRemoteIP(context?.Request, bTryUseXForwardFor);
        }
        public static string USER_AGENT(HttpRequest request)
        {
            return GetHeaderValue(request, _USER_AGENT_);
        }

        public static string USER_AGENT(DataTable postData)
        {
            return postData.GetSingleLastValueEx<string>(_USER_AGENT_);
        }

        public static string REMOTE_ADDR(HttpRequest request)
        {
            string Result = null;
            try
            {
                Result = GetRemoteIP(request);
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    return GetHeaderValue(request, _REMOTE_ADDR_);
                }
            }
            catch (Exception)
            {
                //throw;
            }
            

            return Result;
        }
        public static string REMOTE_ADDR(DataTable postData)
        {
            return postData.GetSingleLastValueEx<string>(_REMOTE_ADDR_);
        }
        public static string HTTP_REFERER(HttpRequest request)
        {
            return GetHeaderValue(request, _HTTP_REFERER_);
        }
        public static string HTTP_REFERER(DataTable postData)
        {
            return postData.GetSingleLastValueEx<string>(_HTTP_REFERER_);
        }
        public static string HTTP_HOST(HttpRequest request)
        {
            return GetHeaderValue(request, _HTTP_HOST_);
        }
        public static string HTTP_HOST(DataTable postData)
        {
            return postData.GetSingleLastValueEx<string>(_HTTP_HOST_);
        }

        #region Request Header/Get/Post 처리
        public static bool CreateRequestRecordSet(HttpContext context, out DataTable recordSet, bool bExceptionThrow = true)
        {
            recordSet = null;
            HttpRequest request = context?.Request;
            return CreateRequestRecordSet(request, recordSet, bExceptionThrow);
        }
        public static bool CreateRequestRecordSet(HttpRequest request, DataTable recordSet, bool bExceptionThrow = true)
        {
            //DataTable Result = new DataTable();
            //Result.Columns.AddRange(new DataColumn[]{
            //    new DataColumn {ColumnName = "type", DataType = typeof(string)}
            //    , new DataColumn{ ColumnName = "key", DataType = typeof(string)}
            //    , new DataColumn { ColumnName = "value", DataType = typeof(object) }
            //    , new DataColumn { ColumnName = "values", DataType = typeof(object) }
            //});
            //return Result;
            bool Result = false;
            recordSet = null;
            try
            {
                recordSet = new DataTable();
                recordSet.Columns.AddRange(new DataColumn[]{
                      new DataColumn { ColumnName = "no", DataType = typeof(int), AutoIncrement = true, AutoIncrementSeed = 1}

                    , new DataColumn { ColumnName = "type", DataType = typeof(string), DefaultValue = "NONE" } //GET, POST, HEAD, FILE
                    , new DataColumn { ColumnName = "key", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "value", DataType = typeof(string) }

                    , new DataColumn { ColumnName = "file", DataType = typeof(System.Net.Http.MultipartFileData) }
                    , new DataColumn { ColumnName = "file_type", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "file_name", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "file_save", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "file_size", DataType = typeof(long), DefaultValue = -1 }

                    , new DataColumn { ColumnName = "name", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "remark", DataType = typeof(string) }
                    , new DataColumn { ColumnName = "is_array", DataType = typeof(bool)}
                    , new DataColumn { ColumnName = "is_use", DataType = typeof(bool), DefaultValue = true }
                    //, new DataColumn { ColumnName = "count", DataType = typeof(int), DefaultValue=1 }
                    , new DataColumn { ColumnName = "create_date", DataType = typeof(DateTime), DefaultValue=DateTime.Now }
                    , new DataColumn { ColumnName = "update_date", DataType = typeof(DateTime), DefaultValue=DateTime.Now }
                });
                SetUserCustomAgent(request, recordSet, true);
                Result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = false;
                if (bExceptionThrow == true)
                {
                    throw ex;
                }
            }
            return Result;
        }

        public static string GetUserCustomAgent(HttpRequest request)
        {
            string userAgent = GetHeaderValue(request, _HTTP_USER_AGENT_);
            string remoteAddr = GetRemoteIP(request); //GetHeaderValue<string>(request, "REMOTE_ADDR");
            string httpReferer = GetHeaderValue(request, _HTTP_REFERER_);
            string httpHost = GetHeaderValue(request, _HTTP_HOST_);

            string Result = userAgent;

            if (!remoteAddr.IsNullOrWhiteSpaceEx())
            {
                Result = string.Format("[{0}]{1}", remoteAddr, Result);
            }
            if (!httpReferer.IsNullOrWhiteSpaceEx())
            {
                Result = string.Format("{0} / HTTP_REFERER : {1}", Result, httpReferer);
            }
            if (!httpHost.IsNullOrWhiteSpaceEx())
            {
                Result = string.Format("{0} / HTTP_HOST : {1}", Result, httpHost);
            }
            return Result;
        }

        /// <summary>
        /// DataTable Resource에 HttpRequest 정보를 ExtendedProperties에 추가하고, 옵션에 따라 DataRow로 추가
        /// </summary>
        /// <param name="request">HttpRequest Resource</param>
        /// <param name="dataTable">DataTable Resource</param>
        /// <param name="bStructMachedToDataRowAppend">정해진 구조를 가진 DataTable일 경우 Datarow로 추가 옵션</param>
        /// <returns>DataRow 추가 여부?</returns>
        private static bool SetUserCustomAgentToExtendedProperties(HttpRequest request, DataTable dataTable, bool bStructMachedToDataRowAppend = false)
        {
            bool Result = false;
            if (request != null && dataTable != null)
            {
                string userAgent = GetHeaderValue(request, _HTTP_USER_AGENT_);
                string customAgent = userAgent;
                if (userAgent.IsNullOrWhiteSpaceEx() != true)
                {
                    dataTable.SetExtendedPropertiesEx(_HTTP_USER_AGENT_, userAgent);
                }
                //string remoteAddr = GetRemoteIP(request); //GetHeaderValue<string>(request, "REMOTE_ADDR");
                string remoteAddr = GetHeaderValue(request, _REMOTE_ADDR_);
                if (remoteAddr.IsNullOrWhiteSpaceEx() != true)
                {
                    dataTable.SetExtendedPropertiesEx(_REMOTE_ADDR_, remoteAddr);
                    customAgent = string.Format("[{0}]{1}", remoteAddr, customAgent);
                }
                string httpReferer = GetHeaderValue(request, _HTTP_REFERER_);
                if (httpReferer.IsNullOrWhiteSpaceEx() != true)
                {
                    dataTable.SetExtendedPropertiesEx(_HTTP_REFERER_, httpReferer);
                    customAgent = string.Format("{0}//HTTP_REFERER=>{1}", customAgent, httpReferer);
                }
                string httpHost = GetHeaderValue(request, _HTTP_HOST_);
                if (httpHost.IsNullOrWhiteSpaceEx() != true)
                {
                    dataTable.SetExtendedPropertiesEx(_HTTP_HOST_, httpHost);
                    customAgent = string.Format("{0}//HTTP_HOST=>{1}", customAgent, httpHost);
                }

                if (!customAgent.IsNullOrWhiteSpaceEx())
                {
                    dataTable.SetExtendedPropertiesEx(_CUSTOM_USER_AGENT_, customAgent);
                }

                if(bStructMachedToDataRowAppend == true && dataTable.Columns.Contains("type") && dataTable.Columns.Contains("key") && dataTable.Columns.Contains("name") && dataTable.Columns.Contains("value"))
                {
                    DataRow row = null;
                    if (remoteAddr.IsNullOrWhiteSpaceEx() != true)
                    {
                        row = dataTable.NewRow();
                        row["type"] = "CUSTOM";
                        row["key"] = _REMOTE_ADDR_;
                        row["name"] = _REMOTE_ADDR_;
                        //if (item.Key.EndsWith("[]"))
                        //{
                        //    row["key"] = item.Key.Replace("[]", string.Empty);
                        //}
                        row["value"] = remoteAddr;
                        dataTable.Rows.Add(row);
                        Result = true;
                    }
                    if (userAgent.IsNullOrWhiteSpaceEx() != true)
                    {
                        row = dataTable.NewRow();
                        row["type"] = "CUSTOM";
                        row["key"] = _HTTP_USER_AGENT_;
                        row["name"] = _HTTP_USER_AGENT_;
                        row["value"] = userAgent;
                        dataTable.Rows.Add(row);
                        Result = true;
                    }
                    if (httpHost.IsNullOrWhiteSpaceEx() != true)
                    {
                        row = dataTable.NewRow();
                        row["type"] = "CUSTOM";
                        row["key"] = _HTTP_HOST_;
                        row["name"] = _HTTP_HOST_;
                        row["value"] = httpHost;
                        dataTable.Rows.Add(row);
                        Result = true;
                    }
                    if (httpReferer.IsNullOrWhiteSpaceEx() != true)
                    {
                        row = dataTable.NewRow();
                        row["type"] = "CUSTOM";
                        row["key"] = _HTTP_REFERER_;
                        row["name"] = _HTTP_REFERER_;
                        row["value"] = httpReferer;
                        dataTable.Rows.Add(row);
                        Result = true;
                    }
                    if (customAgent.IsNullOrWhiteSpaceEx() != true)
                    {
                        row = dataTable.NewRow();
                        row["type"] = "CUSTOM";
                        row["key"] = _CUSTOM_USER_AGENT_;
                        row["name"] = _CUSTOM_USER_AGENT_;
                        row["value"] = customAgent;
                        dataTable.Rows.Add(row);
                        Result = true;
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// DataTable Resource에 HttpRequest 정보를 ExtendedProperties에 추가 하거나, 옵션에 따라 DataRow로 추가
        /// </summary>
        /// <param name="request">HttpRequest Resource</param>
        /// <param name="dataTable">DataTable Resource</param>
        /// <param name="bStructMachedToDataRowAppend">정해진 구조를 가진 DataTable일 경우 Datarow로 추가 옵션</param>
        public static void SetUserCustomAgent(HttpRequest request, DataTable dataTable, bool bStructMachedToDataRowAppend = false)
        {
            SetUserCustomAgentToExtendedProperties(request, dataTable, bStructMachedToDataRowAppend);
        }

        public static T GetHeaderValue<T>(HttpRequest request, string headerName)
        {
            StringValues values;
            if (request?.Headers?.TryGetValue(headerName, out values) ?? false) //_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhiteSpaceEx())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public static T GetHeaderValue<T>(HttpContext context, string headerName)
        {
            return GetHeaderValue<T>(context?.Request, headerName);
        }
        public static Dictionary<string, string> GetQueryStringList(HttpRequest request)
        {
            Dictionary<string, string> Result = null;
            if (request != null)
            {
                var pairs = request.Query;//.GetQueryNameValuePairs();//IEnumerable<KeyValuePair<string, string>>
                if (pairs != null && pairs.Count() > 0)
                {
                    Result = new Dictionary<string, string>();
                    foreach (var item in pairs)
                    {
                        //Result.AddEx(item.Key, item.Value, true);
                    }
                }
            }
            return Result;
        }

        public static string GetQueryStringValue(HttpRequest request, string name)
        {

            string Result = string.Empty;
            Dictionary<string, string> param = GetQueryStringList(request);
            if (param != null && param.Count > 0)
            {

                if (!name.IsNullOrWhiteSpaceEx() && name != "*")
                {
                    Result = param.LastOrDefault(val => val.Key.ToLower() == name.ToLower()).Value;
                    //var find = from val in param where val.Key.ToLower() == queryKey.ToLower() select val.Value;
                    //if(find != null && find.Count() > 0)
                    //{

                    //    List<string> list = find.ToList();
                    //    Result = list[list.Count - 1];
                    //}
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (KeyValuePair<string, string> item in param)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    }
                    Result = builder.ToString();
                    builder.Clear();
                }
            }
            return Result;
        }

        public static Dictionary<string, List<string>> GetHeaderListMultiple(HttpRequest request)
        {
            Dictionary<string, List<string>> Result = null;
            if (request != null)
            {
                Result = new Dictionary<string, List<string>>();
                var pairs = request.Headers;
                foreach (var item in pairs)
                {
                    Result.AddEx(item.Key, item.Value.ToList<string>(), true);
                }
            }
            return Result;
        }

        public static Dictionary<string, string> GetHeaderList(HttpRequest request, string separator = null)
        {
            Dictionary<string, string> Result = null;
            Dictionary<string, List<string>> headers = GetHeaderListMultiple(request);
            if (headers != null)
            {
                Result = new Dictionary<string, string>();
                foreach (var item in headers)
                {
                    string _key = item.Key;
                    string _value = item.Value.LastOrDefault();
                    if (item.Value.Count > 1 && separator != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (string str in item.Value)
                        {
                            if (builder.Length > 0)
                            {
                                builder.Append(separator);
                            }
                            builder.Append(str);
                        }
                        _value = builder.ToString();
                        builder.Clear();
                    }
                    Result.Add(_key, _value);
                }
            }
            return Result;
        }

        public static string GetHeaderValue(HttpRequest request, string name)
        {
            string Result = string.Empty;

            if (request != null && !name.IsNullOrWhiteSpaceEx())
            {
                Dictionary<string, string> header = GetHeaderList(request, " ");
                if (header != null)
                {
                    var matches = from val in header where val.Key.ToLower() == name.ToLower() select val.Value;
                    if (matches != null && matches.Count() > 0)
                    {
                        foreach (string match in matches)
                        {
                            Result = match.ToStringEx();
                        }
                    }
                }
            }
            return Result;
        }

        public static string GetRequestValue(HttpRequest request, string name, bool bFirstQueryStringGetValue = true)
        {
            string Result = null;
            string headerValue = GetHeaderValue(request, name);
            if (!headerValue.IsNullOrWhiteSpaceEx())
            {
                Result = headerValue;
            }
            if (headerValue.IsNullOrWhiteSpaceEx() || bFirstQueryStringGetValue == true)
            {
                Result = GetQueryStringValue(request, name);
            }
            return Result;
        }
        #endregion

        #region Session / 유사 Session 처리
        public static DataTable CreateSessionRecordSet(string name = null)
        {
            DataTable Result = new DataTable(name);
            Result.Columns.AddRange(new DataColumn[]{
                  new DataColumn { ColumnName = "session_id", DataType = typeof(string) }
                , new DataColumn { ColumnName = "module", DataType = typeof(string) }
                , new DataColumn { ColumnName = "key", DataType = typeof(string) }
                , new DataColumn { ColumnName = "value", DataType = typeof(object) }
                , new DataColumn { ColumnName = "val01", DataType = typeof(object) }
                , new DataColumn { ColumnName = "val02", DataType = typeof(object) }
                , new DataColumn { ColumnName = "val03", DataType = typeof(object) }
                , new DataColumn { ColumnName = "val04", DataType = typeof(object) }
                , new DataColumn { ColumnName = "val05", DataType = typeof(object) }
                , new DataColumn { ColumnName = "remark", DataType = typeof(object) }
                , new DataColumn { ColumnName = "reg_date", DataType = typeof(DateTime), DefaultValue = DateTime.Now }
                , new DataColumn { ColumnName = "mod_date", DataType = typeof(DateTime), DefaultValue = DateTime.Now }
                , new DataColumn { ColumnName = "is_use", DataType = typeof(bool), DefaultValue = true }
            });
            //if (!name.IsNullOrWhiteSpaceEx())
            //{
            //    Result.TableName = name;
            //}
            return Result;
        }

        /// <summary>
        /// 유사 Session DataTable에 값 설정
        /// </summary>
        /// <param name="sess">유사 Session DataTable</param>
        /// <param name="key">KEY(MOULE이 없고 '/'로 구분시 First : KEY, Last : MODULE)</param>
        /// <param name="value">VALUE</param>
        /// <param name="module">MODULE</param>
        /// <returns>작업 여부?(NULL : 미작업, True : 추가(Append), False : 수정(Modify))</returns>
        public static bool? SetSessionValue(DataTable sess, string key, object value, string module = null)
        {
            bool? Result = null;
            if(sess != null && !key.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    string sessID = sess.TableName;
                    if (module.IsNullOrWhiteSpaceEx())
                    {
                        string[] tmpKeys = key.Split("/");
                        key = tmpKeys[0];
                        if (tmpKeys.Length > 1)
                        {
                            module = tmpKeys[tmpKeys.Length - 1];
                        }
                    }
                    string mWhere = string.Format("key = '{0}'", key);
                    if (!module.IsNullOrWhiteSpaceEx())
                    {
                        mWhere += string.Format(" AND module = '{0}'", module);
                    }
                    DataRow[] findRows = sess.Select(mWhere);
                    if (findRows != null && findRows.Length > 0)
                    {
                        foreach (DataRow row in findRows)
                        {
                            row["value"] = value;
                            row["mod_date"] = DateTime.Now;
                        }
                        Result = false;
                    }
                    else
                    {
                        DataRow row = sess.NewRow();
                        row["key"] = key;
                        row["module"] = module;
                        row["value"] = value;
                        row["reg_date"] = DateTime.Now;
                        row["mod_date"] = DateTime.Now;
                        sess.Rows.Add(row);
                        Result = true;
                    }
                }
                catch (Exception ex)
                {
                    Result = null;
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return Result;
        }

        public static DataRow[] GetSessionDataRow(DataTable sess, string key, string module = null)
        {
            DataRow[] Result = null;
            if (sess != null && !key.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    string sessID = sess.TableName;
                    if (module.IsNullOrWhiteSpaceEx())
                    {
                        string[] tmpKeys = key.Split("/");
                        key = tmpKeys[0];
                        if (tmpKeys.Length > 1)
                        {
                            module = tmpKeys[tmpKeys.Length - 1];
                        }
                    }
                    string mWhere = string.Format("key = '{0}'", key);
                    if (!module.IsNullOrWhiteSpaceEx())
                    {
                        mWhere += string.Format(" AND module = '{0}'", module);
                    }
                    Result = sess.Select(mWhere);
                }
                catch (Exception ex)
                {
                    Result = null;
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return Result;
        }

        public static object[] GetSessionValues(DataTable sess, string key, string module = null)
        {
            object[] Result = null;
            try
            {
                DataRow[] rows = GetSessionDataRow(sess, key, module);
                if(rows != null && rows.Length > 0)
                {
                    int n = rows.Length;
                    Result = new object[n];
                    for(int i = 0; i < n; i++)
                    {
                        Result[i] = rows[i]["value"];
                    }
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return Result;
        }

        public static object GetSessionValue(DataTable sess, string key, string module = null, HxMultiplePosition position = HxMultiplePosition.None)
        {
            object Result = null;
            try
            {
                object[] values = GetSessionValues(sess, key, module);
                if (values != null && values.Length > 0)
                {
                    int n = values.Length;
                    if (n > 1)
                    {
                        switch (position)
                        {
                            case HxMultiplePosition.Last:
                                Result = values[n - 1];
                                break;
                            case HxMultiplePosition.All:
                                Result = (object)values.ToList();
                                break;
                            default:
                                Result = values[0];
                                break;
                        }
                    }
                    else
                    {
                        Result = values[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        #endregion
    }
}
