using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Estrutura.Persistence
{
    public interface IFabrica
    {
        StandardKernel Kernel { get; set; }

        T Obter<T>();

        T ObterRepositorio<T>(object unidadeTrabalho);
    }
}
