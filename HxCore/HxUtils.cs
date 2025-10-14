using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Management;

namespace HxCore
{
    public partial class HxUtils : HxBase
    {
        public override string GetName()
        {
            string Result = base.GetName();
            if(Result.IsNullOrWhiteSpaceEx() == true)
                Result = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            return Result;
        }

        public static void DebuggerLaunch()
        {
            System.Diagnostics.Debugger.Launch();
        }

        //protected static HxEnvVars CacheEnvVars { get; set; }

        public const string _CDF_API_KEY_NM_ = HxOpenApiDbRec._CDF_KEY_NAME_;
        public const string _CDF_API_PASS_NM_ = HxOpenApiDbRec._CDF_PASS_NAME_;


        public static HxSourceConnectionRec SourceConnInfo { get; protected set; } = default;
        public static HxDbConnectionRec DbConnInfo { get; protected set; } = SourceConnInfo.DbConnInfo;

        public static HxOpenApiJsonRec ApiConnInfo { get; protected set; } = SourceConnInfo.OpenApiInfo;


        #region Static Intance
        /*
        private static HxUtils _instance = null;
        static HxUtils()
        {
            _instance = new HxUtils();
        }

        public HxUtils()
        {
        }

        /// <summary>
        /// [Static]Instance Object
        /// </summary>
        protected static HxUtils Instance
        {
            get { return _instance ?? (_instance = new HxUtils()); }
            private set { _instance = value; }
        }
        */
        #endregion

        #region OS 관련
        /// <summary>
        /// OS 64Bit?
        /// </summary>
        public static bool IsOS64Bit
        {
            get { return Environment.Is64BitOperatingSystem; }
        }
        /// <summary>
        /// Application 64Bit Run?
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsApp64Bit
        {
            get { return IntPtr.Size == 8 ? true : false; }
            //if (IntPtr.Size == 8)
            //    return true;
            //else
            //    return false;

        }

        public static bool IsCpu64Bit
        {
            get { return Environment.Is64BitProcess; }
        }

        public static bool IsOSWindows
        {
            get { return GetIsOSPlatformWindows(); }
        }
        public static bool GetIsOSPlatformWindows()
        {
            //return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
            return GetOSPlatform() == OSPlatform.Windows ? true : false;
            
        }
        public static bool GetIsOSPlatformOSX()
        {
            //return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
            return GetOSPlatform() == OSPlatform.OSX ? true : false;
        }
        public static bool GetIsOSPlatformLinux()
        {
            //return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
            return GetOSPlatform() == OSPlatform.Linux ? true : false;
        }
        public static System.Runtime.InteropServices.OSPlatform GetOSPlatform()
        {
            System.Runtime.InteropServices.OSPlatform Result = System.Runtime.InteropServices.OSPlatform.Create(System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            try
            {
                if (GetIsOSPlatformWindows() == true)
                {
                    Result = System.Runtime.InteropServices.OSPlatform.Windows;
                }
                else if (GetIsOSPlatformOSX() == true)
                {
                    Result = System.Runtime.InteropServices.OSPlatform.OSX;
                } else if(GetIsOSPlatformLinux() == true)
                {
                    Result = System.Runtime.InteropServices.OSPlatform.Linux;
                }
                else
                {
                    //참조 : https://stackoverflow.com/questions/38790802/determine-operating-system-in-net-core
                    //bool bWindows = false;
                    //bool bLinux = false;
                    //bool bMacOsX = false;
                    string windir = Environment.GetEnvironmentVariable("windir");
                    if (!string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir))
                    {
                        //bWindows = true;
                        Result = System.Runtime.InteropServices.OSPlatform.Windows;
                    }
                    else if (File.Exists(@"/proc/sys/kernel/ostype"))
                    {
                        string osType = File.ReadAllText(@"/proc/sys/kernel/ostype");
                        if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
                        {
                            // Note: Android gets here too
                            //bLinux = true;
                            Result = System.Runtime.InteropServices.OSPlatform.Linux;
                        }
                    }
                    else if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
                    {
                        // Note: iOS gets here too
                        //bMacOsX = true;
                        Result = System.Runtime.InteropServices.OSPlatform.OSX;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                //throw ex;
            }
            return Result;
            
        }

        /// <summary>
        /// OS의 폴더 구분자
        /// </summary>
        /// <returns>구분자</returns>
        public static char DirSeparatorChar => Path.DirectorySeparatorChar;

        /// <summary>
        /// 실행 폴더 경로(폴더명 맨뒤에 구분 기호 포함)
        /// </summary>
        public static string AppBaseDir
        {
            get { return GetAppBaseDir(true); }
        }

        private static string GetAppFullPath() 
        {
            string Result = Assembly.GetExecutingAssembly().Location;
            if (Result.IsNullOrWhiteSpaceEx() == true)
                Result = Assembly.GetExecutingAssembly().GetName().CodeBase;
            Result = Result.Replace("file:\\", null);
            return Result;
        }
        /// <summary>
        /// Application 실행 경로
        /// </summary>
        /// <param name="bEndDirSeparatorChar">실행경로 맨뒤 폴더 구분기호 포함 여부</param>
        /// <returns>폴더 경로</returns>
        public static string GetAppBaseDir(bool bEndDirSeparatorChar = true)
        {
            string Result = string.Empty;
            try
            {
                //Result = string.Format(".{0}", DirSeparatorChar);
                //Result = Path.GetFullPath(Result);
                ////Result = Application.StartupPath;
                //Result = AppDomain.CurrentDomain.BaseDirectory;
                //Result = System.IO.Directory.GetCurrentDirectory();
                //Result = Environment.CurrentDirectory;
                //Result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //Result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                Result = AppDomain.CurrentDomain.BaseDirectory;

                //Result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                Result = Result.Replace("file:\\", null);
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    Result = System.IO.Directory.GetCurrentDirectory();
                }
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    Result = Environment.CurrentDirectory;
                }
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    Result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    Result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                }
                // Result = System.IO.Path.GetDirectory(Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                //Result = System.Environment.CurrentDirectory; // System.IO.Directory.GetCurrentDirectory()
                Debug.WriteLine(ex.Message.ToString());
            }
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                Result = @"." + DirSeparatorChar;
                Result = Path.GetFullPath(Result);
            }

            if (bEndDirSeparatorChar == true)
            {
                if (Result.IsNullOrWhiteSpaceEx() != true && Result.EndsWith(DirSeparatorChar.ToString()) != true)
                {
                    Result += DirSeparatorChar;
                }
            }
            return Result;
        }

        public static string GetAppCurrDir(bool bEndDirSeparatorChar = true)
        {
            string Result = HxUtils.AppBaseDir;
            if (Result.IsNullOrWhiteSpaceEx())
            {
                Result = @"." + DirSeparatorChar;
                Result = Path.GetFullPath(Result);
            }
            if (bEndDirSeparatorChar == true)
            {
                if (Result.IsNullOrWhiteSpaceEx() != true && Result.EndsWith(DirSeparatorChar.ToString()) != true)
                {
                    Result += DirSeparatorChar;
                }
            }
            return Result;
        }

        private bool Test()
        {
            bool Result = false;
            /*
#if defined(_WIN64)
                    bIs64Bit = true;
#elif defined(_WIN32)
                    bis64Bit = false;
#endif
            */
            return Result;
        }

        #endregion

        

        #region Array / List / Dictionary
        public static string GetArrayJoin(string[] values, string separatorChar = " ", string formatString = "{0}")
        {
            /*
            string Result = null;
            //StringBuilder builder
            StringBuilder builder = new StringBuilder();
            if (values != null && values.Length > 0)
            {
                foreach (string s in values)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(separatorChar);
                    }
                    builder.AppendFormat(formatString, s);
                }
            }
            if (builder.Length > 0)
            {
                Result = builder.ToStringEx();
                builder.Clear();
            }
            return Result;
            */
            return HxString.GetArrayJoin(values, separatorChar, formatString);
        }
        public static string GetArrayJoin(object[] values, string separatorChar = " ", string formatString = "{0}")
        {
            return HxString.GetArrayJoin(values, separatorChar, formatString);
        }
