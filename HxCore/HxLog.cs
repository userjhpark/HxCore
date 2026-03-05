using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace HxCore
{
    internal sealed class HxLogTest : HxBase
    {
        //로깅할 곳에 로그 변수 추가

        //private ILog _logger = null;
        //public readonly ILog Logger
        //{
        //    get { return _logger; }
        //}

        //string LogDirPath;

        public HxLogTest(Type type, string logDirPath, string logConfigFullName = null)
        {
            /*
            //if (!logDirPath.IsNullOrWhiteSpaceEx())
            //{
            //    LogDirPath = logDirPath;
            //} else
            //{
                
            //    LogDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //}

            //if (LogDirPath.IsNullOrWhiteSpaceEx())
            //{
            //    LogDirPath = Path.GetTempPath();
            //}

            //DirectoryInfo dirInfo = new DirectoryInfo(LogDirPath);
            //if (dirInfo.Exists == false)
            //{
            //    if (dirInfo != null)
            //        dirInfo.Create();
            //}
            */


            //Type type = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

            ////LogConfig.xml 경로를 설정해준다.
            ////string appPath = AppDomain.CurrentDomain.BaseDirectory;
            //string appPath = HxUtils.AppBaseDir;
            //XmlConfigurator.Configure( new FileInfo( Path.Combine(appPath, @"bin\LogConfig.xml" ) ));
            ////설정파일의 appender의 name에 아까 입력한 RollingFile을 입력하여 logger를 추가한다.
            //_logger = log4net.LogManager.GetLogger("RollingFile");
            //// ----------- 이렇게 해서 기본적인 변수 선언과 사용준비는 끝
            //ILog log = LogManager.GetLogger("Program");

            //ILog log = LogManager.GetLogger(type);
            //log.debug
        }

        protected override void FreeAndNull()
        {
            //if(Log)
            base.FreeAndNull();
        }

        public static void DebugWriteLog(Exception e, string title = null, string appendMessage = null)
        {
            DebugWriteLog(e?.Message, title, appendMessage);
        }

        public static void DebugWriteLog(string errorMessage, string title = null, string appendMessage = null)
        {
            string strDateTime = DateTime.Now.ToDateTimeStringEx();
            string logMessage = null;
            if (title.IsNullOrWhiteSpaceEx())
            {
                errorMessage = string.Format("[{0}] {1}", title, errorMessage);

            }

            logMessage = string.Format("{0}-{1}", strDateTime, errorMessage);
            Debug.WriteLine(logMessage);
        }
    }

    public class HxLog : TraceListener
    {
        //참고 출처 : https://deois.tistory.com/12
        //#region Static Intance
        //private static HxLog _instance = null;
        //static HxLog()
        //{
        //    _instance = Create();
        //}
        //public static HxLog Instance
        //{
        //    get { return _instance ?? (_instance = Create()); }
        //    private set { _instance = value; }
        //}
        ////internal static TVdcs _
        ////{
        ////    get { return _instance ?? (_instance = Create()); }
        ////    private set { _instance = value; }
        ////}



        //internal static HxLog Create()
        //{
        //    return new HxLog();
        //}

        //public static bool Run(bool bInit = false)
        //{
        //    if (_instance != null || bInit == true)
        //    {
        //        _instance = Create();
        //        if (_instance != null)
        //            return true;
        //    }
        //    return false;
        //}
        //#endregion


        private string LogName { get; set; }
        public string InputFileName { get; protected set; }
        public string SaveFileFullName { get; protected set; }

        private StreamWriter StreamWriterTrace { get; set; }
        private string DateNowString => DateTime.Today.ToDateStringEx();

        public HxLog() : this("logger")
        {
            ; ;
        }

        public HxLog(string name, bool bProcessID = false, bool bFileUnique = false) : base(name)
        {
            LogName = name;
            CreateFile(name, bProcessID, bFileUnique);
        }

        

        public override void Write(string message)
        {
            this.CheckFile();
            if (this.StreamWriterTrace?.BaseStream?.CanWrite == true)
            {
                this.StreamWriterTrace.Write(message);
            }
        }
        public override void WriteLine(string message)
        {
            this.CheckFile();
            if (this.StreamWriterTrace?.BaseStream?.CanWrite == true)
            {
                this.StreamWriterTrace.WriteLine(message);
            }

        }

        
        

        public void WriteLog(string message, HxMessageType messageType = HxMessageType.None)
        {
            int nProcessID = Process.GetCurrentProcess().Id;
            string processID = nProcessID.ToStringEx().PadLeft(5, '0');
            string logMessage = string.Format("{0} {1}-[{2}] {3}", DateTime.Now.ToDateMicroTimeLongStringEx(), processID, messageType.ToStringEx(), message);
            this.WriteLine(logMessage);
        }

        public void WriteLog(Exception ex)
        {
            this.WriteExceptionLog(ex);
        }
        public void WriteMessageLog(string message)
        {
            this.WriteLog(message, HxMessageType.Message);
        }
        public void WriteHistoryLog(string message)
        {
            this.WriteLog(message, HxMessageType.History);
        }

        public void WriteNoticeLog(string message)
        {
            this.WriteLog(message, HxMessageType.Notice);
        }
        public void WriteInformationLog(string message)
        {
            this.WriteLog(message, HxMessageType.Information);
        }
        public void WriteWarningLog(string message)
        {
            this.WriteLog(message, HxMessageType.Warning);
        }
        public void WriteErrorLog(string message)
        {
            this.WriteLog(message, HxMessageType.Error);
        }
        
        public void WriteExceptionLog(Exception ex, string title = null)
        {
            Debug.WriteLine(ex);
            if(title.IsNullOrWhiteSpaceEx() != true)
                this.WriteLog("<<" + title + ">> : " + ex.Message, HxMessageType.Exception);
            else
                this.WriteLog(ex.Message, HxMessageType.Exception);
        }
        


        private void CheckFile(bool bProcessID = false)
        {
            if (this.DateNowString != DateTime.Today.ToDateStringEx())
            {
                this.CloseFile();
                this.CreateFile(LogName, bProcessID);
            }
            if (this.StreamWriterTrace != null && this.StreamWriterTrace?.BaseStream?.CanWrite != true)
            {
                this.CreateFile(LogName, true);
            }

        }
        private void CreateFile(string name, bool bProcessID = false, bool bFileUnique = false)
        {
            SaveFileFullName = GetSaveFileName(name, bProcessID, bFileUnique);

            StreamWriterTrace = new StreamWriter(SaveFileFullName, true)
            {
                AutoFlush = true
            };
        }

        private void CloseFile()
        {
            try
            {
                this.StreamWriterTrace?.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            CloseFile();
            base.Dispose(disposing);
        }

        private string GetSaveFileName(string name, bool bProcessID = false, bool bFileUnique = false)
        {

            //DateNowString = DateTime.Today.ToDateStringEx();

            string fileDirPath = HxFile.GetFileDirPath(name);
            string fileName = HxFile.GetFileName(name);
            string fileExt = HxFile.GetFileNameExt(name);

            if (fileDirPath.IsNullOrWhiteSpaceEx() || (fileDirPath.IsNullOrWhiteSpaceEx() != true && Directory.Exists(fileDirPath) != true))
            {
                Directory.CreateDirectory(fileDirPath);
            }
            if (Directory.Exists(fileDirPath) != true)
            {
                fileDirPath = HxUtils.AppBaseDir;
            }
            if (Directory.Exists(fileDirPath) != true)
            {
                fileDirPath = Path.GetTempPath();
            }
            if (fileExt.IsNullOrWhiteSpaceEx())
            {
                fileExt = "log";
            }
            
            
            //string saveName = string.Format("{0}_{1}.{2}", fileName2, DateNowString, fileExt);

            string saveName = Path.Combine(fileDirPath, fileName);
            if (bProcessID && HxFile.FileExists(saveName) == true)
            {
                string fileName2 = HxFile.GetFileNameWithOutExt(name);
                int nProcessID = Process.GetCurrentProcess().Id;
                string strProcessID = nProcessID.ToStringEx().PadLeft(5, '0');
                saveName = string.Format("{0}_{1}_{3}.{2}", fileName2, DateNowString, fileExt, strProcessID);
                saveName = Path.Combine(fileDirPath, saveName);
            }
            if (bFileUnique == true)
            {
                //saveName = HxFile.GetFileUniquePath(saveName, HxFileOverwriteType.RenameSequence);
                saveName = HxFile.GetFileUniquePath(saveName, HxFileOverwriteType.RenameSequence);
            }
            return saveName;
        }
    }
}
