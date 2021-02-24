using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RenCart.API.Dtos;
using RenCart.API.Interfaces;
using RenCart.API.Models;
using RenCart.API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        readonly IBookService bookService;
        private readonly IOptions<AppSettings> settings;
        private readonly IMapper mapper;
        private readonly AppDbContext db;

        public BooksController(IBookService bookService, IOptions<AppSettings> settings, IMapper mapper,AppDbContext db)
        {
            this.bookService = bookService;
            this.settings = settings;
            this.mapper = mapper;
            this.db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await bookService.GetAllBooks();
            return Ok(books);
        }
        //[Authorize(Policy = Policy.Admin)]
        [HttpGet("seedbooks")]
        public async Task<IActionResult> SeedBooks()
        {
            var faker = new Faker<Book>().RuleFor(x => x.Author, y => y.Person.FullName)
                .RuleFor(x => x.Title, y => y.Internet.UserName())
                .RuleFor(x => x.CategoryId, y => y.Random.Int(1, 4))
                .RuleFor(x => x.CategoryName, y => y.PickRandom(new string[] { "Romance", "Mystry", "Fiction and Fantasy", "Educational" }))
                .RuleFor(x => x.Author, y => y.Person.FullName)
                .RuleFor(x => x.CoverImage, y => y.Person.Avatar)
                .RuleFor(x => x.Price, y => y.Commerce.Price(89.99m, 899.99m, 3, "").First());
            var books = faker.Generate(20);
            await db.AddRangeAsync(books);
            await db.SaveChangesAsync();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(long id)
        {
            var book = await bookService.GetBook(id);
            return Ok(book);
        }
        [HttpGet("bookbypage/{page}")]
        public async Task<IActionResult> GetBook(int page = 1)
        {
            var books = await bookService.GetAllBooksByPage(page);
            return Ok(books);
        }
        [Authorize(Policy = Policy.Admin)]
        [HttpPost]
        public async Task<IActionResult> PostBook([FromForm] BookCreateDto bookDto)
        {
            var isSaved = await bookService.SaveBook(bookDto);
            if (isSaved)
            {
                return Ok();
            }
            return BadRequest(ModelState);
        }
        [Authorize(Policy = Policy.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, [FromForm] BookCreateDto bookDto)
        {
            var isSaved = await bookService.SaveBook(bookDto);
            if (isSaved)
            {
                return Ok(isSaved);
            }
            return BadRequest(ModelState);
        }
        [Authorize(Policy = Policy.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var isSaved = await bookService.DeleteBook(id);
            if (isSaved)
            {
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}
