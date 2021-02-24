using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public decimal Price { get; set; }
        public byte[] CoverImageContent { get; set; }

        //1-1 Book-Category
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
