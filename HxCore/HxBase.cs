using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace HxCore
{
    public abstract class HxBase : HxDefs, IHxBase
    {
        #region System Define / Const
        //public const string _CUSTOM_USER_AGENT_ = "CUSTOM_USER_AGENT";
        //public const string _CUSTOM_USER_AGENT_ = HxDefs._CUSTOM_USER_AGENT_;
        //public const string _USER_AGENT_ = HxDefs._USER_AGENT_;
        //public const string _REMOTE_ADDR_ = HxDefs._REMOTE_ADDR_;
        //public const string _REFERER_ = HxDefs._REFERER_;
        //public const string _HOST_ = HxDefs._HOST_;
        //public const string _QUERY_STRING_ = HxDefs._QUERY_STRING_;
        //
        //public const string _HTTP_USER_AGENT_ = HxDefs._HTTP_USER_AGENT_;
        //public const string _HTTP_REFERER_ = HxDefs._HTTP_REFERER_;
        //public const string _HTTP_HOST_ = HxDefs._HTTP_HOST_;
        //public const string _REQUEST_SCHEME_ = HxDefs._REQUEST_SCHEME_; //$_SERVER['REQUEST_SCHEME'] : URI 스킴 - http
        //public const string _REQUEST_URI_ = HxDefs._REQUEST_URI_; //$_SERVER['REQUEST_URI'] : 요청 URI. 이 페이지에 접근하기 위해 입력한 URI - /index.html
        #endregion

        protected bool IsCreated = false;

        /// <summary>
        /// Class Name
        /// </summary>
        public virtual string Name
        {
            get { return this.GetName(); }
        }
        /// <summary>
        /// Class Name
        /// </summary>
        /// <returns>Class Name</returns>
        public virtual string GetName(){
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            //System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
            //NET35 return typeof(TwilioTest).Assembly;
            //return new AutoRun(typeof(TwilioTest).GetTypeInfo().Assembly).Execute(args);
            //retrun = new AutoRun(typeof(UbMainForm).GetTypeInfo().Assembly).Execute(args);
            //Application.Run(MainForm = new UbMainForm(args));
            //int ip = new AutoRun(typeof(HxCore.HxCrypt).GetTypeInfo().Assembly).Execute(args);
        }

        public HxBase()
        {
            Construct();
        }
        protected virtual void Construct()
        {
            if (IsCreated != true)
            {
                DebugMessage(string.Format("* * * * Create Class {0} * * * *", this.Name));
            }
        }

        #region 파괴자
        /// <summary>
        /// 파괴 여부
        /// </summary>
        protected bool IsDisposed = false;

        

        /// <summary>
        /// 파괴 Method
        /// </summary>
        public virtual void Dispose()
        {
            //throw new NotImplementedException();
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize in next line.
            this.Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.Collect();
            GC.WaitForFullGCComplete();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Class내에서 할당된 Resource 해제
        /// </summary>
        protected virtual void FreeAndNull()
        {
            ; ;
        }

        /// <summary>
        /// 파괴 Method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!this.IsDisposed)
                {
                    this.IsDisposed = true;
                    // Released unmanaged Resources
                    if (disposing == true)
                    {
                        FreeAndNull();
                    }
                }
            }
            finally
            {
                //base.Dispose(disposedStatus);
                this.IsDisposed = true;
            }
        }

        /// <summary>
        /// 파괴자
        /// </summary>
        ~HxBase()
        {
            this.Dispose(false);
        }
        #endregion

        #region 디버깅 처리
        /// <summary>
        /// 디버깅 메세지 출력
        /// </summary>
        /// <param name="message">Text Message</param>
        [System.Diagnostics.Conditional("DEBUG")]
        protected virtual void DebugMessage(string message)
        {
            StringBuilder sb = new StringBuilder();
            string str;
            sb.Append("\n**************************************************************************************************\n");
            sb.Append(message);
            sb.Append("\n**************************************************************************************************\n");
            str = sb.ToString();
            sb.Replace(str, null);
            Console.WriteLine(str);
            //Debug.WriteLine(str);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugMessageOut(string message)
        {
            StringBuilder sb = new StringBuilder();
            string str = null;
            sb.Append("\n**************************************************************************************************\n");
            sb.Append(message);
            sb.Append("\n**************************************************************************************************\n");
            str = sb.ToString();
            sb.Clear();
            Console.WriteLine(str);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugLogWrite(log4net.ILog log, object message)
        {
            if(log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Debug(message);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugInfoLogWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Info(message);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugWarnLogWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Warn(message);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugErrorLogWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Error(message);
            }
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugFatalLogWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Fatal(message);
            }
        }

        public static void LogWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Debug(message);
              
            }
        }
        public static void LogInfoWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Info(message);
            }
        }
        public static void LogWarnWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Warn(message);
            }
        }
        public static void LogErrorWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Error(message);
            }
        }
        public static void LogFatalWrite(log4net.ILog log, object message)
        {
            if (log != null && log.IsDebugEnabled == true && message != null)
            {
                log.Fatal(message);
            }
        }

        #endregion

        #region Methods
        public static void SetEncryptColumnToRowValueChage(DataTable sender, string cryptColName, string cryptKey = null)
        {
            //#warning "구현 해야함!"
            if (cryptColName.IsNullOrWhiteSpaceEx() != true && sender != null && sender.Rows.Count > 0 && sender.Columns.Contains(cryptColName))
            {
                int n = sender.Rows.Count;
                for (int i = 0; i < n; i++)
                {
                    DataRow row = sender.Rows[i];
                    string value = row[cryptColName].ToStringEx();
                    if (value.IsNullOrWhiteSpaceEx() != true)
                    {
                        row[cryptColName] = HxCrypt.Encrypt(value, cryptKey);
                    }
                }
                sender.AcceptChanges();
            }
        }
        #endregion
    }
}
