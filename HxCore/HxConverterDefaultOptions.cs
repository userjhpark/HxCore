namespace HxCore
{
    public static class HxConverterDefaultOptions
    {
        public static readonly Newtonsoft.Json.JsonSerializerSettings JsonSettings = new Newtonsoft.Json.JsonSerializerSettings
        {
            MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Ignore,
            DateParseHandling = Newtonsoft.Json.DateParseHandling.None,
            Converters =
                    {
                        new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal }
                    },
        };
    }
}
