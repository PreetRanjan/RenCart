using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private AppDbContext db;

        public CategoryController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await db.Categories.Select(c =>
            new CategoryDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name
            })
                .ToListAsync();
            return Ok(categories);
        }
        [HttpPost]
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> Post(CategoryDto dto)
        {
            var category = new Category()
            {
                Name = dto.CategoryName
            };
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return Created(HttpContext.Request.Path.Value, category);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> Put(CategoryDto dto)
        {
            var category = new Category()
            {
                Id = dto.CategoryId,
                Name = dto.CategoryName
            };
            db.Categories.Update(category);
            await db.SaveChangesAsync();
            return Ok();
        }
        [Authorize(Policy = Policy.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = db.Categories.Find(id);
            db.Categories.Remove(category);
            var result = await db.SaveChangesAsync();
            return Ok(result);
        }
    }
}
