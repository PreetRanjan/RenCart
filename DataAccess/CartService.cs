using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RenCart.API.Dtos;
using RenCart.API.Interfaces;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenCart.API.DataAccess
{
    public class CartService : ICartService, ISaveChange
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;

        public CartService(AppDbContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<bool> AddToCart(string userId, long bookId)
        {
            try
            {
                string cartId = await GetCartId(userId);
                var extCartItem = db.CartItems.SingleOrDefault(x => x.BookId == bookId && x.CartId == cartId);
                if (extCartItem != null)
                {
                    extCartItem.Quantity += 1;
                    db.Entry(extCartItem).State = EntityState.Modified;
                }
                else
                {
                    int quantity = 1;
                    var price = db.Books.SingleOrDefault(x => x.Id == bookId).Price;
                    var cartItem = new CartItem
                    {
                        BookId = bookId,
                        CartId = cartId,
                        Quantity = quantity,
                        Price = price
                    };
                    db.CartItems.Add(cartItem);
                }
                return await SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> ClearCart(string userId)
        {
            try
            {
                var cartId = await GetCartId(userId);
                db.CartItems.RemoveRange(db.CartItems.Where(x => x.CartId == cartId));
                return await SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<bool> DeleteFromCart(string userId, long bookId)
        {
            try
            {
                string cartId = await GetCartId(userId);
                var cartItem = await db.CartItems.SingleOrDefaultAsync(x => x.BookId == bookId && x.CartId == cartId);
                if (cartItem.Quantity == 1)
                {
                    db.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= 1;
                    db.Entry(cartItem).State = EntityState.Modified;
                }
                
                return await SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetCartId(string userId)
        {
            try
            {
                var myCart = await db.Carts.SingleOrDefaultAsync(x => x.AppUserId == userId);
                if (myCart != null)
                {
                    return myCart.Id;
                }
                else
                {
                    return await CreateCart(userId);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task<string> CreateCart(string userId)
        {
            try
            {
                var newCart = new Cart
                {
                    AppUserId = userId
                };
                db.Carts.Add(newCart);
                await db.SaveChangesAsync();
                return newCart.Id;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<int> GetCartItemCount(string userId)
        {
            try
            {
                var cartId = await GetCartId(userId);
                var cartCount = await db.CartItems.CountAsync(x => x.CartId == cartId);
                return cartCount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CartItemDto>> GetItemsInCart(string cartId)
        {
            try
            {
                var cartItemsDto = new List<CartItemDto>();
                var cartItems = await db.CartItems.Where(x => x.CartId == cartId).ToListAsync();
                if (cartItems == null)
                {
                    return cartItemsDto;
                }
                foreach (var item in cartItems)
                {
                    var book = await db.Books.SingleOrDefaultAsync(x => x.Id == item.BookId);
                    var cartIt = new CartItemDto
                    {
                        Quantity = item.Quantity
                    };
                    cartIt.Book = mapper.Map<BookDto>(book);
                    cartItemsDto.Add(cartIt);
                }
                return cartItemsDto;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task MergeCart(string tempUserId, string permUserId)
        {
            try
            {
                if (tempUserId == permUserId && !string.IsNullOrEmpty(tempUserId) && !string.IsNullOrEmpty(permUserId))
                {
                    
                    var tempCartId = await GetCartId(tempUserId);
                    var permCartId = await GetCartId(permUserId);
                    var tempCartItems = await db.CartItems.Where(x => x.CartId == tempCartId).ToListAsync();
                    foreach (CartItem tempItem in tempCartItems)
                    {
                        var item = await db.CartItems.SingleOrDefaultAsync(x => x.BookId == tempItem.BookId && x.CartId == tempItem.CartId);
                        if (tempItem !=null)
                        {
                            tempItem.Quantity += item.Quantity;
                            db.Entry(tempItem).State = EntityState.Modified;
                        }
                        else
                        {
                            var newCartitem = new CartItem
                            {
                                CartId = permCartId,
                                BookId = item.BookId,
                                Quantity = item.Quantity
                            };
                            db.CartItems.Remove(item);
                            db.CartItems.Add(newCartitem);
                            await SaveChangesAsync();
                        }
                    }
                    DeleteCart(tempCartId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveFromCart(string userId, long bookId)
        {
            try
            {
                var cartId = await GetCartId(userId);
                var cartItem = await db.CartItems.SingleOrDefaultAsync(x => x.BookId == bookId && x.CartId == cartId);
                cartItem.Quantity -= 1;
                db.Entry(cartItem).State = EntityState.Modified;
                return await SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await db.SaveChangesAsync() > 0) ? true : false;
        }
        
        void DeleteCart(string cartId)
        {
            var cart = db.Carts.Find(cartId);
            db.Carts.Remove(cart);
            db.SaveChanges();
        }
    }
}
