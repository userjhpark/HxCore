using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Data;
using System.Globalization;

namespace HxCore
{
    public abstract class HxDbModelSetValue : HxSetValueObject, IHxSetValue
    {
        #region CDF
        public const string _CDF_IS_USE_ = "is_use";
        //public const string _CDF_RAW_GUID_ = "raw_guid";
        public const string _CDF_REG_DATE_ = "reg_date";
        public const string _CDF_REG_AGENT_ = "reg_agent";
        public const string _CDF_REG_UNO_ = "reg_uno";
        public const string _CDF_REG_USER_ = "reg_user";
        public const string _CDF_MOD_DATE_ = "mod_date";
        public const string _CDF_MOD_AGENT_ = "mod_agent";
        public const string _CDF_MOD_UNO_ = "mod_uno";
        public const string _CDF_MOD_USER_ = "mod_user";
        #endregion

        #region Fields
        // 사용 여부 사용 여부(Y:진행중,N:완료)
        [JsonProperty("is_use")]
        public string IS_USE { get; set; }

        // GUID 
        //[JsonProperty("raw_guid")]
        //public string RAW_GUID { get; set; }

        // 최초 생성일 
        [JsonProperty("reg_date")]
        public DateTime? REG_DATE { get; set; }

        // 최초 생성 정보 
        [JsonProperty("reg_agent")]
        public string REG_AGENT { get; set; }

        // 최초 생성자 UNO 
        [JsonProperty("reg_uno")]
        public int? REG_UNO { get; set; }

        // 최초 생성자 정보 
        [JsonProperty("reg_user")]
        public string REG_USER { get; set; }

        // 최종 수정일 
        [JsonProperty("mod_date")]
        public DateTime? MOD_DATE { get; set; }

        // 최종 수정 정보 
        [JsonProperty("mod_agent")]
        public string MOD_AGENT { get; set; }

        // 최종 수정자 UNO 
        [JsonProperty("mod_uno")]
        public int? MOD_UNO { get; set; }

        // 최초 수정자 정보 
        [JsonProperty("mod_user")]
        public string MOD_USER { get; set; }
        #endregion
        public HxDbModelSetValue()
        {
            ; ;
        }

        public HxDbModelSetValue(DataView dv, int index = 0)
            : this()
        {
            this.SetValue(dv, index);
        }
        public HxDbModelSetValue(DataTable dt, int index = 0)
            : this()
        {
            this.SetValue(dt, index);
        }

        public HxDbModelSetValue(DataRow dr)
           : this()
        {
            this.SetValue(dr);
        }

        public void CopyData(HxDbModelSetValue param)
        {
            this.IS_USE = param.IS_USE;
            //this.RAW_GUID = param.RAW_GUID;
            this.REG_DATE = param.REG_DATE;
            this.REG_AGENT = param.REG_AGENT;
            this.REG_UNO = param.REG_UNO;
            this.REG_USER = param.REG_USER;
            this.MOD_DATE = param.MOD_DATE;
            this.MOD_AGENT = param.MOD_AGENT;
            this.MOD_UNO = param.MOD_UNO;
            this.MOD_USER = param.MOD_USER;
        }

        //public new static string GetQueryString(string queryString, string mWhere)
        //{
        //    return HxUtils.GetQueryString(queryString, mWhere);
        //}

        public override void SetMatchFieldValue(string name, object value)
        {
            switch (name)
            {
                case _CDF_IS_USE_: IS_USE = value.ToStringEx(); break;
                //case _CDF_RAW_GUID_: RAW_GUID = value.ToStringEx(); break;
                case _CDF_REG_DATE_: REG_DATE = value.ToNullableDateTimeEx(); break;
                case _CDF_REG_AGENT_: REG_AGENT = value.ToStringEx(); break;
                case _CDF_REG_UNO_: REG_UNO = value.ToNullableIntEx(); break;
                case _CDF_REG_USER_: REG_USER = value.ToStringEx(); break;
                case _CDF_MOD_DATE_: MOD_DATE = value.ToNullableDateTimeEx(); break;
                case _CDF_MOD_AGENT_: MOD_AGENT = value.ToStringEx(); break;
                case _CDF_MOD_UNO_: MOD_UNO = value.ToNullableIntEx(); break;
                case _CDF_MOD_USER_: MOD_USER = value.ToStringEx(); break;
                default:
                    base.SetMatchFieldValue(name, value);
                    break;
            }
        }

        public static IHxSetValue FromJson(string json) => JsonConvert.DeserializeObject<IHxSetValue>(json, Converter.JsonSettings);
        public static string ToJson(IHxSetValue self) => JsonConvert.SerializeObject(self, Converter.JsonSettings);
       

        public static class Converter
        {
            public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                    {
                        new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                    },
            };
        }
    }
    public static class HxDbModelSetValueExtensions
    {
        public static object FromJsonToObjectEx(string json) => JsonConvert.DeserializeObject(json, HxDbModelSetValue.Converter.JsonSettings);
        public static IHxSetValue FromJsonEx(string json) => JsonConvert.DeserializeObject<IHxSetValue>(json, HxDbModelSetValue.Converter.JsonSettings);
        public static string ToJsonEx(this IHxSetValue self) => JsonConvert.SerializeObject(self, HxDbModelSetValue.Converter.JsonSettings);
    }
}
