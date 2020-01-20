using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ninject;

namespace Estrutura.Mvc
{
    public class NinjectFilterAttribute : FilterAttributeFilterProvider
    {
        private readonly IKernel _kernel;

        public NinjectFilterAttribute(IKernel kernel)
        {
            this._kernel = kernel;
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetControllerAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                this._kernel.Inject(attribute);
            }

            return attributes;
        }

        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetActionAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                this._kernel.Inject(attribute);
            }

            return attributes;
        }
    }
}
