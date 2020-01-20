using adminlte.Models;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Models
{
    public class Avalista
    {
        public virtual int Codigo { get; set; }
        
        public virtual Pedido Pedido { get; set; }

        public virtual Filial Filial { get; set; }

        public virtual int Empresa { get; set; }
    }
}
