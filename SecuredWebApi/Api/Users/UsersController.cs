using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecuredWebApi.Domain.Users;
using SecuredWebApi.Services;

namespace SecuredWebApi.Api.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserMapper _userMapper;
        private readonly UserRepository _userRepository;
        private readonly UserAuthenticationService _userAuthService;

        public UsersController(UserMapper userMapper, UserRepository userRepository, UserAuthenticationService userAuthService)
        {
            _userMapper = userMapper;
            _userRepository = userRepository;
            _userAuthService = userAuthService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register([FromBody] UserRequestDto userRequestDto)
        {
            User user =_userMapper.ToDomain(userRequestDto);
            _userRepository.Save(user);
            return Ok();
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public ActionResult<string> Authenticate([FromBody] UserRequestDto userRequestDto)
        {
            var securityToken = _userAuthService.Authenticate(userRequestDto.Email, userRequestDto.Password);

            if (securityToken != null)
            {
                return Ok(securityToken.RawData);
            }

            return BadRequest("Email or Password incorrect!");
        }

        [HttpGet("current")]
        [Authorize]
        public ActionResult<UserReplyDto> GeCurrentUser()
        {
            var authenticatedUser = _userAuthService.GetCurrentLoggedInUser(User);
            if (authenticatedUser != null)
            {
                return Ok(_userMapper.ToDto(authenticatedUser));
            }
            return BadRequest("Could not find your user information... Contact us :)");
        }

        [HttpGet("admin")]
        [Authorize(Policy = "AdminOnly")]
        public string ProtectMeClaim()
        {
            return "A protected page: only an authenticated admin (email equal to admin@gmail.com) providing a valid token gets access!";
        }

    }
}
