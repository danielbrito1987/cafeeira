using adminlte.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("ew99usu");

            Id(x => x.Codigo, "r999usu_codusu");

            Map(x => x.Nome, "r910usu_nomcom");
            Map(x => x.Login, "r999usu_nomcom");
            Map(x => x.TipCol, "r999usu_tipcol");
            Map(x => x.CodEmp, "r999usu_numemp");
            Map(x => x.CodFil, "r999usu_codfil");
            Map(x => x.CodLoc, "r999usu_codloc");
            Map(x => x.NumCad, "r999usu_numcad");
        }
    }
}