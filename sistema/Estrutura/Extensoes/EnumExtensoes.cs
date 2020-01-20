using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    public static class EnumExtensoes
    {
        public static string ObterDescricaoEnum(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        /// <summary>
        /// Método que obtém o valor do atributo de descrição para eumeradors
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ObterDescricaoEnum<T>(this T? value) where T : struct
        { 
            if (!typeof(T).IsEnum)
                throw new Exception("Must be an enum.");

            if (!value.HasValue) 
                return string.Empty;

            FieldInfo fi = value.GetType().GetField(value.Value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
