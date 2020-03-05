using Estrutura;
using Estrutura.Mvc;
using PonteAlta.Infra;
using PonteAlta.Models;
using PonteAlta.Persistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminlte.Controllers
{
    [AllowAnonymous]
    [ExtendController]
    public class ConsultaProdutoController : Controller, IControllerBase
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public ConsultaProdutoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObterTodosTabelaPreco()
        {
            var tabPreco = UnidadeTrabalho.ObterTodos<TabelaPreco>();

            return Json(tabPreco.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterProduto(string codProduto, string descProduto, string codTpr)
        {
            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);

            List<ProdutoConsultar> resultadoConsulta = PesquisarProduto(codEmpFiltroPadrao, codFilialFiltroPadrao, codProduto, descProduto, codTpr);

            return Json(resultadoConsulta);
        }

        private List<ProdutoConsultar> PesquisarProduto(int codEmpFiltroPadrao, int codFilialFiltroPadrao, string codProduto, string descProduto, string codTpr)
        {
            string query = string.Format(@"DECLARE  @VCodEmp INTEGER = {0},
		                                             @VCodFil INTEGER = {1},
		                                             @VCodTpr VARCHAR(4) = '{2}',
		                                             @VCodPro VARCHAR(14) = '{3}',		 
		                                             @VDesPro VARCHAR(100) = '{4}'
                                            SELECT *
                                            FROM dbo.fnConsultaPreco(@VCodEmp,
						                                             @VCodFil,
						                                             @VCodTpr,
						                                             @VCodPro,									
						                                             @VDesPro);", codEmpFiltroPadrao,
                                                                                codFilialFiltroPadrao,
                                                                                codTpr,
                                                                                !string.IsNullOrEmpty(codProduto) ? removeAcentuacao(codProduto) : string.Empty,
                                                                                !string.IsNullOrEmpty(descProduto) ? removeAcentuacao(descProduto) : string.Empty);

            var resultado = UnidadeTrabalho.ExecuteSql<ProdutoConsultar>(query);

            return resultado.ToList();
        }

        private string removeAcentuacao(string palavra)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(palavra);
            return System.Text.Encoding.UTF8.GetString(bytes).ToUpper();
        }

        private bool IsNumeric(string dados)
        {
            bool isNumeric = false;
            decimal numero;

            if(decimal.TryParse(dados, out numero))
            {
                isNumeric = true;
            }

            return isNumeric;
        }
    }
}