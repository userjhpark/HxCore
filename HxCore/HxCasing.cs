using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.Json;

//using System.Text.Json;
using System.Threading;

namespace HxCore
{
    public class HxCasing
    {
        public static HxNameingCaseType ToCasingType(string input)
        {
            HxNameingCaseType Result = HxNameingCaseType.NormalCase;
            string strInputName = ToPascalCase(input);
            switch (strInputName)
            {
                case "PascalCase":
                case "Pascal":
                    Result = HxNameingCaseType.PascalCase;
                    break;
                case "CamelCase":
                case "Camel":
                    Result = HxNameingCaseType.CamelCase;
                    break;
                case "SnakeCase":
                case "Snake":
                    Result = HxNameingCaseType.SnakeCase;
                    break;
                case "KebabCase":
                case "Kebab":
                    Result = HxNameingCaseType.KebabCase;
                    break;
                case "LowerCase":
                case "Lower":
                    Result = HxNameingCaseType.LowerCase;
                    break;
                case "UpperCase":
                case "Upper":
                    Result = HxNameingCaseType.UpperCase;
                    break;
                case "JsonCase":
                case "Json":
                    Result = HxNameingCaseType.JsonCase;
                    break;
                case "NormalCase":
                case "Normal":
                    Result = HxNameingCaseType.NormalCase;
                    break;
                case "DefaultCase":
                case "Default":
                case "Null":
                case "None":
                case "N/a":
                default:
                    Result = HxNameingCaseType.DefaultCase;
                    break;
            }
            return Result;
        }

        public static string ToString(object input, HxNameingCaseType casingType = HxNameingCaseType.NormalCase)
        {
            return ToString(input.ToStringEx(), casingType);
        }
        public static string ToString(string input, HxNameingCaseType casingType = HxNameingCaseType.NormalCase)
        {
            if (input.IsNullOrWhiteSpaceEx() == true) { return null; }

            string Result = null;

            switch (casingType)
            {
                case HxNameingCaseType.PascalCase:
                    Result = ToPascalCase(input);
                    break;
                case HxNameingCaseType.CamelCase:
                    Result = ToCamelCase(input);
                    break;
                case HxNameingCaseType.SnakeCase:
                    Result = ToSnakeCase(input);
                    break;
                case HxNameingCaseType.KebabCase:
                    Result = ToKebabCase(input);
                    break;
                case HxNameingCaseType.LowerCase:
                    Result = ToLowerCase(input);
                    break;
                case HxNameingCaseType.UpperCase:
                    Result = ToUpperCase(input);
                    break;
                case HxNameingCaseType.JsonCase:
                case HxNameingCaseType.NormalCase:
                case HxNameingCaseType.DefaultCase:
                default:
                    Result = input.ToStringEx();
                    break;
            }
            return Result;
        }
        public static string ToString(string input, string casingString = "NormalCase")
        {
            HxNameingCaseType type = ToCasingType(casingString);
            return ToString(input, type);
        }

        public static string ToPascalCase(string input)
        {
            string Result = input;
            if (input.IsNullOrWhiteSpaceEx() == true) { return Result; }

            // '_' 문자를 기준으로 문자열을 분리
            string[] parts = input.Split('_');
            var sbr = new StringBuilder();
            foreach (string part in parts)
            {
                // 분리된 부분이 비어있지 않은 경우에만 처리
                if (part.Length > 0)
                {
                    // 첫 글자는 대문자로 변환
                    sbr.Append(char.ToUpper(part[0]));
                    // 두 번째 글자부터는 소문자로 변환하여 추가
                    sbr.Append(part.Substring(1).ToLower());
                }
            }
            Result = sbr.ToString();
            return Result;
        }

        public static string ToCamelCase(string input)
        {
            string Result = input;
            if (input.IsNullOrWhiteSpaceEx() == true) { return Result; }

            // '_' 문자를 기준으로 문자열을 분리
            string[] parts = input.Split('_');
            var sbr = new StringBuilder();

            // 첫 번째 부분은 전체를 소문자로 변환
            if (parts.Length > 0)
            {
                sbr.Append(parts[0].ToLower());
            }

            // 두 번째 부분부터는 PascalCase 규칙을 적용
            for (int i = 1; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length > 0)
                {
                    sbr.Append(char.ToUpper(part[0]));
                    sbr.Append(part.Substring(1).ToLower());
                }
            }

            Result = sbr.ToString();
            return Result;
        }

        public static string ToSnakeCase(string input)
        {
            string Result = input;
            if (input.IsNullOrWhiteSpaceEx() == true) { return Result; }

            string str = ToPascalCase(input);
            var sbr = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                // 현재 문자가 대문자일 경우
                if (char.IsUpper(c))
                {
                    // 첫 글자가 아니며, 바로 앞 문자가 밑줄이 아닐 때 밑줄을 추가
                    // (Pascal/camelCase를 처리하기 위함)
                    if (i > 0 && str[i - 1] != '_')
                    {
                        sbr.Append('_');
                    }
                }

                // 현재 문자를 빌더에 추가
                sbr.Append(c);
            }

