//using Microsoft.AspNetCore.Http;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

using HttpRequest = Microsoft.AspNetCore.Http.HttpRequest;

namespace HxCore
{
    public class HxNet
    {
        public const string _REGEX_IPv4_PATTERN_ = HxDefs._REGEX_IPv4_PATTERN_;
        public const string _REGEX_URI_PATTERN_ = HxDefs._REGEX_URI_PATTERN_;
        //public const string _URI_PATTERN_ = HxURIStructRec._URI_PATTERN_;

        public const string _NCSI_MSFT_CONN_URL_ = "http://www.msftconnecttest.com/connecttest.txt";
        protected const string _NCSI_MSFT_CONN_RESULT_ = "Microsoft Connect Test";

        public const string _NCSI_MSFT_NCSI_URL_ = "http://www.msftncsi.com/ncsi.txt";
        protected const string _NCSI_MSFT_NCSI_RESULT_ = "Microsoft NCSI";

        public const string _NCSI_HTENC_URL_ = "http://www.htenc.co.kr/api/ncsi/";
        protected const string _NCSI_HTENC_RESULT_ = "Hi-Tech Engineering Co.,Ltd. - NCSI";

        protected const string _NCSI_DNS_HOST_ = "dns.msftncsi.com";
        protected const string _NCSI_DNS_IP_ADDRESS_ = "131.107.255.255";

