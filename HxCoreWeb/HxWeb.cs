using System;
using System.Collections.Generic;

namespace HxCore.Web
{
    using HxCore;
    using Microsoft.AspNetCore.Http;
    //using Newtonsoft.Json;
    //using Org.BouncyCastle.Asn1.Ocsp;
    //using Org.BouncyCastle.Ocsp;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;

    public partial class HxWeb : HxUtils
    {
        /*
        #region 재정의
        public static string RemotedAddress(HttpRequest request)
        {
            if(request != null)
            {
                var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;
                return remoteIpAddress.ToString();
            }
            return null;
        }


        #endregion
        /// <summary>
        /// HTTP Request 값 가져오기 : COOKIE(option) < GET < POST < HEADER
        /// </summary>
        /// <param name="request">Request Resource</param>
        /// <returns>Dictionary Resource</returns>
        public static Dictionary<string, object> RequestVars(HttpRequest request, bool pUseCookie = false)
        {
            Dictionary<string, object> Result = null;
            if(request != null)
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

        public static Dictionary<string, object> RequestVars(HttpRequest request, HxHttpMethodType methodType)
        {
            Dictionary<string, object> Result = null;
            if (request != null)
            {
                try
                {
                    if(request.ContentType == HxDefs._CONTENT_TYPE_APPLICATION_FORM_URLENCODED_)
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
                                        if(o.Value.ToStringEx().IsNullOrWhiteSpaceEx() != true && o.Value.Count == 1)
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

                        if((methodType & HxHttpMethodType.HEADER) != 0)
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

        */
    }
}
