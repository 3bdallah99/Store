using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
