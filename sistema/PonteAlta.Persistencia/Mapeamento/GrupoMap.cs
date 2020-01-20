using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PonteAlta.Models;
using FluentNHibernate.Mapping;

namespace PonteAlta.Mapeamento
{
    public class GrupoMap : ClassMap<Grupo>
    {
        public GrupoMap()
        {
            Table("USU_VE069GRE");

            Id(u => u.Codigo, "CODGRE");//.GeneratedBy.Assigned();
            Map(g => g.Nome, "NOMGRE");

        }
    }
}