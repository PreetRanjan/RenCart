using RenCart.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateUser(LoginDto loginDto);
        Task<ResultDto> RegisterUser(UserCreateDto model);
        Task<bool> CheckUserAvailability(string username);
        Task<bool> IsUserExists(string userId);
    }
}
