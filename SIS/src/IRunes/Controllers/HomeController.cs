using IRunes.Extensions;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.View("home/indexloggedout", request);
            }

            Dictionary<string, string> dict = new Dictionary<string, string>
            { { "Username", request.GetUsername() } };

            return this.View("home/indexloggedin", request, dict);
        }
    }
}
