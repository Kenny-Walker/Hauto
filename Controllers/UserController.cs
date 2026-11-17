using Hauto.Implementations.Service;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hauto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _UserService;
        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto createUser)
        {
            var User = await _UserService.CreateUser(createUser); return Ok(User);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto updateUser)
        {
            var User = await _UserService.UpdateUser(updateUser); return Ok(User);
        }

        [HttpPost("AddPasscode")]
        public async Task<IActionResult> AddPasscode([FromForm] AddPasscodeDto addPasscode)
        {
            var User = await _UserService.AddPasscode(addPasscode); return Ok(User);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var User = await _UserService.GetUser(Email, Password); return Ok(User);
        }
    }
}
