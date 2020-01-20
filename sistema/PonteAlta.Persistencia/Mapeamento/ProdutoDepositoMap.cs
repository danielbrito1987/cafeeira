using FluentNHibernate.Mapping;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class ProdutoDepositoMap : ClassMap<ProdutoDeposito>
    {
        public ProdutoDepositoMap()
        {
            Table("USU_VE120DEP");

            CompositeId()
            .KeyProperty(x => x.Empresa, "CODEMP")
            .KeyProperty(x => x.Filial, "CODFIL")
            .KeyProperty(x => x.Produto, "CODPRO")
            .KeyProperty(x => x.Derivacao, "CODDER")
            .KeyProperty(x => x.Deposito, "CODDEP");
        }
    }
}
