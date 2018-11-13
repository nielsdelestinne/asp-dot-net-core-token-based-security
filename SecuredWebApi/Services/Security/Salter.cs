using System;
using System.Security.Cryptography;

namespace SecuredWebApi.Services.Security
{
    public class Salter
    {
        public string CreateRandomSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
