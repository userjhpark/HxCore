using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace HxCore
{
    public class HxDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDisposable
    {
        protected Dictionary<TKey, TValue> bind = null;

        public HxDictionary()
        {
            bind = new Dictionary<TKey, TValue>();
        }

        public new TValue this[TKey key]
        {
            get
            {
                if (bind != null && bind.ContainsKey(key))
                {
                    return bind[key];
                }
                return default(TValue);
            }

            set
            {
                if (bind != null)
                {
                    bind.AddEx(key, (TValue)value);
                }
            }
        }

        

        public void Dispose()
        {
            if (bind != null)
            {
                bind.Clear();
            }
            bind = null;
        }

        public static HxDictionary<TKey, TValue> Copy(HxDictionary<TKey, TValue> source)
        {
            if(source != null)
            {
                HxDictionary<TKey, TValue> Result = new HxDictionary<TKey, TValue>();
                foreach(KeyValuePair<TKey, TValue> keyValuePair in source)
                {
                    Result.AddEx(keyValuePair.Key, keyValuePair.Value);
                }
                return Result;
            }
            return null;
        }
    }


    public class HxDictionaryStringValue : HxDictionary<string, object>
    {
        public HxDictionaryStringValue()
            :base()
        {
            ; ;
        }

        public static HxDictionaryStringValue Copy(HxDictionaryStringValue source)
        {
            if (source != null)
            {
                HxDictionaryStringValue Result = new HxDictionaryStringValue();
                foreach (KeyValuePair<string, object> keyValuePair in source)
                {
                    Result.AddEx(keyValuePair.Key, keyValuePair.Value);
                }
                return Result;
            }
            return null;
        }
    }
}
