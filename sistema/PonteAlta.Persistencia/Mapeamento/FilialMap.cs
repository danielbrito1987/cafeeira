using adminlte.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class FilialMap : ClassMap<Filial>
    {
        public FilialMap()
        {
            Table("USU_VE070FIL");

            Id(x => x.Codigo, "CODFIL");

            Map(x => x.Nome, "SIGFIL");

            Map(x => x.Empresa, "CODEMP");
        }
    }
}
