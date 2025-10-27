using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore
{
    public class HxString
    {
        public const int _ENCODING_CODE_PAGE_EUCKR_ = 51949;

        private static System.Text.Encoding _euckrEncoding = null;
        public static System.Text.Encoding EuckrEncoding
        {
            get
            {
                if(_euckrEncoding == null)
                {
                    int euckrCodePage = _ENCODING_CODE_PAGE_EUCKR_;
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); //System.Text.CodePagesEncodingProvider.Instance
                    _euckrEncoding = System.Text.Encoding.GetEncoding(euckrCodePage);
                }
                return _euckrEncoding;
            }
        }
        

        static HxString()
        {
            ; ;
        }


        public static string GetRegexPattern(HxRegexPatternType patternType)
        {
            string Result = null;

            switch (patternType)
            {
                case HxRegexPatternType.Numberic:
                    Result = @"^-?(?:[0-9]+(?:\.[0-9]*)?|(?:[0-9]+)?\.[0-9]+)$"; //@"/-?(?:[0-9]+(?:\.[0-9]*)?|(?:[0-9]+)?\.[0-9]+)/g"
                    break;
                case HxRegexPatternType.WebUri:
                    Result = @"^(http[s]?)+\:\/\/([^:\/\s]+)([^#?\s]+)(?:\?([^#]*)?(#.*)?)?$";
                    /*
                     * ex)
                     http://htenc.co.kr/download/CompanyBrochure.pdf

                     https://www.reGexr.com/more/less/path/foo.php?q=bar&same[12]=xxx&same[11]=#sldns13123nfdwdw

                     http://htenc.co.kr/download/CompanyBrochure.pdf?aaaa=dddd&aaa=1
                     * */
                    break;
            }

            return Result;
        }



        //0050fcc5ae46
        /// <summary>
        /// 공백 또는 Null, Empty 여부
        /// </summary>
        /// <param name="value">입력 문자열</param>
        /// <returns>bool</returns>
        public static bool IsNullOrEmpty(string value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && (
                    string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || value.Trim().Length == 0
                    || value == int.MinValue.ToString() || value == Int64.MinValue.ToString() || value == decimal.MinValue.ToString() || value == double.MinValue.ToString() || value == float.MinValue.ToString()
                    || value == DateTime.MinValue.ToString() || value == (new DateTime(1900, 1, 1)).ToString()
                )
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 공백 또는 Null, Empty 여부
        /// </summary>
        /// <param name="value">입력 문자열</param>
        /// <returns>bool</returns>
        public static bool IsNullOrWhiteSpace(string value)
        {
            if(value != null)
            {
                value = value.Trim();
            }
            return IsNullOrEmpty(value);
        }
        
        /// <summary>
        /// 숫자 타입 여부
        /// </summary>
        /// <param name="value">입력 문자열</param>
        /// <returns>bool</returns>
        public static bool IsNumberic(string value)
        {
            bool Result = false;
            try
            {
                string strPattern = GetRegexPattern(HxRegexPatternType.Numberic);
                if (!IsNullOrWhiteSpace(value))
                {
                    System.Text.RegularExpressions.Regex regExpr = new System.Text.RegularExpressions.Regex(strPattern);

                    Result = regExpr.IsMatch(value);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }
        
        public static bool IsWebUri(string value)
        {
            string strPattern = GetRegexPattern(HxRegexPatternType.WebUri);
            return IsRegexMatch(value, strPattern, RegexOptions.IgnoreCase);
        }
        
        /// <summary>
        /// StringBuilder Clear Method
        /// </summary>
        /// <param name="Sender">StringBuilder Resource</param>
        public static void Clear(StringBuilder Sender)
        {
            try
            {
                Sender.Clear();
            }
            finally
            {
                Sender.Length = 0;
                Sender.Capacity = 0;
            }

        }
        /// <summary>
        /// 실수형일 경우 "."으로 끝나거나 "0"으로 끝나면 생략한 문자열 타입의 숫자값
        /// </summary>
        /// <param name="value">입력 값</param>
        /// <returns>문자열 숫자</returns>
        public static string FormatNumber(double value)
        {
            string s = String.Format("{0:0.00}", value);
            s = s.TrimEnd('0');
            s = s.TrimEnd('.');
            return s;
        }
        /// <summary>
        /// 실수형일 경우 "."으로 끝나거나 "0"으로 끝나면 생략한 문자열 타입의 숫자값
        /// </summary>
        /// <param name="value">입력 값</param>
        /// <returns>문자열 숫자</returns>
        public static string FormatNumber<T>(T value)
            where T : IComparable<double>, IComparable<float>, IComparable<decimal>
        {
            string s = String.Format("{0:0.00}", value);
            s = s.TrimEnd('0');
            s = s.TrimEnd('.');
            return s;
        }
        /// <summary>
        /// 문자열을 실수(Double)로 변환
        /// </summary>
        /// <param name="input">입력 값</param>
        /// <param name="bSucessed">성공 여부</param>
        /// <param name="defaultValue">기본 값</param>
        /// <returns>변환 값</returns>
        public static double String2Double(string input, out bool bSucessed, double defaultValue = 0.0)
        {
            bSucessed = false;
            char[] charsToTrim = { ' ', '%', '.', ',' };
            input = input.Trim(charsToTrim);
            if (input.IsNullOrWhiteSpaceEx())
                return defaultValue;
            bSucessed = true;
            double v;
            try
            {
                v = Convert.ToDouble(input);
            }
            catch (FormatException)
            {
                bSucessed = false;
                v = defaultValue;
            }
            return v;
        }
        /// <summary>
        /// 문자열을 실수형으로 변환
        /// </summary>
        /// <param name="input">입력 값</param>
        /// <param name="bSucessed">성공 여부</param>
        /// <param name="defaultValue">기본 값</param>
        /// <returns>변환 값</returns>
        public static T String2Double<T>(string input, out bool bSucessed, T defaultValue = default)
            where T : IComparable<double>, IComparable<float>, IComparable<decimal>
        {
            bSucessed = false;
            char[] charsToTrim = { ' ', '%', '.', ',' };
            input = input.Trim(charsToTrim);
            if (input.IsNullOrWhiteSpaceEx())
                return defaultValue;
            bSucessed = true;
            T v;
            try
            {
                v = HxConvert.ToConvert<T>(input);
            }
            catch (FormatException)
            {
                bSucessed = false;
                v = defaultValue;
            }
            return v;
        }

        /// <summary>
        /// Byte형을 문자열로 변환
        /// </summary>
        /// <param name="input">입력 값</param>
        /// <param name="format">문자 포멧</param>
        /// <returns>변환 값</returns>
        public static string GetBytes2String(byte[] input, string format = null)
        {
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < input.Length; i++)
            {
                sBuilder.Append(input[i].ToString(format));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        
        /// <summary>
        /// Byte형을 Encoding Type 문자열로 변환
        /// </summary>
        /// <param name="input">입력값</param>
        /// <param name="encodingType">Encoding Type</param>
        /// <returns>변환 값</returns>
        public static string GetBytes2String(byte[] input, HxEncodingType encodingType = HxEncodingType.None)
        {
            string Result;
            switch (encodingType)
            {
                case HxEncodingType.UTF7:
                    Result = Encoding.UTF7.GetString(input);
                    break;
                case HxEncodingType.UTF32:
                    Result = Encoding.UTF32.GetString(input);
                    break;
                case HxEncodingType.Unicode:
                    Result = Encoding.Unicode.GetString(input);
                    break;
                case HxEncodingType.BigEndianUnicode:
                    Result = Encoding.BigEndianUnicode.GetString(input);
                    break;
                case HxEncodingType.ASCII:
                    Result = Encoding.ASCII.GetString(input);
                    break;
                case HxEncodingType.Default:
                    Result = Encoding.Default.GetString(input);
                    break;
                case HxEncodingType.UTF8:
                case HxEncodingType.None:
                default:
                    Result = Encoding.UTF8.GetString(input);
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 문자열을 Encoding Type에 따른 Byte형으로 변환
        /// </summary>
        /// <param name="input">입력 값</param>
        /// <param name="encodingType">Encoding Type</param>
        /// <returns>변환 값</returns>
        public static byte[] GetString2Bytes(string input, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            if(input.IsNullOrWhiteSpaceEx() == true) { return null; }

            byte[] Result;
            switch (encodingType)
            {
                case HxEncodingType.UTF7:
                    Result = Encoding.UTF7.GetBytes(input);
                    break;
                case HxEncodingType.UTF32:
                    Result = Encoding.UTF32.GetBytes(input);
                    break;
                case HxEncodingType.Unicode:
                    Result = Encoding.Unicode.GetBytes(input);
                    break;
                case HxEncodingType.BigEndianUnicode:
                    Result = Encoding.BigEndianUnicode.GetBytes(input);
                    break;
                case HxEncodingType.ASCII:
                    Result = Encoding.ASCII.GetBytes(input);
                    break;
                case HxEncodingType.Default:
                    Result = Encoding.Default.GetBytes(input);
                    break;
                case HxEncodingType.UTF8:
                case HxEncodingType.None:
                default:
                    Result = Encoding.UTF8.GetBytes(input);
                    break;
            }
            return Result;
        }
        
        public static string ByteToString(byte[] input, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            return GetBytes2String(input, encodingType);
        }
        public static byte[] StringToByteArray(string hex)
        {
            //출처 : https://stackoverflow.com/questions/6397235/write-bytes-to-file
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        #region 2025-10-01 By JHP / 추가
        /// <summary>
        /// Byte형을 문자열로 변환
        /// </summary>
        /// <param name="input">입력 값(Bytes)</param>
        /// <param name="format">문자 포멧</param>
        /// <returns>변환 값(String)</returns>
        public static string ConvertBytesToString(byte[] input, string format = null)
        {
            return GetBytes2String(input, format);
        }
        public static string ConvertBytesToHexStringLower(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) { return string.Empty; }

            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b); // Lowercase format
            return hex.ToString();
        }
        public static string ConvertBytesToHexStringUpper(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:X2}", b); // Uppercase format
            return hex.ToString();
        }
        public static byte[] ConvertHexStringToByteArray(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return Array.Empty<byte>();
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        public static string GetMD5Checksum(byte[] bytes)
        {
            string Result = null;

            if (bytes == null || !bytes.Any()) { return Result; }

            var hashAlgorithm = System.Security.Cryptography.MD5.Create();
            if (hashAlgorithm == null) {  return Result; }
            
            var hashBytes = hashAlgorithm.ComputeHash(bytes);
            if(hashBytes == null || hashBytes.Any() != true || hashBytes.Length <= 0) { return Result; }

            Result = ConvertBytesToHexStringLower(hashBytes) ?? string.Empty;

            return Result;
            //return System.Security.Cryptography.MD5.HashData(bytes) is byte[] hash ? Convert.ToHexStringLower(hash) : string.Empty;
        }
        public static string GetMD5Checksum(string input)
        {
            return HxCrypt.Md5(input);
        }
        public static string GetSHA1Checksum(byte[] bytes)
        {
            string Result = null;

            if (!bytes.Any()) { return Result; }

            var hashAlgorithm = System.Security.Cryptography.SHA1.Create();
            if (hashAlgorithm == null) { return Result; }

            var hashBytes = hashAlgorithm.ComputeHash(bytes);
            if (hashBytes == null || hashBytes.Any() != true || hashBytes.Length <= 0 || hashBytes.Length != bytes.Length) { return Result; }

            Result = ConvertBytesToHexStringLower(hashBytes) ?? string.Empty;

            return Result;
            //return System.Security.Cryptography.SHA1.HashData(bytes) is byte[] hash ? Convert.ToHexStringLower(hash) : string.Empty;
        }
        public static string GetSHA256Checksum(byte[] bytes)
        {
            string Result = null;

            if (!bytes.Any()) { return Result; }

            var hashAlgorithm = System.Security.Cryptography.SHA256.Create();
            if (hashAlgorithm == null) { return Result; }

            var hashBytes = hashAlgorithm.ComputeHash(bytes);
            if (hashBytes == null || hashBytes.Any() != true || hashBytes.Length <= 0 || hashBytes.Length != bytes.Length) { return Result; }

            Result = ConvertBytesToHexStringLower(hashBytes) ?? string.Empty;

            return Result;
            //return System.Security.Cryptography.SHA256.HashData(bytes) is byte[] hash ? Convert.ToHexStringLower(hash) : string.Empty;
        }
        public static string GetSHA512Checksum(byte[] bytes)
        {
            string Result = null;

            if (!bytes.Any()) { return Result; }

            var hashAlgorithm = System.Security.Cryptography.SHA512.Create();
            if (hashAlgorithm == null) { return Result; }

            var hashBytes = hashAlgorithm.ComputeHash(bytes);
            if (hashBytes == null || hashBytes.Any() != true || hashBytes.Length <= 0 || hashBytes.Length != bytes.Length) { return Result; }

            Result = ConvertBytesToHexStringLower(hashBytes) ?? string.Empty;

            return Result;
            //return System.Security.Cryptography.SHA512.HashData(bytes) is byte[] hash ? Convert.ToHexStringLower(hash) : string.Empty;
        }

        #endregion

        public static Encoding GetEncodingType(HxEncodingType encodingType)
        {
            Encoding Result = null;

            switch (encodingType)
            {
                case HxEncodingType.ASCII:
                    Result = Encoding.ASCII;
                    break;
                case HxEncodingType.UTF7:
                    Result = Encoding.UTF7;
                    break;
                case HxEncodingType.UTF8:
                    Result = Encoding.UTF8;
                    break;
                case HxEncodingType.UTF32:
                    Result = Encoding.UTF32;
                    break;
                case HxEncodingType.Unicode:
                    Result = Encoding.Unicode;
                    break;
                case HxEncodingType.BigEndianUnicode:
                    Result = Encoding.BigEndianUnicode;
                    break;
                case HxEncodingType.None:
                case HxEncodingType.Default:
                default:
                    Result = Encoding.Default;
                    break;
            }

            return Result;
        }

        public static int GetByteCount(string input, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            string str = input;
            int Result = 0;

            if (str == null) return Result;

            try
            {
                switch (encodingType)
                {
                    
                    case HxEncodingType.Default:
                        Result = Encoding.Default.GetByteCount(str);
                        break;
                    case HxEncodingType.ASCII:
                        Result = Encoding.ASCII.GetByteCount(str);
                        break;
                    case HxEncodingType.UTF7:
                        Result = Encoding.UTF7.GetByteCount(str);
                        break;
                    case HxEncodingType.UTF8:
                        Result = Encoding.UTF8.GetByteCount(str);
                        break;
                    case HxEncodingType.UTF32:
                        Result = Encoding.UTF32.GetByteCount(str);
                        break;
                    case HxEncodingType.Unicode:
                        Result = Encoding.Unicode.GetByteCount(str);
                        break;
                    case HxEncodingType.BigEndianUnicode:
                        Result = Encoding.BigEndianUnicode.GetByteCount(str);
                        break;
                    case HxEncodingType.None:
                    default:
                        Result = str.Length;
                        break;
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }
        public static int GetByteLength(string input, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            return GetByteCount(input, encodingType);
        }

        /// <summary>
        /// 랜덤으로 요청 자리수 만큼의 문자열 생성
        /// </summary>
        /// <param name="maxLength">요청 자리수(1 이상, 0일 경우 기본값(8))</param>
        /// <param name="minCharAscii">문자열 범위 최소값 자리수(1 이상, 0이하 경우 기본값(48))</param>
        /// <param name="maxCharAscii">문자열 범위 대값 자리수(1 이상, 0이하 경우 기본값(48))</param>
        /// <returns>랜덤 문자열</returns>
        public static string GetRandomString(uint maxLength = 8, int minCharAscii = 48, int maxCharAscii = 120)
        {
            Random sland = new Random();
            if (maxLength <= 0)
            {
                maxLength = 8;
            }
            if(minCharAscii <= 0)
            {
                minCharAscii = 48;
            }
            if(maxCharAscii <= 0)
            {
                maxCharAscii = 120;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < maxLength; i++)
            {
                int randnumber = sland.Next(minCharAscii, maxCharAscii);
                while ((randnumber >= 58 && randnumber <= 64) || (randnumber >= 91 && randnumber <= 96))
                {
                    randnumber = sland.Next(minCharAscii, maxCharAscii);
                }
                sb.Append(Convert.ToChar(randnumber));
            }
            string Result = sb.ToString();
            return Result;
        }

        public static string GetNumberString(int sender, string format = "#,##0", IFormatProvider provider = null)
        {
            //default format = "#,##0" = "N0"
            /*
(C) Currency: . . . . . . . . ($1,234.00)
(D) Decimal:. . . . . . . . . -1234
(E) Scientific: . . . . . . . -1.234565E+003
(F) Fixed point:. . . . . . . -1234.57
(G) General:. . . . . . . . . -1234
    (default):. . . . . . . . -1234 (default = 'G')
(N) Number: . . . . . . . . . -1,234.00
(P) Percent:. . . . . . . . . -123,456.50 %
(R) Round-trip: . . . . . . . -1234.565
(X) Hexadecimal:. . . . . . . FFFFFB2E
            */
            if (provider != null)
            {
                return sender.ToString(format, provider);
            }
            else
            {
                return sender.ToString(format);
            }
        }
        public static string GetNumberString(uint sender, string format = "#,##0", IFormatProvider provider = null)
        {
            if (provider != null)
            {
                return sender.ToString(format, provider);
            }
            else
            {
                return sender.ToString(format);
            }
        }
        public static string GetNumberString(long sender, string format = "#,##0", IFormatProvider provider = null)
        {
            if (provider != null)
            {
                return sender.ToString(format, provider);
            }
            else
            {
                return sender.ToString(format);
            }
        }
        public static string GetNumberString(ulong sender, string format = "#,##0", IFormatProvider provider = null)
        {
            if (provider != null)
            {
                return sender.ToString(format, provider);
            }
            else
            {
                return sender.ToString(format);
            }
        }
        public static string GetNumberString(double sender, string format = "#,##0", IFormatProvider provider = null)
        {
            if (provider != null)
            {
                return sender.ToString(format, provider);
            }
            else
            {
                return sender.ToString(format);
            }
        }
        public static string GetNumberString(decimal sender, string format = "#,##0", IFormatProvider provider = null)
        {
            if (provider != null)
            {
                return sender.ToString(format, provider);
            }
            else
            {
                return sender.ToString(format);
            }
        }

        #region DateTime
        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format</param>
        /// <returns>DateTime String</returns>
        public static string GetTodayString(string dateFormat = "yyyy-MM-dd")
        {
            return GetNowDateTime(dateFormat);
        }
        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format</param>
        /// <returns>DateTime String</returns>
        public static string GetNowDateTime(string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            //string Result = string.Empty;
            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
            //return Result;
            return DateTime.Now.ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
            
        }

        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyy-MM-dd HH:mm:ss.fffffff</param>
        /// <returns>DateTime String</returns>
        public static string GetNowLongDateTime(string dateFormat = "yyyy-MM-dd HH:mm:ss.fffffff")
        {
            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
            return DateTime.Now.ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyyMMddHHmmssfffffff)</param>
        /// <returns>DateTime String</returns>
        public static string GetNowLongDateTimeString(string dateFormat = "yyyyMMddHHmmssfffffff")
        {
            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
            return DateTime.Now.ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyyMMddHHmmss)</param>
        /// <returns>DateTime String</returns>
        public static string GetNowDateTimeString(string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
            return DateTime.Now.ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 현재 날짜(Today / NOW)를 특정 Format형태의 String으로 반환
        /// </summary>
        /// <param name="dateFormat">Date Format(Default Format : yyyyMMdd)</param>
        /// <returns>DateTime String</returns>
        public static string GetNowDateString(string dateFormat = "yyyyMMdd")
        {
            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
            return DateTime.Now.ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DateTime Now
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime GetNowDateTime()
        {
            //return DateTime.Now;
            return DateTime.Now;
        }

        /// <summary>
        /// DateTime을 지정된 String Format으로 반환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">DateTime String Format</param>
        /// <returns>String DateTime</returns>
        public static string GetString(DateTime value, string dateFormat = "yyyy-MM-dd")
        {
            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //HH:mm:ss
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
            return value.ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// DateTime을 지정된 String Format으로 반환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">DateTime String Format</param>
        /// <returns>String DateTime</returns>
        public static string GetString(DateTime? value, string dateFormat = "yyyy-MM-dd")
        {
            string Result = string.Empty;
            if(value == null)
            {
                //value = new DateTime(1900, 1, 1);
                return null;
            }
            DateTime dateTime;
            try
            {
                dateTime = (DateTime)value;
                return GetString(dateTime, dateFormat);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return string.Empty;
                //dateTime = new DateTime(1900, 1, 1);
            }
            

            //string strDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            //HH:mm:ss
            //DateTime parsedDate;
            //DateTime.TryParseExact(strDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            //Result = strDate;
           
        }
        /// <summary>
        /// 날짜 형식을 포함한 object를 DateTime 형태로 가져오기
        /// </summary>
        /// <param name="value">날짜 형식을 포함한 object</param>
        /// <param name="dateFormat">날짜포멧</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTime(object value, string dateFormat = "yyyy-MM-dd") //HH:mm:ss
        {
            DateTime Result = new DateTime(1900, 1, 1);
            DateTime minCustomDateValue = new DateTime(1900, 1, 1);
            try
            {
                
                if (value.IsNullOrWhiteSpaceEx() != true)
                {
                    Type type = value.GetType();
                    DateTime dateTimeShort = HxString.GetDateTime(value.ToString(), dateFormat);
                    if (typeof(DateTime) == type || typeof(Nullable<DateTime>) != type)
                    {
                        Result = (DateTime)value;
                    }
                    else
                    {
                        if (dateFormat.IsNullOrWhiteSpaceEx() == true)
                        {
                            dateFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        }
                        //string ShortDatePattern = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        Result = HxString.GetDateTime(value.ToStringEx(), dateFormat);
                    }
                    //Result = Result.GetValueOrDefault(minCustomDateValue);
                    if (Result == minCustomDateValue || Result == DateTime.MinValue || (Result != null && Result <= minCustomDateValue))
                    {
                        Result = minCustomDateValue;
                    }
                }
                else
                {
                    Result = minCustomDateValue;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Result = minCustomDateValue;
            }
            return Result;
        }
        /// <summary>
        /// 날짜 형식을 포함한 문자열을 DateTime 형태로 가져오기
        /// </summary>
        /// <param name="value">날짜 형식을 포함한 문자열</param>
        /// <param name="dateFormat">날짜포멧</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTime(string value, string dateFormat = "yyyy-MM-dd") //HH:mm:ss
        {
            DateTime Result = new DateTime(1900, 1, 1);
            try
            {
                //string strDate = string.Format("{0:yyyy-MM-dd}", value);
                bool bFlag = false;
                if(dateFormat.IsNullOrWhiteSpaceEx() == true)
                {
                    dateFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                }
                bFlag = DateTime.TryParseExact(value, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Result);
                if (bFlag != true)
                {
                    bFlag = DateTime.TryParse(value, out Result);
                }
                if (bFlag != true)
                {
                    Result = new DateTime(1900, 1, 1);
                    //Result = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                //Result = DateTime.MinValue;
                Result = new DateTime(1900, 1, 1);
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return Result;
        }
        /// <summary>
        /// 날짜 형식(한국)을 포함한 문자열을 DateTime으로 형태로 가져오기 ex) 2024/1/1, 2024.1.01, 2024-01-01
        /// </summary>
        /// <param name="value">날짜 형식(한국)을 포함한 문자열</param>
        /// <returns>DateTime?</returns>
        public static DateTime? GetDateTimeFromKorFormat(string value)
        {
            DateTime? Result = null;
            if (value.IsNullOrWhiteSpaceEx() != true)
            {
                string strPatternDateType1 = @"^(\d{4,4})+(?:\/)+(\d{1,2})+(?:\/)+(\d{1,2})$";
                string strPatternDateType2 = @"^(\d{4,4})+(?:\.)+(\d{1,2})+(?:\.)+(\d{1,2})$";
                string strPatternDateType3 = @"^(\d{4,4})+(?:\-)+(\d{1,2})+(?:\-)+(\d{1,2})$";

                int iYear, iMonth, iDay;

                var matches = value.RegexMatchesEx(strPatternDateType1);

                if (matches == null || matches.Count < 0 || matches[0] == null || matches[0].Groups.Count < 4)
                {
                    matches = value.RegexMatchesEx(strPatternDateType2);
                    if (matches == null || matches.Count < 0 || matches[0] == null || matches[0].Groups.Count < 4)
                    {
                        matches = value.RegexMatchesEx(strPatternDateType3);
                    }
                }
                if (matches != null && matches.Count > 0 && matches[0] != null && matches[0].Groups.Count > 3)
                {
                    //value = $"{matches[0].Groups[1]}-{matches[0].Groups[2].ToStringEx().PadLeftEx(2, '0')}-{matches[0].Groups[3].ToStringEx().PadLeftEx(2, '0')}";
                    iYear = matches[0].Groups[1].ToIntEx();
                    iMonth = matches[0].Groups[2].ToIntEx();
                    iDay = matches[0].Groups[3].ToIntEx();
                    Result = new DateTime(iYear, iMonth, iDay);
                }
            }
            return Result;
        }

        public static DateTime GetDateTruncate(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }
        public static DateTime GetDateStart(DateTime dateTime)
        {
            return GetDateTruncate(dateTime);
        }
        public static DateTime GetDateStart(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                dateTime = new DateTime();
            }
            return GetDateTruncate((DateTime)dateTime);
        }
        public static DateTime GetDateCeiling(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }
        public static DateTime GetDateEnd(DateTime dateTime)
        {
            return GetDateCeiling(dateTime);
        }
        public static DateTime GetDateEnd(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                dateTime = new DateTime();
            }
            return GetDateCeiling((DateTime)dateTime);
        }
        public static DateTime GetDateRound(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 00, 00);
        }

        public static long GetUtcNowToUnixTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        public static long GetNowToUnixTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        #endregion
        public static string GetFileDirReplace(string input, string oldValue, string newValue)
        {
            string Result = input;
            if (oldValue.IsNullOrWhiteSpaceEx() != true)
            {
                Result = Result?.Replace(oldValue, newValue);
            }
            Result = Result?.RegexReplaceEx(@"^(\\\\\?\\)", string.Empty);
            return Result;
        }
        public static string GetSafeFileName(string fileName, bool bOptionSpecialCharReplace = true, string replaceChar = null)
        {
            string Result = fileName;
            if (Result.IsNullOrWhiteSpaceEx() != true)
            {
                Result = GetFileDirReplace(Result, null, null);
                if (bOptionSpecialCharReplace == true)
                {
                    //선택 변경 항목: ＝＆％＃＇；
                    Result = Result.RegexReplaceEx(@"=", "＝");
                    Result = Result.RegexReplaceEx(@"&", "＆");
                    Result = Result.RegexReplaceEx(@"%", "％");
                    Result = Result.RegexReplaceEx(@"#", "＃");
                    Result = Result.RegexReplaceEx(@"'", "＇");
                    Result = Result.RegexReplaceEx(@";", "；");
                }

                //기본 변경 항목: ＼／：＊？“＜＞│
                Result = Result.RegexReplaceEx(@"\", "＼");
                Result = Result.RegexReplaceEx(@"/", "／");
                Result = Result.RegexReplaceEx(@":", "：");
                Result = Result.RegexReplaceEx(@"*", "＊");
                Result = Result.RegexReplaceEx(@"?", "？");
                Result = Result.RegexReplaceEx("\"", "“");
                Result = Result.RegexReplaceEx(@"<", "＜");
                Result = Result.RegexReplaceEx(@">", "＞");
                Result = Result.RegexReplaceEx(@"|", "│");

                Result = Result.RegexReplaceEx(HxDefs._REGEX_BAD_NAME_PERTTERN_, replaceChar ?? string.Empty);
            }
            return Result;
        }
        public static string GetSafeDirName(string inputStr, bool bOptionSpecialCharReplace = true, string replaceChar = null)
        {
            string Result = inputStr;
            if (Result.IsNullOrWhiteSpaceEx() != true)
            {
                Result = GetFileDirReplace(Result, null, null);
                if (bOptionSpecialCharReplace == true)
                {
                    //선택 변경 항목: ＝＆％＃＇；
                    Result = Result.RegexReplaceEx("\\=", "＝");
                    Result = Result.RegexReplaceEx("\\&", "＆");
                    Result = Result.RegexReplaceEx("\\%", "％");
                    Result = Result.RegexReplaceEx("\\#", "＃");
                    Result = Result.RegexReplaceEx("\\'", "＇");
                    Result = Result.RegexReplaceEx("\\;", "；");
                }

                //기본 변경 항목: ＼／ː＊？“＜＞│
                //**DIR은 제외 //Result = Result.RegexReplaceEx(@"\", "＼");
                if (System.IO.Path.DirectorySeparatorChar != '\\')
                {
                    Result = Result.RegexReplaceEx("\\\\", "＼");
                }
                if (System.IO.Path.DirectorySeparatorChar != '/')
                {
                    Result = Result.RegexReplaceEx("\\/", "／");
                }
                //Result = Result.RegexReplaceEx("\\:", "：");
                Result = Result.RegexReplaceEx("\\*", "＊");
                Result = Result.RegexReplaceEx("\\?", "？");
                Result = Result.RegexReplaceEx("\\\"", "“");
                Result = Result.RegexReplaceEx("\\<", "＜");
                Result = Result.RegexReplaceEx("\\>", "＞");
                Result = Result.RegexReplaceEx("\\|", "│");

                //Result = Result.RegexReplaceEx(HxDefs._REGEX_BAD_NAME_PERTTERN_, replaceChar ?? string.Empty);
            }
            return Result;
        }
        public static string GetSafeWordName(string inputStr, string replaceChar = null)
        {
            string Result = inputStr;
            if (Result.IsNullOrWhiteSpaceEx() != true)
            {
                //"q`w^e!r@t#y$u%i^o&p*a(s)d_f-g+h=j{k}l|z:x\"c<v>b?n[m]q\\w;e'r,t.y/u"
                Result = Regex.Replace(Result,
                    "\\W",  /*Matches any nonword character. Equivalent to '[^A-Za-z0-9_]'*/
                    replaceChar??string.Empty,
                    RegexOptions.IgnoreCase
                    );
                //"qwertyuiopasd_fghjklzxcvbnmqwertyu"
            }
            return Result;
        }

        public static string GetFileSystemSafeName(string s)
        {
            //참고 : https://stackoverflow.com/questions/333175/is-there-a-way-of-making-strings-file-path-safe-in-c
            //출처 : https://stackoverflow.com/a/16083025
            return new string(s.Where(IsFileSystemSafe).ToArray());
        }

        public static bool IsFileSystemSafe(char c)
        {
            return !System.IO.Path.GetInvalidFileNameChars().Contains(c);
        }

        /// <summary>
        /// 정규식을 이용한 문자열 치환
        /// </summary>
        /// <param name="input">입력 문자열</param>
        /// <param name="pattern">정규식 패턴</param>
        /// <param name="replacement">치환 문자열</param>
        /// <param name="options">정규식 옵션</param>
        /// <returns>치환된 문자열</returns>
        public static string RegexReplace(string input, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            string Result = null;
            if (input.IsNullOrWhiteSpaceEx() == true || pattern.IsNullOrWhiteSpaceEx() == true) return input;
            //Result = value.Replace(oldValue, newValue);
            if (replacement == null)
            {
                replacement = string.Empty;
            }
            try
            {
                Regex regex = new Regex(pattern, options);
                Result = regex.Replace(input, replacement);
                return Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            
        }

        public static bool IsRegexMatch(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (input.IsNullOrWhiteSpaceEx() == true || pattern.IsNullOrWhiteSpaceEx() == true) return false;
            try
            {
                Regex regex = new Regex(pattern, options);
                return regex.IsMatch(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
                throw ex;
            }
            //return false;
        }
        public static Match RegexMatch(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (input.IsNullOrWhiteSpaceEx() == true || pattern.IsNullOrWhiteSpaceEx() == true) return null;
            try
            {
                Regex regex = new Regex(pattern, options);
                return regex.Match(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
                throw ex;
            }
            //return null;
            //regex.
        }

        public static MatchCollection RegexMatches(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (input.IsNullOrWhiteSpaceEx() == true || pattern.IsNullOrWhiteSpaceEx() == true) return null;
            try
            {
                Regex regex = new Regex(pattern, options);
                return regex.Matches(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
                throw ex;
            }
            //return null;
            //regex.
        }

        /// <summary>
        /// Array To List
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="array">Array Object</param>
        /// <returns>List</returns>
        public static List<T> GetList<T>(T[] array)
        {
            List<T> Result = null;
            if (array != null && array.Length > 0)
            {
                Result = new List<T>();
                foreach (var item in array)
                {
                    if(Result.Contains(item) != true)
                    {
                        Result.Add(item);
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// List to Array
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">List Resource</param>
        /// <returns>Array</returns>
        public static T[] GetArray<T>(List<T> list)
        {
            T[] Result = null;
            if(list != null && list.Count > 0)
            {
                Result = list.ToArray();
            }
            return Result;
        }
        public static string GetArrayJoin<T>(T[] values, string separatorChar = " ", string formatString = "{0}", bool isNullToAppend = true)
        {
            //return HxUtils.GetArrayJoin(values, separatorChar);
            string Result = null;
            //StringBuilder builder
            StringBuilder builder = new StringBuilder();
            if (values != null && values.Length > 0)
            {
                foreach (object o in values)
                {
                    string s = o.ToStringEx();
                    if(isNullToAppend != true && s.IsNullOrWhiteSpaceEx() == true) { continue; }

                    if (builder.Length > 0)
                    {
                        builder.Append(separatorChar);
                    }
                    builder.AppendFormat(formatString, s);
                }
                if (builder.Length > 0)
                {
                    Result = builder.ToStringEx();
                    builder.Clear();
                }
            }

            if(Result.IsNullOrWhiteSpaceEx() && values != null)
            {
                Result = string.Join(separatorChar, values);
            }
            return Result;
        }

        public static string GetListJoin<T>(List<T> list, string separatorChar = " ", string formatString = "{0}")
        {
            T[] values = GetArray(list);
            return GetArrayJoin(values, separatorChar, formatString);
        }

        /// <summary>
        /// 이메일 규칙?
        /// </summary>
        /// <param name="email">이메일 주소</param>
        /// <returns>boolean</returns>
        public static bool IsValidateEmail(string email)
        {
            bool Result = false;
            if (IsNullOrWhiteSpace(email) != true)
            {
                
                Result = Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
            }
            return Result;
        }
        public static string GetAnsiToUTF8String(string inputString)
        {
            string Result = inputString;
            try
            {

                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    byte[] bytes = GetString2Bytes(inputString, HxEncodingType.Default);
                    Result = Encoding.UTF8.GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static string GetUTF8ToUTF8String(string inputString)
        {
            string Result = inputString;
            try
            {

                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    byte[] bytes = GetString2Bytes(inputString, HxEncodingType.UTF8);
                    Result = Encoding.UTF8.GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static string GetUTF8ToAnsiString(string inputString)
        {
            string Result = inputString;
            try
            {

                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    byte[] bytes = GetString2Bytes(inputString, HxEncodingType.UTF8);
                    Result = Encoding.ASCII.GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static string GetStringToBase64Encode(string inputString, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            string Result = null;
            try
            {
                
                if(inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    byte[] bytes = GetString2Bytes(inputString, encodingType);
                    Result = GetByteToBase64Encode(bytes);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static string GetStringToBase64Encode(string inputString, Encoding encoding)
        {
            string Result = null;
            try
            {

                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    //Type a = typeof(encoding);
                    byte[] bytes = GetString2Bytes(inputString);
                    Result = GetByteToBase64Encode(bytes);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static string GetByteToBase64Encode(byte[] inputBytes, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            //참조 : http://www.csharpstudy.com/Tip/Tip-base64.aspx
            string Result = null;
            try
            {
                if (inputBytes != null && inputBytes.Length > 0)
                    Result = Convert.ToBase64String(inputBytes, options);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static string GetImageToBase64Encode(Image image, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            if(image == null) { return null; }
            byte[] bytes = HxImagePicture.ImageToByteArray(image);
            if (bytes == null || bytes.Length <= 0) { return null; }

            //return Convert.ToBase64String(bytes);
            return HxString.GetByteToBase64Encode(bytes, options);

        }
        public static string GetStringFromBase64Decode(string inputString, HxEncodingType encodingType = HxEncodingType.Unicode)
        {
            //참조 : http://www.csharpstudy.com/Tip/Tip-base64.aspx
            string Result = null;
            try
            {
                if(inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    byte[] bytes = Convert.FromBase64String(inputString);
                    Result = GetBytes2String(bytes, encodingType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static byte[] GetBytesFromBase64Decode(string inputString)
        {
            //참조 : http://www.csharpstudy.com/Tip/Tip-base64.aspx
            byte[] Result = null;
            try
            {
                if (inputString.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = Convert.FromBase64String(inputString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }


        public static string GetAndStartString(string additionalConditions)
        {
            string Result = additionalConditions;
            if(IsRegexMatch(additionalConditions.Trim(), "^(and )", RegexOptions.IgnoreCase) != true)
            {
                Result = " AND " + additionalConditions;
            }
            return Result;
        }

        /// <summary>
        /// Get Safe Query String
        /// </summary>
        /// <param name="queryString">쿼리 문자열</param>
        /// <returns>안전변환 쿼리 문자열</returns>
        public static string GetSafeQueryString(string queryString)
        {
            string Result = queryString.Trim();
            if (Result.IsNullOrWhiteSpaceEx())
            {
                Result = Result.RegexReplaceEx("\r\n", Environment.NewLine, System.Text.RegularExpressions.RegexOptions.Multiline);
                Result = Result.Replace(Environment.NewLine, "\n");
                Result = Result.RegexReplaceEx("\r", string.Empty, System.Text.RegularExpressions.RegexOptions.Multiline);
                Result = Result.RegexReplaceEx("\t", "    ", System.Text.RegularExpressions.RegexOptions.Multiline);
                Result = Result.RegexReplaceEx(@"([\n]+)$", "", System.Text.RegularExpressions.RegexOptions.Multiline);
            }
            return Result;
        }
        /// <summary>
        /// Get SQL/Query String / SELECT * FROM ( inputQueryString ) WHERE 1 = 1
        /// </summary>
        /// <param name="baseQuery">기본 쿼리</param>
        /// <param name="additionalWhere">조건절</param>
        /// <param name="additionalOrderby">정렬 조건</param>
        /// <returns>SQL 쿼리</returns>
        public static string SelectQueryString(string baseQuery, string additionalWhere, string additionalOrderby = null) //string queryString, string mWhere, string mWhere2 = null, string mWhere3 = null)
        {
            string Result = null;
            if (baseQuery.IsNullOrWhiteSpaceEx() != true)
            {
                if (baseQuery.Trim().ToUpper().StartsWith("WITH") != true && baseQuery.Trim().ToUpper().StartsWith("SELECT") != true)
                {
                    baseQuery = string.Format("SELECT * FROM ( {0} ) WHERE 1 = 1", baseQuery.Trim());
                }
                Result = WhereQueryString(baseQuery, additionalWhere);
                if (additionalOrderby.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = OrderByQueryString(Result, additionalOrderby);
                }
                /*
                if(mWhere2.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = WhereQueryString(Result, mWhere2);
                }
                if (mWhere3.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = WhereQueryString(Result, mWhere3);
                }
                */
        }
            return Result;
        }

        /// <summary>
        /// Get SQL/Query String / SELECT * FROM ( inputQueryString ) WHERE 1 = 1
        /// </summary>
        /// <param name="baseQuery">기본 쿼리</param>
        /// <param name="mWhereParams">조건절 Array</param>
        /// <returns>SQL 쿼리</returns>
        public static string SelectQueryString(string baseQuery, params string[] additionalParams)
        {
            string Result = null;
            if (baseQuery.IsNullOrWhiteSpaceEx() != true)
            {
                if (baseQuery.Trim().ToUpper().StartsWith("WITH ") != true && baseQuery.Trim().ToUpper().StartsWith("SELECT ") != true)
                {
                    baseQuery = string.Format("SELECT * FROM ( {0} ) WHERE 1 = 1", baseQuery.Trim());
                }
                Result = WhereQueryString(baseQuery, additionalParams);
            }
            return Result;
        }
        /// <summary>
        /// Get SQL/Query String / SELECT * FROM ( inputQueryString ) WHERE 1 = 1
        /// </summary>
        /// <param name="baseQuery">기본 쿼리</param>
        /// <param name="additionalOrderby">정렬 조건</param>
        /// <param name="additionalParams">검색 조건절 Array</param>
        /// <returns></returns>
        public static string SelectQueryStringWithOrderBy(string baseQuery, string additionalOrderby, params string[] additionalParams)
        {
            string Result = null;
            Result = SelectQueryString(baseQuery, additionalParams);
            if (Result.IsNullOrWhiteSpaceEx() != true && additionalOrderby.IsNullOrWhiteSpaceEx() != true)
            {
                Result = OrderByQueryString(Result, additionalOrderby);
            }
            return Result;
        }

        /// <summary>
        /// Get SQL/Query String 
        /// </summary>
        /// <param name="baseQuery">기본 쿼리</param>
        /// <param name="additionalConditions">조건절</param>
        /// <returns>SQL 쿼리</returns>
        public static string WhereQueryString(string baseQuery, string additionalConditions = null)
        {
            /*
            if (baseQuery.IsNullOrWhiteSpaceEx()) throw new ArgumentNullException(nameof(baseQuery));
            if (additionalConditions.IsNullOrWhiteSpaceEx()) return baseQuery;
            StringBuilder sb = new StringBuilder(baseQuery.Trim());
            if (!additionalConditions.TrimStart().StartsWith("AND", StringComparison.OrdinalIgnoreCase) &&
                !additionalConditions.TrimStart().StartsWith("OR", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append(" AND ");
            }
            else
            {
                sb.Append(" ");
            }
            sb.Append(additionalConditions.Trim());
            return sb.ToString();
            */

            string Result = baseQuery;
            if (!Result.IsNullOrWhiteSpaceEx() && !additionalConditions.IsNullOrWhiteSpaceEx())
            {
                if (Result.ToUpper().Contains("WHERE ") != true)
                {
                    Result += " WHERE 1 = 1 ";
                }
                if (!additionalConditions.ToUpper().Trim().StartsWith("AND ") && !additionalConditions.ToUpper().Trim().StartsWith("OR ") && !additionalConditions.ToUpper().Trim().StartsWith("ORDER BY ")
                    && !additionalConditions.ToUpper().Trim().StartsWith("START WITH ") && !additionalConditions.ToUpper().Trim().StartsWith("CONNECT BY ")
                    )
                {
                    Result += " AND ";
                }
                Result += " " + additionalConditions;
            }
            return Result;
        }
        
        /// <summary>
        /// Get SQL/Query String 
        /// </summary>
        /// <param name="baseQuery">기본 쿼리</param>
        /// <param name="additionalParams">조건절 Array</param>
        /// <returns>SQL 쿼리</returns>
        public static string WhereQueryString(string baseQuery, params string[] additionalParams)
        {
            string Result = baseQuery;
            if (!Result.IsNullOrWhiteSpaceEx() && !additionalParams.IsNullOrWhiteSpaceEx())
            {
                if (!Result.ToUpper().Contains("WHERE "))
                {
                    Result += " WHERE 1 = 1 ";
                }

                foreach(string mWhere in additionalParams)
                {
                    Result = WhereQueryString(Result, mWhere);
                }
            }
            return Result;
        }

        public static string OrderByQueryString(string baseQuery, string additionalOrderby)
        {
            string Result = baseQuery;
            if(Result.IsNullOrWhiteSpaceEx() != true && additionalOrderby.IsNullOrWhiteSpaceEx() != true)
            {
                string customOrderBy = additionalOrderby;
                
                string[] lines = Result.Trim().RegexReplaceEx("\r+", "\n").RegexReplaceEx("\n+", "\n").RegexReplaceEx("(\n{1,})$", string.Empty).Split('\n');
                if(lines != null && lines.Length > 0)
                {
                    customOrderBy = customOrderBy.RegexReplaceEx(@"(\s*,\s*)", ",").RegexReplaceEx("(,)+(,){1,}", ", ");
                    string lastLine = lines[lines.Length - 1];
                    if (lastLine != null)
                    {
                        if (lastLine.IsRegexMatchEx("(ORDER BY)", RegexOptions.IgnoreCase) == true)
                        {
                            customOrderBy = customOrderBy.RegexReplaceEx("(ORDER BY)", ",", RegexOptions.IgnoreCase);
                        }
                        else if(customOrderBy.IsRegexMatchEx("(ORDER BY)", RegexOptions.IgnoreCase) == true)
                        {
                            //customOrderBy = " ORDER BY " + customOrderBy;
                        }
                        else
                        {
                            customOrderBy = " ORDER BY " + customOrderBy;
                        }
                        //if (lastLine.IsRegexMatchEx("(ORDER BY)", RegexOptions.IgnoreCase) == true && customOrderBy.IsRegexMatchEx("(ORDER BY)", RegexOptions.IgnoreCase) == true)
                        //{
                        //    customOrderBy = ", " + customOrderBy.RegexReplaceEx("(ORDER BY)", "", RegexOptions.IgnoreCase);
                        //}
                        //else if (lastLine.IsRegexMatchEx("(ORDER BY)", RegexOptions.IgnoreCase) == true && customOrderBy.IsRegexMatchEx("(ORDER BY)", RegexOptions.IgnoreCase) != true)
                        //{
                        //    customOrderBy = ", " + customOrderBy;
                        //}



                        //if (customOrderBy.IsRegexMatchEx("(ORDER BY)", RegexOptions.IgnoreCase) != true)
                        //{
                        //    Result += " ORDER BY " + customOrderBy;
                        //}
                        //else
                        //{
                        //    Result += " " + customOrderBy;
                        //}
                        Result += " " + customOrderBy;
                    }
                }
            }
            return Result;
        }


        public static string[] SplitEx(string input, string separator, StringSplitOptions option = StringSplitOptions.None)
        {
            if (!input.IsNullOrWhiteSpaceEx() && !separator.IsNullOrWhiteSpaceEx())
            {
                return input.Split(new string[] { separator }, option);
            }
            else
            {
                return null;
            }
        }

        public static string[] SplitCharEx(string input, char[] separator, StringSplitOptions option = StringSplitOptions.None, bool isSeparatorNullToDefaultSpecialCharacters = true)
        {
            if (!input.IsNullOrWhiteSpaceEx() && separator.IsNullOrWhiteSpaceEx() && isSeparatorNullToDefaultSpecialCharacters == true)
            {
                separator = new char[] { '＃', '？', '＆', '＝', '％', '＠', '￦', '※', '●', '■', '☜', '☞', '¶' };
            }
            if (!input.IsNullOrWhiteSpaceEx() && separator != null && separator.Length > 0 && !separator.IsNullOrWhiteSpaceEx())
            {
                return input.Split(separator, option);
            }
            else
            {
                return null;
            }
        }

        public static List<string> SplitToListEx(string input, string separator, bool bOverwrite = false)
        {
            string[] strArray = input.SplitEx(separator);
            if (strArray != null && strArray.Length > 0)
            {
                List<string> Result = new List<string>();
                foreach (string str in strArray)
                {
                    Result.AddEx(str, bOverwrite);
                }
                return Result;
            }
            else
            {
                return null;
            }
        }

        public static List<T> SplitToListEx<T>(string input, string separator, bool bOverwrite = false)
        {
            string[] strArray = input.SplitEx(separator);
            if (strArray != null && strArray.Length > 0)
            {
                List<T> Result = new List<T>();
                foreach (string str in strArray)
                {
                    if (str.IsNullOrWhiteSpaceEx() != true)
                    {
                        T value = str.ToConvertEx<T>();
                        if (value.IsNullOrWhiteSpaceEx() != true)
                        {
                            Result.AddEx(value, bOverwrite);
                        }
                    }
                }
                return Result;
            }
            else
            {
                return null;
            }
        }

        public static string[] SplitToArray(string input, string pattern)
        {
            return Regex.Split(input, pattern);
        }

        public static string[] SplitToLineArray(string input)
        {
            return SplitToArray(input, "\r\n|\r|\n");
        }

        public static List<string> SplitToLineList(string input)
        {
            List<string> Result = null;
            if (input.IsNullOrWhiteSpaceEx() != true)
            {
                Result = new List<string>();
                using (System.IO.StringReader sr = new System.IO.StringReader(input))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Result.Add(line);
                    }
                }
            }
            return Result;
        }

        public static int FindCharCount(string input, string search)
        {
            string[] StringArray = input.Split(new string[] { search }, StringSplitOptions.None);
            return StringArray?.Length??0 - 1;
        }
        
        public static string UrlEncode(string input)
        {
            return System.Web.HttpUtility.UrlEncode(input);
        }
        public static string UrlDecode(string input)
        {
            return System.Web.HttpUtility.UrlDecode(input);
        }

        public static string HtmlEncode(string input)
        {
            return System.Web.HttpUtility.HtmlEncode(input);
        }
        public static string HtmlDecode(string input)
        {
            return System.Web.HttpUtility.HtmlDecode(input);
        }

        private static string UnicodeToHangle(string input = "&#50500;&#47924;")
        {
            input = System.Web.HttpUtility.HtmlDecode(input).Replace("&#", string.Empty);
            string[] arr = input.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder();

            foreach (string a in arr)
            {
                int i = int.Parse(a);
                sb.Append(char.ConvertFromUtf32(i));
            }

            //Console.WriteLine(sb.ToString());

            return sb.ToString();

            //출처: http://rex0725.tistory.com/6?category=720744 [private db]
        }

        public static string UriEscapeString(string input, bool bReplaceEscapeAnd = false)
        {
            string Result = input;
            try
            {
                if (input.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = Uri.EscapeUriString(input);
                    if (bReplaceEscapeAnd == true && Result.Contains("&"))
                    {
                        Result = Result.Replace("&", "%26");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public static string PadLeft(string input, int totalWidth, char paddingChar = ' ', bool isInputWidthBigApply = false)
        {
            if (input.IsNullOrWhiteSpaceEx() == true || totalWidth <= 0) return input;
            
            int nLength = input.Length;
            if(nLength >= totalWidth && isInputWidthBigApply == false) return input;

            return input.PadLeft(totalWidth, paddingChar);
        }

        public static string PadRight(string input, int totalWidth, char paddingChar = ' ', bool isInputWidthBigApply = false)
        {
            if (input.IsNullOrWhiteSpaceEx() == true || totalWidth <= 0) return input;

            int nLength = input.Length;
            if (nLength >= totalWidth && isInputWidthBigApply == false) return input;

            return input.PadRight(totalWidth, paddingChar);
        }

        public static string ToCutString(string input, uint length, string cutStrReplace = "...")
        {
            string Result = null;
            if(input.IsNullOrWhiteSpaceEx() != true && length > 0 && input.Length > length)
            {
                Result = input.Substring(0, length.ToIntEx());
                if(cutStrReplace.IsNullOrWhiteSpaceEx() != true)
                {
                    Result += cutStrReplace;
                }
            }
            else
            {
                Result = input;
            }
            return Result;
        }


    }


    public class HxUTF8StringWriter : System.IO.StringWriter
    {
        //출처 : https://stackoverflow.com/questions/3871738/force-xdocument-to-write-to-string-with-utf-8-encoding
        public override Encoding Encoding => Encoding.UTF8;
        //
        // 요약:
        //     Initializes a new instance of the System.IO.StringWriter class.
        public HxUTF8StringWriter()
            : base()
        {
            ; ;
        }
        //
        // 요약:
        //     Initializes a new instance of the System.IO.StringWriter class with the specified
        //     format control.
        //
        // 매개 변수:
        //   formatProvider:
        //     An System.IFormatProvider object that controls formatting.
        public HxUTF8StringWriter(IFormatProvider formatProvider)
            : base(formatProvider)
        {
            ; ;
        }
        //
        // 요약:
        //     Initializes a new instance of the System.IO.StringWriter class that writes to
        //     the specified System.Text.StringBuilder.
        //
        // 매개 변수:
        //   sb:
        //     The System.Text.StringBuilder object to write to.
        //
        // 예외:
        //   T:System.ArgumentNullException:
        //     sb is null.
        public HxUTF8StringWriter(StringBuilder sb)
            : base(sb)
        {
            ; ;
        }
        //
        // 요약:
        //     Initializes a new instance of the System.IO.StringWriter class that writes to
        //     the specified System.Text.StringBuilder and has the specified format provider.
        //
        // 매개 변수:
        //   sb:
        //     The System.Text.StringBuilder object to write to.
        //
        //   formatProvider:
        //     An System.IFormatProvider object that controls formatting.
        //
        // 예외:
        //   T:System.ArgumentNullException:
        //     sb is null.
        public HxUTF8StringWriter(StringBuilder sb, IFormatProvider formatProvider)
            : base(sb, formatProvider)
        {
            ; ;
        }

        

        #region Convert from Oracle's RAW(16) to .NET's GUID
        //출처 : https://stackoverflow.com/questions/7289734/convert-from-oracles-raw16-to-nets-guid
        public static string OracleToDotNet(string text)
        {
            byte[] bytes = ParseHex(text);
            Guid guid = new Guid(bytes);
            return guid.ToString("N").ToUpperInvariant();
        }

        public static string DotNetToOracle(string text)
        {
            Guid guid = new Guid(text);
            return BitConverter.ToString(guid.ToByteArray()).Replace("-", "");
        }

        public static byte[] ParseHex(string text)
        {
            // Not the most efficient code in the world, but
            // it works...
            byte[] ret = new byte[text.Length / 2];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = Convert.ToByte(text.Substring(i * 2, 2), 16);
            }
            return ret;
        }
        #endregion

        public struct HxUriRec
        {
            public const string _URL_PATTERN_ = @"^(?<protocol>https?):\/\/(?<domain>[^\/]+)(?<path>\/[^?]*)(?<query>\?.*)?$";
            public string BaseUrl => $"{Protocol}://{Domain}";
            public string Uri { get; private set; }
            
            public string Protocol { get; private set; }
            public string Domain { get; private set; }
            public string Path { get; private set; }
            public string QueryString { get; private set; }
            
            public HxUriRec(string inputUri)
            {
                Uri = inputUri;
                Protocol = string.Empty;
                Domain = string.Empty;
                Path = string.Empty;
                QueryString = string.Empty;
                if(Uri.IsNullOrWhiteSpaceEx() != true)
                {
                    var regex = new Regex(_URL_PATTERN_);

                    Match match = regex.Match(Uri);

                    if (match.Success)
                    {
                        Protocol = match.Groups["protocol"].Value;
                        Domain = match.Groups["domain"].Value;
                        Path = match.Groups["path"].Value;
                        QueryString = match.Groups["query"].Value;
                        /*
                        // 기본 도메인 URL (프로토콜 + 도메인)
                        string baseUrl = $"{Protocol}://{Domain}";

                        Console.WriteLine($"✅ URL 분석 성공:");
                        Console.WriteLine($"-------------------------------------------------");
                        Console.WriteLine($"- 프로토콜 (Protocol)  : {Protocol}");
                        Console.WriteLine($"- 기본 도메인 (Base URL) : {baseUrl}");
                        Console.WriteLine($"- 뒷부분 URI (Path)    : {this.Path}");
                        Console.WriteLine($"- 쿼리스트링 (Query)   : {QueryString}");
                        */
                    }
                }
            }

            public void SetUrlParser(string uri)
            {
                
            }
        }
        

        public void s()
        {
            DataTable dt = new DataTable();
            var q = dt.AsEnumerable();
        }

        //public static 
        
    }
}
