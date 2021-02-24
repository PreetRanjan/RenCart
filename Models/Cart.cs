using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public class Cart
    {
        public Cart()
        {
            DateCreated = DateTime.Now;
            Id = Guid.NewGuid().ToString();
            CartItems = new HashSet<CartItem>();
        }
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        
        public ICollection<CartItem> CartItems { get; set; }
    }
}
