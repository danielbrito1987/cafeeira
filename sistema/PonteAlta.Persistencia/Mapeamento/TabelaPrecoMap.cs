using FluentNHibernate.Mapping;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class TabelaPrecoMap : ClassMap<TabelaPreco>
    {
        public TabelaPrecoMap()
        {
            Table("USU_VE081TAB");

            Id(x => x.Codigo, "CODTPR");

            Map(x => x.Descricao, "DESTPR");
        }
    }
}