        public const string _CHECKIP_IPINFO_URL_ = "https://ipinfo.io/ip";
        public const string _CHECKIP_HTENC_URL_ = "http://www.htenc.co.kr/api/ip/";
        public const string _CHECKIP_DYNDNS_URL_ = "http://checkip.dyndns.org";
        public const string _CHECKIP_IPAPI_URL_ = "http://ip-api.com/csv";
        public static string GetClientContent(string uri)
        {
            string Result = null;
            try
            {
                if (!uri.IsNullOrWhiteSpaceEx())
                {
                    using (WebClient webClient = new WebClient())
                    {
                        Result = webClient.DownloadString(uri);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw ex;
            }
            return Result;

        }

        public static string GetClientDownloadFile(string address, string clientDownloadFileName = null)
        {
            string Result = null;
            if(address.IsNullOrWhiteSpaceEx() == true) return Result;

            if (HxString.IsWebUri(address) == true)
            {
                Uri uri = new Uri(address);
                Result = GetClientDownloadFile(uri, clientDownloadFileName);
            }
            return Result;
        }

        public static string GetClientDownloadFile(Uri address, string clientDownloadFileName = null)
        {
            string Result = null;

            if (address == null && address.Segments.Length <= 0) return Result;

            using (WebClient webClient = new WebClient())
            {
                string strDirName = HxFile.GetLocalTempDirectory();
                if (clientDownloadFileName.IsNullOrWhiteSpaceEx() == true)
                {
                    clientDownloadFileName = address.Segments[address.Segments.Length - 1];
                }
                //System.IO.Path.GetFileName(clientDownloadFileName);
                
                clientDownloadFileName = Path.Combine(strDirName, clientDownloadFileName);
                clientDownloadFileName = HxFile.GetFileUniquePath(clientDownloadFileName, HxFileOverwriteType.RenameDateMicroTime);
                clientDownloadFileName = HxFile.GetFileUniquePath(clientDownloadFileName, HxFileOverwriteType.RenameSequence);
                string strDirPath = HxFile.GetFileDirPath(clientDownloadFileName);
                HxFile.DirectoryCreate(strDirPath);
                webClient.DownloadFile(address, clientDownloadFileName);

                Result = clientDownloadFileName;
            }

            return Result;
        }


        public static string GetRemoteIpAddress(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            string Result = null;
            if (request != null)
            {
                Result = request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            //if (Result.IsNullOrWhiteSpaceEx())
            //{
            //    Result = UserHostAddress();
            //}
            //if (Result.IsNullOrWhiteSpaceEx())
            //{
            //    Result = UserGlobalAddress();
            //}
            return Result;
        }
        public static string GetUserRemoteAddress(Microsoft.AspNetCore.Http.HttpRequest request = null)
        {
            string Result = null;
            if (request != null)
            {
                Result = GetRemoteIpAddress(request);
            }
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                Result = GetUserHostAddress();
            }
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                Result = GetUserGlobalAddress();
                //Result = GlobalAddress;
            }
            return Result;
        }
        public static string GetUserGlobalAddress()
        {
            string Result = null;
            string ip;
            WebClient webClient = new WebClient();
            try
            {
                if (GetIsNetworkAvailable() == true)
                {
                    //Single TEXT : http://ipinfo.io/ip
                    //JSON : http://ipinfo.io/json

                    //Multi TEXT : http://ip-api.com/line
                    //JSON : http://ip-api.com/json
                    //XML : http://ip-api.com/xml
                    //CSV : http://ip-api.com/csv
                    //PHP : http://ip-api.com/php

                    if (Result.IsNullOrWhiteSpaceEx())
                    {
                        //출처 : http://www.msjo.kr/post/159559259874/delphi-c-초간단-공인아이피public-ip-알아오기
                        ip = webClient.DownloadString(_CHECKIP_IPINFO_URL_);
                        Result = Regex.Replace(ip.Trim(), @"\t|\n|\r", String.Empty);
                    }

                    if (Result.IsNullOrWhiteSpaceEx() || Result?.IsRegexMatchEx(_REGEX_IPv4_PATTERN_) != true)
                    {
                        ip = webClient.DownloadString(_CHECKIP_HTENC_URL_);
                        Result = Regex.Replace(ip.Trim(), @"\t|\n|\r", String.Empty);
                    }

                    if (Result.IsNullOrWhiteSpaceEx() || Result?.IsRegexMatchEx(_REGEX_IPv4_PATTERN_) != true)
                    {
                        //http://www.csharpstudy.com/Tip/Tip-network-connectivity.aspx
                        ip = webClient.DownloadString(_CHECKIP_DYNDNS_URL_);
                        if (!ip.IsNullOrWhiteSpaceEx() && ip.Contains(":"))
                        {
                            Result = ip.Split(':')[1].Split('<')[0].Trim();
                        }
                    }
                    
                    if (Result.IsNullOrWhiteSpaceEx() || Result?.IsRegexMatchEx(_REGEX_IPv4_PATTERN_) != true)
                    {
                        ip = webClient.DownloadString(_CHECKIP_IPAPI_URL_);
                        if (ip.IsNullOrWhiteSpaceEx() != true)
                        {
                            string[] ipItems = ip.SplitEx(",");
                            if (ipItems != null && ipItems.Length > 0)
                            {
                                if (ipItems[0].ToLower() == "success")
                                {
                                    ip = ipItems[ipItems.Length - 1];
                                    Result = Regex.Replace(ip.Trim(), @"\t|\n|\r", String.Empty); ;
                                }
                            }
                        }
                    }

                    if (Result.IsNullOrWhiteSpaceEx() != true && Result.IsRegexMatchEx(_REGEX_IPv4_PATTERN_) != true)
                    {
                        Result = string.Empty;
                    }

                   

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Result = null;
                //throw;
            }
            finally
            {
                webClient.Dispose();
            }
            return Result;
        }

        public static IPAddress GetUserHostIPAddress()
        {
            return Dns.GetHostAddresses(Dns.GetHostName()).Where(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
        }
        public static string GetUserHostAddress()
        {
            string Result = null;
            IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).Where(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
            if (ip != null)
            {
                Result = ip.ToStringEx();
            }
            return Result;
        }

        public static string GetUserHostName()
        {
            string Result = Dns.GetHostName();
            return Result;
        }

        public static List<string> GetUserAdressList(string hostName = null)
        {
            List<string> Result = null;
            if (hostName.IsNullOrWhiteSpaceEx())
            {
                hostName = GetUserHostName();
            }
            if (!hostName.IsNullOrWhiteSpaceEx())
            {
                IPHostEntry GetIP = Dns.GetHostEntry(hostName);

                //int n = 1;

                Result = new List<string>();

                for (int i = 0; i < GetIP.AddressList.Length; i++)
                {
                    if (GetIP.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)  // IPv4에 해당하면 출력
                    {
                        string ipAddr = GetIP.AddressList[i].ToString();
                        if (ipAddr != "127.0.0.1" && ipAddr != "::1")
                        {
                            Result.AddEx(ipAddr);
                        }
                        //n++;
                    }
                }
            }
            return Result;
        }

        public static string GetUserDomainName()
        {
            return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
        }

        public static bool GetIsNetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
        public static bool GetIsInternetConnected()
        {
            // Check NCSI test link
            WebClient webClient = new WebClient();
            try
            {
                if (GetIsNetworkAvailable() == true)
                {
                    //http://www.csharpstudy.com/Tip/Tip-network-connectivity.aspx
                    string result;
                    result = webClient.DownloadString(_NCSI_HTENC_URL_);
                    if (result == _NCSI_HTENC_RESULT_)
                    {
                        return true;
                    }

                    result = webClient.DownloadString(_NCSI_MSFT_NCSI_URL_);
                    if (result == _NCSI_MSFT_NCSI_RESULT_)
                    {
                        return true;
                    }

                    result = webClient.DownloadString(_NCSI_MSFT_CONN_URL_);
                    if (result == _NCSI_MSFT_CONN_RESULT_)
                    {
                        return true;
                    }

                    // Check NCSI DNS IP
                    var dnsHost = Dns.GetHostEntry(_NCSI_DNS_HOST_);
                    if (dnsHost.AddressList.Count() > 0 && dnsHost.AddressList[0].ToString().Trim() == _NCSI_DNS_IP_ADDRESS_)
                    {
                        return true;
                    }

                    string ip = GetUserGlobalAddress();
                    if (ip.IsNullOrWhiteSpaceEx() != true)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //try
                //{
                //    string result = webClient.DownloadString(_DYNDNS_TEST_URL_);
                //    string ip = result.Split(':')[1].Split('<')[0].Trim();
                //    if(ip.IsNullOrWhiteSpaceEx() == true)
                //    {
                //        return false;
                //    }
                //}
                //catch (Exception exDy)
                //{
                //    Debug.WriteLine(exDy);
                //    //throw;
                //    return false;
                //}
                return false;
            }
            finally
            {
                webClient.Dispose();
            }

            return false;
        }

        #region 네트워크 실시간 시간 가져오기
        //Link #01 - http://stackoverflow.com/questions/10769118/how-to-query-date-at-time-windows-com-ntp-server-by-using-c-net-code
        //Link #02 - https://mschwarztoolkit.svn.codeplex.com/svn/NTP/NtpClient.cs
        /// <summary>
        /// 네트워크 실시간 시간 가져오기
        /// </summary>
        /// <returns>날짜/시간</returns>
        public DateTime GetNetworkTime()
        {
            //MessageBox.Show(
            //time.nist.gov
            //time.windows.com
            return this.GetNetworkTime("time.nist.gov");
        }

        /// <summary>
        /// 네트워크 실시간 시간 가져오기
        /// </summary>
        /// <param name="ntpServer">NTP Server Host/IP</param>
        /// <returns>날짜/시간</returns>
        public DateTime GetNetworkTime(string ntpServer)
        {
            IPAddress[] address = Dns.GetHostEntry(ntpServer).AddressList;
            if (address == null || address.Length == 0)
                throw new ArgumentException("Could not resolve ip address from '" + ntpServer + "'.", "ntpServer");

            IPEndPoint ep = new IPEndPoint(address[0], 123);
            //string str = string.Empty;
            return this.GetNetworkTime(ep);
        }

        /// <summary>
        /// 네트워크 실시간 시간 가져오기
        /// </summary>
        /// <param name="ep">NTP Server Host/IP and Port</param>
        /// <returns>날짜/시간</returns>
        public DateTime GetNetworkTime(IPEndPoint ep)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            s.Connect(ep);

            byte[] ntpData = new byte[48]; // RFC 2030 
            ntpData[0] = 0x1B;
            for (int i = 1; i < 48; i++)
                ntpData[i] = 0;

            s.Send(ntpData);
            s.Receive(ntpData);

            byte offsetTransmitTime = 40;
            ulong intpart = 0;
            ulong fractpart = 0;

            for (int i = 0; i <= 3; i++)
                intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

            for (int i = 4; i <= 7; i++)
                fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

            ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);
            s.Close();

            TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);

            DateTime dateTime = new DateTime(1900, 1, 1);
            dateTime += timeSpan;

            //TimeSpan offsetAmount = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
            TimeSpan offsetAmount = TimeZoneInfo.Local.GetUtcOffset(dateTime);
            DateTime Result = (dateTime + offsetAmount);

            return Result;
            //return dateTime;
        }
        #endregion

        #region SMTP
        public static bool SendMail(string mailServer, bool bEnableSsl, string subject, string message, bool bMessageHtml, string fromAddress, string toAddress, string ccAddress, List<string> attachmentList, string mailUserName = null, string mailPassword = null, string mailDomain = null)
        {
            List<string> toAddressList = null;
            List<string> ccAddressList = null;
            if (!toAddress.IsNullOrWhiteSpaceEx())
            {
                toAddressList = new List<string>();

                string[] toAddressArray = toAddress.Split(';');

                //if(toAddressArray != null && toAddressArray.Length > 0)
                //{

                //    foreach(string str in toAddressArray)
                //    {
                //        if (!str.Trim().IsNullOrWhiteSpaceEx())
                //        {
                //            toAddressList.AddEx(str, true);
                //        }
                //    }
                //}
                toAddressList = toAddressArray.ToListEx();
            }
            if (!ccAddress.IsNullOrWhiteSpaceEx())
            {
                ccAddressList = new List<string>();

                string[] ccAddressArray = ccAddress.Split(';');
                //if (toAddressArray != null && toAddressArray.Length > 0)
                //{

                //    foreach (string str in toAddressArray)
                //    {
                //        if (!str.Trim().IsNullOrWhiteSpaceEx())
                //        {
                //            toAddressList.AddEx(str, true);
                //        }
                //    }
                //}
                ccAddressList = ccAddressArray.ToListEx();
            }
            return SendMail(mailServer, bEnableSsl, subject, message, bMessageHtml, fromAddress, toAddressList, ccAddressList, attachmentList, mailUserName, mailPassword, mailDomain);
        }
        /// <summary>
        /// SMTP 메일 발송
        /// </summary>
        /// <param name="mailServer">메일서버 [Host:Port]</param>
        /// <param name="bEnableSsl">SSL?</param>
        /// <param name="subject">메일 제목</param>
        /// <param name="body">메일 본문</param>
        /// <param name="bMessageHtml">본문 Html?</param>
        /// <param name="fromAddress">발신자 주소</param>
        /// <param name="toAddressList">수신자 주소 리스트</param>
        /// <param name="ccAddressList">참자조 주소 리스트</param>
        /// <param name="attachmentList">첨부파일 리스트</param>
        /// <param name="mailUserName">인증 로그인 ID</param>
        /// <param name="mailPassword">인증 로그인 패스워드</param>
        /// <param name="mailDomain">인증 로그인 도메인(AD Domain)</param>
        /// <returns>성공 여부</returns>
        public static bool SendMail(string mailServer, bool bEnableSsl, string subject, string body, bool bMessageHtml, string fromAddress, List<string> toAddressList, List<string> ccAddressList, List<string> attachmentList, string mailUserName = null, string mailPassword = null, string mailDomain = null)
        {
            bool Result = false;
            if (!mailServer.IsNullOrWhiteSpaceEx() && !fromAddress.IsNullOrWhiteSpaceEx() && toAddressList != null && toAddressList.Count > 0)
            {

                //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' }); //https://gist.github.com/jacking75/1825458
                MailMessage message = new MailMessage();
                try
                {
                    message.Subject = subject;

                    message.Body = body;
                    message.IsBodyHtml = bMessageHtml;

                    message.From = new MailAddress(fromAddress);

                    foreach (string s in toAddressList)
                    {
                        message.To.Add(new MailAddress(s));
                    }

                    foreach (string s in ccAddressList)
                    {
                        message.CC.Add(new MailAddress(s));
                    }

                    foreach (string s in attachmentList)
                    {
                        string path = Path.GetFullPath(s);
                        if (File.Exists(path))
                        {
                            Attachment atch = new Attachment(s);
                            message.Attachments.Add(atch);
                        }
                    }

                    string[] mailServerArray = mailServer.Split(':');
                    string mailHost = mailServerArray[0];
                    int mailPort = 25;
                    if (mailServerArray.Length > 1)
                    {
                        mailPort = mailServerArray[1].ToIntEx();
                    }

                    SmtpClient client = new SmtpClient(mailHost, mailPort)
                    {
                        EnableSsl = bEnableSsl
                    };
                    if (!mailUserName.IsNullOrWhiteSpaceEx() || !mailPassword.IsNullOrWhiteSpaceEx())
                    {
                        client.Credentials = new NetworkCredential(mailUserName, mailPassword, mailDomain);
                    }
                    else
                    {
                        client.UseDefaultCredentials = false;
                    }
                    //client.DeliveryMethod = SmtpDeliveryMethod.Network; //System.Net.Mail.SmtpDeliveryMethod.PickupDirectoryFromIis; // 이걸 하지 않으면 Gmail에 인증을 받지 못한다.
                    client.Send(message);

                    Result = true;
                }
                catch (SmtpException ex)
                {
                    Result = false;
                    throw ex;
                }
                catch (Exception ex)
                {
                    Result = false;
                    throw ex;
                }
                finally
                {
                    message.Dispose();
                }

                //new SmtpClient
            }
            return Result;
        }
        #endregion

        #region 네트워크 체크
        public static IPAddress GetLocalIPAddress()
        //출처 : https://stackoverflow.com/questions/6803073/get-local-ip-address
        {
            IPAddress Result = null;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return Result;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            Result = host?.AddressList?.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            if(Result == null || Result.ToStringEx().IsNullOrWhiteSpaceEx() == true)
            {
                Result = GetUserHostIPAddress();
            }
            return Result;
        }

        protected static bool GetIsRemoteIpOpenPortCheked(string ipAdress, int port)
        //참조 : https://acpi.tistory.com/123
        {
            bool Result = false;
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);
                IAsyncResult ret = socket.BeginConnect(ipAdress, port, null, null);
                Result = ret.AsyncWaitHandle.WaitOne(100, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
            return Result;
        }
        protected static bool GetIsRemoteNameOpenPortChecked(string hostName, int port)
        {
            bool Result = false;
            if (hostName.IsNullOrWhiteSpaceEx() != true)
            {
                var host = Dns.GetHostEntry(hostName);
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string strIPAddress = ip.ToString();
                        if (Result != true && strIPAddress.IsNullOrWhiteSpaceEx() != true)
                        {
                            Result = GetIsRemoteIpOpenPortCheked(strIPAddress, port);
                        }
                    }
                }
            }
            return Result;
        }

