using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace HxCore
{
    public struct HxApplicationManifestRec
    {
        public string Name;
        public string VersionValue;
        public string ProcessorArchitecture;
        public string DependencyType;
        public string CodeBase;

        public Version Version
        {
            get
            {
                if(VersionValue.IsNullOrWhiteSpaceEx() != true)
                {
                    Version Result;
                    if(Version.TryParse(VersionValue, out Result))
                    {
                        return Result;
                    }
                    else
                    {
                        return new Version(0, 0, 0, 0);
                    }
                }
                return null;
            }
        }

        public HxApplicationManifestRec(bool init = true)
        {
            this.Name = ".application";
            this.VersionValue = string.Empty;
            this.ProcessorArchitecture = string.Empty;
            this.DependencyType = string.Empty;
            this.CodeBase = string.Empty;
        }

        public static HxApplicationManifestRec Create(string xmlContents)
        {
            if (xmlContents.IsNullOrWhiteSpaceEx() != true)
            {
                return HxApplicationManifestRec.GetApplicationInfoFromString(xmlContents);
            }
            return default;
        }

        public static HxApplicationManifestRec Create(FileInfo xmlFileInfo)
        {
            return GetApplicationInfoFromFile(xmlFileInfo);
        }


        private static HxApplicationManifestRec GetApplicationInfoFromString(string xmlContents)
        {
            HxApplicationManifestRec Result = new HxApplicationManifestRec();
            try
            {
                string strNodeName = string.Empty;
                string strNodeValue = string.Empty;
                string strNodePath = string.Empty;
                if (xmlContents.IsNullOrWhiteSpaceEx() != true)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(xmlContents);

                    XmlNamespaceManager manager = new XmlNamespaceManager(xml.NameTable);
                    manager.AddNamespace("asmv1", "urn:schemas-microsoft-com:asm.v1");
                    manager.AddNamespace("asmv2", "urn:schemas-microsoft-com:asm.v2");
                    manager.AddNamespace("xrml", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
                    manager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    manager.AddNamespace("asmv3", "urn:schemas-microsoft-com:asm.v3");
                    manager.AddNamespace("dsig", "http://www.w3.org/2000/09/xmldsig#");
                    manager.AddNamespace("co.v1", "urn:schemas-microsoft-com:clickonce.v1");
                    manager.AddNamespace("co.v2", "urn:schemas-microsoft-com:clickonce.v2");
                    manager.AddNamespace("assemblyIdentity", "urn:schemas-microsoft-com:asm.v1");
                    manager.AddNamespace("description", "urn:schemas-microsoft-com:asm.v1");
                    manager.AddNamespace("Signature", "http://www.w3.org/2000/09/xmldsig#");

                    string strXmlIdentityNodePath = "/asmv1:assembly/asmv1:assemblyIdentity";
                    XmlNode xnIdentitySelect = xml.SelectSingleNode(strXmlIdentityNodePath, manager);
                    XmlAttributeCollection xnIdentityAttr = xnIdentitySelect.Attributes;
                    //string value = xnSelect.InnerXml;
                    string value = xnIdentitySelect.OuterXml;
                    string strAttrName = xnIdentityAttr["name"].Value;
                    string strAttrVer = xnIdentityAttr["version"].Value;
                    string strProcessArchitecture = xnIdentityAttr["processorArchitecture"].Value;

                    string strXmlManifestNodePath = "/asmv1:assembly/asmv2:dependency/asmv2:dependentAssembly";
                    XmlNode xnManifest = xml.SelectSingleNode(strXmlManifestNodePath, manager);
                    XmlAttributeCollection xnManifestAttr = xnManifest.Attributes;
                    string strDependencyType = xnManifestAttr["dependencyType"].Value;
                    string strCodeBase = xnManifestAttr["codebase"].Value;

                    Result.Name = strAttrName;
                    Result.VersionValue = strAttrVer;
                    Result.ProcessorArchitecture = strProcessArchitecture;
                    Result.DependencyType = strDependencyType;
                    Result.CodeBase = strCodeBase;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw ex;
            }
            return Result;
        }

        private static HxApplicationManifestRec GetApplicationInfoFromFile(FileInfo xmlFileInfo)
        {
            if (xmlFileInfo != null && xmlFileInfo.Exists == true)
            {
                return HxApplicationManifestRec.GetApplicationInfoFromFile(xmlFileInfo.FullName);
            }
            return default;
        }

        private static HxApplicationManifestRec GetApplicationInfoFromFile(string xmlFileName)
        {
            HxApplicationManifestRec Result = new HxApplicationManifestRec();
            try
            {
                string strNodeName = string.Empty;
                string strNodeValue = string.Empty;
                string strNodePath = string.Empty;
                if (System.IO.File.Exists(xmlFileName))
                {
                    //string contents = HxFile.GetFileReader(xmlFileName);
                    string contents = HxFile.GetTextFileReadAllText(xmlFileName);
                    Result = GetApplicationInfoFromString(contents);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw ex;
            }
            return Result;
        }

    }

}
