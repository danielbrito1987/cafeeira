using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    public static class EnumValuesExtensions
    {
        /// <summary>
        /// Retorna o valor do tipo String associado ao item da enumeração através do atributo StringValue.
        /// </summary>
        /// <param name="value">Item da enumeração do qual se deseja o valor.</param>
        /// <returns>Retorna o valor do tipo String associado ao item da enumeração através do atributo StringValue.</returns>
        public static string AsString(this Enum value, System.Resources.ResourceManager resourceManager = null, System.Globalization.CultureInfo cultureInfo = null)
        {
            if (value.IsEmpty())
                return null;

            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            StringValueAttribute[] attribs = (StringValueAttribute[])fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false);

            string stringValue = attribs.Length > 0 ? attribs[0].Value : value.ToString();

            if (resourceManager != null && !String.IsNullOrEmpty(stringValue))
            {
                string localizedStringValue = resourceManager.GetString(stringValue, cultureInfo ?? System.Globalization.CultureInfo.CurrentCulture);

                if (!String.IsNullOrEmpty(localizedStringValue))
                    return localizedStringValue;
            }

            return stringValue;
        }

    }
}
