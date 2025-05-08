using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController (IServiceManger serviceManger) :ControllerBase
    {
        [HttpPost("{basketid}")]
        [Authorize]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent(string basketid)
        {
           var result =  await serviceManger.paymentService.CreateOrUpdatePaymentIntentAsync(basketid);
            return Ok(result);
        }
    }
}
