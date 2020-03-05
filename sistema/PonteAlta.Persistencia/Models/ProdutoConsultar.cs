using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class ProdutoConsultar
    {
        public virtual string CodPro { get; set; }

        public virtual string CodDer { get; set; }

        public virtual string DesPro { get; set; }

        public virtual string DesDer { get; set; }

        public virtual string UniMed { get; set; }

        public virtual string CodTpr { get; set; }

        public virtual decimal SalEst { get; set; }

        public virtual decimal PreBas { get; set; }
    }
}
