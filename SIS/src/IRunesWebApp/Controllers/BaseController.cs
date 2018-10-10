using IRunesWebApp.Data;
using Services;
using SIS.HTTP.Cookies;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace IRunesWebApp.Controllers
{
    public class BaseController
    {
        private const string RootDirectoryRelativePath = "../../../";
        private const string DirectorySeparator = "/";
        private const string ViewsFolderName = "Views";
        private const string HtmlExtention = ".html";
        private const string ControllerDefaultNameEnd = "Controller";

        protected IRunesDbContext Context { get;private set; }
        protected readonly UserCookieService cookieService;

        protected IDictionary<string,string> ViewBag { get; set; }

        public BaseController()
        {
            this.Context = new IRunesDbContext();
            this.cookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
            
    }

        public void SignInUser(string username,IHttpRequest request)
        {
            request.Session.AddParameter("username", username);
            var userCookieValue = this.cookieService.GetUserCookie(username);
            request.Cookies.Add(new HttpCookie("IRunes_Auth", userCookieValue));
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }
        private string GetCurrentControllerName() => this.GetType().Name.Replace(ControllerDefaultNameEnd, string.Empty);
        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            

            string filePath = RootDirectoryRelativePath +
                ViewsFolderName +
                DirectorySeparator +
                this.GetCurrentControllerName() +
                DirectorySeparator +
                viewName +
                HtmlExtention;

            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"View {viewName} not found.",SIS.HTTP.Enums.HttpResponseStatusCode.NotFound);
            }
            var fileContent = File.ReadAllText(filePath);

            foreach (var viewBagKey in ViewBag.Keys)
            {
                if (fileContent.Contains($"{{{{{viewBagKey}}}}}"))
                {
                    fileContent = fileContent.Replace($"{{{{{viewBagKey}}}}}",ViewBag[viewBagKey]);
                }
               
            }
            var response = new HtmlResult(fileContent, SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
            return response;
        }
    }
}


