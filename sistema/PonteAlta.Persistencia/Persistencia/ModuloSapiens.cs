using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estrutura;
using Ninject.Modules;

namespace PonteAlta.Persistencia.Persistencia
{
    public class ModuloVIX : NinjectModule
    {
        public override void Load()
        {
            base.Bind<IUnidadeTrabalho>().To<UnidadeTrabalho>();
        }
    }
}
