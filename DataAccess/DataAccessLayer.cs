using Microsoft.Extensions.DependencyInjection;
using RenCart.API.DataAccess;
using RenCart.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart
{
    public static class DataAccessLayer
    {
        public static void AddDataAccessLayers(this IServiceCollection services)
        {
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICartService, CartService>();
            //services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWishListService, WishListService>();
        }
    }
}
