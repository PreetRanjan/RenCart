using RenCart.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(string userId);
        Task<List<SingleOrderDto>> GetOrderList(string userId);
    }
}
