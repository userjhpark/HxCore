using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HxCore
{
    
    public class HxEnvVars //HxEnvironmentVars
    {
        public Microsoft.AspNetCore.Http.HttpRequest WebRequest { get; set; }

        public string SourceName { get; set; }
        public HxDbConnectionRec DbConnInfo { get; set; }
        public HxOpenApiJsonRec? APIConnInfo { get; set; }
        public HxFlowApiRec? FlowConnInfo { get; set; }

        public HxRemoteServiceType RemoteServiceType { get; set; }


        public string HostName { get; set; }
        public string PCMachineName { get; set; }
        public string PCUserName { get; set; }
        public string PCDomainName { get; set; }

        public string IntranetGlobalPattern { get; set; }
        public string RemoteAddress { get; set; }
        public string GlobalAddress { get; set; }

        public bool IsInternetConnected { get; set; }
        public bool IsDbConnected { get; set; }
        public bool IsWebApiConnected { get; set; }


        public DirectoryInfo AppLocalExecuteDirectoryInfo { get; private set; }
        public DirectoryInfo AppLocalWorkDirectoryInfo { get; private set; }
        public DirectoryInfo AppLocalTempDirectoryInfo { get; private set; }
        public DirectoryInfo AppLocalLogDirectoryInfo { get; private set; }
        public DirectoryInfo AppLocalDownloadDirectoryInfo { get; private set; }
        public string AppTempDir => AppLocalTempDirectoryInfo?.FullName ?? Path.GetTempPath();
        public string AppWorkDir => AppLocalWorkDirectoryInfo?.FullName ?? HxUtils.GetAppCurrDir();
        public string AppDownloadDir => AppLocalDownloadDirectoryInfo?.FullName ?? AppTempDir;

        public string RemoteCosURI { get; protected set; }
        
        public HxEnvVars(HttpRequest webRequest, string sourceName, HxDbConnectionRec dbConnInfo, HxOpenApiJsonRec aPIConnInfo, HxFlowApiRec flowConnInfo, HxRemoteServiceType remoteServiceType, string hostName, string pCMachineName, string pCUserName, string pCDomainName, string intranetGlobalPattern, string remoteAddress, string globalAddress, bool isInternetConnected, bool isDbConnected, bool isWebApiConnected)
        {
            this.WebRequest = webRequest;
            this.SourceName = sourceName;
            this.DbConnInfo = dbConnInfo;
            this.APIConnInfo = aPIConnInfo;
            this.FlowConnInfo = flowConnInfo;
            this.RemoteServiceType = remoteServiceType;
            this.HostName = hostName;
            this.PCMachineName = pCMachineName;
            this.PCUserName = pCUserName;
            this.PCDomainName = pCDomainName;
            this.IntranetGlobalPattern = intranetGlobalPattern;
            this.RemoteAddress = remoteAddress;
            this.GlobalAddress = globalAddress;
            this.IsInternetConnected = isInternetConnected;
            this.IsDbConnected = isDbConnected;
            this.IsWebApiConnected = isWebApiConnected;
        }
        public HxEnvVars(string execDirStr = null, string workDirStr = null, string downloadDirStr = null, string tempDirStr = null, string logDirStr = null, string remoteCosUriStr = null, HttpRequest request = null)
        {
            this.WebRequest = null;
            this.SourceName = null;
            this.DbConnInfo = default;
            this.APIConnInfo = null;
            this.FlowConnInfo = null;
            this.RemoteServiceType = HxRemoteServiceType.None;
            this.HostName = null;
            this.PCMachineName = null;
            this.PCUserName = null;
            this.PCDomainName = null;
            this.IntranetGlobalPattern = null;
            this.RemoteAddress = HxNet.GetUserRemoteAddress(request);
            this.GlobalAddress = HxNet.GetUserGlobalAddress();
            this.IsInternetConnected = HxNet.GetIsInternetConnected();
            this.IsDbConnected = false;
            this.IsWebApiConnected = false;

            if (Environment.MachineName.IsNullOrWhiteSpaceEx() != true)
            {
                PCMachineName = Environment.MachineName;
            }
            if (Environment.UserName.IsNullOrWhiteSpaceEx() != true)
            {
                PCUserName = Environment.UserName;
            }
            if(Environment.UserDomainName.IsNullOrWhiteSpaceEx() != true)
            {
                PCDomainName = Environment.UserDomainName;
            }

            //string strProcessName = AppDomain.CurrentDomain.FriendlyName;
            //HxLoadAssembly AppAssembly = new HxLoadAssembly(strProcessName);
            RemoteCosURI = remoteCosUriStr;
            if (execDirStr.IsNullOrWhiteSpaceEx() == true)
            {
                //execDirStr = AppAssembly.LoadAssemblyDir;

                execDirStr = HxUtils.AppBaseDir;
            }
            if (workDirStr.IsNullOrWhiteSpaceEx() == true)
            {
                workDirStr = execDirStr;
            }
            if (downloadDirStr.IsNullOrWhiteSpaceEx() == true)
            {
                downloadDirStr = Path.Combine(workDirStr, "Download");
            }
            if (tempDirStr.IsNullOrWhiteSpaceEx() == true)
            {
                tempDirStr = Path.Combine(workDirStr, "Temp");
                if (tempDirStr.IsNullOrWhiteSpaceEx() == true || HxFile.DirectoryExists(tempDirStr) != true)
                {
                    tempDirStr = Path.GetTempPath();
                }
            }
            if (logDirStr.IsNullOrWhiteSpaceEx() == true)
            {
                logDirStr = Path.Combine(workDirStr, "Log");
            }

            AppLocalExecuteDirectoryInfo = new DirectoryInfo(execDirStr);
            AppLocalWorkDirectoryInfo = new DirectoryInfo(workDirStr);
            AppLocalDownloadDirectoryInfo = new DirectoryInfo(downloadDirStr);
            AppLocalTempDirectoryInfo = new DirectoryInfo(tempDirStr);
            AppLocalLogDirectoryInfo = new DirectoryInfo(logDirStr);
        }
    }
    public class HxEnvironmentVars : HxEnvVars
    {
        //
    }
    
}
