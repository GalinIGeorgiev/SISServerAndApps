using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services.Contracts
{
    public interface IUserService
    {
        bool UserExists(string username);

        bool EmailIsTaken(string email);

        void CreateUser(string username, string passwordHashed, string email);

        User GetUser(string login);
    }
}
