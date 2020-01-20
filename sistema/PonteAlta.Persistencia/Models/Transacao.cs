using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonteAlta.Models
{
    public class Transacao
    {
        public virtual string Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual int Empresa { get; set; }

    }
}