#endregion

        public static List<T> AttributeList<T>(System.Reflection.MemberInfo sender)
            where T : System.Attribute
        {
            List<T> Result = new List<T>();
            //Type type = sender.GetType();
            //Type type = typeof(T);
            //object[] attributes = type.GetCustomAttributes(true);
            object[] attributes = sender.GetCustomAttributes(true);
            if (attributes.Length > 0)
            {
                foreach (object attribute in attributes)
                {
                    //Console.Write("  {0}", attribute.ToString());
                    T da = (attribute as T);
                    if (da != null)
                    {
                        //Console.WriteLine(".Description={0}", da.Description);
                        Result.Add(da);
                    }
                }
            }
            else
            {
                Result.Clear();
                Result = null;
            }
            return Result;
        }
        /// <summary>
        /// Type To String Name
        /// </summary>
        /// <param name="type">Object Type</param>
        /// <returns>String Name</returns>
        public static string ObjectTypeString(Type type)
        {
            string Result;
            if (type == typeof(char) || type == typeof(Char))
            {
                Result = "CHAR";
            }
            else if (type == typeof(string) || type == typeof(String))
            {
                Result = "STRING";
            }
            else if (type == typeof(int) || type == typeof(uint) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64) || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64))
            {
                Result = "NUMBER";
            }
            else if (type == typeof(decimal) || type == typeof(Decimal) || type == typeof(double) || type == typeof(Double) || type == typeof(float))
            {
                Result = "NUMBERIC";
            }
            else if (type == typeof(bool) || type == typeof(Boolean))
            {
                Result = "IS";
            }
            else
            {
                Result = type.GetType().ToString();
                //Result = typeof(type).ToString();
            }
            return Result;
        }

        #region Compare / Diff.
        //출처 : https://stackoverflow.com/questions/10454519/best-way-to-compare-two-complex-objects
        public static bool DeepCompare(object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            //Compare two object's class, return false if they are difference
            if (obj.GetType() != another.GetType()) return false;

            var result = true;
            //Get all properties of obj
            //And compare each other
            foreach (var property in obj.GetType().GetProperties())
            {
                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                if (!objValue.Equals(anotherValue)) result = false;
            }

            return result;
        }

        public static bool Compare(object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            //properties: int, double, DateTime, etc, not class
            if (!obj.GetType().IsClass) return obj.Equals(another);

            var result = true;
            foreach (var property in obj.GetType().GetProperties())
            {
                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                //Recursion
                if (!DeepCompare(objValue, anotherValue)) result = false;
            }
            return result;
        }

        public static bool JsonCompare(object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            var objJson = JsonConvert.SerializeObject(obj);
            var anotherJson = JsonConvert.SerializeObject(another);

            return objJson == anotherJson;
        }
        #endregion


        //public static bool IsNullOrMinus<T>(T value)
        //    where T : IComparable, IComparable<Int16>, IComparable<Int16?>, IComparable<Int32>, IComparable<Int32?>, IComparable<Int64>, IComparable<Int64?>
        //{
        //    object val = value.Conver
        //}

        #region Process 관련
        /// <summary>
        /// Get 프로세스 이름으로 프로세스 목록 가져오기
        /// </summary>
        /// <param name="processName">프로세스 이름</param>
        /// <param name="bMutex">현재 실행 프로세스 제외 여부?</param>
        /// <returns>프로세스 리스트</returns>
        public static List<Process> ProcessesByName(string processName, bool bMutex = true)
        {
            List<Process> Result = null;
            //var a = process
            Process[] findPorcess = System.Diagnostics.Process.GetProcessesByName(processName);
            Process currPorcess = System.Diagnostics.Process.GetCurrentProcess();
            foreach (Process proc in findPorcess)
            {
                if (bMutex == true)
                {
                    if (proc.Id != currPorcess.Id)
                    {
                        if (Result == null)
                            Result = new List<Process>();
                        Result.Add(proc);
                    }
                }
                else
                {
                    if (Result == null)
                        Result = new List<Process>();
                    Result.Add(proc);
                }
            }
            return Result;
        }
        /// <summary>
        /// Get 프로세스 이름으로 프로세스중 처음 한개만 가져오기
        /// </summary>
        /// <param name="processName">프로세스 이름</param>
        /// <param name="bMutex">현재 실행 프로세스 제외 여부?</param>
        /// <returns>프로세스</returns>
        public static Process ProcessOneByName(string processName, bool bMutex = true)
        {
            List<Process> ProcessList = ProcessesByName(processName, bMutex);
            if(ProcessList != null && ProcessList.Any() == true && ProcessList.Count > 0)
            {
                return ProcessList[0] ?? ProcessList[ProcessList.Count - 1] ?? null;
                //return ProcessList
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// System.Drawing.Point에 상대 Point 더하기
        /// </summary>
        /// <param name="sender">기준이 될 Point</param>
        /// <param name="width">덧셈 할 폭(X) 값</param>
        /// <param name="height">덧셈 할 폭(Y) 값</param>
        /// <returns>덧셈이 된 상대 Point</returns>
        public static System.Drawing.Point GetDrawingPointAdd(System.Drawing.Point ptSource, int width, int height)
        {
            System.Drawing.Point Result;

            Result = ptSource;
            Result.X += width;
            Result.Y += height;

            return Result;
        }
        /// <summary>
        /// System.Drawing.Point에 상대 Point 더하기
        /// </summary>
        /// <param name="sender">기준이 될 Point</param>
        /// <param name="addPoint">덧셈 할 Point</param>
        /// <returns>덧셈이 된 상대 Point</returns>
        public static System.Drawing.Point GetDrawingPointAdd(System.Drawing.Point ptSource, System.Drawing.Point addPoint)
        {
            System.Drawing.Point Result;

            Result = ptSource;
            Result.X += addPoint.X;
            Result.Y += addPoint.Y;

            return Result;
        }
        

        /// <summary>
        /// 1900-01-01 최소 날짜(DB등 호환용 임의 지정)
        /// </summary>
        /// <returns></returns>
        public static DateTime MinDateTime()
        {
            return new DateTime(1900, 1, 1);
        }
        /// <summary>
        /// DateTime Now
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime NowDateTime()
        {
            //return DateTime.Now;
            return HxString.GetNowDateTime();
        }
        
        /// <summary>
        /// DateTime Now
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime TodayDateTime()
        {
            return NowDateTime();
        }


        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyy-MM-dd HH:mm:ss.fffffff</param>
        /// <returns>DateTime String</returns>
        public static string NowLongDateTime(string dateFormat = "yyyy-MM-dd HH:mm:ss.fffffff")
        {
            return HxString.GetNowDateTimeString(dateFormat);
        }
        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyyMMddHHmmssfffffff)</param>
        /// <returns>DateTime String</returns>
        public static string NowLongDateTimeString(string dateFormat = "yyyyMMddHHmmssfffffff")
        {
            return HxString.GetNowDateTimeString(dateFormat);
        }
        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyyMMddHHmmss)</param>
        /// <returns>DateTime String</returns>
        public static string NowDateTimeString(string dateFormat = "yyyyMMddHHmmss")
        {
            return HxString.GetNowDateTimeString(dateFormat);
        }
        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyyMMddHHmmss)</param>
        /// <returns>DateTime String</returns>
        public static string NowDateTime2String(string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return HxString.GetNowDateTimeString(dateFormat);
        }
        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyyMMdd)</param>
        /// <returns>DateTime String</returns>
        public static string NowDateString(string dateFormat = "yyyyMMdd")
        {
            return HxString.GetNowDateTimeString(dateFormat);
        }



