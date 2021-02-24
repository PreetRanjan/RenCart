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
    public class WishListController : ControllerBase
    {
        private readonly IWishListService wishListService;
        private readonly IBookService bookService;
        private readonly IUserService userService;

        public WishListController(IWishListService wishListService,IBookService bookService,IUserService userService)
        {
            this.wishListService = wishListService;
            this.bookService = bookService;
            this.userService = userService;
        }
        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string userId)
        {
            bool isUserExists = await userService.IsUserExists(userId);
            IEnumerable<BookDto> bookList = new List<BookDto>();
            if (isUserExists)
            {
                var wishListId = await wishListService.GetWishListId(userId);
                bookList = await bookService.GetBooksInWishList(wishListId);
                return Ok(bookList);
            }
            else
            {
                return Ok(bookList);
            }
            
        }
        [Authorize]
        [HttpPost("togglewishlist/{userId}/{bookId}")] 
        public async Task<IActionResult> Post(string userId,long bookId)
        {
            var isAdded = await wishListService.ToogleWishListItem(userId, bookId);
            return Ok(new ResultDto(isAdded, new List<Error>() { new Error(StatusCodes.Status500InternalServerError.ToString(),"Something went wrong")}));
        }
    }
}
