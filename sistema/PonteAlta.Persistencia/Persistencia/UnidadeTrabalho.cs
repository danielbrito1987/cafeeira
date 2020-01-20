using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Estrutura;
using NHibernate;
using NHibernate.Linq;
using Estrutura.Persistence;

namespace PonteAlta.Persistencia.Persistencia
{
    public class UnidadeTrabalho : IUnidadeTrabalho
    {
        public ISession Sessao { get; set; }

        public IFabrica Fabrica { get; set; }

        private ITransaction transaction { get; set; }

        public UnidadeTrabalho()
        {
            Sessao = SessionFactory.Instancia.ObterSessao();
        }

        public void BeginTransaction()
        {
            transaction = Sessao.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            transaction.Dispose();
        }

        public IList<T> ExecuteSql<T>(string sql, Dictionary<string, object> parameters = null)
        {
            ISQLQuery sqlQuery = Sessao.CreateSQLQuery(sql);

            if (parameters != null)
                foreach (var item in parameters)
                    sqlQuery.SetParameter(item.Key, item.Value);

            sqlQuery.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<T>());

            return sqlQuery.List<T>();
        }

        public void ExecuteProcedure(string procedure, Dictionary<string, object> parameters = null)
        {
            ISQLQuery sqlQuery = Sessao.CreateSQLQuery(procedure);

            if (parameters != null)
                foreach (var item in parameters)
                    sqlQuery.SetParameter(item.Key, item.Value);

            sqlQuery.ExecuteUpdate();
        }

        public T ObterPorId<T>(object id)
        {
            return Sessao.Get<T>(id);
        }

        public T ObterPorId<T>(T entidade, string[] propModificadas)
        {
            object codigo = null;

            codigo = entidade.GetType().GetProperty("Codigo").GetValue(entidade);

            T entidadeAux = ObterPorId<T>(codigo);

            if (propModificadas != null)
            {
                foreach (string prop in propModificadas)
                {
                    PropertyInfo property = entidade.GetType().GetProperty(prop);

                    if (property != null)
                        property.SetValue(entidadeAux, property.GetValue(entidade));
                }
            }

            return entidadeAux;
        }

        public IQueryable<T> ObterTodos<T>()
        {
            if (Sessao.IsOpen)
                return Sessao.Query<T>();
            else
                return null;
        }

        public void Salvar<T>(T entidade)
        {
            Sessao.Flush();
            //Sessao.Clear();
            Sessao.Save(entidade);
        }

        public void Atualizar<T>(T entidade)
        {
            Sessao.Update(entidade);
        }

        public void Remover<T>(int id)
        {
            object entidade = Sessao.Get<T>(id);

            if (entidade != null)
                Sessao.Delete(entidade);
        }

        public void Remover<T>(T entidade)
        {
            Sessao.Delete(entidade);
        }


        public IList<T> ExecuteSql<T>(string sql)
        {

            ISQLQuery sqlQuery = Sessao.CreateSQLQuery(sql);
            sqlQuery.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<T>());

            return sqlQuery.List<T>();

        }
    }
}
