using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RenCart.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IBookService bookService;

        public ShoppingCartsController(ICartService cartService,IBookService bookService)
        {
            this.cartService = cartService;
            this.bookService = bookService;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var cartId = await cartService.GetCartId(userId);
            var books = await cartService.GetItemsInCart(cartId);
            return Ok(books);
        }
        [HttpGet("setshoppingcarts/{olduserid}/{newuserid}")]
        public async Task<IActionResult> SetShoppingCart(string olduserid,string newuserid)
        {
            await cartService.MergeCart(olduserid, newuserid);
            return Ok(await cartService.GetCartItemCount(newuserid));
        }
        [HttpPost("addtocart/{userid}/{bookid}")]
        public async Task<IActionResult> AddtoCart(string userid,long bookid)
        {
            var isAdded = await cartService.AddToCart(userid, bookid);
            return Ok(await cartService.GetCartItemCount(userid));
        }
        [HttpPut("remove/{userid}/{bookid}")]
        public async Task<IActionResult> RemoveOneItem(string userid,long bookid)
        {
            var isAdded = await cartService.DeleteFromCart(userid, bookid);
            return Ok(isAdded);
        }
        [HttpDelete("userid")]
        public async Task<IActionResult> DeleteAllCart(string userid)
        {
            var isAdded = await cartService.ClearCart(userid);
            return Ok(isAdded);
        }
    }
}
