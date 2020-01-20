using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estrutura.Exceptions;
using Estrutura.Repositories;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net;
using System.Web.Mvc;

namespace Estrutura.Mvc
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            base.OnAuthorization(filterContext);


            IUnidadeTrabalho unidadeTrabalho = ((IControllerBase)filterContext.ControllerContext.Controller).UnidadeTrabalho;

            IRepositorioPermissao repPermissao = unidadeTrabalho.Fabrica.ObterRepositorio<IRepositorioPermissao>(unidadeTrabalho);


            HttpActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            HttpControllerDescriptor controllerDescriptor = actionDescriptor.ControllerDescriptor;

            if (!filterContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() &&
                !filterContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                string url = string.Format("/{0}/{1}", controllerDescriptor.ControllerName, actionDescriptor.ActionName);

                try
                {
                    int aaa;
                    //filterContext.Request = filterContext.Request.CreateResponse(HttpStatusCode.OK, new SuccessResult() { Message = dataException.Message, Success = false }); ;
                    //verifica se o usuário está autenticado
                    if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                        aaa = 1;//new RedirectResult(Estrutura.Seguranca.Config.UrlLogin);*/
                    else
                    {
                        /*if (!repPermissao.PossuiPermissaoPagina(url))
                            throw new CoreException("Acesso negado. Você não possui permissão para acessar a página selecionada. " + url);
*/
                        //if (url != "/MonitorarAcessos/ObterTodos")
                        //    registrarAudit(filterContext.HttpContext.User.Identity.Name, unidadeTrabalho, url);
                    }
                }
                catch (CoreException ex)
                {
                    /* filterContext.Result = new JsonResult
                     {
                         JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                         Data = new { Success = false, Message = ex.Message },
                         ContentType = "application/json"
                     };*/
                }
            }
        }

        public void registrarAudit(string userIdentName, IUnidadeTrabalho unidadeTrabalho, string url)
        {
            try
            {
                unidadeTrabalho.BeginTransaction();

                //IRepositorioAuditoria repAudit = Fabrica.Instancia.ObterRepositorio<IRepositorioAuditoria>(unidadeTrabalho);
                //IRepositorioAcao repAcao = Fabrica.Instancia.ObterRepositorio<IRepositorioAcao>(unidadeTrabalho);
                //IRepositorioUsuario repUsuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(unidadeTrabalho);

                //Acao acao = repAcao.ObterPorUrl(url);
                //Usuario usuario = repUsuario.ObterPorLogin(userIdentName);

                //if (acao == null)
                //    return;

                //Auditoria audit = new Auditoria
                //{
                //    Acao = acao,
                //    Url = url,
                //    Usuario = usuario,
                //    DataRegistro = DateTime.Now,
                //    Descricao = acao != null ? !string.IsNullOrEmpty(acao.Descricao) ? acao.Descricao : string.Empty : string.Empty
                //};

                //repAudit.Salvar(audit);
                unidadeTrabalho.Commit();
            }
            catch (Exception ex)
            {
                unidadeTrabalho.Rollback();
                throw ex;
            }
            finally
            {
                unidadeTrabalho.Dispose();
            }
        }
    }
}
