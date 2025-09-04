using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HxCore;


namespace HxCore.Web
{
    public static class HxWebExtension
    {
        public static List<string> SplitCsvEx(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
        public static List<string> ToListEx(this Microsoft.Extensions.Primitives.StringValues sender)
        {
            List<string> Result = null;
            if(sender.Count > 0)
            {
                Result = new List<string>();
                string[] arr = sender.ToStringSingleEx().Split(',');
                for (int i = 0; i < arr.Length; i++) {
                    Result.Add(arr[i].Trim());
                }
            }
            return Result;
        }

        public static string ToStringSingleEx(this Microsoft.Extensions.Primitives.StringValues sender, HxMultiplePosition position = HxMultiplePosition.None)
        {
            if (sender.Count > 0)
            {
                switch (position)
                {
                    case HxMultiplePosition.First:
                        return sender.ToFirstOrDefaultEx();
                    case HxMultiplePosition.Last:
                        return sender.ToLastOrDefaultEx();
                    default:
                        return sender.ToStringEx();
                }
            }
            return null;
        }

        public static string ToLastOrDefaultEx(this Microsoft.Extensions.Primitives.StringValues sender)
        {
            if (sender.Count > 0)
            {
                return sender.ToStringEx().Split(',').LastOrDefault().Trim();
            }
            return null;
        }

        public static string ToFirstOrDefaultEx(this Microsoft.Extensions.Primitives.StringValues sender)
        {
            if (sender.Count > 0)
            {
                return sender.ToStringEx().Split(',').FirstOrDefault().Trim();
            }
            return null;
        }
    }
}
