using IRunes.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace IRunes.Services
{
    public class HashService : IHashService
    {
        private const int Iterations = 50000;

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];

            new RNGCryptoServiceProvider().GetBytes(salt);

            return salt;
        }

        public string HashPassword(string password, byte[] salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);

            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public bool IsPasswordValid(string enteredPassword, string base64Hash)
        {
            byte[] hashBytes = Convert.FromBase64String(base64Hash);
            byte[] salt = new byte[16];

            Array.Copy(hashBytes, 0, salt, 0, 16);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}

