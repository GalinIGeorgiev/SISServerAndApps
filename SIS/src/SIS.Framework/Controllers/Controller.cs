using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SIS.Framework.Controllers
{
    public abstract class Controller
    {
        protected Controller() { }

        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);

            var viewFullyQualifiedName = ControllerUtilities
                .GetViewFullyQualifiedName(controllerName,viewName);

            var view = new View(viewFullyQualifiedName);
            return new ViewResult(view);
        }

        protected IRedirectable RedirectAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
