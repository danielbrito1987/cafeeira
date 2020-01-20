using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PonteAlta.Models;
using FluentNHibernate.Mapping;

namespace PonteAlta.Mapeamento
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("USU_VE085CLI");

            Id(u => u.Codigo, "CODCLI");//.GeneratedBy.Assigned();
            Map(g => g.Nome, "NOMCLI").Nullable();
            Map(g => g.Apelido, "APECLI").Nullable();
            Map(g => g.InscricaoEstadual, "INSEST").Nullable();
            Map(g => g.Situacao, "SITCLI").Nullable().Precision(4);
            Map(g => g.CPF, "CGCCPF").Nullable();

            Map(u => u.CodGrupo, "CODGRE");
            //References(u => u.Convenio, "CODCON").Nullable();
        }
    }
}