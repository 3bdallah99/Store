using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")] //Get  : /api/Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            return NotFound(); // 404
        }

        [HttpGet("servererror")] //Get  : /api/Buggy/servererror
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception(); // 500
            return Ok();
        }

        [HttpGet("badrequest")] // Get : /api/buggy/badrequest
        public IActionResult GetBadRequest()
        {
            return BadRequest(); // 400
        }

        [HttpGet("badrequest/{id}")] // Get : /api/buggy/badrequest/4
        public IActionResult GetBadRequest(int id) // ValidationError
        {
            return BadRequest();
        }

        [HttpGet("unauthorized")] // Get : /api/buggy/unauthorized
        public IActionResult GetUnauthorizedRequest() 
        {
            return Unauthorized();
        }
        


    }
}
