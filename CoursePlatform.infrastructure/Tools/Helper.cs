using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CoursePlatform.infrastructure.Tools
{
    public static class Helper
    {
        public static IDictionary<int, string> GetEnumDictionary<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T is not an Enum type");

            return Enum.GetValues(typeof(T))
                .Cast<object>()
                .ToDictionary(k => (int)k, v => ((Enum)v).GetDescription());
        }

        public static string GetDescription(this Enum enumeration)
        {
            string value = enumeration.ToString();
            Type enumType = enumeration.GetType();
            var descAttribute = (DescriptionAttribute[])enumType
                .GetField(value)
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descAttribute.Length > 0 ? descAttribute[0].Description : value;
        }


        public static bool AddItem<Tkey, Tval>(IDictionary<Tkey,Tval> source, Tkey key, Tval val)
        {
            if (source is null)
            {
                source = new Dictionary<Tkey, Tval>();
            }

            return source.TryAdd(key, val);
           
        }
    }
}
