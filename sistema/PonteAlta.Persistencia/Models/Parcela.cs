using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class Parcela
    {
        public virtual int Codigo { get; set; }

        public virtual int SequenciaParcela { get; set; }

        public virtual string DataVencimento { get; set; }

        public virtual Double Percentual { get; set; }

        public virtual Double ValorParcela { get; set; }

        public virtual Pedido Pedido { get; set; }

        public virtual int Filial { get; set; }

        public virtual int Empresa { get; set; }
    }
}
