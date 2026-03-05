using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    public class HxEnumerable
    {
        #region "Dictionary<string, object> 처리 메소드"
        /// <summary>
        /// Dictionary 타입 - 추가
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="bOverwrite">기존 Key가 존재할 경우 / true : 덮어쓰기,겹쳐쓰기(Last Value), false : 건너띄기(First Value)</param>
        /// <returns>추가 여부</returns>
        public static bool Add<TKey, TVal>(Dictionary<TKey, TVal> sender, TKey key, TVal value, bool bOverwrite = false)
        {
            //return SetListCaptionAdd(Obj, key, value, isExistCassModify);
            bool Result = false;
            if (sender != null)
            {
                if (!sender.ContainsKey(key))
                {
                    sender.Add(key, value);
                    Result = true;
                }
                else if (sender.ContainsKey(key) && bOverwrite == true)
                {
                    sender[key] = value;
                    Result = true;
                }
            }
            return Result;
        }

        /// <summary>
        /// Dictionary 타입 - 수정
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="bNotExistCaseAdd">기존 Key가 없는 경우</param>
        /// <returns>수정 여부</returns>
        public static bool Mod<TKey, TVal>(Dictionary<TKey, TVal> sender, TKey key, TVal value, bool bNotExistCaseAdd = false)
        {
            //return SetListCaptionMod(Obj, key, value, isNotExistCassAdd);
            bool Result = false;
            if (sender.ContainsKey(key))
            {
                sender[key] = value;
                Result = true;
            }
            else if (!sender.ContainsKey(key) && bNotExistCaseAdd == true)
            {
                sender.Add(key, value);
                Result = true;
            }
            return Result;
        }

        /// <summary>
        /// Dictionary 타입 - 복사 / From
        /// </summary>
        /// <param name="sender">대상 Object</param>
        /// <param name="sourceList">원본 Object</param>
        /// <returns>복사 성공 여부</returns>
        public static bool CopyFrom<TKey, TVal>(Dictionary<TKey, TVal> sender, Dictionary<TKey, TVal> sourceList)
        {
            bool Result = false;
            try
            {
                if (sender == null)
                {
                    sender = new Dictionary<TKey, TVal>();
                }
                foreach (KeyValuePair<TKey, TVal> list in sourceList)
                {
                    //SetListCaptionAdd(target_list, list.Key, list.Value, true);
                    //sender.AddEx(list.Key, list.Value, true);
                    Add(sender, list.Key, list.Value, true);
                }
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                throw ex;
            }
            return Result;
        }

        /// <summary>
        /// Dictionary 타입 - 복사 / To
        /// </summary>
        /// <param name="sender">원본 Object</param>
        /// <param name="targetList">대상 Object</param>
        /// <returns>복사 성공 여부</returns>
        public static bool CopyTo<TKey, TVal>(Dictionary<TKey, TVal> sender, Dictionary<TKey, TVal> targetList)
        {
            /* 
             * dnCore.dnFunDictionary에서 처리
             * 
            return ATargetList.CopyFromEx(Sender);
             * */
            return CopyFrom(targetList, sender);
        }

        /// <summary>
        /// Dictionary 타입 - 추가
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sender">Object</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="bExistCaseModify">기존 Key가 존재할 경우 / true : 덮어쓰기(Last Value), false : 건너띄기(First Value)</param>
        /// <returns>추가 여부</returns>
        public static bool Add<T>(Dictionary<string, T> sender, string key, T value, bool bExistCaseModify = false)
        {
            bool Result = false;
            if (!sender.ContainsKey(key))
            {
                sender.Add(key, value);
                Result = true;
            }
            else if (sender.ContainsKey(key) && bExistCaseModify == true)
            {
                sender[key] = value;
                Result = true;
            }
            return Result;
        }

        /// <summary>
        /// Dictionary 타입 - 수정
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sender">Object</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="bNotExistCaseAdd">기존 Key가 없는 경우(true : 추가하기, false : 건너띄기)</param>
        /// <returns>수정 여부</returns>
        public static bool Mod<T>(Dictionary<string, T> sender, string key, T value, bool bNotExistCaseAdd = false)
        {
            //return SetListCaptionMod(Obj, key, value, isNotExistCassAdd);
            bool Result = false;
            if (sender.ContainsKey(key))
            {
                sender[key] = value;
                Result = true;
            }
            else if (!sender.ContainsKey(key) && bNotExistCaseAdd == true)
            {
                sender.Add(key, value);
                Result = true;
            }
            return Result;
        }

        

        /// <summary>
        /// Dictionary 타입 - 복사 / From
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sender">대상 Object</param>
        /// <param name="sourceList">원본 Object</param>
        /// <returns>복사 성공 여부</returns>
        public static bool CopyFrom<T>(Dictionary<string, T> sender, Dictionary<string, T> sourceList)
        {
            bool Result = false;
            try
            {
                foreach (KeyValuePair<string, T> list in sourceList)
                {
                    //SetListCaptionAdd(target_list, list.Key, list.Value, true);
                    //sender.AddEx(list.Key, list.Value, true);
                    Add(sender, list.Key, list.Value, true);
                }
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                throw ex;
            }
            return Result;
        }

        /// <summary>
        /// Dictionary 타입 - 복사 / To
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sender">원본 Object</param>
        /// <param name="targetList">대상 Object</param>
        /// <returns>복사 성공 여부</returns>
        public static bool CopyTo<T>(Dictionary<string, T> sender, Dictionary<string, T> targetList)
        {
            return CopyFrom<T>(targetList, sender);
        }
        #endregion

        #region List 처리 메소드
        /// <summary>
        /// List 값 추가
        /// </summary>
        /// <typeparam name="T">Item Value Type</typeparam>
        /// <param name="sender">Object</param>
        /// <param name="AValue">Item Value</param>
        /// <param name="bOverwrite">기존 Item 값이 존재할 경우 / true : 덮어쓰기(Last Value), false : 건너띄기(First Value)</param>
        /// <returns>추가 여부</returns>
        public static bool Add<T>(List<T> sender, T AValue, bool bOverwrite = false)
        {
            bool Result = false;
            if (!sender.Contains(AValue) || bOverwrite == true)
            {
                sender.Add(AValue);
                Result = true;
            }
            return Result;
        }
        /// <summary>
        /// List 값 수정
        /// </summary>
        /// <typeparam name="T">Item Value Type</typeparam>
        /// <param name="sender">List Resource</param>
        /// <param name="value">Item Value</param>
        /// <param name="bNotExistCaseAdd">기존 Value가 없는 경우(true : 추가하기)</param>
        /// <returns>수정(추가) 여부</returns>
        public static bool Mod<T>(List<T> sender, T value, bool bNotExistCaseAdd = false)
        {
            bool Result = false;

            int index = -1;
            if (sender != null)
            {
                if (sender.Contains(value) && (index = sender.IndexOf(value)) >= 0)
                {
                    sender[index] = value;
                    Result = true;
                }
                else if (bNotExistCaseAdd == true)
                {
                    sender.Add(value);
                    Result = true;
                }
            }
            return Result;
        }
        #endregion
    }
}
