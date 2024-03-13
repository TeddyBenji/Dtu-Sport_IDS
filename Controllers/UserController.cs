using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 
using Dtu_Sport_IDS.Services;

namespace Dtu_Sport_IDS.Controllers
{
    [ApiController]
    [Route("Dtu/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            var userServiceResponse = await _userService.RegisterUserAsync(model);

            if (!userServiceResponse.Success)
            {
                return BadRequest(userServiceResponse.Message);
            }

            return Ok(userServiceResponse.Message);
        }
    
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var result = await _userService.DeleteUserAsync(username);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    



    }
}



