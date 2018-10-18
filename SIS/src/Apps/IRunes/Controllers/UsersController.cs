using IRunes.Extensions;
using IRunes.Models;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IRunes.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        private readonly IUserService userService;

        public UsersController()
        {
            this.hashService = new HashService();
            this.userService = new UserService();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            if (request.IsLoggedIn())
            {
                return this.Redirect("/");
            }

            return this.View("users/login", request);
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            if (request.IsLoggedIn())
            {
                return this.Redirect("/");
            }

            return this.View("users/register", request);
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            if (request.IsLoggedIn())
            {
                return this.Redirect("/");
            }

            string login = (string)request.FormData["login"];
            string password = (string)request.FormData["password"];

            User user = this.userService.GetUser(login);

            if (user == null)
            {
                return this.Error("Invalid login or password entered!", HttpResponseStatusCode.Forbidden, request);
            }

            string base64Hash = user.HashedPassword;

            if (!this.hashService.IsPasswordValid(password, base64Hash))
            {
                return this.Error("Invalid login or password", HttpResponseStatusCode.Forbidden, request);
            }

            request.Session.AddParameter("username", user.Username);
            return this.Redirect("/");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            if (request.IsLoggedIn())
            {
                return this.Redirect("/");
            }

            string username = (string)request.FormData["username"];
            string password = (string)request.FormData["password"];
            string confirmPassword = (string)request.FormData["confirmPassword"];
            string email = (string)request.FormData["email"];

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email))
            {
                return this.Error("All fields are required", HttpResponseStatusCode.BadRequest, request);
            }

            if (!Regex.IsMatch(username, "^[A-Za-z0-9_.-]+$"))
            {
                return this.Error("Username can only contain the following characters: A-Z a-z 0-9 - . _",
                    HttpResponseStatusCode.BadRequest, request);
            }

            if (password != confirmPassword)
            {
                return this.Error("Passwords do not match", HttpResponseStatusCode.BadRequest, request);
            }

            if (this.userService.UserExists(username))
            {
                return this.Error("Username is already taken", HttpResponseStatusCode.BadRequest, request);
            }

            if (this.userService.EmailIsTaken(email))
            {
                return this.Error("An account with this email already exists", HttpResponseStatusCode.BadRequest,
                    request);
            }

            string base64Hash = this.hashService.HashPassword(password, this.hashService.GenerateSalt());

            this.userService.CreateUser(username, base64Hash, email);

            request.Session.AddParameter("username", username);

            IHttpResponse response = this.Redirect("/");
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (request.IsLoggedIn())
            {
                request.Session.RemoveParameter("username");
            }

            IHttpResponse response = this.Redirect("/");
            return response;
        }
    }


}