        public static bool GetIsRemoteHostOpenPortChecked(string hostAddress, int port)
        {
            bool Result = false;
            bool bIPv4 = HxString.IsRegexMatch(hostAddress, _REGEX_IPv4_PATTERN_);
            if (bIPv4)
            {
                Result = GetIsRemoteIpOpenPortCheked(hostAddress, port);
            }
            else
            {
                Result = GetIsRemoteNameOpenPortChecked(hostAddress, port);
            }
            return Result;
        }
        public static bool GetIsRemoteURIOpenPortChecked(string host, int defaultPort = 80)
        {
            bool Result = false;
            if (host.IsNullOrWhiteSpaceEx() != true)
            {
                HxURIStructRec uri = new HxURIStructRec(host, defaultPort);
                if (uri.Host.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = GetIsRemoteHostOpenPortChecked(uri.Host, uri.Port.ToIntEx(defaultPort));
                }
            }
            return Result;

        }

        public static IPAddress GetRemoteIPAddress(HttpRequest request = null)
        {
            IPAddress Result = null;
            if (request != null)
            {
                Result = request.HttpContext.Connection.RemoteIpAddress;
            }
            if(Result == null || Result.ToStringEx().IsNullOrWhiteSpaceEx()) 
            {
                Result = GetUserHostIPAddress();
            }
            return Result;
        }

