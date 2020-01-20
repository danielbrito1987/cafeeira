using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace adminlte.Models
{
    public class Empresa
    {
        public virtual int Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string SiglaEmpresa { get; set; }
    }
}