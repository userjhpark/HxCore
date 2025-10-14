using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore
{
    public static partial class HxExtension
    {
        public static bool IsNullOrEmptyEx(this string value)
        {
            return String.IsNullOrEmpty(value);
        }
        /// <summary>
        /// [Extension] 공백 또는 Null 여부
        /// </summary>
        /// <param name="value">입력 문자열</param>
        /// <returns>True : 공백 또는 Null</returns>
        public static bool IsNullOrWhiteSpaceEx(this string value)
        {
            return HxString.IsNullOrWhiteSpace(value);
        }
        /// <summary>
        /// [Extension] 공백 또는 Null 여부
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>True : 공백 또는 Null</returns>
        public static bool IsNullOrWhiteSpaceEx(this object value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                return HxString.IsNullOrWhiteSpace(value.ToStringEx());
            }
        }
        /// <summary>
        /// [Extension] Null, int.MinValue, Zero(0) 여부
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsNullOrZeroMinEx(this int? value)
        {
            if (value == null)
            {
                return true;
            } else if (value != null && (value == 0 || value == int.MinValue))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, Zero(0), Minor 여부
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsNullOrZeroMinorEx(this int? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && (value <= 0 || value == int.MinValue))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, Minor 여부
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsNullOrMinorEx(this int? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && (value < 0 || value == int.MinValue))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, int.MinValue 여부( without Zero(0) )
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsNullOrMinValueEx(this int? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && value == int.MinValue)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] int.MinValue보다 작거나 같은지 체크
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsMinValueEx(this int value)
        {
            if (value <= int.MinValue)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, int.MinValue, Zero(0) 여부
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsZeroMinEx(this int value)
        {
            if ((value == 0 || value == int.MinValue))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, int.MinValue 여부( without Zero(0) )
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsZeroMinorValueEx(this int value)
        {
            if (value <= 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, int.MinValue 여부( without Zero(0) )
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsMinorValueEx(this int value)
        {
            if (value < 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, int.MinValue, Zero(0) 여부
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsZeroMinEx(this uint value)
        {
            if ((value == 0 || value <= uint.MinValue))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// [Extension] Null, int.MinValue 여부( without Zero(0) )
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>결과?</returns>
        public static bool IsZeroMinorValueEx(this decimal value)
        {
            if (value <= 0)
            {
                return true;
            }
            return false;
        }
        public static bool IsNullOrZeroMinEx(this decimal? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && (value == 0 || value == int.MinValue || value == decimal.MinValue))
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrMinValueEx(this decimal? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && value == int.MinValue || value == decimal.MinValue)
            {
                return true;
            }
            return false;
        }
        public static bool IsNullOrZeroMinorValueEx(this decimal? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && value <= 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsMinValueEx(this decimal value)
        {
            if (value <= (decimal)int.MinValue)
            {
                return true;
            }
            return false;
        }

        public static bool IsMinorValueEx(this decimal value)
        {
            if (value < 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsZeroOrMinValueEx(this decimal value)
        {
            if (value <= (decimal)int.MinValue || value == 0)
            {
                return true;
            }
            return false;
        }



        public static bool IsNullOrZeroMinEx(this long? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && (value == 0 || value == int.MinValue || value == decimal.MinValue || value == long.MinValue))
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrMinValueEx(this long? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && value == int.MinValue || value == decimal.MinValue || value == long.MinValue)
            {
                return true;
            }
            return false;
        }
        public static bool IsNullOrZeorMinorValueEx(this long? value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value != null && value <= 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsMinValueEx(this long value)
        {
            if (value <= (long)int.MinValue)
            {
                return true;
            }
            return false;
        }
        public static bool IsZeroOrMinValueEx(this long value)
        {
            if (value <= (long)int.MinValue || value == 0)
            {
                return true;
            }
            return false;
        }
        public static bool IsZeroOrMinorValueEx(this long value)
        {
            if (value <= (long)int.MinValue || value <= 0)
            {
                return true;
            }
            return false;
        }
        public static string ToCutStringEx(this string input, uint length, string cutStrReplace = "...")
        {
            return HxString.ToCutString(input, length, cutStrReplace);
        }

        /// <summary>
        /// [Extension] System.Object를 System.String로 반환 (Null일 경우 Empty)
        /// </summary>
        /// <param name="sender">Object</param>
        /// <returns>String</returns>
        public static string ToStringEx(this object sender, bool bBooleanTypeNotNullToConvertYorN = false)
        {
            if (sender != null && HxString.IsNullOrWhiteSpace(sender?.ToString()) != true && bBooleanTypeNotNullToConvertYorN == true)
            {
                string Result = string.Empty;
                string str = sender?.ToString();
                if (str.IsNullOrWhiteSpaceEx() != true)
                {
                    switch (str.ToUpper())
                    {
                        case "TRUE":
                        case "Y":
                        case "YES":
                        case "USE":
                        case "USED":
                        case "ENABLE":
                        case "OK":
                        case "SUCCESS":
                            Result = "Y";
                            break;
                        case "FALSE":
                        case "N":
                        case "NO":
                        case "NOT":
                        case "UN":
                        case "DISABLE":
                        case "CANCEL":
                        case "FAIL":
                            Result = "N";
                            break;
                    }
                }
                return Result;
            }
            return (sender == null ? string.Empty : sender.ToString());
        }
        public static string ToStringEx(this object sender, string defaultValue)
        {
            string Result = sender.ToStringEx();
            if(Result.IsNullOrWhiteSpaceEx() == true && defaultValue.IsNullOrWhiteSpaceEx() != true)
            {
                Result = defaultValue;
            }
            return Result;
        }

        public static string ToStringNullorEmptyEx(this object sender, string defaultValue = "", bool bBooleanTypeNotNullToConvertYorN = false)
        {
            if(sender.IsNullOrWhiteSpaceEx() != true)
            {
                return sender.ToStringEx(bBooleanTypeNotNullToConvertYorN);
            }
            else
            {
                return defaultValue;
            }
        }
        public static string ToStringEx(this bool sender, bool bNotNullToConvertYorN = false)
        {
            string Result = sender.ToString();
            if (bNotNullToConvertYorN == true)
            {
                if (sender == true)
                {
                    Result = "Y";
                }
                else if (sender == false)
                {
                    Result = "N";
                }
            }
            return Result;
        }

        public static string ToStringEx(this bool? sender, bool bNotNullToConvertYorN = false)
        {
            string Result = sender?.ToString();
            if (bNotNullToConvertYorN == true)
            {
                if (sender == true)
                {
                    Result = "Y";
                }
                else if (sender == false)
                {
                    Result = "N";
                }
            }
            return Result;
        }

        public static string ToSize2KByteStringEx(this long sender)
        {
            return HxFile.GetSize2KByteString(sender);
        }
        public static string ToSize2KByteStringEx(this long? sender)
        {
            return HxFile.GetSize2KByteString(sender);
        }

        /// <summary>
        /// 용량 단위 표기한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>문자방식 사이즈</returns>
        public static string ToSize2HumanSizeStringEx(this long? size)
        {
            return HxFile.GetSize2HumanSizeString<long?>(size);
        }
        /// <summary>
        /// 용량 단위 표기한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>문자방식 사이즈</returns>
        public static string ToSize2HumanSizeStringEx(this long size)
        {
            return HxFile.GetSize2HumanSizeString<long>(size);
        }

        public static string ToNumberStringEx(this int sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender, format, provider);
        }
        public static string ToNumberStringEx(this int? sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender.ToIntEx(0), format, provider);
        }
        public static string ToNumberStringEx(this uint sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender, format, provider);
        }
        public static string ToNumberStringEx(this long sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender, format, provider);
        }
        public static string ToNumberStringEx(this ulong sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender, format, provider);
        }
        public static string ToNumberStringEx(this double sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender, format, provider);
        }
        public static string ToNumberStringEx(this decimal sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender, format, provider);
        }
        public static string ToNumberStringEx(this decimal? sender, string format = "#,##0", IFormatProvider provider = null)
        {
            return HxString.GetNumberString(sender.ToDecimalEx(0), format, provider);
        }

        public static string ToStringEx(this DateTime sender, string dateFormat = "yyyy-MM-dd")
        {
            return HxString.GetString(sender, dateFormat);
        }
        public static string ToStringEx(this DateTime? sender, string dateFormat = "yyyy-MM-dd")
        {
            return HxString.GetString(sender, dateFormat);
        }

        public static string ToDateTimeStringEx(this DateTime? sender, string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return HxString.GetString(sender, dateFormat);
        }

        public static string ToStringEx(this byte[] sender, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            return HxString.GetBytes2String(sender, encodingType);
        }

        public static string ToStringEx(this List<string> sender, string separatorChar = " ")
        {
            if(sender != null && sender.Count > 0)
            {
                string[] arry = sender.ToArray();
                return HxUtils.GetArrayJoin(arry, separatorChar);
            }
            else
            {
                return null;
            }
        }

        public static string ToStringJoinEx<T>(this T[] sender, string separatorChar = " ", string formatString = "{0}")
        {
            return HxString.GetArrayJoin(sender, separatorChar, formatString);
        }
        public static string ToStringJoinEx<T>(this List<T> sender, string separatorChar = " ", string formatString = "{0}")
        {
            return HxString.GetListJoin(sender, separatorChar, formatString);
        }
        public static string ToStringJoinEx<T>(this List<T> sender, char separatorChar = ' ', string formatString = "{0}")
        {
            return HxString.GetListJoin(sender, separatorChar.ToStringEx(), formatString);
        }

        public static string ToBase64StringEx(this byte[] sender)
        {
            return HxString.GetByteToBase64Encode(sender);
        }

        /// <summary>
        /// [Extension] StringBuilder Clear
        /// </summary>
        /// <param name="Sender">StringBuilder Resource</param>
        public static void ClearEx(this StringBuilder Sender)
        {
            HxString.Clear(Sender);
        }
        /// <summary>
        /// [Extension]Object To Bool
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Bool</returns>
        public static bool ToBoolEx(this object sender, bool defaultValue = false)
        {
            return HxConvert.ToBool(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]String To Bool
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Bool</returns>
        public static bool ToBoolEx(this string sender, bool defaultValue = false)
        {
            return HxConvert.ToBool(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]String To Bool?
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Bool?</returns>
        public static bool? ToNullableBoolEx(this object sender, bool? defaultValue = null)
        {
            return HxConvert.ToNullableBool(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To Int
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Int</returns>
        public static int ToIntEx(this object sender, int defaultValue = int.MinValue)
        {
            return HxConvert.ToInt(sender, defaultValue);
        }
        public static int ToIntEx(this object sender, uint defaultValue)
        {
            return HxConvert.ToInt(sender, (int)defaultValue);
        }
        /// <summary>
        /// [Extension]Object To UInt
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>UInt</returns>
        public static uint ToUIntEx(this object sender, uint defaultValue = uint.MinValue)
        {
            return HxConvert.ToUInt(sender, defaultValue);
        }
        public static int ToPlusIntEx(this int sender, int defaultValue = int.MinValue)
        {
            return HxConvert.ToPlusInt(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To UInt
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Int?</returns>
        public static int? ToNullableIntEx(this object sender, int? defaultValue = null)
        {
            return HxConvert.ToInt(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To Decimal
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Decimal</returns>
        public static decimal ToDecimalEx(this object sender, decimal defaultValue = (decimal)int.MinValue)
        {
            return HxConvert.ToDecimal(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To Float
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>float</returns>
        public static float ToFloatEx(this object sender, float defaultValue = (float)int.MinValue)
        {
            return HxConvert.ToFloat(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To Decimal?
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Decimal?</returns>
        public static decimal? ToNullableDecimalEx(this object sender, decimal? defaultValue = null)
        {
            return HxConvert.ToDecimal(sender, defaultValue);
        }
        public static double ToDoubleEx(this object sender, double defaultValue = (double)int.MinValue)
        {
            return HxConvert.ToDecimal(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To Long
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Long</returns>
        public static long ToLongEx(this object sender, long defaultValue = (long)int.MinValue)
        {
            return HxConvert.ToLong(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To ULong
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>ULong</returns>
        public static ulong ToULongEx(this object sender, ulong defaultValue = 0)
        {
            return HxConvert.ToULong(sender, defaultValue);
        }
        /// <summary>
        /// [Extension]Object To Long?
        /// </summary>
        /// <param name="sender">value</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Long?</returns>
        public static long? ToNullableLongEx(this object sender, long? defaultValue = null)
        {
            return HxConvert.ToLong(sender, defaultValue);
        }

        /// <summary>
        /// [Extension] System.Object를 변환될 타입별로 반환(Generic 타입)
        /// </summary>
        /// <typeparam name="T">리턴 Type</typeparam>
        /// <param name="sender">Object</param>
        /// <returns>Generic Type</returns>
        public static T ToConvertEx<T>(this object sender)
        {
            return HxConvert.ConvertTo<T>(sender);
        }
        
        /// <summary>
        /// [Extension] 지정된 날짜포멧의 문자열을 DateTime으로 형 변환(NULL : 1900-01-01)
        /// </summary>
        /// <param name="value">날짜포멧의 문자열(String with DateTime Format)</param>
        /// <param name="dateFormat">날짜포멧</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTimeEx(this object value, string dateFormat = "yyyy-MM-dd")
        {
            if (value != null && value is DateTime)
            {
                return (DateTime)value;
            }
            else
            {
                return HxString.GetDateTime(value, dateFormat);
            }
        }
        /// <summary>
        /// [Extension] 지정된 날짜포멧의 문자열을 DateTime으로 형 변환(NULL : 1900-01-01)
        /// </summary>
        /// <param name="value">날짜포멧의 문자열(String with DateTime Format)</param>
        /// <param name="dateFormat">날짜포멧</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTimeEx(this string value, string dateFormat = "yyyy-MM-dd")
        {
            //if (value.IsNullOrWhiteSpaceEx())
            //{
            //    return HxUtils.GetNowDateTime();
            //}
            return HxString.GetDateTime(value, dateFormat);
        }
        /// <summary>
        /// [Extension] 해당 날짜의 시작 일시 (2021-10-26 00:00:00)
        /// </summary>
        /// <param name="value">입력 일시</param>
        /// <returns>시작 일시</returns>
        public static DateTime ToDateStartEx(this DateTime value)
        {
            return HxString.GetDateStart(value);
        }
        /// <summary>
        /// [Extension] 해당 날짜의 시작일시 (2021-10-26 00:00:00)
        /// </summary>
        /// <param name="value">입력 일시</param>
        /// <returns>시작 일시</returns>
        public static DateTime ToDateStartEx(this DateTime? value)
        {
            return HxString.GetDateStart(value);
        }
        /// <summary>
        /// [Extension] 해당 날짜의 마지막 일시 (2021-10-26 23:59:59)
        /// </summary>
        /// <param name="value">입력 일시</param>
        /// <returns>마지막 일시</returns>
        public static DateTime ToDateEndEx(this DateTime value)
        {
            return HxString.GetDateEnd(value);
        }
        /// <summary>
        /// [Extension] 해당 날짜의 마지막 일시 (2021-10-26 23:59:59)
        /// </summary>
        /// <param name="value">입력 일시</param>
        /// <returns>마지막 일시</returns>
        public static DateTime ToDateEndEx(this DateTime? value)
        {
            return HxString.GetDateEnd(value);
        }
        /// <summary>
        /// [Extension] 날짜 형식(한국)을 포함한 문자열을 DateTime으로 형태로 가져오기 ex) 2024/1/1, 2024.1.01, 2024-01-01
        /// </summary>
        /// <param name="value">날짜 형식(한국)을 포함한 문자열</param>
        /// <returns>DateTime?</returns>
        public static DateTime? GetDateTimeFromKorFormatEx(this string value)
        {
            return HxString.GetDateTimeFromKorFormat(value);
        }

        public static DateTime? ToNullableDateEx(this object value, string dateFormat = null, bool IsNullEqualsMinValue = false)
        {
            DateTime? Result;
            try
            {
                DateTime minCustomDateValue = new DateTime(1900, 1, 1);
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
                    if (Result == minCustomDateValue || Result == DateTime.MinValue ||(Result != null && Result <= minCustomDateValue))
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
                if (Result == null && IsNullEqualsMinValue == true)
                {
                    Result = minCustomDateValue;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Result = null;
            }
            return Result;
        }

        public static DateTime? ToNullableDateEx(this string value, string dateFormat = "yyyy-MM-dd")
        {
            DateTime? Result = null;
            try
            {
                if (value.IsNullOrWhiteSpaceEx() != true)
                {
                    DateTime minCustomDateValue = new DateTime(1900, 1, 1);
                    Result = value.ToDateTimeEx(dateFormat);
                    //Result = Result.GetValueOrDefault(minCustomDateValue);
                    if (Result == minCustomDateValue || Result == DateTime.MinValue)
                    {
                        Result = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Result = null;
            }
            return Result;
        }

        public static DateTime? ToNullableDateTimeEx(this object value, string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return value.ToNullableDateEx(dateFormat);
        }
        public static DateTime? ToNullableDateTimeEx(this string value, string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return value.ToNullableDateTimeEx(dateFormat);
        }

        /// <summary>
        /// [Extension] 최소 지정날짜 지정(1900-01-01 / DB등 호환 목적)
        /// </summary>
        /// <param name="sender">Object</param>
        /// <returns></returns>
        public static DateTime ToMinValueEx(this DateTime sender)
        {
            return HxUtils.MinDateTime();
        }

        /// <summary>
        /// [Extension] DateTime을 지정된 날짜포멧의 문자열로 변환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">날짜포멧</param>
        /// <returns>DateTime String</returns>
        public static string ToDateTimeStringEx(this DateTime value, string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return HxString.GetString(value, dateFormat);
        }

        /// <summary>
        /// [Extension] DateTime을 지정된 날짜포멧의 문자열로 변환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">날짜/시간 포멧</param>
        /// <returns>DateTime String</returns>
        public static string ToNullableDateTimeStringEx(this DateTime? value, string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return HxString.GetString(value, dateFormat);
        }
        /// <summary>
        /// [Extension] DateTime을 지정된 날짜포멧의 문자열로 변환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">날짜/시간 포멧</param>
        /// <returns>DateTime String</returns>
        public static string ToDateMicroTimeLongStringEx(this DateTime value, string dateFormat = "yyyy-MM-dd HH:mm:ss.fffffff")
        {
            //string dateFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
            return HxString.GetString(value, dateFormat);
        }

        /// <summary>
        /// [Extension] DateTime을 지정된 날짜포멧의 문자열로 변환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">날짜/시간 포멧</param>
        /// <returns>DateTime String</returns>
        public static string ToDateStringEx(this DateTime value, string dateFormat = "yyyy-MM-dd")
        {
            //string dateFormat = "yyyy-MM-dd HH:mm:ss";
            return HxString.GetString(value, dateFormat);
        }

        /// <summary>
        /// [Extension] DateTime을 지정된 날짜포멧의 문자열로 변환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">날짜/시간 포멧</param>
        /// <returns>DateTime String</returns>
        public static string ToDateTimeStringDefaultFormatAEx(this DateTime value, string dateFormat = "yyyy-MM-dd HHmmss")
        {
            //string dateFormat = "yyyy-MM-dd HH:mm:ss";
            return HxString.GetString(value, dateFormat);
        }
        /// <summary>
        /// [Extension] DateTime을 지정된 날짜포멧의 문자열로 변환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">날짜/시간 포멧</param>
        /// <returns>DateTime String</returns>
        public static string ToDateTimeStringDefaultFormatBEx(this DateTime value, string dateFormat = "yyyyMMdd_HHmmss")
        {
            //string dateFormat = "yyyy-MM-dd HH:mm:ss";
            return HxString.GetString(value, dateFormat);
        }
        /// <summary>
        /// [Extension] DateTime을 지정된 날짜포멧의 문자열로 변환
        /// </summary>
        /// <param name="value">DateTime Value</param>
        /// <param name="dateFormat">날짜/시간 포멧</param>
        /// <returns>DateTime String</returns>
        public static string ToDateTimeStringDefaultFormatCEx(this DateTime value, string dateFormat = "yyyyMMdd HHmmss")
        {
            //string dateFormat = "yyyy-MM-dd HH:mm:ss";
            return HxString.GetString(value, dateFormat);
        }

        public static string ToDateShortStringEx(this DateTime value)
        {
            string dateFormat = "yy.MM.dd";
            return HxString.GetString(value, dateFormat);
        }
        public static string ToDateStringEx(this DateTime value)
        {
            string dateFormat = "yyyy-MM-dd";
            return HxString.GetString(value, dateFormat);
        }

        /// <summary>
        /// [Extension] 정규식을 이용한 문자열 치환
        /// </summary>
        /// <param name="sender">입력 문자열</param>
        /// <param name="pattern">정규식 패턴</param>
        /// <param name="replacement">치환 문자열</param>
        /// <param name="options">정규식 옵션</param>
        /// <returns>치환된 문자열</returns>
        public static string RegexReplaceEx(this string sender, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            return HxString.RegexReplace(sender, pattern, replacement, options);
        }
        /// <summary>
        /// [Extension] 정규식을 이용한 문자 패턴 일치 여부
        /// </summary>
        /// <param name="sender">입력 문자열</param>
        /// <param name="pattern">정규식 패턴</param>
        /// <param name="options">정규식 옵션</param>
        /// <returns>일치 여부</returns>
        public static bool IsRegexMatchEx(this string sender, string pattern, RegexOptions options = RegexOptions.None)
        {
            return HxString.IsRegexMatch(sender, pattern, options);
        }
        /// <summary>
        /// [Extension] 정규식을 이용한 문자 패턴 일치 Match
        /// </summary>
        /// <param name="sender">입력 문자열</param>
        /// <param name="pattern">정규식 패턴</param>
        /// <param name="options">정규식 옵션</param>
        /// <returns>일치하는 Match</returns>
        public static Match RegexMatchEx(this string sender, string pattern, RegexOptions options = RegexOptions.None)
        {
            return HxString.RegexMatch(sender, pattern, options);
        }
        /// <summary>
        /// [Extension] 정규식을 이용한 문자 패턴 일치 Matches
        /// </summary>
        /// <param name="sender">입력 문자열</param>
        /// <param name="pattern">정규식 패턴</param>
        /// <param name="options">정규식 옵션</param>
        /// <returns>일치하는 Matches</returns>
        public static MatchCollection RegexMatchesEx(this string sender, string pattern, RegexOptions options = RegexOptions.None)
        {
            return HxString.RegexMatches(sender, pattern, options);
        }
        public static int FindCharCountEx(this string sender, string search)
        {
            return HxString.FindCharCount(sender, search);
        }

        /// <summary>
        /// [Extension] 구분 문자열로 자르기
        /// </summary>
        /// <param name="sender">문자열</param>
        /// <param name="separator">구분자</param>
        /// <param name="option">Split 옵션 (None, RemoveEmptyEntries)</param>
        /// <returns>배열</returns>
        public static string[] SplitEx(this string sender, string separator, StringSplitOptions option = StringSplitOptions.None)
        {
            return HxString.SplitEx(sender, separator, option);
        }
        public static string[] SplitCharEx(this string sender, char[] separator = null, StringSplitOptions option = StringSplitOptions.None, bool isSeparatorNullToDefaultSpecialCharacters = true)
        {
            return HxString.SplitCharEx(sender, separator, option, isSeparatorNullToDefaultSpecialCharacters);
        }
        /// <summary>
        /// [Extension] 구분 문자열로 자르기
        /// </summary>
        /// <param name="sender">문자열</param>
        /// <param name="separator">구분자</param>
        /// <param name="bOverwrite">중복 처리 방법 (True : 덮어쓰기, False : 건너뛰기)</param>
        /// <returns>List Resource</returns>
        public static List<string> SplitToListEx(this string sender, string separator, bool bOverwrite = false)
        {
            return HxString.SplitToListEx(sender, separator, bOverwrite);
        }

        public static List<T> SplitToListEx<T>(this string sender, string separator, bool bOverwrite = false)
        {
            return HxString.SplitToListEx<T>(sender, separator, bOverwrite);
        }

        public static string[] SplitToArrayEx(this string sender, string pattern)
        {
            return HxString.SplitToArray(sender, pattern);
        }
        public static string[] SplitToArrayEx(this string sender)
        {
            return HxString.SplitToLineArray(sender);
        }
        public static List<string> SplitToLineListEx(this string sender)
        {
            return HxString.SplitToLineList(sender);
        }

        /// <summary>
        /// [Extension] System.Drawing.Point에 상대 Point 더하기
        /// </summary>
        /// <param name="sender">기준이 될 Point</param>
        /// <param name="addPoint">덧셈 할 Point</param>
        /// <returns>덧셈이 된 상대 Point</returns>
        public static System.Drawing.Point AddEx(this System.Drawing.Point sender, System.Drawing.Point addPoint)
        {
            return HxUtils.GetDrawingPointAdd(sender, addPoint);
        }
        /// <summary>
        /// [Extension] System.Drawing.Point에 상대 Point 더하기
        /// </summary>
        /// <param name="sender">기준이 될 Point</param>
        /// <param name="width">덧셈 할 폭(X) 값</param>
        /// <param name="height">덧셈 할 폭(Y) 값</param>
        /// <returns>덧셈이 된 상대 Point</returns>
        public static System.Drawing.Point AddEx(this System.Drawing.Point sender, int width, int height)
        {
            return HxUtils.GetDrawingPointAdd(sender, width, height);
        }
        /// <summary>
        /// [Extension] List Value Append
        /// </summary>
        /// <typeparam name="T">입력 타입</typeparam>
        /// <param name="sender">List Resource</param>
        /// <param name="value">입력 값</param>
        /// <param name="bOverwrite">중복 처리 방법 (True : 덮어쓰기, False : 건너뛰기)</param>
        /// <returns>처리 결과</returns>
        public static bool AddEx<T>(this List<T> sender, T value, bool bOverwrite = false)
        {
            return HxEnumerable.Add<T>(sender, value, bOverwrite);
        }
        /// <summary>
        /// [Extension] LIST Value Modify
        /// </summary>
        /// <typeparam name="T">입력 타입</typeparam>
        /// <param name="sender">List Resource</param>
        /// <param name="value">입력 값</param>
        /// <param name="bNotExistCaseAdd">기존 Value가 없는 경우(true : 추가하기)</param>
        /// <returns></returns>
        public static bool ModEx<T>(this List<T> sender, T value, bool bNotExistCaseAdd = false)
        {
            return HxEnumerable.Mod<T>(sender, value, bNotExistCaseAdd);
        }
        /// <summary>
        /// [Extension] List Value Clear
        /// </summary>
        /// <typeparam name="T">입력 타입</typeparam>
        /// <param name="sender">List Resource</param>
        [Obsolete("필요 없어보임!")]
        private static void ClearEx<T>(this List<T> sender)
        {
            if (sender != null)
            {
                sender.Clear();
            }
        }
        /// <summary>
        /// [Extension] IEnumerable Value Append
        /// </summary>
        /// <typeparam name="T">입력 타입</typeparam>
        /// <param name="sender">List Resource</param>
        /// <param name="value">입력 값</param>
        /// <param name="bNotExistCaseAdd">기존 Value가 없는 경우(true : 추가하기)</param>
        /// <returns></returns>
        public static IEnumerable<T> AddEx<T>(this IEnumerable<T> sender, T value, bool bNotExistCaseAdd = false)
        {
            bool bAddAction = true;
            if(sender != null && sender.Count() > 0)
            {
                int e = sender.Where(r => r.Equals(value)).Count();
                if(e > 0 && bNotExistCaseAdd != true)
                {
                    bAddAction = false;
                }
            }
            if (bAddAction == true)
            {
                foreach (var cur in sender)
                {
                    yield return cur;
                }
                yield return value;
            }
        }


        public static Dictionary<TKey, TVal> CopyEx<TKey, TVal>(this Dictionary<TKey, TVal> sender)
        {
            Dictionary<TKey, TVal> Result = null;
            if (sender != null)
            {
                Result = new Dictionary<TKey, TVal>();
                foreach (KeyValuePair<TKey, TVal> pair in sender)
                {
                    Result.AddEx(pair.Key, pair.Value);
                }
            }
            return Result;
        }

        /// <summary>
        /// [Extension] Dictionary에 값 추가
        /// </summary>
        /// <typeparam name="TKey">KEY Type</typeparam>
        /// <typeparam name="TVal">VALUE Type</typeparam>
        /// <param name="sender">Resource</param>
        /// <param name="key">KEY Name</param>
        /// <param name="value">VALUE</param>
        /// <param name="bOverwrite">겹쳐쓰기(Override)</param>
        /// <returns>성공 여부?</returns>
        public static bool AddEx<TKey, TVal>(this Dictionary<TKey, TVal> sender, TKey key, TVal value, bool bOverwrite = false)
        {
            return HxEnumerable.Add<TKey, TVal>(sender, key, value, bOverwrite);
        }
        /// <summary>
        /// [Extension] Dictionary에 값 추가
        /// </summary>
        /// <param name="sender">Resource</param>
        /// <param name="key">KEY Name</param>
        /// <param name="value">VALUE</param>
        /// <param name="bOverwrite">겹쳐쓰기(Override)</param>
        /// <returns>성공 여부?</returns>
        public static bool AddDbEx(this Dictionary<string, object> sender, string key, object value, bool bOverwrite = false)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }
            return HxEnumerable.Add<string, object>(sender, key, value, bOverwrite);
        }

        
        /// <summary>
        /// [Extension] Dictionary에 값 수정
        /// </summary>
        /// <typeparam name="TKey">KEY Type</typeparam>
        /// <typeparam name="TVal">VALUE Type</typeparam>
        /// <param name="sender">Resource</param>
        /// <param name="key">KEY Name</param>
        /// <param name="value">VALUE</param>
        /// <param name="bNotExistCaseAdd">KEY가 존재하지 않을 경우 추가 여부?</param>
        /// <returns>성공 여부?</returns>
        public static bool ModEx<TKey, TVal>(this Dictionary<TKey, TVal> sender, TKey key, TVal value, bool bNotExistCaseAdd = false)
        {
            return HxEnumerable.Mod<TKey, TVal>(sender, key, value, bNotExistCaseAdd);
        }
        public static bool MergeEx<TKey, TVal>(this Dictionary<TKey, TVal> sender, Dictionary<TKey, TVal> keyValuePairs, bool bNotExistCaseAdd = false)
        {
            if (keyValuePairs == null || keyValuePairs.Count <= 0)
                return false;

            if (sender == null || sender.Count <= 0)
            {
                sender = keyValuePairs;
            }
            else
            {
                if(sender == null)
                {
                    sender = new Dictionary<TKey, TVal>();
                }
                foreach(var item in keyValuePairs)
                {
                    sender.AddEx(item.Key, item.Value, bNotExistCaseAdd);
                }
            }
            return (sender != null && sender.Count > 0) ? true : false;
        }
        /// <summary>
        /// [Extension] Dictionary Clear
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sender"></param>
        [Obsolete("이전 버전용이 었으나, 현재는 불 필요함")]
        private static void ClearEx<TKey, TValue>(this Dictionary<TKey, TValue> sender)
        {
            if (sender != null)
            {
                sender?.Clear();
            }
        }

        /// <summary>
        /// [Extension] Dictionary의 값 가져오기 (Object)
        /// </summary>
        /// <param name="sender">Resource</param>
        /// <param name="key">KEY Name</param>
        /// <param name="IgnoreCase">Original KEY로 값이 없는 경우 소문자로 확인</param>
        /// <returns>값</returns>
        public static object GetValueEx(this Dictionary<string, object> sender, string key, bool IgnoreCase = true)
        {
            if (sender != null && sender.Count > 0 && key.IsNullOrWhiteSpaceEx() != true)
            {
                if (sender.ContainsKey(key))
                {
                    return sender[key];
                }
                else if (IgnoreCase == true)
                {
                    string lowerKey = key.ToLower();
                    if (sender.ContainsKey(lowerKey))
                    {
                        return sender[lowerKey];
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// [Extension] Dictionary의 값 가져오기 (Object To String)
        /// </summary>
        /// <param name="sender">Resource</param>
        /// <param name="key">KEY Name</param>
        /// <param name="IgnoreCase">Original KEY로 값이 없는 경우 소문자로 확인</param>
        /// <returns>값</returns>
        public static string GetValueToStringEx(this Dictionary<string, object> sender, string key, bool IgnoreCase = true)
        {
            return sender?.GetValueEx(key, IgnoreCase)?.ToStringEx();
        }
        /// <summary>
        /// [Extension] Dictionary의 값 가져오기 (Object To Nullable-Decimal)
        /// </summary>
        /// <param name="sender">Resource</param>
        /// <param name="key">KEY Name</param>
        /// <param name="IgnoreCase">Original KEY로 값이 없는 경우 소문자로 확인</param>
        /// <returns>값</returns>
        public static decimal? GetValueToNullableDecimalEx(this Dictionary<string, object> sender, string key, bool IgnoreCase = true)
        {
            return sender?.GetValueEx(key, IgnoreCase)?.ToNullableDecimalEx();
        }
        /// <summary>
        /// [Extension] Dictionary의 값 가져오기 (Object To Nullable-Boolean)
        /// </summary>
        /// <param name="sender">Resource</param>
        /// <param name="key">KEY Name</param>
        /// <param name="IgnoreCase">Original KEY로 값이 없는 경우 소문자로 확인</param>
        /// <returns>값</returns>
        public static bool? GetValueToNullableBoolEx(this Dictionary<string, object> sender, string key, bool IgnoreCase = true)
        {
            return sender?.GetValueEx(key, IgnoreCase)?.ToBoolEx();
        }

        /// <summary>
        /// [Extension] DataTable TO Json
        /// </summary>
        /// <param name="sender">DataTable Resource</param>
        /// <param name="bSerializeObject">SerializeObject 사용 여부</param>
        /// <returns></returns>
        public static string ToJsonStringEx(this DataTable sender, bool bSerializeObject = true)
        {
            return HxUtils.ConvertDatatableToJsonString(sender, bSerializeObject);
        }
        [Obsolete("더 이상 사용하지 마세요.")]
        public static string ToJsonStringEx(this object sender, bool bSerializeObject)
        {
            //return HxUtils.ConvertSerializeObjectToJsonString(sender);
            return HxUtils.JsonSerializeObject(sender);
        }
        public static string ToJsonStringEx(this object sender)
        {
            //return HxUtils.ConvertSerializeObjectToJsonString(sender);
            return HxUtils.JsonSerializeObject(sender);
        }
        public static string ToJsonStringWithNameingCaseEx(this object sender, HxNameingCaseType caseType)
        {
            //return HxUtils.ConvertSerializeObjectToJsonString(sender);
            return HxUtils.JsonSerializeWithNameingCase(sender, caseType);
        }
        
        /// <summary>
        /// [Extension] DataTable To JSON JArray
        /// </summary>
        /// <param name="sender">DataTable Resource</param>
        /// <param name="bSerializeObject">SerializeObject 여부</param>
        /// <returns>JSON JArray</returns>
        public static JArray ToJArrayEx(this DataTable sender, bool bSerializeObject = true)
        {
            if (sender != null && sender.Rows.Count > 0)
            {
                return HxUtils.ConvertDatatableToJArray(sender, bSerializeObject);
            }
            return null;
        }
        public static JToken ToJTokenEx(this DataTable sender, bool bSerializeObject = true)
        {
            if (sender != null && sender.Columns.Count > 0)
            {
                try
                {
                    string json = HxUtils.ConvertDatatableToJsonString(sender, bSerializeObject);
                    return JToken.Parse(json);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw;
                }
                
            }
            return null;
        }
        public static JObject ToJObjectEx(this DataTable sender, bool bSerializeObject = true)
        {
            if (sender != null && sender.Columns.Count > 0)
            {
                try
                {
                    string json = HxUtils.ConvertDatatableToJsonString(sender, bSerializeObject);
                    return JObject.Parse(json);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw;
                }

            }
            return null;
        }
        public static JToken ToJsonTokenEx(this DataRow sender, bool bSerializeObject = true)
        {
            if (sender != null && sender.Table.Columns.Count > 0)
            {
                DataTable dt = sender.Table.Clone();
                dt.ImportRow(sender);
                JArray json = HxUtils.ConvertDatatableToJArray(dt, bSerializeObject);
                return json?[0];
            }
            return null;
        }

        /// <summary>
        /// [Extension] DataSet에서 이름으로 찾은 DataTable 가져오기
        /// </summary>
        /// <param name="sender">DataSet Resource</param>
        /// <param name="name">DataTable Name</param>
        /// <returns>Find DataTable</returns>
        public static DataTable GetDataTableEx(this DataSet sender, string name)
        {
            DataTable Result = null;
            if(sender != null && sender.Tables.Count > 0 && sender.Tables.Contains(name))
            {
                Result = sender.Tables[name];
            }
            return Result;
        }
        /// <summary>
        /// [Extension] DataSet에서 Index로 찾은 DataTable 가져오기
        /// </summary>
        /// <param name="sender">DataSet Resource</param>
        /// <param name="index">DataTable Index</param>
        /// <returns>Find DataTable</returns>
        public static DataTable GetDataTableEx(this DataSet sender, int index)
        {
            DataTable Result = null;
            if (sender != null && sender.Tables.Count >= index)
            {
                Result = sender.Tables[index];
            }
            return Result;
        }
        /// <summary>
        /// [Extension] DataTable Merge
        /// </summary>
        /// <param name="sender">원본 DataTable</param>
        /// <param name="copyDataTable">복사할 DataTable</param>
        /// <returns>작업 여부?</returns>
        public static bool MergeDataTableEx(this DataTable sender, DataTable copyDataTable)
        {
            return HxUtils.MergeDataTable(sender, copyDataTable);
        }

        /// <summary>
        /// [Extension] DataTable.DataRowCollection To DataRow Array
        /// </summary>
        /// <param name="data">Source DataTable</param>
        /// <returns>DataRow Array</returns>
        public static DataRow[] ToDataRowArrayEx(this DataTable data)
        {
            DataRow[] Result = null;
            Result = HxConvert.ConvertToDataRowArray(data);
            return Result;
        }

        public static DataRow[] ToDataRowArrayEx(this DataRowCollection rows)
        {
            DataRow[] Result = null;
            Result = HxConvert.ConvertToDataRowArray(rows);
            return Result;
        }

        /// <summary>
        /// [Extension] DataTable의 Property  확장 속성
        /// </summary>
        /// <param name="sender">DataTable Resource</param>
        /// <param name="property_name">KEY</param>
        /// <param name="value">VALUE</param>
        /// <param name="bOverWrite">겹쳐쓰기(Override)</param>
        public static void SetExtendedPropertiesEx(this DataTable sender, string property_name, object value, bool bOverWrite = true)
        {
            HxUtils.DoExtendedPropertiesAdd(sender.ExtendedProperties, property_name, value, bOverWrite);
        }
        /// <summary>
        /// [Extension] DataTable(with key / value COLUMNS)인 경우 최종값 하나만 가져오기
        /// </summary>
        /// <typeparam name="T">Retrun Type</typeparam>
        /// <param name="sender">DataTable Resource</param>
        /// <param name="name">Key name</param>
        /// <param name="column">Value Column-name</param>
        /// <returns>VALUE</returns>
        public static T GetSingleLastValueEx<T>(this DataTable sender, string name, string column = "value")
        {
            T Result;
            try
            {
                Result = HxUtils.SingleLastValue<T>(sender, name, column);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public static T GetValueEx<T>(this DataTable sender, string name, string column = "value")
        {
            T Result;
            try
            {
                Result = HxUtils.SingleLastValue<T>(sender, name, column);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        /// <summary>
        /// [Extension] Property  확장 속성
        /// </summary>
        /// <param name="sender">Source Resource</param>
        /// <param name="property_name">KEY</param>
        /// <param name="value">VALUE</param>
        /// <param name="bOverWrite">겹쳐쓰기(Override)</param>
        public static void SetExtendedPropertiesEx(this DataColumn sender, string property_name, object value, bool bOverWrite = true)
        {
            HxUtils.DoExtendedPropertiesAdd(sender.ExtendedProperties, property_name, value, bOverWrite);
        }
        //public static void SetPropertiesEx(this PropertyCollection sender, string property_name, object value, bool bNotValueIsRemove = false)
        //{

        //    if (!sender.ContainsKey(property_name) && (bNotValueIsRemove != true || (bNotValueIsRemove == true && value != null)) )
        //    {
        //        sender.Add(property_name, value);
        //    }
        //    else
        //    {
        //        if (bNotValueIsRemove == true && value == null)
        //        {
        //            sender.Remove(property_name);
        //        }
        //        else
        //        {
        //            sender[property_name] = value;
        //        }

        //    }
        //}

        public static T[] ToSingleArrayEx<T>(this T sender)
        {
            T[] Result = new T[1];
            Result[0] = sender;
            return Result;
        }

        /// <summary>
        /// [Extension] Array To List
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="sender">Array Object</param>
        /// <returns>List</returns>
        public static List<T> ToListEx<T>(this T[] sender)
        {
            return HxString.GetList<T>(sender);
        }
        /// <summary>
        /// [Extension] List to Array
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="sender">List Resource</param>
        /// <returns>Array</returns>
        public static T[] ToArrayEx<T>(this List<T> sender)
        {
            return HxString.GetArray<T>(sender);
        }
        //==================
        #region Struct(Record) - DataTable
        /// <summary>
        /// DataTable을 Struct Array(RecordSet)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="sender">Source DataTable</param>
        /// <returns>Multiple Struct Array(RecordSet)</returns>
        public static T[] ToRecordSetEx<T>(this DataTable sender)
            where T : IHxSetValue, new()
        {
            return HxUtils.ConvertDataTableToRecordSet<T>(sender);
        }
        /// <summary>
        /// DataTable의 특정Index를 Struct(Record)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="sender">Source DataTable</param>
        /// <param name="index">Index</param>
        /// <returns>Single Struct(Record)</returns>
        public static T ToRecordEx<T>(this DataTable sender, int index = 0)
            where T : IHxSetValue, new()
        {
            return HxUtils.ConvertDataTableToRecord<T>(sender, index);
        }
        /// <summary>
        /// DataTable의 특정Index를 Struct(Record) Nullable Type으로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="sender">Source DataTable</param>
        /// <param name="index">Index</param>
        /// <returns>Single Struct(Record) : Nullable Type</returns>
        public static T ToNullableRecordEx<T>(this DataTable sender, int index = 0)
            where T : IHxSetValue, new()
        {
            return HxUtils.ConvertDataTableToNullableRecord<T>(sender, index);
        }
        /// <summary>
        /// DataRow를 Struct(Record)로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="sender">Sorce DataRow</param>
        /// <returns>Single Struct(Record)</returns>
        public static T ToRecordEx<T>(this DataRow sender)
            where T : IHxSetValue, new()
        {
            return HxUtils.ConvertDataRowToRecord<T>(sender);
        }
        /// <summary>
        /// DataRow를 Struct(Record) Nullable Type으로 변경
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="sender">Sorce DataRow</param>
        /// <returns>Single Struct(Record) : Nullable</returns>
        public static T? ToNullableRecordEx<T>(this DataRow sender)
            where T : struct, IHxSetValue
        {
            return HxUtils.ConvertDataRowToNullableRecord<T>(sender);
        }
        
        public static DataTable ToDataTableEx<T>(this IEnumerable<T> sender, string tableName = null, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue, new()
        {
            return HxConvert.ConvertRecordSetToDataTable<T>(sender, tableName, HxDbColumnNameCharType.Lower);
        }
        public static DataTable ToDataTableEx<T>(this IList<T> sender)
        {
            return HxConvert.ToDataTable<T>(sender);
        }

        /// <summary>
        /// Struct(Record)의 Properties 타입을 DataTable로 구조로 변경(Single DataRow 옵션)
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="recordSet">Source : Struct Array</param>
        /// <param name="insertRow">값 포함 여부</param>
        /// <param name="tableName">TableName</param>
        /// <returns>DataTable (With Single Data Option)</returns>
        public static DataTable ToPropertiesDataTableEx<T>(this T[] sender, bool insertRow = true, string tableName = null)
            where T : struct
        {
            return HxUtils.ConvertStructPropertiesToDataTable(sender, insertRow, tableName);
        }

        /// <summary>
        /// Struct(Record)의 Properties 타입을 DataTable로 구조로 변경(Single DataRow 옵션)
        /// </summary>
        /// <typeparam name="T">Struct Type</typeparam>
        /// <param name="recordSet">Source : Struct Array</param>
        /// <param name="insertRow">값 포함 여부</param>
        /// <param name="tableName">TableName</param>
        /// <returns>DataTable (With Single Data Option)</returns>
        public static DataTable ToPropertiesDataTableEx<T>(this T sender, bool insertRow = true, string tableName = null)
            where T : struct
        {
            return HxUtils.ConvertStructPropertiesToDataTable(sender, insertRow, tableName);
        }


        

        /*
        /// <summary>
        /// DataRow의 특정 필드 값(Struct) 가져오기
        /// </summary>
        /// <typeparam name="T">Struct Resource</typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="field">필드 명</param>
        /// <returns>Struct</returns>
        public static T? GetValue<T>(this DataRow row, string field) where T : struct
        {
            if (row.IsNull(field))
                return new T?();
            else
                return (T?)row[field];
        }
        /// <summary>
        /// DataRow의 특정 필드 값(Class) 가져오기
        /// </summary>
        /// <typeparam name="T">Class Resource</typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="field">필드 명</param>
        /// <returns>Class</returns>
        public static T GetReference<T>(this DataRow row, string field) where T : class
        {
            if (row.IsNull(field))
                return default(T);
            else
                return (T)row[field];
        }
        */
        #endregion

        
        public static IEnumerable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftKey, Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> result)
        {
            return left.GroupJoin(right, leftKey, rightKey, (l, r) => new { l, r })
                    .SelectMany(
                        o => o.r.DefaultIfEmpty(),
                        (l, r) => new { lft = l.l, rght = r })
                    .Select(o => result.Invoke(o.lft, o.rght));
        }

        public static string ToStringEx(this HxResultValue sender, bool IsNullConvert = true)
        {
            return HxUtils.ResultValueToString(sender, IsNullConvert);
        }
        public static DataTable ToDataTableEx<T>(IEnumerable<T> collection)
        {
            DataTable dt = new DataTable();

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                Type columnType = prop.PropertyType;
                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = Nullable.GetUnderlyingType(columnType);
                }
                dt.Columns.Add(prop.Name, columnType);
            }

            foreach (T item in collection)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
        public static DataTable ToDataTable(this HxResultValue sender, string tableName = null)
        {
            return HxConvert.ConvertToDataTable(sender, tableName);
        }

        public static DataTable ToDataTable<T>(this HxResultValue sender, string tableName = null, HxDbColumnNameCharType colNameCharType = HxDbColumnNameCharType.Original)
            where T : IHxSetValue
        {
            return HxConvert.ConvertToDataTable<T>(sender, tableName, colNameCharType);
        }

        //public static HxDictionary<TKey, TValue> CopyEx<TKey, TValue>(this HxDictionary<TKey, TValue> sender)
        //{
        //    return HxDictionary<TKey, TValue>.Copy(sender);
        //}
        //public static HxDictionaryStringValue CopyEx(this HxDictionaryStringValue sender)
        //{
        //    return HxDictionaryStringValue.Copy(sender);
        //}

        public static T ToRecordEx<T>(this Dictionary<string, object> post)
            where T : HxDbModelSetValue, new()
        {
            T Result = null;
            if (post != null && post.Count > 0)
            {
                Result = new T();
                foreach (KeyValuePair<string, object> o in post)
                {
                    Result.SetMatchFieldValue(o.Key, o.Value);
                }
            }
            return Result;
        }

        public static T DeepCopyStructEx<T>(this T value)
            where T : struct
        {
            return value;
        }
        public static T DeepCopyClassEx<T>(this T obj)
            where T : class
        {
            return HxUtils.DeepCopy<T>(obj);
        }
        public static T DeepCopyEx<T>(this T obj)
            where T : class
        {
            return obj.DeepCopyClassEx();
        }

        public static string ToEnumMemberValueStringEx(this Enum sender)
        {
            //출처 : https://stackoverflow.com/questions/27372816/how-to-read-the-value-for-an-enummember-attribute
            var attr =
                sender.GetType().GetMember(sender.ToString()).FirstOrDefault()?.
                    GetCustomAttributes(false).OfType<System.Runtime.Serialization.EnumMemberAttribute>().
                    FirstOrDefault();
                if (attr == null)
                    return sender.ToString();
            return attr.Value;
        }
        public static string ToDescriptionAttributeValueStringEx(this Enum sender)
        {

            string Result = sender.ToString();

            Result = HxEnumHelper.GetDescription(sender);
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                Result = sender.GetAttributeEx<System.ComponentModel.DescriptionAttribute>().ToStringEx();
            }
            if (Result.IsNullOrWhiteSpaceEx() == true)
            {
                Result = sender.ToString();
            }
            return Result;
        }
        public static TAttribute GetAttributeEx<TAttribute>(this Enum sender)
            where TAttribute : Attribute
        {
            return HxAttribute.GetAttribute<TAttribute>(sender);
        }

        public static string GetDescriptionEx(this Enum sender, bool bNullIsGetName = false)
        {
            string Result = HxEnum.GetDescriptionAttr(sender);
            if(Result.IsNullOrWhiteSpaceEx() == true && bNullIsGetName == true)
            {
                Result = HxEnum.GetEnumName(sender);
            }
            return Result;
        }

        /// <summary>
        /// Enum Type의 Name을 String으로 변환
        /// </summary>
        /// <param name="sender">Enum Type</param>
        /// <returns></returns>
        public static string GetNameEx(this Enum sender)
        {
            //return Enum.GetName(sender.GetType(), sender);
            return HxEnum.GetEnumName(sender);
        }
        public static int GetValueEx<TEnumType>(this TEnumType value)
        {
            //return Enum.GetName(sender.GetType(), sender);
            return (int)Enum.ToObject(value.GetType(), value);
        }
        public static int GetValueEx(this Enum value)
        {
            //return Enum.GetName(sender.GetType(), sender);
            return (int)Enum.ToObject(value.GetType(), value);
        }

        public static TEnumType ConverToEnumEx<TEnumType>(this String input)
        {
            return HxEnum.ConverToEnum<TEnumType>(input);
        }
        public static Dictionary<string, TValue> GetDictionaryEx<T, TValue>(this T sender)
            where T : class, new()
        {
            return HxConvert.GetDictionary<T, TValue>(sender);
        }

        public static Dictionary<string, object> GetDictionaryValueObjectEx<T>(this T sender)
            where T : class, new()
        {
            return HxConvert.GetDictionaryValueObject<T>(sender);
        }

        public static string GetFileSystemSafeNameEx(this string s)
        {
            //참고 : https://stackoverflow.com/questions/333175/is-there-a-way-of-making-strings-file-path-safe-in-c
            //출처 : https://stackoverflow.com/a/16083025
            return HxString.GetFileSystemSafeName(s);
        }

        #region Compare / Diff.
        //출처 : https://stackoverflow.com/questions/10454519/best-way-to-compare-two-complex-objects
        //또는 참조 필요 : https://github.com/GregFinzer/Compare-Net-Objects
        public static bool DeepCompareEx(this object sender, object another)
        {
            return HxUtils.DeepCompare(sender, another);
        }
        public static bool CompareEx(this object sender, object another)
        {
            return HxUtils.Compare(sender, another);
        }
        public static bool JsonCompareEx(this object sender, object another)
        {
            return HxUtils.JsonCompare(sender, another);
        }
        #endregion

        public static string PadLeftEx(this string input, int totalWidth, char paddingChar = ' ', bool isInputWidthBigApply = false)
        {
            return HxString.PadLeft(input, totalWidth, paddingChar, isInputWidthBigApply);
        }
        public static string PadRightEx(this string input, int totalWidth, char paddingChar = ' ', bool isInputWidthBigApply = false)
        {
            return HxString.PadRight(input, totalWidth, paddingChar, isInputWidthBigApply);
        }
    }
}
