using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Models
{
    public class Placa
    {
        public virtual int CodTransportadora { get; set; }

        public virtual string PlacaVeiculo { get; set; }
    }
}
