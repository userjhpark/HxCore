using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using static HxCore.HxUTF8StringWriter;

    partial class HxUtils
    {
        #region Web Resource
        public static HttpRequest WebRequest { get; protected set; }

        public static void SetHttpRequest(HttpRequest request)
        {
            WebRequest = request;
        }

        public static bool IsIPLocalAddress
        {
            get { return GetIsLocalIPEquals(RemoteAddress); }
        }
        #endregion

        /// <summary>
        /// 입력된 파일명을 확인하여 로컬파일이 아닌 웹경로일 경우 로컬 임시폴더로 다운로드 후 파일명 반환
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <returns>로컬 파일명</returns>
        public static string GetLocalFileName(string fileName)
        {
            string Result = fileName;

            if (fileName.IsNullOrWhiteSpaceEx() != true && HxFile.FileExists(Result) != true && HxString.IsWebUri(Result))
            {
                Result = HxNet.GetClientDownloadFile(Result);
            }

            return Result;
        }
        public static string RemoteAddress => GetRemotedAddress(null);


        private static string _RemoteAddress;
        public static string GetRemotedAddress(HttpRequest request, bool bInit = false)
        {
            if (bInit == true || IsReloadEnvironment == true || _RemoteAddress.IsNullOrWhiteSpaceEx() == true)
            {
                _RemoteAddress = GetRemoteIpAddress(request, bInit);

                if (_RemoteAddress.IsNullOrWhiteSpaceEx() == true || _RemoteAddress.IsRegexMatchEx(_REGEX_IPv4_PATTERN_) != true)
                {
                    _RemoteAddress = GetUserHostAddress();
                }
                if (_RemoteAddress.IsNullOrWhiteSpaceEx() == true || _RemoteAddress.IsRegexMatchEx(_REGEX_IPv4_PATTERN_) != true)
                {
                    //Result = UserGlobalAddress();
                    _RemoteAddress = GetUserGlobalAddress();
                }
            }
            return _RemoteAddress;
            ;

            //return GetRemoteIpAddress(request, bInit);
        }

        public static string GetRemoteIpAddress(HttpRequest request, bool bInit = false)
        {
            string Result = null;
            if (IsReloadEnvironment == true || Result.IsNullOrWhiteSpaceEx() != true || bInit == true)
            {
                if (request == null)
                {
                    request = WebRequest;
                }
                if (request != null)
                {
                    Result = request.HttpContext.Connection.RemoteIpAddress.ToString();
                    if (Result.IsNullOrWhiteSpaceEx() == true || Result.IsRegexMatchEx(_REGEX_IPv4_PATTERN_) != true)
                    {
                        System.Net.IPAddress remoteIPAddress = HxNet.GetRemoteIPAddress(request);
                        Result = remoteIPAddress.ToString();
                    }
                }
            }
            return Result;
        }

        //공인 아이피
        public static string GlobalAddress
        {
            get
            {
                string Result = null;
                if (Result.IsNullOrWhiteSpaceEx() == true)
                {
                    Result = HxUtils.GetUserGlobalAddress(false);
                }
                return Result;
            }
        }
        
        
        #region 재정의



        #endregion
        /// <summary>
        /// HTTP Request 값 가져오기 : COOKIE(option) < GET < POST < HEADER
        /// </summary>
        /// <param name="request">Request Resource</param>
        /// <returns>Dictionary Resource</returns>
        public static Dictionary<string, object> GetRequestVars(HttpRequest request, bool pUseCookie = false)
        {
            Dictionary<string, object> Result = null;
            if (request != null)
            {
                Result = new Dictionary<string, object>();

                try
                {
                    if (pUseCookie == true)
                    {
                        var cookieObj = request.Cookies;
                        if (cookieObj != null)
                        {
                            foreach (var o in cookieObj)
                            {
                                Result.AddEx(o.Key, o.Value);
                            }
                        }
                    }
                }
                catch (Exception exCookie)
                {
                    Debug.WriteLine(exCookie);
                    //throw;
                }

                try
                {
                    var queryObj = request.Query;
                    if (queryObj != null)
                    {
                        foreach (var o in queryObj)
                        {
                            Result.AddEx(o.Key, o.Value);
                        }
                    }
                }
                catch (Exception exQuery)
                {
                    Debug.WriteLine(exQuery);
                    throw;
                }

                try
                {
                    var formObj = request?.Form;
                    if (formObj != null)
                    {
                        foreach (var o in formObj)
                        {
                            Result.AddEx(o.Key, o.Value);
                        }
                    }
                }
                catch (Exception exForm)
                {
                    Debug.WriteLine(exForm);
                    //throw;
                }

                try
                {
                    var headerObj = request.Headers;
                    if (headerObj != null)
                    {
                        foreach (var o in headerObj)
                        {
                            Result.AddEx(o.Key, o.Value);
                        }
                    }
                }
                catch (Exception exHeader)
                {
                    Debug.WriteLine(exHeader);
                    //throw;
                }
            }
            return Result;
        }

        public static Dictionary<string, object> GetRequestVars(HttpRequest request, HxHttpMethodType methodType)
        {
            Dictionary<string, object> Result = null;
            if (request != null)
            {
                try
                {
                    if (request.ContentType == HxDefs._CONTENT_TYPE_APPLICATION_FORM_URLENCODED_)
                    {
                        Debug.WriteLine(HxDefs._CONTENT_TYPE_APPLICATION_FORM_URLENCODED_);
                    }


                    if (methodType != HxHttpMethodType.NONE)
                    {
                        Result = new Dictionary<string, object>();

                        if ((methodType & HxHttpMethodType.COOKIE) != 0)
                        {
                            var cookieObj = request.Cookies;
                            if (cookieObj != null)
                            {
                                foreach (var o in cookieObj)
                                {
                                    Result.AddEx(o.Key, o.Value?.First());
                                }
                            }
                        }
                        if ((methodType & HxHttpMethodType.GET) != 0)
                        {
                            var queryObj = request.Query;
                            if (queryObj != null)
                            {
                                foreach (var o in queryObj)
                                {
                                    if (o.Value.ToStringEx().IsNullOrWhiteSpaceEx() != true && o.Value.Count == 1)
                                    {
                                        Result.AddEx(o.Key, o.Value.First());
                                    }
                                    else
                                    {
                                        Result.AddEx(o.Key, o.Value);
                                    }
                                }
                            }
                        }
                        if ((methodType & HxHttpMethodType.POST) != 0)
                        {
                            try
                            {
                                var formObj = request?.Form;
                                if (formObj != null)
                                {
                                    foreach (var o in formObj)
                                    {
                                        if (o.Value.ToStringEx().IsNullOrWhiteSpaceEx() != true && o.Value.Count == 1)
                                        {
                                            Result.AddEx(o.Key, o.Value.First());
                                        }
                                        else
                                        {
                                            Result.AddEx(o.Key, o.Value);
                                        }
                                    }
                                }
                            }
                            catch (Exception exForm)
                            {
                                Debug.WriteLine(exForm);
                                //throw;
                            }
                        }

                        if ((methodType & HxHttpMethodType.HEADER) != 0)
                        {
                            var headerObj = request.Headers;
                            if (headerObj != null)
                            {
                                foreach (var o in headerObj)
                                {
                                    if (o.Value.ToStringEx().IsNullOrWhiteSpaceEx() != true && o.Value.Count == 1)
                                    {
                                        Result.AddEx(o.Key, o.Value.First());
                                    }
                                    else
                                    {
                                        Result.AddEx(o.Key, o.Value);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exQuery)
                {
                    Debug.WriteLine(exQuery);
                    throw;
                }
            }
            return Result;
        }

        public static Dictionary<string, object> GetRequestRAW(HttpRequest request)
        {
            Dictionary<string, object> Result = null;
            if (request != null)
            {
                Result = new Dictionary<string, object>();

                try
                {
                    HttpContext context = request.HttpContext;
                    using (var reader = new StreamReader(
                        context.Request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false,
                        bufferSize: -1,
                        leaveOpen: true)
                    )
                    {
                        var body = reader.ReadToEndAsync();
                        // Do some processing with body…

                        // Reset the request body stream position so the next middleware can read it
                        context.Request.Body.Position = 0;
                        if (body != null && body.Result.IsNullOrWhiteSpaceEx() != true)
                        {
                            string strRAW = body.Result;
                            strRAW = HttpUtility.UrlDecode(strRAW);
                            JObject jObject = JObject.Parse(strRAW);
                            if (jObject != null && jObject.HasValues)
                            {
                                foreach (JProperty prop in jObject.Properties())
                                {
                                    if (prop != null && prop.Name.IsNullOrWhiteSpaceEx() != true)
                                    {
                                        Result.AddEx(prop.Name, prop.Value.ToString(), true);
                                    }
                                }
                            }
                            //JToken jtRAW = JToken.Parse(strRAW);
                            //if(jtRAW != null && jtRAW.HasValues)
                            //{
                            //    foreach(JToken jt in jtRAW)
                            //    {
                            //        Result.AddEx(jt.);
                            //    }
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw ex;
                }

            }
            return Result;
        }

        

        #region Rest
        public static RestResponse GetRestClientResponse(HxOpenApiJsonRec openApi, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            string callURI = openApi.API_HOST;
            //if (callURI.IsNullOrWhiteSpaceEx() == true) return null;
            if (callURI.IsNullOrWhiteSpaceEx() != true || (callURI.StartsWith("http://") != true && callURI.StartsWith("https://") != true)) return null;

            return GetRestClientResponse(callURI, restMethod, headerValue, postValue);
        }
        public static RestResponse GetRestClientResponse(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null, string apiKeyValueStr = null, string apiPassValueStr = null)
        {
            RestResponse Result = null;
            if (callURI.IsNullOrWhiteSpaceEx() == true) return Result;

            const string _Api_Key_Name_  = HxOpenApiDbRec._CDF_KEY_NAME_;
            const string _Api_Pass_Name_ = HxOpenApiDbRec._CDF_PASS_NAME_;
            try
            {
                string strCallApiUrl = callURI;
                //if (strCallApiUrl.IsNullOrWhiteSpaceEx() == true || (strCallApiUrl.StartsWith("http://") != true && strCallApiUrl.StartsWith("https://") != true))
                //{
                //    strCallApiUrl = APIConnInfo.API_HOST;
                //}

                if (strCallApiUrl.Contains("?") != true && strCallApiUrl.EndsWith("/") != true && callURI.StartsWith("/") != true)
                {
                    strCallApiUrl += "/";
                }

                var client = new RestClient(strCallApiUrl);
                var request = new RestRequest
                {
                    Method = restMethod
                };

                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("accept", "application/json");

                bool bInputApiKey = false;
                bool bInputApiPass = false;

                
                string strApiKeyValue = apiKeyValueStr;
                string strApiPassValue = apiPassValueStr;

                if (headerValue != null && headerValue.Count > 0)
                {

                    foreach (KeyValuePair<string, object> o in headerValue)
                    {
                        string strItemName = o.Key;
                        string strItemValue = o.Value.ToStringEx();
                        if (o.Key == _Api_Key_Name_)
                        {
                            bInputApiKey = true;
                            if (apiKeyValueStr != null && apiKeyValueStr != strItemValue)
                            {
                                strItemValue = apiKeyValueStr;
                            }
                        }
                        else if (o.Key == _Api_Pass_Name_)
                        {
                            bInputApiPass = true;
                            if (apiPassValueStr != null && apiPassValueStr != strItemValue)
                            {
                                strItemValue = apiPassValueStr;
                            }
                        }

                        request.AddHeader(strItemName, strItemValue);
                    }
                }

                if (postValue != null && postValue.Count > 0)
                {
                    foreach (KeyValuePair<string, object> o in postValue)
                    {
                        string strItemName = o.Key;
                        string strItemValue = o.Value.ToStringEx();

                        if (o.Key == _Api_Key_Name_)
                        {
                            bInputApiKey = true;
                            if (apiKeyValueStr != null && apiKeyValueStr != strItemValue)
                            {
                                strItemValue = apiKeyValueStr;
                            }
                        }
                        else if (o.Key == _Api_Pass_Name_)
                        {
                            bInputApiPass = true;
                            if (apiPassValueStr != null && apiPassValueStr != strItemValue)
                            {
                                strItemValue = apiPassValueStr;
                            }
                        }

                        if (restMethod == Method.Post)
                        {
                            //request.AddParameter(strItemName, o.Value);
                            request.AddParameter(strItemName, strItemValue);
                        }
                        else if (restMethod == Method.Get)
                        {
                            strItemValue = strItemValue.IsNullOrWhiteSpaceEx() != true ? System.Web.HttpUtility.UrlEncode(strItemValue.ToStringEx()) : string.Empty;

                            request.AddParameter(strItemName, strItemValue);
                            /**
                            if (strCallUrl.Contains("?") != true)
                            {
                                strCallUrl += "?";
                            }
                            else
                            {
                                strCallUrl += "&";
                            }
                            strCallUrl += $"{strKey}={strValue}";
                            */
                        }
                        else
                        {
                            request.AddHeader(strItemName, strItemValue);
                        }
                    }
                }


                

                if (bInputApiKey == false && apiKeyValueStr.IsNullOrWhiteSpaceEx() != true)
                {
                    request.AddHeader(_Api_Key_Name_, apiKeyValueStr);
                }
                if (bInputApiPass == false && apiPassValueStr.IsNullOrWhiteSpaceEx() != true)
                {
                    request.AddHeader(_Api_Pass_Name_, apiPassValueStr);
                }

                Result = client.Execute(request);
            }
            catch (Exception ex)
            {
                SetDebugWrite(ex, true);
                //throw ex;
            }
            finally
            {

            }
            return Result;
        }
        public static RestResponse GetRestClientResponse(string callURI, Method restMethod, Dictionary<string, object> headerValue, string bodyRawJson)
        {
            RestResponse Result = null;
            if (callURI.IsNullOrWhiteSpaceEx() == true || bodyRawJson.IsNullOrWhiteSpaceEx() == true) return Result;

            HxUriRec webUriInfo = new HxUriRec(callURI);
            if (webUriInfo.BaseUrl.IsNullOrWhiteSpaceEx() == true) { return Result; }
            var options = new RestClientOptions(webUriInfo.BaseUrl);
            var client = new RestClient(options);
            var request = new RestRequest($"{webUriInfo.Path}{webUriInfo.QueryString}", Method.Post);
            //request.AddHeader("X-OCR-SECRET", "ZHVReUZnU3VxVEhsZmRIYW1XYUhxdWxrbHRNdXZ3alI=");
            //request.AddHeader("Content-Type", "application/json");

            if (headerValue != null && headerValue.Count > 0)
            {

                foreach (KeyValuePair<string, object> o in headerValue)
                {
                    string strItemName = o.Key;
                    string strItemValue = o.Value.ToStringEx();
                    request.AddHeader(strItemName, strItemValue);
                }
            }
            
            request.AddStringBody(bodyRawJson, DataFormat.Json);
            Result = client.Execute(request);
            Debug.WriteLine(Result.Content);
            return Result;
        }
        public static HxResultValue GetRestClientContentResultValue(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            HxResultValue Result = new HxResultValue();
            try
            {
                RestResponse response = GetRestClientResponse(callURI, restMethod, headerValue, postValue);
                if (response != null && response.IsSuccessful == true)
                {
                    string strValue = response.Content;
                    Result = HxUtils.JsonDeserializeObject<HxResultValue>(strValue);
                    if (Result != null)
                    {
                        Result.Value2 = response;
                    }
                }
            }
            catch (Exception ex)
            {
                Result.ResultType = HxResultType.Exception;
                Result.DetailMessage += "/Exception : " + ex.Message;
                SetDebugWrite(ex, false, Result.DetailMessage);
            }
            finally
            {

            }
            return Result;
        }
        public static HxResultValue GetRestClientContentResultValue(string callURI, Method restMethod, Dictionary<string, object> headerValue, string postRawJson)
        {
            HxResultValue Result = new HxResultValue();
            try
            {
                RestResponse response = GetRestClientResponse(callURI, restMethod, headerValue, postRawJson);
                if (response != null && response.IsSuccessful == true)
                {
                    string strValue = response.Content;
                    Result.Value = strValue;
                    //Result = HxUtils.JsonDeserializeObject<HxResultValue>(strValue);
                    if (Result != null)
                    {
                        Result.Value2 = response;
                    }
                }
            }
            catch (Exception ex)
            {
                Result.ResultType = HxResultType.Exception;
                Result.DetailMessage += "/Exception : " + ex.Message;
                SetDebugWrite(ex, false, Result.DetailMessage);
            }
            finally
            {

            }
            return Result;
        }
        public static string GetRestClientContentString(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            HxResultValue Result = GetRestClientContentResultValue(callURI, restMethod, headerValue, postValue);
            return Result?.Value?.ToStringEx(true);
        }
        public static DataTable GetRestClientContentDataTable(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            DataTable Result = null;
            try
            {
                //IRestResponse response = GetRestClientResponse(callURI, restMethod, headerValue, postValue);

                HxResultValue restValue = GetRestClientContentResultValue(callURI, restMethod, headerValue, postValue);
                if (restValue != null && restValue.Value != null && restValue.Success == true)
                {
                    Result = JsonDeserializeObject<DataTable>(restValue.Value);
                }
            }
            catch (Exception ex)
            {
                SetDebugWrite(ex, true);
            }
            return Result;
        }
        protected static DataView GetRestClientContentDataView(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            DataView Result = null;
            try
            {
                //IRestResponse response = GetRestClientResponse(callURI, restMethod, headerValue, postValue);

                HxResultValue restValue = GetRestClientContentResultValue(callURI, restMethod, headerValue, postValue);
                if (restValue != null && restValue.Value != null && restValue.Success == true)
                {
                    Result = HxUtils.JsonDeserializeObject<DataView>(restValue.Value);
                    if (Result == null)
                    {
                        DataTable dt = HxUtils.JsonDeserializeObject<DataTable>(restValue.Value);
                        if (dt != null)
                        {
                            Result = dt?.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetDebugWrite(ex, true);
            }
            return Result;
        }
        public static string GetRestClientContentResultValueToJsonString(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            string Result = null;
            try
            {
                HxResultValue response = GetRestClientContentResultValue(callURI, restMethod, headerValue, postValue);
                if (response != null && response.Value != null)
                {
                    Result = HxUtils.JsonSerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                SetDebugWrite(ex, true);
            }
            return Result;
        }
        public static string GetRestClientContentDataTableToJsonString(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            string Result = null;
            DataTable dt;
            try
            {
                HxResultValue response = GetRestClientContentResultValue(callURI, restMethod, headerValue, postValue);
                if (response != null && response.Value != null)
                {
                    dt = response.Value as DataTable;
                    Result = HxUtils.JsonSerializeObject(dt);
                }
            }
            catch (Exception ex)
            {
                SetDebugWrite(ex, true);
            }
            return Result;
        }

        public static object SetRestClientContent(string callURI, Method restMethod = Method.Post, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            object Result = null;
            try
            {
                HxResultValue restValue = GetRestClientContentResultValue(callURI, restMethod, headerValue, postValue);
                if (restValue != null && restValue.Value != null && restValue.Success == true)
                {
                    Result = HxUtils.JsonDeserializeObject<object>(restValue.Value);
                }
            }
            catch (Exception ex)
            {
                SetDebugWrite(ex, true);
            }
            return Result;
        }
        #endregion

        /// <summary>
        /// Local 호출 여부? / 원격 호출자(Client) = LocalHost(Server)
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <returns></returns>
        public static bool GetIsLocalIPEquals(string remoteAddress)
        {
            bool Result = false;
            //Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ip = IPAddress.Any;
            ////listener.Bind(new IPEndPoint(ip, 8080));
            ////listener.Listen(10);

            //String MyIP = IPAddress.Parse(((IPEndPoint)client.Sock.RemoteEndPoint).Address.ToString());

            if (!remoteAddress.IsNullOrWhiteSpaceEx())
            {
                switch (remoteAddress.Trim())
                {
                    case "::1":
                    case "127.0.0.1":
                    case "localhost":
                        Result = true;
                        break;
                    default:
                        break;
                }
            }
            return Result;
        }

        public static HxServiceHostRec GetServiceHostInfo(string host, HxServiceProviderType serviceProviderType)
        {
            HxServiceHostRec Result = default;
            if(host.IsNullOrWhiteSpaceEx() != true)
            {
                string strPattern = @"^([0-9a-zA-Z\.\-_]{1,})+(([:,]{1,1})([0-9]{1,5}))?([\/]{1,1}([0-9a-zA-Z\.\-_]{1,}))$";
                if (serviceProviderType == HxServiceProviderType.MsSQL || serviceProviderType == HxServiceProviderType.PostgreSQL)
                {
                    strPattern = @"^([\w\.\-_]{1,})+(([:,]{1,1})([0-9]{1,5}))?([\/]{1,1}([\w\.\-_\s\(\)]{1,}))$";
                }
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(host, strPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string strDbHost = match.Groups[1].Value;
                    //string strDbPortDelimiter = match.Groups[3].Value;
                    string strDbPort = match.Groups[4].Value;
                    string strDbName = match.Groups[6].Value;
                    if (strDbPort.IsNullOrWhiteSpaceEx() == true) 
                    {
                        strDbPort = HxEnum.GetServiceDefaultPort(serviceProviderType).ToStringEx();
                    }
                    Result = new HxServiceHostRec(serviceProviderType, strDbHost, strDbPort.ToIntEx(0), strDbName);
                }
            }
            return Result;
        }

        public static HxServiceHostRec GetServiceHostInfo(string host, HxServiceProviderType serviceProviderType, out string servcieHost, out int servicePort, out string serviceName)
        {
            HxServiceHostRec Result = GetServiceHostInfo(host, serviceProviderType);
            servcieHost = Result.HostName;
            servicePort = Result.Port;
            serviceName = Result.ServiceName;
            return Result;
        }
        public static HxServiceHostRec GetServiceHostInfo(string host, HxDbProviderType dbProvidrType)
        {
            HxServiceProviderType providerType = HxEnum.GetServiceProviderType(dbProvidrType);
            return GetServiceHostInfo(host, providerType);
        }

        

        public static HxServiceHostRec GetServiceHostInfo(string hostURI)
        {
            HxServiceHostRec Result = default;
            if (hostURI.IsNullOrWhiteSpaceEx() != true && hostURI.Contains(@"://") == true)
            {
                string[] arrStr = hostURI.SplitEx(@"://");
                if (arrStr == null || arrStr.Length <= 1) return Result;

                string protocal = arrStr[0].ToLower();
                string host = arrStr[1];

                HxServiceProviderType providerType = HxEnum.GetServiceProviderType(protocal);

                string strPattern = @"^(?:(\w+):\/\/)?([^:\/\s]+)(?:(\:)([^\/]*))?((\/[^\s/\/]+)*)?\/([^#\s\?]*)(?:\?([^#\s]*))?(#\w*)?$";
                //strPattern = @"^(?:(\w+):\/\/)?([^:\/\s]+)(?:\:([^\/]*))?((\/[^\s/\/]+)*)?\/([^#\s\?]*)(?:\?([^#\s]*))?(#\w*)?$";
                //string strPattern = @"^([0-9a-zA-Z\.\-_]{1,})+(([:,]{1,1})([0-9]{1,5}))?([\/]{1,1}([0-9a-zA-Z\.\-_]{1,}))$";
                if (providerType == HxServiceProviderType.MsSQL || providerType == HxServiceProviderType.PostgreSQL)
                {
                    strPattern = @"^([\w\.\-_]{1,})+(([:,]{1,1})([0-9]{1,5}))?([\/]{1,1}([\w\.\-_\s\(\)]{1,}))$";
                }
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(host, strPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string strProtocol = match.Groups[1].Value;
                    string strHost = match.Groups[2].Value;
                    string strDbPortDelimiter = match.Groups[3].Value;
                    string strPort = match.Groups[4].Value;
                    string strDirAll = match.Groups[5].Value;
                    //string[] strDirs = strDir?.SplitEx("/");
                    string strService = strDirAll?.SplitEx("/").FirstOrDefault(r => r.IsNullOrWhiteSpaceEx() != true);
                    string strDirLast = match.Groups[6].Value;
                    string strFileName = match.Groups[7].Value;
                    string strParams = match.Groups[8].Value;
                    string strBookmark = match.Groups[9].Value;
                    if (strPort.IsNullOrWhiteSpaceEx() == true)
                    {
                        strPort = HxEnum.GetServiceDefaultPort(protocal).ToStringEx();
                    }
                    Result = new HxServiceHostRec(providerType, strHost, strPort.ToIntEx(0), strService);
                }
            }
            return Result;
        }
    }
}
