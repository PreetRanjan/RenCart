using Microsoft.EntityFrameworkCore;
using RenCart.API.Interfaces;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.DataAccess
{
    public class WishListService : IWishListService,ISaveChange 
    {
        private readonly AppDbContext db;

        public WishListService(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<bool> ClearWishList(string userId)
        {
            var wishListId = await GetWishListId(userId);
            var listItems = db.WishListItems.Where(x => x.WishListId == wishListId);
            db.WishListItems.RemoveRange(listItems);
            return await SaveChangesAsync();
        }

        public async Task<string> GetWishListId(string userId)
        {
            try
            {
                var wishList = await db.WishLists.FirstOrDefaultAsync(x => x.AppUserId == userId);
                if (wishList!=null)
                {
                    return wishList.Id;
                }
                else
                {
                    return CreateWishList(userId);
                }
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

        public async Task<bool> ToogleWishListItem(string userId, long bookId)
        {
            try
            {
                if (!db.AppUsers.Any(x=>x.Id == userId) || !db.Books.Any(x=>x.Id == bookId))
                {
                    return false;
                }
                string wishListId = await GetWishListId(userId);
                var existingItem = await db.WishListItems.SingleOrDefaultAsync(x => x.BookId == bookId && x.WishListId == wishListId);
                if (existingItem != null)
                {
                    db.WishListItems.Remove(existingItem);
                    return await SaveChangesAsync();
                }
                else
                {
                    WishListItem wishListItem = new WishListItem
                    {
                        BookId = bookId,
                        WishListId = wishListId
                    };
                    db.WishListItems.Add(wishListItem);
                    return await SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        string CreateWishList(string userId)
        {
            try
            {
                var wishList = new WishList();
                wishList.AppUserId = userId;
                db.WishLists.Add(wishList);
                db.SaveChanges();
                return wishList.Id;
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
