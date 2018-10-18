using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services.Contracts
{
    public interface IHashService
    {
        byte[] GenerateSalt();

        string HashPassword(string password, byte[] salt);

        bool IsPasswordValid(string enteredPassword, string base64Hash);
    }
}
