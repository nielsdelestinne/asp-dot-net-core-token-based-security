using SecuredWebApi.Domain.Users;
using SecuredWebApi.Security;

namespace SecuredWebApi.Api.Users
{
    public class UserMapper
    {
        private readonly Hasher _hasher;
        private readonly Salter _salter;

        public UserMapper(Hasher hasher, Salter salter)
        {
            _hasher = hasher;
            _salter = salter;
        }

        public User ToUser(CreateUserDto userDto)
        {
            return new User(userDto.Email, CreateUserSecurity(userDto.Password));
        }

        private UserSecurity CreateUserSecurity(string userPassword)
        {
            var saltToBeUsed = _salter.CreateRandomSalt();
            return new UserSecurity(
                _hasher.CreateHashOfPasswordAndSalt(userPassword, saltToBeUsed),
                saltToBeUsed);
        }
    }
}
