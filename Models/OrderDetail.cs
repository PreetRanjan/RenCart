using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public class OrderDetail
    {
        public OrderDetail()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
        public long BookId { get; set; }
        public virtual Book Book { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
