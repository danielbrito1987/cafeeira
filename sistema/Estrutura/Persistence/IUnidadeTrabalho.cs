using Estrutura.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura
{
    public interface IUnidadeTrabalho
    {
        IFabrica Fabrica { get; set; }

        void BeginTransaction();

        void Commit();

        void Rollback();

        void Dispose();

        IList<T> ExecuteSql<T>(string sql, Dictionary<string, object> parameters);

        IList<T> ExecuteSql<T>(string sql);

        void ExecuteProcedure(string procedure, Dictionary<string, object> parameters = null);

        T ObterPorId<T>(object id);

        T ObterPorId<T>(T entidade, string[] propModificadas);

        IQueryable<T> ObterTodos<T>();

        void Salvar<T>(T entidade);

        void Atualizar<T>(T entidade);

        void Remover<T>(int id);

        void Remover<T>(T entidade);

    }
}
