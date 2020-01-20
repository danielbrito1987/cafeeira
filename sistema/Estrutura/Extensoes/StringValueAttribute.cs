using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    /// <summary>
    /// Atribui um valor do tipo String para um item de uma enumeração.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }
    }
}
