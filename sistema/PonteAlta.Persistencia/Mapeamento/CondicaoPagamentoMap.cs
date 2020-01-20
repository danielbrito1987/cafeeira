using FluentNHibernate.Mapping;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class CondicaoPagamentoMap : ClassMap<CondicaoPagamento>
    {
        public CondicaoPagamentoMap()
        {
            Table("USU_VE028CPG");

            Id(u => u.Codigo, "CODCPG");

            Map(x => x.Descricao, "DESCPG");
            Map(x => x.Abreviacao, "ABRCPG");
            Map(x => x.TipoEspecial, "CPGESP");
            Map(x => x.ManutencaoParcela, "MANPAR");
            Map(x => x.TipoParcela, "TIPPAR");
            Map(x => x.QuantidadeParcelas, "QTDPAR");
            Map(x => x.TaxaJuros, "TXAJUR");
            Map(x => x.TaxaAcrescimoFinanceiro, "ACRFIN");
            Map(x => x.TipoJuros, "TIPJUR");

            Map(u => u.Empresa, "CODEMP");
            References(u => u.FormaPagamento, "CODFPG");
        }
    }
}
