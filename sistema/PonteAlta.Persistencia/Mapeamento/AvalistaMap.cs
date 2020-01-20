using adminlte.Models;
using FluentNHibernate.Mapping;
using PonteAlta.Models;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class AvalistaMap : ClassMap<Avalista>
    {
        public AvalistaMap()
        {
            Table("USU_VT120AVA");

            Id(u => u.Codigo, "CODAVA");//.GeneratedBy.Assigned();

            Map(u => u.Empresa, "CODEMP");

            References(u => u.Pedido)
                .Class<Pedido>()
              .Access.Property()
              .Cascade.None()
              .LazyLoad()
              .Columns("NUMPED");
            References(x => x.Filial)
                .Class<Filial>()
                .Access.Property()
                .Cascade.None()
                .LazyLoad()
                .Columns("CODFIL");

        }
    }
}
