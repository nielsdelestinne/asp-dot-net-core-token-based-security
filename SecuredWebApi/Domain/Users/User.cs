using System;

namespace SecuredWebApi.Domain.Users
{
    public class User
    {

        public Guid Id { get; }
        public string Email { get; }
        public UserSecurity UserSecurity{ get; }

        public User(string email, UserSecurity userSecurity)
        {
            Id = Guid.NewGuid();
            Email = email;
            UserSecurity = userSecurity;
        }
    }
}
