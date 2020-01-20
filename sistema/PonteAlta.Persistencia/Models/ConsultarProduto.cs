using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Models
{
    public class ConsultaProduto
    {
        public virtual string Codpro { get; set; }

        public virtual string Codder { get; set; }

        public virtual string Despro { get; set; }

        public virtual string Desder { get; set; }

        public virtual string Unimed { get; set; }

        public virtual string Codage { get; set; }

        public virtual string Desage { get; set; }

        public virtual string Codagc { get; set; }

        public virtual string Desagc { get; set; }

        public virtual string Temder { get; set; }

        public virtual string Codtpr { get; set; }

        public virtual string Coddep { get; set; }

        public virtual decimal Salest { get; set; }

        public virtual decimal Prebas { get; set; }
    }
}
