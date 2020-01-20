using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace adminlte.Models
{
    public class Usuario
    {
        public virtual int Codigo { get; set; }

        public virtual string Nome { get; set; }

        public virtual string Login { get; set; }

        public virtual string TipCol { get; set; }

        public virtual string CodEmp { get; set; }

        public virtual string CodFil { get; set; }

        public virtual string CodLoc { get; set; }

        public virtual string NumCad { get; set; }
    }
}