using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Util
{
    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].value;
            }

            return output;
        }

        public static T GetEnumValue<T>(T tipoEnum, string value)
        {
            Type type = tipoEnum.GetType();
            FieldInfo[] fieldInfos = type.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                StringValue[] attrs = fieldInfo.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
                if (attrs != null && attrs.Length == 1)
                {
                    if (attrs[0].value.Equals(value))
                    {
                        return (T)fieldInfo.GetValue(tipoEnum);
                    }
                }
            }
            return tipoEnum;
        }
    }
}
