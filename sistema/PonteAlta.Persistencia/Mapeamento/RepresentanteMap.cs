using FluentNHibernate.Mapping;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class RepresentanteMap : ClassMap<Representante>
    {
        public RepresentanteMap()
        {
            Table("USU_VE090REP");

            Id(x => x.Codigo, "CODREP");

            Map(x => x.Nome, "NOMREP");
        }
    }
}
