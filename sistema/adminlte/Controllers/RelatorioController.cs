using Estrutura;
using PonteAlta.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminlte.Controllers
{
    public class RelatorioController : Controller
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public RelatorioController()
        {
            IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            UnidadeTrabalho = unidadeTrabalho;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GeraRelatorio(string codPedido, string relatorio)
        {
            int codEmpresa = 1;//Convert.ToInt32(Session["empAti"]);
            int codFilial = 3;//Convert.ToInt32(Session["filAti"]);
            int pedido = Convert.ToInt32(codPedido);

            GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient ws = new GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient();

            if (ws == null)
                throw new Exception("Não foi possível realizar uma conexão com o serviço de autenticação.");

            GravarPedido.pedidosvendaRelatoriosIn request = new GravarPedido.pedidosvendaRelatoriosIn();

            request.numPed = pedido;
            request.numPedSpecified = true;

            request.codEmpSpecified = true;
            request.codEmp = codEmpresa;

            request.codFilSpecified = true;
            request.codFil = codFilial;
            request.codMdr = relatorio;

            try
            {
                string usuarioLogado = Session["usuarioLogado"].ToString();
                string senhaUsuarioLogado = Session["senhaUsuLogado"].ToString();
                var Resultado = ws.Relatorios(usuarioLogado, senhaUsuarioLogado, 0, request);

                if(!String.IsNullOrEmpty(Resultado.retornoBase64))
                {
                    byte[] bytes = Convert.FromBase64String(Resultado.retornoBase64);

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(bytes);
                    Response.End();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
                //ViewData["Erro"] = e.Message;
            }

            return View("");
        }
    }
}