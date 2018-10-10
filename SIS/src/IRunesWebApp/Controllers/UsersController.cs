using IRunesWebApp.Model;
using Services;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunesWebApp.Controllers
{
    public class UsersController : BaseController

    {
        private readonly HashService hashService;
        

        public UsersController()
        {
            this.hashService = new HashService();
           
        }
        public IHttpResponse Login(IHttpRequest request) => this.View();
        public IHttpResponse Register(IHttpRequest request) => this.View();

        public IHttpResponse PostLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();

            var hashedPassword = hashService.Hash(password);

            var user = this.Context.Users
                .Where(x => x.UserName == username && x.HashPassword == hashedPassword)
                .FirstOrDefault();

            if (user == null)
            {
                return new RedirectResult("/login");
            }

            this.SignInUser(username ,request);
            return new RedirectResult("/");
        }

        public IHttpResponse PostRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmpassword"].ToString();

            if (string.IsNullOrEmpty(username) || username.Length < 4)
            {
                return new BadRequestResult("Please provide valid username with length more than 4", HttpResponseStatusCode.BadRequest);
            }

            if (password != confirmPassword)
            {
                return new BadRequestResult("Passwords do not match", HttpResponseStatusCode.SeeOther);
            }
            //Hash
            var hashedPassword = hashService.Hash(password);

            var user = new User
            {

                UserName = username,
                HashPassword = hashedPassword
            };
            this.Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges(); 
            }
            catch (Exception e)
            {

                return new BadRequestResult(e.Message,
                    HttpResponseStatusCode.InternalServerError);
            }
            this.SignInUser(username, request);

            return new RedirectResult("/");
        }
    }
}

