using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace adminlte.Models
{
    public class Filial
    {
        public virtual int Codigo { get; set; }
        
        public virtual string Nome { get; set; }

        public virtual int Empresa { get; set; }
    }
}