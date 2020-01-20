using FluentNHibernate.Mapping;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class PlacaMap : ClassMap<Placa>
    {
        public PlacaMap()
        {
            Table("USU_VE073VEI");

            Id(x => x.CodTransportadora, "CODTRA");

            Map(x => x.PlacaVeiculo, "PLAVEI");
        }
    }
}
