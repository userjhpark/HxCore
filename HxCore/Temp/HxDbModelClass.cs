using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace HxCore
{
    /*
    public class HxDbModelClass<T>
    {
        public HxDbModelClass()
        {
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        public static T FromJson(string json) => JsonConvert.DeserializeObject<T>(json, Converter.Settings);

        public static string ToJson(this T self) => JsonConvert.SerializeObject(self, Converter.Settings);
        
    }
    */
}
