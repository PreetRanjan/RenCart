using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RenCart.API.Dtos;
using RenCart.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckOutController : ControllerBase
    {
        private readonly IOrderService orderService;

        public CheckOutController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string userId)
        {
            var isAdded = await orderService.CreateOrder(userId);
            return Ok(new ResultDto(isAdded, null));
        }
    }
}
