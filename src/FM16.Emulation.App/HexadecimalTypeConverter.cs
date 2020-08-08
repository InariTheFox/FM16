using System.ComponentModel;
using System.Globalization;

namespace System
{
    public class HexadecimalTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(byte) || sourceType == typeof(short) ||
                sourceType == typeof(ushort) || sourceType == typeof(int) ||
                sourceType == typeof(uint) || sourceType == typeof(Enum);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value is byte)
                {
                    return $"0x{value:X2}";
                }

                if (value is ushort || value is short)
                {
                    return $"0x{value:X4}";
                }

                if (value is uint || value is int)
                {
                    return $"0x{value:X6}";
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
