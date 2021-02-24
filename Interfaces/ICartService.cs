using RenCart.API.Dtos;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Interfaces
{
    public interface ICartService
    {
        Task<bool> AddToCart(string userId,long bookId);
        Task<bool> RemoveFromCart(string userId,long bookId);
        Task<bool> DeleteFromCart(string userId, long bookId);
        Task<int> GetCartItemCount(string userId);
        Task<bool> ClearCart(string userId);
        Task<string> GetCartId(string userId);
        Task MergeCart(string tempUserId, string permUserId);
        Task<IEnumerable<CartItemDto>> GetItemsInCart(string cartId);
    }
}
