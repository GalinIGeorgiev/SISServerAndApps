using IRunes.Data;
using IRunes.Models;
using IRunes.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Services
{
    public class UserService : IUserService
    {
        public void CreateUser(string username, string passwordHashed, string email)
        {
            User currentUser = new User
            {
                Username = username,
                HashedPassword = passwordHashed,
                Email = email
            };

            using (IRunesDbContext db = new IRunesDbContext())
            {
                db.Users.Add(currentUser);
                db.SaveChanges();
            }
        }

        public bool EmailIsTaken(string email)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Users.Any(u => u.Email == email);
            }
        }

        public User GetUser(string loginCredentials)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username.ToLower() == loginCredentials || u.Email.ToLower() == loginCredentials);
            }
        }

        public bool UserExists(string username)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }
    }
}
