using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dtu_Sport_IDS.Services; 

namespace Dtu_Sport_IDS.Controllers
{
    [ApiController]
    [Route("user/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IUserService _userService;

        public RoleController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.username) || string.IsNullOrWhiteSpace(model.Role))
            {
                return BadRequest("Invalid request.");
            }

            var result = await _userService.AssignRoleAsync(model);
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
