using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Parameters;
using PonteAlta.Persistencia.Persistencia;
using Estrutura.Persistence;
using Estrutura;

namespace PonteAlta.Infra
{
    public class Fabrica : IFabrica
    {
        private static Fabrica fabrica { get; set; }

        //Singleton
        public static Fabrica Instancia
        {
            get
            {
                if (fabrica == null)
                    fabrica = new Fabrica();

                return fabrica;
            }
        }

        public virtual StandardKernel Kernel { get; set; }

        public Fabrica()
        {
            ModuloVIX modulo = new ModuloVIX();
            Kernel = new StandardKernel(modulo);
        }

        public T Obter<T>()
        {
            if (typeof(T) is IUnidadeTrabalho)
            {
                IUnidadeTrabalho unidadeTrabalho = Kernel.Get<IUnidadeTrabalho>();
                unidadeTrabalho.Fabrica = Instancia;

                return (T)unidadeTrabalho;
            }
            else
                return Kernel.Get<T>();
        }

        public T ObterRepositorio<T>(object unidadeTrabalho)
        {
            return Kernel.Get<T>(new ConstructorArgument("unidadeTrabalho", unidadeTrabalho));
        }
    }
}