#region DateTimeUtils //DevExpress.SalesDemo.Model
        public static HxDateTimeRange DayRange(DateTime date)
        {
            DateTime startOfDate = new DateTime(date.Year, date.Month, date.Day);
            DateTime endOfToday = startOfDate.AddDays(1).AddTicks(-1);
            return new HxDateTimeRange(startOfDate, endOfToday);
        }
        public static HxDateTimeRange TodayRange()
        {
            return DayRange(DateTime.Now);
        }
        public static HxDateTimeRange YesterdayRange()
        {
            return DayRange(DateTime.Now.AddDays(-1));
        }
        public static HxDateTimeRange LastWeekRange()
        {
            DayOfWeek firstDay = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime today = DateTime.Today;
            DateTime startOfWeek = today;
            while (startOfWeek.DayOfWeek != firstDay)
            {
                startOfWeek = startOfWeek.AddDays(-1);
            }
            DateTime endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);
            return new HxDateTimeRange(startOfWeek, endOfWeek);
        }
        public static HxDateTimeRange MonthRange(DateTime date)
        {
            DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);
            int daysInCurrentMonth = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime endOfMonth = startOfMonth.AddDays(daysInCurrentMonth).AddTicks(-1);
            return new HxDateTimeRange(startOfMonth, endOfMonth);
        }
        public static HxDateTimeRange ThisMonthRange()
        {
            return MonthRange(DateTime.Now);
        }
        public static HxDateTimeRange LastMonthRange()
        {
            return MonthRange(DateTime.Now.AddMonths(-1));
        }
        public static HxDateTimeRange YtdRange()
        {
            DateTime today = DateTime.Today;
            DateTime startOfYear = new DateTime(today.Year, 1, 1);
            DateTime endOfYear = today;// startOfYear.AddYears(1).AddTicks(-1);
            return new HxDateTimeRange(startOfYear, endOfYear);
        }
        public static HxDateTimeRange OneYearRange()
        {
            return new HxDateTimeRange(DateTime.Today.AddYears(-1), DateTime.Today);
        }

        public static HxDateTimeRange YearRange(DateTime date)
        {
            DateTime startOfYear = new DateTime(date.Year, 1, 1);
            DateTime endOfYear = startOfYear.AddYears(1).AddTicks(-1);
            return new HxDateTimeRange(startOfYear, endOfYear);
        }
        public static HxDateTimeRange LastYearRange()
        {
            return YearRange(DateTime.Today.AddYears(-1));
        }
        public static int LastYear()
        {
            return DateTime.Today.AddYears(-1).Year;
        }
        public static object CurrentYear()
        {
            return DateTime.Now.Year;
        }

        public static bool IsCurrentYear(DateTime date)
        {
            DateTime now = DateTime.Now;
            return now.Year == date.Year;
        }
        public static bool IsCurrentMonth(DateTime date)
        {
            DateTime now = DateTime.Now;
            return IsCurrentYear(date) && now.Month == date.Month;
        }
        public static bool IsToday(DateTime date)
        {
            DateTime now = DateTime.Now;
            return IsCurrentMonth(date) && now.Day == date.Day;
        }
