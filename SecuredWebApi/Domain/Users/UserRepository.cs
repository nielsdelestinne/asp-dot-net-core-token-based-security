using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuredWebApi.Domain.Users
{
    public class UserRepository
    {

        private IDictionary<Guid, User> dummyUserDB;

        public UserRepository()
        {
            dummyUserDB = new Dictionary<Guid, User>();
        }

        public User FindByEmail(string email)
        {
            return dummyUserDB.Values.Where(user => user.Email.Equals(email)).FirstOrDefault();            
        }

        public void Save(User user)
        {
            dummyUserDB[user.Id] = user;
        }


    }
}
