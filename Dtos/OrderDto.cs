using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    public class OrderDto
    {
        public string OrderId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
