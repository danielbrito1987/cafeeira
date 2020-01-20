using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using PonteAlta.Persistencia.Mapeamento;
using NHibernate.Cfg;
using NHibernate.Event;

namespace PonteAlta.Persistencia.Persistencia
{
    public class SessionFactory
    {
        private static SessionFactory fInstancia;

        public static SessionFactory Instancia
        {
            get
            {
                if (fInstancia == null)
                    fInstancia = new SessionFactory();

                return fInstancia;
            }
        }

        private ISessionFactory ISessionFactory { get; set; }

        public SessionFactory()
        {
            try
            {
                FluentConfiguration config = obterConfiguracaoFluent();
                this.ISessionFactory = config.BuildSessionFactory();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        private static FluentConfiguration obterConfiguracaoFluent()
        {
           /* return Fluently.Configure()
                           .Database(MsSqlConfiguration
                                        .MsSql2012
                                        .ConnectionString(c => c.FromConnectionStringWithKey("conexao")))*/
            return Fluently.Configure()
                           .Database(MsSqlConfiguration
                                        .MsSql2008
                                        .ConnectionString(c => c.FromConnectionStringWithKey("conexao")))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsuarioMap>());
        }

        public static void GerarBanco()
        {
            obterConfiguracaoFluent().ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true)).BuildSessionFactory();
        }

        public ISession ObterSessao()
        {
            return this.ISessionFactory.OpenSession();
        }
    }
}
