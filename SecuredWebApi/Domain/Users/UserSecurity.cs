using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuredWebApi.Domain.Users
{
    public class UserSecurity
    {

        public string PasswordHashedAndSalted { get; }
        public string AppliedSalt { get; }

        public UserSecurity(string passwordHashedAndSalted, string appliedSalt)
        {
            PasswordHashedAndSalted = passwordHashedAndSalted;
            AppliedSalt = appliedSalt;
        }

    }
}
