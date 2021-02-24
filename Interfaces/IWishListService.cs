using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Interfaces
{
    public interface IWishListService
    {
        Task<bool> ToogleWishListItem(string userId, long bookId);
        Task<bool> ClearWishList(string userId);
        Task<string> GetWishListId(string userId);
    }
}
