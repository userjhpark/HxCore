using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HxCore
{
    partial class HxUtils
    {
        #region JSON/XML 관련 처리
        /// <summary>
        /// File형태의 JSON을 읽어들임
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <returns>JSON Resource</returns>
        public static JObject JsonFileLoad(string fileName)
        {
            JObject Result = null;
            string str = HxFile.GetFileReader(fileName);
            if (!str.IsNullOrWhiteSpaceEx())
            {
                Result = JObject.Parse(str);
            }
            return Result;
        }
        //TODO : SetJsonFIleSave 구현 필요
        private static bool JsonFIleSave(string fileName, StringBuilder sb, Dictionary<string, object> bind = null)
        {
            bool Result = false;
            try
            {
                using (var sw = new System.IO.StringWriter(sb))
                using (var jsonWriter = new Newtonsoft.Json.JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
                    jsonWriter.WriteStartObject();
                    //jsonWriter.WritePropertyName("keyID"); jsonWriter.WriteValue("key_dgeag");
                    //jsonWriter.WritePropertyName("writer"); jsonWriter.WriteValue("writere");
                    //jsonWriter.WritePropertyName("date"); jsonWriter.WriteValue(DateTime.Now);
                    if (bind != null && bind.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> item in bind)
                        {
                            jsonWriter.WritePropertyName(item.Key); jsonWriter.WriteValue(item.Value);
                        }
                    }
                    jsonWriter.WriteEndObject();

                    string json = sw.ToString();
                    jsonWriter.Close();
                    sw.Close();
                    System.IO.File.WriteAllText(fileName, json);
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                Debug.WriteLine(ex.Message);
                //throw;
            }
            return Result;
        }

        /// <summary>
        /// JSON token 가져오기
        /// </summary>
        /// <param name="json">JSON Resource</param>
        /// <param name="key">Key Name</param>
        /// <returns>JSON token</returns>
        public static JToken FromJObjectFindToValue(JObject json, string key)
        {
            JToken Result = null;
            try
            {
                if (json != null && json.Count > 0 && json[key] != null && json[key].HasValues == true)
                {
                    //string[] sKeys = keyPath.SplitEx("\\");
                    Result = json[key];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }
            return Result;
        }

        /// <summary>
        /// JSON token 가져오기
        /// </summary>
        /// <param name="jToken">JSON token</param>
        /// <param name="key">Key Name</param>
        /// <returns>JSON token</returns>
        public static JToken FromJTokenFindToValue(JToken jToken, string key)
        {
            JToken Result = string.Empty;
            try
            {
                if (jToken != null && jToken.HasValues)
                {
                    //string[] sKeys = keyPath.SplitEx("\\");
                    Result = jToken[key];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }
            return Result;
        }

        /// <summary>
        /// JSON token 가져오기
        /// </summary>
        /// <param name="json">JSON Resource</param>
        /// <param name="keyPath">Key경로</param>
        /// <param name="separator">경로 구분자</param>
        /// <returns>JSON token</returns>
        public static JToken FromJObjectFindPathToValue(JObject json, string keyPath, string separator = "/")
        {
            JToken Result = null;
            try
            {
                if (json != null && json.Count > 0)
                {
                    if (separator.IsNullOrWhiteSpaceEx())
                    {
                        separator = "/";
                    }

                    string[] sKeys = keyPath.SplitEx(separator);

                    int n = sKeys.Length;
                    Result = json;
                    for (int i = 0; i < n; i++)
                    {
                        if (!sKeys[i].IsNullOrWhiteSpaceEx())
                        {
                            Result = FromJTokenFindToValue(Result, sKeys[i]);
                            if (Result == null || (Result != null && !Result.HasValues))
                            {
                                return null;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw;
            }
            return Result;
        }

        public static T ConvertJsonDeserialize<T>(string json)
        {
            T Result = default;
            if (!json.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    Result = JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }
        protected static string ConvertDataTableToJsonString(DataTable data, bool flag = false)
        {
            return JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        }
        public static string ConvertSerializeObjectToJsonString(object data, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(data, settings);
        }
        public static JObject ConvertSerializeToJsonObject(object data, JsonSerializerSettings settings = null)
        {
            string json = ConvertSerializeObjectToJsonString(data, settings);
            if (!json.IsNullOrWhiteSpaceEx())
            {
                return ConvertStringToJObject(json);
            }
            return null;
        }
        /// <summary>
        /// DataTable To JSON JArray
        /// </summary>
        /// <param name="data">DataTable Resource</param>
        /// <param name="bSerializeObject">SerializeObject 여부</param>
        /// <returns>JSON JArray</returns>
        private static JArray ConvertDatatableToJson(DataTable data, bool bSerializeObject = true)
        {
            return ConvertDatatableToJArray(data, bSerializeObject);
        }

        /// <summary>
        ///  DataTable To JSON JArray
        /// </summary>
        /// <param name="data">DataTable Resource</param>
        /// <param name="bSerializeObject">SerializeObject 여부</param>
        /// <returns>JSON JArray</returns>
        public static JArray ConvertDatatableToJArray(DataTable data, bool bSerializeObject = true)
        {
            if (data != null && data.Rows.Count > 0)
            {
                return JArray.Parse(ConvertDatatableToJsonString(data, bSerializeObject));
            }
            return null;
        }
        /// <summary>
        /// DataTable to JSON
        /// </summary>
        /// <param name="data">DataTable Resource</param>
        /// <param name="bSerializeObject">SerializeObject 사용 여부</param>
        /// <returns>JSON String</returns>
        public static string ConvertDatatableToJsonString(DataTable data, bool bSerializeObject = true)
        {
            //출처 : https://stackoverflow.com/questions/17398019/convert-datatable-to-json-in-c-sharp
            string Result = null;
            if (data != null && data.Rows.Count > 0)
            {
                try
                {
                    //var lst = dt.AsEnumerable()
                    //    .Select(r => r.Table.Columns.Cast<DataColumn>()
                    //            .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                    //           ).ToDictionary(z => z.Key, z => z.Value)
                    //    ).ToList();
                    //                    //now serialize it
                    //var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //return serializer.Serialize(lst);
                    //JavaScriptSerializer
                    if (bSerializeObject == true)
                    {
                        Result = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                        //Result = JsonConvert.SerializeObject();
                    }
                    else
                    {
                        DataSet ds = new DataSet();
                        ds.Merge(data);
                        StringBuilder JsonString = new StringBuilder();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            JsonString.Append("[");
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                JsonString.Append("{");
                                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                {
                                    if (j < ds.Tables[0].Columns.Count - 1)
                                    {
                                        JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                                    }
                                    else if (j == ds.Tables[0].Columns.Count - 1)
                                    {
                                        JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                                    }
                                }
                                if (i == ds.Tables[0].Rows.Count - 1)
                                {
                                    JsonString.Append("}");
                                }
                                else
                                {
                                    JsonString.Append("},");
                                }
                            }
                            JsonString.Append("]");
                            Result = JsonString.ToString();
                        }
                        else
                        {
                            Result = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //DebugMessage
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
            return Result;
        }

        /// <summary>
        /// JSON to DataTable
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="tableName">DataTable Name</param>
        /// <returns>DataTable</returns>
        public static DataTable ConvertJsonToDatatable(string json, string tableName = null)
        {
            DataTable Result = null;
            if (!json.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    Result = JsonConvert.DeserializeObject<DataTable>(json);
                    if (Result != null && !tableName.IsNullOrWhiteSpaceEx())
                    {
                        Result.TableName = tableName;
                    }
                }
                catch (Exception ex)
                {
                    Result = null;
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }
        /// <summary>
        /// XML to JSON
        /// </summary>
        /// <param name="doc">XmlDocument Resource</param>
        /// <returns>JSON String</returns>
        public static string ConvertXmlToJsonString(XmlDocument doc)
        {
            if (doc != null)
            {
                return JsonConvert.SerializeXmlNode(doc);
            }
            return null;
        }
        /// <summary>
        /// XML String To JSON
        /// </summary>
        /// <param name="xml">XML String</param>
        /// <returns>JSON String</returns>
        public static string ConvertXmlStrToJsonString(string xml)
        {
            string Result = null;
            if (!xml.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    Result = ConvertXmlToJsonString(doc);
                }
                catch (Exception ex)
                {
                    Result = null;
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }
        /// <summary>
        /// JSON to XML
        /// </summary>
        /// <param name="json">JSON String</param>
        /// <returns>XmlDocument</returns>
        public static XmlDocument ConvertJsonToXml(string json)
        {
            XmlDocument Result = null;
            if (!json.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    Result = JsonConvert.DeserializeXmlNode(json);
                }
                catch (Exception ex)
                {
                    Result = null;
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }
        /// <summary>
        /// DataTable to XML String
        /// </summary>
        /// <param name="data">DataTable Resource</param>
        /// <returns>XML String</returns>
        public static string ConvertDataTable2XmlString(DataTable data)
        {
            string Result = null;
            if(data != null)
            {
                try
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        data.WriteXml(sw);
                        Result = sw.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }
        /// <summary>
        /// DataTable to XPathDocument
        /// </summary>
        /// <param name="data">DataTable Resuorce</param>
        /// <returns>XPathDocument</returns>
        public static XPathDocument ConvertDataTable2XPathDoc(DataTable data)
        {
            XPathDocument Result = null;
            if (data != null)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        data.WriteXml(ms);
                        ms.Position = 0;
                        Result = new XPathDocument(ms);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }
        /// <summary>
        /// String to XPathDocument
        /// </summary>
        /// <param name="input">Input String</param>
        /// <returns>XPathDocument</returns>
        public static XPathDocument ConvertStringToXPathDoc(string input)
        {
            XPathDocument Result = null;
            if (!input.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Result = new XPathDocument(new StringReader(input));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }
        #endregion

        public static JObject ConvertStringToJObject(string str)
        {
            JObject Result;
            str = str?.Trim();
            if (!str.StartsWith("{"))
            {
                str = "{" + str;
            }
            if (!str.EndsWith("}"))
            {
                str += "}";
            }
            Result = JObject.Parse(str);
            return Result;
        }

        public static JArray ConvertStringToJArray(string str)
        {
            try
            {
                return JArray.Parse(str);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                try
                {
                    JObject jObject = ConvertStringToJObject(str);
                    //string json = JsonConvert.SerializeObject(jObject);
                    //int n = jObject.Count;
                    JArray Result = new JArray(jObject);
                    foreach(JToken token in jObject.Children())
                    {
                        Result.Add(token);
                    }
                    return Result;
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2);
                    //throw ex2;
                }
                //throw ex;
            }
            return null;
        }

        //public static string ConvertListToJsonString(List<Dictionary<string, string>> data)
        //{
        //    string Result = null;
        //    int nList = data.Count;
        //    if(nList > 0)
        //    {
        //        for(int index = 0; index < nList; index++)
        //        {
        //            Dictionary<string, string> item = data[index];
        //        }
        //    }
        //    return Result;
        //}

        /// <summary>
        /// DataTable을 JsonObject String으로 변환
        /// </summary>
        /// <param name="dt">DataTable Resource</param>
        /// <returns>Json String</returns>
        public static string DataTableToJsonObj(DataTable dt)
        {
            //출처 : https://stackoverflow.com/questions/17398019/convert-datatable-to-json-in-c-sharp
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        #region PDF/XFDF
        public static Dictionary<string, HxAnnotsRec> XFDFStrToAnnotsRecordList(string xmlString, string author = "")
        {
            Dictionary<string, HxAnnotsRec> Result = null;
            
            string strKeyName = null;
            try
            {
                if (!xmlString.IsNullOrWhiteSpaceEx() && xmlString != "undefined")
                {
                    Result = new Dictionary<string, HxAnnotsRec>();

                    XDocument doc = XDocument.Load(new StringReader(xmlString));
                    var elements = doc.Root.Element(XName.Get("annots", "http://ns.adobe.com/xfdf/"))
                                    .Elements()
                                    .OrderBy(e => int.Parse(e.Attribute("page").Value));
                    ;
                    //Result.Add("ALL", new MarkupAnnotsRec(doc));
                    /*
                    var elements = doc.Root.Element(XName.Get("annots", "http://ns.adobe.com/xfdf/"))
                            .Elements(XName.Get("highlight", "http://ns.adobe.com/xfdf/"))
                            .OrderBy(e => int.Parse(e.Attribute("page").Value));
                    */
                    StringBuilder builder = new StringBuilder();
                    foreach (XElement element in elements)
                    {
                        
                        string S = element.ToString();
                        //string author = string.Format("[{0}]{1}/{2}/{3}", SysEnv.Core.LoginID, SysEnv.Core.LoginName, SysEnv.Core.LoginDutyName, SysEnv.Core.LoginDeptName);
                        HxAnnotsRec annotInfo = new HxAnnotsRec(element, author);
                        int iPage = annotInfo.Page;
                        string strName = annotInfo.Name;
                        strKeyName = $"{annotInfo.Name}:{iPage}";

                        if (Result.ContainsKey(strKeyName))
                        {
                            // TODO: (2023.11.14) - 키가 중복되는 문제 발생, 이유는 알수 없음 / ㅜ.ㅜ
                            strKeyName += ":" + DateTime.Now.ToDateTimeStringDefaultFormatBEx();
                            Debug.WriteLine(strKeyName);
                        }

                        Result.Add(strKeyName, annotInfo);
                        //builder.AppendFormat("<p>{0}</p>", element.Attribute("subject").Value);
                    }
                    //Result += builder.ToStringEx();


                    //jsQueryString = "exportAsXFDFStr(true, true, null, true, 1);";

                    //PDFXEdit.IPXC_AnnotsList annotsList = pdfCtl.Inst.CreateAnnotsList();
                    /*
                    PDFXEdit.IPXV_Document Doc = pdfCtl.Doc;
                    PDFXEdit.PXV_Inst Inst = pdfCtl.Inst;
                    //PDFXEdit.IOperation Op = Inst.CreateOp
                    PDFXEdit.IPXV_AnnotsList annotsList = Inst.CreateAnnotsList();
                    for (uint i = 0; i < Doc.CoreDoc.Pages.Count; i++)
                    {
                        PDFXEdit.IPXC_Page page = Doc.CoreDoc.Pages[i];
                        uint nCnt = page.GetAnnotsCount();
                        for (uint j = 0; j < nCnt; j++)
                        {
                            PDFXEdit.IPXC_Annotation annot = page.GetAnnot(j);

                            //Debug.WriteLine(annot.Actions.);

                        }
                    }

                    //annotsList.
                    */
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(strKeyName + " : "+ ex.Message);
                throw ex;
            }
            
            return Result;
        }
        #endregion
    }
}
