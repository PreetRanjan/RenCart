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
    public class OrderService : IOrderService, ISaveChange
    {
        private readonly AppDbContext db;
        private readonly ICartService cartService;

        public OrderService(AppDbContext db,ICartService cartService)
        {
            this.db = db;
            this.cartService = cartService;
        }
       
        public async Task<bool> CreateOrder(string userId)
        {
            //Generate a OrderId
            StringBuilder orderId = new StringBuilder();
            orderId.Append(GenerateRandomNumber(3));
            orderId.Append("-");
            orderId.Append(GenerateRandomNumber(6));
            //Generate a Txn Id
            StringBuilder txnId = new StringBuilder();
            orderId.Append(GenerateRandomNumber(3));
            orderId.Append("-");
            orderId.Append(GenerateRandomNumber(3));

            //Pull BookIds on the cart and their quantity from CartItems
            var cartid = await cartService.GetCartId(userId);
            var itemsInCart = db.CartItems.Where(c => c.CartId == cartid)
                .Select(x => new { BookId = x.BookId, Quantity = x.Quantity,Price=x.Price });
            //Calculate order total
            decimal orderTotal = itemsInCart.Sum(x => x.Price);
            //Make a order object with userid ,orderid,txnid
            var order = new Order
            {
                Id = orderId.ToString(),
                AppUserId = userId,
                TransactionId = txnId.ToString(),
                OrderTotal = orderTotal
            };
            await SaveChangesAsync();
            //Make OrderDetail object for each item in the cart and add it to database
            //with the orderId generated
            foreach (var item in itemsInCart)
            {
                var orderDetail = new OrderDetail
                {
                    BookId = item.BookId,
                    OrderId = order.Id,
                    Price = order.OrderTotal,
                    Quantity = item.Quantity
                };
                db.OrdersDetails.Add(orderDetail);
            }
            return await SaveChangesAsync();
        }

        public async Task<List<SingleOrderDto>> GetOrderList(string userId)
        {
            var orderList = new List<SingleOrderDto>();
            var orders = db.Orders.Where(x => x.AppUserId == userId)
                .Select(x => new SingleOrderDto
                {
                    OrderDate = x.OrderDate,
                    OrderId = x.Id,
                    OrderTotal = x.OrderTotal
                }).ToList();
            orders.ForEach(item => orderList.Add(item));
            return await Task.FromResult(orderList);
        }

        public Task<bool> SaveChangesAsync()
        {
            return Task.FromResult(db.SaveChanges() > 0 ? true : false);
        }
        int GenerateRandomNumber(int length)
        {
            Random random = new Random();
            return random.Next(Convert.ToInt32(Math.Pow(10, length - 1)),Convert.ToInt32(Math.Pow(10,length)));
        }
    }
}
