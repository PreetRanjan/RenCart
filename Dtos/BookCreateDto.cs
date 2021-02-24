using Microsoft.AspNetCore.Http;
using RenCart.API.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    public class BookCreateDto
    {
        public long BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Author { get; set; }
        [MaxFileSize(2)]
        [OnlyImages]
        public IFormFile CoverImage { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