            // 최종적으로 전체 문자열을 소문자로 변환하여 반환
            return sbr.ToString().ToLower();
        }

        public static string ToKebabCase(string input)
        {
            string Result = input;
            if (input.IsNullOrWhiteSpaceEx() == true) { return Result; }

            string str = ToPascalCase(input);

            var sbr = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                // 대문자를 만나면 하이픈(-) 추가
                if (char.IsUpper(c))
                {
                    if (i > 0 && str[i - 1] != '-')
                    {
                        sbr.Append('-');
                    }
                }

                // snake_case 입력도 처리하기 위해 밑줄(_)을 하이픈(-)으로 변경
                if (c == '_')
                {
                    sbr.Append('-');
                }
                else
                {
                    sbr.Append(c);
                }
            }
            return sbr.ToString().ToLower();
        }
        public static string ToLowerCase(string input)
        {
            return input?.ToLower();
        }
        public static string ToUpperCase(string input)
        {
            return input?.ToUpper();
        }

        public static string ToJsonString(object value, HxNameingCaseType casingType = HxNameingCaseType.PascalCase)
        {
            /*
            JsonSerializerOptions optionsWithCustomCase = JsonSerializerOptions.Default;
            switch (casingType)
            {
                case HxNameingCaseType.PascalCase:
                    //optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = new PascalCaseNamingPolicy(), WriteIndented = true };
                    optionsWithCustomCase = new JsonSerializerOptions { WriteIndented = true };
                    break;
                case HxNameingCaseType.CamelCase:
                    //optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = new CamelCaseNamingPolicy(), WriteIndented = true };
                    optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
                    break;
                case HxNameingCaseType.SnakeCase:
                    //optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = new SnakeCaseNamingPolicy(), WriteIndented = true };
                    optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, WriteIndented = true };
                    break;
                case HxNameingCaseType.KebabCase:
                    //optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = new KebabCaseNamingPolicy(), WriteIndented = true };
                    optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower, WriteIndented = true };
                    break;
                case HxNameingCaseType.LowerCase:
                    optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = new LowerCaseNamingPolicy(), WriteIndented = true };
                    break;
                case HxNameingCaseType.UpperCase:
                    optionsWithCustomCase = new JsonSerializerOptions { PropertyNamingPolicy = new UpperCaseNamingPolicy(), WriteIndented = true };
                    break;
                case HxNameingCaseType.JsonCase:
                    //optionsWithCustomCase = new JsonSerializerOptions { WriteIndented = true };
                    break;
                case HxNameingCaseType.NormalCase:
                    var settingsWithoutAttribute = new JsonSerializerSettings
                    {
                        ContractResolver = new NormalCaseContractResolver()
                    };
                    optionsWithCustomCase = new JsonSerializerOptions { WriteIndented = true };
                    break;
            }
            string jsonWithNewOptions = System.Text.Json.JsonSerializer.Serialize(value, optionsWithCustomCase);
            */

            JsonSerializerSettings optionsWithCustomCase = ToJsonSerializerSettings(casingType);
            string Result = JsonConvert.SerializeObject(value, optionsWithCustomCase);
            return Result ?? JsonConvert.SerializeObject(value);
        }
        public static string ToJsonSerializeString(object value, HxNameingCaseType casingType = HxNameingCaseType.PascalCase)
        {
            return ToJsonString(value, casingType);
        }
        public static T ToJsonDeserializeObject<T>(string json, HxNameingCaseType casingType)
        {
            T Result = default;
            if (!json.IsNullOrWhiteSpaceEx())
            {
                try
                {
                    JsonSerializerSettings optionsWithCustomCase = ToJsonSerializerSettings(casingType);
                    Result = JsonConvert.DeserializeObject<T>(json, optionsWithCustomCase);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    try
                    {
                        Result = JsonConvert.DeserializeObject<T>(json);
                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                        //throw;
                    }

                    Debug.WriteLine(ex.Message);
                    //throw;
                }
            }
            return Result;
        }

        public static JsonSerializerSettings ToJsonSerializerSettings(HxNameingCaseType casingType)
        {
            JsonSerializerSettings Result = new JsonSerializerSettings();
            switch (casingType)
            {
                case HxNameingCaseType.PascalCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxPascalCaseContractResolver() };
                    break;
                case HxNameingCaseType.CamelCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxCamelCaseContractResolver() };
                    break;
                case HxNameingCaseType.SnakeCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxSnakeCaseContractResolver() };
                    break;
                case HxNameingCaseType.KebabCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxKebabCaseContractResolver() };
                    break;
                case HxNameingCaseType.LowerCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxLowerCaseContractResolver() };
                    break;
                case HxNameingCaseType.UpperCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxUpperCaseContractResolver() };
                    break;
                case HxNameingCaseType.NormalCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxNormalCaseContractResolver() };
                    break;
                case HxNameingCaseType.JsonCase:
                    Result = new JsonSerializerSettings { ContractResolver = new HxJsonCaseContractResolver() };
                    break;
                case HxNameingCaseType.DefaultCase:
                default:
                    Result = new JsonSerializerSettings { ContractResolver = new HxDefaultCaseContractResolver() };
                    break;
            }
            return Result;
        }

        public static List<Dictionary<string, object>> ToJsonListWithNamingCase(DataTable inputData, HxNameingCaseType nameingCaseType)
        {
            
            if(inputData == null || inputData.Columns.Count <= 0 || inputData.Rows.Count <= 0) { return null; }

            DataTable copyData = inputData.Copy();
            int idx = 0;
            foreach (DataColumn dc in copyData.Columns)
            {
                string strOldColName = dc.ColumnName;
                string strNewColName = HxCasing.ToString(dc.ColumnName, nameingCaseType);
                if (strOldColName.ToUpper() == strNewColName.ToUpper()) 
                {
                    if (strOldColName != strNewColName)
                    {
                        dc.ColumnName = strNewColName;
                    }
                    continue; 
                }

                if (dc.Table.Columns.Contains(strNewColName))
                {
                    strNewColName += "_" + idx;
                }
                dc.ColumnName = strNewColName;

                idx++;
            }

            /*
            List<Dictionary<string, object>> Result = new List<Dictionary<string, object>>();
            foreach (DataRow row in data.Rows)
            {
                Dictionary<string, object> r = new Dictionary<string, object>();
                foreach (DataColumn col in data.Columns)
                {
                    r[col.ColumnName] = row[col];
                }
                Result.Add(r);
            }
            return Result;
            */
            return HxConvert.ToListDictionary(copyData);
        }
    }




    public enum HxNameingCaseType
    {
        DefaultCase = 0
        , NormalCase
        , PascalCase          // 파스칼 케이스 (e.g., UserName)
        , CamelCase           // 카멜 케이스 (e.g., userName)
        , SnakeCase           // 스네이크 케이스 (e.g., user_name)
        , KebabCase           // 케밥 케이스 (e.g., user-name)
        , LowerCase
        , UpperCase
        , JsonCase            // json 태그가 있으면 그 값을, 없으면 Normal처럼 처리
    }
    public class HxPascalCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // "UserName"
            return HxCasing.ToPascalCase(name);
        }
    }
    public class HxCamelCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // "userName"
            return HxCasing.ToCamelCase(name);
        }
    }
    public class HxSnakeCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // "user_name"
            return HxCasing.ToSnakeCase(name);
        }
    }
    public class HxKebabCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // "userName"
            return HxCasing.ToKebabCase(name);
        }
    }
    public class HxLowerCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // "userName"
            return HxCasing.ToLowerCase(name);
        }
    }
    public class HxUpperCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // "userName"
            return HxCasing.ToUpperCase(name);
        }
    }

    
    public class HxNormalCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            // [JsonProperty]의 PropertyName 대신 원본 멤버 이름을 사용하도록 강제
            property.PropertyName = member.Name;
            return property;
        }
    }
    public class HxPascalCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            // [JsonProperty]의 PropertyName 대신 원본 멤버 이름을 사용하도록 강제
            property.PropertyName = HxCasing.ToPascalCase(member.Name);
            return property;
        }
    }
    public class HxCamelCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            // [JsonProperty]의 PropertyName 대신 원본 멤버 이름을 사용하도록 강제
            property.PropertyName = HxCasing.ToCamelCase(member.Name);
            return property;
        }
    }
    public class HxSnakeCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            // [JsonProperty]의 PropertyName 대신 원본 멤버 이름을 사용하도록 강제
            property.PropertyName = HxCasing.ToSnakeCase(member.Name);
            return property;
        }
    }
    public partial class HxKebabCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            // [JsonProperty]의 PropertyName 대신 원본 멤버 이름을 사용하도록 강제
            property.PropertyName = HxCasing.ToKebabCase(member.Name);
            return property;
        }
    }
    public partial class HxLowerCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            // [JsonProperty]의 PropertyName 대신 원본 멤버 이름을 사용하도록 강제
            property.PropertyName = HxCasing.ToLowerCase(member.Name);
            return property;
        }
    }
    public partial class HxUpperCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            // [JsonProperty]의 PropertyName 대신 원본 멤버 이름을 사용하도록 강제
            property.PropertyName = HxCasing.ToUpperCase(member.Name);
            return property;
        }
    }
    public partial class HxJsonCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        /*
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = member.Name
            return property;
        }
        */
    }
    public class HxDefaultCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {

    }


}
