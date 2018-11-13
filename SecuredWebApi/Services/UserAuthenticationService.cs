using Microsoft.IdentityModel.Tokens;
using SecuredWebApi.Domain.Users;
using SecuredWebApi.Services.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecuredWebApi.Services
{
    public class UserAuthenticationService
    {

        private readonly UserRepository _userRepository;
        private readonly Hasher _hasher;
        private readonly Salter _salter;

        public UserAuthenticationService(UserRepository userRepository, Hasher hasher, Salter salter)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _salter = salter;
        }

        public JwtSecurityToken Authenticate(string providedEmail, string providedPassword)
        {
            User foundUser = _userRepository.FindByEmail(providedEmail);

            if (IsSuccessfullyAuthenticated(providedEmail, providedPassword, foundUser.UserSecurity))
            {
                return new JwtSecurityTokenHandler().CreateToken(CreateTokenDescription(foundUser)) as JwtSecurityToken;
            }
            return null;
        }

        public User GetCurrentLoggedInUser(ClaimsPrincipal principleUser)
        {
            var emailOfAuthenticatedUser = principleUser.FindFirst(ClaimTypes.Email)?.Value;
            return _userRepository.FindByEmail(emailOfAuthenticatedUser);
        }

        public UserSecurity CreateUserSecurity(string userPassword)
        {
            var saltToBeUsed = _salter.CreateRandomSalt();
            return new UserSecurity(
                _hasher.CreateHashOfPasswordAndSalt(userPassword, saltToBeUsed),
                saltToBeUsed);
        }

        private SecurityTokenDescriptor CreateTokenDescription(User foundUser)
        {
            var key = Encoding.ASCII.GetBytes("MyVerySecretKeyThatShouldNotBePlacedLikeThisHere");
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, foundUser.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }

        private bool IsSuccessfullyAuthenticated(string providedEmail, string providedPassword, UserSecurity persistedUserSecurity)
        {
            return _hasher.DoesProvidedPasswordMatchPersistedPassword(providedPassword, persistedUserSecurity);
        }
    }
}
