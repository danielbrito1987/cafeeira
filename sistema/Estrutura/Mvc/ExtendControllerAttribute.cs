using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Estrutura.Exceptions;
using Estrutura.Persistence;

namespace Estrutura.Mvc
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ExtendControllerAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string url = string.Format("/{0}/{1}", filterContext.ActionDescriptor.ActionName, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);

            IUnidadeTrabalho unidadeTrabalho = ((IControllerBase)filterContext.Controller).UnidadeTrabalho;

            if (filterContext.Result is JsonResult || filterContext.Result is EmptyResult)
            {
                JsonResult result = new JsonResult();

                string msg = string.Empty;
                bool success = true;

                if (filterContext.Exception != null)
                {
                    success = false;

                    Exception exception = filterContext.Exception;

                    filterContext.ExceptionHandled = true;

                    msg = exception is CoreException ?
                                    exception.Message :
                                        "Houve um erro no servidor. Contate o administrador ou tente novamente mais tarde.";

                    //if (!(exception is CoreException))
                    //{
                    //    Exception ex = exception;

                    //    if (ex.InnerException != null)
                    //        ex = ex.InnerException;

                    //    LogError logError = new LogError
                    //    {
                    //        Message = ex.Message,
                    //        StackTrace = ex.StackTrace,
                    //        Url = url,
                    //        DataRegistro = DateTime.Now,
                    //        UserName = filterContext.HttpContext.User.Identity.Name
                    //    };

                    //    unidadeTrabalho.Salvar<LogError>(logError);
                    //}
                }

                result.MaxJsonLength = int.MaxValue;

                result.Data = new
                {
                    Success = success,
                    Data = filterContext.Result is JsonResult ? (filterContext.Result as JsonResult).Data : filterContext.Result,
                    Message = msg
                };

                if (filterContext.Result is JsonResult)
                    result.ContentType = (filterContext.Result as JsonResult).ContentType;

                if (filterContext.Result is FileContentResult)
                    result.ContentType = (filterContext.Result as FileContentResult).ContentType;

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                filterContext.Result = result;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
