using FluentNHibernate.Mapping;
using PonteAlta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class TransacaoMap : ClassMap<Transacao>
    {
        public TransacaoMap()
        {
            Table("USU_VE001TNS");

            Id(x => x.Codigo, "CODTNS");

            Map(x => x.Descricao, "DESTNS");

            Map(x => x.Empresa, "CODEMP");
        }
    }
}
