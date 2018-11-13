using System;
using System.Collections.Generic;

namespace SecuredWebApi.Domain.Users
{
    public class UserRepository
    {

        private IDictionary<Guid, User> dummyUserDB;

        public UserRepository()
        {
            dummyUserDB = new Dictionary<Guid, User>();
        }

        public User FindById(Guid userId)
        {
            return dummyUserDB[userId]; // (yes, this is unsafe)
        }

        public void Save(User user)
        {
            dummyUserDB[user.Id] = user;
        }


    }
}