#endregion
        public static Version RunCoreLibraryAssemblyVersion()
        {
            //try
            //{
            //출처: http://withsoju.tistory.com/637 [읽든지 말든지]
            //    //게시(Clickonce 등 일 경우) 응용프로그램 버전
            //    return string.Format("{0} (배포버전)",
            //        System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion);
            //}
            //catch
            //{
            //    //로컬 빌드 버전일 경우 (현재 어셈블리 버전)
            //    return string.Format("{0} (빌드버전)",
            //        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            //}
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
        [Obsolete("GetFileVersion()를 사용 하자.")]
        public static Version FileVersion(FileInfo file)
        {
            return GetFileVersion(file);
        }
        [Obsolete("GetFileVersion()를 사용 하자.")]
        public static Version FileVersion(string path)
        {
            return GetFileVersion(path);
        }

        public static Version GetFileVersion(string path)
        {
            Version Result = null;
            try
            {
                if (File.Exists(path))
                {
                    FileInfo fi = new FileInfo(path);
                    Result = GetFileVersion(fi);
                    //Result = new Version();
                    //string fullPath = Path.GetFullPath(path);
                    //FileVersionInfo fileVerInfo = FileVersionInfo.GetVersionInfo(fullPath);
                    //if (fileVerInfo != null && !fileVerInfo.FileVersion.IsNullOrWhiteSpaceEx())
                    //{
                    //    bool bSuccess = Version.TryParse(fileVerInfo.FileVersion, out Result);
                    //    if (!bSuccess)
                    //    {
                    //        int major = fileVerInfo.FileMajorPart;
                    //        int minor = fileVerInfo.FileMinorPart;
                    //        int build = fileVerInfo.FileBuildPart;
                    //        int revision = fileVerInfo.FilePrivatePart;
                    //        Result = new Version(major, minor, build, revision);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = null;
                //throw;
            }
            return Result;
        }
        public static Version GetFileVersion(FileInfo file)
        {
            Version Result = null;
            try
            {
                if (file.Exists)
                {
                    //Result = new Version();
                    string fullPath = file.FullName;
                    FileVersionInfo fileVerInfo = FileVersionInfo.GetVersionInfo(fullPath);
                    if (fileVerInfo != null && !fileVerInfo.FileVersion.IsNullOrWhiteSpaceEx())
                    {
                        bool bSuccess = Version.TryParse(fileVerInfo.FileVersion, out Result);
                        if (!bSuccess)
                        {
                            int major = fileVerInfo.FileMajorPart;
                            int minor = fileVerInfo.FileMinorPart;
                            int build = fileVerInfo.FileBuildPart;
                            int revision = fileVerInfo.FilePrivatePart;
                            Result = new Version(major, minor, build, revision);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = null;
                //throw;
            }
            return Result;
        }
        public static Version GetAssemblyVersion(string fileName = null)
        {
            try
            {
                if (fileName.IsNullOrWhiteSpaceEx() != true && File.Exists(fileName))
                {
                    return Assembly.LoadFile(fileName).GetName().Version;
                }
                else
                {
                    return Assembly.GetExecutingAssembly().GetName().Version;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return null;
        }

        public static string GetFileVersionString(string fileName = null)
        {
            try
            {
                return GetFileVersionInfo(fileName)?.FileVersion;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return null;
        }
        public static string GetProductVersion(string fileName = null)
        {
            try
            {
                return GetFileVersionInfo(fileName)?.FileVersion;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return null;
        }

        public static FileVersionInfo GetFileVersionInfo(string fileName = null)
        {
            try
            {
                if (fileName.IsNullOrWhiteSpaceEx() == true || File.Exists(fileName) != true)
                {
                    fileName = Assembly.GetExecutingAssembly().Location;
                }
                return FileVersionInfo.GetVersionInfo(fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return null;
        }

        public static Version GetAssemblyCurrentVersion()
        {
#if DEBUG
            string assemblyVersion1 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string assemblyVersion2 = Assembly.LoadFile("your assembly file").GetName().Version.ToString();
            string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
#endif
            return Assembly.GetExecutingAssembly().GetName().Version;
        }


        //public static long
        [Obsolete("GetVersionNumber()를 사용하자.")]
        public static long VersionNumber(Version version)
        {
            return GetVersionNumber(version);
        }
        public static long GetVersionNumber(Version version)
        {
            long Result = long.MaxValue;
            if (version != null)
            {
                Result = InputVersionNumber(version.Major, version.Minor, version.Build, version.Revision);
            }
            return Result;
        }
        [Obsolete("GetVersionNumber()를 사용하자.")]
        public static long VersionNumber(int majorPart, int minorPart = 0, int buildPart = 0, int revisionPart = 0)
        {
            return GetVersionNumber(majorPart, minorPart, buildPart, revisionPart);
        }
        public static long GetVersionNumber(int majorPart, int minorPart = 0, int buildPart = 0, int revisionPart = 0)
        {
            return InputVersionNumber(majorPart, minorPart, buildPart, revisionPart);
        }
        [Obsolete("GetVersionNumber()를 사용하자.")]
        public static long VersionNumber(string input)
        {
            return GetVersionNumber(input);
        }
        public static long GetVersionNumber(string input)
        {
            long Result = long.MaxValue;
            try
            {
                bool bSuccess = Version.TryParse(input, out Version version);
                if (!bSuccess)
                {
                    string[] arry = input.Split('.');
                    if(arry != null && arry.Length > 0)
                    {
                        int major = arry[0].ToIntEx();
                        int minor = 0;
                        int build = 0;
                        int revision = 0;
                        if (arry.Length > 1)
                            minor = arry[1].ToIntEx();
                        if (arry.Length > 2)
                            build = arry[2].ToIntEx();
                        if (arry.Length > 3)
                            revision = arry[3].ToIntEx();
                        Result = InputVersionNumber(major, minor, build, revision);
                    }
                    
                } else
                {
                    Result = InputVersionNumber(version);
                }
            }
            catch (Exception ex)
            {
                Result = -1;
                Debug.WriteLine(ex);
                //throw;
            }
            return Result;
        }

        public static bool? IsVersionUpdateCheck(Version currVersion, Version updateVersion, HxVersionType versionType = HxVersionType.All)
        {
            bool? Result = null;
            if (currVersion != null && updateVersion != null)
            {
                long currVersionNumber;
                long updateVersionNumber;
                switch (versionType)
                {
                    case HxVersionType.Major:
                        currVersionNumber = GetVersionNumber(currVersion.Major);
                        updateVersionNumber = GetVersionNumber(updateVersion.Major);
                        break;
                    case HxVersionType.Minor:
                        currVersionNumber = GetVersionNumber(currVersion.Major, currVersion.Minor);
                        updateVersionNumber = GetVersionNumber(updateVersion.Major, updateVersion.Minor);
                        break;
                    case HxVersionType.Build:
                        currVersionNumber = GetVersionNumber(currVersion.Major, currVersion.Minor, currVersion.Build);
                        updateVersionNumber = GetVersionNumber(updateVersion.Major, updateVersion.Minor, updateVersion.Build);
                        break;
                    case HxVersionType.Revision:
                    case HxVersionType.All:
                    case HxVersionType.None:
                    default:
                        currVersionNumber = GetVersionNumber(currVersion);
                        updateVersionNumber = GetVersionNumber(updateVersion);
                        break;
                }
                if (currVersionNumber < updateVersionNumber)
                {
                    Result = true;
                }
                else if (currVersionNumber >= 0 || updateVersionNumber < 0)
                {
                    Result = null;
                }
                else
                {
                    Result = false;
                }
            }
            return Result;
        }

        public static long FileVersionNumber(FileInfo file)
        {
            long Result = long.MinValue;
            try
            {
                if (file != null && file.Exists)
                {
                    string fullPath = file.FullName;
                    FileVersionInfo fileVerInfo = FileVersionInfo.GetVersionInfo(fullPath);
                    if (fileVerInfo != null)
                    {
                        //string ver = fileVerInfo.FileVersion;
                        //string desc = fileVerInfo.FileDescription;
                        //string ver2 = fileVerInfo.ProductVersion;
                        int major = fileVerInfo.FileMajorPart;
                        int minor = fileVerInfo.FileMinorPart;
                        int build = fileVerInfo.FileBuildPart;
                        int revision = fileVerInfo.FilePrivatePart;
                        Result = InputVersionNumber(major, minor, build, revision);
                    }
                }
            }
            catch (Exception ex)
            {
                Result = -1;
                Debug.WriteLine(ex);
                //throw ex;
            }

            return Result;
        }
        public static long FileVersionNumber(string path)
        {
            long Result = long.MinValue;
            try
            {
                if (File.Exists(path))
                {
                    Result = FileVersionNumber(new FileInfo(path));
                    //string fullPath = Path.GetFullPath(path);
                    //FileVersionInfo fileVerInfo = FileVersionInfo.GetVersionInfo(fullPath);
                    //if (fileVerInfo != null)
                    //{
                    //    //string ver = fileVerInfo.FileVersion;
                    //    //string desc = fileVerInfo.FileDescription;
                    //    //string ver2 = fileVerInfo.ProductVersion;
                    //    int major = fileVerInfo.FileMajorPart;
                    //    int minor = fileVerInfo.FileMinorPart;
                    //    int build = fileVerInfo.FileBuildPart;
                    //    int revision = fileVerInfo.FilePrivatePart;
                    //    Result = GetInputVersionNumber(major, minor, build, revision);
                    //}
                }
            }
            catch (Exception ex)
            {
                Result = -1;
                Debug.WriteLine(ex);
                //throw ex;
            }
            
            return Result;
        }

        private static long InputVersionNumber(Version version)
        {
            long Result = long.MinValue;
            if (version != null)
            {
                Result = InputVersionNumber(version.Major, version.Minor, version.Build, version.Revision);
            }
            return Result;
        }
        private static long InputVersionNumber(int majorPart, int minorPart = 0, int buildPart = 0, int revisionPart = 0)
        {
            long Result;
            try
            {
                long major = majorPart > 0   ? majorPart   * (long)Math.Pow(10, 9) : 0;
                long minor = minorPart > 0   ? minorPart   * (long)Math.Pow(10, 6) : 0;
                long build = buildPart > 0   ? buildPart   * (long)Math.Pow(10, 3) : 0;
                long revision = revisionPart > 0 ? revisionPart * (long)Math.Pow(10, 1) : 0;
                Result = major + minor + build + revision;
            }
            catch (Exception ex)
            {
                Result = -1;
                Debug.WriteLine(ex);
                //throw ex;
            }
            
            return Result;
        }
        private static long InputVersionNumber(string input)
        {
            long Result = long.MaxValue;
            try
            {
                bool bSuccess = Version.TryParse(input, out Version version);
                if (!bSuccess)
                {
                    string[] arry = input.Split('.');
                    if (arry != null && arry.Length > 0)
                    {
                        int major = arry[0].ToIntEx();
                        int minor = 0;
                        int build = 0;
                        int revision = 0;
                        if (arry.Length > 1)
                            minor = arry[1].ToIntEx();
                        if (arry.Length > 2)
                            build = arry[2].ToIntEx();
                        if (arry.Length > 3)
                            revision = arry[3].ToIntEx();
                        Result = InputVersionNumber(major, minor, build, revision);
                    }

                }
                else
                {
                    Result = InputVersionNumber(version);
                }
            }
            catch (Exception ex)
            {
                Result = -1;
                Debug.WriteLine(ex);
                //throw;
            }
            return Result;
        }

        public static Version InputVersion(string input)
        {
            if (!Version.TryParse(input, out Version Result))
            {
                Result = null;
            }
            return Result;
        }



        public static string GetQueryString(string queryString, string mWhere)
        {
            return WhereQueryString(queryString, mWhere);
        }

        /// <summary>
        /// Get SQL/Query String 
        /// </summary>
        /// <param name="queryString">기본 쿼리</param>
        /// <param name="mWhere">조건절</param>
        /// <returns>SQL 문자열</returns>
        public static string WhereQueryString(string queryString, string mWhere)
        {
            return HxString.WhereQueryString(queryString, mWhere);
        }

        /// <summary>
        /// Get SQL/Query String 
        /// </summary>
        /// <param name="queryString">기본 쿼리</param>
        /// <param name="mWhereArry">조건절 Array</param>
        /// <returns>SQL 문자열</returns>
        public static string WhereQueryString(string queryString, params string[] mWhereArry)
        {
            return HxString.WhereQueryString(queryString, mWhereArry);
        }
        /// <summary>
        /// Get SQL/Query String 
        /// </summary>
        /// <param name="queryString">기본 쿼리</param>
        /// <param name="mOrderBy">정렬 조건</param>
        /// <returns>SQL 문자열</returns>
        public static string OrderByQueryString(string queryString, string mOrderBy)
        {
            return HxString.OrderByQueryString(queryString, mOrderBy);
        }

        /// <summary>
        /// Get SQL/Query String / SELECT * FROM ( inputQueryString ) WHERE 1 = 1
        /// </summary>
        /// <param name="queryString">기본 쿼리</param>
        /// <param name="mWhere">조건절</param>
        /// <param name="mWhere2">조건절2</param>
        /// <param name="mWhere3">조건절2</param>
        /// <returns>SQL 문자열</returns>
        
        public static string SelectQueryString(string queryString, string mWhere = null, string mOrderBy = null) //string queryString, string mWhere, string mWhere2 = null, string mWhere3 = null)
        {
            return HxString.SelectQueryString(queryString, mWhere, mOrderBy);
        }
        /// <summary>
        /// Get SQL/Query String / SELECT * FROM ( inputQueryString ) WHERE 1 = 1
        /// </summary>
        /// <param name="queryString">기본 쿼리</param>
        /// <param name="mWhereParams">조건절 Array</param>
        /// <returns>SQL 문자열</returns>
        public static string SelectQueryString(string queryString, params string[] mWhereArray)
        {
            return HxString.SelectQueryString(queryString, mWhereArray);
        }
        
        /// <summary>
        /// Get SQL/Query String / SELECT * FROM ( inputQueryString ) WHERE 1 = 1
        /// </summary>
        /// <param name="queryString">기본 쿼리</param>
        /// <param name="mWhere">조건절</param>
        /// <param name="mWhere2">조건절2</param>
        /// <param name="mWhere3">조건절2</param>
        /// <returns>SQL 문자열</returns>
        public static string SelectQueryStringWithOrderBy(string queryString, string mOrderBy, params string[] mWhereArray) //string queryString, string mWhere, string mWhere2 = null, string mWhere3 = null)
        {
            return HxString.SelectQueryStringWithOrderBy(queryString, mOrderBy, mWhereArray);
        }

        public static int RegexTagMatchCount(string input, bool IgnoreCase = true, string pattern = @"(?:{{)(\$)([\w.]+)+(?:\:([\w-~,]+))?( ?\/)?(?:}})")
        {
            //
            if (IgnoreCase != false)
                return RegexTagMatchCount(input, RegexOptions.IgnoreCase, pattern);
            else
                return RegexTagMatchCount(input, RegexOptions.None, pattern);
        }

        public static string RegexParrernReplace(string pattern)
        {
            if(pattern.IsNullOrWhiteSpaceEx() != true && pattern.StartsWith("/") && pattern.EndsWith("/"))
            {
                
                pattern = pattern.RegexReplaceEx("^(/)|(/)$", string.Empty);
                if (pattern.StartsWith("/"))
                    pattern = pattern.Substring(1, pattern.Length - 1);
                if (pattern.EndsWith("/"))
                    pattern = pattern.Substring(0, pattern.Length - 1);
            }
            return pattern;
        }

        public static int RegexTagMatchCount(string input, RegexOptions options, string pattern = @"(?:{{)(\$)([\w.]+)+(?:\:([\w-~,]+))?( ?\/)?(?:}})")
        {
            int Result = -1;
            MatchCollection matches = Regex.Matches(input, pattern, options);

            int n = matches.Count;
            if (n > 0)
            {
                Result = 0;
                for (int i = 0; i < n; i++)
                {
                    Match match = matches[i];
                    if (match.Success)
                    {
                        //_ = match.Value;
                        string strVarCase = match.Groups[1].Value;
                        //string strVarName = match.Groups[2].Value;
                        //string strVarOption = match.Groups[3].Value;
                        //string strEndDefine = match.Groups[4].Value;
                        //strReplaceText = input.Replace(strValue, string.Empty);
                        //if (strColName.IsNullOrWhiteSpaceEx())
                        //    strColName = strVarName;
                        if (strVarCase.IsNullOrWhiteSpaceEx() != true)
                        {
                            Result++;
                        }
                    }
                    
                }
            }
            return Result;
        }

        public static string RegexTagMatchVarName(string input, RegexOptions options = RegexOptions.IgnoreCase, string pattern = @"(?:{{)(\$)([\w.]+)+(?:\:([\w-~,]+))?( ?\/)?(?:}})")
        {
            string Result = null;
            Match match = Regex.Match(input, pattern, options);
            if (match != null && match.Success)
            {
                //string strValue = match.Value;
                string strVarCase = match.Groups[1].Value;
                string strVarName = match.Groups[2].Value;
                //string strVarOption = match.Groups[3].Value;
                //string strEndDefine = match.Groups[4].Value;
                //strReplaceText = input.Replace(strValue, string.Empty);
                //if (strColName.IsNullOrWhiteSpaceEx())
                //    strColName = strVarName;
                if (strVarCase.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = strVarName;
                }
            }
            return Result;
        }

        #region JSON Convert
        public static class Converter
        {
            public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                    {
                        new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal }
                    },
            };
        }

        /// <summary>
        /// JsonConvert.SerializeObject To String
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>JSON string representation of the object.</returns>
        public static string JsonSerializeObject(object value)
        {
            //return JsonConvert.SerializeObject(value);
            return HxConvert.JsonSerializeObject(value);
        }
        /// <summary>
        /// JsonConvert.SerializeObject To String
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>JSON string representation of the object.</returns>
        public static string JsonSerializeObject(object value, Newtonsoft.Json.Formatting formatting)
        {
            //return JsonConvert.SerializeObject(value, formatting);
            return HxConvert.JsonSerializeObject(value, formatting);
        }
        /// <summary>
        /// Serializes the specified object to a JSON string using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The Newtonsoft.Json.JsonSerializerSettings used to serialize the object. If this
        //     is null, default serialization settings will be used.</param>
        /// <returns>JSON string representation of the object.</returns>
        public static string JsonSerializeObject(object value, JsonSerializerSettings settings)
        {
            //return JsonConvert.SerializeObject(value, settings);
            return HxConvert.JsonSerializeObject(value, settings);
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///     Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object JsonDeserializeObject(string value)
        {
            //return JsonConvert.DeserializeObject(value);
            return HxConvert.JsonDeserializeObject(value);
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///     Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object JsonDeserializeObject(object value)
        {
            //return JsonConvert.DeserializeObject(value.ToStringEx());
            return HxConvert.JsonDeserializeObject(value.ToStringEx());
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///      Deserializes the JSON to a .NET object using Newtonsoft.Json.JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.
        /// If this is null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object JsonDeserializeObject(string value, JsonSerializerSettings settings)
        {
            //return JsonConvert.DeserializeObject(value, settings);
            return HxConvert.JsonDeserializeObject(value, settings);
        }
        /// <summary>
        /// JsonConvert.DeserializeObject To Object
        ///      Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The System.Type of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        /// 
        public static object JsonDeserializeObject(string value, Type type)
        {
            //return JsonConvert.DeserializeObject(value, type);
            return HxConvert.JsonDeserializeObject(value, type);
        }
        public static T JsonDeserializeObject<T>(object value)
        {
            return HxConvert.JsonDeserializeObject<T>(value.ToStringEx());
        }
        public static T JsonDeserializeObject<T>(string value)
        {
            bool bException1;
            try
            {
                //return JsonConvert.DeserializeObject<T>(value);
                return HxConvert.JsonDeserializeObject<T>(value);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1);
                bException1 = true;
                //throw ex1;
            }
            if(bException1 == true)
            {
                try
                {
                    //object value2 = JsonConvert.DeserializeObject(value);
                    //return JsonConvert.DeserializeObject<T>(value2.ToStringEx());
                    object value2 = HxConvert.JsonDeserializeObject(value);
                    return HxConvert.JsonDeserializeObject<T>(value2.ToStringEx());
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2);
                    throw ex2;
                }
            }
            return default;
        }

        public static string JsonSerializeWithNameingCase(object sender, HxNameingCaseType caseType)
        {
            //return HxUtils.ConvertSerializeObjectToJsonString(sender);
            return HxCasing.ToJsonString(sender, caseType);
        }

        public static T JsonDeserializeObjectWithNameingCase<T>(string value, HxNameingCaseType caseType)
        {
            return HxCasing.ToJsonDeserializeObject<T>(value, caseType);
        }



        #endregion

        #region DataTable To List<Dictionary<string, object>> => Json Array??
        public static List<Dictionary<string, object>> ToJsonList(DataTable data)
        {
            return HxConvert.ToJsonList(data);
        }
        public static List<Dictionary<string, object>> ToJsonListWithNamingCase(DataTable data, HxNameingCaseType nameingCaseType)
        {
            return HxCasing.ToJsonListWithNamingCase(data, nameingCaseType);
        }
        public static List<Dictionary<string, object>> ToJsonList(DataTable data, HxNameingCaseType nameingCaseType)
        {
            return HxCasing.ToJsonListWithNamingCase(data, nameingCaseType);
        }
        #endregion

        #region HxResultValue
        /// <summary>
        /// HxResultValue Resource To JsonConvert.SerializeObject(String)
        /// </summary>
        /// <param name="pSuccess">성공여부?</param>
        /// <param name="pResultMessageType">메시지 Type(Enum)</param>
        /// <param name="pDetailMessageString">상세 메시지</param>
        /// <param name="pValue">값 Object</param>
        /// <returns>Convert To String</returns>
        public static string ResultValueToString(bool? pSuccess, HxResultMessageType pResultMessageType, string pDetailMessageString, object pValue)
        {
            HxResultValue rValue = null;
            if (pSuccess != null)
            {
                rValue = new HxResultValue
                {
                    Success = pSuccess,
                    MessageType = pResultMessageType,
                    DetailMessage = pDetailMessageString,
                    Value = pValue
                };
            }
            return ResultValueToString(rValue);
        }
        /// <summary>
        /// HxResultValue Resource To JsonConvert.SerializeObject(String)
        /// </summary>
        /// <param name="pValue">HxResultValue Value</param>
        /// <returns>Convert To String</returns>
        public static string ResultValueToString(HxResultValue pValue, bool IsNullConvert = true)
        {
            try
            {
                if ((pValue != null && pValue.Value != null) || IsNullConvert == true)
                {
                    return JsonSerializeObject(pValue);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
            return null;
        }
        #endregion

        #region PC/장비 정보
        public static bool IsReloadEnvironment { get; protected set; } = false;


        
        /// <summary>
        /// [OS]UserName
        /// </summary>
        public static string PCUserName => GetOSUserName();
        private static string _PCUserName = Environment.UserName;
        /// <summary>
        /// [OS]UserName
        /// </summary>
        /// <returns>UserName</returns>
        public static string GetOSUserName(bool bInit = false)
        {
            if (bInit == true || IsReloadEnvironment == true || _PCUserName.IsNullOrWhiteSpaceEx() == true)
            {
                _PCUserName = Environment.UserName;
            }
            return _PCUserName;
        }
        public static string GetUserName(bool bInit = false)
        {
            return GetOSUserName(bInit);
        }


        /// <summary>
        /// [OS]UserDomainName
        /// </summary>
        public static string PCDomainName => GetOSUserDomainName();
        private static readonly string _PCDomainName = Environment.UserDomainName;
        /// <summary>
        /// [OS]UserDomainName
        /// </summary>
        /// <returns>UserDomainName</returns>
        public static string GetOSUserDomainName(bool bInit = false)
        {
            if (bInit == true || IsReloadEnvironment == true || _PCDomainName.IsNullOrWhiteSpaceEx() == true)
            {
                _PCUserName = Environment.UserDomainName;
            }
            return _PCDomainName;
        }
        /// <summary>
        /// [OS]MachineName
        /// </summary>
        /// <returns>MachineName</returns>
        public static string PCMachineName => GetOSMachineName();
        private static string _PCMachineName = Environment.MachineName;
        /// <summary>
        /// [OS]MachineName
        /// </summary>
        /// <returns>MachineName</returns>
        public static string GetOSMachineName(bool bInit = false)
        {
            if (bInit == true || IsReloadEnvironment == true || _PCMachineName.IsNullOrWhiteSpaceEx() == true)
            {
                _PCMachineName = Environment.MachineName;
            }
            return _PCMachineName;
        }
        public static string GetUserMachineName(bool bInit = false)
        {
            return GetOSMachineName(bInit);
        }
        /// <summary>
        /// [OS]Custom UserAgent
        /// </summary>
        /// <returns>Custom UserAgent</returns>
        public static string GetOSCustomUserAgent(bool bInit = false)
        {
            return string.Format("OSUserName:{0}, DomainName:{1}, MachineName:{2}, OSVersion:{3}, Is64BitProcess:{4}, Is64BitOperatingSystem:{5}, Version:{6}, RemoteAddress:{7}/{8}"
                    , GetOSUserName(bInit), GetOSUserDomainName(bInit), GetOSMachineName(bInit), Environment.OSVersion, Environment.Is64BitProcess, Environment.Is64BitOperatingSystem, Environment.Version
                    , GetUserGlobalAddress(bInit), GetUserHostAddress(bInit)
                );
        }
        
        /// <summary>
        /// [OS]Custom UserAgent
        /// </summary>
        public static string PCCustomUserAgent => GetOSCustomUserAgent();
        [Obsolete("PCCustomUserAgent를 사용 합시다.")]
        /// <summary>
        /// [OS]Custom UserAgent
        /// </summary>
        public static string OSCustomUserAgent => GetOSCustomUserAgent();
        #endregion

        #region HxNet 참조
        
        public static bool IsNetworkAvailable()
        {
            return HxNet.GetIsNetworkAvailable();
        }

        public static bool IsInternetConnected()
        {
            return HxNet.GetIsInternetConnected();
        }

        public static string UserGlobalAddress => GetUserGlobalAddress(false);
        private static string _UserGlobalAddress = HxNet.GetUserGlobalAddress();
        public static string GetUserGlobalAddress(bool bInit = false)
        {
            if (bInit == true || IsReloadEnvironment == true || _UserGlobalAddress.IsNullOrWhiteSpaceEx() == true)
            {
                _UserGlobalAddress = HxNet.GetUserGlobalAddress();
            }
            return _UserGlobalAddress;
        }

        //public static string UserHostAddress => GetUserHostAddress(false);
        [Obsolete("GetUserHostAddress() 사용 하자.")]
        public static string UserHostAddress => GetUserHostAddress(false);
        private static string _UserHostAddress = HxNet.GetUserHostAddress();
        public static string GetUserHostAddress(bool bInit = false)
        {
            if (bInit == true || IsReloadEnvironment == true || _UserHostAddress.IsNullOrWhiteSpaceEx() == true)
            {
                _UserHostAddress = HxNet.GetUserHostAddress();
            }
            return _UserHostAddress;
        }

        public static string UserHostName => GetUserHostName(false);
        private static string _UserHostName = HxNet.GetUserHostName();
        public static string GetUserHostName(bool bInit = false)
        {

            if (bInit == true || IsReloadEnvironment == true || _UserHostName.IsNullOrWhiteSpaceEx() == true)
            {
                _UserHostName = HxNet.GetUserHostName();
            }
            return _UserHostName;
        }
        #endregion

        #region 어셈블리 특성 접근자

        public static string HxCoreTitle
        {
            get => HxFile.AssemblyTitle;
        }

        public static string HxCoreVersion
        {
            get => HxFile.AssemblyVersion;
        }

        public static string HxCoreDescription
        {
            get => HxFile.AssemblyDescription;
        }

        public static string HxCoreProduct
        {
            get => HxFile.AssemblyProduct;
        }

        public static string HxCoreCopyright
        {
            get => HxFile.AssemblyCopyright;
        }

        public static string HxCoreCompany
        {
            get => HxFile.AssemblyCompany;
        }
        public static string HxCoreFileVersion
        {
            get => HxFile.AssemblyFileVersion;
        }

        
        #endregion

        #region PDF 관련
        public static string TempFileNameWithSuffix(string fileName, string suffix = null, string fileExt = null, string suffixSPChar = "√")
        {
            string Result = null;
            try
            {
                if (fileExt.IsNullOrWhiteSpaceEx() == true)
                {
                    fileExt = HxFile.GetFileNameExt(fileName);
                }
                if (suffix.IsNullOrWhiteSpaceEx() != true)
                {
                    fileName += $"{suffixSPChar}{suffix}";
                }
                Result = Result = string.Format("{0}.tmp.{1}", fileName, fileExt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Result;
        }
        
        public static string TempFileNameWithPrefix(HxFilePropLocalRemoteRec fileInfo, string prefix = null, string fileExt = null, string sp1 = "―", string sp2 = "√")
        {
            string Result = null;
            try
            {
                if (fileInfo.FILE_NAME.IsNullOrWhiteSpaceEx() != true && fileInfo.FILE_PATH.IsNullOrWhiteSpaceEx() != true)
                {
                    
                    string fileSaveName = (fileInfo.FILE_SAVE.IsNullOrWhiteSpaceEx() == true ? HxFile.GetFileName(fileInfo.FILE_NAME) : HxFile.GetFileName(fileInfo.FILE_SAVE));
                    string fileOriName = (fileInfo.ORIGINAL_NAME.IsNullOrWhiteSpaceEx() != true ? HxFile.GetFileNameWithOutExt(fileInfo.ORIGINAL_NAME) : HxFile.GetFileNameWithOutExt(fileSaveName));
                    if (fileExt.IsNullOrWhiteSpaceEx() == true)
                    {
                        fileExt = HxFile.GetFileNameExt(fileSaveName);
                    }

                    string remoteFullName = Path.Combine(fileInfo.FILE_PATH, fileSaveName);
                    long? fileSize = fileInfo.FILE_SIZE;
                    

                    string tmpFileName = $"{fileOriName}";
                    if (tmpFileName.IsNullOrWhiteSpaceEx())
                    {
                        tmpFileName = HxCrypt.RandPass();
                    }
                    /*
                    //string sp1 = "―"; string sp2 = "√";
                    string fileCheck = fileInfo.FILE_CHECK.ToStringEx();
                    if (fileCheck.IsNullOrWhiteSpaceEx())
                    {
                        fileCheck = HxCrypt.Md5(remoteFullName);
                    }
                    if (fileCheck.IsNullOrWhiteSpaceEx() != true) {
                        tmpFileName += $"{sp2}{fileCheck}";
                    }

                    
                    string fileNum = fileInfo.FILE_NO.ToStringEx();
                    if (fileNum.IsNullOrWhiteSpaceEx() != true)
                    {
                        fileNum = string.Format("{0}", fileNum.PadLeft(4, '0'));
                    }
                    
                    
                    string groupNum = fileInfo.GROUP_NO.ToStringEx();
                    if (groupNum.IsNullOrWhiteSpaceEx() != true)
                    {
                        groupNum = string.Format("{0}{1}", sp2, groupNum.PadLeft(4, 'x'));
                    }
                    
                    Result = string.Format("{0}{1}{2}{3}.tmp.{4}", prefix, fileNum, sp1, tmpFileName, fileExt); //, HxString.GetNowLongDateTimeString()
                    */
                    Result = string.Format("{0}{1}.{2}", prefix, tmpFileName, fileExt); //, HxString.GetNowLongDateTimeString()

                }
            }
            catch (Exception ex)
            {
                //SysEnv.AppLog.WriteExceptionLog(ex);
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }


        #endregion

        public static HxFilePatternToDoc5Rec GetPatternFileInfo(string inputFileName, string pattern = @"([a-zA-Z0-9\-\(\)\,\&\~＃＄％＆／＋ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ]{4,})(?:(?:_|\ )(.{3,}))?(?:_|\.)(?:Rev|R)(?:\.)?([0-9a-zA-Z]{1,2})+(.{2,})?(?:\.)(\w+)$", string subPattern = @"^([\w\-\(\)\,]+)(.{2,})?(?:\.)(\w+)$")
        {
            //tring defaultPattern1 = @"^([\w\-\(\)\,]+)(.{2,})?(?:\.)(\w+)$";
            //string defaultPattern2 = @"^(.{4,})(?:(?:.)(\w+))$";
            return new HxFilePatternToDoc5Rec(inputFileName, pattern, subPattern);
        }

        public static string LongFileName(string path)
        {
            return HxFile.GetLongFileName(path);
        }

        public static string UriEscapeString(string input, bool bReplaceEscapeAnd = false)
        {
            return HxString.UriEscapeString(input, bReplaceEscapeAnd);
        }

        public static TEnum EnumType<TEnum>(string input, bool ignoreCase = true)
            where TEnum : struct, Enum
        {
            TEnum Result = default;
            try
            {
                bool bSuccess = System.Enum.TryParse<TEnum>(input, ignoreCase, out Result);
                if(bSuccess != true)
                {
                    foreach (TEnum enumType in Enum.GetValues(typeof(TEnum)))
                    {
                        string str = enumType.ToString();
                        if(ignoreCase == true && str != null && input != null)
                        {
                            str = str.ToLower();
                            input = str.ToLower();
                        }
                        if (str == input)
                        {
                            return enumType;
                        }
                    }

                    Result = (TEnum)System.Enum.Parse(typeof(TEnum), input, ignoreCase);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }

        protected static void SetDebugWrite(Exception ex, bool IsThrowExecute = true, string message = null)
        {
            Debug.WriteLine(ex);
            if (message.IsNullOrWhiteSpaceEx() != true)
            {
                Debug.WriteLine(message);
            }
            if (IsThrowExecute == true)
            {
                throw ex;
            }
        }

        private static void DebugWrite(Exception ex, bool IsThrowExecute = true, string message = null)
        {
            SetDebugWrite(ex, IsThrowExecute, message);
        }

        #region Service Provider
        public static HxDbProviderType GetDbProviderType(string providerStr)
        {
            return HxEnum.GetDbProviderType(providerStr);
        }
        public static int GetServiceDefaultPort(HxServiceProviderType providerType)
        {
            return HxEnum.GetServiceDefaultPort(providerType);
        }
        public static int GetServiceDefaultPort(string providerStr)
        {
            return HxEnum.GetServiceDefaultPort(providerStr);
        }
        public static string GetServiceProviderProtocol(int defaultPort)
        {
            return HxEnum.GetServiceProviderProtocol(defaultPort);
        }
        public static string GetServiceProviderProtocol(HxServiceProviderType providerType)
        {
            return HxEnum.GetServiceProviderProtocol(providerType);
        }
        #endregion //Service Provider

        #region Network / PC Domain
        public static string GetUserDomainName()
        {
            string Result = string.Empty;
            if (Result.IsNullOrWhiteSpaceEx())
            {
                Result = Environment.UserDomainName;
            }
            return Result;
        }

        public static string GetUserWorkgroup()
        {
            string Result = string.Empty;
            try
            {
                SelectQuery query = new SelectQuery("Win32_ComputerSystem");
                if (query != null)
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                    foreach (ManagementObject mo in searcher.Get().Cast<ManagementObject>())
                    {
                        if ((bool)mo["partofdomain"] != true)
                        {
                            //Console.WriteLine("Workgroup {0} ", mo["workgroup"]);
                            Result = mo["workgroup"].ToStringEx();
                        }
                        else
                        {
                            //Console.WriteLine("Domain {0} ", mo["workgroup"]);
                            Result = mo["workgroup"].ToStringEx();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex);
                //throw;
            }
            return Result;
        }

        //public static string GetUserGlobalAddress()
        //{
        //    return HxNet.GetUserGlobalAddress();
        //}
        //public static string GetUserIPAddress()
        //{
        //    return HxNet.GetUserIPAddress();
        //}
        //public static string GetUserHostName()
        //{
        //    return HxNet.GetUserHostName();
        //}
        public static string GetUserMacAddress(string ip = null)
        {
            string Result = string.Empty;
            try
            {
                if (ip.IsNullOrWhiteSpaceEx() == true)
                {
                    ip = GetUserHostAddress(true);
                }
                if (ip.IsNullOrWhiteSpaceEx() == true)
                {
                    List<string> list = HxNet.GetUserAdressList();
                    if (list != null && list.Count > 0)
                    {
                        ip = list[0];
                    }
                }
                if (ip.IsNullOrWhiteSpaceEx() != true)
                {
                    ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
                    ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
                    foreach (ManagementObject mo in query1.Get().Cast<ManagementObject>())
                    {
                        string[] address = (string[])mo["IPAddress"];
                        if (address[0] == ip && mo["MACAddress"] != null)
                        {
                            Result = mo["MACAddress"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result = null;
                //throw ex;
            }

            return Result;
        }
        public static string GetUserVolumeId(string drive = null)
        {
            if (drive.IsNullOrWhiteSpaceEx())
            {
                drive = Environment.SystemDirectory;
            }

            if (drive.Contains(":\\"))
            {
                //C:\ -> C
                int index = drive.IndexOf(":\\");
                drive = drive.Substring(0, index);
            }



            string Result;
            try
            {
                Result = GetUserVolumeSerial(drive);
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    Result = GetUserVolumeSerial("C");
                }
                if (Result.IsNullOrWhiteSpaceEx())
                {
                    ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM win32_LogicalDisk");
                    ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
                    foreach (ManagementObject mo in query1.Get().Cast<ManagementObject>())
                    {
                        int driveType = mo.GetPropertyValue("DriveType").ToIntEx();
                        string driveName = mo.GetPropertyValue("Name").ToStringEx();
                        Result = mo.GetPropertyValue("VolumeName").ToStringEx();
                        if (!Result.IsNullOrWhiteSpaceEx())
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }

        public static string GetUserVolumeSerial(string drive)
        {
            string Result = string.Empty;
            if (drive.Contains(":\\"))
            {
                //C:\ -> C
                int index = drive.IndexOf(":\\");
                drive = drive.Substring(0, index);
            }
            if (!drive.IsNullOrWhiteSpaceEx())
            {
                using (ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":"""))
                {
                    disk.Get();
                    if (disk != null)
                    {
                        Result = disk["VolumeSerialNumber"].ToString();
                    }
                }
            }
            return Result;
        }

        public static List<string> GetUserVolumeIdList()
        {
            List<string> Result = null;
            ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM win32_LogicalDisk");
            if (oq != null)
            {
                Result = new List<string>();
                ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
                foreach (ManagementObject mo in query1.Get().Cast<ManagementObject>())
                {
                    _ = mo.GetPropertyValue("DriveType").ToIntEx();
                    _ = mo.GetPropertyValue("Name").ToStringEx();
                    string volumeSerial = mo.GetPropertyValue("VolumeName").ToStringEx();
                    Result.AddEx(volumeSerial);
                }
            }
            return Result;
        }

        public static string GetUserCPUId()
        {
            string Result = string.Empty;
            try
            {
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                foreach (ManagementObject managObj in managCollec.Cast<ManagementObject>())
                {
                    //Get only the first CPU's ID
                    Result = managObj.Properties["processorID"].Value.ToStringEx();
                    if (!Result.IsNullOrWhiteSpaceEx())
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = null;
                //throw ex;
            }

            return Result;
        }

        public static string GetUserUniqueID(string drive = null)
        {
            string Result;
            if (drive.IsNullOrWhiteSpaceEx())
            {
                //Find first drive
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.Contains(":\\"))
            {
                //C:\ -> C
                int index = drive.IndexOf(":\\");
                drive = drive.Substring(0, index);
            }

            string volumeSerial = GetUserVolumeSerial(drive);
            string cpuID = GetUserCPUId();

            //Mix them up and remove some useless 0's
            if (!cpuID.IsNullOrWhiteSpaceEx() && !volumeSerial.IsNullOrWhiteSpaceEx())
            {
                Result = cpuID.Substring(13) + cpuID.Substring(1, 4) + volumeSerial + cpuID.Substring(4, 4);
            }
            else if (!cpuID.IsNullOrWhiteSpaceEx() && volumeSerial.IsNullOrWhiteSpaceEx())
            {
                Result = cpuID.Substring(13) + cpuID.Substring(1, 4) + "HTHTHTHT" + cpuID.Substring(4, 4);
            }
            else if (cpuID.IsNullOrWhiteSpaceEx() && !volumeSerial.IsNullOrWhiteSpaceEx())
            {
                Result = "HTE" + "HTHT" + volumeSerial + "HTHT";
            }
            else
            {
                Result = null;
            }
            return Result;
        }


        #endregion

        public static T DeepCopy<T>(T obj)
            where T : class
        {
            T Result = default;
            using (var ms = new MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                Result = formatter.Deserialize(ms) as T;
            }
            return Result;
        }


    }
}