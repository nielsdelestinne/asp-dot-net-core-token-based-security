using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace SecuredWebApi.Security
{
    public class Hasher
    {

        public string CreateHashOfPasswordAndSalt(string userPassword, string salt)
        {
            return Convert.ToBase64String(GenerateHashOfPasswordAndSalt(userPassword, salt));
        }

        private byte[] GenerateHashOfPasswordAndSalt(string userPassword, string salt)
        {
            return KeyDerivation.Pbkdf2(
                password: userPassword,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
        }

        public bool Validate(string userPassword, string userAppliedSalt, string userHashedPasswordAndSalt)
        {
            return CreateHashOfPasswordAndSalt(userPassword, userAppliedSalt).Equals(userHashedPasswordAndSalt);
        }

    }
}
