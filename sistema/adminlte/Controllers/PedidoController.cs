using PonteAlta.Models;
using PonteAlta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estrutura;
using PonteAlta.Infra;
using Estrutura.Mvc;
using PonteAlta.Persistencia.Models;
using adminlte.Models;
using System.Web.Script.Serialization;
using System.Text;
using System.Globalization;
using RazorToPdf;

namespace adminlte.Controllers
{
    [AllowAnonymous]
    [ExtendController]
    public class PedidoController : Controller, IControllerBase
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public PedidoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
        }

        // GET: Pedido
        public ActionResult Index()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult NovoPedido()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Login", "Account");

            TempData["listItemPedido"] = new List<PedidoItem>();
            TempData["listaAvalista"] = new List<Cliente>();
            TempData["listaParcela"] = new List<Parcela>();

            return View();
        }

        public ActionResult FecharPedido(Pedido pedido)
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ImprimirOrcamento(string pedidoPost = null, bool isJson = false)
        {
            TempData.Keep("listItemPedido");
            TempData.Keep("listaParcela");
            TempData.Keep("listaAvalista");

            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            var listaAvalista = TempData["listaAvalista"] as List<GrupoEmpresa>;

            if (pedidoPost == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Não foi possível localizar o pedido a ser enviado, favor verificar."
                });
            }

            if (listItemPedido.Count <= 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Não foi possível localizar o pedido a ser enviado, favor verificar."
                });
            }
            //return Json(Utils.MensagemJson("Favor informar pelo menos um item no pedido.", Utils.JsonType.error));

            Pedido pedido = new JavaScriptSerializer().Deserialize<Pedido>(pedidoPost);
            CondicaoPagamento condPgto = UnidadeTrabalho.ObterTodos<CondicaoPagamento>().Where(x => x.Codigo == pedido.CodCondicaoPagamento).FirstOrDefault();
            //CondicaoPagamentoProc condPgto = ObterCondPagamento(pedido.CodCondicacaoPagamento != null ? pedido.CodCondicacaoPagamento : pedido.CondicaoPagamento.Codigo, pedido.FormaPagamento.Codigo.ToString(), false, pedido.CodigoConvenio);

            //pedido.CondicaoPagamento.Abreviacao = condPgto.ABRCPG;
            pedido.CodCondicaoPagamento = condPgto.Codigo;
            //pedido.CondicaoPagamento.CodigoCampanha = condPgto.CODCAM.ToString();
            pedido.DescCondicaoPagamento = condPgto.Descricao;
            //pedido.CondicaoPagamento.Empresa.Codigo = Convert.ToInt32(condPgto.CODEMP);
            //pedido.CondicaoPagamento.Filial.Codigo = Convert.ToInt32(condPgto.CODFIL);
            //pedido.CondicaoPagamento.Fonte = condPgto.FONTE;
            //pedido.CondicaoPagamento.FormaPagamento.Codigo = Convert.ToInt32(condPgto.CODFPG);
            //pedido.CondicaoPagamento.FormaPagamento.Descricao = condPgto.DESFPG;
            //pedido.DataIniJuros = condPgto.INIJUR;
            //pedido.CondicaoPagamento.ManutencaoParcela = condPgto.MANPAR;
            //pedido.CondicaoPagamento.PodSel = condPgto.PODSEL;
            //pedido.CondicaoPagamento.QuantidadeParcelas = Convert.ToInt32(condPgto.QTDPAR);
            //pedido.CondicaoPagamento.TaxaAcrescimoFinanceiro = Convert.ToDouble(condPgto.ACRFIN);
            //pedido.CondicaoPagamento.TaxaJuros = Convert.ToDouble(condPgto.TXAJUR);
            //pedido.CondicaoPagamento.TipoCalculo = condPgto.FORCAL;
            //pedido.CondicaoPagamento.TipoCPR = condPgto.CPGCPR;
            //pedido.CondicaoPagamento.TipoEspecial = condPgto.CPGESP;
            //pedido.CondicaoPagamento.TipoJuros = condPgto.TIPJUR;
            //pedido.CondicaoPagamento.TipoParcela = Convert.ToInt32(condPgto.TIPPAR);
            //pedido.CondicaoPagamento.VlrDis = Convert.ToDouble(condPgto.VLRDIS);
            //pedido.CondicaoPagamento.VlrMet = Convert.ToDouble(condPgto.VLRMET);
            //pedido.CondicaoPagamento.VlrVen = Convert.ToDouble(condPgto.VLRVEN);

            //pedido.CondicaoPagamento = condPgto;
            pedido.DataIniJuros = Convert.ToDateTime(pedido.DataIniJuros.ToString("dd/MM/yyyy"));

            if (pedido.TaxaJuros <= 0 && pedido.AcrFin <= 0)
                pedido.DataIniJuros = Convert.ToDateTime("31/12/1900");

            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);
            int codUsuarioLogado = Convert.ToInt32(Session["codUsu"]);
            string usuarioLogado = Session["usuarioLogado"].ToString();
            string senhaUsuarioLogado = Session["senhaUsuLogado"].ToString();



            if (pedido.CIFFOB == "C" && pedido.ValorFrete > 0)
            {
                double somaFrete = listItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorFrete);
                if (somaFrete <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Favor incluir ao menos um item no rateio do frete."
                    });
                }
            }

            //if ((pedido.CondicaoPagamento.TipoCPR == "T" || pedido.CondicaoPagamento.TipoCPR == "I") && (pedido.CodigoTPR == string.Empty || pedido.CodigoTPR == null))
            //    return Json(Utils.MensagemJson("Selecione uma Tabela CPR.", Utils.JsonType.error));

            //pedido.GrupoEmpresa = UnidadeTrabalho.ObterPorId<GrupoEmpresa>(pedido.GrupoEmpresa.Codigo);
            //pedido.Cliente = UnidadeTrabalho.ObterPorId<Cliente>(pedido.Cliente.Codigo);
            //pedido.Representante = UnidadeTrabalho.ObterTodos<Representante>().FirstOrDefault(x => x.Codigo == pedido.Representante.Codigo);
            pedido.CodTransacao = pedido.CodTransacao; //UnidadeTrabalho.ObterPorId<Transacao>(pedido.Transacao.Codigo);
            pedido.CodFormaPagamento = pedido.CodFormaPagamento; //UnidadeTrabalho.ObterPorId<FormaPagamento>(pedido.FormaPagamento.Codigo);
            pedido.CodCondicaoPagamento = pedido.CodCondicaoPagamento; //UnidadeTrabalho.ObterTodos<CondicaoPagamento>().FirstOrDefault(x => x.Codigo == pedido.CondicaoPagamento.Codigo);
            //pedido.TabelaCPR = pedido.TabelaCPR != "" ? pedido.NomeTPR : "";
            pedido.NomeTransportadora = Convert.ToInt32(pedido.CodigoTransportadora) > 0 ? UnidadeTrabalho.ObterTodos<Transportadora>().FirstOrDefault(x => x.Codigo == Convert.ToInt32(pedido.CodigoTransportadora)).Nome : string.Empty;
            ViewBag.Pedido = pedido;

            var cliente = UnidadeTrabalho.ObterTodos<Cliente>().Where(x => x.Codigo == pedido.Codigo && x.CodGrupo == pedido.CodGrupoEmpresa).FirstOrDefault();

            ViewBag.Cliente = cliente;

            ViewBag.Itens = listItemPedido;

            var listaParcelas = TempData["listaParcela"] as List<Parcela>;

            ViewBag.Parcelas = listaParcelas.OrderBy(x => x.SequenciaParcela).ToList();

            if (isJson)
            {
                return Json(new
                {
                    success = true,
                    message = "OK"
                });
            }

            return new PdfActionResult("ImprimirOrcamento", (writer, document) =>
            {
                //document.SetPageSize(new iTextSharp.text.Rectangle(500f, 500f, 90));
                document.SetMargins(10, 10, 10, 10);
                document.NewPage();
            });
        }

        // GET: Pedido/Details/5
        public ActionResult ObterPedido(int codigoPedido)
        {
            Session["ObterProdutoUnico"] = null;
            Session["listItemPedidoComparacao"] = null;
            Session["demaisItensComparacao"] = null;
            TempData.Keep();

            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);
            double totalLiquido = 0;

            if (codigoPedido <= 0)
                throw new Exception("Não foi possível localizar o código de referência.");

            var resultado = UnidadeTrabalho.ObterTodos<Pedido>().Where(x => x.CodFilial == codFilialFiltroPadrao && x.Codigo == codigoPedido);
            resultado = resultado.Where(x => x.CodEmpresa == codEmpFiltroPadrao);

            Pedido pedido = resultado.FirstOrDefault();

            //Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            //parameters.Add("codEmpresa", codEmpFiltroPadrao);
            //parameters.Add("codFilial", codFilialFiltroPadrao);
            //parameters.Add("codPedido", codigoPedido);

            //var resultado = UnidadeTrabalho.ExecuteSql<ConsultaPedido>(@"SELECT * FROM TABLE(FUNC_PEDIDO_DGERAIS(:codEmpresa, :codFilial, :codPedido))", parameters);

            //ConsultaPedido consultPedido = resultado.FirstOrDefault();
            //Pedido pedido = ConverterParaPedido(consultPedido);

            if (pedido != null && pedido.Codigo > 0)
            {
                TempData["Pedido"] = pedido;
                TempData.Keep();

                //Obter Procuradores
                //ObterProcuradores(pedido.GrupoEmpresa.Codigo.ToString());

                //Obter Avalistas
                //ObterTodosAvalista(pedido.Codigo);

                //CondicaoPagamentoProc condicao = ObterCondPagamento(pedido.CodCondicaoPagamento.Codigo.ToString(), pedido.FormaPagamento.Codigo.ToString(), false);
                //CondicaoPagamentoProc condicao = ObterCondPagamento(pedido.CodCondicacaoPagamento, pedido.FormaPagamento.Codigo.ToString(), false, pedido.CodigoConvenio);
                //CondicaoPagamentoProc condicao = new CondicaoPagamentoProc();

                //Obter Parcelas
                //pedido.CondicaoPagamento = UnidadeTrabalho.ObterTodos<CondicaoPagamento>().Where(x => x.Codigo == pedido.CodCondicacaoPagamento
                //                                                                                    && x.Filial.Codigo == pedido.Filial.Codigo
                //                                                                                    && x.FormaPagamento.Codigo == pedido.FormaPagamento.Codigo).FirstOrDefault();

                //if(condicao.CPGESP == "S")                
                //condicao = ObterCondPagamento(pedido.CodCondicacaoPagamento, pedido.FormaPagamento.Codigo.ToString(), false, pedido.CodigoConvenio);

                //if (condicao != null && condicao.CPGCPR != "I")
                //    ObterParcelasCPR(pedido.CodCondicacaoPagamento, pedido.CodigoConvenio, pedido.DataEmissao.ToShortDateString(), condicao.CPGCPR, pedido.TabelaCPR);
                //else if (condicao != null && condicao.CPGESP == "S")
                //    ObterTodasParcelas(pedido.Codigo);

                //if (condicao != null)
                //{
                //    if (condicao.CPGESP == "S")
                //        ObterTodasParcelas(pedido.Codigo);
                //}
                //else
                //{
                //    throw new CoreException("Não foi possível localizar a condição de pagamento " + pedido.CodCondicacaoPagamento + ".");
                //}

                //Obter Itens do Pedido
                //ObterItensPedido(pedido.Codigo);

                //Somente ocorre recalculo dos itens quando o pedido está com situação Não Fechado
                if (pedido.SituacaoPedido == 9)
                {
                    //if (condicao != null)
                    //{
                    //    //if (condicao.CPGESP == "S")
                    //    //    ObterTodasParcelas(pedido.Codigo);

                    //    string tipoCalculo = condicao.CPGESP == "S" ? "Juros" : "Base";
                    //}
                    //if (condicao.CPGESP == "S" || condicao.ACRFIN > 0 || condicao.TXAJUR > 0)
                    //{
                    //Recalculo de itens
                    //string tipoCalculo = condicao.CPGESP == "S" ? "Juros" : "Base";

                    //RecalculaItens(tipoCalculo, condicao.CPGESP, condicao.TIPJUR == "N" ? (pedido.AcrFin > 0 ? pedido.AcrFin.ToString() : condicao.ACRFIN.ToString()) : (pedido.TaxaJuros > 0 ? pedido.TaxaJuros.ToString() : condicao.TXAJUR.ToString()), condicao.QTDPAR.ToString(), null, null, condicao.CPGCPR,
                    //    pedido.TabelaCPR, pedido.DataEmissao.ToString("dd/MM/yyyy"), condicao.CODCPG, pedido.Cliente.Codigo.ToString(), pedido.Transacao.Codigo.ToString(),
                    //    null, pedido.DataIniJuros != null ? pedido.DataIniJuros.ToString("dd/MM/yyyy") : (condicao.INIJUR.Year > 1900 ? condicao.INIJUR.ToString("dd/MM/yyyy") : DateTime.Now.ToString()), pedido.CodRepresentante.ToString());
                    //}

                    /*
                    if (pedido.ValorFrete == 0)
                        HabilitarFreteParaTodosItens();
                        */
                    //var formaPgto = UnidadeTrabalho.ObterPorId<FormaPagamento>(int.Parse(pedido.CODFPG.ToString()));
                    //pedido.FPGCPR = formaPgto.TipoCPR;

                    var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;

                    foreach (PedidoItem itemPedido in listItemPedido)
                    {
                        totalLiquido += Math.Round((itemPedido.ValorLiquido * 100), 2) / 100;
                    }

                }
                else
                    ViewData["KeepOpened"] = false;

                List<Parcela> listaParcelas = new List<Parcela>();

                if (TempData["listaParcela"] != null)
                    listaParcelas = TempData["listaParcela"] as List<Parcela>;

                var l = resultado.FirstOrDefault();
                var resultadoPedido = new
                {
                    Codigo = (l.Codigo > 0) ? l.Codigo : 0,
                    SituacaoCodigo = (l.SituacaoPedido > 0) ? l.SituacaoPedido : 0,
                    Situacao = l.DescSituacaoPedido,
                    //ObsAprovacao = !string.IsNullOrEmpty(l.OBSMOT) ? l.OBSMOT : "",

                    DtEmissao = (l.DataEmissao != null) ? l.DataEmissao.ToString("dd/MM/yyyy") : "",
                    HrEmissao = TimeSpan.FromMinutes(Double.Parse(l.HoraEmissao.ToString())).ToString("hh':'mm"),

                    //CodigoConvenio = (l.CODCON > 0) ? l.CODCON : 0,
                    //NomeConvenio = (pedido.NomeConvenio != null) ? pedido.NomeConvenio.ToString() : "",

                    //CodigoTPR = (l.CODCPR != null) ? l.CODCPR.ToString() : "",
                    //NomeTPR = (pedido.NomeTPR != null) ? pedido.NomeTPR.ToString() : "",

                    CodigoTransportadora = (l.CodigoTransportadora != null) ? l.CodigoTransportadora.ToString() : "",
                    NomeTransportadora = (pedido.NomeTransportadora != null) ? pedido.NomeTransportadora.ToString() : "",
                    NomePlaca = (l.NomePlaca != null) ? l.NomePlaca.ToString() : "",
                    CIFFOB = (l.CIFFOB != null) ? l.CIFFOB.ToString() : "",
                    ValorFrete = (l.ValorFrete > 0) ? l.ValorFrete.ToString("N2") : "",
                    ObsPedido = (l.ObsPedido != null) ? l.ObsPedido.ToString() : "",
                    ObsEntrega = (l.ObsEntrega != null) ? l.ObsEntrega.ToString() : "",
                    //QtdSaca = (l.QTDSAC > 0) ? l.QTDSAC.ToString() : "",
                    //Filial = (l.Filial != null) ? l.Filial.Nome.ToString() : "",
                    FilialCodigo = (l.CodFilial > 0) ? l.CodFilial.ToString() : "",

                    CondicaoPagamentoDescricao = l.DescCondicaoPagamento,
                    CondicaoPagamentoCodigo = l.CodCondicaoPagamento,
                    //CondicaoPagamentoTipoCPR = (condicao != null && condicao.CPGCPR != null) ? condicao.CPGCPR : "N",
                    //CondicaoPagamentoEspecial = (condicao != null && condicao.CPGESP != null) ? condicao.CPGESP : "N",
                    FormaPagamentoDescricao = (l.DescFormaPagamento != null) ? l.DescFormaPagamento : "",
                    FormaPagamentoCodigo = (l.CodFormaPagamento > 0) ? l.CodFormaPagamento : 0,
                    //FormaPagamentoTipoCPR = (pedido.FormaPagamento.TipoCPR != null) ? pedido.FormaPagamento.TipoCPR.ToString() : "N",

                    TransacaoDescricao = (l.DescTransacao != null) ? l.DescTransacao : "",
                    TransacaoCodigo = (l.CodTransacao > 0) ? l.CodTransacao.ToString() : "",
                    //TransacaoCodigoTipoCPR = (pedido.Transacao != null) ? pedido.Transacao.TipoCPR.ToString() : "N",
                    //CodMdr = l.CODMDR,

                    //EmpresaDescricao = (l.Empresa != null) ? l.Empresa.Descricao.ToString() : "",
                    EmpresaCodigo = (l.CodEmpresa > 0) ? l.CodEmpresa.ToString() : "",

                    GrupoEmpresaDescricao = (l.NomeCliente != null) ? l.NomeCliente.ToString() : "",
                    GrupoEmpresaCodigo = l.CodGrupoEmpresa.ToString(),

                    ClienteNome = (l.NomeCliente != null) ? l.NomeCliente.ToString() : "",
                    ClienteCodigo = (l.CodCliente > 0) ? l.CodCliente.ToString() : "",

                    RepresentanteNome = l.NomeRepresentante.ToString(),
                    RepresentanteCodigo = l.CodRepresentante.ToString(),

                    ValorBruto = l.ValorBruto,
                    ValorDesconto = l.ValorDesconto,
                    ValorAcrescimo = l.ValorAcrescimo,
                    ValorLiquido = l.ValorLiquido,
                    //QtdSac = (l.QTDSAC > 0) ? Convert.ToDouble(l.QTDSAC).ToString("N2") : "",
                    //TabelaCPR = (l.CODCPR != null) ? l.CODCPR.ToString() : "",
                    PodeAlterar = (l.PodeAlterar != null) ? l.PodeAlterar.ToString() : "",
                    //RefreshParcelas = (pedido.CondicaoPagamento != null && pedido.CondicaoPagamento.TipoEspecial == "S" && (listaParcelas.Count > 0)) ? "S" : "N",
                    PedBlo = l.BloqueioPedido,

                    //NumeroOrdemCompra = l.NUMOCP,
                    //FilialOrdemCompra = l.FILOCP,

                    AcrFin = l.AcrFin.ToString(),
                    TaxaJuros = l.TaxaJuros.ToString(),
                    DataIniJur = l.DataIniJuros.ToString(),

                    //QtdParcelas = condicao != null ? condicao.QTDPAR : 0,
                    //ManutencaoParcelas = condicao != null ? condicao.MANPAR : "N",
                    //RepRec = l.REPREC != null ? l.REPREC : "N",
                    //Arras = TempData["Arras"]

                };

                return Json(resultadoPedido);
            }
            else
                return Json(null);
        }

        public ActionResult ObterTodasParcelas(int codPedido)
        {
            TempData.Keep();

            CultureInfo cultura = CultureInfo.GetCultureInfo("pt-BR");
            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);
            var resultado = UnidadeTrabalho.ObterTodos<Parcela>().Where(x => x.Empresa == codEmpFiltroPadrao && x.Filial == codFilialFiltroPadrao && x.Pedido.Codigo == codPedido).OrderBy(x => x.SequenciaParcela);

            Pedido pedido = (Pedido)TempData["Pedido"];
            if (pedido != null)
                ViewData["showNewButton"] = pedido.SituacaoPedido == 9 ? true : false;

            TempData["listaParcela"] = resultado.ToList();

            return Json(resultado.ToList());
        }

        public ActionResult ObterTodosPedidos(int? pesquisar_pedido_codigo, string pesquisar_pedido)
        {

            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);

            var resultado = UnidadeTrabalho.ObterTodos<Pedido>().Where(x => x.CodEmpresa == codEmpFiltroPadrao
                                                                            && x.CodFilial == codFilialFiltroPadrao);

            List<Pedido> list = null;
            if (pesquisar_pedido_codigo > 0 || pesquisar_pedido != "")
            {
                if (pesquisar_pedido_codigo > 0)
                    resultado = resultado.Where(x => x.Codigo == pesquisar_pedido_codigo);

                if (pesquisar_pedido != "")
                {
                    pesquisar_pedido = removeAcentuacao(pesquisar_pedido);
                    resultado = resultado.Where(x => x.NomeCliente.Contains(pesquisar_pedido) || x.NomeCliente.Contains(pesquisar_pedido));
                }

                list = resultado.OrderBy(x => x.DataEmissao).ToList();
            }

            TempData["PedidoConsulta"] = list;
            TempData.Keep("PedidoConsulta");

            return Json(list);
        }

        public ActionResult ObterTodosClientes(string pesquisa)
        {
            if (pesquisa.Length < 3)
            {
                return Json(new
                {
                    success = false,
                    message = "É necessário pelo menos 3 caractéres para pesquisar."
                });
            }

            List<Cliente> lista = new List<Cliente>();

            var clientes = UnidadeTrabalho.ObterTodos<Cliente>();

            if (!String.IsNullOrEmpty(pesquisa))
            {
                clientes = clientes.Where(x => x.Nome.Contains(pesquisa));
            }

            lista = clientes.ToList();

            //return PartialView("GridModalCliente", lista);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterTodosGrupos()
        {
            var grupos = UnidadeTrabalho.ObterTodos<Grupo>();

            return Json(grupos.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterTodosRepresentantes()
        {
            var rep = UnidadeTrabalho.ObterTodos<Representante>();

            return Json(rep.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterTodosTransportadora()
        {
            var transp = UnidadeTrabalho.ObterTodos<Transportadora>();

            return Json(transp.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterTodasTransacoes()
        {
            var trans = UnidadeTrabalho.ObterTodos<Transacao>();

            return Json(trans.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterTodosFormaPgto()
        {
            var forma = UnidadeTrabalho.ObterTodos<FormaPagamento>();

            return Json(forma.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterTodosCondicaoPgto(string formaPgto)
        {
            var condPgto = UnidadeTrabalho.ObterTodos<CondicaoPagamento>();
            condPgto = condPgto.Where(x => x.FormaPagamento.Codigo == formaPgto);

            return Json(condPgto.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterVeiculos(int codTransportadora)
        {
            var veiculos = UnidadeTrabalho.ObterTodos<Placa>().Where(x => x.CodTransportadora == codTransportadora).ToList();

            return Json(veiculos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterAvalista(int Codigo)
        {
            TempData.Keep();

            List<Cliente> lista = TempData["listaAvalista"] != null ? TempData["listaAvalista"] as List<Cliente> : new List<Cliente>();

            Cliente cliente = UnidadeTrabalho.ObterTodos<Cliente>().Where(x => x.Codigo == Codigo).FirstOrDefault();

            lista.Add(cliente);

            TempData["listaAvalista"] = lista;

            return Json(lista);
        }

        public ActionResult RemoverAvalista(string Codigo)
        {
            var listaAvalista = TempData["listaAvalista"] as List<GrupoEmpresa>;
            listaAvalista.Remove(listaAvalista.Where(x => x.Codigo == Convert.ToInt32(Codigo)).First());

            TempData["listaAvalista"] = listaAvalista;

            return Json(listaAvalista);
        }

        public JsonResult ObterTodosAvalista()
        {
            return Json(null);
        }

        public ActionResult ObterCliente(int Codigo)
        {
            Cliente cli = new Cliente();

            cli = UnidadeTrabalho.ObterTodos<Cliente>().Where(x => x.Codigo == Codigo).FirstOrDefault();

            var grupo = UnidadeTrabalho.ObterTodos<Grupo>().Where(x => x.Codigo == cli.CodGrupo).FirstOrDefault();

            return Json(new
            {
                cli.Codigo,
                cli.Nome,
                NomeGrupo = grupo != null ? grupo.Nome : "",
                CodGrupo = grupo != null ? grupo.Codigo : 0
            });
        }

        public ActionResult ObterGrupo(int Codigo)
        {
            var grupo = UnidadeTrabalho.ObterTodos<Grupo>().Where(x => x.Codigo == Codigo).FirstOrDefault();
            var cliente = UnidadeTrabalho.ObterTodos<Cliente>().Where(x => x.CodGrupo == grupo.Codigo).FirstOrDefault();

            return Json(new
            {
                cliente.Codigo,
                cliente.Nome,
                NomeGrupo = grupo != null ? grupo.Nome : "",
                CodGrupo = grupo != null ? grupo.Codigo : 0
            });
        }

        public ActionResult ObterRepresentante(int Codigo)
        {
            var rep = UnidadeTrabalho.ObterTodos<Representante>().Where(x => x.Codigo == Codigo).FirstOrDefault();

            return Json(new
            {
                rep.Codigo,
                rep.Nome
            });
        }

        public ActionResult ObterTransportadora(int Codigo)
        {
            var transp = UnidadeTrabalho.ObterPorId<Transportadora>(Codigo);

            return Json(new
            {
                transp.Codigo,
                transp.Nome
            });
        }

        public ActionResult ObterTransacao(string Codigo)
        {
            var trans = UnidadeTrabalho.ObterTodos<Transacao>().Where(x => x.Codigo == Codigo).FirstOrDefault();

            return Json(new
            {
                trans.Codigo,
                trans.Descricao
            });
        }

        public ActionResult ObterFormaPgto(string Codigo)
        {
            var forma = UnidadeTrabalho.ObterTodos<FormaPagamento>().Where(x => x.Codigo == Codigo).FirstOrDefault();

            return Json(new
            {
                forma.Codigo,
                forma.Descricao,
                forma.Abreviacao
            });
        }

        public ActionResult ObterCondicaoPgto(string codFiltro, string codFormaPagamento)
        {
            decimal TaxaJuros = 0;

            var resultado = UnidadeTrabalho.ObterTodos<CondicaoPagamento>().Where(x => x.Codigo == codFiltro && x.FormaPagamento.Codigo == codFormaPagamento).FirstOrDefault();

            if (resultado != null)
            {
                if (resultado.TipoJuros == "N")
                {
                    TaxaJuros = (resultado.TaxaAcrescimoFinanceiro > 0) ? resultado.TaxaAcrescimoFinanceiro : 0;
                }
                else
                {
                    TaxaJuros = (resultado.TaxaJuros > 0) ? resultado.TaxaJuros : 0;
                }

                return Json(new
                {
                    resultado.Codigo,
                    Descricao = resultado.Descricao != null ? resultado.Descricao : "",
                    Abreviacao = resultado.Abreviacao != null ? resultado.Abreviacao : "",
                    resultado.TipoEspecial,
                    resultado.ManutencaoParcela,
                    resultado.QuantidadeParcelas,
                    TaxaJuros
                });
            }
            else
            {
                return Json(new
                {
                    retorno = "A Condição de Pagamento " + codFiltro + " não está disponível."
                });
            }
        }

        public ActionResult ObterVeiculo(string codFiltro, int codigoTransportadora)
        {
            var veiculo = UnidadeTrabalho.ObterTodos<Placa>().Where(x => x.PlacaVeiculo == codFiltro && x.CodTransportadora == codigoTransportadora).FirstOrDefault();

            if(veiculo != null)
            {
                return Json(new
                {
                    veiculo.CodTransportadora,
                    veiculo.PlacaVeiculo
                });
            } else
            {
                return Json(new
                {
                    CodTransportadora = codigoTransportadora,
                    PlacaVeiculo = codFiltro
                });
            }
        }

        public ActionResult IncluirItem(string produto, string dataEmissao, string dataIniJur)
        {
            TempData.Keep();

            PedidoItem product = new JavaScriptSerializer().Deserialize<PedidoItem>(produto);
            DateTime entrega = Convert.ToDateTime(product.DataEntrega);

            product.DataEntrega = entrega.ToShortDateString();

            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;

            if (listItemPedido == null)
                listItemPedido = new List<PedidoItem>();

            if (Convert.ToDateTime(dataIniJur) < Convert.ToDateTime(dataEmissao))
                dataIniJur = dataEmissao;

            if (listItemPedido.Count > 0)
            {
                foreach (var item in listItemPedido)
                {
                    //item.DataEntrega = entrega.AddDays(1).ToShortDateString();

                    if (item.CodigoProduto == product.CodigoProduto)
                    {
                        return Json(new
                        {
                            Error = true,
                            Message = "Este produto já foi inserido."
                        });
                    }

                }
            }

            listItemPedido.Add(product);

            //if (ModelState.IsValid)
            //{
            //    ValidaItemPedido validaItem = ValidarItem(product, condPgto, dataEmissao, transProd, codCliente,
            //    codGrupo, codRepres, codForma, Convert.ToInt32(numero_pedido), Convert.ToInt32(codigo_convenio), Convert.ToDecimal(valor_desconto));

            //    if (validaItem.BLOITE == "S")
            //    {
            //        ViewData[EditResultKey_ItemPedido] = validaItem.BLOMSG;
            //        throw new Exception(validaItem.BLOMSG);
            //    }
            //    else
            //    {
            //        ViewData[EditResultKey_ItemPedido] = validaItem.BLOMSG;
            //        listItemPedido.Add(product);

            //        CalcularRateioFrete(vlFrete);
            //    }
            //}
            //else
            //    ViewData["EditError"] = "Por favor, corrija todos os erros.";

            TempData["listItemPedido"] = listItemPedido;

            return Json(listItemPedido);
        }

        public ActionResult EditarItem(string produto, string dataEmissao, string dataIniJur)
        {
            TempData.Keep();

            PedidoItem product = new JavaScriptSerializer().Deserialize<PedidoItem>(produto);

            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;

            if (Convert.ToDateTime(dataIniJur) < Convert.ToDateTime(dataEmissao))
                dataIniJur = dataEmissao;

            if (ModelState.IsValid)
            {
                int removido = listItemPedido.RemoveAll(x => x.Sequencia == product.Sequencia && x.CodigoProduto == product.CodigoProduto);
                if (removido == 1)
                {
                    listItemPedido.Add(product);
                    TempData["listItemPedido"] = listItemPedido;

                    //if (vlFrete > 0)
                    //    CalcularRateioFrete(vlFrete);
                }
            }

            return Json(listItemPedido);
        }

        public JsonResult RemoverItem(string CodigoProduto, double vlFrete)
        {
            TempData.Keep();

            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;

            int removido = 0;

            removido = listItemPedido.RemoveAll(x => x.CodigoProduto == CodigoProduto);

            if (removido == 1)
                ViewData["EditError"] = "Item removido com sucesso.";

            TempData["listItemPedido"] = listItemPedido;
            CalcularRateioFrete(vlFrete);

            return Json(listItemPedido);
        }

        public ActionResult IncluirParcela(string parcela, string EmissaoPedido, string PermiteManutencao)
        {
            TempData.Keep("listaParcela");

            Parcela parc = new JavaScriptSerializer().Deserialize<Parcela>(parcela);

            //var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            var listaParcela = TempData["listaParcela"] as List<Parcela>;

            double somaParcelas = 0;

            try
            {
                foreach (var item in listaParcela)
                {
                    if (item.DataVencimento == parc.DataVencimento)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = "Já existe uma parcela com a data de vencimento informada."
                        });
                    }
                }

                if (Convert.ToDateTime(parc.DataVencimento) < Convert.ToDateTime(EmissaoPedido))
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "O vencimento da parcela não pode ser anterior à data do pedido."
                    });
                }

                if (PermiteManutencao == "N")
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Manutenção de parcela não permitido."
                    });
                }
                else if (parc.Percentual > 100)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Não é permitido inserir uma parcela maior que o valor total do pedido."
                    });
                }
                else
                {
                    if (listaParcela != null)
                    {
                        if (listaParcela.Count <= 0)
                            listaParcela = new List<Parcela>();
                    }
                    else
                        listaParcela = new List<Parcela>();

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            if (parc.DataVencimento != null && parc.Percentual > 0 && parc.ValorParcela > 0)
                            {
                                parc.SequenciaParcela = listaParcela.Count + 1;
                                listaParcela.Add(parc);
                            }
                            else
                                throw new Exception("Existem campos obrigatório em branco!");
                        }
                        catch (Exception e)
                        {
                            return Json(new
                            {
                                Success = false,
                                Message = e.Message
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = "Existem campos obrigatórios em branco!"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }

            TempData.Keep("listaParcela");
            TempData["listaParcela"] = listaParcela;

            return Json(listaParcela);
        }

        public ActionResult RemoveParcela(string SequenciaParcela)
        {
            TempData.Keep();

            var listaParcela = TempData["listaParcela"] as List<Parcela>;
            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;

            try
            {
                if (listItemPedido.Count <= 0)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Não é possível excluir uma parcela sem itens no pedido."
                    });
                }

                listaParcela.Remove(listaParcela.Where(x => x.SequenciaParcela == Convert.ToInt32(SequenciaParcela)).First());

                TempData["listaParcela"] = listaParcela;
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }

            return Json(listaParcela);
        }

        public ActionResult RemoverTodasParcelas()
        {
            TempData.Keep();

            var listaParcela = new List<Parcela>();

            TempData["listaParcela"] = listaParcela;

            return Json(listaParcela);
        }

        private void CalcularRateioFrete(double valorFrete)
        {
            TempData.Keep();
            var listaItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            listaItemPedido = listaItemPedido == null ? new List<PedidoItem>() : listaItemPedido;
            //double valorLiquidoPedido = listaItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorLiquido);
            double valorBase = Math.Round(listaItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorBase), 2);
            double valorDesconto = Math.Round(listaItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorDescontoUsuario), 2);
            double valorAcrescimo = Math.Round(listaItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorAcrescimoUsuario), 2);
            double valorLiquidoPedido = valorBase + valorDesconto - valorAcrescimo;

            foreach (var item in listaItemPedido)
            {
                if (item.RateioFrete)
                {
                    double valorLiq = (item.PrecoBase * item.QuantidadePedido) - item.ValorAcrescimoUsuario + item.ValorDescontoUsuario;
                    double porcentagemFrete = Math.Round(((100 * valorLiq) / valorLiquidoPedido), 2);
                    item.ValorFrete = Math.Round(valorFrete * (porcentagemFrete / 100), 2);
                }
                else
                {
                    item.ValorFrete = 0;
                }

                TempData["listItemPedido"] = listaItemPedido;

            }

            double somaFrete = Math.Round(listaItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorFrete), 2);
            double dif = Math.Round(somaFrete - valorFrete, 2);
            if (dif != 0)
            {
                PedidoItem ultItem = listaItemPedido.Where(x => x.RateioFrete).LastOrDefault();
                if (ultItem != null)
                {
                    ultItem.ValorFrete = Math.Round(ultItem.ValorFrete - dif, 2);
                    //if (dif > 0)
                    //    ultItem.ValorFrete = ultItem.ValorFrete - dif;
                    //else
                    //    ultItem.ValorFrete = ultItem.ValorFrete + (dif * -1);
                }
            }

        }

        public ActionResult RecalculaItens(string tipoCalculo, string tipo_especial, string taxa_juros, string total_parcelas, string agComercial = null, string agEstoque = null, string codTabela = null, string dataEmissao = null, string condPgto = null, string codCli = null, string transProd = null, string codDerivacao = null, string dataIniJur = null, string codRepresentante = null, double vlrDescontoTotal = 0, double vlrArredondamento = 0, double vlrLiquido = 0)
        {
            TempData.Keep();

            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            var listaParcela = TempData["listaParcela"] as List<Parcela>;
            ConsultaProduto prod = new ConsultaProduto();
            PedidoItem itemNovo = new PedidoItem();

            DateTime dataVencimento = DateTime.MinValue;
            DateTime dataPedido = Convert.ToDateTime(dataEmissao);

            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);

            if (Convert.ToDateTime(dataIniJur) < Convert.ToDateTime(dataEmissao))
                dataIniJur = dataEmissao;

            //if (string.IsNullOrEmpty(taxa_juros))
            //{
            //    ConsultaParcelasCondPgto consulta = ObterObjetoParcelasTipoCPR(condPgto, null, dataEmissao, tipo_cpr, codTabela);

            //    if (consulta != null)
            //        taxa_juros = consulta.TXAFIN.ToString();
            //}

            if (taxa_juros != "" && Convert.ToDouble(taxa_juros) >= 0)
            {
                if (tipoCalculo != "")
                {
                    var somaFrete = listItemPedido != null ? listItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorFrete) : 0;
                    var jurosFrete = 0.00;

                    if (listItemPedido != null && listItemPedido.Count > 0)
                    {
                        List<PedidoItem> itensRemover = new List<PedidoItem>();

                        if (somaFrete > 0)
                            jurosFrete = CalcularJurosFrete(listaParcela, tipoCalculo, tipo_especial, taxa_juros, total_parcelas, somaFrete, dataIniJur);

                        //var produtosUnicos = ObterProdutoUnico(listItemPedido, tipo_especial, taxa_juros, total_parcelas, agComercial, agEstoque, codTabela, dataEmissao, condPgto, codCli, transProd, codRepresentante);

                        foreach (PedidoItem itemPedido in listItemPedido)
                        {
                            var percentual = vlrLiquido > 0 ? (vlrDescontoTotal * 100) / vlrLiquido : 0;
                            var percentAcrescimo = vlrLiquido > 0 ? (vlrArredondamento * 100) / vlrLiquido : 0;
                            var valorLiquidoItem = itemPedido.PrecoUnitario * itemPedido.QuantidadePedido;

                            itemPedido.ValorDescontoUsuario = percentual > 0 ? valorLiquidoItem * (percentual / 100) : 0;
                            itemPedido.ValorAcrescimoUsuario = percentAcrescimo > 0 ? valorLiquidoItem * (percentAcrescimo / 100) : 0;

                            if(itemPedido.ValorDescontoUsuario > 0)
                            {
                                string hora = "";
                                string minutos = "";
                                string segundo = "";

                                hora = DateTime.Now.Hour < 10 ? string.Format("0{0}", DateTime.Now.Hour) : DateTime.Now.Hour.ToString();
                                minutos = DateTime.Now.Minute < 10 ? string.Format("0{0}", DateTime.Now.Minute) : DateTime.Now.Minute.ToString();
                                segundo = DateTime.Now.Second < 10 ? string.Format("0{0}", DateTime.Now.Second) : DateTime.Now.Second.ToString();

                                itemPedido.HoraDesconto = hora + ":" + minutos + ":" + segundo;
                            }
                            
                            //itemPedido.ValorDescontoUsuario = itemPedido.ValorDescontoUsuario + (vlrDescontoTotal / listItemPedido.Count);
                            //itemPedido.ValorAcrescimoUsuario = itemPedido.ValorAcrescimoUsuario + (vlrArredondamento / listItemPedido.Count);
                            //itemPedido.ValorLiquido = (itemPedido.PrecoUnitario * itemPedido.QuantidadePedido) + (vlrArredondamento / listItemPedido.Count) - (vlrDescontoTotal / listItemPedido.Count);
                            //prod = produtosUnicos.Where(x => x.Codpro == itemPedido.CodigoProduto && x.Codder == itemPedido.CodigoDerivacao).FirstOrDefault();
                            //if (prod != null && (prod.Prebas != Convert.ToDecimal(itemPedido.PrecoBase)))
                            //    itemNovo = ConverteListaParaPedidoItem(prod, tipo_especial, taxa_juros, total_parcelas, codEmpFiltroPadrao, codFilialFiltroPadrao);
                            //else if (prod == null)
                            //    itensRemover.Add(itemPedido);
                            //else
                            itemNovo = itemPedido;

                            //if (itemNovo.PrecoBase != itemPedido.PrecoBase && tipoCalculo != "A")
                            CalculaItemIndividual(itemPedido, itemNovo, itensRemover, listaParcela, tipoCalculo, tipo_especial, taxa_juros, total_parcelas, jurosFrete, somaFrete, dataIniJur);
                        }
                    }

                }
                else
                {
                    return Json(new
                    {
                        Error = true,
                        Message = "Não foi possível reacalcular os itens do pedido. Selecione novamente a condição de pagamento!"
                    });
                }
            }

            TempData["listItemPedido"] = listItemPedido;
            return Json(listItemPedido);
        }

        public ActionResult AjustaParcelas(double valorLiquidoPedido, double valorTotalParcelas, double percentualTotalParcelas)
        {
            TempData.Keep();
            var listaParcela = TempData["listaParcela"] as List<Parcela>;

            if (valorTotalParcelas != valorLiquidoPedido && valorTotalParcelas != 0 && valorLiquidoPedido != 0)
            {
                double diferenca = 0;

                if (valorLiquidoPedido > valorTotalParcelas)
                    diferenca = valorLiquidoPedido - valorTotalParcelas;
                else
                    diferenca = valorTotalParcelas - valorLiquidoPedido;

                foreach (var item in listaParcela)
                {
                    if (item.SequenciaParcela == (listaParcela.Count))
                    {
                        if(valorLiquidoPedido < valorTotalParcelas)
                            item.ValorParcela = item.ValorParcela - diferenca;
                        else
                            item.ValorParcela = item.ValorParcela + diferenca;
                    }
                }
            }

            TempData["listaParcela"] = listaParcela;

            return Json(listaParcela);
        }

        public ActionResult CalcularParcela(string valorLiquido, string dataVencimento)
        {
            TempData.Keep();
            var listaParcela = TempData["listaParcela"] as List<Parcela>;

            listaParcela = new List<Parcela>();
            Parcela parc = new Parcela();

            parc.SequenciaParcela = listaParcela.Count + 1;
            parc.Codigo = 0;
            parc.DataVencimento = dataVencimento;
            parc.Percentual = 100;
            parc.ValorParcela = Convert.ToDouble(valorLiquido);

            listaParcela.Add(parc);

            TempData["listaParcela"] = listaParcela;

            return Json(listaParcela);
        }

        public ActionResult ObterDataVencimento()
        {
            TempData.Keep("listaParcela");
            List<Parcela> lista = new List<Parcela>();
            string retorno = string.Empty;

            if (TempData["listaParcela"] != null)
                lista = TempData["listaParcela"] as List<Parcela>;

            if (lista.Count == 1)
            {
                foreach (var item in lista)
                {
                    if (item.DataVencimento == DateTime.Today.ToShortDateString() && item.Percentual == 100)
                    {
                        retorno = item.DataVencimento;
                        break;
                    }
                }
            }

            return Json(retorno);
        }

        public ActionResult RecalcularParcelas(double frete = 0, string TipoCondicaoPagamento = null, string ParcelasValidas = null, string DataVencimento = null, string ValorLiquido = null, string ValorLiquidoSemJuros = null, string ValorLiquidoComJuros = null, string TipoCalculoCondPgto = null, string ValorTotalPercentual = null)
        {
            TempData.Keep();
            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            var listaParcela = TempData["listaParcela"] as List<Parcela>;

            //double liquidoTotal = frete + listItemPedido.Sum(x => x.ValorLiquido);
            double liquidoTotal = frete + Convert.ToDouble(ValorLiquido.Replace('.', ','));
            string tipoCalculo = string.Empty;
            List<Parcela> listaP = new List<Parcela>();

            List<Parcela> novasParcelas = new List<Parcela>();

            if (TipoCondicaoPagamento == "S" && ParcelasValidas == "Sim")
                tipoCalculo = "Juros";
            else
                tipoCalculo = "Base";

            if (tipoCalculo == "Base")
            {
                foreach (Parcela item in listaParcela)
                {
                    Parcela p = new Parcela();
                    p.Percentual = item.Percentual;
                    p.ValorParcela = (liquidoTotal * item.Percentual) / 100;
                    p.DataVencimento = item.DataVencimento;
                    p.SequenciaParcela = item.SequenciaParcela;

                    novasParcelas.Add(p);
                }

                listaParcela = novasParcelas;
            }
            else
            {
                double valorDesconto = 0.00;
                //double valorLiquidoPedido = Convert.ToDouble(ValorLiquido.Replace(".", ","));
                double totalPercentual = Convert.ToDouble(ValorTotalPercentual);
                double valor = 0.00;
                double valorDescartado = 0;

                if (listaParcela.Count > 0)
                {
                    foreach (var item in listaParcela)
                    {
                        valor = item.Percentual;
                        valorDesconto = ((liquidoTotal * valor) / 100);

                        double d = Math.Truncate(valorDesconto * 100) / 100;

                        Parcela p = new Parcela();

                        p.ValorParcela = d;
                        p.Percentual = item.Percentual;
                        p.DataVencimento = item.DataVencimento;
                        p.SequenciaParcela = item.SequenciaParcela;

                        novasParcelas.Add(p);

                        double valorArredondado = Math.Round(valorDesconto, 2) - valorDesconto;

                        if (valorArredondado < 0)
                        {
                            valorDescartado = valorDescartado + valorArredondado;
                        }
                    }

                    //verificando soma das parcelas novas
                    double novoValor = novasParcelas.Sum(x => x.ValorParcela);

                    //double diff = 0.00;
                    //if (novoValor != liquidoTotal)
                    //{
                    //    diff = liquidoTotal - novoValor;
                    //    diff = (diff * 100) / 100;
                    //}

                    foreach (var item in novasParcelas)
                    {
                        if (item.SequenciaParcela == novasParcelas.Count)
                            item.ValorParcela = Math.Round(item.ValorParcela, 2);
                    }

                    //if (Math.Round(diff, 2) > 0)
                    //{
                    //    foreach (var item in novasParcelas)
                    //    {
                    //        if (item.SequenciaParcela == novasParcelas.Count)
                    //            item.ValorParcela = Math.Round(item.ValorParcela + diff, 2);
                    //    }
                    //}
                }

                if (listaP.Count > 0)
                    listaParcela = listaP;
                else
                    listaParcela = novasParcelas;
            }

            TempData["listaParcela"] = listaParcela;

            return Json(listaParcela);
        }

        public JsonResult ObterItensPedido(int codPedido)
        {
            TempData.Keep();
            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);
            List<PedidoItem> itens = new List<PedidoItem>();

            var resultado = UnidadeTrabalho.ObterTodos<PedidoItem>().Where(x => x.Empresa == codEmpFiltroPadrao && x.Filial == codFilialFiltroPadrao && x.Pedido.Codigo == codPedido).OrderBy(x => x.Sequencia);

            foreach (var item in resultado.ToList())
            {
                int minutes = Convert.ToInt32(item.HoraDesconto);
                var result = TimeSpan.FromMinutes(minutes);

                item.HoraDesconto = result.ToString(@"hh\:mm");

                itens.Add(item);
            }

            TempData["listItemPedido"] = itens;

            return Json(resultado.ToList());
            //Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            //int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            //int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);

            //parameters.Add("codEmpresa", codEmpFiltroPadrao);
            //parameters.Add("codFilial", codFilialFiltroPadrao);
            //parameters.Add("codPedido", codPedido);

            //var resultado = UnidadeTrabalho.ExecuteSql<ItensPedido>(@"SELECT * FROM TABLE(FUNC_PEDIDO_ITENS(:codEmpresa, :codFilial, :codPedido))", parameters).ToList();
            ////var resultado = UnidadeTrabalho.ObterTodos<PedidoItem>().Where(x => x.Empresa.Codigo == codEmpFiltroPadrao && x.Filial.Codigo == codFilialFiltroPadrao && x.Pedido.Codigo == codPedido).OrderBy(x => x.Sequencia);

            //var lista = ConverterParaPedidoItem(resultado.ToList());

            //TempData["listItemPedido"] = lista;

            //return PartialView("EditingForm", lista);
        }

        public ActionResult IncluirPedido(string pedidoPost = null, bool isJson = false, string parcelasValidas = null)
        {
            TempData.Keep("listItemPedido");
            TempData.Keep("listaParcela");
            TempData.Keep("listaAvalista");

            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            var listaAvalista = TempData["listaAvalista"] as List<GrupoEmpresa>;

            if (pedidoPost == null)
                throw new Exception("Não foi possível localizar o pedido a ser enviado, favor verificar.");

            if (listItemPedido.Count <= 0)
                throw new Exception("Favor informar pelo menos um item no pedido.");

            Pedido pedido = new JavaScriptSerializer().Deserialize<Pedido>(pedidoPost);
            CondicaoPagamento condPgto = UnidadeTrabalho.ObterTodos<CondicaoPagamento>().Where(x => x.Codigo == pedido.CodCondicaoPagamento.ToString()).FirstOrDefault();

            pedido.CodCondicaoPagamento = condPgto.Codigo;
            pedido.DescCondicaoPagamento = condPgto.Descricao;

            pedido.TaxaJuros = Convert.ToDouble(condPgto.TaxaJuros);

            pedido.DataIniJuros = Convert.ToDateTime(pedido.DataIniJuros.ToString("dd/MM/yyyy"));

            if (pedido.TaxaJuros <= 0 && pedido.AcrFin <= 0)
                pedido.DataIniJuros = Convert.ToDateTime("31/12/1900");

            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);
            int codUsuarioLogado = Convert.ToInt32(Session["codUsu"]);
            string usuarioLogado = Session["usuarioLogado"].ToString();
            string senhaUsuarioLogado = Session["senhaUsuLogado"].ToString();

            GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient ws = new GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient();

            if (ws == null)
                throw new Exception("Não foi possível realizar uma conexão com o serviço de autenticação.");

            if (pedido.CIFFOB == "C" && pedido.ValorFrete > 0)
            {
                double somaFrete = listItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorFrete);
                if (somaFrete <= 0)
                    throw new Exception("Favor incluir ao menos um item no rateio do frete.");
            }

            GravarPedido.pedidosvendaGravarPedidoInPedido pedidoNovo = new GravarPedido.pedidosvendaGravarPedidoInPedido();

            pedidoNovo.opeExe = "I"; //"A" para Alterar, "I" para Inserir, "C" para Carregar,  "E" para excluir, "R" para reabrir ou "P" para imprimir o pedido
            pedidoNovo.sitPed = "9"; //Para cancelar, informar "A" e preencher o campo "SitPed" = 5 - Cancelado
            pedidoNovo.fecPed = "S"; //passar sempre S

            pedidoNovo.cifFob = pedido.CIFFOB;
            pedidoNovo.codCli = pedido.CodCliente.ToString();
            //pedidoNovo.codCon = pedido.CodigoConvenio;
            pedidoNovo.codCpg = pedido.CodCondicaoPagamento.ToString();
            pedidoNovo.codEmp = (codEmpFiltroPadrao > 0) ? codEmpFiltroPadrao.ToString() : "";
            pedidoNovo.codFil = (codFilialFiltroPadrao > 0) ? codFilialFiltroPadrao.ToString() : "";
            pedidoNovo.codFpg = (pedido.CodFormaPagamento > 0) ? pedido.CodFormaPagamento.ToString() : "";
            pedidoNovo.codGre = !string.IsNullOrEmpty(pedido.CodGrupoEmpresa.ToString()) ? pedido.CodGrupoEmpresa.ToString() : "0";
            pedidoNovo.codRep = !string.IsNullOrEmpty(pedido.CodRepresentante.ToString()) ? pedido.CodRepresentante.ToString() : "0";
            pedidoNovo.codTra = (pedido.CodigoTransportadora != null) ? pedido.CodigoTransportadora.ToString() : "0";
            pedidoNovo.USU_TxaJur = !string.IsNullOrEmpty(pedido.TaxaJuros.ToString()) ? pedido.TaxaJuros.ToString() : "0";
            pedidoNovo.USU_AcrFin = !string.IsNullOrEmpty(pedido.AcrFin.ToString()) ? pedido.AcrFin.ToString() : "0";

            pedidoNovo.datEmi = pedido.DataEmissao.ToShortDateString();

            pedidoNovo.USU_ObsPed = pedido.ObsPedido;
            pedidoNovo.USU_ObsEnt = pedido.ObsEntrega;
            pedidoNovo.plaVei = pedido.NomePlaca;
            pedidoNovo.temPar = parcelasValidas == "Sim" ? "S" : "N";

            pedidoNovo.tnsPro = pedido.CodTransacao.ToString();
            pedidoNovo.vlrFre = PreparaValor(pedido.ValorFrete.ToString());
            pedidoNovo.vlrLiq = PreparaValor(pedido.ValorLiquido.ToString());

            //preparando avalistas
            //criando a lista de avalistas
            List<GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas> listAvalistas = new List<GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas>();

            if (listaAvalista != null && listaAvalista.Count > 0)
            {
                foreach (var aval in listaAvalista)
                {
                    GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas avalista = new GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas();

                    avalista.codAva = aval.Codigo;
                    avalista.codAvaSpecified = true;

                    //inserindo na lista o avalista
                    listAvalistas.Add(avalista);
                }
            }

            //preparando os itens do pedido
            //criando uma lista para itens do pedido
            List<GravarPedido.pedidosvendaGravarPedidoInPedidoProduto> itensPedido = new List<GravarPedido.pedidosvendaGravarPedidoInPedidoProduto>();
            double valorPedido = 0;

            int seqProd = 0; //variável criada para ajustar a sequência dos itens do pedido

            foreach (var item in listItemPedido)
            {
                seqProd++;

                valorPedido += Math.Round(item.ValorLiquido, 2);
                //populando o item do pedido
                GravarPedido.pedidosvendaGravarPedidoInPedidoProduto itemPedidoNovo = new GravarPedido.pedidosvendaGravarPedidoInPedidoProduto();

                itemPedidoNovo.codPro = item.CodigoProduto.ToString();
                itemPedidoNovo.codTpr = item.CodigoTRP != null ? item.CodigoTRP.ToString() : "";

                if (Convert.ToDateTime(item.DataEntrega.ToString()) < DateTime.Today)
                    throw new Exception("A data de entrega para o item <b>" + item.DescricaoProduto.ToString() + "</b> não pode ser menor que a data atual.");

                itemPedidoNovo.datEnt = Convert.ToDateTime(item.DataEntrega).ToShortDateString();
                itemPedidoNovo.seqIpd = seqProd.ToString();
                itemPedidoNovo.qtdPed = (item.QuantidadePedido > 0) ? item.QuantidadePedido.ToString() : "0";
                itemPedidoNovo.preUni = (item.PrecoUnitario > 0) ? PreparaValor(item.PrecoUnitario.ToString("N5")) : "0";
                itemPedidoNovo.opeExe = "I"; //"A" para Alterar, "I" para Inserir, "C" para Carregar,  "E" para excluir
                itemPedidoNovo.vlrAcr = (item.ValorAcrescimoUsuario > 0) ? PreparaValor(Math.Round(item.ValorAcrescimoUsuario, 2, MidpointRounding.AwayFromZero).ToString()) : "0";
                itemPedidoNovo.vlrDsc = (item.ValorDescontoUsuario > 0) ? PreparaValor(Math.Round(item.ValorDescontoUsuario, 2, MidpointRounding.AwayFromZero).ToString()) : "0";
                itemPedidoNovo.perAcr = (item.PercentualAcrescimoUsuario > 0) ? PreparaValor(item.PercentualAcrescimoUsuario.ToString()) : "0";
                itemPedidoNovo.perDsc = (item.PercentualDescontoUsuario > 0) ? PreparaValor(item.PercentualDescontoUsuario.ToString()) : "0";
                itemPedidoNovo.codDer = (item.CodigoDerivacao != null) ? item.CodigoDerivacao.ToString() : " ";
                itemPedidoNovo.vlrFre = (item.ValorFrete > 0) ? PreparaValor(item.ValorFrete.ToString()) : "0";

                double totalLiquido = Math.Round((item.PrecoUnitario * item.QuantidadePedido), 2);

                itemPedidoNovo.vlrLiq = (string.IsNullOrEmpty(item.ValorLiquido.ToString())) ? "0" : PreparaValor(Math.Round(item.ValorLiquido, 2, MidpointRounding.AwayFromZero).ToString());
                itemPedidoNovo.USU_PreBas = (item.PrecoBase > 0) ? PreparaValor(item.PrecoBase.ToString()) : "0";
                itemPedidoNovo.codDep = item.CodigoDep;
                double percentual = 0.00;
                bool valorValido = double.TryParse(item.PercentualDescontoUsuario.ToString(), out percentual);

                //se foi preenchido algum desconto para o item é obriatorio preencher os dados de data, horario e o usuario
                if (valorValido && percentual > 0)
                {
                    itemPedidoNovo.USU_DatDsc = item.DataDesconto;
                    itemPedidoNovo.USU_HorDsc = TimeSpan.Parse(item.HoraDesconto).TotalMinutes.ToString();
                    itemPedidoNovo.USU_UsuDsc = item.UsuarioDesconto;
                }
                else
                {
                    itemPedidoNovo.USU_DatDsc = "";
                    itemPedidoNovo.USU_HorDsc = "";
                    itemPedidoNovo.USU_UsuDsc = "";
                }

                //inserindo na lista de itens do pedido novo
                itensPedido.Add(itemPedidoNovo);
            }

            //criando a lista de parcelas
            List<GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas> listaNovasParcelas = new List<GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas>();
            double totalParcelas = 0;

            var listaParcelas = TempData["listaParcela"] as List<Parcela>;
            listaParcelas = listaParcelas.OrderBy(x => x.SequenciaParcela).ToList();

            if (listaParcelas != null && listaParcelas.Count > 0)
            {
                //Método utilizado para organizar as parcelas por sequência e data. Também faz o ajustes de centavos.
                //OrganizarParcelas();

                foreach (var parcelaItem in listaParcelas)
                {
                    totalParcelas += Math.Round(parcelaItem.ValorParcela, 2);
                    GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas parcela = new GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas();

                    parcela.opeExe = "I"; //"A" para Alterar, "I" para Inserir, "C" para Carregar,  "E" para excluir
                    parcela.perPar = (parcelaItem.Percentual > 0) ? PreparaValor(parcelaItem.Percentual.ToString()) : "0";
                    parcela.seqPar = parcelaItem.SequenciaParcela.ToString();
                    parcela.vlrPar = (parcelaItem.ValorParcela > 0) ? PreparaValor(parcelaItem.ValorParcela.ToString()) : "0";
                    parcela.vctPar = parcelaItem.DataVencimento;

                    //inserindo na lista a parcela
                    listaNovasParcelas.Add(parcela);
                }
            }

            //inserindo a lista de itens no pedido novo um array dentro do outro;
            pedidoNovo.produto = itensPedido.ToArray();
            pedidoNovo.avalistas = listAvalistas.ToArray();
            pedidoNovo.parcelas = listaNovasParcelas.ToArray();

            pedidoNovo.codUsu = codUsuarioLogado.ToString();

            //criando uma lista para inserir no pedido
            List<GravarPedido.pedidosvendaGravarPedidoInPedido> pedidoLista = new List<GravarPedido.pedidosvendaGravarPedidoInPedido>();

            pedidoLista.Add(pedidoNovo);

            //inserindo a lista de pedidos no parameters do gravar pedido
            GravarPedido.pedidosvendaGravarPedidoIn pedidoIn = new GravarPedido.pedidosvendaGravarPedidoIn();
            pedidoIn.pedido = pedidoLista.ToArray();

            GravarPedido.pedidosvendaGravarPedidoOut resultado = ws.GravarPedido(usuarioLogado, senhaUsuarioLogado, 0, pedidoIn);
            var respostaPedido = new GravarPedido.pedidosvendaGravarPedidoOut();

            return Json(resultado.respostaPedido.Select(l => new
            {
                mensagemRetorno = l.retorno == "OK" ? resultado.mensagemRetorno : l.retorno,
                retorno = l.retorno,
                numPed = l.numPed,
                codFil = l.codFil,
                codEmp = l.codEmp,
                pedBlo = l.pedBlo,
                msgRetorno = l.msgRet,
                situacaoPedido = RetornaSituacaoPedido(Convert.ToInt32(l.sitPed)),
                retornoParcela = (l.gridPar != null) ? l.gridPar.FirstOrDefault().retorno : "",
                retornoProduto = (l.gridPro != null) ? l.gridPro.FirstOrDefault().retorno : "",
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarPedido(string pedidoPost = null)
        {
            TempData.Keep("listItemPedido");
            TempData.Keep("listaParcela");
            TempData.Keep("listaAvalista");

            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            //var listaPostParcelas = TempData["listaParcela"] as List<Parcela>;
            var listaPostAvalistas = TempData["listaAvalista"] as List<GrupoEmpresa>;

            Pedido pedido = new JavaScriptSerializer().Deserialize<Pedido>(pedidoPost);
            //int hora = pedido.HoraEmissao;
            string usuarioLogado = Session["usuarioLogado"].ToString();
            string senhaUsuarioLogado = Session["senhaUsuLogado"].ToString();

            CondicaoPagamento condPgto = UnidadeTrabalho.ObterTodos<CondicaoPagamento>().Where(x => x.Codigo == pedido.CodCondicaoPagamento && x.FormaPagamento.Codigo == pedido.CodFormaPagamento.ToString()).FirstOrDefault();

            //CondicaoPagamentoProc condPgto = ObterCondPagamento(pedido.CondicaoPagamento.Codigo, pedido.FormaPagamento.Codigo.ToString(), false, pedido.CodigoConvenio);
            //ConsultaParcelasCondPgto condicaoPgto = ObterObjetoParcelasTipoCPR(condPgto.CODCPG, pedido.CodigoConvenio, pedido.DataEmissao.ToShortDateString(), condPgto.CPGCPR, pedido.CodigoTPR);

            //ValidaPedido validaPedido = ValidarPedido(pedido.Codigo, pedido.CodGrupoEmpresa, pedido.CodCliente, pedido.CodRepresentante, pedido.Transacao.Codigo, pedido.CodFormaPgto, condPgto.CODCPG, pedido.CodigoTPR, Convert.ToDouble(pedido.QtdSaca), pedido.ValorLiquido);

            //if (validaPedido != null && validaPedido.BLOITE == "S")
            //    throw new CoreException(validaPedido.BLOMSG);

            //pedido.CondicaoPagamento.Abreviacao = condPgto.ABRCPG;
            pedido.CodCondicaoPagamento = condPgto.Codigo;
            //pedido.CondicaoPagamento.CodigoCampanha = condPgto.CODCAM.ToString();
            //pedido.CondicaoPagamento.Descricao = condPgto.DESCPG;
            //pedido.CondicaoPagamento.Empresa.Codigo = Convert.ToInt32(condPgto.CODEMP);
            //pedido.CondicaoPagamento.Filial.Codigo = Convert.ToInt32(condPgto.CODFIL);
            //pedido.CondicaoPagamento.Fonte = condPgto.FONTE;
            //pedido.CondicaoPagamento.FormaPagamento.Codigo = Convert.ToInt32(condPgto.CODFPG);
            //pedido.CondicaoPagamento.FormaPagamento.Descricao = condPgto.DESFPG;
            //pedido.DataIniJuros = condPgto.INIJUR;
            //pedido.CondicaoPagamento.ManutencaoParcela = condPgto.MANPAR;
            //pedido.CondicaoPagamento.PodSel = condPgto.PODSEL;
            //pedido.CondicaoPagamento.QuantidadeParcelas = Convert.ToInt32(condPgto.QTDPAR);
            pedido.AcrFin = Convert.ToDouble(condPgto.TaxaAcrescimoFinanceiro);

            if (condPgto.TipoEspecial != "I")
                pedido.TaxaJuros = Convert.ToDouble(condPgto.TaxaJuros);
            else
                pedido.TaxaJuros = Convert.ToDouble(condPgto.TaxaAcrescimoFinanceiro);

            //pedido.CondicaoPagamento.TipoCalculo = condPgto.FORCAL;
            //pedido.CondicaoPagamento.TipoCPR = condPgto.CPGCPR;
            //pedido.CondicaoPagamento.TipoEspecial = condPgto.CPGESP;
            //pedido.CondicaoPagamento.TipoJuros = condPgto.TIPJUR;
            //pedido.CondicaoPagamento.TipoParcela = Convert.ToInt32(condPgto.TIPPAR);
            //pedido.CondicaoPagamento.VlrDis = Convert.ToDouble(condPgto.VLRDIS);
            //pedido.CondicaoPagamento.VlrMet = Convert.ToDouble(condPgto.VLRMET);
            //pedido.CondicaoPagamento.VlrVen = Convert.ToDouble(condPgto.VLRVEN);

            if (pedido == null)
                throw new Exception("Não foi possível localizar o pedido a ser enviado, favor verificar.");

            if (listItemPedido.Count() == 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Favor informar pelo menos um item no pedido."
                });

                throw new Exception("Favor informar pelo menos um item no pedido.");
            }

            if (pedido.Codigo == int.MinValue)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Não foi possível localizar o código do pedido para alteração."
                });

                throw new Exception("Não foi possível localizar o código do pedido para alteração.");
            }

            //if ((pedido.CondicaoPagamento.TipoCPR == "T" || pedido.CondicaoPagamento.TipoCPR == "I") && (pedido.CodigoTPR == string.Empty || pedido.CodigoTPR == null))
            //    throw new Exception("Selecione uma Tabela CPR.");

            pedido.DataIniJuros = Convert.ToDateTime(pedido.DataIniJuros.ToString("dd/MM/yyyy"));

            if (pedido.TaxaJuros <= 0 && pedido.AcrFin <= 0)
                pedido.DataIniJuros = Convert.ToDateTime("31/12/1900");

            CultureInfo cultura = CultureInfo.GetCultureInfo("pt-BR");

            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);
            int codUsuarioLogado = Convert.ToInt32(Session["codUsu"]);

            GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient ws = new GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient();

            if (ws == null)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Não foi possível realizar uma conexão com o serviço de autenticação."
                });

                throw new Exception("Não foi possível realizar uma conexão com o serviço de autenticação.");
            }

            if (pedido.CIFFOB == "C" && pedido.ValorFrete > 0)
            {
                double somaFrete = listItemPedido.Where(x => x.RateioFrete).Sum(x => x.ValorFrete);
                if (somaFrete <= 0)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Favor incluir ao menos um item no rateio do frete."
                    });

                    throw new Exception("Favor incluir ao menos um item no rateio do frete.");
                }
            }

            GravarPedido.pedidosvendaGravarPedidoInPedido pedidoNovo = new GravarPedido.pedidosvendaGravarPedidoInPedido();

            pedidoNovo.opeExe = "A"; //"A" para Alterar, "I" para Inserir, "C" para Carregar,  "E" para excluir, "R" para reabrir ou "P" para imprimir o pedido
            pedidoNovo.sitPed = "9"; //Para cancelar, informar "A" e preencher o campo "SitPed" = 5 - Cancelado
            pedidoNovo.numPed = pedido.Codigo.ToString();
            pedidoNovo.cifFob = pedido.CIFFOB;
            pedidoNovo.codCli = pedido.CodCliente.ToString();
            //pedidoNovo.codCon = pedido.CodigoConvenio;
            pedidoNovo.codCpg = pedido.CodCondicaoPagamento.ToString();
            pedidoNovo.codEmp = codEmpFiltroPadrao.ToString();
            pedidoNovo.codFil = codFilialFiltroPadrao.ToString();
            pedidoNovo.codFpg = pedido.CodFormaPagamento.ToString();
            pedidoNovo.codGre = pedido.CodGrupoEmpresa.ToString();
            pedidoNovo.codRep = pedido.CodRepresentante.ToString();
            pedidoNovo.codTra = pedido.CodigoTransportadora != null ? pedido.CodigoTransportadora.ToString() : "";
            pedidoNovo.datEmi = pedido.DataEmissao != null ? pedido.DataEmissao.ToString("dd/MM/yyyy") : "";
            //pedidoNovo.horEmi = TimeSpan.Parse(hora.ToString()).TotalMinutes.ToString();
            pedidoNovo.fecPed = "S"; //nao sei
            pedidoNovo.USU_ObsPed = pedido.ObsPedido;
            pedidoNovo.USU_ObsEnt = pedido.ObsEntrega;
            //pedidoNovo.USU_QtdSac = (pedido.QtdSaca != null) ? pedido.QtdSaca.ToString() : "";
            pedidoNovo.plaVei = pedido.NomePlaca;
            pedidoNovo.temPar = (condPgto != null && condPgto.TipoEspecial != null) ? condPgto.ManutencaoParcela.ToString() : "";
            //pedidoNovo.USU_CodCpr = (pedido.TabelaCPR != null) ? pedido.TabelaCPR.ToString() : "";
            pedidoNovo.tnsPro = pedido.CodTransacao.ToString();
            pedidoNovo.vlrFre = PreparaValor(pedido.ValorFrete.ToString());
            pedidoNovo.vlrLiq = PreparaValor(pedido.ValorLiquido.ToString());
            pedidoNovo.USU_TxaJur = !string.IsNullOrEmpty(pedido.TaxaJuros.ToString()) ? pedido.TaxaJuros.ToString() : "0";
            pedidoNovo.USU_AcrFin = !string.IsNullOrEmpty(pedido.AcrFin.ToString()) ? pedido.AcrFin.ToString() : "0";

            pedidoNovo.codUsu = codUsuarioLogado.ToString();
            //preparando parcelas
            //criando a lista
            List<GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas> listaParcelas = new List<GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas>();

            var listaPostParcelas = TempData["listaParcela"] as List<Parcela>;
            listaPostParcelas = listaPostParcelas.OrderBy(x => x.SequenciaParcela).ToList();

            if (listaPostParcelas != null && listaPostParcelas.Count > 0)
            {
                //Método utilizado para organizar as parcelas por sequência e data
                //OrganizarParcelas();

                double aux = 0;
                foreach (var parcelaItem in listaPostParcelas)
                {
                    GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas parcela = new GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas();

                    parcela.opeExe = "I";
                    parcela.perPar = (parcelaItem.Percentual > 0) ? PreparaValor(parcelaItem.Percentual.ToString()) : "0";
                    parcela.seqPar = parcelaItem.SequenciaParcela.ToString();
                    parcela.vlrPar = (parcelaItem.ValorParcela > 0) ? PreparaValor(parcelaItem.ValorParcela.ToString()) : "0";
                    parcela.vctPar = parcelaItem.DataVencimento.Split(' ')[0];
                    aux += parcelaItem.ValorParcela;
                    //inserindo na lista a parcela
                    listaParcelas.Add(parcela);
                }

                //if (pedido.ValorLiquido.ToString("N2") != aux.ToString("N2"))
                //    throw new CoreException("O valor total das parcelas está diferente do valor Total do pedido.");
            }

            //preparando avalistas
            //criando a lista
            List<GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas> listaAvalistas = new List<GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas>();

            if (listaPostAvalistas != null)
            {
                foreach (var aval in listaPostAvalistas)
                {
                    GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas avalista = new GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas();

                    avalista.codAva = aval.Codigo;
                    avalista.codAvaSpecified = true;

                    //inserindo na lista o avalista
                    listaAvalistas.Add(avalista);
                }
            }

            //preparando os itens do pedido
            //criando uma lista para itens do pedido
            List<GravarPedido.pedidosvendaGravarPedidoInPedidoProduto> itensPedido = new List<GravarPedido.pedidosvendaGravarPedidoInPedidoProduto>();

            int seqProd = 0;

            foreach (var item in listItemPedido)
            {
                seqProd++;
                //populando o item do pedido
                GravarPedido.pedidosvendaGravarPedidoInPedidoProduto itemPedidoNovo = new GravarPedido.pedidosvendaGravarPedidoInPedidoProduto();

                itemPedidoNovo.codPro = item.CodigoProduto.ToString();
                itemPedidoNovo.codTpr = item.CodigoTRP.ToString();

                if (Convert.ToDateTime(item.DataEntrega.ToString()) < DateTime.Today)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "A data de entrega para o item <b>" + item.DescricaoProduto.ToString() + "</b> não pode ser menor que a data atual."
                    });

                    throw new Exception("A data de entrega para o item <b>" + item.DescricaoProduto.ToString() + "</b> não pode ser menor que a data atual.");
                }

                itemPedidoNovo.datEnt = item.DataEntrega.Split(' ')[0];
                //itemPedidoNovo.seqIpd = item.Sequencia.ToString();
                itemPedidoNovo.seqIpd = seqProd.ToString();
                itemPedidoNovo.qtdPed = (item.QuantidadePedido > 0) ? item.QuantidadePedido.ToString() : "0";
                itemPedidoNovo.preUni = (item.PrecoUnitario > 0) ? PreparaValor(item.PrecoUnitario.ToString("N5")) : "0";
                itemPedidoNovo.opeExe = "I";
                itemPedidoNovo.vlrAcr = (item.ValorAcrescimoUsuario > 0) ? PreparaValor(Math.Round(item.ValorAcrescimoUsuario, 2, MidpointRounding.AwayFromZero).ToString()) : "0";
                itemPedidoNovo.vlrDsc = (item.ValorDescontoUsuario > 0) ? PreparaValor(Math.Round(item.ValorDescontoUsuario, 2, MidpointRounding.AwayFromZero).ToString()) : "0";
                itemPedidoNovo.perAcr = (item.PercentualAcrescimoUsuario > 0) ? PreparaValor(item.PercentualAcrescimoUsuario.ToString()) : "0";
                itemPedidoNovo.perDsc = (item.PercentualDescontoUsuario > 0) ? PreparaValor(item.PercentualDescontoUsuario.ToString()) : "0";
                itemPedidoNovo.codDer = (item.CodigoDerivacao != null) ? item.CodigoDerivacao.ToString() : " ";
                itemPedidoNovo.vlrLiq = (string.IsNullOrEmpty(item.ValorLiquido.ToString("N2")) ? "0" : PreparaValor(item.ValorLiquido.ToString("N2")));
                itemPedidoNovo.USU_PreBas = (item.PrecoBase > 0) ? PreparaValor(item.PrecoBase.ToString()) : "0";
                itemPedidoNovo.vlrFre = (item.ValorFrete > 0) ? PreparaValor(item.ValorFrete.ToString()) : "0";
                itemPedidoNovo.codDep = item.CodigoDep;

                double percentual = 0.00;
                bool valorValido = double.TryParse(item.PercentualDescontoUsuario.ToString(), out percentual);

                //se foi preenchido algum desconto para o item é obriatorio preencher os dados de data, horario e o usuario
                if (valorValido && percentual > 0)
                {
                    //itemPedidoNovo.USU_DatDsc = DateTime.Today.ToString("dd/MM/yyyy");
                    //itemPedidoNovo.USU_HorDsc = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                    //itemPedidoNovo.USU_UsuDsc = codUsuarioLogado.ToString();

                    itemPedidoNovo.USU_DatDsc = item.DataDesconto;
                    itemPedidoNovo.USU_HorDsc = TimeSpan.Parse(item.HoraDesconto).TotalMinutes.ToString();
                    itemPedidoNovo.USU_UsuDsc = item.UsuarioDesconto;

                }
                else
                {
                    itemPedidoNovo.USU_DatDsc = "";
                    itemPedidoNovo.USU_HorDsc = "";
                    itemPedidoNovo.USU_UsuDsc = "";
                }

                //inserindo na lista de itens do pedido novo
                itensPedido.Add(itemPedidoNovo);
            }

            //inserindo a lista de itens no pedido novo um array dentro do outro;
            pedidoNovo.produto = itensPedido.ToArray();
            pedidoNovo.avalistas = listaAvalistas.ToArray();
            pedidoNovo.parcelas = listaParcelas.ToArray();

            //criando uma lista para inserido o pedido
            List<GravarPedido.pedidosvendaGravarPedidoInPedido> pedidoLista = new List<GravarPedido.pedidosvendaGravarPedidoInPedido>();

            pedidoLista.Add(pedidoNovo);

            //inserindo a lista de pedidos no parameters do gravar pedido
            GravarPedido.pedidosvendaGravarPedidoIn pedidoIn = new GravarPedido.pedidosvendaGravarPedidoIn();
            pedidoIn.pedido = pedidoLista.ToArray();

            GravarPedido.pedidosvendaGravarPedidoOut resultado = ws.GravarPedido(usuarioLogado, senhaUsuarioLogado, 0, pedidoIn);
            var respostaPedido = new GravarPedido.pedidosvendaGravarPedidoOutRespostaPedido();

            return Json(resultado.respostaPedido.Select(l => new
            {
                mensagemRetorno = l.retorno == "OK" ? resultado.mensagemRetorno : l.retorno,
                retorno = l.retorno,
                numPed = l.numPed,
                codFil = l.codFil,
                codEmp = l.codEmp,
                pedBlo = l.pedBlo,
                msgRetorno = l.msgRet,
                situacaoPedido = l.sitPed != null ? RetornaSituacaoPedido(Convert.ToInt32(l.sitPed)) : "",
                podeAlterar = pedidoNovo.fecPed,
                retornoParcela = (l.gridPar != null) ? l.gridPar.FirstOrDefault().retorno : "",
                retornoProduto = (l.gridPro != null) ? l.gridPro.FirstOrDefault().retorno : "",
            }).ToList());
        }

        public ActionResult HabilitarPedido(int codPedido)
        {
            TempData.Keep();

            string msgRetornoCondPgto = string.Empty;
            string usuarioLogado = Session["usuarioLogado"].ToString();
            string senhaUsuLogado = Session["senhaUsuLogado"].ToString();
            int codFilial = Convert.ToInt32(Session["filAti"]);
            int codEmpresa = Convert.ToInt32(Session["empAti"]);

            if (codPedido <= 0)
                throw new Exception("Não foi possível localizar o pedido a ser enviado, favor verificar.");

            if (codFilial <= 0)
                throw new Exception("Não foi possível localizar a filial do pedido.");

            if (codEmpresa <= 0)
                throw new Exception("Não foi possível localizar a empresa do pedido.");

            var pedidoSelecionado = UnidadeTrabalho.ObterTodos<Pedido>().Where(x => x.CodEmpresa == codEmpresa && x.CodFilial == codFilial && x.Codigo == codPedido).FirstOrDefault();
            //Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            //parameters.Add("codEmpresa", codEmpresa);
            //parameters.Add("codFilial", codFilial);
            //parameters.Add("codPedido", codPedido);

            //var resultadoPedido = UnidadeTrabalho.ExecuteSql<ConsultaPedido>(@"SELECT * FROM TABLE(FUNC_PEDIDO_DGERAIS(:codEmpresa, :codFilial, :codPedido))", parameters).FirstOrDefault();

            //Pedido pedidoSelecionado = ConverterParaPedido(resultadoPedido);

            if (pedidoSelecionado == null)
                throw new Exception("Não foi possível localizar o pedido a ser enviado, favor verificar.");

            if ((pedidoSelecionado.SituacaoPedido != 1 && pedidoSelecionado.PodeAlterar == "S"))
                throw new Exception("Operação não permitida devido a situação do pedido.");

            CondicaoPagamento condicao = UnidadeTrabalho.ObterTodos<CondicaoPagamento>().Where(x => x.Codigo == pedidoSelecionado.CodCondicaoPagamento).FirstOrDefault();
            //CondicaoPagamentoProc condicao = ObterCondPagamento(pedidoSelecionado.CodCondicacaoPagamento, pedidoSelecionado.CodFormaPgto.ToString(), false, pedidoSelecionado.CodigoConvenio);
            //ConsultaParcelasCondPgto condicaoPgto = ObterObjetoParcelasTipoCPR(condicao.CODCPG, pedidoSelecionado.CodigoConvenio, pedidoSelecionado.DataEmissao.ToShortDateString(), condicao.CPGCPR, pedidoSelecionado.CodigoTPR);

            if (condicao == null)
                msgRetornoCondPgto = "Não foi possível localizar a condição de pagamento " + condicao.Codigo + ".";

            //pedidoSelecionado.CondicaoPagamento = new CondicaoPagamento();

            //pedidoSelecionado.CondicaoPagamento.Abreviacao = condicao.ABRCPG;
            pedidoSelecionado.CodCondicaoPagamento = condicao.Codigo;
            //pedidoSelecionado.CondicaoPagamento.CodigoCampanha = condicao.CODCAM.ToString();
            pedidoSelecionado.DescCondicaoPagamento = condicao.Descricao;
            //pedido.CondicaoPagamento.Empresa.Codigo = Convert.ToInt32(condPgto.CODEMP);
            //pedido.CondicaoPagamento.Filial.Codigo = Convert.ToInt32(condPgto.CODFIL);
            //pedidoSelecionado.CondicaoPagamento.Fonte = condicao.FONTE;
            //pedido.CondicaoPagamento.FormaPagamento.Codigo = Convert.ToInt32(condPgto.CODFPG);
            //pedido.CondicaoPagamento.FormaPagamento.Descricao = condPgto.DESFPG;
            //pedidoSelecionado.DataIniJuros = condicao.INIJUR;
            //pedidoSelecionado.CondicaoPagamento.ManutencaoParcela = condicao.MANPAR;
            //pedidoSelecionado.CondicaoPagamento.PodSel = condicao.PODSEL;
            //pedidoSelecionado.CondicaoPagamento.QuantidadeParcelas = Convert.ToInt32(condicao.QTDPAR);
            //pedidoSelecionado.CondicaoPagamento.TaxaAcrescimoFinanceiro = Convert.ToDouble(condicao.ACRFIN);

            //if (condicao.CPGCPR == "I")
            //    pedidoSelecionado.CondicaoPagamento.TaxaJuros = Convert.ToDouble(condicaoPgto.TXAFIN);
            //else
            //    pedidoSelecionado.CondicaoPagamento.TaxaJuros = Convert.ToDouble(condicao.TXAJUR);
            pedidoSelecionado.TaxaJuros = Convert.ToDouble(condicao.TaxaJuros);
            pedidoSelecionado.AcrFin = Convert.ToDouble(condicao.TaxaAcrescimoFinanceiro);

            GravarPedido.pedidosvendaGravarPedidoOut resultado = habilitarPed(codPedido, codFilial, codEmpresa, "9", "N", pedidoSelecionado.DataEmissao, pedidoSelecionado.HoraEmissao, usuarioLogado, senhaUsuLogado, pedidoSelecionado.TaxaJuros.ToString(), pedidoSelecionado.DataIniJuros.ToString(), pedidoSelecionado.AcrFin.ToString());

            Pedido pedido = (Pedido)TempData["Pedido"];
            if (pedido != null && resultado.respostaPedido.FirstOrDefault().retorno == "OK")
            {
                pedido.SituacaoPedido = 9;
                TempData["Pedido"] = pedido;

                //if (pedido.ValorFrete == 0)
                //    HabilitarFreteParaTodosItens();
            }

            return Json(resultado.respostaPedido.Select(l => new
            {
                mensagemRetorno = l.retorno == "OK" ? resultado.mensagemRetorno : l.retorno,
                retornoCondPgto = msgRetornoCondPgto,
                retorno = l.retorno,
                numPed = l.numPed,
                msgRetorno = l.msgRet,
                situacaoPedido = RetornaSituacaoPedido(Convert.ToInt32(l.sitPed)),
                CodCondicaoPagamento = condicao != null ? condicao.Codigo : pedidoSelecionado.CodCondicaoPagamento,
                DescCondicaoPagamento = condicao != null ? condicao.Descricao : pedidoSelecionado.DescCondicaoPagamento,
                //CondicaoPagamentoTipoCPR = (condicao != null && condicao.CPGCPR != null) ? condicao.CPGCPR : "N",
                CondicaoPagamentoEspecial = (condicao != null && condicao.TipoEspecial != null) ? condicao.TipoEspecial : "N",
                //TabelaCPR = pedidoSelecionado.CodigoTPR,
                //DescTabelaCPR = pedidoSelecionado.NomeTPR,
                QtdParcelas = condicao != null ? condicao.QuantidadeParcelas : 0,
                ManutencaoParcelas = condicao != null ? condicao.ManutencaoParcela : "N",
                TaxaJuros = PreparaValor(pedidoSelecionado.TaxaJuros.ToString())
            }).ToList());
        }

        private GravarPedido.pedidosvendaGravarPedidoOut habilitarPed(int codPedido, int codFilial, int codEmpresa, string sitPed, string fecPed, DateTime dataEmissao, int horaEmissao, string usuarioLogado, string senhaUsuLogado, string taxaJuros, string iniJur, string acrFin)
        {
            CultureInfo cultura = CultureInfo.GetCultureInfo("pt-BR");
            int codUsuarioLogado = Convert.ToInt32(Session["codUsu"]);

            GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient ws = new GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient();

            if (ws == null)
                throw new Exception("Não foi possível realizar uma conexão com o serviço de autenticação.");

            GravarPedido.pedidosvendaGravarPedidoInPedido pedidoNovo = new GravarPedido.pedidosvendaGravarPedidoInPedido();

            pedidoNovo.opeExe = "A"; //"A" para Alterar, "I" para Inserir, "C" para Carregar,  "E" para excluir, "R" para reabrir ou "P" para imprimir o pedido
            pedidoNovo.sitPed = sitPed; //Para cancelar, informar "A" e preencher o campo "SitPed" = 5 - Cancelado
            pedidoNovo.numPed = codPedido.ToString();
            pedidoNovo.fecPed = fecPed;
            pedidoNovo.codEmp = codEmpresa.ToString();
            pedidoNovo.codFil = codFilial.ToString();
            pedidoNovo.datEmi = dataEmissao != null ? dataEmissao.ToString("dd/MM/yyyy") : "";
            pedidoNovo.horEmi = horaEmissao.ToString();
            pedidoNovo.codUsu = codUsuarioLogado.ToString();
            pedidoNovo.USU_TxaJur = taxaJuros;
            pedidoNovo.USU_AcrFin = acrFin;
            //pedidoNovo.USU_IniJur = iniJur;
            //pedidoNovo.parcelas = TempData["listaParcelas"] as GravarPedido.pedidosvendaGravarPedidoInPedidoParcelas[];
            //pedidoNovo.avalistas = TempData["listaAvalista"] as GravarPedido.pedidosvendaGravarPedidoInPedidoAvalistas[];
            //pedidoNovo.produto = TempData["listItemPedido"] as GravarPedido.pedidosvendaGravarPedidoInPedidoProduto[];

            //criando uma lista para inserido o pedido
            List<GravarPedido.pedidosvendaGravarPedidoInPedido> pedidoLista = new List<GravarPedido.pedidosvendaGravarPedidoInPedido>();

            pedidoLista.Add(pedidoNovo);

            //inserindo a lista de pedidos no parameters do gravar pedido
            GravarPedido.pedidosvendaGravarPedidoIn pedidoIn = new GravarPedido.pedidosvendaGravarPedidoIn();
            pedidoIn.pedido = pedidoLista.ToArray();

            GravarPedido.pedidosvendaGravarPedidoOut resultado = ws.GravarPedido(usuarioLogado, senhaUsuLogado, 0, pedidoIn);
            var respostaPedido = new GravarPedido.pedidosvendaGravarPedidoOutRespostaPedido();

            return resultado;
        }

        public ActionResult CancelarPedido(int CodPedido)
        {
            int CodFilial = Convert.ToInt32(Session["filAti"]);
            int CodEmpresa = Convert.ToInt32(Session["empAti"]);

            if (CodPedido <= 0)
                throw new Exception("Não foi possível localizar o pedido a ser enviado, favor verificar.");

            if (CodFilial <= 0)
                throw new Exception("Não foi possível localizar a filial do pedido.");

            if (CodEmpresa <= 0)
                throw new Exception("Não foi possível localizar a empresa do pedido.");


            var pedidoSelecionado = UnidadeTrabalho.ObterTodos<Pedido>().Where(x => x.CodEmpresa == CodEmpresa && x.CodFilial == CodFilial && x.Codigo == CodPedido).FirstOrDefault();

            if (pedidoSelecionado == null)
                throw new Exception("Não foi possível localizar o pedido a ser enviado, favor verificar.");

            if ((pedidoSelecionado.SituacaoPedido != 1 && pedidoSelecionado.PodeAlterar == "S"))
                throw new Exception("Operação não permitida devido a situação do pedido.");

            GravarPedido.pedidosvendaGravarPedidoOut resultado = cancelarPed(CodPedido, CodFilial, CodEmpresa, pedidoSelecionado.DataEmissao, pedidoSelecionado.HoraEmissao, pedidoSelecionado.DataIniJuros.ToString(), pedidoSelecionado.TaxaJuros.ToString(), pedidoSelecionado.AcrFin.ToString());

            return Json(resultado.respostaPedido.Select(l => new
            {
                mensagemRetorno = l.retorno == "OK" ? resultado.mensagemRetorno : l.retorno,
                retorno = l.retorno,
                numPed = l.numPed,
                msgRetorno = l.msgRet,
                situacaoPedido = RetornaSituacaoPedido(Convert.ToInt32(l.sitPed)),
                retornoParcela = (l.gridPar != null) ? l.gridPar.FirstOrDefault().retorno : "",
                retornoProduto = (l.gridPro != null) ? l.gridPro.FirstOrDefault().retorno : "",
            }).ToList());
        }

        private GravarPedido.pedidosvendaGravarPedidoOut cancelarPed(int codPedido, int codFilial, int codEmpresa, DateTime dataEmissao, int horaEmissao, string iniJur, string taxaJuros, string acrFin)
        {
            CultureInfo cultura = CultureInfo.GetCultureInfo("pt-BR");
            int codUsuarioLogado = Convert.ToInt32(Session["codUsu"]);
            string usuarioLogado = Session["usuarioLogado"].ToString();
            string senhaUsuLogado = Session["senhaUsuLogado"].ToString();

            GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient ws = new GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient();

            if (ws == null)
                throw new Exception("Não foi possível realizar uma conexão com o serviço de autenticação.");

            GravarPedido.pedidosvendaGravarPedidoInPedido pedidoNovo = new GravarPedido.pedidosvendaGravarPedidoInPedido();

            pedidoNovo.opeExe = "A"; //"A" para Alterar, "I" para Inserir, "C" para Carregar,  "E" para excluir, "R" para reabrir ou "P" para imprimir o pedido
            pedidoNovo.sitPed = "5"; //Para cancelar, informar "A" e preencher o campo "SitPed" = 5 - Cancelado
            pedidoNovo.numPed = codPedido.ToString();
            pedidoNovo.fecPed = "S"; //PASSA SEMRE S
            pedidoNovo.codEmp = codEmpresa.ToString();
            pedidoNovo.codFil = codFilial.ToString();
            pedidoNovo.datEmi = dataEmissao != null ? dataEmissao.ToString("dd/MM/yyyy") : "";
            pedidoNovo.horEmi = horaEmissao.ToString();
            pedidoNovo.USU_TxaJur = taxaJuros;
            pedidoNovo.USU_AcrFin = acrFin;
            //pedidoNovo.USU_IniJur = iniJur;

            pedidoNovo.codUsu = codUsuarioLogado.ToString();

            //criando uma lista para inserido o pedido
            List<GravarPedido.pedidosvendaGravarPedidoInPedido> pedidoLista = new List<GravarPedido.pedidosvendaGravarPedidoInPedido>();

            pedidoLista.Add(pedidoNovo);

            //inserindo a lista de pedidos no parameters do gravar pedido
            GravarPedido.pedidosvendaGravarPedidoIn pedidoIn = new GravarPedido.pedidosvendaGravarPedidoIn();
            pedidoIn.pedido = pedidoLista.ToArray();

            GravarPedido.pedidosvendaGravarPedidoOut resultado = ws.GravarPedido(usuarioLogado, senhaUsuLogado, 0, pedidoIn);
            var respostaPedido = new GravarPedido.pedidosvendaGravarPedidoOutRespostaPedido();

            return resultado;
        }

        public ActionResult EditarParcela(string parcela, string PermiteManutencao, string EmissaoPedido)
        {
            TempData.Keep();
            var listaParcela = TempData["listaParcela"] as List<Parcela>;

            Parcela parc = new JavaScriptSerializer().Deserialize<Parcela>(parcela);

            try
            {
                if (Convert.ToDateTime(parc.DataVencimento) < Convert.ToDateTime(EmissaoPedido))
                {
                    return Json(new
                    {
                        success = false,
                        message = "O vencimento da parcela não pode ser anterior à data do pedido."
                    });
                }

                if (PermiteManutencao == "S")
                {
                    for (int i = 0; i < listaParcela.Count; i++)
                    {
                        if (listaParcela[i].SequenciaParcela == Convert.ToInt32(parc.SequenciaParcela))
                        {
                            listaParcela[i].DataVencimento = parc.DataVencimento;
                            listaParcela[i].Percentual = parc.Percentual;
                            listaParcela[i].ValorParcela = parc.ValorParcela;
                        }
                    }

                    TempData["listaParcela"] = listaParcela;
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Manutenção de parcela não permitido."
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Ocorreu um erro ao editar a parcela."
                });
            }

            return Json(listaParcela);
        }

        private void CalculaItemIndividual(PedidoItem itemPedido, PedidoItem itemNovo, List<PedidoItem> itensRemover, List<Parcela> listaParcela,
            string tipoCalculo, string tipo_especial, string taxa_juros, string total_parcelas, double jurosFrete, double somaFrete, string dataIniJur = null)
        {
            DateTime iniJur = Convert.ToDateTime(dataIniJur);

            if (itemNovo != null && itemNovo.PrecoBase > 0 && itemNovo.codTabela != "")
            {
                itemPedido.CodigoTRP = itemNovo.codTabela != null ? itemNovo.codTabela : itemPedido.CodigoTRP;
                //itemPedido.PrecoBase = Math.Round(itemNovo.PrecoBase, 2);
                itemPedido.PrecoBase = itemNovo.PrecoBase;

                if (tipoCalculo == "Base")
                    itemPedido.PrecoUnitario = itemNovo.PrecoBase;

                double precoUnitario = 0;
                double taxaJurosNormal = Convert.ToDouble(taxa_juros.Replace(".", ","));
                double totalPrecoUnitario = 0;
                double totalLiquidoCalculado = 0;
                double rateioFrete = 0;
                //double PercentualProduto = 0;

                if (tipoCalculo == "Base")
                    precoUnitario = itemPedido.PrecoBase;
                else
                    precoUnitario = itemPedido.PrecoUnitario;

                //var totalJurosCalculado = itemPedido.PrecoBase * taxaJurosNormal;
                if (somaFrete > 0)
                {
                    rateioFrete = ((itemPedido.ValorFrete * 100) / somaFrete);
                    rateioFrete = rateioFrete * jurosFrete / 100;
                }


                if (tipoCalculo == "Juros")
                {
                    double TaxaJurosCalculada = 0;

                    foreach (var itemParcela in listaParcela)
                    {
                        //TaxaJurosCalculada += CalcularJuros((itemPedido.PrecoBase + itemPedido.ValorFrete), precoUnitario, itemParcela.Percentual, tipo_especial, iniJur,
                        //    itemParcela.DataVencimento, tipoCalculo, itemPedido.ValorLiquido.ToString(), taxa_juros);
                        TaxaJurosCalculada += CalcularJuros((itemPedido.PrecoBase), precoUnitario, itemParcela.Percentual, tipo_especial, iniJur,
                            Convert.ToDateTime(itemParcela.DataVencimento), tipoCalculo, itemPedido.ValorLiquido.ToString(), taxa_juros);

                    }

                    double NovoValorComJuros = Math.Round((itemPedido.PrecoBase * 1 + TaxaJurosCalculada * 1), 5, MidpointRounding.AwayFromZero);

                    totalLiquidoCalculado = (NovoValorComJuros * itemPedido.QuantidadePedido) + rateioFrete;
                    totalPrecoUnitario = NovoValorComJuros;

                }
                else
                {
                    if (tipo_especial == "N")
                    {
                        //totalPrecoUnitario = Convert.ToDouble(CalculaPrecoUnitario(taxa_juros, total_parcelas, Convert.ToDecimal(itemPedido.PrecoBase + itemPedido.ValorFrete))) - itemPedido.ValorFrete;
                        totalPrecoUnitario = Convert.ToDouble(CalculaPrecoUnitario(taxa_juros, total_parcelas, Convert.ToDecimal(itemPedido.PrecoBase)));

                    }
                    else
                        totalPrecoUnitario = precoUnitario;

                    //totalLiquidoCalculado = Math.Round(totalPrecoUnitario, 2) * itemPedido.QuantidadePedido;
                    //totalLiquidoCalculado = Math.Round(totalPrecoUnitario, 5, MidpointRounding.AwayFromZero) * Math.Round(itemPedido.QuantidadePedido, 5, MidpointRounding.AwayFromZero) + rateioFrete;
                    totalLiquidoCalculado = Math.Round(totalPrecoUnitario, 5, MidpointRounding.AwayFromZero) * itemPedido.QuantidadePedido;

                }

                if (itemPedido.ValorDescontoUsuario >= 0)
                {
                    var percentualDesconto = (itemPedido.ValorDescontoUsuario * 100) / totalLiquidoCalculado;
                    itemPedido.PercentualDescontoUsuario = Math.Round(percentualDesconto, 2, MidpointRounding.AwayFromZero);
                }

                if (itemPedido.ValorAcrescimoUsuario >= 0)
                {
                    var percentualAcrescimo = Math.Round((itemPedido.ValorAcrescimoUsuario * 100) / totalLiquidoCalculado, 5, MidpointRounding.AwayFromZero);
                    var aux = Math.Round(totalPrecoUnitario * percentualAcrescimo / 100, 2, MidpointRounding.AwayFromZero);
                    itemPedido.PercentualAcrescimoUsuario = percentualAcrescimo;
                }

                //itemPedido.PrecoUnitario = Math.Round(totalPrecoUnitario, 2);
                //itemPedido.ValorLiquido = Math.Round(totalLiquidoCalculado + itemPedido.ValorAcrescimoUsuario - itemPedido.ValorDescontoUsuario, 2);
                //itemPedido.ValorBruto = Math.Round(totalLiquidoCalculado, 2);
                //itemPedido.ValorBase = itemPedido.QuantidadePedido * Math.Round(itemPedido.PrecoBase, 2);
                itemPedido.PrecoUnitario = Math.Round(totalPrecoUnitario + (rateioFrete / itemPedido.QuantidadePedido), 5, MidpointRounding.AwayFromZero);
                //itemPedido.ValorLiquido = Math.Round((totalLiquidoCalculado + itemPedido.ValorAcrescimoUsuario - itemPedido.ValorDescontoUsuario), 2, MidpointRounding.AwayFromZero);
                //itemPedido.ValorLiquido = Math.Round(totalLiquidoCalculado, 2, MidpointRounding.AwayFromZero) + Math.Round(itemPedido.ValorAcrescimoUsuario, 2, MidpointRounding.AwayFromZero) - Math.Round(itemPedido.ValorDescontoUsuario, 2, MidpointRounding.AwayFromZero);

                itemPedido.ValorLiquido = Math.Round((itemPedido.PrecoBase * itemPedido.QuantidadePedido) + itemPedido.ValorAcrescimoUsuario - itemPedido.ValorDescontoUsuario, 5);

                itemPedido.ValorBruto = totalLiquidoCalculado;
                itemPedido.ValorBase = itemPedido.QuantidadePedido * Math.Round(itemPedido.PrecoBase, 5, MidpointRounding.AwayFromZero);

            }
            else
            {
                itensRemover.Add(itemPedido);
            }
        }

        private double CalcularJurosFrete(List<Parcela> listaParcela, string tipoCalculo, string tipo_especial, string taxa_juros, string total_parcelas, double somaFrete, string dataIniJur = null)
        {
            double vlrJurosFrete = 0;
            DateTime iniJur = Convert.ToDateTime(dataIniJur);

            if (tipoCalculo == "Juros")
            {
                double TaxaJurosCalculada = 0;

                foreach (var itemParcela in listaParcela)
                {
                    TaxaJurosCalculada += CalcularJuros((somaFrete), somaFrete, itemParcela.Percentual, tipo_especial, iniJur,
                        Convert.ToDateTime(itemParcela.DataVencimento), tipoCalculo, somaFrete.ToString(), taxa_juros);

                }

                vlrJurosFrete = Math.Round((somaFrete + TaxaJurosCalculada), 2) - somaFrete;
            }

            else
            {
                vlrJurosFrete = Convert.ToDouble(CalculaPrecoUnitario(taxa_juros, total_parcelas, Convert.ToDecimal(somaFrete))) - somaFrete;
            }

            return Math.Round(vlrJurosFrete, 2);
        }

        public ActionResult AlterarDataEntrega(string dataSelecionada)
        {
            var listItemPedido = TempData["listItemPedido"] as List<PedidoItem>;
            try
            {
                DateTime dataEntrega = Convert.ToDateTime(dataSelecionada);
                foreach (PedidoItem item in listItemPedido)
                {
                    item.DataEntrega = dataSelecionada;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao converter data!");
            }

            TempData["listItemPedido"] = listItemPedido;

            return Json(listItemPedido);
        }

        public double CalcularJuros(double PrecoBase, double PrecoUnitario, double Percentual, string condicaoPagamentoEspecial, DateTime DataPedido, DateTime DataVencimento, string TipoCalculoCondicaoPagamento, string ValorTotalLiquidoSemJuros, string TaxaJuros)
        {
            DateTime dVctPar = DateTime.MinValue;
            var aCodCpg = condicaoPagamentoEspecial;
            var nQtdDia = 0.00;
            var nQtdDiaAux = 0.00;

            var nPerPar = 0.00;
            var nPreLiq = 0.00;
            var nVlrJurTot = 0.00;
            var nTotDia = 0.00;
            var nPreUni = PrecoUnitario;
            var nVlrJur = 0.00;
            double nTxaJur = Convert.ToDouble(TaxaJuros.Replace('.', ','));
            double nPerJur = 0;

            DateTime dataPedido = DataPedido;

            if (dataPedido != DateTime.MinValue)
            {
                dataPedido = DataPedido;

                //se o tipo de calculo for A precisa pegar da data atual e nao mais da data da emissao
                if (TipoCalculoCondicaoPagamento == "A")
                {
                    dataPedido = DateTime.Now;
                }
            }

            var valorLiquidoPedido = ValorTotalLiquidoSemJuros;

            var nTaxaJur = TaxaJuros;

            dVctPar = DataVencimento;

            nQtdDia = CalculaDias(dataPedido, dVctPar);
            nQtdDiaAux = CalculaDias(dataPedido, dVctPar);

            nPerPar = Percentual;

            nPreLiq = PrecoBase;

            if (nQtdDia > 0)
            {
                nTotDia = nTotDia + nQtdDia;

                nQtdDia = nQtdDia / 30;

                nTxaJur = (nTxaJur / 100) + 1;
                nPerJur = Math.Pow(nTxaJur, nQtdDia);

                nPreUni = (nPreLiq * nPerPar) / 100;
                nVlrJur = nPreUni * nPerJur;
                nVlrJur = nVlrJur - nPreUni;

                nVlrJurTot = nVlrJurTot + nVlrJur;
            }

            return nVlrJurTot;
        }

        public int CalculaDias(DateTime data1, DateTime data2)
        {
            TimeSpan ts = data2 - data1;

            int dias = ts.Days;

            return dias;
        }

        //private List<ConsultaProduto> ObterProdutoUnico(List<PedidoItem> listItemPedido, string tipo_especial, string taxa_juros,
        //    string total_parcelas, string agComercial, string agEstoque, string codTabela,
        //    string dataEmissao, string condPgto, string codCli, string transProd, string codRepresentante)
        //{
        //    var demaisItens = new
        //    {
        //        tipo_especial,
        //        taxa_juros,
        //        total_parcelas,
        //        agComercial,
        //        agEstoque,
        //        codTabela,
        //        dataEmissao,
        //        condPgto,
        //        codCli,
        //        transProd,
        //        codRepresentante
        //    };
        //    var sessaoProdutoUnico = Session["ObterProdutoUnico"] as List<ConsultaProduto>;
        //    if (!demaisItens.Equals(Session["demaisItensComparacao"]) || !listItemPedido.Equals(Session["listItemPedidoComparacao"]) || (sessaoProdutoUnico == null || sessaoProdutoUnico.Count != listItemPedido.Count))
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
        //        int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);

        //        foreach (PedidoItem item in listItemPedido)
        //        {
        //            if (item.CodigoProduto == "")
        //                throw new Exception("Não foi possível localizar o código de referência.");

        //            sql.AppendFormat(@"DECLARE @VCodEmp INTEGER = {0},
        //                                  @VCodFil INTEGER = {1},
        //                                  @VCodCli INTEGER = {2},
        //                                  @VCodCpg VARCHAR(6) = '{3}',
        //                                  @VCodPro VARCHAR(14) = '{4}',
        //                                  @VDesPro VARCHAR(100) = '{5}',
        //                                  @VCodDer VARCHAR(7) = '{6}',
        //                                  @VDatEmi VARCHAR(10) = '{7}',
        //                                  @VTnsPro VARCHAR(6) = '{8}'
        //                                SELECT *
        //                                FROM dbo.fnBuscaProdutoV2(@VCodEmp,
        //                                     @VCodFil,
        //                                     @VCodCli,
        //                                     @VCodCpg,
        //                                     @VCodPro,
        //                                     @VDesPro,
        //                                     @VCodDer,
        //                                     @VDatEmi,
        //                                     @VTnsPro) UNION ",
        //                    codEmpFiltroPadrao,
        //                    codFilialFiltroPadrao,
        //                    codCli,
        //                    condPgto,
        //                    item.CodigoProduto,
        //                    item.DescricaoProduto,
        //                    (String.IsNullOrEmpty(item.CodigoDerivacao) ? "" : item.CodigoDerivacao),
        //                    dataEmissao,
        //                    transProd
        //                );
        //        }
        //        sql.Remove(sql.Length - 6, 6);

        //        var resultado = UnidadeTrabalho.ExecuteSql<ConsultaProduto>(sql.ToString(), null).ToList();
        //        Session["ObterProdutoUnico"] = resultado;
        //        Session["demaisItensComparacao"] = demaisItens;
        //        Session["listItemPedidoComparacao"] = listItemPedido;
        //        sessaoProdutoUnico = Session["ObterProdutoUnico"] as List<ConsultaProduto>;
        //    }
        //    return sessaoProdutoUnico;
        //}

        public ActionResult ObterProduto(string codPedido, string codProduto, string descProduto, string tipo_especial, string taxa_juros, string total_parcelas, string agComercial = null, string agEstoque = null, string codTabela = null, string dataEmissao = null, string condPgto = null, string codCli = null, string transProd = null, string codDerivacao = null, string codRepresentante = null, string qtdProduto = null, double vlrAcre = 0, double percAcre = 0, double vlrDesc = 0, double percDesc = 0)
        {
            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"].ToString());
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"].ToString());

            int codigoPedido = !string.IsNullOrEmpty(codPedido) ? Convert.ToInt32(codPedido) : 0;

            //ConsultaProduto resultado = ObterProdutoUnico(codEmpFiltroPadrao, codFilialFiltroPadrao, codProduto, descProduto, dataEmissao, codCli, transProd, condPgto, codDerivacao);
            var resultado = PesquisarProduto(codEmpFiltroPadrao, codFilialFiltroPadrao, codProduto, descProduto, "MGMG");

            PedidoItem itemPedido = ConverteProdutoParaPedidoItem(resultado.FirstOrDefault(), tipo_especial, taxa_juros, total_parcelas, codEmpFiltroPadrao, codFilialFiltroPadrao, codDerivacao, Convert.ToDouble(qtdProduto), vlrAcre, percAcre, vlrDesc, percDesc);

            if (itemPedido.PrecoBase > 0)
                return Json(itemPedido, JsonRequestBehavior.AllowGet);

            return Json(itemPedido.PrecoBase, JsonRequestBehavior.AllowGet);
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

        private PedidoItem ConverteProdutoParaPedidoItem(ProdutoConsultar item, string tipo_especial, string taxa_juros, string total_parcelas, int codEmpFiltroPadrao, int codFilialFiltroPadrao, string codDerivacao = null, double qtdeProduto = 0, double vlrAcre = 0, double percAcre = 0, double vlrDesc = 0, double percDesc = 0)
        {
            if (item != null)
            {
                PedidoItem p = new PedidoItem();

                p.CodigoProduto = (item.CodPro != null) ? item.CodPro : "";
                //p.TemDerivacao = item.TemDer;
                p.DescricaoProduto = (item.DesPro != null) ? item.DesPro : "";
                p.UnidadeMedida = (item.UniMed != null) ? item.UniMed : "";
                //p.CodigoAgrupamento = (!string.IsNullOrEmpty(item.Codage)) ? item.Codage : "";
                //p.Agrupamento = (item.Desage != null) ? item.Desage : "";
                p.PrecoBase = (item.PreBas > 0) ? Convert.ToDouble(item.PreBas) : 0;
                p.PrecoUnitario = tipo_especial == "N" ? Convert.ToDouble(CalculaPrecoUnitario(taxa_juros, total_parcelas, item.PreBas)) : Convert.ToDouble(item.PreBas);
                p.codTabela = (!string.IsNullOrEmpty(item.CodTpr)) ? item.CodTpr : "";
                p.CodigoTRP = (!string.IsNullOrEmpty(item.CodTpr)) ? item.CodTpr : "";
                p.DerivacaoSelecionada = item.CodDer;
                //p.DepositoSelecionado = item.CodDep;
                //p.CodigoDep = item.Coddep;
                p.CodigoDerivacao = item.CodDer;
                p.SalEst = Convert.ToDouble(item.SalEst);
                p.DataEntrega = DateTime.Today.ToShortDateString();
                p.QuantidadePedido = qtdeProduto;
                p.ValorAcrescimoUsuario = vlrAcre;
                p.PercentualAcrescimoUsuario = percAcre;
                p.ValorDescontoUsuario = vlrDesc;
                p.PercentualDescontoUsuario = percDesc;
                p.ValorLiquido = ((p.PrecoBase * p.QuantidadePedido) + p.ValorAcrescimoUsuario) - p.ValorDescontoUsuario;

                var resultadoDerivacao = UnidadeTrabalho.ObterTodos<Derivacao>().Where(x => x.CodEmpresa == codEmpFiltroPadrao
                                                                                            && x.CodProduto == item.CodPro).ToList();

                List<Derivacao> listaDerivacao = new List<Derivacao>();

                foreach (var der in resultadoDerivacao)
                {
                    Derivacao prod = new Derivacao();
                    prod.Codigo = der.Codigo;
                    prod.Nome = der.Nome;

                    listaDerivacao.Add(prod);
                }

                var produtoDeposito = UnidadeTrabalho.ObterTodos<ProdutoDeposito>().Where(pd => pd.Empresa == codEmpFiltroPadrao &&
                                                                                                pd.Filial == codFilialFiltroPadrao &&
                                                                                                pd.Produto == item.CodPro);
                if (!string.IsNullOrWhiteSpace(codDerivacao))
                {
                    produtoDeposito = produtoDeposito.Where(pd => pd.Derivacao.Equals(codDerivacao));
                }
                else if (resultadoDerivacao.Count() > 0)
                {
                    if (!string.IsNullOrWhiteSpace(resultadoDerivacao.FirstOrDefault().Codigo))
                    {
                        string codDeri = resultadoDerivacao.FirstOrDefault().Codigo;
                        produtoDeposito = produtoDeposito.Where(pd => pd.Derivacao.Equals(codDeri));
                    }
                }

                p.ListaDerivacao = listaDerivacao;
                p.ListaDepositos = produtoDeposito.ToList();

                return p;
            }
            else
                return null;
        }

        private PedidoItem ConverteListaParaPedidoItem(ConsultaProduto item, string tipo_especial, string taxa_juros, string total_parcelas, int codEmpFiltroPadrao, int codFilialFiltroPadrao, string codDerivacao = null, double qtdeProduto = 0, double vlrAcre = 0, double percAcre = 0, double vlrDesc = 0, double percDesc = 0)
        {
            if (item != null)
            {
                PedidoItem p = new PedidoItem();

                p.CodigoProduto = (item.Codpro != null) ? item.Codpro : "";
                p.TemDerivacao = item.Temder;
                p.DescricaoProduto = (item.Despro != null) ? item.Despro : "";
                p.UnidadeMedida = (item.Unimed != null) ? item.Unimed : "";
                p.CodigoAgrupamento = (!string.IsNullOrEmpty(item.Codage)) ? item.Codage : "";
                p.Agrupamento = (item.Desage != null) ? item.Desage : "";
                p.PrecoBase = (item.Prebas > 0) ? Convert.ToDouble(item.Prebas) : 0;
                p.PrecoUnitario = tipo_especial == "N" ? Convert.ToDouble(CalculaPrecoUnitario(taxa_juros, total_parcelas, item.Prebas)) : Convert.ToDouble(item.Prebas);
                p.codTabela = (!string.IsNullOrEmpty(item.Codtpr)) ? item.Codtpr : "";
                p.CodigoTRP = (!string.IsNullOrEmpty(item.Codtpr)) ? item.Codtpr : "";
                p.DerivacaoSelecionada = item.Codder;
                p.DepositoSelecionado = item.Coddep;
                p.CodigoDep = item.Coddep;
                p.CodigoDerivacao = item.Codder;
                p.SalEst = Convert.ToDouble(item.Salest);
                p.DataEntrega = DateTime.Today.ToShortDateString();
                p.QuantidadePedido = qtdeProduto;
                p.ValorAcrescimoUsuario = vlrAcre;
                p.PercentualAcrescimoUsuario = percAcre;
                p.ValorDescontoUsuario = vlrDesc;
                p.PercentualDescontoUsuario = percDesc;
                p.ValorLiquido = ((p.PrecoBase * p.QuantidadePedido) + p.ValorAcrescimoUsuario) - p.ValorDescontoUsuario;

                var resultadoDerivacao = UnidadeTrabalho.ObterTodos<Derivacao>().Where(x => x.CodEmpresa == codEmpFiltroPadrao
                                                                                            && x.CodProduto == item.Codpro).ToList();

                List<Derivacao> listaDerivacao = new List<Derivacao>();

                foreach (var der in resultadoDerivacao)
                {
                    Derivacao prod = new Derivacao();
                    prod.Codigo = der.Codigo;
                    prod.Nome = der.Nome;

                    listaDerivacao.Add(prod);
                }

                var produtoDeposito = UnidadeTrabalho.ObterTodos<ProdutoDeposito>().Where(pd => pd.Empresa == codEmpFiltroPadrao &&
                                                                                                pd.Filial == codFilialFiltroPadrao &&
                                                                                                pd.Produto == item.Codpro);
                if (!string.IsNullOrWhiteSpace(codDerivacao))
                {
                    produtoDeposito = produtoDeposito.Where(pd => pd.Derivacao.Equals(codDerivacao));
                }
                else if (resultadoDerivacao.Count() > 0)
                {
                    if (!string.IsNullOrWhiteSpace(resultadoDerivacao.FirstOrDefault().Codigo))
                    {
                        string codDeri = resultadoDerivacao.FirstOrDefault().Codigo;
                        produtoDeposito = produtoDeposito.Where(pd => pd.Derivacao.Equals(codDeri));
                    }
                }

                p.ListaDerivacao = listaDerivacao;
                p.ListaDepositos = produtoDeposito.ToList();

                return p;
            }
            else
                return null;
        }

        private decimal CalculaPrecoUnitario(string taxa_juros, string total_parcelas, decimal preco_base)
        {
            double nVlrTot = 0;
            double nTaxJur = taxa_juros != string.Empty ? Convert.ToDouble(taxa_juros.Replace(".", ",")) : 0;
            int nQtdPar = total_parcelas != string.Empty ? Convert.ToInt16(total_parcelas) : 0;

            if (nTaxJur > 0)
            {
                // 1 Parte do Calculo @@@
                double nCalc01;
                nCalc01 = (1 + nTaxJur / 100);
                nCalc01 = Math.Pow(nCalc01, nQtdPar);
                nCalc01 = (nTaxJur / 100) * nCalc01;


                //2 Parte do Calculo @@@
                double nCalc02;
                nCalc02 = (1 + nTaxJur / 100);
                nCalc02 = Math.Pow(nCalc02, nQtdPar);
                nCalc02 = nCalc02 - 1;

                // 3 Parte do Calculo @@@
                double nCalc03;
                nCalc03 = Convert.ToDouble(preco_base) * nCalc01;
                nCalc03 = nCalc03 / nCalc02;

                // VALORES FINAIS @@@
                nVlrTot = nCalc03 * nQtdPar;
            }
            else
                return preco_base;

            return Convert.ToDecimal(nVlrTot);
        }

        public JsonResult ObterProdutoPesquisar(string codProduto, string descProduto, string dataEmissao = null, string codCli = null, string transProd = null, string codCondPgto = null, string codDer = null)
        {
            int codEmpFiltroPadrao = Convert.ToInt32(Session["empAti"]);
            int codFilialFiltroPadrao = Convert.ToInt32(Session["filAti"]);

            List<ConsultaProduto> resultadoConsulta = PesquisarProduto(codEmpFiltroPadrao, codFilialFiltroPadrao, codProduto, descProduto, dataEmissao, codCli, transProd, codCondPgto, codDer);

            return Json(resultadoConsulta);
        }

        private List<ConsultaProduto> PesquisarProduto(int codEmpFiltroPadrao, int codFilialFiltroPadrao, string codProduto, string descProduto, string dataEmissao = null, string codCli = null, string transProd = null, string codCondPgto = null, string codDer = null)
        {
            string query = string.Format(@"DECLARE @VCodEmp INTEGER = {0},
		                                            @VCodFil INTEGER = {1},
		                                            @VCodCli INTEGER = {2},
		                                            @VCodCpg VARCHAR(6) = '{3}',
		                                            @VCodPro VARCHAR(14) = '{4}',
		                                            @VDesPro VARCHAR(100) = '{5}',
		                                            @VCodDer VARCHAR(7) = '{6}',
		                                            @VDatEmi VARCHAR(10) = '{7}',
		                                            @VTnsPro VARCHAR(6) = '{8}'
                                            SELECT *
                                            FROM dbo.fnBuscaProdutoV2(@VCodEmp,
					                                            @VCodFil,
					                                            @VCodCli,
					                                            @VCodCpg,
					                                            @VCodPro,
					                                            @VDesPro,
					                                            @VCodDer,
					                                            @VDatEmi,
					                                            @VTnsPro);", codEmpFiltroPadrao,
                                                                             codFilialFiltroPadrao,
                                                                             codCli,
                                                                             codCondPgto,
                                                                             removeAcentuacao(codProduto),
                                                                             !string.IsNullOrEmpty(descProduto) ? removeAcentuacao(descProduto) : string.Empty,
                                                                             codDer,
                                                                             dataEmissao,
                                                                             transProd);

            var resultado = UnidadeTrabalho.ExecuteSql<ConsultaProduto>(query);

            return resultado.ToList();
        }

        private ConsultaProduto ObterProdutoUnico(int codEmpFiltroPadrao, int codFilialFiltroPadrao, string codProduto, string descProduto, string dataEmissao = null, string codCli = null, string transProd = null, string codCondPgto = null, string codDer = null)
        {
            string query = string.Format(@"DECLARE @VCodEmp INTEGER = {0},
		                                            @VCodFil INTEGER = {1},
		                                            @VCodCli INTEGER = {2},
		                                            @VCodCpg VARCHAR(6) = '{3}',
		                                            @VCodPro VARCHAR(14) = '{4}',
		                                            @VDesPro VARCHAR(100) = '{5}',
		                                            @VCodDer VARCHAR(7) = '{6}',
		                                            @VDatEmi VARCHAR(10) = '{7}',
		                                            @VTnsPro VARCHAR(6) = '{8}'
                                            SELECT *
                                            FROM dbo.fnBuscaProdutoV2(@VCodEmp,
					                                            @VCodFil,
					                                            @VCodCli,
					                                            @VCodCpg,
					                                            @VCodPro,
					                                            @VDesPro,
					                                            @VCodDer,
					                                            @VDatEmi,
					                                            @VTnsPro);", codEmpFiltroPadrao,
                                                                             codFilialFiltroPadrao,
                                                                             codCli,
                                                                             codCondPgto,
                                                                             removeAcentuacao(codProduto),
                                                                             !string.IsNullOrEmpty(descProduto) ? removeAcentuacao(descProduto) : string.Empty,
                                                                             codDer,
                                                                             dataEmissao,
                                                                             transProd);

            var resultado = UnidadeTrabalho.ExecuteSql<ConsultaProduto>(query);

            return resultado.FirstOrDefault();
        }

        public ActionResult ObterCondicaoPagamento(string codFiltro, string codFormaPagamento)
        {
            string msgCondPgto = string.Empty;
            CondicaoPagamento resultado = UnidadeTrabalho.ObterTodos<CondicaoPagamento>().Where(x => x.Codigo == codFiltro && x.FormaPagamento.Codigo == codFormaPagamento).FirstOrDefault();

            if (resultado == null)
                msgCondPgto = "A Condição de Pagamento " + codFiltro + " não está disponível.";

            decimal TaxaJuros = 0;

            if (resultado != null)
            {
                if (resultado.TipoEspecial == "N")
                {
                    TaxaJuros = (resultado.TaxaAcrescimoFinanceiro > 0) ? resultado.TaxaAcrescimoFinanceiro : 0;
                }
                else
                {
                    TaxaJuros = (resultado.TaxaJuros > 0) ? resultado.TaxaJuros : 0;
                }

                Session["ObterProdutoUnico"] = null;
                Session["demaisItensComparacao"] = null;
                Session["listItemPedidoComparacao"] = null;

                return Json(new
                {
                    Codigo = resultado.Codigo,
                    Descricao = (resultado.Descricao != null) ? resultado.Descricao.ToString() : "",
                    Abreviacao = (resultado.Abreviacao != null) ? resultado.Abreviacao.ToString() : "",
                    TipoEspecialCodigo = (resultado.TipoEspecial != null) ? resultado.TipoEspecial.ToString() : "",
                    TotalParcelas = resultado.QuantidadeParcelas > 0 ? resultado.QuantidadeParcelas : 0,
                    TaxaJuros = TaxaJuros,
                    ManutencaoParcela = resultado.ManutencaoParcela
                }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new
                {
                    retornoCondPgto = msgCondPgto
                });
        }

        public ActionResult ValidaDesconto(string login, string senha)
        {
            Response.Cache.SetNoStore();
            if (login == null)
                throw new Exception("Usuário e/ou senha inválido, tente novamente.");

            WS_Usuario.sapiens_SyncMCWFUsersClient ws = new WS_Usuario.sapiens_SyncMCWFUsersClient();
            if (ws == null)
                throw new Exception("Não foi possível realizar uma conexão com o serviço de autenticação.");

            WS_Usuario.mcwfUsersAuthenticateJAASIn request = new WS_Usuario.mcwfUsersAuthenticateJAASIn();
            request.pmUserName = login;
            request.pmUserPassword = senha;
            //request.tipAce = "D";

            var Resultado = ws.AuthenticateJAAS(login, senha, 0, request);

            if (Resultado.pmLogged == "0")
            {
                PermissaoUsuario usuario = UnidadeTrabalho.ObterTodos<PermissaoUsuario>().Where(x => x.NOMUSU == login).FirstOrDefault();
                return Json(new
                {
                    venPds = usuario.VENPDS,
                    usuario = usuario.CODUSU
                });
            }
            else
            {
                throw new Exception(Resultado.erroExecucao);
            }
        }

        public ActionResult Recalcular(double vlrArredondamento, double vlrDesconto)
        {
            TempData.Keep();

            var listaItens = TempData["listItemPedido"] as List<PedidoItem>;

            if (listaItens == null || listaItens.Count <= 0)
            {
                return Json(new
                {
                    Error = true,
                    Message = "Nenhum item para aplicar recálculo"
                });
            }
            else
            {
                int qtdItens = listaItens.Count;

                foreach (var item in listaItens)
                {
                    item.ValorDescontoUsuario = (vlrDesconto / qtdItens);
                    item.ValorLiquido = (item.PrecoUnitario * item.QuantidadePedido) + (vlrArredondamento / qtdItens) - (vlrDesconto / qtdItens);
                }
            }

            return Json(listaItens);
        }

        public ActionResult TrocaFilial(int codFilial)
        {
            CultureInfo cultura = CultureInfo.GetCultureInfo("pt-BR");
            int codUsuarioLogado = Convert.ToInt32(Session["codUsu"]);
            string usuarioLogado = Session["usuarioLogado"].ToString();
            string senhaUsuLogado = Session["senhaUsuLogado"].ToString();
            string codEmpPadrao = Session["empAti"].ToString();

            GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient ws = new GravarPedido.sapiens_Synccom_senior_g5_co_cafeeira_pedidosvendaClient();

            if (ws == null)
                throw new Exception("Não foi possível realizar uma conexão com o serviço de autenticação.");

            GravarPedido.pedidosvendaTrocarFilialIn trocarFilial = new GravarPedido.pedidosvendaTrocarFilialIn();
            trocarFilial.codEmp = codEmpPadrao;
            trocarFilial.codFil = codFilial.ToString();
            trocarFilial.codUsu = codUsuarioLogado.ToString();

            GravarPedido.pedidosvendaTrocarFilialOut resultado = ws.TrocarFilial(usuarioLogado, senhaUsuLogado, 0, trocarFilial);

            //GravarPedido.pedidosvendaGravarPedidoOut resultado = ws.GravarPedido(usuarioLogado, senhaUsuarioLogado, 0, pedidoIn);
            //var respostaPedido = new GravarPedido.pedidosvendaGravarPedidoOut();

            if (!string.IsNullOrEmpty(resultado.erroExecucao))
            {
                return Json(new
                {
                    success = false,
                    erro = resultado.erroExecucao
                });
            }

            PermissaoUsuario usuario = UnidadeTrabalho.ObterTodos<PermissaoUsuario>().Where(x => x.CODUSU == codUsuarioLogado).FirstOrDefault();
            Session["filAti"] = usuario.FILATI.ToString();
            Session["filAtiNome"] = usuario.FILATI.ToString() + " " + usuario.SIGFIL.ToString();

            return Json(resultado.ToString());
        }

        private string removeAcentuacao(string palavra)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(palavra);
            return System.Text.Encoding.UTF8.GetString(bytes).ToUpper();
        }

        private string RetornaSituacaoPedido(int intSituacao)
        {
            string strRetorno = "";

            switch (intSituacao)
            {
                case 1:
                    strRetorno = "Aberto total";
                    break;
                case 2:
                    strRetorno = "Aberto parcial";
                    break;
                case 3:
                    strRetorno = "Suspenso";
                    break;
                case 4:
                    strRetorno = "Liquidado";
                    break;
                case 5:
                    strRetorno = "Cancelado";
                    break;
                case 6:
                    strRetorno = "Aguardando integração WMAS";
                    break;
                case 7:
                    strRetorno = "Em transmissão";
                    break;
                case 8:
                    strRetorno = "Preparação análise ou NF";
                    break;
                case 9:
                    strRetorno = "Não fechado";
                    break;
                default:
                    strRetorno = "Situação não identificada";
                    break;
            }

            return strRetorno;
        }

        public bool UsuarioLogado()
        {
            return Session["codUsu"] != null;
        }

        public string PreparaValor(string valor)
        {
            string valorTratado = valor.Replace(".", "");

            return valorTratado;
        }

        /// <summary>
        /// Método criado para fazer uma requisição ao servidor, de tempo em tempo, com isso não perde a sessão.
        /// </summary>
        /// <returns></returns>
        public string Ping()
        {
            return "OK";
        }
    }
}
