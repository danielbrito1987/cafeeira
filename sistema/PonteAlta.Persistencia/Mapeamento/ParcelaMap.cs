using FluentNHibernate.Mapping;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class ParcelaMap : ClassMap<Parcela>
    {
        public ParcelaMap()
        {
            Table("USU_VE120PAR ");

            Id(u => u.Codigo, "SEQPAR");//.GeneratedBy.Assigned();
            Map(g => g.SequenciaParcela, "SEQPAR").Nullable();
            Map(g => g.Percentual, "PERPAR").Nullable();
            Map(g => g.ValorParcela, "VLRPAR").Nullable();
            Map(g => g.DataVencimento, "VCTPAR").Nullable().Precision(4);
            Map(u => u.Filial, "CODFIL").Nullable();
            Map(u => u.Empresa, "CODEMP").Nullable();

            References(u => u.Pedido, "NUMPED");
        }
    }
}
