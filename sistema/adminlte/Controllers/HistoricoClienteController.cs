using Estrutura;
using PonteAlta.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminlte.Controllers
{
    public class HistoricoClienteController : Controller
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public HistoricoClienteController()
        {
            IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            UnidadeTrabalho = unidadeTrabalho;
        }

        // GET: HistoricoCliente
        public ActionResult Index()
        {
            return View();
        }
    }
}