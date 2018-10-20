using SIS.Framework.ActionResults.Base;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods.Base;
using SIS.Framework.Controllers;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var controllerName = string.Empty;
            var actionName = string.Empty;
            var requestMethod = request.RequestMethod.ToString();


            if (request.Url == "/")
            {
                controllerName = "Home";
                actionName = "index";
            }
            else
            {
                var requestUrlSplit = request.Url
                .Split("/", StringSplitOptions.RemoveEmptyEntries);

                controllerName = requestUrlSplit[0];
                actionName = requestUrlSplit[1];
            }


            // controller
            var controller = this.GetController(controllerName, request);

            //action
            var action = this.GetAction(requestMethod, controller, actionName);

            if (controller == null || action == null)
            {
                throw new ArgumentException();
            }
            return PrepareResponse(controller,action);
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            IActionResult actionResult = (IActionResult)action.Invoke(controller, null);
            string invocatonResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocatonResult,HttpResponseStatusCode.Ok);
            }
            else if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocatonResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported.");

            }
        }

        private MethodInfo GetAction(string requestMethod, Controller controller, string actionName)
        {

            var actions = this.GetSuitableMethods(controller, actionName);

            if (!actions.Any())
            {
                return null;
            }

            foreach (var action in actions)
            {
                var httpMethodAttributes = action
                    .GetCustomAttributes()
                    .Where(x => x is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>()
                    .ToList();

                if (!httpMethodAttributes.Any() && requestMethod.ToLower()=="get")
                {
                    return action;
                }

                foreach (var httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.isValid(requestMethod))
                    {
                        return action;
                    }
                }                
            }


            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }
            var methods = controller.GetType().GetMethods()
                .Where(mi => mi.Name.ToLower() == actionName.ToLower());
            return methods;
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                return null;
            }

            var fullQualifiedControllerName =
                $"{MvcContext.Get.AssemblyName}" +
                $".{MvcContext.Get.ControllerFolder}" +
                $".{controllerName}" +
                $"{MvcContext.Get.ControllerSuffix}, " +
                $"{MvcContext.Get.AssemblyName}";

            var controllerType = Type.GetType(fullQualifiedControllerName);

            var controller = (Controller)Activator.CreateInstance(controllerType);

            return controller;
        }
    }
}
