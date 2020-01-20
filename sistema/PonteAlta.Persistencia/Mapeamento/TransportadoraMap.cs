using FluentNHibernate.Mapping;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class TransportadoraMap : ClassMap<Transportadora>
    {
        public TransportadoraMap()
        {
            Table("USU_VE073TRA");

            Id(x => x.Codigo, "CODTRA");

            Map(x => x.Nome, "NOMTRA");
        }
    }
}
