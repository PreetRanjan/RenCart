using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    public class BookOrderDto
    {
        [Required]
        public long BookId { get; set; }
        [Required]
        [Range(1,10)]
        public int Quantity { get; set; }
    }
}
