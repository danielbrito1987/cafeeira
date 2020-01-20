using Estrutura;
using Estrutura.Mvc;
using System;

namespace VIX.API.Helpers.Estrutura
{
    /*
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TransactionAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            IUnidadeTrabalho unidadeTrabalho = ((IControllerBase)filterContext.ControllerContext.Controller).UnidadeTrabalho;

            unidadeTrabalho.BeginTransaction();
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            IUnidadeTrabalho unidadeTrabalho = ((IControllerBase)filterContext.ActionContext.ControllerContext.Controller).UnidadeTrabalho;

            try
            {
                base.OnActionExecuted(filterContext);

                if (filterContext.Exception != null)
                    unidadeTrabalho.Rollback();
                else unidadeTrabalho.Commit();

            }
            catch (Exception ex) { throw ex; }
            finally
            {
                unidadeTrabalho.Dispose();
            }
        }
    }*/
}