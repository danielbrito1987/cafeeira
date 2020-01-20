using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Util
{
    public class StringValue : Attribute
    {
        public string value { get; private set; }

        public StringValue(string value)
        {
            this.value = value;
        }
    }
}