        public static string GetRemoteIPAddressString(HttpRequest request)
        {
            IPAddress ip = GetRemoteIPAddress(request);
            if (ip != null) return ip.ToString();
            return null;
        }
        #endregion //네트워크 체크

        public static RestResponse RestClientResponse(string callURI, Method restMethod = Method.Get, Dictionary<string, object> headerValue = null, Dictionary<string, object> postValue = null)
        {
            RestResponse Result = null;
            try
            {
                string strCallApiUrl = callURI;

                if (strCallApiUrl.EndsWith("/") != true && callURI.StartsWith("/") != true)
                {
                    //strCallApiUrl += "/";
                }
                strCallApiUrl += callURI;

                var request = new RestRequest
                {
                    Method = restMethod
                };

                if (headerValue != null && headerValue.Count > 0)
                {
                    foreach (KeyValuePair<string, object> o in headerValue)
                    {
                        request.AddHeader(o.Key, o.Value.ToStringEx());
                    }
                }

                if (postValue != null && postValue.Count > 0)
                {
                    foreach (KeyValuePair<string, object> o in postValue)
                    {
                        string strKey = o.Key;
                        string strValue = o.Value.ToStringEx();

                        if (restMethod == Method.Post)
                        {
                            //request.AddParameter(strKey, o.Value);
                            request.AddParameter(strKey, strValue);
                        }
                        else if (restMethod == Method.Get)
                        {
                            strValue = strValue.IsNullOrWhiteSpaceEx() != true ? System.Web.HttpUtility.UrlEncode(strValue.ToStringEx()) : string.Empty;

                            request.AddParameter(strKey, strValue);
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
                            request.AddHeader(strKey, o.Value.ToStringEx());
                        }
                    }
                }

                var client = new RestClient(strCallApiUrl);
                Result = client.Execute(request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            finally
            {

            }
            return Result;
        }

        public static Version GetClickOnceVersion(string url)
        {
            if (url.IsNullOrWhiteSpaceEx() != true)
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        string contents = webClient.DownloadString(url);
                        if (contents.IsNullOrWhiteSpaceEx() != true)
                        {
                            HxApplicationManifestRec rec = HxApplicationManifestRec.Create(contents);
                            return rec.Version;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw ex;
                }
            }
            return null;
        }
    }
}
