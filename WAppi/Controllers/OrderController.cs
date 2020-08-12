using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WAppi.Models;
using WAppi.Services.Interfaces;

namespace WAppi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IP24Service _p24Service;

        public OrderController(IP24Service p24Service)
        {
            _p24Service = p24Service;
        }

        //api/callback/create
        //metoda post
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync()
        {

            //simulating order
            var order = new Order();

            await _p24Service.RegisterTransactionAsync(order);
            return Ok();
        }
    }
}
