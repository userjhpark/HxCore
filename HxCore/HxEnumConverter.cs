using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HxCore
{
    /// <summary>
    /// EnumConverter Extend Class
    /// </summary>
    public class HxEnumConverter : EnumConverter
    {
        // EnumType Converter에서 사용되는 temp 변수입니다.
        private System.Type _enumType;
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="type">Enum Type</param>
        public HxEnumConverter(Type type)
            : base(type)
        {
            _enumType = type;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destType)
        {
            System.Reflection.FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
            DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            if (dna != null)
                return dna.Description;
            else
                return value.ToString();
        }

        public object ConvertTo(object value, System.Type destType, Type attr_type)
        {
            System.Reflection.FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
            if (attr_type == typeof(DisplayNameAttribute))
            {
                DisplayNameAttribute dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(fi, typeof(DisplayNameAttribute));
                if (dna != null)
                    return dna.DisplayName;
                else
                    return value.ToString();
            }
            else if (attr_type == typeof(DescriptionAttribute))
            {
                DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if (dna != null)
                    return dna.Description;
                else
                    return value.ToString();
            }
            else
                return value.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type srcType)
        {
            return srcType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            foreach (System.Reflection.FieldInfo fi in _enumType.GetFields())
            {
                DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if ((dna != null) && ((string)value == dna.Description))
                    return Enum.Parse(_enumType, fi.Name);
            }
            return Enum.Parse(_enumType, (string)value);
        }

        public object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type attr_type)
        {
            foreach (System.Reflection.FieldInfo fi in _enumType.GetFields())
            {
                if (attr_type == typeof(DisplayNameAttribute))
                {
                    DisplayNameAttribute dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(fi, typeof(DisplayNameAttribute));
                    if ((dna != null) && ((string)value == dna.DisplayName))
                        return Enum.Parse(_enumType, fi.Name);
                }
                else if (attr_type == typeof(DescriptionAttribute))
                {
                    DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                    if ((dna != null) && ((string)value == dna.Description))
                        return Enum.Parse(_enumType, fi.Name);
                }

            }
            return Enum.Parse(_enumType, (string)value);
        }
    }
}
