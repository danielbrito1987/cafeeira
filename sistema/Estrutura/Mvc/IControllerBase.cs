using Estrutura.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Mvc
{
    public interface IControllerBase
    {
        IUnidadeTrabalho UnidadeTrabalho { get; set; }
    }
}
