using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public class AppUser
    {
        public AppUser()
        {
            Id = Guid.NewGuid().ToString();
            DateJoined = DateTime.Now;
        }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        //public string WishListId { get; set; }
        public virtual WishList WishList { get; set; }

        //public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

        public DateTime DateJoined { get; set; }
    }
}
