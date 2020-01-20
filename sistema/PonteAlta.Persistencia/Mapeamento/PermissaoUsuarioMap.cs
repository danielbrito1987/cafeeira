using FluentNHibernate.Mapping;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class PermissaoUsuarioMap : ClassMap<PermissaoUsuario>
    {
        public PermissaoUsuarioMap()
        {
            Table("USU_VE099USU");

            Id(x => x.CODUSU, "CODUSU");

            Map(x => x.NOMUSU, "NOMUSU");
            Map(x => x.INTNET, "INTNET");
            Map(x => x.FILATI, "FILATI");
            Map(x => x.SIGFIL, "SIGFIL");
            Map(x => x.EMPATI, "EMPATI");
            Map(x => x.NOMCOM, "NOMCOM");
            Map(x => x.VENPDS, "VENPDS");
            Map(x => x.VENLPV, "VENLPV");
            Map(x => x.VENLPF, "VENLPF");
        }
    }
}
