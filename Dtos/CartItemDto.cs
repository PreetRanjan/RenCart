using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    public class CartItemDto
    {
        public BookDto Book { get; set; }
        public int Quantity { get; set; }
    }
}
