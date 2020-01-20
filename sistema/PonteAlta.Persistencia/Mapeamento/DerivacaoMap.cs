using FluentNHibernate.Mapping;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonteAlta.Persistencia.Mapeamento
{
    public class DerivacaoMap : ClassMap<Derivacao>
    {
        public DerivacaoMap()
        {
            Table("E075DER");
            LazyLoad();
            CompositeId()
              .KeyProperty(x => x.CodEmpresa, set => {
                  set.Type("Int32");
                  set.ColumnName("CODEMP");
                  set.Access.Property();
              })
              .KeyProperty(x => x.CodProduto, set => {
                  set.Type("String");
                  set.ColumnName("CODPRO");
                  set.Length(14);
                  set.Access.Property();
              })
              .KeyProperty(x => x.Codigo, set => {
                  set.Type("String");
                  set.ColumnName("CODDER");
                  set.Length(7);
                  set.Access.Property();
              });
            Map(x => x.Nome)
              .Column("DESDER")
              .CustomType("String")
              .Access.Property()
              .Generated.Never().CustomSqlType("VARCHAR2(50 CHAR)")
              .Length(50);
            Map(x => x.DESCPL)
              .Column("DESCPL")
              .CustomType("String")
              .Access.Property()
              .Generated.Never().CustomSqlType("VARCHAR2(90 CHAR)")
              .Length(90);
        }
    }
}
