using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore.Data
{
    public class HxDbUtils : HxUtils
    {
        #region DB Resource
        protected static IHxDb DB { get; private set; }
        public static IHxDb CreateDb(HxDbConnectionRec conn)
        {
            IHxDb Result = null;
            if (conn.User.IsNullOrWhiteSpaceEx() != true)
            {
                //Result = new IHxDb(conn.User, conn.Password, conn.HostName);
                Result = HxSQL.CreateDb(conn);
            }
            return Result;
        }

        public static bool SetDb(IHxDb db)
        {
            DB = db;
            return IsDbOpen(null);
        }
        public static bool IsDbOpen(IHxDb db = null)
        {
            if (db == null) db = DB;
            if (db != null && db.Open()) return true;
            return false;
        }
        #endregion

        public static void DoOpenWithAnnotsToSaveAs(string formLocation, string[] pXfdfFullNames)
        {
            string outputFileNameAndPath = Path.Combine(HxFile.GetFileDirPath(formLocation), "SaveAs_" + HxFile.GetFileName(formLocation));
            File.Copy(formLocation, outputFileNameAndPath, true);


            if (formLocation.IsNullOrWhiteSpaceEx() != true && File.Exists(formLocation))
            {
                //string outputFileNameAndPath = Path.Combine(HxFile.GetFileDirPath(formLocation), "SaveAs_" + HxFile.GetFileName(formLocation));
                File.Copy(formLocation, outputFileNameAndPath, true);
                using (FileStream outputStream = new FileStream(outputFileNameAndPath, FileMode.Open))
                {
                    // We receive the XML bytes
                    XfdfReader xfdf = new XfdfReader(pXfdfFullNames[0]);
                    // We get the corresponding form
                    PdfReader reader = new PdfReader(formLocation);
                    // We create an OutputStream for the new PDF

                    // Now we create the PDF
                    PdfStamper stamper = new PdfStamper(reader, outputStream);
                    // We alter the fields of the existing PDF
                    AcroFields fields = stamper.AcroFields;
                    fields.SetFields(xfdf);
                    // take away all interactivity
                    stamper.FormFlattening = true;

                    // close the stamper
                    stamper.Close();
                    reader.Close();



                    /*
                    FdfReader fdfReader = new FdfReader(pXfdfFullNames[0]);
                    PdfReader formReader = new PdfReader(formLocation);
                    PdfStamper pdfStamper = new PdfStamper(formReader, outputStream);
                    AcroFields pdfForm = pdfStamper.AcroFields;

                    pdfForm.SetFields(fdfReader);
                    pdfStamper.FormFlattening = true;
                    pdfStamper.Writer.CloseStream = false;

                    //pdfStamper.Close();
                    //outputStream.Close();

                    fdfReader.Close();
                    formReader.Close();
                    */
                }
            }
        }
       
        

        /// <summary>
        /// Get Query String To DataTable ( = GetQueryData )
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="bind">Paramters / Binds</param>
        /// <returns>DataTable</returns>
        protected static DataTable GetQueryDataTable(string queryString, Dictionary<string, object> bind, bool bStoredProcedure = false)
        {
            DataTable Result = null;
            string SQL = queryString;
            Result = GetQueryData(SQL, bind, bStoredProcedure);
            return Result;
        }
        /// <summary>
        /// Get Query String To DataTable
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="mWhere">Query Where 조건절</param>
        /// <param name="bind">Paramters / Binds</param>
        /// <returns>DataTable</returns>
        protected static DataTable GetQueryDataTable(string queryString, string mWhere = null, Dictionary<string, object> bind = null, bool bStoredProcedure = false)
        {
            DataTable Result = null;
            string SQL = GetQueryString(queryString, mWhere);
            Result = GetQueryData(SQL, bind, bStoredProcedure);
            return Result;
        }
        /// <summary>
        /// Get Query String To DataTable
        /// </summary>
        /// <param name="queryString">Query String</param>
        /// <param name="bind">Paramters / Binds</param>
        /// <returns>DataTable</returns>
        public static DataTable GetQueryData(string queryString, Dictionary<string, object> bind = null, bool bStoredProcedure = false)
        {
            return GetQueryData(DB, queryString, bind, bStoredProcedure);
        }
        public static DataTable GetQueryData(IHxDb db, string queryString, Dictionary<string, object> bind = null, bool bStoredProcedure = false)
        {
            //return GetQueryData(db, queryString, bind, bStoredProcedure);
            DataTable Result = null;
            //string SQL = this.GetQueryString(queryType, mWhere);
            //dnCore.Data.dnDbSql queryDb = null;
            if (!queryString.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    //queryDb = new dnCore.Data.dnDbSql(this.Db.DbProviderType);
                    //using (TDbSQL queryDb = new TDbSQL())
                    //{
                    //    //using (dnCore.Data.dnDbSql queryDb = new dnCore.Data.dnDbSql(this.DB.DbProviderType))
                    //    //string connectionString = queryDb.GetConnectionString(Defs.DB_USER, Defs.DB_PASSWD, string.Format("{0}:{1}/{2}", Defs.DB_HOST, Defs.DB_PORT, Defs.DB_NAME));
                    //    //queryDb.Connect(connectionString);
                    //    if (queryDb != null && queryDb.Open())
                    //    {
                    //        DataTable dt = queryDb.QueryDataTable(queryString, bind);
                    //        if (dt != null)
                    //        {
                    //            Result = dt.Copy();
                    //        }
                    //    }
                    //}
                    if (db != null && db.Open() == true)
                    {
                        ;
                        //DataTable dt = this.FDB.QueryDataTable(queryString, bind);
                        //if (dt != null)
                        //{
                        //    Result = dt.Copy();
                        //}
                        Result = db.QueryDataTable(queryString, bind, bStoredProcedure);
                    }

                }
                catch (System.Data.Common.DbException ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
                finally
                {
                    //if (queryDb != null)
                    //{
                    //    queryDb.Close();
                    //    queryDb.Free();
                    //    queryDb = null;
                    //}
                }
            }

            return Result;
        }

        public static object GetQueryOne(string queryString, Dictionary<string, object> bind = null)
        {
            return GetQueryOne(DB, queryString, bind);
        }
        public static object GetQueryOne(IHxDb db, string queryString, Dictionary<string, object> bind = null)
        {
            return db?.QueryOne(queryString, bind);
        }

        public static bool InsertDefaultColumnMatch(string strCustomUser, ref StringBuilder sbrCol, ref StringBuilder sbrVal, ref Dictionary<string, object> bind, decimal? uno = null, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {
            return HxSetValueDbTable.InsertDefaultColumnMatch(strCustomUser, ref sbrCol, ref sbrVal, ref bind, uno, columnsType);
        }
        public static bool InsertDefaultColumnMatch(IHxDb db, decimal? uno, ref StringBuilder sbrCol, ref StringBuilder sbrVal, ref Dictionary<string, object> bind, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {

            return HxSetValueDbTable.InsertDefaultColumnMatch(db, uno, ref sbrCol, ref sbrVal, ref bind, columnsType);
        }
        public static bool UpdateDefaultColumnMatch(string strCustomUser, ref StringBuilder sbrRow, ref Dictionary<string, object> bind, decimal? uno = null, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {
            return HxSetValueDbTable.UpdateDefaultColumnMatch(strCustomUser, ref sbrRow, ref bind, uno, columnsType);

        }
        public static bool UpdateDefaultColumnMatch(IHxDb db, decimal? uno, ref StringBuilder sbrRow, ref Dictionary<string, object> bind, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {
            return HxSetValueDbTable.UpdateDefaultColumnMatch(db, uno, ref sbrRow, ref bind, columnsType);

        }
    }


}
