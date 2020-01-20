using FluentNHibernate.Mapping;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class FormaPagamentoMap : ClassMap<FormaPagamento>
    {
        public FormaPagamentoMap()
        {
            Table("USU_VE066FPG");

            Id(x => x.Codigo, "CODFPG");

            Map(x => x.Empresa, "CODEMP");
            Map(x => x.Descricao, "DESFPG");
            Map(x => x.Abreviacao, "ABRFPG");
        }
    }
}
