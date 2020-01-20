using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonteAlta.Models
{
    public class FormaPagamento
    {
        public virtual string Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string Abreviacao { get; set; }
        
        public virtual int Empresa { get; set; }
    }
}