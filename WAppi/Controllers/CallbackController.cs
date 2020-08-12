using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WAppi.Services.Interfaces;

namespace WAppi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IP24Service _p24Service;

        public CallbackController(IP24Service p24Service)
        {
            _p24Service = p24Service;
        }

        //api/callback/p24
        //metoda post
        [HttpPost("p24")]
        public async Task<IActionResult> GetTransactionStatusAsync()
        {
            var form = Request.Form;
            await _p24Service.VerifyTransactionAsync(form);
            return Ok();
        }
    }
}
