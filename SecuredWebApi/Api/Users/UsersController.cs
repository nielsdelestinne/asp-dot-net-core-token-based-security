using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecuredWebApi.Domain.Users;

namespace SecuredWebApi.Api.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserMapper _userMapper;
        private readonly UserRepository _userRepository;

        public UsersController(UserMapper userMapper, UserRepository userRepository)
        {
            _userMapper = userMapper;
            _userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register([FromBody] CreateUserDto createUserDto)
        {
            User user =_userMapper.ToUser(createUserDto);
            _userRepository.Save(user);
            return Created("api/Users", user.Id.ToString());
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
