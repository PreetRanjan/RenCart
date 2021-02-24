using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public class WishList
    {
        public WishList()
        {
            DateCreated = DateTime.Now;
            WishListItems = new HashSet<WishListItem>();
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
