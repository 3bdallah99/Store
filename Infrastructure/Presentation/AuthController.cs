using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManger serviceManager) :ControllerBase
    {

        [HttpPost("login")] //Post : /api/Auth/Login
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await serviceManager.authenticationService.LoginAsync(loginDto);
            return Ok(result);
        } 
        [HttpPost("register")] //Post : /api/Auth/register
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await serviceManager.authenticationService.RegisterAsync(registerDto);
            return Ok(result);
        }
		// Api/Auth/EmailExist
		[HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return Ok(await serviceManager.authenticationService.CheckEmailExistsAsync(email));
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
                var email =  User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.authenticationService.GetUserByEmail(email);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("Address")]
        // api/Auth/Address
        public async Task<IActionResult> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.authenticationService.GetUserAddress(email);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<IActionResult> UpdateAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = serviceManager.authenticationService.UpdateUserAddress(address, email);
            return Ok(result);
        }

    }
}
