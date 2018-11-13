using SecuredWebApi.Domain.Users;
using SecuredWebApi.Services;

namespace SecuredWebApi.Api.Users
{
    public class UserMapper
    {
        private readonly UserAuthenticationService _userAuthService;

        public UserMapper(UserAuthenticationService userAtuhService)
        {
            _userAuthService = userAtuhService;
        }

        public User ToDomain(UserRequestDto userDto)
        {
            return new User(userDto.Email, _userAuthService.CreateUserSecurity(userDto.Password));
        }

        public UserReplyDto ToDto(User user)
        {
            return new UserReplyDto {
                Email = user.Email
            };
        }
    }
}
