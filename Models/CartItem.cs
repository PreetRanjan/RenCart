using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public class CartItem
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }
        public long BookId { get; set; }
        public virtual Book Book { get; set; }
        public decimal Price { get; set; }

    }
}
